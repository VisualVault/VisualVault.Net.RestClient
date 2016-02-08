using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
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
        /// gets the top level folders
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<Folder> GetTopLevelFolders(RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.GetListResult<Folder>(VVRestApi.GlobalConfiguration.Routes.Folders, "folderPath=" + this.UrlEncode("/"), options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
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

        public List<Document> GetFolderDocuments(Guid folderId, string sortBy, string sortDirection, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            var sb = new StringBuilder();
            sb.Append("sort=");
            sb.Append(UrlEncode(sortBy));
            sb.Append("&sortDir=");
            sb.Append(UrlEncode(sortDirection));

            return HttpHelper.GetListResult<Document>(VVRestApi.GlobalConfiguration.Routes.FolderDocuments, sb.ToString(), options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId);
        }

        public Folder GetFolderByFolderId(Guid folderId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.Get<Folder>(VVRestApi.GlobalConfiguration.Routes.FoldersId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId);
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

            return HttpHelper.Get<FolderIndexField>(VVRestApi.GlobalConfiguration.Routes.FoldersIdIndexFieldsId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId, fieldId);
        }

        /// <summary>
        /// returns a list of child folders
        /// </summary>
        public List<Folder> GetChildFolders(Guid parentFolderid, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.GetListResult<Folder>(VVRestApi.GlobalConfiguration.Routes.FoldersIdFolders, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, parentFolderid);
        }

        public Folder CreateTopLevelFolder(string name, string description, bool allowRevisions, string prefix, string suffix, DocDatePosition datePosition, DocSeqType sequenceType, ExpireAction expireAction, bool expirationRequired, int expirationDays, bool reviewRequired, int reviewDays)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required but was an empty string", "name");
            }

            dynamic postData = new ExpandoObject();
            postData.Name = name;
            if (!String.IsNullOrWhiteSpace(description))
            {
                postData.Description = description;
            }
            postData.allowRevisions = allowRevisions;

            if (!String.IsNullOrWhiteSpace(prefix))
            {
                postData.dcPrefix = prefix;
            }
            if (!String.IsNullOrWhiteSpace(suffix))
            {
                postData.dcSuffix = suffix;
            }
            postData.dcDatePosition = datePosition;

            postData.dcSeqType = sequenceType;
            postData.expireAction = expireAction;
            postData.expirationRequired = expirationRequired;
            postData.expirationDays = expirationDays;
            postData.reviewRequired = reviewRequired;
            postData.reviewDays = reviewDays;

            return HttpHelper.Post<Folder>(GlobalConfiguration.Routes.Folders, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
        }

        public Folder CreateChildFolder(Guid folderId, string name)
        {
            if (folderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("FolderId is required but was an empty Guid", "folderId");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required but was an empty string", "name");
            }

            dynamic postData = new ExpandoObject();
            postData.Name = name;

            return HttpHelper.Post<Folder>(GlobalConfiguration.Routes.FoldersId, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, folderId);
        }

        public Folder CreateChildFolder(Guid folderId, string name, string description, bool inheritNamingConventions, bool inheritRecordRetention, bool allowRevision, string prefix, string suffix, DocDatePosition datePosition, DocSeqType sequenceType, ExpireAction expireAction, bool expirationRequired, int expirationDays, bool reviewRequired, int reviewDays)
        {
            if (folderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("FolderId is required but was an empty Guid", "folderId");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required but was an empty string", "name");
            }

            dynamic postData = new ExpandoObject();
            postData.Name = name;
            if (!String.IsNullOrWhiteSpace(description))
            {
                postData.Description = description;
            }
            postData.allowRevision = allowRevision;

            postData.inheritNamingConvention = inheritNamingConventions;
            postData.inheritRecordRetention = inheritRecordRetention;

            if (!String.IsNullOrWhiteSpace(prefix))
            {
                postData.dcPrefix = prefix;
            }
            if (!String.IsNullOrWhiteSpace(suffix))
            {
                postData.dcSuffix = suffix;
            }
            postData.dcDatePosition = datePosition;

            postData.dcSeqType = sequenceType;
            postData.expireAction = expireAction;
            postData.expirationRequired = expirationRequired;
            postData.expirationDays = expirationDays;
            postData.reviewRequired = reviewRequired;
            postData.reviewDays = reviewDays;

            return HttpHelper.Post<Folder>(GlobalConfiguration.Routes.FoldersId, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, folderId);
        }

        public Folder CreateFolderByPath(string path, string description, bool inheritNamingConventions, bool inheritRecordRetention, bool allowRevision, string prefix, string suffix, DocDatePosition datePosition, DocSeqType sequenceType, ExpireAction expireAction, bool expirationRequired, int expirationDays, bool reviewRequired, int reviewDays)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path is required but was an empty string", "path");
            }

            dynamic postData = new ExpandoObject();

            postData.folderpath = path;

            if (!String.IsNullOrWhiteSpace(description))
            {
                postData.Description = description;
            }
            postData.allowRevisions = allowRevision;

            postData.inheritNamingConvention = inheritNamingConventions;
            postData.inheritRecordRetention = inheritRecordRetention;

            if (!String.IsNullOrWhiteSpace(prefix))
            {
                postData.dcPrefix = prefix;
            }
            if (!String.IsNullOrWhiteSpace(suffix))
            {
                postData.dcSuffix = suffix;
            }
            postData.dcDatePosition = datePosition;

            postData.dcSeqType = sequenceType;
            postData.expireAction = expireAction;
            postData.expirationRequired = expirationRequired;
            postData.expirationDays = expirationDays;
            postData.reviewRequired = reviewRequired;
            postData.reviewDays = reviewDays;

            return HttpHelper.Post<Folder>(GlobalConfiguration.Routes.Folders, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
        }


        public FolderIndexField UpdateFolderIndexField(Guid folderId, Guid fieldId, bool overriden, Guid queryId, string displayField, string valueField, Guid dropDownListId, bool required, string defaultValue)
        {
            if (folderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("FolderId is required but was an empty Guid", "folderId");
            }

            if (fieldId.Equals(Guid.Empty))
            {
                throw new ArgumentException("FieldId is required but was an empty Guid", "fieldId");
            }

            dynamic postData = new ExpandoObject();

            postData.overriden = overriden;
            postData.queryId = queryId;
            postData.queryValueField = valueField;
            postData.queryDisplayField = displayField;
            postData.dropDownListId = dropDownListId;
            postData.required = required;
            postData.defaultValue = defaultValue;

            return HttpHelper.Put<FolderIndexField>(GlobalConfiguration.Routes.FoldersIdIndexFieldsId, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, folderId, fieldId);
        }
        
        public List<IndexFieldSelectOption> GetFolderIndexFieldSelectOptionsList(Guid folderId, Guid fieldId, RequestOptions options = null)
        {            
            if (folderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("FolderId is required but was an empty Guid", "folderId");
            }
                        
            if (fieldId.Equals(Guid.Empty))
            {
                throw new ArgumentException("FieldId is required but was an empty Guid", "fieldId");
            }
            
            return HttpHelper.GetListResult<IndexFieldSelectOption>(VVRestApi.GlobalConfiguration.Routes.FoldersIdIndexFieldsIdSelectOptions, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId, fieldId);
        }
        
        public List<SecurityMember> GetFolderSecurityMembers(Guid folderId, RequestOptions options = null)
        {
            if(options != null)
            {
                if (!string.IsNullOrWhiteSpace(options.Fields))
                {
                    options.Fields = UrlEncode(options.Fields);
                }
            }

            return HttpHelper.GetListResult<SecurityMember>(VVRestApi.GlobalConfiguration.Routes.FoldersIdSecurityMembers, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId);
        }

        public int UpdateSecurityMembers(Guid folderId, List<SecurityMemberApplyAction> securityActionList, bool cascadeSecurityChanges = false)
        {
            var successCount = 0;

            dynamic postData = new ExpandoObject();
            postData.securityActions = securityActionList;
            postData.cascadeSecurityChanges = cascadeSecurityChanges;

            var result = HttpHelper.Put(VVRestApi.GlobalConfiguration.Routes.FoldersIdSecurityMembers, "", GetUrlParts(), this.ApiTokens, postData, folderId);
            var data = result.GetValue("data") as JObject;
            if (data != null)
            {
                successCount = Convert.ToInt32(data.GetValue("successCount").Value<string>());
            }

            return successCount;
        }

        public int AddSecurityMember(Guid folderId, Guid memberId, MemberType memberType, RoleType securityRole, bool cascadeSecurityChanges = false)
        {
            var successCount = 0;

            dynamic postData = new ExpandoObject();
            postData.memberType = memberType;
            postData.securityRole = securityRole;
            postData.cascadeSecurityChanges = cascadeSecurityChanges;

            var result = HttpHelper.Put(VVRestApi.GlobalConfiguration.Routes.FoldersIdSecurityMembersId, "", GetUrlParts(), this.ApiTokens, postData, folderId, memberId);
            var data = result.GetValue("data") as JObject;
            if (data != null)
            {
                successCount = Convert.ToInt32(data.GetValue("successCount").Value<string>());
            }

            return successCount;
        }

        public int RemoveSecurityMember(Guid folderId, Guid memberId, bool cascadeSecurityChanges = false)
        {
            var successCount = 0;
            var result = HttpHelper.Delete(VVRestApi.GlobalConfiguration.Routes.FoldersIdSecurityMembersId, "cascadeSecurityChanges=" + cascadeSecurityChanges.ToString().ToLower(), GetUrlParts(), this.ApiTokens, folderId, memberId);
            var data = result.GetValue("data") as JObject;
            if (data != null)
            {
                successCount = Convert.ToInt32(data.GetValue("successCount").Value<string>());
            }
            
            return successCount;
        }

        public void RemoveFolder(Guid folderId)
        {
            var result = HttpHelper.Delete(VVRestApi.GlobalConfiguration.Routes.FoldersId, "", GetUrlParts(), this.ApiTokens, folderId);
        }

        public Folder CreateUsersTopLevelContainerFolder()
        {
            dynamic postData = new ExpandoObject();

            return HttpHelper.Put<Folder>(GlobalConfiguration.Routes.FoldersHome, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
        }

        public Folder CreateUsersHomeFolder(Guid usId)
        {
            dynamic postData = new ExpandoObject();

            return HttpHelper.Put<Folder>(GlobalConfiguration.Routes.FoldersHomeId, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, usId);
        }

        public Folder GetUserHomeFolder(RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.Get<Folder>(VVRestApi.GlobalConfiguration.Routes.FoldersHome, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }
    }
}
