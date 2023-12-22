using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using VVRestApi.Common.Messaging;
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
            return result?.Workflow;
        }

        public StudioWorkflow GetWorkflowByName(string name)
        {
            var result = HttpHelper.GetBaseUrl<WorkflowResponse>(GlobalConfiguration.RoutesStudioApi.WorkflowLatestPublished, $"name={name}", null, GetUrlParts(), ClientSecrets, ApiTokens);
            return result?.Workflow;
        }

        public StudioWorkflowVariables GetWorkflowVariables(Guid workflowId)
        {
            var result = HttpHelper.GetBaseUrl<StudioWorkflowVariables>(GlobalConfiguration.RoutesStudioApi.WorkflowVariables, "", null, GetUrlParts(), ClientSecrets, ApiTokens, workflowId);
            return result;
        }

        public Guid TriggerWorkflow(Guid workflowId, int workflowRevision, string objectId, List<WorkflowVariable> workflowVariables)
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

        public bool TerminateWorkflow(Guid workflowId, Guid workflowInstanceId)
        {
            var result = HttpHelper.PostBaseUrl(GlobalConfiguration.RoutesStudioApi.WorkflowTerminate, "", GetUrlParts(), ApiTokens, ClientSecrets, null, workflowId, workflowInstanceId);
            if (result != null)
            {
                JToken metaNode = result["meta"];
                if (metaNode != null)
                {
                    JToken statusNode = metaNode["status"];
                    if (statusNode != null)
                    {
                        if (Enum.TryParse<HttpStatusCode>(statusNode.ToString(), out var statusCode))
                            return statusCode == HttpStatusCode.OK;
                    }
                }
            }
            return false;
        }

        public WorkflowInstance GetRunningWorkflowForObject(string objectId, Guid workflowId)
        {
            var result = HttpHelper.GetBaseUrl<WorkflowInstance>(GlobalConfiguration.RoutesStudioApi.WorkflowHistoryRunningObject, $"", null, GetUrlParts(), ClientSecrets, ApiTokens, workflowId, objectId);
            return result;
        }

        public List<WorkflowInstance> GetWorkflowHistoryForObject(string objectId, Guid workflowId)
        {
            var result = HttpHelper.GetBaseUrl<WorkflowHistoryResponse>(GlobalConfiguration.RoutesStudioApi.WorkflowHistoryObject, $"", null, GetUrlParts(), ClientSecrets, ApiTokens, workflowId, objectId);
            return result?.Items;
        }
    }
}
