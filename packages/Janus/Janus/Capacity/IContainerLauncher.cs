// -----------------------------------------------------------------------
// <copyright file="IContainerLauncher.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Capacity
{
    using System.Threading.Tasks;

    /// <summary>
    /// Something capable of launching containers.
    /// </summary>
    public interface IContainerLauncher
    {
        /// <summary>
        /// Launch an engine container.
        /// </summary>
        /// <param name="scale">The scale required for the container.</param>
        /// <param name="destinationHostIp">The IP address.</param>
        /// <returns>The container id.</returns>
        Task<string> LaunchEngineContainerAsync(ContainerScale scale, string destinationHostIp);
    }
}