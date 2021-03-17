// -----------------------------------------------------------------------
// <copyright file="DockerContainerManager.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Capacity.Containers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Amazon.EC2;
    using Amazon.EC2.Model;
    using Amazon.ECR;
    using Amazon.SimpleSystemsManagement;
    using Amazon.SimpleSystemsManagement.Model;
    using Docker.DotNet;
    using Docker.DotNet.Models;
    using Janus.Config;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A DockerContainerManager class.
    /// </summary>
    public class DockerContainerManager : IContainerManager
    {
        private const ushort ContainerPort = 5000;
        private const string GpuRuntime = "nvidia";
        private const string TablebaseLocation = "/tablebase";

        private readonly CloudConfig config;
        private readonly IAmazonEC2 ec2;
        private readonly IAmazonECR ecr;
        private readonly IAmazonSimpleSystemsManagement ssm;
        private readonly ILogger<DockerContainerManager> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerContainerManager"/> class.
        /// </summary>
        /// <param name="config">The Cloud Config.</param>
        /// <param name="ec2">The EC2 client.</param>
        /// <param name="ecr">The ECR client.</param>
        /// <param name="ssm">The SSM client.</param>
        /// <param name="logger">The logger.</param>
        public DockerContainerManager(
            CloudConfig config,
            IAmazonEC2 ec2,
            IAmazonECR ecr,
            IAmazonSimpleSystemsManagement ssm,
            ILogger<DockerContainerManager> logger)
        {
            this.config = config;
            this.ec2 = ec2;
            this.ecr = ecr;
            this.ssm = ssm;
            this.logger = logger;
        }

        /// <inheritdoc cref="IContainerManager" />
        public async Task<string> LaunchEngineContainerAsync(ContainerScale scale, string instanceId)
        {
            var hostIp = await this.GetIpFromInstanceAsync(instanceId);
            var docker = this.CreateClientFromHost(hostIp);
            string imageId = await this.GetImageIdAsync(docker, instanceId);
            int availablePort = await this.SelectAvailablePortAsync(docker);

            var createParams = this.CreateDefaultContainerParameters(scale, imageId, availablePort);
            if (scale.Type == ContainerType.GPU)
            {
                createParams.Env = new List<string>()
                {
                    "NVIDIA_VISIBLE_DEVICES=all",
                };
                createParams.HostConfig.Runtime = GpuRuntime;
            }

            var createdContainer = await docker.Containers.CreateContainerAsync(createParams);
            await docker.Containers.StartContainerAsync(createdContainer.ID, new ContainerStartParameters());
            return createdContainer.ID;
        }

        /// <summary>
        /// Lists containers running on the instance.
        /// </summary>
        /// <param name="instanceId">The instance id.</param>
        /// <returns>A list of containers.</returns>
        public async Task<List<LaunchedContainer>> ListContainersAsync(string instanceId)
        {
            var ip = await this.GetIpFromInstanceAsync(instanceId);
            var docker = this.CreateClientFromHost(ip);
            var containers = await docker.Containers.ListContainersAsync(new ContainersListParameters()
            {
                All = false,
            });
            return containers.Select(c => new LaunchedContainer()
            {
                ContainerId = c.ID,
                Port = c.Ports.First(p => p.PrivatePort == 5000).PublicPort,
            }).ToList();
        }

        private static string GetImagePullScript()
        {
            var myType = typeof(DockerContainerManager);
            string myNamespace = myType.Namespace;
            string scriptPath = $"{myNamespace}.pull_image.sh";
            using var stream = typeof(DockerContainerManager).Assembly.GetManifestResourceStream(scriptPath);
            using var reader = new StreamReader(stream!);
            return reader.ReadToEnd();
        }

        private async Task<string> GetImageIdAsync(DockerClient client, string instanceId)
        {
            var images = await client.Images.ListImagesAsync(new ImagesListParameters());
            var makiImage = images.FirstOrDefault();
            if (makiImage == null)
            {
                await this.PullLatestImageAsync(instanceId);
                images = await client.Images.ListImagesAsync(new ImagesListParameters());
                makiImage = images.First();
            }

            return makiImage.ID;
        }

        private async Task PullLatestImageAsync(string instanceId)
        {
            this.logger.LogInformation("Image ID not found on instance {InstanceId}, pulling image", instanceId);
            string script = GetImagePullScript();
            var runReq = new SendCommandRequest()
            {
                InstanceIds = new List<string>()
                {
                    instanceId,
                },
                DocumentName = "AWS-RunShellScript",
                DocumentVersion = "1",
                Parameters = new Dictionary<string, List<string>>()
                {
                    { "commands", script.Split('\n').ToList() },
                },
            };
            var sendResponse = await this.ssm.SendCommandAsync(runReq);
            this.logger.LogInformation("Started image pull operation...");

            GetCommandInvocationResponse getRes;
            while (true)
            {
                var getReq = new GetCommandInvocationRequest()
                {
                    InstanceId = instanceId,
                    CommandId = sendResponse.Command.CommandId,
                };
                getRes = await this.ssm.GetCommandInvocationAsync(getReq);
                this.logger.LogInformation("Current status is {Status}", getRes.Status.Value);
                if (getRes.Status != CommandInvocationStatus.Pending &&
                    getRes.Status != CommandInvocationStatus.InProgress)
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            var status = getRes.Status;
            if (status == CommandStatus.Cancelled || status == CommandStatus.Cancelling)
            {
                var opCancelledEx = new OperationCanceledException("Imager pull command invocation was cancelled.");
                this.logger.LogError("Command invocation was cancelled", opCancelledEx);
                throw opCancelledEx;
            }
            else if (status == CommandInvocationStatus.Failed)
            {
                string errorMessage = getRes.StandardErrorContent;
                string cause = getRes.StatusDetails;
                string responseCode = getRes.ResponseCode.ToString();
                this.logger.LogError(
                    "Command invocation failed with status code {ResponseCode}: {Cause}\n{ErrorMessage}",
                    responseCode,
                    cause,
                    errorMessage);
                throw new OperationCanceledException("Imager pull command invocation was cancelled.");
            }
        }

        private async Task<int> SelectAvailablePortAsync(DockerClient client)
        {
            var containers = await client.Containers.ListContainersAsync(new ContainersListParameters()
            {
                All = false,
            });
            return 8080 + containers.Count;
        }

        private DockerClient CreateClientFromHost(string host) => new DockerClientConfiguration(new Uri($"http://{host}:2345")).CreateClient();

        private CreateContainerParameters CreateDefaultContainerParameters(ContainerScale scale, string imageId, int port) => new CreateContainerParameters()
        {
            Image = imageId,
            ExposedPorts = new Dictionary<string, EmptyStruct>()
            {
                { $"{ContainerPort}/tcp", default(EmptyStruct) },
            },
            Env = new List<string>()
            {
                $"ENGINE_NAME={scale.Type.ToEngineName()}",
            },
            HostConfig = new HostConfig()
            {
                CPUCount = scale.Cores,
                Memory = (1024 * 1024) * scale.MemoryMegabytes,
                Runtime = GpuRuntime,
                PortBindings = new Dictionary<string, IList<PortBinding>>()
                {
                    {
                        $"{ContainerPort}/tcp", new List<PortBinding>()
                        {
                            new PortBinding()
                            {
                                HostPort = port.ToString(),
                            },
                        }
                    },
                },
                Binds = new List<string>()
                {
                    $"{TablebaseLocation}:{TablebaseLocation}:ro",
                },
            },
        };

        private async Task<string> GetIpFromInstanceAsync(string instanceId)
        {
            var instanceReq = new DescribeInstancesRequest()
            {
                InstanceIds = new List<string>()
                {
                    instanceId,
                },
            };
            var instanceRes = await this.ec2.DescribeInstancesAsync(instanceReq);
            var reservation = instanceRes.Reservations.FirstOrDefault();
            if (reservation == null)
            {
                throw new ArgumentException($"No instance found with id '{instanceId}'.");
            }

            var instance = reservation.Instances.FirstOrDefault();
            if (instance == null)
            {
                throw new ArgumentException($"No instance found with id '{instanceId}'.");
            }

            if (this.config.IsInCloud)
            {
                var iface = instance.NetworkInterfaces.First();
                var ipv6 = iface.Ipv6Addresses.FirstOrDefault()?.Ipv6Address;
                return ipv6 ?? instance.PrivateIpAddress;
            }
            else
            {
                return instance.PublicIpAddress;
            }
        }
    }
}