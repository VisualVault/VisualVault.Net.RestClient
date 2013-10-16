using System.Runtime.InteropServices;

namespace VVRestApiCOM
{
    [ClassInterface(ClassInterfaceType.None)] 
    public class ApiResponse :IApiResponse
    {
        public ApiResponse()
        {
            
        }

        public string Value { get; set; }

        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }
    }
}
