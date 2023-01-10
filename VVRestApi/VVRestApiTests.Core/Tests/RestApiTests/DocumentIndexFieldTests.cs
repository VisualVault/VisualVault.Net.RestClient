using NUnit.Framework;
using System;
using System.Collections.Generic;
using VVRestApi.Common;
using VVRestApi.Vault;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class DocumentIndexFieldTests : TestBase
    {

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

    }
}
