using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="ccRecipients"></param>
        /// <param name="bccRecipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="hasAttachments"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        public Email CreateEmail(string recipients, string ccRecipients, string bccRecipients, string subject, string body, bool hasAttachments, List<Guid> attachments = null)
        {
            dynamic postData = new ExpandoObject();

            postData.recipients = recipients;
            postData.ccrecipients = ccRecipients;
            postData.bccrecipients = bccRecipients;
            postData.subject = subject;
            postData.body = body;
            postData.hasattachments = hasAttachments;
            postData.documents = attachments;

            var result = HttpHelper.Post<Email>(GlobalConfiguration.Routes.Emails, string.Empty, GetUrlParts(), ClientSecrets, this.ApiTokens, postData);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="ccRecipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="hasAttachments"></param>
        /// <param name="attachments">List of file attachments, where the key represents the filename and the value contains the data stream</param>
        /// <returns></returns>
        public Email CreateEmail(string recipients, string ccRecipients, string subject, string body, bool hasAttachments, List<KeyValuePair<string, Stream>> attachments = null)
        {
            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("recipients", recipients),
                new KeyValuePair<string, string>("ccrecipients", ccRecipients),
                new KeyValuePair<string, string>("subject", subject),
                new KeyValuePair<string, string>("body", body),
                new KeyValuePair<string, string>("hasattachments", hasAttachments.ToString()),
            };

            var result = HttpHelper.PostMultiPart<Email>(GlobalConfiguration.Routes.Emails, string.Empty, GetUrlParts(), this.ApiTokens, ClientSecrets, postData, attachments);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="ccRecipients"></param>
        /// <param name="bccRecipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="hasAttachments"></param>
        /// <param name="attachments">List of file attachments, where the key represents the filename and the value contains the data stream</param>
        /// <returns></returns>
        public Email CreateEmail(string recipients, string ccRecipients, string bccRecipients, string subject, string body, bool hasAttachments, List<KeyValuePair<string, Stream>> attachments = null)
        {
            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("recipients", recipients),
                new KeyValuePair<string, string>("ccrecipients", ccRecipients),
                new KeyValuePair<string, string>("bccrecipients", bccRecipients),
                new KeyValuePair<string, string>("subject", subject),
                new KeyValuePair<string, string>("body", body),
                new KeyValuePair<string, string>("hasattachments", hasAttachments.ToString()),
            };

            var result = HttpHelper.PostMultiPart<Email>(GlobalConfiguration.Routes.Emails, string.Empty, GetUrlParts(), this.ApiTokens, ClientSecrets, postData, attachments);

            return result;
        }
    }
}
