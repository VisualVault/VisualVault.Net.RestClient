using System;
using System.Collections.Generic;
using System.Text;

namespace VVRestApi.Common
{
    public abstract class CriteriaListItemBase
    {
        public SearchCriteriaType Clause { get; set; }
        public string Condition { get; set; }
        public string Criteria { get; set; }
        public bool LeftBracket { get; set; }
        public string LookIn { get; set; }
        public bool RightBracket { get; set; }
    }
}
