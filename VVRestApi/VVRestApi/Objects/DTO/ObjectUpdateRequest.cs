using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VVRestApi.Objects.DTO
{
    public class ObjectUpdateRequest
    {
        public Guid RevisionId { get; set; }

        public Dictionary<string, object> Properties { get; set; }

        public List<RelatedObjectUpdateRequest> RelatedObjectUpdates { get; set; }
    }

    public class RelatedObjectUpdateRequest : ObjectUpdateRequest
    {
        public Guid Id { get; set; }
    }
}
