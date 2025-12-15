using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using VVRestApi.Common;
using VVRestApi.Objects.Enums;

namespace VVRestApi.Objects.Models
{
    public class Model : RestObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid ModifyById { get; set; }

        public string ModifyBy { get; set; } = string.Empty;

        public DateTime ModifyDate { get; set; }

        public IEnumerable<Property> PropertyList { get; set; }

        public IEnumerable<RelatedModel> RelatedModels { get; set; } 
    }
}
