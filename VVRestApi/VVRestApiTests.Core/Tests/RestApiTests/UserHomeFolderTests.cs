using NUnit.Framework;
using System;
using VVRestApi.Vault;
using VVRestApi.Vault.Library;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class UserHomeFolderTests : TestBase
    {

        [Test]
        public void CreateUsersTopLevelContainerFolder()
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


            var folder = vaultApi.Folders.CreateUsersTopLevelContainerFolder();
            Assert.IsNotNull(folder);
        }

        [Test]
        public void CreateUsersHomeFolder()
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

            var folder = vaultApi.Folders.CreateUsersHomeFolder(new Guid(_ResourceOwnerUsID));
            Assert.IsNotNull(folder);
        }

        [Test]
        public void GetUsersHomeFolder()
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

            var folder = vaultApi.Folders.GetUserHomeFolder();
            Assert.IsNotNull(folder);
        }

        [Test]
        public void GetSpecificUserHomeFolder()
        {
            var vaultApi = new VaultApi(this);
            Assert.IsNotNull(vaultApi);

            Guid usId = Guid.Parse("c0f354e7-1b3a-e911-adec-d8f2ca59d73c");

            var folder = vaultApi.Folders.GetSpecificUserHomeFolder(usId);
            Assert.IsNotNull(folder);
        }

    }
}
