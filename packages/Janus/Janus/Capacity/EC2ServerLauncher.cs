// -----------------------------------------------------------------------
// <copyright file="EC2ServerLauncher.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Capacity
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Amazon.EC2;
    using Amazon.EC2.Model;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A EC2ServerLauncher class.
    /// </summary>
    public class EC2ServerLauncher : IServerLauncher
    {
        private readonly IAmazonEC2 ec2;
        private readonly ILogger<EC2ServerLauncher> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EC2ServerLauncher"/> class.
        /// </summary>
        /// <param name="ec2">The EC2 API client.</param>
        /// <param name="logger">The logger.</param>
        public EC2ServerLauncher(IAmazonEC2 ec2, ILogger<EC2ServerLauncher> logger)
        {
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

        /// <inheritdoc cref="IServerLauncher" />
        public async Task<List<LaunchedCapacity>> LaunchServersAsync(ServerType type, ServerSize size, int quantity)
        {
            var request = new RunInstancesRequest()
            {
                MinCount = 1,
                MaxCount = 1,
                Placement = new Placement()
                {
                    AvailabilityZone = "us-east-2b",
                },
                SubnetId = "subnet-09d03c648f8165683",
                ImageId = "ami-0bbee170ac3055ded",
                Monitoring = true,
                IamInstanceProfile = new IamInstanceProfileSpecification()
                {
                    Arn = "arn:aws:iam::541249553451:instance-profile/ManagedInstance",
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
                            SnapshotId = "snap-01965aa39dc821be8",
                            VolumeType = VolumeType.Gp2,
                        },
                    },
                },
            };
            var response = await this.ec2.RunInstancesAsync(request);
            return response.Reservation.Instances.Select(i => new LaunchedCapacity()
            {
                InstanceId = i.InstanceId,
            }).ToList();
        }
    }
}