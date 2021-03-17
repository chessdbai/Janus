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
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.EC2;
    using Amazon.EC2.Model;
    using Amazon.ECR;
    using Amazon.ECR.Model;
    using Docker.DotNet;
    using Docker.DotNet.Models;
    using Janus.Config;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A DockerContainerManager class.
    /// </summary>
    public class DockerContainerManager : IContainerManager
    {
        private const string ImageId = "939bc060017a";
        private const ushort ContainerPort = 5000;
        private const string GpuRuntime = "nvidia";
        private const string TablebaseLocation = "/tablebase";

        private readonly CloudConfig config;
        private readonly IAmazonEC2 ec2;
        private readonly IAmazonECR ecr;
        private readonly ILogger<DockerContainerManager> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerContainerManager"/> class.
        /// </summary>
        /// <param name="config">The Cloud Config.</param>
        /// <param name="ec2">The EC2 client.</param>
        /// <param name="ecr">The ECR client.</param>
        /// <param name="logger">The logger.</param>
        public DockerContainerManager(
            CloudConfig config,
            IAmazonEC2 ec2,
            IAmazonECR ecr,
            ILogger<DockerContainerManager> logger)
        {
            this.config = config;
            this.ec2 = ec2;
            this.ecr = ecr;
            this.logger = logger;
        }

        /// <inheritdoc cref="IContainerManager" />
        public async Task<string> LaunchEngineContainerAsync(ContainerScale scale, string destinationHostIp)
        {
            var docker = this.CreateClientFromHost(destinationHostIp);

            var createParams = this.CreateDefaultContainerParameters(scale);
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

        private DockerClient CreateClientFromHost(string host) => new DockerClientConfiguration(new Uri($"http://{host}:2345")).CreateClient();

        private CreateContainerParameters CreateDefaultContainerParameters(ContainerScale scale) => new CreateContainerParameters()
        {
            Image = ImageId,
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
                                HostPort = "8080",
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