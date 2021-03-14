// -----------------------------------------------------------------------
// <copyright file="DockerContainerLauncher.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Capacity
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.ECR;
    using Amazon.ECR.Model;
    using Docker.DotNet;
    using Docker.DotNet.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A DockerContainerLauncher class.
    /// </summary>
    public class DockerContainerLauncher : IContainerLauncher
    {
        private const string ImageId = "939bc060017a";
        private const ushort ContainerPort = 5000;
        private const string GpuRuntime = "nvidia";
        private const string TablebaseLocation = "/tablebase";

        private readonly IAmazonECR ecr;
        private readonly ILogger<DockerContainerLauncher> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerContainerLauncher"/> class.
        /// </summary>
        /// <param name="ecr">The ECR client.</param>
        /// <param name="logger">The logger.</param>
        public DockerContainerLauncher(
            IAmazonECR ecr,
            ILogger<DockerContainerLauncher> logger)
        {
            this.ecr = ecr;
            this.logger = logger;
        }

        /// <inheritdoc cref="IContainerLauncher" />
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
    }
}