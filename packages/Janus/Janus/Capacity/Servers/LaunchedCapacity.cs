// -----------------------------------------------------------------------
// <copyright file="LaunchedCapacity.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Capacity.Servers
{
    using System;

    /// <summary>
    /// A LaunchedCapacity class.
    /// </summary>
    public record LaunchedCapacity
    {
        /// <summary>
        /// Gets or sets the instance id.
        /// </summary>
        public string InstanceId { get; set; }

        /// <summary>
        /// Gets or sets the instance type.
        /// </summary>
        public string InstanceType { get; set; }

        /// <summary>
        /// Gets or sets the launch time.
        /// </summary>
        public DateTime LaunchTime { get; set; }
    }
}