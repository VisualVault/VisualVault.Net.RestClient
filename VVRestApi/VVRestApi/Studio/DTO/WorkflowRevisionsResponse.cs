using System;
using System.Collections.Generic;
using System.Text;
using VVRestApi.Common;
using VVRestApi.Studio.Models;

namespace VVRestApi.Studio.DTO
{
    internal class WorkflowRevisionsResponse : RestObject
    {
        public List<StudioWorkflow> Revisions { get; set; }
    }
}
