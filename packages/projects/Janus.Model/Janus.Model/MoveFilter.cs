// -----------------------------------------------------------------------
// <copyright file="MoveFilter.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Model
{
    /// <summary>
    /// An object storing a move in either SAN or UCI notation
    /// used to restrict the engine's search in the start position.
    /// </summary>
    public class MoveFilter
    {
        /// <summary>
        /// Gets or sets the move in <a href="https://en.wikipedia.org/wiki/Algebraic_notation_(chess)">SAN notation</a>.
        /// </summary>
        public string SAN { get; set; }

        /// <summary>
        /// Gets or sets the move in <a href="https://en.wikipedia.org/wiki/Universal_Chess_Interface">UCI move notation</a>.
        /// </summary>
        public string UCI { get; set; }
    }
}