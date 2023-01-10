using NUnit.Framework;
using System;
using System.Collections.Generic;
using VVRestApi.Common;
using VVRestApi.Vault;
using VVRestApi.Vault.Library;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class DocumentTests : TestBase
    {
        [Test]
        public void NewDocument()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var generalFolder = vaultApi.Folders.GetFolderByPath(GeneralFolderDefaultName);
            if (generalFolder != null)
            {
                var document = vaultApi.Documents.CreateDocument(generalFolder.Id, "SixthNewDocument", "Sixth New Document in Arbys", "1", DocumentState.Released);

                Assert.IsNotNull(document);
            }

        }

        [Test]
        public void GetDocument()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var document = vaultApi.Documents.GetDocument(dlId);

            Assert.IsNotNull(document);
        }

        [Test]
        public void GetDocumentBySearch()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            RequestOptions options = new RequestOptions
            {
                Query = "[FolderPath] like '/Sandbox/NBNCo Legal/Invoice Process/%'",
                Fields = "DocumentId,FolderPath,CreatedBy,Account,Approval Status,GC,Invoice Amount,Invoice Date,Invoice Number,Line,Matter Description,Matter Number,PO Number,Supplier"
            };

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documents = vaultApi.Documents.GetDocumentsBySearch(options);

            foreach (Document document in documents)
            {
                var fields = document.IndexFields;
            }

            Assert.IsNotNull(documents);
        }

        [Test]
        public void GetDocumentBySearchIncludeIndexFields()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var options = new RequestOptions();//c9b9db43-5bcf-e411-8281-14feb5f06078

            options.Query = "[Animals Two] eq 'Monkey' OR LEN([Cats]) > 8";
            //options.Query = "[Name Field] eq 'Receipt'";
            //options.Query = "LEN([Cats]) = 9 AND [Cats] = 'Chartreux'";
            //options.Query = "LEN('Chartreux') = 9 AND [Cats] = 'Chartreux'";
            //options.Expand = true;
            //options.Query = "name eq 'Arbys-00074'";
            options.Fields = "Id,DocumentId,Name,Description,ReleaseState,Text1,Cats";

            //options.Query = "[Name Field] eq 'Receipt'";
            //options.Fields = "Id,DocumentId,Name,Description,ReleaseState,Name Field,Start Date";

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var document = vaultApi.Documents.GetDocumentsBySearch(options);

            Assert.IsNotNull(document);
        }

        [Test]
        public void GetDocumentRevisions()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            //var documentRevisionList = vaultApi.Documents.GetDocumentRevisions(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documentRevisionList = vaultApi.Documents.GetDocumentRevisions(dlId);

            Assert.IsNotNull(documentRevisionList);
        }

        [Test]
        public void GetDocumentRevision()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");
            var dhId = new Guid("F74A31AF-AAAC-E411-8279-14FEB5F06078");

            //var documentRevision = vaultApi.Documents.GetDocumentRevision(dlId, dhId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documentRevision = vaultApi.Documents.GetDocumentRevision(dlId, dhId);

            Assert.IsNotNull(documentRevision);
        }

        [Test]
        public void CreateNewDocumentWithZeroByteFile()
        {

            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);


            var folder = vaultApi.Folders.GetFolderByPath("/Arbys");

            Assert.NotNull(folder);

            if (folder != null)
            {
                //var indexFields = new List<KeyValuePair<string, string>>();
                var indexFields = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Name", "Leather Hat")
                };


                var document = vaultApi.Documents.CreateDocumentWithEmptyFile(folder.Id, "12345.txt", "Zero Byte Upload", "1", "12345.txt", 55125, DocumentState.Released, indexFields);

                Assert.IsNotNull(document);

                //var fileArray = new byte[0];

                //var returnObject = vaultApi.Files.UploadZeroByteFile(document.DocumentId, "12345.txt", 5, fileArray);

            }
        }

        [Test]
        public void UpdateDocumentMetadata()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);


            var document = vaultApi.Documents.UpdateDocumentMetadata(new Guid(), "1234", "displayRevValue", "descriptionValue", "keywordsValue", "commentsValue", ArchiveType.Active, DocumentState.Released);

            Assert.IsNotNull(document);
        }

    }
}
