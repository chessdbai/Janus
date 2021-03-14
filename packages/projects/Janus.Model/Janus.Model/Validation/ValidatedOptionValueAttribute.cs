// -----------------------------------------------------------------------
// <copyright file="ValidatedOptionValueAttribute.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Model.Validation
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// A custom validation attribute to validate OptionValue.
    /// </summary>
    public class ValidatedOptionValueAttribute : ValidationAttribute
    {
        private const string MissingOrAmbiguousValueSetError =
            "The option value must have one, and only one, of: StringValue, BooleanValue, IntegerValue set.";

        /// <inheritdoc cref="ValidationAttribute" />
        public override bool RequiresValidationContext => false;

        /// <summary>
        /// Determines if the given <see cref="OptionValue" /> is complete.
        /// </summary>
        /// <param name="value">The OptionValue instance.</param>
        /// <param name="validationContext">Validation context.</param>
        /// <returns>The result of the validation.</returns>
        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            var optionValue = (OptionValue)value;

            int nullCount = 0;
            nullCount += optionValue.TextValue == null ? 1 : 0;
            nullCount += optionValue.BooleanValue == null ? 1 : 0;
            nullCount += optionValue.IntegerValue == null ? 1 : 0;

            if (nullCount != 2)
            {
                return new ValidationResult(MissingOrAmbiguousValueSetError);
            }

            return ValidationResult.Success;
        }
    }
}