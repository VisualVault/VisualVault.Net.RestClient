// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginTests.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// <summary>
//   The login tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using NUnit.Framework;
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
using VVRestApi.Vault.Configuration;
using VVRestApi.Vault.DocumentViewer;
using VVRestApi.Vault.Forms;
using VVRestApi.Vault.Library;
using VVRestApi.Vault.Meta;
using VVRestApi.Vault.PersistedData;
using VVRestApi.Vault.Users;
using VVRestApiTests.TestHelpers;

namespace VVRestApiTests.Core.Tests
{

    using VVRestApi.Common;
    using VVRestApi.Vault;

    /// <summary>
    ///     The login tests.
    /// </summary>
    [TestFixture]
    public class RestApiTests : IClientSecrets
    {

        #region Constants

        //Base URL to VisualVault.  Copy URL string preceding the version number ("/v1")
        const string _VaultApiBaseUrl = "http://localhost/visualvault4_1_13";

        //API version number (number following /v in the URL).  Used to provide backward compatitiblity.
        const string _ApiVersion = "1";

        //OAuth2 token endpoint, exchange credentials for api access token
        //typically the VaultApiBaseUrl + /oauth/token unless using an external OAuth server
        private const string _OAuthServerTokenEndPoint = "http://localhost/visualvault4_1_13/oauth/token";

        //your customer alias value.  Visisble in the URL when you log into VisualVault
        const string _CustomerAlias = "Main";

        //your customer database alias value.  Visisble in the URL when you log into VisualVault
        const string _DatabaseAlias = "VisualVaultCustomerMaintest";

        //Copy "API Key" value from User Account Property Screen
        const string _ClientId = "fab5d5b7-0878-4018-a27c-c95d3cd04f45";


        //Copy "API Secret" value from User Account Property Screen
        const string _ClientSecret = "4NPWkHSvbeR0yPQ+vbq3avoGONUSIgUBfcReLidq1NA=";


        // Scope is used to determine what resource types will be available after authentication.  If unsure of the scope to provide use
        // either 'vault' or no value.  'vault' scope is used to request access to a specific customer vault (aka customer database). 
        const string _Scope = "vault";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string _ResourceOwnerUserName = "";
        const string _ResourceOwnerUsID = "";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string _ResourceOwnerPassword = "";

        #endregion

        #region Client Authentication Properties

        public string BaseUrl
        {
            get { return _VaultApiBaseUrl; }
        }
        public string ApiKey
        {
            get { return _ClientId; }
        }
        public string ApiSecret
        {
            get { return _ClientSecret; }
        }
        public string CustomerAlias
        {
            get { return _CustomerAlias; }
        }
        public string DatabaseAlias
        {
            get { return _DatabaseAlias; }
        }
        public string ApiVersion
        {
            get { return _ApiVersion; }
        }
        public string OAuthTokenEndPoint
        {
            get { return _OAuthServerTokenEndPoint; }
        }
        public string Scope
        {
            get { return _Scope; }
        }

        #endregion


        #region Authentication, Credentials and Token Tests

