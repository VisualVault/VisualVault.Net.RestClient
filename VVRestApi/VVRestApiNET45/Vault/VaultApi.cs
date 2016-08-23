// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VaultApi.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VVRestApi.Administration.Customers;
using VVRestApi.Common.Messaging;
using VVRestApi.Vault.Annotations;
using VVRestApi.Vault.CustomQueries;
using VVRestApi.Vault.DocumentViewer;
using VVRestApi.Vault.Version;

namespace VVRestApi.Vault
{
    using VVRestApi.Common;
    using VVRestApi.Vault.Forms;
    using VVRestApi.Vault.Groups;
    using VVRestApi.Vault.Library;
    using VVRestApi.Vault.Meta;
    using VVRestApi.Vault.Sites;
    using VVRestApi.Vault.Users;
    using VVRestApi.Vault.ScheduledProcess;

    /// <summary>
    ///     Vaults are the specific customer database instances you work with.
    ///     Obtain a working Vault by logging in with a valid username and secret key along with the aliases of the customer and database you want to work with.
    /// </summary>
    public class VaultApi : BaseApi
    {
        /// <summary>
        /// Creates a VaultApi helper object which will make HTTP API calls using the provided client application/developer credentials.
        /// (OAuth2 protocol Client Credentials Grant Type)
        /// </summary>
        public VaultApi(IClientSecrets clientSecrets)
        {
            this.ApiTokens = HttpHelper.GetAccessToken(clientSecrets.OAuthTokenEndPoint, clientSecrets.ApiKey, clientSecrets.ApiSecret).Result;

            if (!string.IsNullOrEmpty(this.ApiTokens.AccessToken))
            {
                this.ClientSecrets = clientSecrets;

                this.REST = new RestManager(this);
                this.DocumentViewer = new DocumentViewerManager(this);
                this.Sites = new SitesManager(this);
                this.ScheduledProcess = new ScheduledProcessManager(this);
                this.CurrentUser = new CurrentUserManager(this);
                this.Users = new UsersManager(this);
                this.Groups = new GroupsManager(this);
                this.Folders = new FoldersManager(this);
                this.Files = new FilesManager(this);
                this.FormInstances = new FormInstancesManager(this);
                this.FormTemplates = new FormTemplatesManager(this);
                this.Documents = new DocumentsManager(this);
                this.IndexFields = new IndexFieldManager(this);
                this.Files = new FilesManager(this);
                this.CustomQueryManager = new CustomQueryManager(this);
                this.DocumentShares = new DocumentShareManager(this);
                this.DocumentApprovals = new ApprovalRequestManager(this);

                this.Meta = new MetaManager(this);
                this.PersistedData = new PersistedData.PersistedDataManager(this);
                this.Customer = new CustomerManager(this);
                this.Version = new VersionManager(this);
                
            }
        }

        /// <summary>
        /// Creates a VaultApi helper object which will make HTTP API calls using the provided client application/developer credentials.
        /// (OAuth2 protocol Client Credentials Grant Type), allows a user to provide an existing token
        /// </summary>m>
        public VaultApi(IClientSecrets clientSecrets, Tokens tokens)
        {
            if (!string.IsNullOrEmpty(tokens.AccessToken))
            {
                this.ApiTokens = tokens;
                this.ClientSecrets = clientSecrets;

                this.REST = new RestManager(this);
                this.DocumentViewer = new DocumentViewerManager(this);
                this.Sites = new SitesManager(this);
                this.ScheduledProcess = new ScheduledProcessManager(this);
                this.CurrentUser = new CurrentUserManager(this);
                this.Users = new UsersManager(this);
                this.Groups = new GroupsManager(this);
                this.Folders = new FoldersManager(this);
                this.Files = new FilesManager(this);
                this.FormInstances = new FormInstancesManager(this);
                this.FormTemplates = new FormTemplatesManager(this);
                this.Documents = new DocumentsManager(this);
                this.IndexFields = new IndexFieldManager(this);
                this.Files = new FilesManager(this);
                this.CustomQueryManager = new CustomQueryManager(this);
                this.DocumentShares = new DocumentShareManager(this);
                this.DocumentApprovals = new ApprovalRequestManager(this);

                this.Meta = new MetaManager(this);
                this.PersistedData = new PersistedData.PersistedDataManager(this);
                this.Customer = new CustomerManager(this);
                this.Version = new VersionManager(this);
            }
        }

