using System;
using System.Collections.Generic;
using System.Text;

namespace VVRestApi.Common.Search
{
    public abstract class CriteriaListItemBase
    {
        /// <summary>
        /// Type of comparison or operation applied to the criteria (e.g. Equal, GreaterThan, In).
        /// </summary>
        public SearchCriteriaType Clause { get; set; }

        /// <summary>
        /// Logical condition used to combine this criteria with others (e.g. AND, OR).
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// Value used for the comparison.
        /// </summary>
        public string Criteria { get; set; }

        /// <summary>
        /// Indicates whether this criteria starts with a left parenthesis "(".
        /// Used for grouping logical expressions.
        /// </summary>
        public bool LeftBracket { get; set; }

        /// <summary>
        /// Field where the criteria is applied.
        /// </summary>
        public string LookIn { get; set; }

        /// <summary>
        /// Indicates whether this criteria ends with a right parenthesis ")".
        /// Used for grouping logical expressions.
        /// </summary>
        public bool RightBracket { get; set; }
    }
}
