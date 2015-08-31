using System;
using System.Collections.Generic;
using System.Dynamic;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Library
{
    public class IndexFieldManager : VVRestApi.Common.BaseApi
    {
        internal IndexFieldManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }


        public List<IndexFieldDefinition> GetIndexFields(RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.GetListResult<IndexFieldDefinition>(VVRestApi.GlobalConfiguration.Routes.IndexFields, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        public IndexFieldDefinition CreateIndexField(string label, string description, FolderIndexFieldType fieldType, Guid dropdownListId, Guid queryId, string queryDisplayField, string queryValueField, bool required, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException("label is required but was an empty string", "label");
            }

            dynamic postData = new ExpandoObject();
            postData.label = label;

            if (!String.IsNullOrWhiteSpace(description))
            {
                postData.Description = description;
            }

            postData.fieldType = fieldType;
            postData.dropdownListId = dropdownListId;
            postData.queryId = queryId;

            if (!String.IsNullOrWhiteSpace(queryValueField))
            {
                postData.queryValueField = queryValueField;
            }
            if (!String.IsNullOrWhiteSpace(queryDisplayField))
            {
                postData.queryDisplayField = queryDisplayField;
            }
            postData.required = required;

            if (!String.IsNullOrWhiteSpace(defaultValue))
            {
                postData.defaultValue = defaultValue;
            }

            return HttpHelper.Post<IndexFieldDefinition>(GlobalConfiguration.Routes.IndexFields, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
        }

        public IndexFieldDefinition UpdateIndexField(Guid id, string label, string description, Guid dropdownListId, Guid queryId, string queryDisplayField, string queryValueField, bool required, string defaultValue)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentException("id is required but was an empty Guid", "id");
            }

            dynamic postData = new ExpandoObject();

            if (!String.IsNullOrWhiteSpace(label))
            {
                postData.label = label;
            }
            if (!String.IsNullOrWhiteSpace(description))
            {
                postData.Description = description;
            }

            postData.dropdownListId = dropdownListId;
            postData.queryId = queryId;

            if (!String.IsNullOrWhiteSpace(queryValueField))
            {
                postData.queryValueField = queryValueField;
            }
            if (!String.IsNullOrWhiteSpace(queryDisplayField))
            {
                postData.queryDisplayField = queryDisplayField;
            }
            postData.required = required;

            if (!String.IsNullOrWhiteSpace(defaultValue))
            {
                postData.defaultValue = defaultValue;
            }

            return HttpHelper.Put<DocumentIndexField>(VVRestApi.GlobalConfiguration.Routes.IndexFieldsId, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, id);
        }

        public IndexFieldDefinition UpdateIndexFieldLabel(Guid id, string label)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentException("id is required but was an empty Guid", "id");
            }

            dynamic postData = new ExpandoObject();

            if (!String.IsNullOrWhiteSpace(label))
            {
                postData.label = label;
            }

            return HttpHelper.Put<DocumentIndexField>(VVRestApi.GlobalConfiguration.Routes.IndexFieldsId, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, id);
        }

        public IndexFieldDefinition UpdateIndexFieldDescription(Guid id, string description)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentException("id is required but was an empty Guid", "id");
            }

            dynamic postData = new ExpandoObject();

            if (!String.IsNullOrWhiteSpace(description))
            {
                postData.Description = description;
            }


            return HttpHelper.Put<DocumentIndexField>(VVRestApi.GlobalConfiguration.Routes.IndexFieldsId, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, id);
        }

        public IndexFieldDefinition UpdateIndexFieldDropDownListId(Guid id, Guid dropdownListId)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentException("id is required but was an empty Guid", "id");
            }

            dynamic postData = new ExpandoObject();

            postData.dropdownListId = dropdownListId;

            return HttpHelper.Put<DocumentIndexField>(VVRestApi.GlobalConfiguration.Routes.IndexFieldsId, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, id);
        }

        public IndexFieldDefinition UpdateIndexFieldQueryId(Guid id, Guid queryId)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentException("id is required but was an empty Guid", "id");
            }

            dynamic postData = new ExpandoObject();

            postData.queryId = queryId;

            return HttpHelper.Put<DocumentIndexField>(VVRestApi.GlobalConfiguration.Routes.IndexFieldsId, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, id);
        }

        public IndexFieldDefinition UpdateIndexFieldRequired(Guid id, bool required)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentException("id is required but was an empty Guid", "id");
            }

            dynamic postData = new ExpandoObject();

            postData.required = required;

            return HttpHelper.Put<DocumentIndexField>(VVRestApi.GlobalConfiguration.Routes.IndexFieldsId, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, id);
        }

        public IndexFieldDefinition UpdateIndexFieldDefaultValue(Guid id, string defaultValue)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentException("id is required but was an empty Guid", "id");
            }

            dynamic postData = new ExpandoObject();

            if (!String.IsNullOrWhiteSpace(defaultValue))
            {
                postData.defaultValue = defaultValue;
            }
            else
            {
                postData.defaultValue = "";
            }

            return HttpHelper.Put<DocumentIndexField>(VVRestApi.GlobalConfiguration.Routes.IndexFieldsId, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, id);
        }

    }
}