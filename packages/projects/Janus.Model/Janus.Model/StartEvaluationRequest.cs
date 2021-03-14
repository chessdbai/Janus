// -----------------------------------------------------------------------
// <copyright file="StartEvaluationRequest.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Janus.Model.Validation;

    /// <summary>
    /// An API request to start the evaluation from the current position.
    /// </summary>
    public class StartEvaluationRequest
    {
        /// <summary>
        /// Gets or sets the position to start evaluating from.
        /// </summary>
        [Required]
        [ValidatedStartingPosition]
        public StartingPosition Position { get; set; }

        /// <summary>
        /// Gets or sets the criteria the engine uses to know when
        /// it should stop the evaluation.
        /// </summary>
        public CompletionCriteria CompletionCriteria { get; set; }

        /// <summary>
        /// Gets or sets a list of moves the engine is allowed to consider
        /// in the start position. If the list is null or empty, the engine
        /// will consider all moves.
        /// </summary>
        public List<MoveFilter> SearchMoves { get; set; }

        /// <summary>
        /// Gets or sets the time control and remaining time for the players.
        /// </summary>
        public ClockState Clock { get; set; }

        /// <summary>
        /// Gets or sets the list of engine options.
        /// </summary>
        public Dictionary<string, OptionValue> EngineOptions { get; set; }

        /// <summary>
        /// Gets or sets the name of the engine.
        /// </summary>
        public string EngineName { get; set; }

        /// <summary>
        /// Gets or sets the number of CPU cores to use.
        /// </summary>
        public int Cores { get; set; }

        /// <summary>
        /// Gets or sets the number of GPUs to use.
        /// </summary>
        public int GPUs { get; set; }
    }
}