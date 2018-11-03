using System.Xml.Schema;
using System.Xml.Serialization;

namespace VVRestApi.Vault.Configuration
{
    public class AwsS3ProviderProperties
    {
        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Path { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string BucketName { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string AccessKey { get; set; }

        /// <remarks/>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SecretKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string AwsRegionSystemName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool UseProxy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ProxyHost { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int ProxyPort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ServerSideEncryptionMethod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ServerSideEncryptionKmsKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool UseDefaultAwsKmsEncryptionKmsKey { get; set; }
    }
}
