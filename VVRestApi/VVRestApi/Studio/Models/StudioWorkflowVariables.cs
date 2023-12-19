using System;
using System.Collections.Generic;
using VVRestApi.Common;

namespace VVRestApi.Studio.Models
{
    public class StudioWorkflowVariables : RestObject
    {
        public StudioWorkflowVariables()
        {
        }

        public List<WorkflowVariable> WorkflowVariables { get; set; }
        public List<WorkflowVariable> DataSetVariables { get; set; }
    }
}
