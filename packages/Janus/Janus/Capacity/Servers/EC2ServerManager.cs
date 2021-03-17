// -----------------------------------------------------------------------
// <copyright file="EC2ServerManager.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Capacity.Servers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Amazon.EC2;
    using Amazon.EC2.Model;
    using Janus.Config;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A Ec2ServerManager class.
    /// </summary>
    public class EC2ServerManager : IServerManager
    {
        private readonly IConfigRetriever configRetriever;
        private readonly IAmazonEC2 ec2;
        private readonly ILogger<EC2ServerManager> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EC2ServerManager"/> class.
        /// </summary>
        /// <param name="configRetriever">The config retriever.</param>
        /// <param name="ec2">The EC2 API client.</param>
        /// <param name="logger">The logger.</param>
        public EC2ServerManager(
            IConfigRetriever configRetriever,
            IAmazonEC2 ec2,
            ILogger<EC2ServerManager> logger)
        {
            this.configRetriever = configRetriever;
            this.ec2 = ec2;
            this.logger = logger;
        }

        /// <summary>
        /// Determines the EC2 instance type to launch based on the requested server type and size.
        /// </summary>
        /// <param name="type">The server type.</param>
        /// <param name="size">The server size.</param>
        /// <returns>The EC2 instance type.</returns>
        public static InstanceType InstanceTypeFromServerTypeAndSize(ServerType type, ServerSize size)
        {
            if (type == ServerType.CPU)
            {
                // TODO: Compare c5-series with:
                //       * c6g (ARM)
                //       * z1d (nice clock speed = 4Ghz)
                //       * m5zn (turbo clock speed up to 4.5Ghz)
                //       *
                return InstanceType.M5znLarge;
            }
            else
            {
                return InstanceType.P38xlarge;
            }
        }

        /// <inheritdoc cref="IServerManager" />
        public async Task<List<LaunchedCapacity>> LaunchServersAsync(ServerType type, ServerSize size, int quantity)
        {
            var random = new Random();
            var config = (await this.configRetriever.RetrieveConfigAsync()).ServerOptions;
            var az = config.AvailabilityZones[random.Next(0, config.AvailabilityZones.Count)];
            var request = new RunInstancesRequest()
            {
                MinCount = 1,
                MaxCount = 1,
                Placement = new Placement()
                {
                    AvailabilityZone = az.Az,
                },
                SubnetId = az.SubnetId,
                ImageId = config.Ami,
                Monitoring = true,
                IamInstanceProfile = new IamInstanceProfileSpecification()
                {
                    Arn = config.InstanceProfile,
                },
                InstanceType = InstanceTypeFromServerTypeAndSize(type, size),
                SecurityGroupIds = new List<string>(new[] { "sg-05aad9a19aa99f7fd", }),
                BlockDeviceMappings = new List<BlockDeviceMapping>()
                {
                    new BlockDeviceMapping()
                    {
                        DeviceName = "/dev/sdf",
                        Ebs = new EbsBlockDevice()
                        {
                            SnapshotId = config.TablebaseSnapshotId,
                            VolumeType = VolumeType.Gp2,
                        },
                    },
                },
                TagSpecifications = new List<TagSpecification>()
                {
                    new TagSpecification()
                    {
                        ResourceType = ResourceType.Instance,
                        Tags = new List<Tag>()
                        {
                            new Tag()
                            {
                                Key = "LaunchedBy",
                                Value = "Janus",
                            },
                        },
                    },
                },
            };
            var response = await this.ec2.RunInstancesAsync(request);
            return response.Reservation.Instances.Select(i => new LaunchedCapacity()
            {
                InstanceId = i.InstanceId,
                AvailabilityZone = az.Az,
                LaunchTime = i.LaunchTime,
                InstanceType = i.InstanceType.Value,
            }).ToList();
        }

        /// <inheritdoc cref="IServerManager"/>
        public async Task<List<LaunchedCapacity>> ListServersAsync(ServerType? type)
        {
            var config = await this.configRetriever.RetrieveConfigAsync();
            var req = new DescribeInstancesRequest()
            {
                Filters = new List<Filter>()
                {
                    new Filter()
                    {
                        Name = "tag:LaunchedBy",
                        Values = new List<string>()
                        {
                            "Janus",
                        },
                    },
                },
            };
            if (type != null)
            {
                var instanceTypes = type == ServerType.CPU
                    ? config.ServerOptions.CpuServerTypes
                    : config.ServerOptions.GpuServerTypes;
                req.Filters.Add(new Filter()
                {
                    Name = "instance-type",
                    Values = instanceTypes,
                });
            }

            var res = await this.ec2.DescribeInstancesAsync(req);
            return res.Reservations.SelectMany(r => r.Instances)
                .Select(i => i.ToLaunchedCapacity(config))
                .ToList();
        }
    }
}