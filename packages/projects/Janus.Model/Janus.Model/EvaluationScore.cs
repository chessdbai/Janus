// -----------------------------------------------------------------------
// <copyright file="EvaluationScore.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// A class collecting the different types of evaluations returnable by the engine.
    /// </summary>
    public class EvaluationScore
    {
        /// <summary>
        /// Gets or sets the number of moves until mate.
        /// </summary>
        public int? MovesUntilMate { get; set; }

        /// <summary>
        /// Gets or sets the centipawn eval of the position.
        /// </summary>
        public int? Centipawns { get; set; }

        /// <summary>
        /// Gets the centipawn eval as a decimal.
        /// </summary>
        [JsonIgnore]
        public decimal? DecimalScore
        {
            get
            {
                if (this.Centipawns == null)
                {
                    return null;
                }

                return (decimal)this.Centipawns.Value / 100m;
            }
        }

        /// <summary>
        /// Returns a string representation of this evaluation.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            if (this.MovesUntilMate != null)
            {
                return $"#{this.MovesUntilMate.Value}";
            }
            else if (this.Centipawns != null)
            {
                return this.DecimalScore!.Value!.ToString("F");
            }
            else
            {
                return null;
            }
        }
    }
}