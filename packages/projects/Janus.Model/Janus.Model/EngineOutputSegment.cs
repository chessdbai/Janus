// -----------------------------------------------------------------------
// <copyright file="EngineOutputSegment.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// A parsed, single piece of output from the engine.
    /// </summary>
    public class EngineOutputSegment
    {
        /// <summary>
        /// Gets or sets the raw output string.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets the raw error string.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the parse evaluated line.
        /// </summary>
        public EvaluatedLine Line { get; set; }

        /// <summary>
        /// Gets or sets the best lines.
        /// </summary>
        public Dictionary<int, EvaluatedLine> BestLines { get; set; }

        /// <summary>
        /// Gets or sets the result. Only used when engine reports that it found
        /// the best move in a given situation (noted by the 'bestmove' UCI keyword).
        /// </summary>
        public EvaluatePositionResult Result { get; set; }
    }
}