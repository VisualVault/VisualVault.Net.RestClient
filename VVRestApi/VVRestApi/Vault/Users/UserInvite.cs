using System;
using VVRestApi.Common;

namespace VVRestApi.Vault.Users
{
    /// <summary>
    /// UserInvite contains properties such as emailaddress, firt name, last name current state of the invite and a date when invite was accepted of cancelled
    /// </summary>
    public class UserInvite : RestObject
    {
        public int Id { get; set; }
        public Guid AuthKey { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserInviteState InviteState { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CustomerDatabaseId { get; set; }
        public DateTime ActionDate { get; set; }
        public Guid CreateById { get; set; }
        public DateTime CreateDate { get; set; }
    }
}