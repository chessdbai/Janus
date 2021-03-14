// -----------------------------------------------------------------------
// <copyright file="CompletionCriteria.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Model
{
    using System;

    /// <summary>
    /// A class with data that tells the engine when it has reached
    /// the end of the requested evaluation session.
    /// </summary>
    public class CompletionCriteria
    {
        /// <summary>
        /// Gets or sets the max time to calculate for.
        /// </summary>
        public TimeSpan? PonderTime { get; set; }

        /// <summary>
        /// Gets or sets the max depth.
        /// </summary>
        public ushort? MaxDepth { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of moves to look for mate.
        /// </summary>
        public ushort? Mate { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of nodes to search.
        /// </summary>
        public ulong? MaxNodes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the analysis should be infinite.
        /// </summary>
        public bool Infinite { get; set; }
    }
}