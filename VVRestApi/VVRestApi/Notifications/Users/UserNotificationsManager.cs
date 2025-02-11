
using System;
using VVRestApi.Common.Messaging;
using VVRestApi.Forms;

namespace VVRestApi.Notifications.Users
{
    public class UserNotificationsManager : NotificationsApi
    {
        protected UserNotificationsManager() { }

        public UserNotificationsManager(NotificationsApi api)
        {
            base.Populate(api);
        }

        public void ForceUIRefresh(Guid userGuid)
        {
            HttpHelper.PostNoCustomerAlias(GlobalConfiguration.RoutesNotificationApi.ForceUIRefresh, "", GetUrlParts(), ApiTokens, ClientSecrets, null, userGuid);
        }
    }
}
