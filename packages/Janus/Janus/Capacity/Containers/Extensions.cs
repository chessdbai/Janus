// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Capacity.Containers
{
    /// <summary>
    /// A Extensions class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the engine name from the container type.
        /// </summary>
        /// <param name="ct">The container type.</param>
        /// <returns>The engine name.</returns>
        public static string ToEngineName(this ContainerType ct) =>
            ct == ContainerType.CPU
                ? "stockfish"
                : "leela";
    }
}