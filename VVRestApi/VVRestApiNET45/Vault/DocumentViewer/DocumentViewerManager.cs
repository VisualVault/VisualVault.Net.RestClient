using System;
using System.Collections.Generic;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.DocumentViewer
{
    public class DocumentViewerManager : VVRestApi.Common.BaseApi
    {

        internal DocumentViewerManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }


        public List<AnnotationLayer> GetAnnotationLayers()
        {
            return HttpHelper.GetListResult<AnnotationLayer>(VVRestApi.GlobalConfiguration.Routes.DocumentViewerAnnotationsLayers, "", null, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        public int GetUserAnnotationPrivilege(Guid usId, string layerName, RequestOptions options = null)
        {
            var queryString = string.Format("layerName={0}", layerName);

            var result = HttpHelper.Get(VVRestApi.GlobalConfiguration.Routes.UsersIdAnnotationPrivilege, queryString, options, GetUrlParts(), this.ApiTokens, usId);

            return result.Value<int>("data");
        }


    }
}