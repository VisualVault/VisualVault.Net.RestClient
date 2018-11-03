using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.ScheduledProcess
{
    public class ScheduledProcessManager : VVRestApi.Common.BaseApi
    {
        internal ScheduledProcessManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        public void CallCompleteScheduledProcessUsingQueryString(Guid token, string message, bool scheduledProcessOutcome)
        {
            var action = "Complete";

            var queryString = new StringBuilder();

            queryString.AppendFormat("{0}={1}", "action", UrlEncode(action));
            queryString.AppendFormat("&{0}={1}", "message", UrlEncode(message));
            queryString.AppendFormat("&{0}={1}", "result", UrlEncode(scheduledProcessOutcome.ToString()));

            var result = HttpHelper.Post(VVRestApi.GlobalConfiguration.Routes.ScheduledProcessId, queryString.ToString(), GetUrlParts(), this.ApiTokens, null, token);
        }

        public void CallCompleteScheduledProcessUsingPostedData(Guid token, string message, bool scheduledProcessOutcome)
        {
            dynamic postData = new ExpandoObject();

            postData.action = "Complete";

            if (!String.IsNullOrWhiteSpace(message))
            {
                postData.message = message;
            }

            postData.result = scheduledProcessOutcome.ToString().ToLower();

            var result = HttpHelper.Post(VVRestApi.GlobalConfiguration.Routes.ScheduledProcessId, "", GetUrlParts(), this.ApiTokens, postData, token);
        }

        public void RunScheduledProcesses()
        {
            dynamic postData = new ExpandoObject();

            HttpHelper.Put(VVRestApi.GlobalConfiguration.Routes.ScheduledProcessRun, "", GetUrlParts(), this.ApiTokens, this.ClientSecrets, postData);
        }
    }
}
