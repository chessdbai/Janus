// -----------------------------------------------------------------------
// <copyright file="ConfiguredAvailabilityZone.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Config
{
    /// <summary>
    /// A ConfiguredAvailabilityZone class.
    /// </summary>
    public class ConfiguredAvailabilityZone
    {
        /// <summary>
        /// Gets or sets the AWS availability zone id.
        /// </summary>
        public string Az { get; set; }

        /// <summary>
        /// Gets or sets the subnet id.
        /// </summary>
        public string SubnetId { get; set; }
    }
}