// -----------------------------------------------------------------------
// <copyright file="EvaluatePositionResult.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Model
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class storing the final evaluation and series of
    /// moves according to the engine.
    /// </summary>
    public class EvaluatePositionResult
    {
        /// <summary>
        /// Gets or sets the starting position.
        /// </summary>
        public StartingPosition StartingPosition { get; set; }

        /// <summary>
        /// Gets the max depth.
        /// </summary>
        public int MaxDepth => (
                from l
                    in this.Lines
                orderby l.Depth descending
                select l)
            .First().Depth;

        /// <summary>
        /// Gets or sets all the lines, if multiple lines were requested with
        /// the UCI option MultiPV.
        /// </summary>
        public List<EvaluatedLine> Lines { get; set; }
    }
}