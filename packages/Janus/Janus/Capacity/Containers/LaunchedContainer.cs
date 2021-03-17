// -----------------------------------------------------------------------
// <copyright file="LaunchedContainer.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Capacity.Containers
{
    /// <summary>
    /// A LaunchedContainer class.
    /// </summary>
    public class LaunchedContainer
    {
        /// <summary>
        /// Gets or sets the container id.
        /// </summary>
        public string ContainerId { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the container is available to run engine analysis.
        /// </summary>
        public bool Free { get; set; }
    }
}