using System;
using VVRestApi.Common;

namespace VVRestApi.Vault.Library
{
    public class SecurityMember : RestObject
    {

        public SecurityMember()
        {
            this.ParentID = Guid.Empty;
            this.MemberID = Guid.Empty;
            this.MemberName = string.Empty;
            this.MemberType = MemberType.User;
            this.MemberRole = RoleType.None;
        }

        public Guid Id { get; set; }

        /// <summary>
        ///     ID of the object (Site, Document or Folder) that a User has a Security Role to
        /// </summary>
        /// <value><see cref="System.Guid" />	(System.Guid)</value>
        /// <remarks>
        /// </remarks>
        public Guid ParentID { get; set; }


        /// <summary>
        ///     ID of the User or Group with the Security Role
        /// </summary>
        /// <value><see cref="System.Guid" />	(System.Guid)</value>
        /// <remarks>
        /// </remarks>
        public System.Guid MemberID { get; set; }


        /// <summary>
        ///     Name of the User or Group with the Security Role.
        /// </summary>
        /// <value><see cref="System.Guid" />	(System.Guid)</value>
        /// <remarks>
        /// </remarks>
        public string MemberName { get; set; }

        /// <summary>
        ///     User or Group Member Type of the Security Member
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// </remarks>
        public MemberType MemberType { get; set; }

        /// <summary>
        ///     Security Role of the User or Group
        /// </summary>
        /// <value><see cref="RoleType" />	(Security.RoleType)</value>
        /// <remarks>
        /// </remarks>
        public RoleType MemberRole { get; set; }

        /// <summary>
        /// returns the string value of the Role Type enum
        /// </summary>
        public string MemberRoleValue { get; set; }

    }
}