// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalConfiguration.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace VVRestApi
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public static class GlobalConfiguration
    {
        #region Static Fields

        private static string adminApiJwtIssuer = "self:user";

        private static string vaultApiJwtIssuer = "self:user";

        private static int defaultPageSize = 200;

        #endregion

        #region Public Properties

        /// <summary>
        /// The default page size to return for paged results
        /// </summary>
        public static int DefaultPageSize
        {
            get
            {
                return defaultPageSize;
            }
            set
            {
                defaultPageSize = value;
            }
        }

        /// <summary>
        ///     Allows you to set the JWT issuer that is used by the GetAdministrationApi method
        /// </summary>
        public static string AdminApiJwtIssuer
        {
            get
            {
                return adminApiJwtIssuer;
            }

            set
            {
                adminApiJwtIssuer = value;
            }
        }

        /// <summary>
        ///     If set to true, every request will send over 'suppress_response_codes=true' on the query string which will result in a 200 response even when there are errors
        /// </summary>
        public static bool SuppressResponseCodes { get; set; }

        /// <summary>
        ///     Allows you to set the JWT issuer that is used by the GetVaultApi method
        /// </summary>
        public static string VaultApiJwtIssuer
        {
            get
            {
                return vaultApiJwtIssuer;
            }

            set
            {
                vaultApiJwtIssuer = value;
            }
        }

        #endregion

        #region Properties

        private static JsonSerializerSettings DefaultSerializerSettings { get; set; }

        #endregion

        #region Methods

        internal static JsonSerializerSettings GetJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented, ContractResolver = new CamelCasePropertyNamesContractResolver(), DefaultValueHandling = DefaultValueHandling.Ignore };
            if (settings.Converters.Count == 0)
            {
                settings.Converters.Add(new ExpandoObjectConverter());
            }

            return settings;
        }

        #endregion

        /// <summary>
        ///     A list of the available REST API routes
        /// </summary>
        public static class Routes
        {
            #region Servers

            /// <summary>
            ///     ~/Servers/
            /// </summary>
            public const string Servers = "~/Servers/";

            /// <summary>
            ///     ~/Servers/{0:id}
            /// </summary>
            public const string ServersId = "~/Servers/{0}/";

            /// <summary>
            ///     ~/Servers/{0:id}/{1:action}
            /// </summary>
            public const string ServersIdAction = "~/Servers/{0}/{1}/";

            /// <summary>
            ///     ~/Servers/{0:id}/{1:action}/{2:childid}
            /// </summary>
            public const string ServersIdActionId = "~/Servers/{0}/{1}/{2}/";

            #endregion

            #region Licenses

            /// <summary>
            ///     ~/Licenses/
            /// </summary>
            public const string Licenses = "~/Licenses/";

            /// <summary>
            ///     ~/Licenses/{0:id}
            /// </summary>
            public const string LicensesId = "~/Licenses/{0}/";

            /// <summary>
            ///     ~/Licenses/{0:id}/{1:action}
            /// </summary>
            public const string LicensesIdAction = "~/Licenses/{0}/{1}/";

            /// <summary>
            ///     ~/Licenses/{0:id}/{1:action}/{2:childid}
            /// </summary>
            public const string LicensesIdActionId = "~/Licenses/{0}/{1}/{2}/";

            #endregion

            #region Customers

            /// <summary>
            ///     ~/Customers/
            /// </summary>
            public const string Customers = "~/Customers/";

            /// <summary>
            ///     ~/Customers/{0:id}
            /// </summary>
            public const string CustomersId = "~/Customers/{0}/";

            /// <summary>
            ///     ~/Customers/{0:id}/{1:action}
            /// </summary>
            public const string CustomersIdAction = "~/Customers/{0}/{1}/";

            /// <summary>
            ///     ~/Customers/{0:id}/{1:action}/{2:childid}
            /// </summary>
            public const string CustomersIdActionId = "~/Customers/{0}/{1}/{2}/";

            #endregion

            #region Emails

            /// <summary>
            ///     ~/Emails/
            /// </summary>
            public const string Emails = "~/Emails/";

            /// <summary>
            ///     ~/Emails/{0:id}
            /// </summary>
            public const string EmailsId = "~/Emails/{0}/";

            /// <summary>
            ///     ~/Emails/{0:id}/{1:action}
            /// </summary>
            public const string EmailsIdAction = "~/Emails/{0}/{1}/";

            /// <summary>
            ///     ~/Emails/{0:id}/{1:action}/{2:childid}
            /// </summary>
            public const string EmailsIdActionId = "~/Emails/{0}/{1}/{2}/";

            #endregion

            #region Folders

            /// <summary>
            ///     ~/folders/{0:id}/documents
            /// </summary>
            public const string FolderDocuments = "~/Folders/{0}/Documents";

            /// <summary>
            ///     ~/Folders/
            /// </summary>
            public const string Folders = "~/Folders/";

            /// <summary>
            ///     ~/Folders/{0:id}
            /// </summary>
            public const string FoldersId = "~/Folders/{0}/";

            /// <summary>
            ///     ~/Folders/{0:id}/folders
            /// </summary>
            public const string FoldersIdFolders = "~/Folders/{0}/Folders";

            /// <summary>
            ///     ~/Folders/{0:id}/indexfields
            /// </summary>
            public const string FoldersIndexFields= "~/Folders/{0}/Indexfields";

            /// <summary>
            ///     ~/Folders/{0:id}/indexfields/{1:childid}
            /// </summary>
            public const string FoldersIdIndexFieldsId = "~/Folders/{0}/Indexfields/{1}";

            /// <summary>
            ///     ~/Folders/{0:id}/{1:action}
            /// </summary>
            public const string FoldersIdAction = "~/Folders/{0}/{1}/";

            /// <summary>
            ///     ~/Folders/{0:id}/{1:action}/{2:childid}
            /// </summary>
            public const string FoldersIdActionId = "~/Folders/{0}/{1}/{2}/";

            /// <summary>
            ///     ~/Folders/{0:id}/indexfields/{1:childid}
            /// </summary>
            public const string FoldersIdIndexFieldsIdSelectOptions = "~/folders/{0}/indexfields/{1}/selectOptions";

            #endregion

            #region Files

            /// <summary>
            ///     ~/Files/
            /// </summary>
            public const string Files = "~/Files/";

            /// <summary>
            ///     ~/Files/{0:id}
            /// </summary>
            public const string FilesId = "~/Files/{0}/";

            /// <summary>
            ///     ~/Files/File/
            /// </summary>
            public const string FilesNoFileAllowed = "~/Files/File/";

            #endregion

            #region FormTemplates

            /// <summary>
            ///     ~/FormTemplates/
            /// </summary>
            public const string FormTemplates = "~/FormTemplates/";

            /// <summary>
            ///     ~/formtemplates/{0:id}/forms
            /// </summary>
            public const string FormTemplatesForms = "~/formtemplates/{0}/forms";

            /// <summary>
            ///     ~/formtemplates/{0:id}/forms
            /// </summary>
            public const string FormTemplatesFormsId = "~/formtemplates/{0}/forms/{1}";

            /// <summary>
            ///     ~/FormTemplates/{0:id}
            /// </summary>
            public const string FormTemplatesId = "~/FormTemplates/{0}/";

            /// <summary>
            ///     ~/FormTemplates/{0:id}/{1:action}
            /// </summary>
            public const string FormTemplatesIdAction = "~/FormTemplates/{0}/{1}/";

            /// <summary>
            ///     ~/FormTemplates/{0:id}/{1:action}/{2:childid}
            /// </summary>
            public const string FormTemplatesIdActionId = "~/FormTemplates/{0}/{1}/{2}/";

            #endregion

            #region Sites

            /// <summary>
            ///     ~/Sites/
            /// </summary>
            public const string Sites = "~/Sites/";

            /// <summary>
            ///     ~/sites/{0:id}/groups
            /// </summary>
            public const string SitesGroups = "~/sites/{0}/groups";

            /// <summary>
            ///     ~/Sites/{0:id}
            /// </summary>
            public const string SitesId = "~/Sites/{0}/";

            /// <summary>
            ///     ~/Sites/{0:id}/{1:action}
            /// </summary>
            public const string SitesIdAction = "~/Sites/{0}/{1}/";

            /// <summary>
            ///     ~/Sites/{0:id}/{1:action}/{2:childid}
            /// </summary>
            public const string SitesIdActionId = "~/Sites/{0}/{1}/{2}/";

            /// <summary>
            ///     ~/sites/{0:id}/users
            /// </summary>
            public const string SitesUsers = "~/sites/{0}/users";

            #endregion

            #region Users

            /// <summary>
            ///     ~/Users/
            /// </summary>
            public const string Users = "~/Users/";

            /// <summary>
            ///     ~/Users/{0:id}
            /// </summary>
            public const string UsersId = "~/Users/{0}/";

            /// <summary>
            ///     ~/Users/{0:id}/{1:action}
            /// </summary>
            public const string UsersIdAction = "~/Users/{0}/{1}/";

            /// <summary>
            ///     ~/Users/{0:id}/{1:action}/{2:childid}
            /// </summary>
            public const string UsersIdActionId = "~/Users/{0}/{1}/{2}/";

            #endregion

            #region Meta

            /// <summary>
            ///     ~/Meta/
            /// </summary>
            public const string Meta = "~/Meta/";

            /// <summary>
            ///     ~/Meta/{0:id}
            /// </summary>
            public const string MetaId = "~/Meta/{0}/";

            /// <summary>
            ///     ~/Meta/{0:id}/{1:action}
            /// </summary>
            public const string MetaIdAction = "~/Meta/{0}/{1}/";

            /// <summary>
            ///     ~/Meta/{0:id}/{1:action}/{2:childid}
            /// </summary>
            public const string MetaIdActionId = "~/Meta/{0}/{1}/{2}/";

            #endregion

            #region Groups

            /// <summary>
            ///     ~/Groups/
            /// </summary>
            public const string Groups = "~/Groups/";

            /// <summary>
            ///     ~/Groups/{0:id}
            /// </summary>
            public const string GroupsId = "~/Groups/{0}/";

            /// <summary>
            ///     ~/Groups/{0:id}/users
            /// </summary>
            public const string GroupsIdUsers = "~/Groups/{0}/users";

            /// <summary>
            ///     ~/Groups/{0:id}/{1:action}
            /// </summary>
            public const string GroupsIdAction = "~/Groups/{0}/{1}/";

            /// <summary>
            ///     ~/Groups/{0:id}/{1:action}/{2:childid}
            /// </summary>
            public const string GroupsIdActionId = "~/Groups/{0}/{1}/{2}/";

            #endregion

            #region PersistedData

            /// <summary>
            ///     ~/PersistedData/
            /// </summary>
            public const string PersistedData = "~/PersistedData/";

            /// <summary>
            ///     ~/PersistedData/{0:id}
            /// </summary>
            public const string PersistedDataId = "~/PersistedData/{0}/";

            /// <summary>
            ///     ~/PersistedData/{0:id}/{1:action}
            /// </summary>
            public const string PersistedDataIdAction = "~/PersistedData/{0}/{1}/";

            /// <summary>
            ///     ~/PersistedData/{0:id}/{1:action}/{2:childid}
            /// </summary>
            public const string PersistedDataIdActionId = "~/PersistedData/{0}/{1}/{2}/";

            #endregion

            #region Documents

            /// <summary>
            ///     ~/Documents/
            /// </summary>
            public const string Documents = "~/Documents/";

            /// <summary>
            ///     ~/Documents/{0:id}
            /// </summary>
            public const string DocumentsId = "~/Documents/{0}";

            /// <summary>
            ///     ~/Documents/{0:id}/revisions
            /// </summary>
            public const string DocumentsRevisions = "~/Documents/{0}/Revisions";

            /// <summary>
            ///     ~/Documents/{0:id}/revisions/{1:childid}
            /// </summary>
            public const string DocumentsRevisionsId = "~/Documents/{0}/Revisions/{1}";

            /// <summary>
            ///     ~/Documents/{0:id}/revisions/{1:childid}/indexfields
            /// </summary>
            public const string DocumentsRevisionsIdIndexFields = "~/Documents/{0}/Revisions/{1}/Indexfields";

            /// <summary>
            ///     ~/Documents/{0:id}/revisions/{1:childid}/indexfields/{2}
            /// </summary>
            public const string DocumentsRevisionsIdIndexFieldsId = "~/Documents/{0}/Revisions/{1}/Indexfields/{2}";

            /// <summary>
            ///     ~/Documents/{0:id}/indexfields
            /// </summary>
            public const string DocumentsIndexFields = "~/Documents/{0}/Indexfields";

            /// <summary>
            ///     ~/Documents/{0:id}/indexfields/{1:childid}
            /// </summary>
            public const string DocumentsIndexFieldsId = "~/documents/{0}/indexfields/{1}";

            #endregion
            
            #region IndexFields

            /// <summary>
            ///     ~/IndexFields/
            /// </summary>
            public const string IndexFields = "~/IndexFields/";

            /// <summary>
            ///     ~/IndexFields/
            /// </summary>
            public const string IndexFieldsId = "~/IndexFields/{0}";

            /// <summary>
            ///     ~/IndexFields/
            /// </summary>
            public const string IndexFieldsIdFoldersId = "~/IndexFields/{0}/folders/{1}";

            #endregion


        }
    }
}