using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using VVRestApi.Common;

namespace VVRestApi.Studio.Models
{
    public class WorkflowInstance : RestObject
    {
        [JsonProperty("instance_id")]
        public Guid Id { get; set; }

        [JsonProperty("object_id")]
        public string ObjectId { get; set; }

        [JsonProperty("workflow_definition_id")]
        public Guid WorkflowId { get; set; }

        [JsonProperty("workflow_name")]
        public string WorkflowName { get; set; }

        [JsonProperty("version")]
        public int WorkflowVersion { get; set; }

        [JsonProperty("status")]
        public WorkflowStatus Status { get; set; }

        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        [JsonProperty("end_time")]
        public DateTime EndTime { get; set; }

        [JsonProperty("persistence_id")]
        public int PersistenceId { get; set; }

        [JsonProperty("initiated_by")]
        public string InitiatedBy { get; set; }
    }

    [DefaultValue(Unknown)]
    public enum WorkflowStatus
    {
        Unknown = -1,
        Running = 0,
        Suspended = 1,
        Completed = 2,
        Terminated = 3

    }
}
