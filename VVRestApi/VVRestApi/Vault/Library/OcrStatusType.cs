using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVRestApi.Vault.Library
{
    public enum OcrStatusType
    {
        /// <summary>
        /// No OCR has been performed
        /// </summary>
        None = 0,
        /// <summary>
        /// OCR was performed on this revision
        /// </summary>
        Success = 1,
        /// <summary>
        /// OCR was performed, but no text was extracted
        /// </summary>
        SuccessNoTextExtracted = 2,
        /// <summary>
        /// Failed to OCR
        /// </summary>
        Failure = 3,
        /// <summary>
        /// Failed to OCR
        /// </summary>
        FailureNoRetry = 4,
        /// <summary>
        /// The current document is the result of a successful OCR
        /// </summary>
        ResultingDocumentFromSuccess = 5
    }
}