using System;
using System.Collections.Generic;
using System.Text;
using VVRestApi.Objects.Enums;

namespace VVRestApi.Objects.Models
{
    public class Property
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public PropertyType Type { get; set; }

        public bool Required { get; set; }

        public bool Unique { get; set; }

        public PropertyAdditionalProperties AdditionalProperties { get; set; }
    }
}
