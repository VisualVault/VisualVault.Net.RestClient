using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Library
{
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class FoldersManager : BaseApi
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
            return HttpHelper.Get<Folder>(GlobalConfiguration.Routes.Folders, "folderPath=" + this.UrlEncode(folderPath), options, GetUrlParts(), this.ClientSecrets,this.ApiTokens);
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

            return HttpHelper.GetListResult<Folder>(GlobalConfiguration.Routes.Folders, "folderPath=" + this.UrlEncode("/"), options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
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

            return HttpHelper.GetListResult<Document>(GlobalConfiguration.Routes.FolderDocuments, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId);
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

            return HttpHelper.GetListResult<Document>(GlobalConfiguration.Routes.FolderDocuments, sb.ToString(), options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId);
        }

        public int GetFolderDocumentsCount(Guid folderId, bool includeSubfolders, RequestOptions options = null)
        {
            var count = -1;

            var queryString = "metaonly=true&includesubfolders=" + includeSubfolders;

            var result = HttpHelper.Get(GlobalConfiguration.Routes.FolderDocuments, queryString, options,
                GetUrlParts(), this.ApiTokens, this.ClientSecrets, folderId);

            return result.Value<int>("data");
        }

        public Folder GetFolderByFolderId(Guid folderId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.Get<Folder>(GlobalConfiguration.Routes.FoldersId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId);
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
            
            return HttpHelper.GetListResult<FolderIndexField>(GlobalConfiguration.Routes.FoldersIndexFields, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId);
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

            return HttpHelper.Get<FolderIndexField>(GlobalConfiguration.Routes.FoldersIdIndexFieldsId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId, fieldId);
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

            return HttpHelper.GetListResult<Folder>(GlobalConfiguration.Routes.FoldersIdFolders, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, parentFolderid);
        }

        /// <summary>
        /// Get the VV Express home folder for a specific user.
        /// </summary>
        /// <param name="usId"></param>
        /// <returns></returns>
        public Folder GetSpecificUserHomeFolder(Guid usId)
        {
            return HttpHelper.Get<Folder>(GlobalConfiguration.Routes.FoldersHomeId, "", null, GetUrlParts(), this.ClientSecrets, this.ApiTokens, usId);
        }

        public Folder CopyFolder(Guid sourceFolderId, Guid targetFolderId)
        {
            if (sourceFolderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("sourceFolderId is required but was an empty Guid", "sourceFolderId");
            }

            if (targetFolderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("targetFolderId is required but was an empty Guid", "targetFolderId");
            }

            dynamic postData = new ExpandoObject();
            postData.sourceFolderId = sourceFolderId;
            postData.targetFolderId = targetFolderId;

            return HttpHelper.Post<Folder>(GlobalConfiguration.Routes.FoldersCopy, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
        }

        public Folder CopyFolder(string sourceFolderPath, string targetFolderPath)
        {
            if (string.IsNullOrWhiteSpace(sourceFolderPath))
            {
                throw new ArgumentException("sourceFolderPath is required but was an empty string", "sourceFolderPath");
            }

            if (string.IsNullOrWhiteSpace(targetFolderPath))
            {
                throw new ArgumentException("targetFolderPath is required but was an empty string", "targetFolderPath");
            }

            dynamic postData = new ExpandoObject();
            postData.sourceFolderPath = sourceFolderPath;
            postData.targetFolderPath = targetFolderPath;

            return HttpHelper.Post<Folder>(GlobalConfiguration.Routes.FoldersCopy, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
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

        public Folder MoveFolder(Guid sourceFolderId, Guid targetFolderId)
        {
            if (sourceFolderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("sourceFolderId is required but was an empty Guid", "sourceFolderId");
            }

            if (targetFolderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("targetFolderId is required but was an empty Guid", "targetFolderId");
            }

            dynamic postData = new ExpandoObject();
            postData.sourceFolderId = sourceFolderId;
            postData.targetFolderId = targetFolderId;

            return HttpHelper.Put<Folder>(GlobalConfiguration.Routes.FoldersMove, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
        }

        public Folder MoveFolder(string sourceFolderPath, string targetFolderPath)
        {
            if (string.IsNullOrWhiteSpace(sourceFolderPath))
            {
                throw new ArgumentException("sourceFolderPath is required but was an empty string", "sourceFolderPath");
            }

            if (string.IsNullOrWhiteSpace(targetFolderPath))
            {
                throw new ArgumentException("targetFolderPath is required but was an empty string", "targetFolderPath");
            }

            dynamic postData = new ExpandoObject();
            postData.sourceFolderPath = sourceFolderPath;
            postData.targetFolderPath = targetFolderPath;

            return HttpHelper.Put<Folder>(GlobalConfiguration.Routes.FoldersMove, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
        }

        public FolderIndexField UpdateFolderIndexFieldToNotOverride(Guid folderId, Guid fieldId)
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

            postData.overriden = "false";

            return HttpHelper.Put<FolderIndexField>(GlobalConfiguration.Routes.FoldersIdIndexFieldsId, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, folderId, fieldId);
        }

        public FolderIndexField UpdateFolderIndexFieldOverrideSettings(Guid folderId, Guid fieldId, Guid queryId, string displayField, string valueField, Guid dropDownListId, bool required, string defaultValue)
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
            
            return HttpHelper.GetListResult<IndexFieldSelectOption>(GlobalConfiguration.Routes.FoldersIdIndexFieldsIdSelectOptions, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId, fieldId);
        }
        
        public List<SecurityMember> GetFolderSecurityMembers(Guid folderId, RequestOptions options = null)
        {
            if (!string.IsNullOrWhiteSpace(options?.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.GetListResult<SecurityMember>(GlobalConfiguration.Routes.FoldersIdSecurityMembers, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, folderId);
        }

        public int UpdateSecurityMembers(Guid folderId, List<SecurityMemberApplyAction> securityActionList, bool cascadeSecurityChanges = false)
        {
            var successCount = 0;

            dynamic postData = new ExpandoObject();
            postData.securityActions = securityActionList;
            postData.cascadeSecurityChanges = cascadeSecurityChanges;

            var result = HttpHelper.Put(GlobalConfiguration.Routes.FoldersIdSecurityMembers, "", GetUrlParts(), this.ApiTokens, this.ClientSecrets, postData, folderId);
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

            var result = HttpHelper.Put(GlobalConfiguration.Routes.FoldersIdSecurityMembersId, "", GetUrlParts(), this.ApiTokens, this.ClientSecrets, postData, folderId, memberId);
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
            var result = HttpHelper.Delete(GlobalConfiguration.Routes.FoldersIdSecurityMembersId, "cascadeSecurityChanges=" + cascadeSecurityChanges.ToString().ToLower(), GetUrlParts(), this.ApiTokens, this.ClientSecrets, folderId, memberId);
            var data = result.GetValue("data") as JObject;
            if (data != null)
            {
                successCount = Convert.ToInt32(data.GetValue("successCount").Value<string>());
            }
            
            return successCount;
        }

        public void RemoveFolder(Guid folderId)
        {
            var result = HttpHelper.Delete(GlobalConfiguration.Routes.FoldersId, "", GetUrlParts(), this.ApiTokens, this.ClientSecrets, folderId);
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

            return HttpHelper.Get<Folder>(GlobalConfiguration.Routes.FoldersHome, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        public Folder UpdateFolder(Guid folderId, string name, string description)
        {
            if (folderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("FolderId is required but was an empty Guid", "folderId");
            }

            dynamic postData = new ExpandoObject();

            postData.name = name;
            postData.description = description;

            return HttpHelper.Put<Folder>(GlobalConfiguration.Routes.FoldersId, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, folderId);
        }
    }
}