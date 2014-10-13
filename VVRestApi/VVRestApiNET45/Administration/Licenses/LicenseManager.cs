using VVRestApi.Common.Messaging;
using VVRestApi.Vault;

namespace VVRestApi.Administration.Licenses
{
    using Newtonsoft.Json.Linq;
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class LicenseManager : VVRestApi.Common.BaseApi
    {
        internal LicenseManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        /// <summary>
        /// Sets the current server license
        /// </summary>
        /// <param name="license"></param>
        public JObject SetServerLicense(string license)
        {
            return HttpHelper.Put(GlobalConfiguration.Routes.Licenses, string.Empty, this.GetUrlParts(), this.ApiTokens, new { license = license });
        }
    }
}