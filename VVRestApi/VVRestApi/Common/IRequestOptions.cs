using System;
using System.Collections.Generic;
using System.Text;

namespace VVRestApi.Common
{
    public interface IRequestOptions
    {
        void PrepForRequest();
        string GetQueryString(string query);
    }
}
