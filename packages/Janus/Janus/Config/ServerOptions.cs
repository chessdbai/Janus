// -----------------------------------------------------------------------
// <copyright file="ServerOptions.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Config
{
    using System.Collections.Generic;

    /// <summary>
    /// A ServerOptions class.
    /// </summary>
    public class ServerOptions
    {
        /// <summary>
        /// Gets or sets the Amazon Machine Image id.
        /// </summary>
        public string Ami { get; set; }

        /// <summary>
        /// Gets or sets the security group id.
        /// </summary>
        public string SecurityGroupId { get; set; }

        /// <summary>
        /// Gets or sets the Instance Profile ARN.
        /// </summary>
        public string InstanceProfile { get; set; }

        /// <summary>
        /// Gets or sets the tablebase snapshot id.
        /// </summary>
        public string TablebaseSnapshotId { get; set; }

        /// <summary>
        /// Gets or sets the instance types to use when
        /// launching a GPU-heavy server.
        /// </summary>
        public List<string> GpuServerTypes { get; set; }

        /// <summary>
        /// Gets or sets the instance types to use when
        /// launching a compute-heavy server.
        /// </summary>
        public List<string> CpuServerTypes { get; set; }

        /// <summary>
        /// Gets or sets the availability zones to launch instances in.
        /// </summary>
        public List<ConfiguredAvailabilityZone> AvailabilityZones { get; set; }
    }
}