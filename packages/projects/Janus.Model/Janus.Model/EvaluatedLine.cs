// -----------------------------------------------------------------------
// <copyright file="EvaluatedLine.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Model
{
    /// <summary>
    /// A series of chess moves parsed by the engine and including an evaluation.
    /// </summary>
    public class EvaluatedLine
    {
        /// <summary>
        /// Gets or sets the SAN line text.
        /// </summary>
        public string LineSan { get; set; }

        /// <summary>
        /// Gets or sets the UCI line text.
        /// </summary>
        public string LineUci { get; set; }

        /// <summary>
        /// Gets or sets the rank of this line compared with other lines,
        /// if multiple lines were requested.
        /// </summary>
        public int LineRank { get; set; }

        /// <summary>
        /// Gets or sets the depth of this line.
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// Gets or sets the evaluation of this line.
        /// </summary>
        public EvaluationScore Evaluation { get; set; }

        /// <summary>
        /// Converts this line into a string representation.
        /// </summary>
        /// <returns>The string representation of this line.</returns>
        public override string ToString()
        {
            return $"{this.LineSan} {this.Evaluation} (depth {this.Depth})";
        }
    }
}