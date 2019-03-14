using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVRestApi.Vault.Library
{
    public enum OcrErrorCodeType
    {
        None = 0,
        ErrorThrown = 1,
        OcrProcessingError = 2,
        OcrOutputSaveError = 3,
        CheckinError = 4
    }
}
