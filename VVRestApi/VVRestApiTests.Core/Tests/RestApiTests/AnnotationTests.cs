using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using VVRestApi.Vault;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class AnnotationTests : TestBase
    {

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
    }
}