        [Test]
        public void ClientCredentialsGrantType_LoginTest()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);
        }

        [Test]
        public void ResourceOwnerGrantType_LoginTest()
        {
            VaultApi vaultApi = new VaultApi(this, _ResourceOwnerUserName, _ResourceOwnerPassword);

            Assert.IsNotNull(vaultApi);
        }

        [Test]
        public void RefreshTokenTest()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            Assert.IsTrue(vaultApi.RefreshAccessToken());

            Assert.IsTrue(vaultApi.RefreshAccessToken());
        }

        [Test]
        public void GetVaultUserWebLoginToken()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var user = vaultApi.Users.GetUser(_ResourceOwnerUserName);

            if (user != null)
            {
                string value = user.GetWebLoginToken();

                //if necessary to validate an application user's credentials they can be provided as parameters
                //login token only returned if credentials are valid.
                //example:  string value = user.GetWebLoginToken("someuser","password");

                Assert.IsNotEmpty(value);
            }
        }

        [Test]
        public void GetDefaultCustomerAndDatabaseAliases()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var defaultCustomerInfo = vaultApi.Users.GetUserDefaultCustomerAndDatabaseInfo();
        }

        [Test]
        public void VVRestApiNet2LoginTest()
        {
            //VVRestAPINet2.Vault.VaultApi vaultApi = new VVRestAPINet2.Vault.VaultApi(this);

            //Assert.NotNull(vaultApi);
        }

        #endregion

        #region Sites Users and Group Tests

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

        #endregion

        #region Form Tests

        [Test]
        public void FormTemplatesTest()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            Page<FormTemplate> formTemplates = vaultApi.FormTemplates.GetFormTemplates();

            foreach (FormTemplate formTemplate in formTemplates.Items)
            {
                Assert.IsNotEmpty(formTemplate.Name);
                Debug.WriteLine("Form template name: " + formTemplate.Name);
            }

            Assert.IsTrue(formTemplates.Items.Count > 0);
        }

        [Test]
        public void FormTemplateNameTest()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var formTemplate = vaultApi.FormTemplates.GetFormTemplate("Encounters");

            Assert.IsNotNull(formTemplate);

            Debug.WriteLine("Form template name: " + formTemplate.Name);
        }

        [Test]
        public void FormDataTest()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            //var formTemplateId = new Guid("99FE73F4-590E-E211-80A1-14FEB5F06078");
            var formTemplateId = new Guid("FDC7C07B-5ACC-E511-AB37-5CF3706C36ED");

            //var options = new RequestOptions
            //{
            //    Fields = "dhdocid,revisionid,City,First Name,Unit"
            //};
            //var options = new RequestOptions
            //{
            //    Query = "[InstanceName] LIKE %27%2500%25%27",
            //    Expand = true
            //};
            var options = new RequestOptions
            {
                Query = "[InstanceName] eq 'Two Text-000004'",
                Expand = true
            };

            //f9cb9977-5b0e-e211-80a1-14feb5f06078

            var formdata = vaultApi.FormTemplates.GetFormInstanceData(formTemplateId, options);
            Assert.IsNotNull(formdata);

            //foreach (FormTemplate formTemplate in formTemplates.Items)
            //{
            //    Assert.IsNotNullOrEmpty(formTemplate.Name);
            //}

            //Assert.IsNotNull(formTemplates);
        }

        [Test]
        public void FormDataInstanceTest()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var formTemplateId = new Guid("99FE73F4-590E-E211-80A1-14FEB5F06078");

            //var options = new RequestOptions
            //{
            //    Fields = "dhdocid,revisionid,City,First Name,Unit"
            //};
            var options = new RequestOptions
            {
                Expand = true
            };

            var formId = new Guid("f9cb9977-5b0e-e211-80a1-14feb5f06078");

            var formdata = vaultApi.FormTemplates.GetFormInstanceData(formTemplateId, formId, options);
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
            VaultApi vaultApi = new VaultApi(this);

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
            VaultApi vaultApi = new VaultApi(this);

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

        [Test]
        public void GetRelatedDocumentsOfFormInstance()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var formInstanceId = new Guid("be1cd9c6-5acc-e511-ab37-5cf3706c36ed");

            //"FormTemplates/bde2c653-f735-e511-80c8-0050568dab97/forms", "q=[instancename] eq '" + masterFormId + "'"	"q=[instancename] eq 'Patient-000053'"
            //"bde2c653-f735-e511-80c8-0050568dab97"
            //Query = "[instancename] eq 'Patient-000053'",
            var requestOptions = new RequestOptions
            {
                Query = "[Name Field] like '%truck%'"
            };

            //var documents = vaultApi.FormInstances.GetDocumentsRelatedToFormInstance(formInstanceId, requestOptions);
            var documents = vaultApi.FormInstances.GetDocumentsRelatedToFormInstance(formInstanceId);

            //foreach (FormTemplate formTemplate in formTemplates.Items)
            //{
            //    Assert.IsNotNullOrEmpty(formTemplate.Name);
            //}

            Assert.IsNotEmpty(documents);
        }


        #endregion

        #region Form Instance Relation Tests

        [Test]
        public void FormRelateDoc()
        {
            VaultApi vaultApi = new VaultApi(this);
            var formId = new Guid("60b6f7db-f0be-e511-a698-e094676f83f7");
            var docId = new Guid("1c28741c-ab22-e611-a6aa-e094676f83f7");
            var options = new RequestOptions { };
            vaultApi.FormInstances.RelateDocumentToFormInstance(formId, docId, options);
        }

        [Test]
        public void FormRelateForm()
        {
            VaultApi vaultApi = new VaultApi(this);
            var formId = new Guid("60b6f7db-f0be-e511-a698-e094676f83f7");
            var formId2 = new Guid("1c28741c-ab22-e611-a6aa-e094676f83f7");
            var options = new RequestOptions { };
            vaultApi.FormInstances.RelateFormToFormInstance(formId, formId2, options);
        }

        [Test]
        public void FormRelateProject()
        {
            VaultApi vaultApi = new VaultApi(this);
            var formId = new Guid("60b6f7db-f0be-e511-a698-e094676f83f7");
            var projectId = new Guid("1c28741c-ab22-e611-a6aa-e094676f83f7");
            var options = new RequestOptions { };
            vaultApi.FormInstances.RelateProjectToFormInstance(formId, projectId, options);
        }

        [Test]
        public void FormUnRelateDoc()
        {
            VaultApi vaultApi = new VaultApi(this);
            var formId = new Guid("60b6f7db-f0be-e511-a698-e094676f83f7");
            var docId = new Guid("1c28741c-ab22-e611-a6aa-e094676f83f7");
            var options = new RequestOptions { };
            vaultApi.FormInstances.UnRelateDocumentToFormInstance(formId, docId, options);
        }

        [Test]
        public void FormUnRelateForm()
        {
            VaultApi vaultApi = new VaultApi(this);
            var formId = new Guid("60b6f7db-f0be-e511-a698-e094676f83f7");
            var formId2 = new Guid("1c28741c-ab22-e611-a6aa-e094676f83f7");
            var options = new RequestOptions { };
            vaultApi.FormInstances.UnRelateFormToFormInstance(formId, formId2, options);
        }

        [Test]
        public void FormUnRelateProject()
        {
            VaultApi vaultApi = new VaultApi(this);
            var formId = new Guid("60b6f7db-f0be-e511-a698-e094676f83f7");
            var projectId = new Guid("1c28741c-ab22-e611-a6aa-e094676f83f7");
            var options = new RequestOptions { };
            vaultApi.FormInstances.UnRelateProjectToFormInstance(formId, projectId, options);
        }

        #endregion

        #region Folder Tests

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

        #endregion

        #region Users Home Folder Test

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

        #endregion

        #region Folder IndexField Tests

        [Test]
        public void GetFolderIndexFields()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);
            //var folderId = new Guid("C9B9DB43-5BCF-E411-8281-14FEB5F06078");


            var arbysFolder = vaultApi.Folders.GetFolderByPath("/Archer");
            Assert.IsNotNull(arbysFolder);

            //var options = new RequestOptions
            //{
            //    Fields = "Id,FieldType,Label,Required,DefaultValue,OrdinalPosition,FolderOrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById"
            //};
            //var options = new RequestOptions
            //{
            //    Fields = "Id,FieldType,Label"
            //};

            var options = new RequestOptions
            {
                Fields = "Id,FieldType,Label"
            };

            var indexFieldList = vaultApi.Folders.GetFolderIndexFields(arbysFolder.Id, options);

            Assert.IsNotEmpty(indexFieldList);
        }

        [Test]
        public void GetFolderIndexField()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var arbysFolder = vaultApi.Folders.GetFolderByPath("/Arbys");
            if (arbysFolder != null)
            {
                var indexFieldList = vaultApi.Folders.GetFolderIndexFields(arbysFolder.Id, new RequestOptions { Fields = "Id,FieldType,Label,Required,DefaultValue,OrdinalPosition,FolderOrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
                Assert.IsNotNull(indexFieldList);

                var idxField = indexFieldList.FirstOrDefault(i => i.Label == "Text1");
                if (idxField != null)
                {
                    //var options = new RequestOptions
                    //{
                    //    Fields = "Id,FieldType,Label,Required,DefaultValue,OrdinalPosition,FolderOrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById"
                    //};
                    //var options = new RequestOptions
                    //{
                    //    Fields = "Id,FieldType,Label"
                    //};

                    var options = new RequestOptions
                    {
                        Fields = "Id,FieldType,Label"
                    };

                    var indexField = vaultApi.Folders.GetFolderIndexField(arbysFolder.Id, idxField.Id, options);
                    Assert.IsNotNull(indexField);
                }
            }
        }

        [Test]
        public void GetFolderIndexFieldSelectOptions()
        {
            var vaultApi = new VaultApi(this);

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
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var indexFieldDefinitionId = new Guid("B3C44BAA-944D-E511-82A3-5CF3706C36ED");


            var imagesFolder = vaultApi.Folders.GetFolderByPath("/Images");
            if (imagesFolder != null)
            {

                var folderIndexField = vaultApi.IndexFields.RelateFolderToIndexFieldDefinition(indexFieldDefinitionId, imagesFolder.Id);

                Assert.IsNotNull(folderIndexField);
            }

        }

        [Test]
        public void UpdateFolderIndexField()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var arbysFolder = vaultApi.Folders.GetFolderByPath("/Arbys");
            if (arbysFolder != null)
            {
                var indexFieldList = vaultApi.Folders.GetFolderIndexFields(arbysFolder.Id, new RequestOptions { Fields = "Id,FieldType,Label,Required,DefaultValue,OrdinalPosition,FolderOrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
                Assert.IsNotNull(indexFieldList);

                var idxField = indexFieldList.FirstOrDefault(i => i.Label == "Text1");
                if (idxField != null)
                {
                    var folderIndexField = vaultApi.Folders.UpdateFolderIndexFieldOverrideSettings(arbysFolder.Id, idxField.Id, Guid.Empty, "", "", Guid.Empty, false, "Sample Text");

                    Assert.AreEqual("Sample Text", folderIndexField.DefaultValue);
                }

            }
        }


        #endregion

        #region Document Tests


        [Test]
        public void NewDocument()
        {
            var vaultApi = new VaultApi(this);

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
            var vaultApi = new VaultApi(this);

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


        #endregion

        #region Shared Document Tests

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

        #endregion

        #region Document Favorites Test

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
        #endregion

        #region Document IndexField Tests

        [Test]
        public void GetDocumentIndexFields()
        {
            var vaultApi = new VaultApi(this);

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
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6FED0440-5C11-E511-828F-14FEB5F06078");

            var dataId = new Guid("71ED0440-5C11-E511-828F-14FEB5F06078");

            var indexFieldList = vaultApi.Documents.GetDocumentIndexField(dlId, dataId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });

            Assert.IsNotNull(indexFieldList);
        }

        [Test]
        public void GetDocumentRevisionIndexFields()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");
            var dhId = new Guid("F74A31AF-AAAC-E411-8279-14FEB5F06078");

            var docRevIndexFields = vaultApi.Documents.GetDocumentRevisionIndexFields(dlId, dhId);

            Assert.IsNotNull(docRevIndexFields);
        }

        [Test]
        public void GetDocumentRevisionIndexField()
        {
            var vaultApi = new VaultApi(this);

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
            var vaultApi = new VaultApi(this);

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
            var vaultApi = new VaultApi(this);

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
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var indexFields = vaultApi.IndexFields.GetIndexFields();

            Assert.IsNotEmpty(indexFields);
        }

        [Test]
        public void GetGlobalIndexFieldDefinition()
        {
            var vaultApi = new VaultApi(this);

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
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var indexField = vaultApi.IndexFields.CreateIndexField("US Parks 1", "List of US Parks", FolderIndexFieldType.DatasourceDropDown, Guid.Empty, new Guid("1AC5A2D0-1F4D-E511-82A3-5CF3706C36ED"), "Name", "Id", true, "2");

            Assert.IsNotNull(indexField);
        }



        #endregion

        #region File Tests

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
            var vaultApi = new VaultApi(this);

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


        #endregion

        #region Annotations

        [Test]
        public void GetUsersAnnotationsPrivilege()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var usId = new Guid("abd2f88a-8861-e111-8e23-14feb5f06078");

            var privilege = vaultApi.DocumentViewer.GetUserAnnotationPermissions(usId, "Signature");

            Assert.Greater(privilege, 0);
        }

        [Test]
        public void GetDocumentAnnotations()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dhId = new Guid("3f38898c-3278-e511-82ab-5cf3706c36ed");

            var data = vaultApi.DocumentViewer.GetDocumentAnnotationsByLayer(dhId, "Signature");

            Assert.IsNotNull(data);
        }

        [Test]
        public void GetAnnotationLayers()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var annotationLayers = vaultApi.DocumentViewer.GetAnnotationLayers();
        }

        [Test]
        public void GetAnnotationLayersWithPrivileges()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var annotationLayers = vaultApi.DocumentViewer.GetAnnotationLayersWithPrivileges(new Guid(_ResourceOwnerUsID));
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




            //
            //{
            //    
            //    
            //    
            //    
            //    
            //    
            //    
            //    
            //};



            //var vaultApi = new VaultApi(this);
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
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "Department";
            var queryId = new Guid("AEB2F858-7B96-E111-972B-14FEB5F06078");

            var results = vaultApi.CustomQueryManager.GetCustomQueryResults(queryName);

            var count = results;
        }

        [Test]
        public void GetCustomQueryByQueryId()
        {
            var vaultApi = new VaultApi(this);

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
            Assert.IsNotEmpty(request);
        }

        [Test]
        public void CallScheduledProcessCompleteUsingQueryString()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var spToken = new Guid("C1647446-4417-4341-B1F2-D82FAAEE20EA");

            vaultApi.ScheduledProcess.CallCompleteScheduledProcessUsingQueryString(spToken, "Test Message", true);

            //Assert.IsNotNull(data);
        }

        [Test]
        public void CallScheduledProcessCompleteUsingPostedData()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var spToken = new Guid("C1647446-4417-4341-B1F2-D82FAAEE20EA");

            vaultApi.ScheduledProcess.CallCompleteScheduledProcessUsingPostedData(spToken, "Test Message", true);

            //Assert.IsNotNull(data);
        }

        [Test]
        public void RunScheduledProcesses()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            vaultApi.ScheduledProcess.RunScheduledProcesses();

            //Assert.IsNotNull(data);
        }


        [Test]
        public void PersistedDataTest()
        {
            VaultApi vaultApi = new VaultApi(this);

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

            var user = vaultApi.Users.GetUser(_ResourceOwnerUserName);

            if (user != null)
            {
                string loginToken = user.GetWebLoginToken();

                Assert.IsNotEmpty(loginToken);

                string url = string.Format("{0}/v1/en/Customer412/Main/vvlogin?token={1}&returnurl=userportal%3fportalname={2}%26persistedId={3}", _VaultApiBaseUrl, loginToken, "InvoiceData", data.Id);

                LogEventManager.Info(string.Format("Token login URL with Persisted Data Id query string parameter:{0} {1}", Environment.NewLine, url));
            }

        }

        [Test]
        public void CreateCustomerTest()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var newCustomer = vaultApi.Customer.CreateCustomer("Customer2", "Customer2", "Main", "Customer2.Admin", "p",
                                             "username@company.com", 1, 5, true);

            Assert.IsNotNull(newCustomer);
        }

        [Test]
        public void GetApiObjectFieldNames()
        {
            VaultApi vaultApi = new VaultApi(this);

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
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dbInfo = vaultApi.Customer.GetCustomerDatabaseInfo();

            Assert.IsNotNull(dbInfo);

        }

        [Test]
        public void GetCustomerDatabaseConfigInfo()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var databaseConfiguration = vaultApi.ConfigurationManager.GetDatabaseConfiguration();

            try
            {
                if (databaseConfiguration != null)
                {
                    foreach (ContentProvider provider in databaseConfiguration.ContentProviders)
                    {
                        switch (provider.ContentProviderType)
                        {
                            case ContentProviderType.AwsS3Provider:
                                AwsS3Provider s3Provider = new AwsS3Provider(provider);
                                break;
                            case ContentProviderType.FileSystemProvider:
                                FileSystemProvider fileSystemProvider = new FileSystemProvider(provider);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(databaseConfiguration);
            }

            Assert.IsNotNull(databaseConfiguration);

        }

        #endregion

    }
}