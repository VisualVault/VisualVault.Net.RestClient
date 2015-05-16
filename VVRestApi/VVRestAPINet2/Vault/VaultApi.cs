
using VVRestAPINet2.Common;
using VVRestAPINet2.Common.Messaging;
using VVRestAPINet2.Vault.Library;

namespace VVRestAPINet2.Vault
{
    public class VaultApi : BaseApi
    {
        /// <summary>
        /// Creates a VaultApi helper object which will make HTTP API calls using the provided client application/developer credentials.
        /// (OAuth2 protocol Client Credentials Grant Type)
        /// </summary>
        public VaultApi(ClientSecrets clientSecrets)
        {
            this.ApiTokens = HttpHelper.GetAccessToken(clientSecrets.OAuthTokenEndPoint, clientSecrets.ApiKey, clientSecrets.ApiSecret);

            if (!string.IsNullOrEmpty(this.ApiTokens.AccessToken))
            {
                this.ClientSecrets = clientSecrets;

                this.Files = new FilesManager(this);
            }
        }

        /// <summary>
        /// Creates a VaultApi helper object which will make HTTP API calls using the provided client application/developer credentials.
        /// (OAuth2 protocol Client Credentials Grant Type)
        /// </summary>
        public VaultApi(ClientSecrets clientSecrets, string userName, string password)
        {
            this.ApiTokens = HttpHelper.GetAccessToken(clientSecrets.OAuthTokenEndPoint, clientSecrets.ApiKey, clientSecrets.ApiSecret, userName, password);

            if (!string.IsNullOrEmpty(this.ApiTokens.AccessToken))
            {
                this.ClientSecrets = clientSecrets;

                this.Files = new FilesManager(this);
            }
        }

        #region Properties

        /// <summary>
        /// Retrieve and manage folders and their contents
        /// </summary>
        public FilesManager Files { get; private set; }

        #endregion
    }
}
