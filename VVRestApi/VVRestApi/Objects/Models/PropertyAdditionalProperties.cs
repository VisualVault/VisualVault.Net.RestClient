using System;
using System.Collections.Generic;
using System.Text;

namespace VVRestApi.Objects.Models
{
    public class PropertyAdditionalProperties
    {
        public string InitialValue { get; set; }

        public IEnumerable<string> ItemList { get; set; }
    }
}
