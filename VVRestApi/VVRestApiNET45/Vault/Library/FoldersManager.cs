using System;
using System.Collections.Generic;
using System.Dynamic;
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

        /// <summary>
        /// returns the documents of a folder
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<Document> GetFolderDocuments(Guid folderId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.GetListResult<Document>(VVRestApi.GlobalConfiguration.Routes.FolderDocuments, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId);
        }



        /// <summary>
        /// returns the Folder's IndexFields
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<FolderIndexField> GetFolderIndexFields(Guid folderId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            
            return HttpHelper.GetListResult<FolderIndexField>(VVRestApi.GlobalConfiguration.Routes.FoldersIndexFields, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId);
        }

        /// <summary>
        /// returns a specific Folder IndexField
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="fieldId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public FolderIndexField GetFolderIndexField(Guid folderId, Guid fieldId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.Get<FolderIndexField>(VVRestApi.GlobalConfiguration.Routes.FoldersIndexFieldsId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId, fieldId);
        }


    }
}