// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginTests.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// <summary>
//   The login tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using NUnit.Framework;

namespace VVRestApiTests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VVRestApi.Common;
    using VVRestApi.Common.Sorting;
    using VVRestApi.Objects.DTO;
    using VVRestApi.Objects.Models;
    using VVRestApi.Vault;

    /// <summary>
    ///     The login tests.
    /// </summary>
    [TestFixture]
    public class ObjectsApiTests : IClientSecrets
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
        const string _CustomerAlias = "";

        //your customer database alias value.  Visisble in the URL when you log into VisualVault
        const string _DatabaseAlias = "";

        //Copy "API Key" value from User Account Property Screen
        const string _ClientId = "";


        //Copy "API Secret" value from User Account Property Screen
        const string _ClientSecret = "";


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

        /// <summary>
        /// Audience is used to identify known applications. If unsure of the audience, leave blank
        /// </summary>
        const string _Audience = "";

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

        public string Audience
        {
            get { return _Audience; }
        }

        #endregion

        #region Tests Objects

        [Test]
        public void GetObject()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var objectId = Guid.Parse("bcce2d62-a8c6-43b1-ad30-4f3aecc8782a");

            var objectResponse = vaultApi.ObjectsApi.Objects.GetObject(objectId);
            Assert.IsNotNull(objectResponse);
            Assert.AreEqual(objectId, objectResponse.Id);
        }

        [Test]
        public void CreateObject()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var createObject = new ObjectCreateRequest
            {
                ModelId = Guid.Parse("21878852-4538-43d0-8184-df1f9cb94a8a"),
                Properties = new Dictionary<string, object>
                {
                    { "4bf71032-f0c3-47f2-887d-ecc1d73e2458", "German" },
                    { "495bf35c-ff9f-450f-b200-d68834d04217", 30 }
                }
            };

            var objectResponse = vaultApi.ObjectsApi.Objects.CreateObject(createObject);
            Assert.IsNotNull(objectResponse);
            Assert.AreEqual(createObject.ModelId, objectResponse.ModelId);
        }

        [Test]
        public void UpdateObject()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var objectId = Guid.Parse("2c736212-f327-4f28-8225-be8d950b58ec");

            var updateObject = new ObjectUpdateRequest
            {
                RevisionId = Guid.Parse("0963ef30-307c-461a-a02e-8e31aff6bdf0"),
                Properties = new Dictionary<string, object>
                {
                    { "4bf71032-f0c3-47f2-887d-ecc1d73e2458", "Hernan" }
                }
            };

            var objectResponse = vaultApi.ObjectsApi.Objects.UpdateObject(objectId, updateObject);
            Assert.IsNotNull(objectResponse);

            objectResponse.Properties.TryGetValue("4bf71032-f0c3-47f2-887d-ecc1d73e2458", out var propertyChanged);
            Assert.AreEqual(propertyChanged, "Hernan");
        }

        [Test]
        public void GetObjectsByModelId()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var modelId = Guid.Parse("21878852-4538-43d0-8184-df1f9cb94a8a");

            var searchRequest = new ObjectSearchRequest
            {
                Page = 0,
                Take = 10,
                Sort = Enumerable.Empty<SortCriteria>(),
                CriteriaList = Enumerable.Empty<ObjectCriteriaListItem>(),
                PropertyList = new List<string>
                {
                    "4bf71032-f0c3-47f2-887d-ecc1d73e2458"
                }
            };

            var objectResponse = vaultApi.ObjectsApi.Objects.GetObjectsByModelId(modelId, searchRequest);

            Assert.IsNotNull(objectResponse);
            Assert.IsNotNull(objectResponse.Result);
            Assert.IsTrue(objectResponse.Result.Any());
        }

        [Test]
        public void DeleteObject()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var objectId = Guid.Parse("c92bb516-a371-4d31-b523-9b4a4c82dd16");

            var objectResponse = vaultApi.ObjectsApi.Objects.DeleteObject(objectId);
            Assert.IsNotNull(objectResponse);
            Assert.IsTrue(objectResponse);
        }

        #endregion

        #region Tests Models

        [Test]
        public void GetModels()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var models = vaultApi.ObjectsApi.Models.GetModels();
            Assert.IsNotNull(models);
            Assert.IsTrue(models.Any());
        }

        [Test]
        public void GetModel()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var modelId = Guid.Parse("4f8917c3-de2e-41d8-9790-e81db1f8da49");

            var model = vaultApi.ObjectsApi.Models.GetModel(modelId);
            Assert.IsNotNull(model);
            Assert.AreEqual(modelId, model.Id);
        }

        #endregion
    }
}