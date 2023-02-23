using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VVRestApi.Common;
using VVRestApi.Vault;
using VVRestApi.Vault.Forms;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class FormTests : TestBase
    {
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

            var formTemplateId = new Guid("170c3575-df2a-ec11-a845-9c2976cf2d89");

            var options = new RequestOptions
            {
                Fields = "html"
            };
            //var options = new RequestOptions
            //{
            //    Expand = true
            //};

            var formId = new Guid("8e383521-28df-4abd-afdc-91b00072f53e");

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
    }
}
