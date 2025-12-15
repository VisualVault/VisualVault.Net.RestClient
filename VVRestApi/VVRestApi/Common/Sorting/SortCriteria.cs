using System;
using System.Collections.Generic;
using System.Text;

namespace VVRestApi.Common.Sorting
{
    public class SortCriteria
    {
        public string SortField { get; set; } = string.Empty;
        public SortDirection Direction { get; set; } = SortDirection.Ascending;
    }
}
