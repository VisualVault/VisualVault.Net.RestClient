using NUnit.Framework;
using System;
using VVRestApi.Common;
using VVRestApi.Vault;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class FormInstanceRelationTests : TestBase
    {
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
        public void FormRelateFormByDocId()
        {
            VaultApi vaultApi = new VaultApi(this);
            var formId = new Guid("60b6f7db-f0be-e511-a698-e094676f83f7");
            string docID = "Personal-000047";
            var options = new RequestOptions { };
            vaultApi.FormInstances.RelateFormByDocId(formId, docID, options);
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
    }
}
