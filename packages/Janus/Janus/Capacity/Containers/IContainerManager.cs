// -----------------------------------------------------------------------
// <copyright file="IContainerManager.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Capacity.Containers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Something capable of launching containers.
    /// </summary>
    public interface IContainerManager
    {
        /// <summary>
        /// Launch an engine container.
        /// </summary>
        /// <param name="scale">The scale required for the container.</param>
        /// <param name="instanceId">The IP address.</param>
        /// <returns>The container id.</returns>
        Task<string> LaunchEngineContainerAsync(ContainerScale scale, string instanceId);

        /// <summary>
        /// Lists the running containers on an instance.
        /// </summary>
        /// <param name="instanceId">The instance id.</param>
        /// <returns>A list of running containers.</returns>
        Task<List<LaunchedContainer>> ListContainersAsync(string instanceId);
    }
}