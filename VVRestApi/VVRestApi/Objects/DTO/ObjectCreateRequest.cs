using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VVRestApi.Objects.DTO
{
    public class ObjectCreateRequest 
    {
        public Guid ModelId { get; set; }

        public Dictionary<string, object> Properties { get; set; }

        public List<RelatedObjectUpdateRequest> RelatedModels { get; set; }
    }
}
