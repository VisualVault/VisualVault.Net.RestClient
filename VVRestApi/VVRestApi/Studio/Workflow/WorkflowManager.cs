using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Forms;
using VVRestApi.Studio.DTO;
using VVRestApi.Studio.Models;

namespace VVRestApi.Studio.Workflow
{
    /// <summary>
    /// Manages Workflow related tasks in VVStudio
    /// </summary>
    public class WorkflowManager : StudioApi
    {
        protected WorkflowManager()
        {

        }
        public WorkflowManager(StudioApi api)
        {
            base.Populate(api);
        }

        public StudioWorkflow GetWorkflow(Guid id)
        {
            var result = HttpHelper.GetBaseUrl<WorkflowResponse>(GlobalConfiguration.RoutesStudioApi.WorkflowLatestPublishedId, $"", null, GetUrlParts(), ClientSecrets, ApiTokens, id);
            return result.Workflow;
        }

        public StudioWorkflow GetWorkflowByName(string name)
        {
            var result = HttpHelper.GetBaseUrl<WorkflowResponse>(GlobalConfiguration.RoutesStudioApi.WorkflowLatestPublished, $"name={name}", null, GetUrlParts(), ClientSecrets, ApiTokens);
            return result.Workflow;
        }

        public StudioWorkflowVariables GetWorkflowVariables(Guid workflowId)
        {
            var result = HttpHelper.GetBaseUrl<StudioWorkflowVariables>(GlobalConfiguration.RoutesStudioApi.WorkflowVariables, "", null, GetUrlParts(), ClientSecrets, ApiTokens, workflowId);
            return result;
        }

        public Guid TriggerWorkflow(Guid workflowId, int workflowRevision, Guid objectId, List<WorkflowVariable> workflowVariables)
        {
            var workflowRequestData = new StudioWorkflowVariables
            {
                WorkflowVariables = workflowVariables,
                DataSetVariables = new List<WorkflowVariable>()
            };

            var workflowRequest = new WorkflowRunRequest
            {
                ObjectId = objectId,
                Reference = "API",
                Data = workflowRequestData
            };

            var result = HttpHelper.PostBaseUrl(GlobalConfiguration.RoutesStudioApi.WorkflowRun, "", GetUrlParts(), ApiTokens, ClientSecrets, workflowRequest, workflowId, workflowRevision);
            if (result != null)
            {
                JToken dataNode = result["data"];
                if (dataNode != null)
                {
                    if (Guid.TryParse(dataNode.ToString(), out var workflowInstanceId))
                        return workflowInstanceId;
                }
            }
            return Guid.Empty;
        }
    }
}
