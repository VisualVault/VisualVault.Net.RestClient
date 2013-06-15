// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VaultApi.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VVRestApi.Vault
{
    using VVRestApi.Common;
    using VVRestApi.Common.Logging;
    using VVRestApi.Common.Messaging;
    using VVRestApi.Vault.Forms;
    using VVRestApi.Vault.Groups;
    using VVRestApi.Vault.Library;
    using VVRestApi.Vault.Meta;
    using VVRestApi.Vault.Sites;
    using VVRestApi.Vault.Users;

    /// <summary>
    ///     Vaults are the specific customer database instances you work with.
    ///     Obtain a working Vault by logging in with a valid username and secret key along with the aliases of the customer and database you want to work with.
    /// </summary>
    public class VaultApi : BaseApi
    {
        #region Constructors and Destructors

        /// <summary>
        /// Creates a VaultApi with all of the properties initialized with the CurrentToken
        /// </summary>
        /// <param name="currentToken"></param>
        public VaultApi(SessionToken currentToken)
        {
            if (currentToken != null && (currentToken.IsValid() && currentToken.TokenType == TokenType.Vault))
            {
                this.CurrentToken = currentToken;

                this.REST = new RestManager(this);
                this.Sites = new SitesManager(this);
                this.CurrentUser = new CurrentUserManager(this);
                this.Users = new UsersManager(this);
                this.Groups = new GroupsManager(this);
                this.Folders = new FoldersManager(this);
                this.FormInstances = new FormInstancesManager(this);
                this.FormTemplates = new FormTemplatesManager(this);
                this.Meta = new MetaManager(this);
            }
            else
            {
                if (currentToken == null)
                {
                    LogEventManager.Error("No access token was return from the login event.");

                }
                else
                {
                    if (!currentToken.IsValid())
                    {
                        LogEventManager.Error("Current token is not valid.");
                    }
                    if (currentToken.TokenType != TokenType.Vault)
                    {
                        LogEventManager.Error("Ivalid token type. Current token type " + currentToken.TokenType);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Allows you to make authenticated REST API calls to the VisualVault server you are currently authenticated to.
        /// </summary>
        public RestManager REST { get; private set; }

        public SitesManager Sites { get; private set; }

        public CurrentUserManager CurrentUser { get; private set; }

        public UsersManager Users { get; private set; }

        public GroupsManager Groups { get; private set; }

        public FoldersManager Folders { get; private set; }

        public Forms.FormTemplatesManager FormTemplates { get; private set; }

        public Forms.FormInstancesManager FormInstances { get; private set; }

        /// <summary>
        /// Get additional information about VisualVault, such as the current version or the field names that you can query against.
        /// </summary>
        public Meta.MetaManager Meta { get; private set; }
    }
}