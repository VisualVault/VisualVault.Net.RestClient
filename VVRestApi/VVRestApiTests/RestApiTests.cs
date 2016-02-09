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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using VVRestApi.Common.Extensions;
using Newtonsoft.Json.Linq;
using VVRestApi.Common.Logging;
using VVRestApi.Vault.DocumentViewer;
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

        string annotationList = @"
<document xmlns=""http://snowbound.com/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" schemaLocation=""http://snowbound.com/XMLSchema flexAnn.xsd"">\r\n  
    <docMeta>\r\n    
        <docID>DOC_ID</docID>\r\n    
        <docName>ANNOTATED_FILE_NAME</docName>\r\n  
    </docMeta>\r\n  
    <docLayers>\r\n    
        <layer />\r\n  
    </docLayers>\r\n  
    <docPages>\r\n    
        <page>\r\n      
            <pageMeta>\r\n        
                <pageNumber>0</pageNumber>\r\n        
                <pageWidth>2200</pageWidth>\r\n        
                <pageHeight>2862</pageHeight>\r\n        
                <doubleByte>true</doubleByte>\r\n      
            </pageMeta>\r\n      
            <pageObjects>\r\n        
                <annObject>\r\n          
                    <annMeta>\r\n            
                        <annId>ANN_ID</annId>\r\n            
                        <annType>Sticky Note</annType>\r\n            
                        <annLayerID>ANNOTATION_LAYER_ID</annLayerID>\r\n            
                        <annOrdinal>0</annOrdinal>\r\n            
                        <annDelete>false</annDelete>\r\n            
                        <annHistory>\r\n              
                            <annCreateDate>2015-10-22T16:08:07.374-0000</annCreateDate>\r\n              
                            <annCreateUser>\r\n              
                            </annCreateUser>\r\n            
                        </annHistory>\r\n          
                    </annMeta>\r\n          
                    <annTransparent>false</annTransparent>\r\n          
                    <fontInfo>\r\n            
                        <fontName>Arial</fontName>\r\n            
                        <fontSize>39</fontSize>\r\n            
                        <fontBold>false</fontBold>\r\n            
                        <fontItalic>false</fontItalic>\r\n            
                        <fontStrike>false</fontStrike>\r\n            
                        <fontUnderline>false</fontUnderline>\r\n            
                        <fontColor>000000</fontColor>\r\n          
                    </fontInfo>\r\n          
                    <textString></textString>\r\n          
                    <fillInfo>\r\n            
                        <fillColor>FCEFA1</fillColor>\r\n            
                        <fillTransparent>false</fillTransparent>\r\n            
                        <fillTransparentPercent />\r\n          
                    </fillInfo>\r\n          
                    <rotationAngle>0</rotationAngle>\r\n          
                    <annStartX>435</annStartX>\r\n          
                    <annStartY>90</annStartY>\r\n          
                    <annWidth>1134</annWidth>\r\n          
                    <annHeight>466</annHeight>\r\n        
                </annObject>\r\n        
                <annObject>\r\n          
                    <annMeta>\r\n
                        <annId>ANN_ID</annId>\r\n            
                        <annType>Rubber Stamp</annType>\r\n            
                        <annLayerID>ANNOTATION_LAYER_ID</annLayerID>\r\n            
                        <annOrdinal>1</annOrdinal>\r\n            
                        <annDelete>false</annDelete>\r\n            
                        <annHistory>\r\n              
                            <annCreateDate></annCreateDate>\r\n              
                            <annCreateUser>\r\n              
                            </annCreateUser>\r\n            
                        </annHistory>\r\n          
                    </annMeta>\r\n          
                    <annTransparent>false</annTransparent>\r\n          
                    <fontInfo>\r\n            
                        <fontName>Arial</fontName>\r\n            
                        <fontSize>39</fontSize>\r\n            
                        <fontBold>false</fontBold>\r\n            
                        <fontItalic>false</fontItalic>\r\n            
                        <fontStrike>false</fontStrike>\r\n            
                        <fontUnderline>false</fontUnderline>\r\n            
                        <fontColor>000000</fontColor>\r\n          
                    </fontInfo>\r\n          
                    <textString></textString>\r\n          
                    <rotationAngle>0</rotationAngle>\r\n          
                    <annStartX>1572</annStartX>\r\n
                    <annStartY>668</annStartY>\r\n          
                    <annWidth>409</annWidth>\r\n          
                    <annHeight>196</annHeight>\r\n        
                </annObject>\r\n      
            </pageObjects>\r\n    
        </page>\r\n  
    </docPages>\r\n
