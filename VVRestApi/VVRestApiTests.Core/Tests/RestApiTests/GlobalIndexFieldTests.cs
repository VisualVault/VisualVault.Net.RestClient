using NUnit.Framework;
using System;
using VVRestApi.Common;
using VVRestApi.Vault;
using VVRestApi.Vault.Library;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class GlobalIndexFieldTests : TestBase
    {

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

    }
}
