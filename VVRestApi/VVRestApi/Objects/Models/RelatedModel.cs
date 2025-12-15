using System;
using System.Collections.Generic;
using System.Text;
using VVRestApi.Objects.Enums;

namespace VVRestApi.Objects.Models
{
    public class RelatedModel
    {
        public Guid Id { get; set; }
        public Guid RelationshipId { get; set; }

        public Guid ModelToId { get; set; }

        public string Name { get; set; } = string.Empty;

        public RelationshipType RelationshipType { get; set; }

        public RelatedModelRole RelatedModelRole { get; set; }

        public IEnumerable<Property> PropertyList { get; set; }

        public IEnumerable<RelationshipPropertyMapping> PropertyMappings { get; set; }
    }
}
