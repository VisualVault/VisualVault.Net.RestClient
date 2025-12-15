using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using VVRestApi.Common.Serialize;
using VVRestApi.Objects.Enums;

namespace VVRestApi.Objects.DTO
{
    public class ObjectSearchRequest 
    {
        public int Page { get; set; } = 0;
        public int Take { get; set; } = 10;
        public IEnumerable<SortCriteria> Sort { get; set; } 
        public IEnumerable<CriteriaListItem> CriteriaList { get; set; } 
        public IEnumerable<string> PropertyList { get; set; } 
    }

    public class SortCriteria
    {
        public string SortField { get; set; } = string.Empty;
        public SortDirection Direction { get; set; } = SortDirection.Ascending;
    }

    public class CriteriaListItem
    {
        public SearchCriteriaType Clause { get; set; }
        public string Condition { get; set; }
        public string Criteria { get; set; }
        public string DataviewSecondaryCondition { get; set; }
        public bool LeftBracket { get; set; }
        public string LookIn { get; set; }
        public string LookInForDataview { get; set; }
        public bool RightBracket { get; set; }
        public string Type { get; set; }
        [JsonConverter(typeof(NullableGuidConverter))]
        public Guid? LookInListItemId { get; set; }
        public string IndexFieldType { get; set; }
        public string TableName { get; set; }
    }
}
