using System;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Vault.Library;

namespace VVRestApi.Vault.Annotations
{
    /// <summary>
    /// creates and returns Annotations
    /// </summary>
    public class AnnotationManager : VVRestApi.Common.BaseApi
    {
        internal AnnotationManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        public int GetUserAnnotationPrivilege(Guid usId, string layerName, RequestOptions options = null)
        {
            var queryString = string.Format("layerName={0}", layerName);

            var result = HttpHelper.Get(VVRestApi.GlobalConfiguration.Routes.UsersIdAnnotationPrivilege, queryString, options, GetUrlParts(), this.ApiTokens, usId);

            return result.Value<int>("data");
        }
    }
}