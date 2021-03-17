// -----------------------------------------------------------------------
// <copyright file="CloudConfig.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Config
{
    using Amazon.Extensions.NETCore.Setup;

    /// <summary>
    /// A CloudConfig class.
    /// </summary>
    public record CloudConfig
    {
        /// <summary>
        /// Gets the application stage.
        /// </summary>
        public string Stage { get; init; }

        /// <summary>
        /// Gets a value indicating whether or not we're currently running in AWS.
        /// </summary>
        public bool IsInCloud { get; init; }

        /// <summary>
        /// Gets the options to use when creating AWS clients.
        /// </summary>
        public AWSOptions Options { get; init; }
    }
}