        /// <summary>
        /// Creates a VaultApi helper object which will make HTTP API calls using the provided client application/developer credentials AND 
        /// the provided UserName/Password credentials.  After authenticating the client application, the Resource Owner credentials are authenticated 
        /// and used for all subsequent HTTP API access
        /// (OAuth2 protocol Resource Owner Grant Type)
        /// </summary>
        /// <param name="clientSecrets"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public VaultApi(IClientSecrets clientSecrets, string userName, string password)
        {
            this.ApiTokens = HttpHelper.GetAccessToken(clientSecrets.OAuthTokenEndPoint, clientSecrets.ApiKey, clientSecrets.ApiSecret, userName, password).Result;

            if (!string.IsNullOrEmpty(this.ApiTokens.AccessToken))
            {
                this.ClientSecrets = clientSecrets;

                this.REST = new RestManager(this);
                this.DocumentViewer = new DocumentViewerManager(this);
                this.Sites = new SitesManager(this);
                this.ScheduledProcess = new ScheduledProcessManager(this);
                this.CurrentUser = new CurrentUserManager(this);
                this.Users = new UsersManager(this);
                this.Groups = new GroupsManager(this);
                this.Folders = new FoldersManager(this);
                this.Files = new FilesManager(this);
                this.FormInstances = new FormInstancesManager(this);
                this.FormTemplates = new FormTemplatesManager(this);
                this.Documents = new DocumentsManager(this);
                this.IndexFields = new IndexFieldManager(this);
                this.Files = new FilesManager(this);
                this.CustomQueryManager = new CustomQueryManager(this);
                this.DocumentShares = new DocumentShareManager(this);
                this.DocumentApprovals = new ApprovalRequestManager(this);

                this.Meta = new MetaManager(this);
                this.PersistedData = new PersistedData.PersistedDataManager(this);
                this.Customer = new CustomerManager(this);
                this.Version = new VersionManager(this);
            }
        }


        #region Properties

        /// <summary>
        /// the DocumentViewer Manager
        /// </summary>
        public DocumentViewer.DocumentViewerManager DocumentViewer { get; private set; }

        /// <summary>
        /// Allows you to make authenticated REST API calls to the VisualVault server you are currently authenticated to.
        /// </summary>
        public RestManager REST { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public CustomerManager Customer { get; set; }

        /// <summary>
        /// Allows you to make calls against the Sites endpoints
        /// </summary>
        public SitesManager Sites { get; private set; }

        /// <summary>
        /// Access current user information
        /// </summary>
        public CurrentUserManager CurrentUser { get; private set; }

        /// <summary>
        /// Retrieve and manage users
        /// </summary>
        public UsersManager Users { get; private set; }

        /// <summary>
        /// Retrieve and manage groups
        /// </summary>
        public GroupsManager Groups { get; private set; }

        /// <summary>
        /// Retrieve and manage folders and their contents
        /// </summary>
        public FoldersManager Folders { get; private set; }

        /// <summary>
        /// Retrieve and manage folders and their contents
        /// </summary>
        public FilesManager Files { get; private set; }

        /// <summary>
        /// Retrieve and manage form templates
        /// </summary>
        public Forms.FormTemplatesManager FormTemplates { get; private set; }

        /// <summary>
        /// Retrieve and manage form instances
        /// </summary>
        public Forms.FormInstancesManager FormInstances { get; private set; }

        /// <summary>
        /// Retrieve and manage Documents
        /// </summary>
        public Library.DocumentsManager Documents { get; private set; }

        /// <summary>
        /// Retrieve and manage IndexFields
        /// </summary>
        public Library.IndexFieldManager IndexFields { get; private set; }

        /// <summary>
        /// Retrieve and manage persisted data
        /// </summary>
        public PersistedData.PersistedDataManager PersistedData { get; private set; }

        /// <summary>
        /// Manage ScheduledProcesses
        /// </summary>
        public ScheduledProcessManager ScheduledProcess { get; private set; }

        /// <summary>
        /// Get additional information about VisualVault, such as the current version or the field names that you can query against.
        /// </summary>
        public Meta.MetaManager Meta { get; private set; }

        public CustomQueryManager CustomQueryManager { get; private set; }

        public DocumentShareManager DocumentShares { get; private set; }

        public ApprovalRequestManager DocumentApprovals { get; private set; }

        public VersionManager Version { get; private set; }

        #endregion

        /// <summary>
        /// Request a new AccessToken using the RefreshToken
        /// </summary>
        /// <returns></returns>
        public bool RefreshAccessToken()
        {
            this.ApiTokens = HttpHelper.RefreshAccessToken(this.ClientSecrets.OAuthTokenEndPoint, this.ClientSecrets.ApiKey, this.ClientSecrets.ApiSecret, this.ApiTokens.RefreshToken).Result;

            return this.ApiTokens.AccessTokenExpiration > DateTime.UtcNow;
        }

    }
}