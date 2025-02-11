using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Forms;
using VVRestApi.Forms.FormInstances;
using VVRestApi.Notifications.Users;
using VVRestApi.Vault;

namespace VVRestApi.Notifications
{
    /// <summary>
    /// Methods to make calls to the notifications api. Notifications must be enabled and a notification api url must be provided to enable this class
    /// If disabled usage will throw an error
    /// </summary>
    public class NotificationsApi : BaseApi
    {
        public bool IsEnabled { get; set; }
        public string BaseUrl { get; set; }

        internal NotificationsApi() { }


        /// <summary>
        /// Creates a NotificationsApi instance
        /// </summary>
        /// <remarks>If token is not a JWT, the NotificationsApi object will be disabled</remarks>
        /// <param name="api"></param>
        /// <param name="jwt">Must be a JWT</param>
        internal NotificationsApi(VaultApi api, Tokens jwt)
        {
            var notificationsApiConfig = api.ConfigurationManager.GetNotificationApiConfiguration();

            if (notificationsApiConfig == null || !jwt.IsJwt)
            {
                return;
            }

            IsEnabled = notificationsApiConfig.IsEnabled;
            BaseUrl = notificationsApiConfig.ApiUrl;

            base.Populate(api.ClientSecrets, jwt);

            UserNotifications = new UserNotificationsManager(this);
        }

        public UserNotificationsManager UserNotifications { get; private set; }

        /// <summary>
        /// Populates the token
        /// </summary>
        /// <param name="clientSecrets"></param>
        /// <param name="apiTokens"> </param>
        internal void Populate(NotificationsApi api)
        {
            this.ClientSecrets = api.ClientSecrets;
            this.ApiTokens = api.ApiTokens;
            this.BaseUrl = api.BaseUrl;
            this.IsEnabled = api.IsEnabled;
        }

        internal new UrlParts GetUrlParts()
        {
            UrlParts urlParts = new UrlParts
            {
                ApiVersion = ClientSecrets.ApiVersion,
                BaseUrl = BaseUrl,
                OAuthTokenEndPoint = ClientSecrets.OAuthTokenEndPoint
            };

            return urlParts;
        }
    }
}
