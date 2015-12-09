using System;
using System.Collections.Generic;
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

        public void CallCompleteScheduledProcess(Guid scheduledProcessId, string message, bool scheduledProcessOutcome)
        {
            var action = "Complete";

            var queryString = new StringBuilder();

            queryString.AppendFormat("{0}={1}&", "action", action);
            queryString.AppendFormat("{0}={1}&", "message", message);
            queryString.AppendFormat("{0}={1}", "result", scheduledProcessOutcome);

            var result = HttpHelper.Post(VVRestApi.GlobalConfiguration.Routes.ScheduledProcessId, queryString.ToString(), GetUrlParts(), this.ApiTokens, null);
        }

    }
}
