using VVRestApi.Common;

namespace VVRestApi.Vault.Users
{
    public class DefaultCustomerInfo : RestObject
    {
        public string CustomerAlias { get; set; }
        public string DatabaseAlias { get; set; }
    }
}