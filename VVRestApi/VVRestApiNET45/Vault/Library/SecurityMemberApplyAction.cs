using System;

namespace VVRestApi.Vault.Library
{
    public class SecurityMemberApplyAction
    {

        public RoleType RoleType { get; set; }

        public Guid MemberId { get; set; }

        public MemberType MemberType { get; set; }

        public SecurityAction Action { get; set; }
    }
}