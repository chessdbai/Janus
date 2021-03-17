// -----------------------------------------------------------------------
// <copyright file="IConfigRetriever.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Config
{
    using System.Threading.Tasks;

    /// <summary>
    /// An interface with methods to retrieve the service configuration.
    /// </summary>
    public interface IConfigRetriever
    {
        /// <summary>
        /// Retrieves the service configuration.
        /// </summary>
        /// <returns>The service configuration.</returns>
        Task<JanusConfig> RetrieveConfigAsync();
    }
}