using System;
using System.Collections.Generic;
using System.Dynamic;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Email
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailManager : BaseApi
    {
        internal EmailManager(VaultApi api)
        {
            Populate(api.ClientSecrets, api.ApiTokens);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="ccRecipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="hasAttachments"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        public Email CreateEmail(string recipients, string ccRecipients, string subject, string body, bool hasAttachments, List<Guid> attachments = null)
        {
            dynamic postData = new ExpandoObject();

            postData.recipients = recipients;
            postData.ccrecipients = ccRecipients;
            postData.subject = subject;
            postData.body = body;
            postData.hasattachments = hasAttachments;
            postData.documents = attachments;

            var result = HttpHelper.Post<Email>(GlobalConfiguration.Routes.Emails, string.Empty, GetUrlParts(), ClientSecrets, this.ApiTokens, postData);

            return result;
        }
    }
}
