using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Library
{
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class FoldersManager : VVRestApi.Common.BaseApi
    {
        internal FoldersManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        /// <summary>
        /// Gets a folder by its path, returns null if none exists
        /// </summary>
        /// <param name="folderPath"> </param>
        /// <param name="options"> </param>
        /// <returns></returns>
        public Folder GetFolderByPath(string folderPath, RequestOptions options = null)
        {
            return HttpHelper.Get<Folder>(VVRestApi.GlobalConfiguration.Routes.Folders, "folderPath=" + this.UrlEncode(folderPath), options, GetUrlParts(), this.ClientSecrets,this.ApiTokens);
        }

    }
}