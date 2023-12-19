using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Studio.DTO;

namespace VVRestApi.Studio.Models
{
    public class StudioWorkflow : RestObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int Revision { get; set; }
        public bool IsPublished { get; set; }
    }
}
