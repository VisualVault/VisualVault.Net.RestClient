using System;
using VVRestApi.Studio.Models;

namespace VVRestApi.Studio.DTO
{
    internal class WorkflowRunRequest
    {
        public Guid ObjectId { get; set; }
        public string Reference { get; set; }
        public StudioWorkflowVariables Data { get; set; }
    }
}
