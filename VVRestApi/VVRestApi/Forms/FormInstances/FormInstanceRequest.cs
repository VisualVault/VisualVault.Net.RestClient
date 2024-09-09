using System;
using System.Collections.Generic;
using System.Text;

namespace VVRestApi.Forms.FormInstances
{
    /// <summary>
    /// Used to create a new form instance via the Forms Api
    /// </summary>
    public class FormInstanceRequest
    {
        /// <summary>
        /// The ID of the FormTemplate for the new instance
        /// </summary>
        public Guid FormTemplateId { get; set; }

        /// <summary>
        /// Collection of form values (Key is either Name or Id of the field. Value is the string value. Props is a collection of extra fields to be saved)
        /// </summary>
        public List<KeyValuePair<string, object>> Fields { get; set; }
    }

    /// Used to create a new form instance revision via the Forms Api
    public class UpdateFormInstanceRequest
    {
        /// <summary>
        /// The ID of the FormTemplate for the new instance
        /// </summary>
        public Guid FormTemplateId { get; set; }

        /// <summary>
        /// The revision ID of the form being updated
        /// </summary>
        public Guid FormId { get; set; }

        /// <summary>
        /// Collection of form values (Key is either Name or Id of the field. Value is the string value. Props is a collection of extra fields to be saved)
        /// </summary>
        public List<KeyValuePair<string, object>> Fields { get; set; }

        public bool ReplaceRevision { get; set; }
    }
}
