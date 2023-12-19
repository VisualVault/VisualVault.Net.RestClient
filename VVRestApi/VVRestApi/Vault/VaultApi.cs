// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VaultApi.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using VVRestApi.Administration.Customers;
using VVRestApi.Common.Messaging;
using VVRestApi.Vault.Configuration;
using VVRestApi.Vault.CustomQueries;
using VVRestApi.Vault.DocumentViewer;
using VVRestApi.Vault.Email;
using VVRestApi.Vault.Scripts;
using VVRestApi.Vault.Version;

namespace VVRestApi.Vault
{
    using VVRestApi.Common;
    using VVRestApi.Documents;
    using VVRestApi.Forms;
    using VVRestApi.Studio;
    using VVRestApi.Vault.Forms;
    using VVRestApi.Vault.Groups;
    using VVRestApi.Vault.Library;
    using VVRestApi.Vault.Meta;
    using VVRestApi.Vault.ScheduledProcess;
    using VVRestApi.Vault.Sites;
    using VVRestApi.Vault.Users;

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
                this.CustomerDatabase = new CustomerDatabaseManager(this);
                this.Version = new VersionManager(this);
                this.Emails = new EmailManager(this);
                this.Scripts = new ScriptsManager(this);

                this.ConfigurationManager = new ConfigurationManager(this);

                // get jwt using api token
                var jwt = HttpHelper.ConvertToJWT(GetUrlParts(), clientSecrets, ApiTokens);
                // these classes will potentially have a different token from the above
                if (!string.IsNullOrEmpty(jwt.AccessToken))
                {
                    this.FormsApi = new FormsApi(this, jwt);
                    this.DocApi = new DocApi(this, jwt);
                    this.StudioApi = new StudioApi(this, jwt);
                }

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
                this.CustomerDatabase = new CustomerDatabaseManager(this);
                this.Version = new VersionManager(this);
                this.Emails = new EmailManager(this);
                this.Scripts = new ScriptsManager(this);
                this.ConfigurationManager = new ConfigurationManager(this);

                //make sure token is a jwt
                var jwt = tokens;
                if (!tokens.IsJwt)
                {
                    // get jwt using api token
                    jwt = HttpHelper.ConvertToJWT(GetUrlParts(), clientSecrets, ApiTokens);
                }

                // these classes will potentially have a different token from the above
                if (!string.IsNullOrEmpty(jwt.AccessToken))
                {
                    this.FormsApi = new FormsApi(this, jwt);
                    this.DocApi = new DocApi(this, jwt);
                    this.StudioApi = new StudioApi(this, jwt);
                }
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
                this.CustomerDatabase = new CustomerDatabaseManager(this);
                this.Version = new VersionManager(this);
                this.Emails = new EmailManager(this);
                this.Scripts = new ScriptsManager(this);
                this.ConfigurationManager = new ConfigurationManager(this);

                // get jwt using api token
                var jwt = HttpHelper.ConvertToJWT(GetUrlParts(), clientSecrets, ApiTokens);
                // these classes will potentially have a different token from the above
                if (!string.IsNullOrEmpty(jwt.AccessToken))
                {
                    this.FormsApi = new FormsApi(this, jwt);
                    this.DocApi = new DocApi(this, jwt);
                    this.StudioApi = new StudioApi(this, jwt);
                }
            }
        }

        /// <summary>
        /// Creates a VaultApi helper object which will make HTTP API calls using a passed in Json Web Token (JWT).
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public VaultApi(IClientSecrets clientSecrets, string jwt)
        {
            //Likely only need to call the GetJWT method when trying to refresh the token
            this.ApiTokens = HttpHelper.GetJWT(clientSecrets.OAuthTokenEndPoint, jwt).Result;

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
                this.CustomerDatabase = new CustomerDatabaseManager(this);
                this.Version = new VersionManager(this);
                this.Emails = new EmailManager(this);
                this.Scripts = new ScriptsManager(this);
                this.ConfigurationManager = new ConfigurationManager(this);

                this.FormsApi = new FormsApi(this, ApiTokens);
                this.DocApi = new DocApi(this, ApiTokens);
                this.StudioApi = new StudioApi(this, ApiTokens);
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
        /// Allows you to make calls against the CustomerDatabase endpoints
        /// </summary>
        public CustomerDatabaseManager CustomerDatabase { get; set; }

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

        public EmailManager Emails { get; private set; }

        public ConfigurationManager ConfigurationManager { get; private set; }

        public ScriptsManager Scripts { get; private set; }

        private FormsApi _formsApi;
        public FormsApi FormsApi
        {
            get
            {
                // throw error if formsApi is not enabled
                if (!_formsApi.IsEnabled || string.IsNullOrWhiteSpace(_formsApi.BaseUrl))
                    throw new InvalidOperationException("Forms API is not configured for this instance");

                return _formsApi;
            }
            set
            {
                _formsApi = value;
            }
        }

        private DocApi _docApi;
        public DocApi DocApi
        {
            get
            {
                // throw error if docApi is not enabled
                if (!_docApi.IsEnabled || string.IsNullOrWhiteSpace(_docApi.BaseUrl))
                    throw new InvalidOperationException("Doc API is not configured for this instance");

                return _docApi;
            }
            set
            {
                _docApi = value;
            }
        }

        private StudioApi _studioApi;
        public StudioApi StudioApi
        {
            get
            {
                // throw error if docApi is not enabled
                if (!_studioApi.IsEnabled || string.IsNullOrWhiteSpace(_studioApi.BaseUrl))
                    throw new InvalidOperationException("VV Studio is not configured for this instance");

                return _studioApi;
            }
            set
            {
                _studioApi = value;
            }
        }

        #endregion

        /// <summary>
        /// Request a new AccessToken using the RefreshToken
        /// </summary>
        /// <returns></returns>
        public bool RefreshAccessToken()
        {
            this.ApiTokens = HttpHelper.RefreshToken(this.ApiTokens, this.ClientSecrets).Result;

            return this.ApiTokens.AccessTokenExpiration > DateTime.UtcNow;
        }
    }
}