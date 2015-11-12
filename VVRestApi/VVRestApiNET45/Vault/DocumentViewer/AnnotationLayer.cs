using VVRestApi.Common;

namespace VVRestApi.Vault.DocumentViewer
{
    public class AnnotationLayer : RestObject
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
    public class AnnotationLayerAndPermission : RestObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Privilege { get; set; }
    }
}