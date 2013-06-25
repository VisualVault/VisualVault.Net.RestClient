namespace VVRestApi.Vault.Library
{
    using VVRestApi.Common;

    public class FoldersManager : VVRestApi.Common.BaseApi
    {
        internal FoldersManager(VaultApi api)
        {
            base.Populate(api.CurrentToken);
        }

        /// <summary>
        /// Gets a folder by its path, returns null if none exists
        /// </summary>
        /// <param name="folderPath">The folder path of the folder to get</param>
        /// <param name="fields">A comma-delimited list of field names to return.</param>
        /// <returns></returns>
        public Folder GetFolderByPath(string folderPath, RequestOptions options = null)
        {
            return HttpHelper.Get<Folder>(VVRestApi.GlobalConfiguration.Routes.Folders, "folderPath=" + this.UrlEncode(folderPath), options, this.CurrentToken);
        }

    }
}