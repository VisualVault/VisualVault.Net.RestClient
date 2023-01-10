using NUnit.Framework;
using System;
using System.Linq;
using VVRestApi.Common;
using VVRestApi.Vault;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class FolderIndexFieldTests : TestBase
    {
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
    }
}
