// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginTests.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// <summary>
//   The login tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using VVRestApi.Common.Extensions;
using Newtonsoft.Json.Linq;
using VVRestApi.Common.Logging;
using VVRestApi.Vault.Forms;
using VVRestApi.Vault.Library;
using VVRestApi.Vault.Meta;
using VVRestApi.Vault.PersistedData;
using VVRestApi.Vault.Users;
using VVRestApiTests.TestHelpers;

namespace VVRestApiTests
{
    using NUnit.Framework;
    using VVRestApi.Common;
    using VVRestApi.Vault;

    /// <summary>
    ///     The login tests.
    /// </summary>
    [TestFixture]
    public class RestApiTests
    {
        #region Constants

        //Base URL to VisualVault.  Copy URL string preceding the version number ("/v1")
        const string VaultApiBaseUrl = "http://development5/VisualVault4_1_11";

        //API version number (number following /v in the URL).  Used to provide backward compatitiblity.
        const string ApiVersion = "1";

        //OAuth2 token endpoint, exchange credentials for api access token
        //typically the VaultApiBaseUrl + /oauth/token unless using an external OAuth server
        private const string OAuthServerTokenEndPoint = "http://development5/VisualVault4_1_11/oauth/token";

        //your customer alias value.  Visisble in the URL when you log into VisualVault
        const string CustomerAlias = "AceOfHearts";

        //your customer database alias value.  Visisble in the URL when you log into VisualVault
        const string DatabaseAlias = "Main";

        //Copy "API Key" value from User Account Property Screen
        const string ClientId = "ce9e042b-8755-42d5-97af-435afe70152b";

        //Copy "API Secret" value from User Account Property Screen
        const string ClientSecret = "/PbgaChHbPoboS/1s07E6pfGCNFSdqPsDnB/yiKHfHw=";

        // Scope is used to determine what resource types will be available after authentication.  If unsure of the scope to provide use
        // either 'vault' or no value.  'vault' scope is used to request access to a specific customer vault (aka customer database). 
        const string Scope = "vault";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string ResourceOwnerUserName = "ace.admin";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string ResourceOwnerPassword = "p";

        #endregion

        #region Tests

