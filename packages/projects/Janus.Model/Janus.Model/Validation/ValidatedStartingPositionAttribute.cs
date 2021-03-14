// -----------------------------------------------------------------------
// <copyright file="ValidatedStartingPositionAttribute.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Model.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ChessBot.Pgn;

    /// <summary>
    /// A custom validation attribute to ensure that the starting position is valid.
    /// </summary>
    public class ValidatedStartingPositionAttribute : ValidationAttribute
    {
        private const string MissingValueError = "One, and only one, of 'Fen' and 'Pgn', must be set.";
        private const string InvalidPgnGameError = "If using Pgn starting position, the PGN file must be valid: ";

        /// <inheritdoc cref="ValidationAttribute" />
        public override bool RequiresValidationContext => false;

        /// <summary>
        /// Determines if the given <see cref="StartingPosition" /> is complete.
        /// </summary>
        /// <param name="value">The OptionValue instance.</param>
        /// <param name="validationContext">Validation context.</param>
        /// <returns>The result of the validation.</returns>
        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            var startingPosition = (StartingPosition)value;
            int nullCount = 0;
            nullCount += string.IsNullOrEmpty(startingPosition.Pgn) ? 1 : 0;
            nullCount += string.IsNullOrEmpty(startingPosition.Fen) ? 1 : 0;

            if (nullCount != 1)
            {
                return new ValidationResult(MissingValueError);
            }

            if (!string.IsNullOrEmpty(startingPosition.Pgn))
            {
                var parser = new PgnGameParser();
                try
                {
                    parser.ParsePgn(startingPosition.Pgn);
                }
                catch (Exception e)
                {
                    return new ValidationResult(InvalidPgnGameError + " " + e.Message);
                }
            }

            return ValidationResult.Success;
        }
    }
}