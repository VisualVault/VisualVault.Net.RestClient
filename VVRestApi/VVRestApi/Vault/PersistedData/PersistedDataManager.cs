using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.PersistedData
{
    using System;
    using System.Dynamic;

    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class PersistedDataManager : VVRestApi.Common.BaseApi
    {
        internal PersistedDataManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        /// <summary>
        /// Create data on the server which can be retrieved later. For use when build custom outside processes and for some custom SQL filters on controls, such as the Document List and Form Data controls, which will replace tokens with a serialized Dictionary converted to JSON. See VisualVault's online help for more information.
        /// </summary>
        /// <param name="uniqueName">Can be an empty string. If a name is given, it must be unique to the scope. Used to retrieve data by name instead of by ID.</param>
        /// <param name="dataScope">User scope: Only the current user will have access. Global scope: all users have access.</param>
        /// <param name="data">Data to post. For objects, you may want to post them as JSON.</param>
        /// <param name="dataMimeType">For your own use. Indicates what format the data is in, such as text/xml or text/json</param>
        /// <param name="linkedObjectId">Not required. </param>
        /// <param name="linkedObjectType">Can be LinkedObjectType.None. Used to help indicate what object this item is dependent on. For example, if it is linked to a search, when that search ID is deleted (indicated by the linkedObjectId), this data will also be deleted.</param>
        /// <param name="expirationDateUtc">Can be null for never expires. The expiration date of the data after which a background service will remove the data from VisualVault.</param>
        /// <returns></returns>
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

            return HttpHelper.Post<PersistedClientData>(VVRestApi.GlobalConfiguration.Routes.PersistedData, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
        }

        /// <summary>
        /// Gets persisted client data by scope, use the options to define a query
        /// </summary>
        /// <param name="dataScope">The scope of data to return, either user or global</param>
        /// <param name="options">A set of options to define how many items to return, a custom query, etc.</param>
        /// <returns></returns>
        public Page<PersistedClientData> GetData(ScopeType dataScope, RequestOptions options = null)
        {
            return HttpHelper.GetPagedResult<PersistedClientData>(VVRestApi.GlobalConfiguration.Routes.PersistedData, "scope=" + (int)dataScope, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        /// <summary>
        /// Gets persisted client data by id, returns null if none exists
        /// </summary>
        /// <param name="id"> </param>
        /// <param name="options">A set of options to define how many items to return, a custom query, etc.</param>
        /// <returns></returns>
        public PersistedClientData GetData(Guid id, RequestOptions options = null)
        {
            if (options == null)
            {
                options = new RequestOptions { Expand = true };
            }

            return HttpHelper.Get<PersistedClientData>(VVRestApi.GlobalConfiguration.Routes.PersistedDataId, string.Empty, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, id);
        }

        /// <summary>
        /// Gets all the persisted client data you have access to, use the options to define a query
        /// </summary>
        /// <param name="options">A set of options to define how many items to return, a custom query, etc.</param>
        /// <returns></returns>
        public Page<PersistedClientData> GetAllData(RequestOptions options = null)
        {

            return HttpHelper.GetPagedResult<PersistedClientData>(VVRestApi.GlobalConfiguration.Routes.PersistedData, string.Empty, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

    }
}