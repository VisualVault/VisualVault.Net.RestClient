using NUnit.Framework;
using System;
using System.Collections.Generic;
using VVRestApi.Common;
using VVRestApi.Vault;
using VVRestApi.Vault.Users;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class UserAndGroupTests : TestBase
    {
        [Test]
        public void GetHomeSiteTest()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var site = vaultApi.Sites.GetSite("Home", null);

            Assert.IsNotNull(site);

            var homeSite = vaultApi.Sites.GetSites(new RequestOptions() { Query = string.Format("StId eq '{0}'", site.Id) });

            Assert.IsNotNull(homeSite);
        }

        [Test]
        public void CreateUser()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var siteId = new Guid("7AD2F88A-8861-E111-8E23-14FEB5F06078");
            var userId = "firstperson.namefirstperson.name@companyxyz.com";
            var firstName = "Gatry";
            var middleInitial = "";
            var lastName = "Samual";
            var emailAddress = "firstperson.name@companyxyz.com";
            var password = "Sample12345";
            //var passwordNeverExpires = true;
            //var passwordExpiresDate = new DateTime(2017, 1, 1);

            //var user = vaultApi.Users.CreateUser(siteId, userId, firstName, middleInitial, lastName, emailAddress, password, passwordNeverExpires, passwordExpiresDate);
            var user = vaultApi.Users.CreateUser(siteId, userId, firstName, middleInitial, lastName, emailAddress, password);

            Assert.IsNotNull(user);
        }

        [Test]
        public void GetUsersTest()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            Page<User> users = vaultApi.Users.GetUsers();

            Assert.IsNotNull(users);
        }

        [Test]
        public void GetGroupMembers()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var groupId = new Guid("1249586F-A961-E111-8E23-14FEB5F06078");

            var groupMembers = vaultApi.Groups.GetGroupMembers(groupId);

            Assert.IsNotEmpty(groupMembers);
        }

        [Test]
        public void GetAccountOwner()
        {
            var vaultApi = new VaultApi(this);
            Assert.IsNotNull(vaultApi);

            User user = vaultApi.Users.GetAccountOwner();
            Assert.IsNotNull(user);

        }

        [Test]
        public void CreateGroup()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var siteId = new Guid("23EE59B9-0599-E511-AB25-5CF3706C36ED");
            var groupName = "The Tiger Claw";
            var description = "New Group Description";

            var group = vaultApi.Groups.CreateGroup(siteId, groupName, description);

            Assert.IsNotNull(group);
        }

        [Test]
        public void UpdateGroupDescription()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var groupId = new Guid("B0B428D4-4183-E511-82B0-5CF3706C36ED");
            var newDescription = "New Group Description";
            var group = vaultApi.Groups.UpdateGroupDescription(groupId, newDescription);

            Assert.IsNotNull(group);
        }


        [Test]
        public void AddGroupMember()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var groupId = new Guid("1249586F-A961-E111-8E23-14FEB5F06078");
            var userId = new Guid("92D49919-BBD1-E411-8281-14FEB5F06078");
            var groupMembers = vaultApi.Groups.AddUserToGroup(groupId, userId);

            Assert.IsNotEmpty(groupMembers);
        }

        [Test]
        public void AddGroupMembers()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var groupId = new Guid("1249586F-A961-E111-8E23-14FEB5F06078");

            var userId1 = new Guid("F3659213-BBD1-E411-8281-14FEB5F06078");
            var userId2 = new Guid("EB659213-BBD1-E411-8281-14FEB5F06078");
            var userId3 = new Guid("92D49919-BBD1-E411-8281-14FEB5F06078");

            var userList = new List<Guid>
            {
                userId1,
                userId2,
                userId3
            };

            var groupMembers = vaultApi.Groups.AddUserToGroup(groupId, userList);

            Assert.IsNotEmpty(groupMembers);
        }

        //CA8A6D05-C78C-E211-A797-14FEB5F06078

        [Test]
        public void RemoveGroupMemberByName()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var groupId = new Guid("22BF8B4C-A961-E111-8E23-14FEB5F06078");
            var memberName = "User.wp";

            vaultApi.Groups.RemoveGroupMember(groupId, memberName);
        }

        [Test]
        public void RemoveGroupMemberById()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var groupId = new Guid("22BF8B4C-A961-E111-8E23-14FEB5F06078");
            var memberId = new Guid("CA8A6D05-C78C-E211-A797-14FEB5F06078");

            vaultApi.Groups.RemoveGroupMember(groupId, memberId);


        }

        [Test]
        public void SetUserAsAccountOwner()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var usId = new Guid("46E2A31F-49AD-E211-9D53-14FEB5F06078");

            var user = vaultApi.Users.SetUserAsAccountOwner(usId);

            Assert.IsNotNull(user);
        }


        [Test]
        public void RemoveUserFromAccountOwner()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var usId = new Guid("46E2A31F-49AD-E211-9D53-14FEB5F06078");

            var user = vaultApi.Users.RemoveUserFromAccountOwner(usId);

            Assert.IsNotNull(user);
        }

        [Test]
        public void ChangeUserPassword()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var usId = new Guid("46E2A31F-49AD-E211-9D53-14FEB5F06078");

            var user = vaultApi.Users.ChangeUserPassword(usId, "p", "walton1");

            Assert.IsNotNull(user);
        }

        [Test]
        public void ChangeUseFirstNameAndLastName()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var usId = new Guid("46E2A31F-49AD-E211-9D53-14FEB5F06078");

            var user = vaultApi.Users.ChangeUserFirstNameAndLastName(usId, "Jimmy John", "Campbell");

            Assert.IsNotNull(user);
        }
    }
}
