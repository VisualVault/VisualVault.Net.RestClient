using VVRestApi.Common;

namespace VVRestApi.Documents
{
    public class DocumentOcrStatusUpdateResponse : RestObject
    {
        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }
    }
}
