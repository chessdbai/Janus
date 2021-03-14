// -----------------------------------------------------------------------
// <copyright file="IServerLauncher.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Capacity
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Required methods to satisfy being a ServerLauncher.
    /// </summary>
    public interface IServerLauncher
    {
        /// <summary>
        /// Launch one or more servers with the specified properties.
        /// </summary>
        /// <param name="type">The type of server.</param>
        /// <param name="size">The size of server.</param>
        /// <param name="quantity">The quantity.</param>
        /// <returns>The list of launched servers.</returns>
        Task<List<LaunchedCapacity>> LaunchServersAsync(ServerType type, ServerSize size, int quantity);
    }
}