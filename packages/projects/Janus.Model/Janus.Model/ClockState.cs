// -----------------------------------------------------------------------
// <copyright file="ClockState.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Model
{
    using System;

    /// <summary>
    /// A data structure containing information about the time control
    /// and state of the clock.
    /// </summary>
    public class ClockState
    {
        /// <summary>
        /// Gets or sets the amount of time white has on the clock.
        /// </summary>
        public TimeSpan? WhiteTime { get; set; }

        /// <summary>
        /// Gets or sets the amount of time black has on the clock.
        /// </summary>
        public TimeSpan? BlackTime { get; set; }

        /// <summary>
        /// Gets or sets the additional time (increment) white receives
        /// after each move.
        /// </summary>
        public TimeSpan? WhiteIncrement { get; set; }

        /// <summary>
        /// Gets or sets the additional time (increment) black receives
        /// after each move.
        /// </summary>
        public TimeSpan? BlackIncrement { get; set; }

        /// <summary>
        /// Gets or sets the number of moves remaining until
        /// the next time control.
        /// </summary>
        public ushort? MovesToGo { get; set; }
    }
}