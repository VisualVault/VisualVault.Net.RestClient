namespace VVRestApi.Administration.Licenses
{
    using System;

    using Newtonsoft.Json.Linq;

    using VVRestApi.Common;
    using VVRestApi.Common.Messaging;

    public class LicenseManager : VVRestApi.Common.BaseApi
    {
        internal LicenseManager(AdministrationApi api)
        {
            base.Populate(api.CurrentToken);
        }

        /// <summary>
        /// Sets the current server license
        /// </summary>
        /// <param name="license"></param>
        public JObject SetServerLicense(string license)
        {
            return HttpHelper.Put(GlobalConfiguration.Routes.Licenses, string.Empty, this.CurrentToken, new { license = license });
    
        }
    }
}