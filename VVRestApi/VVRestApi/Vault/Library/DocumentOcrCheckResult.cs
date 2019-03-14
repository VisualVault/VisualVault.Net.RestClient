using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVRestApi.Vault.Library
{
    public enum DocumentOcrCheckResult
    {
        /// <summary>
        /// OCR not needed (unspecified reason)
        /// </summary>
        OcrNotNeeded = 0,

        /// <summary>
        /// Document needs OCR
        /// </summary>
        OcrNeeded = 1,

        /// <summary>
        /// OCR not needed. Document has already been OCR'd successfully
        /// </summary>
        OcrCompleted = 2,

        /// <summary>
        /// OCR not needed. Index settings on database has it disabled.
        /// </summary>
        OcrDisabledForDatabase = 3,

        /// <summary>
        /// Cannot OCR documents that aren't the latest revision
        /// </summary>
        DocumentNotLatestRevision = 4,

        /// <summary>
        /// Could not find document
        /// </summary>
        DocumentNotFound = 5,

        /// <summary>
        /// Could not find database ocr settings
        /// </summary>
        DatabaseIndexSettingsNotFound = 6,

        /// <summary>
        /// OCR not needed. OCR has been attempted and failed, and retries are not allowed
        /// </summary>
        OcrFailedNoRetry = 7,

        /// <summary>
        /// OCR not needed. Current document is the result of an OCR process.
        /// </summary>
        IsOcrDocument = 8,

        /// <summary>
        /// OCR not needed. Document is not in a folder configured to be OCR'ed.
        /// </summary>
        DocumentNotInOcrFolder = 9,

        /// <summary>
        /// OCR not needed. File type is not configured to be OCR'ed.
        /// </summary>
        OcrNotNeededForFileType = 10
    }
}
