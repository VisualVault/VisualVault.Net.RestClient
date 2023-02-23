using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using VVRestApi.Common;
using VVRestApi.Vault;
using VVRestApi.Vault.Library;
using VVRestApiTests.TestHelpers;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class FileTests : TestBase
    {

        [Test]
        public void GetFileBySearch()
        {
            var readBytes = 0;

            try
            {
                var vaultApi = new VaultApi(this);

                Assert.IsNotNull(vaultApi);

                var options = new RequestOptions();

                options.Query = "[Animals Two] eq 'Monkey' OR LEN([Cats]) > 8";
                //options.Query = "[Name Field] eq 'Receipt'";
                //options.Query = "LEN([Cats]) = 9 AND [Cats] = 'Chartreux'";
                //options.Query = "LEN('Chartreux') = 9 AND [Cats] = 'Chartreux'";
                //options.Expand = true;
                //options.Query = "name eq 'Arbys-00074'";
                //options.Fields = "Id,DocumentId,Name,Description,ReleaseState";
                //options.Query = "[CheckOutBy] LIKE 'Ace%'";


                string filePath = string.Format(@"C:\temp\{0}", "test2.docx");

                File.Delete(filePath);

                using (Stream stream = vaultApi.Files.GetFileBySearch(options))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        int count = 0;
                        do
                        {
                            var buf = new byte[102400];
                            count = stream.Read(buf, 0, 102400);

                            readBytes += count;

                            fs.Write(buf, 0, count);
                        } while (count > 0);
                    }
                }


            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            Assert.Greater(readBytes, 0);
        }

        [Test]
        public void UploadFile()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);


            var generalFolder = vaultApi.Folders.GetFolderByPath(GeneralFolderDefaultName);
            if (generalFolder != null)
            {
                var document = vaultApi.Documents.CreateDocument(generalFolder.Id, "TestDocument", "Test Document", "1", DocumentState.Released);

                Assert.IsNotNull(document);

                var fileArray = TestHelperShared.GetSearchWordTextFile();
            }
        }

        [Test]
        public void UploadFileStream()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var generalFolder = vaultApi.Folders.GetFolderByPath(GeneralFolderDefaultName);
            if (generalFolder != null)
            {
                var document = vaultApi.Documents.CreateDocument(generalFolder.Id, "RandomNewDocument", "Random New Document in TestFolder", "1", DocumentState.Released);
                Assert.IsNotNull(document);

                var documentId = document.DocumentId;

                var fileStream = TestHelperShared.GetSearchWordTextFileStream();

                var indexFields = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("Name", "Test Data") };

                var optParameters = new JObject(){
                        new JProperty("source", "DocumentViewer"),
                        new JProperty("command", "copyPriorRevisionAnnotations")
                    };

                var returnObject = vaultApi.Files.UploadFile(documentId, "SearchWordTextFile.txt", "14", "14", DocumentCheckInState.Released, indexFields, fileStream, optParameters);
                var meta = returnObject.GetValue("meta") as JObject;
                if (meta != null)
                {
                    var status = meta.GetValue("status").Value<string>();
                    Assert.AreEqual("200", status);

                    var checkinstatusString = meta.GetValue("checkInStatus").Value<string>();
                    Assert.AreEqual(CheckInStatusType.CheckedIn.ToString(), checkinstatusString);
                }
            }
        }

        [Test]
        public void CreateDocumentAndUploadFileStream()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var generalFolder = vaultApi.Folders.GetFolderByPath(GeneralFolderDefaultName);
            if (generalFolder != null)
            {
                var indexFields = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Name", "Test Data"),
                        new KeyValuePair<string, string>("CustomerId", "12345")
                    };

                var document = vaultApi.Documents.CreateDocument(generalFolder.Id, "RandomNewDocument", "Random New Document in TestFolder", "1", DocumentState.Released, indexFields);
                Assert.IsNotNull(document);

                var documentId = document.DocumentId;

                var fileStream = TestHelperShared.GetSearchWordTextFileStream();

                //remove Customer Id index field for testing required fields
                indexFields = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Name", "Test Data")
                    };

                var optParameters = new JObject(){
                        new JProperty("source", "DocumentViewer"),
                        new JProperty("command", "copyPriorRevisionAnnotations")
                    };

                var returnObject = vaultApi.Files.UploadFile(documentId, "SearchWordTextFile.txt", "2", "2", DocumentCheckInState.Replace, indexFields, fileStream, optParameters);

                var meta = returnObject.GetValue("meta") as JObject;
                if (meta != null)
                {
                    var status = meta.GetValue("status").Value<string>();
                    Assert.AreEqual("200", status);

                    var checkinstatusString = meta.GetValue("checkInStatus").Value<string>();
                    Assert.AreEqual(CheckInStatusType.CheckedIn.ToString(), checkinstatusString);
                }

                document = vaultApi.Documents.GetDocument(documentId);

                //Check out document using api/v1/{customeralias}/{customerdatabasealias}/documents/{id}/status end point
                var result = vaultApi.Documents.UpdateDocumentCheckInStatus(documentId, CheckInStatus.CheckedOut);

                //Un-release document using api/v1/{customeralias}/{customerdatabasealias}/documents/{id}/revision/{childId}/state end point
                result = vaultApi.Documents.UpdateDocumentReleaseState(documentId, document.Id, DocumentState.Unreleased);
            }
        }

        [Test]
        public void GetDocumentRevisionFile()
        {
            var readBytes = 0;

            try
            {
                VaultApi vaultApi = new VaultApi(this);

                Assert.IsNotNull(vaultApi);

                Guid fileId = new Guid("227348D1-986E-E411-826D-14FEB5F06078");

                string filePath = string.Format(@"C:\temp\{0}", "test2.docx");

                File.Delete(filePath);


                using (Stream stream = vaultApi.Files.GetStream(fileId))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        int count = 0;
                        do
                        {
                            var buf = new byte[102400];
                            count = stream.Read(buf, 0, 102400);

                            readBytes += count;

                            fs.Write(buf, 0, count);
                        } while (count > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            Assert.Greater(readBytes, 0);
        }


        [Test]
        public void UploadZeroByteFile()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);


            var folder = vaultApi.Folders.GetFolderByPath("/Test");

            Assert.NotNull(folder);

            if (folder != null)
            {
                var document = vaultApi.Documents.CreateDocument(folder.Id, "12345.txt", "Zero Byte Upload", "1", DocumentState.Released);

                Assert.IsNotNull(document);

                var fileArray = new byte[0];

                var returnObject = vaultApi.Files.UploadZeroByteFile(document.DocumentId, "12345.txt", 5, fileArray);
            }
        }
    }
}
