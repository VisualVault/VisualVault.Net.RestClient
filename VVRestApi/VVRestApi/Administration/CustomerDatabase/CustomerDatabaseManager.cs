using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using Newtonsoft.Json;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Vault.Users;
using VVRestApi.Vault;


namespace VVRestApi.Administration.Customers
{
    using Newtonsoft.Json.Linq;
    using System;
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class CustomerDatabaseManager : BaseApi
    {
        internal CustomerDatabaseManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }                    

        /// <summary>
        /// Assign a user to a customer database
        /// </summary>
        /// <param name="databaseId">Database ID of database</param>
        /// <param name="userId">Username of user</param>
        /// <returns></returns>
        public ApiMetaData AssignUser(Guid databaseId, string userId)
        {
            dynamic putData = new ExpandoObject();
            putData.userId = userId;

            return HttpHelper.PutNoCustomerAliasReturnMeta(GlobalConfiguration.Routes.CustomerDatabaseAssignUser, string.Empty, GetUrlParts(), ApiTokens, ClientSecrets, putData, databaseId);
        }

        public ApiMetaData RemoveUserFromCustomerDatabase(string databaseId, string userId)
        {
            dynamic putData = new ExpandoObject();
            putData.userId = userId;

            return HttpHelper.DeleteNoCustomerAliasReturnMeta(GlobalConfiguration.Routes.CustomerDatabaseDeleteUser, string.Empty, GetUrlParts(), ApiTokens, ClientSecrets, putData, databaseId, userId);
        }


    }
}