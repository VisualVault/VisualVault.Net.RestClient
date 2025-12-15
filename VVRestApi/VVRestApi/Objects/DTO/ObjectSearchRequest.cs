using System.Collections.Generic;
using VVRestApi.Common;
using VVRestApi.Objects.Models;

namespace VVRestApi.Objects.DTO
{
    public class ObjectSearchRequest 
    {
        public int Page { get; set; } = 0;
        public int Take { get; set; } = 10;
        public IEnumerable<SortCriteria> Sort { get; set; } 
        public IEnumerable<ObjectCriteriaListItem> CriteriaList { get; set; } 
        public IEnumerable<string> PropertyList { get; set; } 
    }
}
