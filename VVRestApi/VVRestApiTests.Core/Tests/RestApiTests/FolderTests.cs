using NUnit.Framework;
using System;
using System.Collections.Generic;
using VVRestApi.Common;
using VVRestApi.Vault;
using VVRestApi.Vault.Library;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class FolderTests : TestBase
    {
        [Test]
        public void GetFolderDocuments()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var arbysFolder = vaultApi.Folders.GetFolderByPath("/Arbys");
            if (arbysFolder != null)
            {
                var documentList = vaultApi.Folders.GetFolderDocuments(arbysFolder.Id, new RequestOptions() { Skip = 0, Take = 10 });

                Assert.IsNotNull(documentList);
            }

        }

        [Test]
        public void GetFolderDocumentsWithSort()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var arbysFolder = vaultApi.Folders.GetFolderByPath("/Arbys");
            if (arbysFolder != null)
            {
                var documentList = vaultApi.Folders.GetFolderDocuments(arbysFolder.Id, "createdate", "desc", new RequestOptions() { Skip = 10, Take = 5 });

                Assert.IsNotNull(documentList);
            }

        }

        [Test]
        public void GetChildFolders()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var arbysFolder = vaultApi.Folders.GetFolderByPath("/Arbys");
            Assert.IsNotNull(arbysFolder);
            if (arbysFolder != null)
            {
                List<Folder> childFolderListList = vaultApi.Folders.GetChildFolders(arbysFolder.Id);

                Assert.IsNotEmpty(childFolderListList);
            }
        }

        [Test]
        public void GetTopLevelFolders()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var topLevelFolders = vaultApi.Folders.GetTopLevelFolders();
            Assert.IsNotEmpty(topLevelFolders);
        }

        [Test]
        public void CreateTopLevelFolder()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var folderName = "Irish Marshes";
            var folderDescription = "The Green Irish Marshes";
            var allowRevisions = true;
            var namingConventionPrefix = "IrishMarshes-";
            var namingConventionSufix = " - IM";
            var datePosition = DocDatePosition.NoDateInsert;
            var docSeqType = VVRestApi.Vault.Library.DocSeqType.TypeInteger;
            var expireAction = ExpireAction.Nothing;
            var expireRequired = false;
            var expirationDays = 0;
            var reviewRequired = false;
            var reviewDays = 0;


            var folder = vaultApi.Folders.CreateTopLevelFolder(folderName, folderDescription, allowRevisions, namingConventionPrefix, namingConventionSufix, datePosition, docSeqType, expireAction, expireRequired, expirationDays, reviewRequired, reviewDays);
            Assert.IsNotNull(folder);

        }

        [Test]
        public void CreateChildFolderWithNameOnly()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var parentFolder = vaultApi.Folders.GetFolderByPath("/General");
            if (parentFolder != null)
            {
                var folder = vaultApi.Folders.CreateChildFolder(parentFolder.Id, "SampleChildFolder2");

                Assert.IsNotNull(folder);
            }
        }

        [Test]
        public void CreateChildFolder()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var parentFolder = vaultApi.Folders.GetFolderByPath("/General");
            if (parentFolder != null)
            {
                var namingConventionPrefix = "ChilderFolder1-";
                var namingConventionSufix = "";
                var datePosition = DocDatePosition.NoDateInsert;
                var docSeqType = VVRestApi.Vault.Library.DocSeqType.TypeInteger;
                var expireAction = ExpireAction.Nothing;
                var expireRequired = false;
                var expirationDays = 0;
                var reviewRequired = false;
                var reviewDays = 0;

                var folder = vaultApi.Folders.CreateChildFolder(parentFolder.Id, "ChildFolder1", "ChildFolder1", false, false, true, namingConventionPrefix, namingConventionSufix, datePosition, docSeqType, expireAction, expireRequired, expirationDays, reviewRequired, reviewDays);


                Assert.IsNotNull(folder);
            }
        }

        [Test]
        public void CreateFolderByPath()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var folderPath = "/Community/Airplanes/Lockheed";

            var folderDescription = "Lockheed Folder";
            var allowRevisions = true;
            var namingConventionPrefix = "Lockheed-";
            var namingConventionSufix = "";
            var datePosition = DocDatePosition.NoDateInsert;
            var docSeqType = VVRestApi.Vault.Library.DocSeqType.TypeInteger;
            var expireAction = ExpireAction.Nothing;
            var expireRequired = false;
            var expirationDays = 0;
            var reviewRequired = false;
            var reviewDays = 0;


            var folder = vaultApi.Folders.CreateFolderByPath(folderPath, folderDescription, false, false, allowRevisions, namingConventionPrefix, namingConventionSufix, datePosition, docSeqType, expireAction, expireRequired, expirationDays, reviewRequired, reviewDays);
            Assert.IsNotNull(folder);

        }

        [Test]
        public void GetFolderById()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var arbysFolder = vaultApi.Folders.GetFolderByPath("/General");
            if (arbysFolder != null)
            {
                var folderById = vaultApi.Folders.GetFolderByFolderId(arbysFolder.Id);

                Assert.IsNotNull(folderById);
            }

        }

        [Test]
        public void RemoveFolderById()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var folderId = new Guid("f37fa084-0b5b-e511-82a6-5cf3706c36ed");
            vaultApi.Folders.RemoveFolder(folderId);


            //var testFolder = vaultApi.Folders.GetFolderByPath("/Colors5");
            //if (testFolder != null)
            //{
            //    var folderId = new Guid("f37fa084-0b5b-e511-82a6-5cf3706c36ed");
            //    vaultApi.Folders.RemoveFolder(testFolder.Id);
            //}
        }

        [Test]
        public void GetFolderSecurityMembers()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var generalFolder = vaultApi.Folders.GetFolderByPath("/General");

            Assert.AreNotEqual(Guid.Empty, generalFolder.Id);

            if (generalFolder != null)
            {
                var securityMembers = vaultApi.Folders.GetFolderSecurityMembers(generalFolder.Id);

                Assert.IsNotEmpty(securityMembers);
            }
        }

        [Test]
        public void SetSecurityMembersOnFolder()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var generalFolder = vaultApi.Folders.GetFolderByPath("/General");

            Assert.AreNotEqual(Guid.Empty, generalFolder.Id);

            if (generalFolder != null)
            {
                var securityActionList = new List<SecurityMemberApplyAction>();
                securityActionList.Add(new SecurityMemberApplyAction
                {
                    Action = SecurityAction.Add,
                    MemberId = new Guid("069493C9-36AD-E211-9D53-14FEB5F06078"),
                    MemberType = MemberType.Group,
                    RoleType = RoleType.Viewer
                });
                securityActionList.Add(new SecurityMemberApplyAction
                {
                    Action = SecurityAction.Add,
                    MemberId = new Guid("B4384FBA-40AD-E211-9D53-14FEB5F06078"),
                    MemberType = MemberType.User,
                    RoleType = RoleType.Owner
                });

                var updateCount = vaultApi.Folders.UpdateSecurityMembers(generalFolder.Id, securityActionList, true);

                Assert.AreEqual(2, updateCount);
            }
        }

        [Test]
        public void AddSecurityMemberOnFolder()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var generalFolder = vaultApi.Folders.GetFolderByPath("/General");

            Assert.AreNotEqual(Guid.Empty, generalFolder.Id);

            if (generalFolder != null)
            {
                var memberId = new Guid("22BF8B4C-A961-E111-8E23-14FEB5F06078");
                var memberType = MemberType.Group;
                var role = RoleType.Editor;

                var updateCount = vaultApi.Folders.AddSecurityMember(generalFolder.Id, memberId, memberType, role);

                Assert.AreEqual(1, updateCount);
            }
        }

        [Test]
        public void RemoveSecurityMemberOnFolder()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var generalFolder = vaultApi.Folders.GetFolderByPath("/General");

            Assert.AreNotEqual(Guid.Empty, generalFolder.Id);

            if (generalFolder != null)
            {
                var memberId = new Guid("22BF8B4C-A961-E111-8E23-14FEB5F06078");

                var updateCount = vaultApi.Folders.RemoveSecurityMember(generalFolder.Id, memberId);

                Assert.AreEqual(1, updateCount);
            }
        }
    }
}
