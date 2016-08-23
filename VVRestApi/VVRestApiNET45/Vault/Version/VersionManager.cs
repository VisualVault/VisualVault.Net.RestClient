using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Version
{
    public class VersionManager : BaseApi
    {
        internal VersionManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        public VaultVersionInfo GetVersionInfo(RequestOptions options = null)
        {
            return HttpHelper.Get<VaultVersionInfo>(VVRestApi.GlobalConfiguration.Routes.VersionGetVersion, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }
    }
}
