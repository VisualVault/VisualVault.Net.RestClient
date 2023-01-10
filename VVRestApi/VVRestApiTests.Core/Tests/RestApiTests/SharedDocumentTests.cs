using NUnit.Framework;
using System;
using System.Collections.Generic;
using VVRestApi.Vault;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class SharedDocumentTests : TestBase
    {

        [Test]
        public void GetDocumentsSharedWithMe()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var documentList = vaultApi.DocumentShares.GetDocumentsSharedWithMe();

            Assert.IsNotNull(documentList);
        }

        [Test]
        public void ShareDocumentWithUser()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            //var userWopUsId = new Guid("369493C9-36AD-E211-9D53-14FEB5F06078");
            var userWpUsId = new Guid("A6DFFCC3-35AD-E211-9D53-14FEB5F06078");

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documentShare = vaultApi.DocumentShares.ShareDocument(dlId, userWpUsId);

            Assert.IsNotNull(documentShare);
        }

        [Test]
        public void ShareDocumentWithUsers()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            var userWopUsId = new Guid("369493C9-36AD-E211-9D53-14FEB5F06078");
            var userWpUsId = new Guid("A6DFFCC3-35AD-E211-9D53-14FEB5F06078");
            var aceAdmin = new Guid("ABD2F88A-8861-E111-8E23-14FEB5F06078");

            var userList = new List<Guid>
            {
                userWopUsId,
                userWpUsId,
                aceAdmin
            };

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documentShare = vaultApi.DocumentShares.ShareDocument(dlId, userList);

            Assert.IsNotEmpty(documentShare);
        }

        [Test]
        public void RemoveUserFromDocumentShare()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            var userWopUsId = new Guid("369493C9-36AD-E211-9D53-14FEB5F06078");

            vaultApi.DocumentShares.RemoveUserFromSharedDocument(dlId, userWopUsId);

            //Assert.IsNotNull(document);
        }

        [Test]
        public void GetListOfDocumentSharesOfDocument()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documentList = vaultApi.DocumentShares.GetListOfUsersDocumentSharedWith(dlId);

            Assert.IsNotNull(documentList);
        }
    }
}
