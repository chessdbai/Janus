// -----------------------------------------------------------------------
// <copyright file="ContainerScale.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable SA1623
namespace Janus.Capacity
{
    /// <summary>
    /// A ContainerScale class.
    /// </summary>
    public record ContainerScale
    {
        /// <summary>
        /// Gets or sets the number of CPU cores.
        /// </summary>
        public int Cores { get; init; }

        /// <summary>
        /// Gets or sets the memory in megabytes.
        /// </summary>
        public int MemoryMegabytes { get; init; }

        /// <summary>
        /// Gets or sets the number of GPUs to allocate.
        /// </summary>
        public int GPUs { get; init; }

        /// <summary>
        /// Gets or sets the type of container.
        /// </summary>
        public ContainerType Type { get; init; }
    }
}
#pragma warning restore SA1623