        [Test]
        public void ClientCredentialsGrantType_LoginTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);
        }

        [Test]
        public void ResourceOwnerGrantType_LoginTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets, RestApiTests.ResourceOwnerUserName, RestApiTests.ResourceOwnerPassword);

            Assert.IsNotNull(vaultApi);
        }

        [Test]
        public void RefreshTokenTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            Assert.IsTrue(vaultApi.RefreshAccessToken());
        }

        [Test]
        public void GetUsersTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            Page<User> users = vaultApi.Users.GetUsers();

            Assert.IsNotNull(users);
        }

        [Test]
        public void GetVaultUserWebLoginToken()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var user = vaultApi.Users.GetUser(ResourceOwnerUserName);

            if (user != null)
            {
                string value = user.GetWebLoginToken();

                //if necessary to validate an application user's credentials they can be provided as parameters
                //login token only returned if credentials are valid.
                //example:  string value = user.GetWebLoginToken("someuser","password");

                Assert.IsNotNullOrEmpty(value);
            }
        }

        [Test]
        public void FormTemplatesTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            Page<FormTemplate> formTemplates = vaultApi.FormTemplates.GetFormTemplates();

            foreach (FormTemplate formTemplate in formTemplates.Items)
            {
                Assert.IsNotNullOrEmpty(formTemplate.Name);
            }

            Assert.IsNotNull(formTemplates);
        }

        [Test]
        public void FormDataTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var formdata = vaultApi.FormTemplates.GetFormInstanceData(new Guid("B90A7D18-5B0E-E211-80A1-14FEB5F06078"), new Guid("F9CB9977-5B0E-E211-80A1-14FEB5F06078"), new RequestOptions
            {
                Fields = "dhdocid,revisionid,City,First Name,Unit"
            });

            //foreach (FormTemplate formTemplate in formTemplates.Items)
            //{
            //    Assert.IsNotNullOrEmpty(formTemplate.Name);
            //}

            //Assert.IsNotNull(formTemplates);
        }

        [Test]
        public void PersistedDataTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            const string dataToStore = "{\"invoice no\":\"272255\",\"formname\":\"orderform\",\"Address\":\"ds\"}";

            string persistedDataUniqueName = ShortGuid.NewGuid();

            PersistedClientData data = vaultApi.PersistedData.CreateData(persistedDataUniqueName, ScopeType.Global, dataToStore, "text/JSON", "", LinkedObjectType.None, null);

            Assert.IsNotNull(data);

            //get the persisted data by Id
            data = vaultApi.PersistedData.GetData(data.Id);

            Assert.IsNotNull(data);

            //Note:  The list of field names below was generated by running the GetApiObjectFieldNames test 
            //which calls the MetaData APi endpoint

            //PersistedData fields: 

            //CreateByUsId
            //CreateDateUtc
            //DataLength
            //DataMimeType
            //ExpirationDateUtc
            //Id
            //LinkedObjectId
            //LinkedObjectType
            //ModifiedByUsId
            //ModifiedDateUtc
            //Name
            //PersistedData
            //Scope

            //example of getting the persisted data by name (vs. using the Id) using a query
            //this api call returns a page of data matching the query parameters
            //in this example we only expect to get one item

            //basic query syntax (note field names must be enclosed in square brackets)
            //
            //[field name] {logical operator} '{predicate}'
            //
            //

            Page<PersistedClientData> dataPage = vaultApi.PersistedData.GetAllData(new RequestOptions() { Query = string.Format("[Name] eq '{0}'", persistedDataUniqueName) });

            Assert.IsNotNull(dataPage);

            var user = vaultApi.Users.GetUser(ResourceOwnerUserName);

            if (user != null)
            {
                string loginToken = user.GetWebLoginToken();

                Assert.IsNotNullOrEmpty(loginToken);

                string url = string.Format("{0}/v1/en/Customer412/Main/vvlogin?token={1}&returnurl=userportal%3fportalname={2}%26persistedId={3}", VaultApiBaseUrl, loginToken, "InvoiceData", data.Id);

                LogEventManager.Info(string.Format("Token login URL with Persisted Data Id query string parameter:{0} {1}", Environment.NewLine, url));
            }

        }

        [Test]
        public void RequestOptionTest()
        {
            VVRestApi.Common.RequestOptions options = new RequestOptions();
            options.Query = "name eq 'world' AND id eq 'whatever'";

            var request = options.GetQueryString("q=userid eq 'vault.config'&stuff=things");
            Assert.IsNotNullOrEmpty(request);
        }

        [Test]
        public void CreateCustomerTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var newCustomer = vaultApi.Customer.CreateCustomer("Customer2", "Customer2", "Main", "Customer2.Admin", "p",
                                             "username@company.com", 1, 5, true);

            Assert.IsNotNull(newCustomer);
        }

        [Test]
        public void GetApiObjectFieldNames()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            List<MetaDataType> ApiObjectTypes = vaultApi.Meta.GetDataTypes();

            StringBuilder sbFieldList = new StringBuilder();

            sbFieldList.AppendLine("API Object Field List");

            foreach (MetaDataType apiObjectType in ApiObjectTypes)
            {
                sbFieldList.AppendLine("-----------------------------------------------------------------");

                sbFieldList.AppendLine(string.Format("{0} fields: {1}", apiObjectType.Name, Environment.NewLine));

                foreach (string fieldName in apiObjectType.AvailableFields)
                {
                    sbFieldList.AppendLine(string.Format("{0}", fieldName));
                }
            }

            LogEventManager.Info(sbFieldList.ToString());
        }

        [Test]
        public void GetFolderDocuments()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var arbysFolder = vaultApi.Folders.GetFolderByPath("/Arbys");
            if (arbysFolder != null)
            {
                var documentList = vaultApi.Folders.GetFolderDocuments(arbysFolder.Id, new RequestOptions() { Skip = 10, Take = 5 });

                Assert.IsNotNull(documentList);
            }

        }


        [Test]
        public void NewDocument()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var arbysFolder = vaultApi.Folders.GetFolderByPath("/Arbys");
            if (arbysFolder != null)
            {
                var document = vaultApi.Documents.CreateDocument(arbysFolder.Id, "SixthNewDocument", "Sixth New Document in Arbys", "1", DocumentState.Released);

                Assert.IsNotNull(document);
            }

        }

        [Test]
        public void GetFolderIndexFields()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var arbysFolder = vaultApi.Folders.GetFolderByPath("/Arbys");
            if (arbysFolder != null)
            {
                var indexFieldList = vaultApi.Folders.GetFolderIndexFields(arbysFolder.Id, new RequestOptions { Fields = "Id,FieldType,Label,Required,DefaultValue,OrdinalPosition,FolderOrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });

                Assert.IsNotNull(indexFieldList);
            }

        }

        [Test]
        public void GetFolderIndexField()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var arbysFolder = vaultApi.Folders.GetFolderByPath("/Arbys");
            if (arbysFolder != null)
            {
                var indexFieldList = vaultApi.Folders.GetFolderIndexFields(arbysFolder.Id, new RequestOptions { Fields = "Id,FieldType,Label,Required,DefaultValue,OrdinalPosition,FolderOrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
                Assert.IsNotNull(indexFieldList);

                var idxField = indexFieldList.FirstOrDefault(i => i.Label == "Text1");
                if (idxField != null)
                {
                    var indexField = vaultApi.Folders.GetFolderIndexField(arbysFolder.Id, idxField.Id, new RequestOptions { Fields = "Id,FieldType,Label,Required,DefaultValue,OrdinalPosition,FolderOrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
                    Assert.IsNotNull(indexField);
                }


            }

        }

        [Test]
        public void UploadFile()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

                        
            var arbysFolder = vaultApi.Folders.GetFolderByPath("/Arbys");
            if (arbysFolder != null)
            {
                var document = vaultApi.Documents.CreateDocument(arbysFolder.Id, "SeventhNewDocument", "Seventh New Document in Arbys", "1", DocumentState.Released);

                Assert.IsNotNull(document);

                var fileArray = TestHelperShared.GetSearchWordTextFile();

                //var returnObject = vaultApi.Files.UploadFile(document.DocumentId, "SearchWordTextFile", fileArray);

            }
        }

        [Test]
        public void UploadFileStream()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);


            var testFolder = vaultApi.Folders.GetFolderByPath("/TestFolder");
            if (testFolder != null)
            {
                var document = vaultApi.Documents.CreateDocument(testFolder.Id, "RandomNewDocument", "Random New Document in TestFolder", "1", DocumentState.Released);
                Assert.IsNotNull(document);
                
                var documentId = document.DocumentId;
                //documentId = Guid.Empty;
                
                var fileStream = TestHelperShared.GetSearchWordTextFileStream();
                //var fileStream = TestHelperShared.GetFileStream(@"c:\temp\video1.mp4");
                
                //if (fileStream == null)
                //{
                //    throw new Exception("Could not get the embedded file: SearchWordTextFile.txt");
                //}

                var indexFields = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Name", "Ralph")
                };

                var returnObject = vaultApi.Files.UploadFile(documentId, "SearchWordTextFile.txt", "1", "1", DocumentCheckInState.Replace, indexFields, fileStream);
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
        public void GetDocumentIndexFields()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6FED0440-5C11-E511-828F-14FEB5F06078");

            var indexFieldList = vaultApi.Documents.GetDocumentIndexFields(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });

            Assert.IsNotNull(indexFieldList);
        }

        [Test]
        public void GetDocumentIndexField()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6FED0440-5C11-E511-828F-14FEB5F06078");

            var dataId = new Guid("71ED0440-5C11-E511-828F-14FEB5F06078");

            var indexFieldList = vaultApi.Documents.GetDocumentIndexField(dlId, dataId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });

            Assert.IsNotNull(indexFieldList);
        }

        [Test]
        public void GetDocument()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var document = vaultApi.Documents.GetDocument(dlId);

            Assert.IsNotNull(document);
        }
        
        [Test]
        public void GetDocumentRevisions()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            //var documentRevisionList = vaultApi.Documents.GetDocumentRevisions(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documentRevisionList = vaultApi.Documents.GetDocumentRevisions(dlId);

            Assert.IsNotNull(documentRevisionList);
        }

        [Test]
        public void GetDocumentRevision()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");
            var dhId = new Guid("F74A31AF-AAAC-E411-8279-14FEB5F06078");

            //var documentRevision = vaultApi.Documents.GetDocumentRevision(dlId, dhId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documentRevision = vaultApi.Documents.GetDocumentRevision(dlId, dhId);

            Assert.IsNotNull(documentRevision);
        }

        [Test]
        public void GetDocumentRevisionIndexFields()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");
            var dhId = new Guid("F74A31AF-AAAC-E411-8279-14FEB5F06078");

            var docRevIndexFields = vaultApi.Documents.GetDocumentRevisionIndexFields(dlId, dhId);

            Assert.IsNotNull(docRevIndexFields);
        }

        [Test]
        public void GetDocumentRevisionIndexField()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");
            var dhId = new Guid("F74A31AF-AAAC-E411-8279-14FEB5F06078");
            var dataId = new Guid("8A3527AF-AAAC-E411-8279-14FEB5F06078");

            var docRevIndexField = vaultApi.Documents.GetDocumentRevisionIndexField(dlId, dhId, dataId);

            Assert.IsNotNull(docRevIndexField);
        }

        [Test]
        public void GetDocumentRevisionFile()
        {
            try
            {
                ClientSecrets clientSecrets = new ClientSecrets
                                              {
                                                  ApiKey = RestApiTests.ClientId,
                                                  ApiSecret = RestApiTests.ClientSecret,
                                                  OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                                                  BaseUrl = RestApiTests.VaultApiBaseUrl,
                                                  ApiVersion = RestApiTests.ApiVersion,
                                                  CustomerAlias = RestApiTests.CustomerAlias,
                                                  DatabaseAlias = RestApiTests.DatabaseAlias,
                                                  Scope = RestApiTests.Scope
                                              };

                VaultApi vaultApi = new VaultApi(clientSecrets);

                Assert.IsNotNull(vaultApi);

                Guid fileId = new Guid("8d57c716-40e5-e411-beee-93df0d4ae3b6");

                string filePath = string.Format(@"C:\temp\Downloads\{0}","test2.docx");

                File.Delete(filePath);

                using(Stream stream = vaultApi.Files.GetStream(fileId))
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    int count = 0;
                    do
                    {
                        byte[] buf = new byte[102400];
                        count = stream.Read(buf, 0, 102400);
                        fs.Write(buf, 0, count);
                    } while (count > 0);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        [Test]
        public void VVRestApiNet2LoginTest()
        {
            //VVRestAPINet2.Common.ClientSecrets clientSecrets = new VVRestAPINet2.Common.ClientSecrets
            //{
            //    ApiKey = RestApiTests.ClientId,
            //    ApiSecret = RestApiTests.ClientSecret,
            //    OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
            //    BaseUrl = RestApiTests.VaultApiBaseUrl,
            //    ApiVersion = RestApiTests.ApiVersion,
            //    CustomerAlias = RestApiTests.CustomerAlias,
            //    DatabaseAlias = RestApiTests.DatabaseAlias,
            //    Scope = RestApiTests.Scope
            //};

            //VVRestAPINet2.Vault.VaultApi vaultApi = new VVRestAPINet2.Vault.VaultApi(clientSecrets);

            //Assert.NotNull(vaultApi);
        }

        #endregion
    }
}