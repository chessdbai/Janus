// -----------------------------------------------------------------------
// <copyright file="ValidatedMoveFilter.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Model.Validation
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// A custom validator to ensure that a move filter contains either a SAN move
    /// or a UCI move.
    /// </summary>
    public class ValidatedMoveFilter : ValidationAttribute
    {
        private const string MissingValueError = "One, and only one, of 'UCI' and 'SAN', must be set.";

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
            var moveFilter = (MoveFilter)value;
            int nullCount = 0;
            nullCount += string.IsNullOrEmpty(moveFilter.UCI) ? 1 : 0;
            nullCount += string.IsNullOrEmpty(moveFilter.SAN) ? 1 : 0;

            if (nullCount != 1)
            {
                return new ValidationResult(MissingValueError);
            }

            return ValidationResult.Success;
        }
    }
}