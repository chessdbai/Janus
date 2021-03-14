// -----------------------------------------------------------------------
// <copyright file="ContainerType.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Capacity
{
    /// <summary>
    /// The type of container.
    /// </summary>
    public enum ContainerType
    {
        /// <summary>
        /// A container not requiring GPU devices.
        /// </summary>
        CPU,

        /// <summary>
        /// A container supporting GPU devices.
        /// </summary>
        GPU,
    }
}