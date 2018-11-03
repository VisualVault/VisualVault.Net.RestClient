using System.Xml.Schema;
using System.Xml.Serialization;

namespace VVRestApi.Vault.Configuration
{
    public class FileSystemProviderProperties
    {
        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Path { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string DomainName { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string UserName { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Password { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int MaxFolderDepth { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int MaxSubFolders { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int MaxFileCount { get; set; }
    }
}
