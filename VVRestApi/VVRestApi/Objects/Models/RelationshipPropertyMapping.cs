using System;
using System.Collections.Generic;
using System.Text;

namespace VVRestApi.Objects.Models
{
    public class RelationshipPropertyMapping
    {
        public Guid FromPropertyId { get; set; }
        public Guid ToPropertyId { get; set; }
    }
}
