namespace VVRestApi.Vault.PersistedData
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;

    using VVRestApi.Common;

    public class PersistedDataManager : VVRestApi.Common.BaseApi
    {
        internal PersistedDataManager(VaultApi api)
        {
            base.Populate(api.CurrentToken);
        }

        public PersistedClientData CreateData(string uniqueName, ScopeType dataScope, string data, string dataMimeType, string linkedObjectId, LinkedObjectType linkedObjectType, DateTime? expirationDateUtc)
        {
            dynamic postData = new ExpandoObject();
            postData.Name = uniqueName;
            postData.Scope = (int)dataScope;
            postData.PersistedData = data;
            postData.DataMimeType = dataMimeType;
            postData.LinkedObjectId = linkedObjectId;
            postData.LinkedObjectType = (int)linkedObjectType;
            postData.ExpirationDateUtc = expirationDateUtc;

            return HttpHelper.Post<PersistedClientData>(VVRestApi.GlobalConfiguration.Routes.PersistedData, string.Empty, this.CurrentToken, postData);
        }

        /// <summary>
        /// Gets persisted client data by scope, use the options to define a query
        /// </summary>
        /// <param name="uniqueName">The name of the site to get</param>
        /// <param name="fields">A comma-delimited list of field names to return.</param>
        /// <returns></returns>
        public Page<PersistedClientData> GetData(ScopeType dataScope, RequestOptions options = null)
        {
            return HttpHelper.GetPagedResult<PersistedClientData>(VVRestApi.GlobalConfiguration.Routes.PersistedData, "scope=" + (int)dataScope, options, this.CurrentToken);
        }

        /// <summary>
        /// Gets persisted client data by id, returns null if none exists
        /// </summary>
        /// <param name="uniqueName">The name of the site to get</param>
        /// <param name="fields">A comma-delimited list of field names to return.</param>
        /// <returns></returns>
        public PersistedClientData GetData(Guid id, RequestOptions options = null)
        {
            if (options == null)
            {
                options = new RequestOptions() {Expand = true};
            }

            return HttpHelper.Get<PersistedClientData>(VVRestApi.GlobalConfiguration.Routes.PersistedDataId, string.Empty, options, this.CurrentToken, id);
        }


        /// <summary>
        /// Gets all the persisted client data you have access to, use the options to define a query
        /// </summary>
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <returns></returns>
        public Page<PersistedClientData> GetAllData(RequestOptions options = null)
        {
            
            return HttpHelper.GetPagedResult<PersistedClientData>(VVRestApi.GlobalConfiguration.Routes.PersistedData, string.Empty, options, this.CurrentToken);
        }


    }
}