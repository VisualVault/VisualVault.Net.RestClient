using System;
using System.Collections.Generic;
using System.Text;
using VVRestApi.Common;
using VVRestApi.Objects.Models;

namespace VVRestApi.Objects.DTO
{
    public class GetObjectsByModelIdResponse : RestObject
    {
        public int Total { get; set; }

        public IEnumerable<ObjectModel> Result { get; set; }
    }
}
