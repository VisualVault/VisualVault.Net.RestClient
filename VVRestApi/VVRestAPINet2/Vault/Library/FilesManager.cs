
using System;
using System.IO;
using VVRestAPINet2.Common;
using VVRestAPINet2.Common.Messaging;

namespace VVRestAPINet2.Vault.Library
{
    public class FilesManager : Common.BaseApi
    {
        internal FilesManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        /// <summary>
        /// Gets a folder by its path, returns null if none exists
        /// </summary>
        /// <param name="documentRevisionId"></param>
        /// <param name="options"> </param>
        /// <returns></returns>
        public Stream GetStream(Guid documentRevisionId, RequestOptions options = null)
        {
            return HttpHelper.GetStream(GlobalConfiguration.Routes.FilesId, "", options, GetUrlParts(), this.ApiTokens, this.ClientSecrets, documentRevisionId);
        }
    }
}
