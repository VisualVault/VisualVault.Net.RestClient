using System;
using VVRestApi.Common;

namespace VVRestApi.Vault.DocumentViewer
{
    public class DocumentViewerAnnotation : RestObject
    {
        public Guid DhId { get; set; }

        public string LayerName { get; set; }

        public Byte[] Annotations { get; set; }
    }
}