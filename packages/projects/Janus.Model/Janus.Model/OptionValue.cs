// -----------------------------------------------------------------------
// <copyright file="OptionValue.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Model
{
    /// <summary>
    /// An engine option name and value to set.
    /// </summary>
    public class OptionValue
    {
        /// <summary>
        /// Gets or sets the text value of the option.
        /// </summary>
        public string TextValue { get; set; }

        /// <summary>
        /// Gets or sets the boolean value of the option.
        /// </summary>
        public bool? BooleanValue { get; set; }

        /// <summary>
        /// Gets or sets the integer value of the option.
        /// </summary>
        public int? IntegerValue { get; set; }
    }
}