using System;
using System.Collections.Generic;
using System.Text;
using VVRestApi.Common;
using VVRestApi.Studio.Models;

namespace VVRestApi.Studio.DTO
{
    internal class WorkflowHistoryResponse : RestObject
    {
        public WorkflowHistoryResponse() { }

        public List<WorkflowInstance> Items { get; set; }
    }
}
