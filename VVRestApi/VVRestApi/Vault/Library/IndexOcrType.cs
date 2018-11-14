using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVRestApi.Vault.Library
{
    public enum IndexOcrType
    {
        None = 0,
        OcrOnly = 1,
        OcrCheckInNewRev = 2,
        OcrCheckInReplace = 3
    }
}
