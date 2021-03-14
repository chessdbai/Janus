// -----------------------------------------------------------------------
// <copyright file="ServerType.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Capacity
{
    /// <summary>
    /// Different types of capacity we should be able to launch.
    /// </summary>
    public enum ServerType
    {
        /// <summary>
        /// A server capable of handling CPU-intensive analysis tasks.
        /// </summary>
        CPU,

        /// <summary>
        /// A server capable of handling GPU-intensive analysis tasks.
        /// </summary>
        GPU,
    }
}