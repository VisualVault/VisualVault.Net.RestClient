using System;
using VVRestApi.Common;

namespace VVRestApi.Vault.Users
{
    public class NotifyUser : RestObject
    {
        public Guid UsId { get; set; }
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
    }
}