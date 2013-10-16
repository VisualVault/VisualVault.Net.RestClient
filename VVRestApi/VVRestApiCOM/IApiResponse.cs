

namespace VVRestApiCOM
{
    public interface IApiResponse
    {
        string Value { get; set; }

        bool IsError { get; set; }

        string ErrorMessage { get; set; }
    }
}
