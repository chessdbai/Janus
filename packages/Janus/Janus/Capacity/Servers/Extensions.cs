// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Capacity.Servers
{
    using System.Linq;
    using Amazon.EC2.Model;
    using Janus.Config;

    /// <summary>
    /// A Extensions class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Convert an instance to a <see cref="LaunchedCapacity"/> object.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="config">The app config.</param>
        /// <returns>The <see cref="LaunchedCapacity"/> object.</returns>
        public static LaunchedCapacity ToLaunchedCapacity(this Instance instance, JanusConfig config) =>
            new LaunchedCapacity()
            {
                InstanceId = instance.InstanceId,
                AvailabilityZone = config.ServerOptions.AvailabilityZones.First(az => az.SubnetId == instance.SubnetId)
                    .Az,
                InstanceType = instance.InstanceType,
                LaunchTime = instance.LaunchTime,
            };
    }
}