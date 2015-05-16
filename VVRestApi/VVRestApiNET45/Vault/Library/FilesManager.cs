using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Library
{
    public class FilesManager : VVRestApi.Common.BaseApi
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
            return HttpHelper.GetStream(VVRestApi.GlobalConfiguration.Routes.FilesId, "", options, GetUrlParts(), this.ApiTokens, this.ClientSecrets, documentRevisionId);
        }
    }
}