</document>
        ";


        #region Constants




        //Base URL to VisualVault.  Copy URL string preceding the version number ("/v1")
        const string VaultApiBaseUrl = "http://development7/VisualVault4_1_12";

        //API version number (number following /v in the URL).  Used to provide backward compatitiblity.
        const string ApiVersion = "1";

        //OAuth2 token endpoint, exchange credentials for api access token
        //typically the VaultApiBaseUrl + /oauth/token unless using an external OAuth server
        private const string OAuthServerTokenEndPoint = "http://development7/VisualVault4_1_12/oauth/token";

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
        const string ResourceOwnerUsID = "ABD2F88A-8861-E111-8E23-14FEB5F06078";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string ResourceOwnerPassword = "p";


        //const string VaultApiBaseUrl = "http://development7/VisualVault4_1_10";
        //private const string OAuthServerTokenEndPoint = "http://development7/VisualVault4_1_10/oauth/token";
        //const string ApiVersion = "1";
        //const string CustomerAlias = "Amazon";
        //const string DatabaseAlias = "Main";
        //const string ClientId = "0ec5fb65-95e3-4833-847e-4d299752bd64";
        //const string ClientSecret = "QzeojEH6vDylTXmYgSrcPqn6AQKWgx/VauvPxNeUXjw=";
        //const string Scope = "vault";
        //const string ResourceOwnerUserName = "amazon.admin";
        //const string ResourceOwnerUsID = "4EEE59B9-0599-E511-AB25-5CF3706C36ED";
        //const string ResourceOwnerPassword = "p";

        #endregion


        #region Authentication, Credentials and Token Tests

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
        public void GetDefaultCustomerAndDatabaseAliases()
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

            var defaultCustomerInfo = vaultApi.Users.GetUserDefaultCustomerAndDatabaseInfo();
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

        #region Sites User and Group Tests

        [Test]
        public void GetHomeSiteTest()
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

            var site = vaultApi.Sites.GetSite("Home", null);

            Assert.IsNotNull(site);

            var homeSite = vaultApi.Sites.GetSites(new RequestOptions() { Query = string.Format("StId eq '{0}'", site.Id) });

            Assert.IsNotNull(homeSite);
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
        public void GetGroupMembers()
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

            var groupId = new Guid("1249586F-A961-E111-8E23-14FEB5F06078");

            var groupMembers = vaultApi.Groups.GetGroupMembers(groupId);

            Assert.IsNotEmpty(groupMembers);
        }

        [Test]
        public void UpdateGroupDescription()
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

            var groupId = new Guid("B0B428D4-4183-E511-82B0-5CF3706C36ED");
            var newDescription = "New Group Desciption";
            var group = vaultApi.Groups.UpdateGroupDescription(groupId, newDescription);

            Assert.IsNotNull(group);
        }


        [Test]
        public void AddGroupMember()
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

            var groupId = new Guid("1249586F-A961-E111-8E23-14FEB5F06078");
            var userId = new Guid("92D49919-BBD1-E411-8281-14FEB5F06078");
            var groupMembers = vaultApi.Groups.AddUserToGroup(groupId, userId);

            Assert.IsNotEmpty(groupMembers);
        }

        [Test]
        public void AddGroupMembers()
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

            var groupId = new Guid("22BF8B4C-A961-E111-8E23-14FEB5F06078");
            var memberName = "User.wp";

            vaultApi.Groups.RemoveGroupMember(groupId, memberName);
        }

        [Test]
        public void RemoveGroupMemberById()
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

            var groupId = new Guid("22BF8B4C-A961-E111-8E23-14FEB5F06078");
            var memberId = new Guid("CA8A6D05-C78C-E211-A797-14FEB5F06078");

            vaultApi.Groups.RemoveGroupMember(groupId, memberId);

            
        }

        #endregion

        #region Form Tests


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
                Debug.WriteLine("Form template name: " + formTemplate.Name);
            }

            Assert.IsTrue(formTemplates.Items.Count > 0);
        }

        [Test]
        public void FormTemplateNameTest()
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

            var formTemplate = vaultApi.FormTemplates.GetFormTemplate("Encounters");

            Assert.IsNotNull(formTemplate);

            Debug.WriteLine("Form template name: " + formTemplate.Name);
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

            var formTemplateId = new Guid("F9CB9977-5B0E-E211-80A1-14FEB5F06078");

            //var options = new RequestOptions
            //{
            //    Fields = "dhdocid,revisionid,City,First Name,Unit"
            //};
            var options = new RequestOptions
            {
                Query = "[VVModifyDate] ge '2015-11-07T22:18:09.444Z'",
                Expand = true
            };

            var formdata = vaultApi.FormTemplates.GetFormInstanceData(formTemplateId, options);
            Assert.IsNotNull(formdata);

            //foreach (FormTemplate formTemplate in formTemplates.Items)
            //{
            //    Assert.IsNotNullOrEmpty(formTemplate.Name);
            //}

            //Assert.IsNotNull(formTemplates);
        }

        [Test]
        public void CreateNewFormInstanceWithData()
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

            var template = vaultApi.FormTemplates.GetFormTemplate("API REST Form Test ");
            Assert.IsNotNull(template);

            //var formTemplateId = new Guid("F9CB9977-5B0E-E211-80A1-14FEB5F06078");

            var fields = new List<KeyValuePair<string, object>>();
            fields.Add(new KeyValuePair<string, object>("Field1", "Some Text Value"));
            fields.Add(new KeyValuePair<string, object>("Field2", "jj"));
            fields.Add(new KeyValuePair<string, object>("Field3", "Bad Date Example"));

            var formInstance = vaultApi.FormTemplates.CreateNewFormInstance(template.Id, fields);
            Assert.IsNotNull(formInstance);

            //foreach (FormTemplate formTemplate in formTemplates.Items)
            //{
            //    Assert.IsNotNullOrEmpty(formTemplate.Name);
            //}

            //Assert.IsNotNull(formTemplates);
        }

        [Test]
        public void FormDataLookupByInstanceNameTest()
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

            //"FormTemplates/bde2c653-f735-e511-80c8-0050568dab97/forms", "q=[instancename] eq '" + masterFormId + "'"	"q=[instancename] eq 'Patient-000053'"
            //"bde2c653-f735-e511-80c8-0050568dab97"
            //Query = "[instancename] eq 'Patient-000053'",
            var requestOptions = new RequestOptions
            {
                Query = "[ModifyDate] ge '2015-11-07T22:18:09.444Z'",
                Fields = "dhdocid,revisionid"
            };

            var formdata = vaultApi.FormTemplates.GetFormInstanceData(new Guid("FDC7C07B-5ACC-E511-AB37-5CF3706C36ED"), requestOptions);

            //foreach (FormTemplate formTemplate in formTemplates.Items)
            //{
            //    Assert.IsNotNullOrEmpty(formTemplate.Name);
            //}

            Assert.IsNotNull(formdata);
        }


        #endregion
        
        #region Folder Tests

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
                var documentList = vaultApi.Folders.GetFolderDocuments(arbysFolder.Id, new RequestOptions() { Skip = 0, Take = 10 });

                Assert.IsNotNull(documentList);
            }

        }

        [Test]
        public void GetFolderDocumentsWithSort()
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
                var documentList = vaultApi.Folders.GetFolderDocuments(arbysFolder.Id, "createdate", "desc", new RequestOptions() { Skip = 10, Take = 5 });

                Assert.IsNotNull(documentList);
            }

        }

        [Test]
        public void GetChildFolders()
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

            var topLevelFolders = vaultApi.Folders.GetTopLevelFolders();
            Assert.IsNotEmpty(topLevelFolders);
        }

        [Test]
        public void CreateTopLevelFolder()
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

            var generalFolder = vaultApi.Folders.GetFolderByPath("/General");

            Assert.AreNotEqual(Guid.Empty, generalFolder.Id);

            if (generalFolder != null)
            {
                var securityActionList = new List<SecurityMemberApplyAction>();
                securityActionList.Add(new SecurityMemberApplyAction
                {
                    Action = SecurityAction.Add,
                    MemberId = new Guid("069493C9-36AD-E211-9D53-14FEB5F06078"),
                    MemberType =  MemberType.Group,
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

            var generalFolder = vaultApi.Folders.GetFolderByPath("/General");

            Assert.AreNotEqual(Guid.Empty, generalFolder.Id);

            if (generalFolder != null)
            {
                var memberId = new Guid("22BF8B4C-A961-E111-8E23-14FEB5F06078");

                var updateCount = vaultApi.Folders.RemoveSecurityMember(generalFolder.Id, memberId);

                Assert.AreEqual(1, updateCount);
            }
        }

        #endregion

        #region Users Home Folder Test

        [Test]
        public void CreateUsersTopLevelContainerFolder()
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

            var folder = vaultApi.Folders.CreateUsersHomeFolder(new Guid(RestApiTests.ResourceOwnerUsID));
            Assert.IsNotNull(folder);
        }

        [Test]
        public void GetUsersHomeFolder()
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

        #endregion

        #region Folder IndexField Tests

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

            var arbysFolder = vaultApi.Folders.GetFolderByPath("/Archer");
            Assert.IsNotNull(arbysFolder);
            if (arbysFolder != null)
            {
                var indexFieldList = vaultApi.Folders.GetFolderIndexFields(arbysFolder.Id);

            }



            //if (arbysFolder != null)
            //{
            //    var indexFieldList = vaultApi.Folders.GetFolderIndexFields(arbysFolder.Id, new RequestOptions { Fields = "Id,FieldType,Label,Required,DefaultValue,OrdinalPosition,FolderOrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });

            //    Assert.IsNotNull(indexFieldList);
            //}

            //var folderId = new Guid("C9B9DB43-5BCF-E411-8281-14FEB5F06078");

            //var indexFieldList = vaultApi.Folders.GetFolderIndexFields(folderId, new RequestOptions { Fields = "Id,FieldType,Label,Required,DefaultValue,OrdinalPosition,FolderOrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            //var indexFieldList = vaultApi.Folders.GetFolderIndexFields(folderId, new RequestOptions { Fields = "Id,FieldType,Label" });
            //Assert.IsNotEmpty(indexFieldList);

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
        public void GetFolderIndexFieldSelectOptions()
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

            var archerFolder = vaultApi.Folders.GetFolderByPath("/Archer");
            Assert.IsNotNull(archerFolder);
            if (archerFolder != null)
            {
                var userList = new Guid("CC742C04-B7A4-E311-868A-14FEB5F06078");
                //var selectOptions = vaultApi.Folders.GetFolderIndexFieldSelectOptionsList(archerFolder.Id, new Guid("2b5308f9-05ec-e311-a839-14feb5f06078"));
                var selectOptions = vaultApi.Folders.GetFolderIndexFieldSelectOptionsList(archerFolder.Id, userList);
                Assert.IsNotEmpty(selectOptions);
            }
        }

        [Test]
        public void RelateFolderToIndexFieldDefinition()
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

            var indexFieldDefinitionId = new Guid("B3C44BAA-944D-E511-82A3-5CF3706C36ED");


            var imagesFolder = vaultApi.Folders.GetFolderByPath("/Images");
            if (imagesFolder != null)
            {

                var folderIndexField = vaultApi.IndexFields.RelateFolderToIndexFieldDefinition(indexFieldDefinitionId, imagesFolder.Id);

                Assert.IsNotNull(folderIndexField);
            }

        }



        #endregion
        
        #region Document Tests


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

            //"id": "6bf19069-0491-e411-8273-14feb5f06078",
            //"documentId": "aa767e69-0491-e411-8273-14feb5f06078",

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var document = vaultApi.Documents.GetDocument(dlId);

            Assert.IsNotNull(document);
        }
        
        [Test]
        public void GetDocumentBySearch()
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

            var options = new RequestOptions();

            //options.Query = "[Animals Two] eq 'Monkey' OR LEN([Cats]) > 8";
            //options.Query = "[Name Field] eq 'Receipt'";
            //options.Query = "LEN([Cats]) = 9 AND [Cats] = 'Chartreux'";
            //options.Query = "LEN('Chartreux') = 9 AND [Cats] = 'Chartreux'";
            //options.Expand = true;
            //options.Query = "name eq 'Arbys-00074'";
            //options.Fields = "Id,DocumentId,Name,Description,ReleaseState";
            options.Query = "[CheckOutBy] LIKE 'Ace%'";

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var document = vaultApi.Documents.GetDocumentsBySearch(options);

            Assert.IsNotNull(document);
        }

        [Test]
        public void GetDocumentBySearchIncludeIndexFields()
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
        public void CreateNewDocumentWithZeroByteFile()
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


        #endregion

        #region Shared Document Tests

        [Test]
        public void GetDocumentsSharedWithMe()
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

            var documentList = vaultApi.DocumentShares.GetDocumentsSharedWithMe();

            Assert.IsNotNull(documentList);
        }

        [Test]
        public void ShareDocumentWithUser()
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

            //var userWopUsId = new Guid("369493C9-36AD-E211-9D53-14FEB5F06078");
            var userWpUsId = new Guid("A6DFFCC3-35AD-E211-9D53-14FEB5F06078");

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documentShare = vaultApi.DocumentShares.ShareDocument(dlId, userWpUsId);

            Assert.IsNotNull(documentShare);
        }

        [Test]
        public void ShareDocumentWithUsers()
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

            var userWopUsId = new Guid("369493C9-36AD-E211-9D53-14FEB5F06078");

            vaultApi.DocumentShares.RemoveUserFromSharedDocument(dlId, userWopUsId);

            //Assert.IsNotNull(document);
        }

        [Test]
        public void GetListOfDocumentSharesOfDocument()
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
            var documentList = vaultApi.DocumentShares.GetListOfUsersDocumentSharedWith(dlId);

            Assert.IsNotNull(documentList);
        }



        #endregion
        
        #region Document Favorites Test

        [Test]
        public void GetDocumentFavorites()
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

            //var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documentList = vaultApi.Documents.GetDocumentFavorites();

            Assert.IsNotNull(documentList);
        }

        [Test]
        public void SetDocumentAsFavorites()
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
            var document = vaultApi.Documents.SetDocumentAsFavorites(dlId);

            Assert.IsNotNull(document);
        }

        [Test]
        public void RemoveDocumentAsFavorites()
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

            vaultApi.Documents.RemoveDocumentAsFavorites(dlId);

            //Assert.IsNotNull(document);
        }
        #endregion
        
        #region Document IndexField Tests

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

            var requestedIndexFields1 = new List<string>()
            {
                 "Textbox",
                 "Numeric",
                 "UserDropDown",
                 "Date",
                  //"States",
                 "Multiline"
            };

            var requestedIndexFields2 = new List<string>() { 
                //"Id",
                //"DhId",
                "FieldId",
                "FieldType",
                "Label",
                //"Required",
                //"Value",
                //"OrdinalPosition",
                //"CreateDate",
                //"CreateById",
                //"CreateBy",
                //"ModifyDate",
                //"ModifyBy",
                //"ModifyById" 
            };

            var indexFieldList = vaultApi.Documents.GetDocumentIndexFields(dlId, new RequestOptions { Fields = string.Join(",", requestedIndexFields2) });

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
            var fieldId = new Guid("3AD6D13A-2E75-E111-84E2-14FEB5F06078");

            var docRevIndexField = vaultApi.Documents.GetDocumentRevisionIndexField(dlId, dhId, fieldId);

            Assert.IsNotNull(docRevIndexField);
        }
        
        [Test]
        public void UpdateIndexFieldForDocument()
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

            var dlId = new Guid("85fa0e91-a64a-e511-82a3-5cf3706c36ed");
            var dhId = new Guid("BD9DB6B5-A64A-E511-82A3-5CF3706C36ED");
            var fieldId = new Guid("3AD6D13A-2E75-E111-84E2-14FEB5F06078");
            var dataId = new Guid("5ABEAFB5-A64A-E511-82A3-5CF3706C36ED");

            var value = "Movies";

            var docIndexField = vaultApi.Documents.UpdateIndexFieldValue(dlId, fieldId, value);

            Assert.AreEqual(value, docIndexField.Value);

        }

        [Test]
        public void UpdateIndexFieldsForDocument()
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

            var dlId = new Guid("85fa0e91-a64a-e511-82a3-5cf3706c36ed");
            //var dhId = new Guid("BD9DB6B5-A64A-E511-82A3-5CF3706C36ED");
            //var dataId = new Guid("5ABEAFB5-A64A-E511-82A3-5CF3706C36ED");

            var indexFields = new List<KeyValuePair<string, string>>
            {       
                new KeyValuePair<string, string>("name", "Some common name"),
                new KeyValuePair<string, string>("text1", "Appropriate text goes here"),
            };

            var docIndexFields = vaultApi.Documents.UpdateIndexFieldValues(dlId, indexFields);

            Assert.IsNotEmpty(docIndexFields);

        }

        #endregion

        #region Global IndexField Tests


        [Test]
        public void GetGlobalIndexFieldDefinitions()
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

            var indexFields = vaultApi.IndexFields.GetIndexFields();

            Assert.IsNotEmpty(indexFields);
        }

        [Test]
        public void GetGlobalIndexFieldDefinition()
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

            var options = new RequestOptions();
            options.Query = "label eq 'Start Date'";
            var matchingIndexDefs = vaultApi.IndexFields.GetIndexFields(options);

            //var indexFields = vaultApi.IndexFields.GetIndexFields();

            Assert.IsNotNull(matchingIndexDefs);
        }

        [Test]
        public void CreateGlobalIndexFieldDefinition()
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

            var indexField = vaultApi.IndexFields.CreateIndexField("US Parks 1", "List of US Parks", FolderIndexFieldType.DatasourceDropDown, Guid.Empty, new Guid("1AC5A2D0-1F4D-E511-82A3-5CF3706C36ED"), "Name", "Id", true, "2");

            Assert.IsNotNull(indexField);
        }



        #endregion

        #region File Tests

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

                        
            //var arbysFolder = vaultApi.Folders.GetFolderByPath("/Arbys");
            //if (arbysFolder != null)
            //{
                var document = vaultApi.Documents.CreateDocument(new Guid("C9B9DB43-5BCF-E411-8281-14FEB5F06078"), "SeventhNewDocument", "Seventh New Document in Arbys", "1", DocumentState.Released);

                Assert.IsNotNull(document);

                var fileArray = TestHelperShared.GetSearchWordTextFile();

                //var returnObject = vaultApi.Files.UploadFile(document.DocumentId, "SearchWordTextFile", fileArray);

            //}
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


            //var testFolder = vaultApi.Folders.GetFolderByPath("/Arbys");
            //if (testFolder != null)
            //{
            var document = vaultApi.Documents.CreateDocument(new Guid("C9B9DB43-5BCF-E411-8281-14FEB5F06078"), "RandomNewDocument", "Random New Document in TestFolder", "1", DocumentState.Released);
            Assert.IsNotNull(document);
                
            var documentId = document.DocumentId;
            //documentId = Guid.Empty;
                
            var fileStream = TestHelperShared.GetSearchWordTextFileStream();
            //var byteArray = TestHelperShared.GetSearchWordTextFile();
            //var fileStream = TestHelperShared.GetFileStream(@"c:\temp\video1.mp4");
                
            //if (fileStream == null)
            //{
            //    throw new Exception("Could not get the embedded file: SearchWordTextFile.txt");
            //}

            var indexFields = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Name", "Test Data")
            };

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
            //}
        }



        [Test]
        public void GetDocumentRevisionFile()
        {
            var readBytes = 0;

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


        #endregion

        #region Annotations

        [Test]
        public void GetUsersAnnotationsPrivilege()
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

            var usId = new Guid("abd2f88a-8861-e111-8e23-14feb5f06078");

            var privilege = vaultApi.DocumentViewer.GetUserAnnotationPermissions(usId, "Signature");

            Assert.Greater(privilege, 0);
        }

        [Test]
        public void GetDocumentAnnotations()
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

            var dhId = new Guid("3f38898c-3278-e511-82ab-5cf3706c36ed");

            var data = vaultApi.DocumentViewer.GetDocumentAnnotationsByLayer(dhId, "Signature");

            Assert.IsNotNull(data);
        }
        
        [Test]
        public void GetAnnotationLayers()
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

            var annotationLayers = vaultApi.DocumentViewer.GetAnnotationLayers();
        }

        [Test]
        public void GetAnnotationLayersWithPrivileges()
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

            var annotationLayers = vaultApi.DocumentViewer.GetAnnotationLayersWithPrivileges(new Guid(RestApiTests.ResourceOwnerUsID));
        }

        [Test]
        public void SaveAnnotation()
        {
            //var byteArrayAnnotations = Convert.FromBase64String(ann);
            //var stringAnnotations = Encoding.UTF8.GetString(byteArrayAnnotations);
            //Debug.WriteLine(stringAnnotations);

            //var sb = new StringBuilder();

            //var xDoc = System.Xml.Linq.XElement.Parse(stringAnnotations);
            //XNamespace df = xDoc.Name.Namespace;




            //var clientSecrets = new ClientSecrets
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



            //var vaultApi = new VaultApi(clientSecrets);
            //Assert.IsNotNull(vaultApi);

            //var usId = new Guid("abd2f88a-8861-e111-8e23-14feb5f06078");
            //var dhId = new Guid("3f38898c-3278-e511-82ab-5cf3706c36ed");
            var stringAnnotations = "";
            var sb = new StringBuilder();

            var xDoc = System.Xml.Linq.XElement.Parse(stringAnnotations);
            XNamespace df = xDoc.Name.Namespace;

            var anns = xDoc.Descendants(df + "annObject").ToList();
            Debug.WriteLine(anns.Count());

            foreach (var xElement in anns)
            {
                var annTypes = xElement.Descendants(df + "annType").FirstOrDefault();

                if (annTypes != null)
                {
                    switch (annTypes.Value)
                    {
                        case "Sticky Note":
                            GetStickyNoteContent(sb, df, xElement);
                            break;
                        case "Rubber Stamp":
                            GetRubberStampContent(sb, df, xElement);
                            break;
                    }
                }
            }

            Debug.WriteLine(sb);
        }
        


        private void GetStickyNoteContent(StringBuilder sb, XNamespace df, XElement xElement)
        {
            var textElement = xElement.Descendants(df + "textString").FirstOrDefault();
            if (textElement != null)
            {
                var textValue = textElement.Value;
                if (!string.IsNullOrWhiteSpace(textValue))
                {
                    var value = DecodeBase64EncodedString(textValue);
                    if (sb.Length > 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(value);
                }

            }
        }


        private void GetRubberStampContent(StringBuilder sb, XNamespace df, XElement xElement)
        {
            var textElement = xElement.Descendants(df + "textString").FirstOrDefault();
            if (textElement != null)
            {
                var textValue = textElement.Value;
                if (!string.IsNullOrWhiteSpace(textValue))
                {
                    var value = DecodeBase64EncodedString(textValue);
                    if (sb.Length > 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(value);
                }

            }
        }

        private string DecodeBase64EncodedString(string annotation)
        {
            var result = "";
            try
            {
                var byteArrayAnnotations = Convert.FromBase64String(annotation);
                result = Encoding.BigEndianUnicode.GetString(byteArrayAnnotations);
            }
            catch (Exception)
            {

            }

            return result;
        }


        #endregion

        #region Custom Query Results

        [Test]
        public void GetCustomQueryByQueryName()
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

            var queryName = "Department";
            var queryId = new Guid("AEB2F858-7B96-E111-972B-14FEB5F06078");

            var results = vaultApi.CustomQueryManager.GetCustomQueryResults(queryName);

            var count = results;
        }

        [Test]
        public void GetCustomQueryByQueryId()
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

            var queryName = "Department";
            var queryId = new Guid("AEB2F858-7B96-E111-972B-14FEB5F06078");

            var results = vaultApi.CustomQueryManager.GetCustomQueryResults(queryId);
        }

        #endregion

        #region Tests

        [Test]
        public void RequestOptionTest()
        {
            VVRestApi.Common.RequestOptions options = new RequestOptions();
            options.Query = "name eq 'world' AND id eq 'whatever'";

            var request = options.GetQueryString("q=userid eq 'vault.config'&stuff=things");
            Assert.IsNotNullOrEmpty(request);
        }

        [Test]
        public void CallScheduledProcessCompleteUsingQueryString()
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

            var spToken = new Guid("C1647446-4417-4341-B1F2-D82FAAEE20EA");

            vaultApi.ScheduledProcess.CallCompleteScheduledProcessUsingQueryString(spToken, "Test Message", true);

            //Assert.IsNotNull(data);
        }

        [Test]
        public void CallScheduledProcessCompleteUsingPostedData()
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

            var spToken = new Guid("C1647446-4417-4341-B1F2-D82FAAEE20EA");

            vaultApi.ScheduledProcess.CallCompleteScheduledProcessUsingPostedData(spToken, "Test Message", true);

            //Assert.IsNotNull(data);
        }

        [Test]
        public void RunScheduledProcesses()
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

            vaultApi.ScheduledProcess.RunScheduledProcesses();

            //Assert.IsNotNull(data);
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
        public void GetCustomerDatabaseInfo()
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

            var dbInfo = vaultApi.Customer.GetCustomerDatabaseInfo();

            Assert.IsNotNull(dbInfo);

        }

        #endregion

    }
}