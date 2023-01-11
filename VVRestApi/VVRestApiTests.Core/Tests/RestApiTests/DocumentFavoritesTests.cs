using NUnit.Framework;
using System;
using VVRestApi.Vault;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class DocumentFavoritesTests : TestBase
    {

        [Test]
        public void GetDocumentFavorites()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            //var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documentList = vaultApi.Documents.GetDocumentFavorites();

            Assert.IsNotNull(documentList);
        }

        [Test]
        public void SetDocumentAsFavorites()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var document = vaultApi.Documents.SetDocumentAsFavorites(dlId);

            Assert.IsNotNull(document);
        }

        [Test]
        public void RemoveDocumentAsFavorites()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            vaultApi.Documents.RemoveDocumentAsFavorites(dlId);

            //Assert.IsNotNull(document);
        }
    }
}
