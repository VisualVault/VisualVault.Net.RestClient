using System;
using System.Collections.Generic;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Forms.FormInstances
{
    /// <summary>
    /// Manages form instances via the Forms Api
    /// </summary>
    public class FormInstancesManager : FormsApi
    {
        protected FormInstancesManager()
        {

        }
        public FormInstancesManager(FormsApi api)
        {
            base.Populate(api);
        }

        /// <summary>
        /// Create a new form instance
        /// </summary>
        /// <param name="templateRevisionId"></param>
        /// <param name="formInstance"></param>
        /// <returns>Form Instance</returns>
        public FormInstance CreateNewFormInstance(Guid templateRevisionId, List<KeyValuePair<string, object>> formInstance)
        {
            var FormInstanceRequest = new FormInstanceRequest
            {
                FormTemplateId = templateRevisionId,
                Fields = formInstance
            };

            var result = HttpHelper.PostNoCustomerAlias<FormInstance>(GlobalConfiguration.RoutesFormsApi.FormInstance, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, FormInstanceRequest);
            result.Revision = 1;
            return result.Meta.StatusCode == System.Net.HttpStatusCode.Created ? result : null;
        }


        /// <summary>
        /// update an existing form instance, with option to replace revision instead of creating a new revision
        /// </summary>
        /// <param name="templateRevisionId"></param>
        /// <param name="formId"></param>
        /// <param name="fieldValues"></param>
        /// <param name="replaceRevision"></param>
        /// <returns>Form Instance</returns>
        public FormInstance CreateNewFormInstanceRevision(Guid templateRevisionId, Guid formId, List<KeyValuePair<string, object>> fieldValues, bool replaceRevision = false)
        {
            var FormInstanceRequest = new UpdateFormInstanceRequest
            {
                FormTemplateId = templateRevisionId,
                FormId = formId,
                Fields = fieldValues,
                ReplaceRevision = replaceRevision
            };

            var result = HttpHelper.PutNoCustomerAlias<FormInstance>(GlobalConfiguration.RoutesFormsApi.FormInstance, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, FormInstanceRequest);
            return result.Meta.StatusCode == System.Net.HttpStatusCode.OK ? result : null;
        }
    }
}
