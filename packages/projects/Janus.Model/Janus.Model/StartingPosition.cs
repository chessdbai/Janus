// -----------------------------------------------------------------------
// <copyright file="StartingPosition.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Model
{
    /// <summary>
    /// A StartingPosition class.
    /// </summary>
    public class StartingPosition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartingPosition"/> class.
        /// </summary>
        public StartingPosition()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartingPosition"/> class.
        /// </summary>
        /// <param name="pgn">The starting PGN.</param>
        /// <param name="fen">The starting FEN.</param>
        public StartingPosition(string pgn = null, string fen = null)
        {
            this.Pgn = pgn;
            this.Fen = fen;
        }

        /// <summary>
        /// Gets or sets the starting Pgn.
        /// </summary>
        public string Pgn { get; set; }

        /// <summary>
        /// Gets or sets the starting FEN.
        /// </summary>
        public string Fen { get; set; }
    }
}