// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalConfiguration.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.CodeDom;

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

            #region Configuration

            /// <summary>
            ///     ~/Configuration/
            /// </summary>
            public const string Configuration = "~/Configuration/";

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

            /// <summary>
            ///     ~/Customers/CustomerDatabases
            /// </summary>
            public const string CustomersCustomerDatabases = "~/Customers/CustomerDatabases";

            public const string CustomersOrganization = "~/customers/organization";

            public const string CustomersBilling = "~/customers/billing";

            public const string CustomersBillingAddress = "~/customers/billing/address";

            public const string CustomersBillingPlan = "~/customers/billing/plan";

            public const string CustomersAccounts = "~/customers/accounts";

            #endregion

            #region Lists

            public const string ListsSubscriptionplans = "~/Lists/Subscriptionplans";

            #endregion

            #region Annotations

            public const string Annotations = "~/Annotations/";

            #endregion

            #region DocumentViewer

            public const string DocumentViewer = "~/DocumentViewer/";
            public const string DocumentViewerAnnotations = "~/DocumentViewer/Annotations";
            public const string DocumentViewerAnnotationsId = "~/DocumentViewer/Annotations/{0}";
            public const string DocumentViewerAnnotationsLayers = "~/DocumentViewer/Annotations/Layers";
            public const string DocumentViewerAnnotationsLayersPermissionsId = "~/DocumentViewer/Annotations/Layers/Permissions/{0}";
            public const string DocumentViewerAnnotationsPermissionsId = "~/DocumentViewer/Annotations/Permissions/{0}";
            public const string DocumentViewerIdAnnotations = "~/DocumentViewer/{0}/Annotations";
            public const string DocumentViewerIdAnnotationsLayers = "~/DocumentViewer/{0}/Annotations/Layers";
            public const string DocumentViewerIdAnnotationsPkey = "~/DocumentViewer/{0}/Annotations/{1}";
            public const string Viewer = "~/Viewer";

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
            public const string Folders = "~/folders/";

            /// <summary>
            ///     ~/Folders/{0:id}
            /// </summary>
            public const string FoldersId = "~/folders/{0}/";

            /// <summary>
            ///     ~/Folders/{0:id}/folders
            /// </summary>
            public const string FoldersIdFolders = "~/folders/{0}/Folders";

            /// <summary>
            ///     ~/Folders/{0:id}/securityMembers
            /// </summary>
            public const string FoldersIdSecurityMembers = "~/folders/{0}/securitymembers";

            /// <summary>
            ///     ~/Folders/{0:id}/securityMembers/{1:childid}
            /// </summary>
            public const string FoldersIdSecurityMembersId = "~/folders/{0}/securitymembers/{1}";

            /// <summary>
            ///     ~/Folders/{0:id}/indexfields
            /// </summary>
            public const string FoldersIndexFields= "~/folders/{0}/indexfields";

            /// <summary>
            ///     ~/Folders/{0:id}/indexfields/{1:childid}
            /// </summary>
            public const string FoldersIdIndexFieldsId = "~/folders/{0}/indexfields/{1}";

            /// <summary>
            ///     ~/Folders/{0:id}/{1:action}
            /// </summary>
            public const string FoldersIdAction = "~/folders/{0}/{1}/";

            /// <summary>
            ///     ~/Folders/{0:id}/{1:action}/{2:childid}
            /// </summary>
            public const string FoldersIdActionId = "~/folders/{0}/{1}/{2}/";

            /// <summary>
            ///     ~/Folders/{0:id}/indexfields/{1:childid}
            /// </summary>
            public const string FoldersIdIndexFieldsIdSelectOptions = "~/folders/{0}/indexfields/{1}/selectOptions";

            /// <summary>
            /// ~/folders/home}
            /// </summary>
            public const string FoldersHome = "~/folders/Home";

            /// <summary>
            /// ~/Folders/home/{0:id}
            /// </summary>
            public const string FoldersHomeId = "~/folders/home/{0}";

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

            #region FormInstance

            /// <summary>
            ///     ~/formInstance/{0:id}/relateForm
            /// </summary>
            public const string FormInstanceIdRelateForm = "~/formInstance/{0}/relateForm/";

            /// <summary>
            ///     ~/formInstance/{0:id}/documents
            /// </summary>
            public const string FormInstanceIdDocuments = "~/formInstance/{0}/documents";

            /// <summary>
            ///     ~/formInstance/{0:id}/relateDocument
            /// </summary>
            public const string FormInstanceIdRelateDocument = "~/formInstance/{0}/relateDocument/";

            /// <summary>
            ///     ~/formInstance/{0:id}/relateProject
            /// </summary>
            public const string FormInstanceIdRelateProject = "~/formInstance/{0}/relateProject/";

            /// <summary>
            ///     ~/formInstance/{0:id}/relateForm
            /// </summary>
            public const string FormInstanceIdUnRelateForm = "~/formInstance/{0}/unrelateForm/";

            /// <summary>
            ///     ~/formInstance/{0:id}/relateForm
            /// </summary>
            public const string FormInstanceIdDeleteForm = "~/formInstance/{0}/";

            /// <summary>
            ///     ~/formInstance/{0:id}/relateDocument
            /// </summary>
            public const string FormInstanceIdUnRelateDocument = "~/formInstance/{0}/unrelateDocument/";

            /// <summary>
            ///     ~/formInstance/{0:id}/relateProject
            /// </summary>
            public const string FormInstanceIdUnRelateProject = "~/formInstance/{0}/unrelateProject/";

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
            public const string FormTemplatesIdForms = "~/formtemplates/{0}/forms";

            /// <summary>
            ///     ~/formtemplates/{0:id}/forms/{1:childid}
            /// </summary>
            public const string FormTemplatesFormsId = "~/formtemplates/{0}/forms/{1}";

            /// <summary>
            ///     ~/formtemplates/{0:id}/forms/{1:childid}
            /// </summary>
            public const string FormTemplatesIdFormsId = "~/formtemplates/{0}/forms/{1}";

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

            public const string UsersDefaultCustomer = "~/Users/DefaultCustomer";

            /// <summary>
            ///     ~/Users/{0:id}
            /// </summary>
            public const string UsersId = "~/Users/{0}/";

            public const string UsersIdGroups = "~/Users/{0}/groups";

            /// <summary>
            ///     ~/Users/{0:id}/{1:action}
            /// </summary>
            public const string UsersIdAction = "~/Users/{0}/{1}/";

            public const string UsersIdAnnotationPrivilege = "~/Users/{0}/annotations/privileges/";

            /// <summary>
            ///     ~/Users/{0:id}/{1:action}/{2:childid}
            /// </summary>
            public const string UsersIdActionId = "~/Users/{0}/{1}/{2}/";

            /// <summary>
            ///     ~/Users/Invites
            /// </summary>
            public const string UsersInvites = "~/Users/Invites";

            /// <summary>
            ///     ~/Users/Invites/{0:id}
            /// </summary>
            public const string UsersInvitesId = "~/Users/Invites/{0}";

            public const string Invite = "~/invite";

            public const string InviteId = "~/invite/{0}";

            /// <summary>
            ///     ~/Users/{0:id}/password
            /// </summary>
            public const string UsersIdPassword = "~/Users/{0}/password";


            /// <summary>
            ///     ~/Users/{0:id}/password
            /// </summary>
            public const string UsersResetId = "~/users/reset/{0}";

            /// <summary>
            ///     ~/Users/AccountOwner
            /// </summary>
            public const string AccountOwner = "~/Users/AccountOwner";


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
            ///     ~/Groups/{0:id}/users/{1:usId}
            /// </summary>
            public const string GroupsIdUsersId = "~/Groups/{0}/users/{1}/";

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
            ///     ~/Documents/revisions/{0:childid}
            /// </summary>
            public const string DocumentsRevisionsChildIdOnly = "~/Documents/Revisions/{0}";


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

            public const string DocumentsFavorites = "~/Documents/favorites";

            public const string DocumentsIdFavorites = "~/Documents/{0}/favorites";

            public const string DocumentsShares = "~/Documents/shares";

            public const string DocumentsIdShares = "~/Documents/{0}/shares";

            public const string DocumentsIdSignatures = "~/documents/{0}/signatures";
            
            public const string DocumentsSignaturesId = "~/documents/signatures/{0}";

            public const string DocumentsIdApprovals = "~/documents/{0}/approvals";

            public const string DocumentsApprovalsId = "~/documents/approvals/{0}";

            public const string DocumentsIdOcr = "~/documents/{0}/ocr";

            public const string DocumentsIdOcrStatus = "~/documents/{0}/ocr/status";

            public const string DocumentsIdMove = "~/documents/{0}/move";

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

            #region ScheduledProcess

            /// <summary>
            ///     ~/ScheduledProcess/
            /// </summary>
            public const string ScheduledProcessId = "~/ScheduledProcess/{0}";

            public const string ScheduledProcessRun = "~/ScheduledProcess/run";

            #endregion

            #region CustomQueries

            /// <summary>
            ///     ~/CustomQuery/
            /// </summary>
            public const string CustomQuery = "~/customquery";

            /// <summary>
            ///     ~/CustomQuery/{id}
            /// </summary>
            public const string CustomQueryId = "~/customquery/{0}";

            #endregion

            #region Version

            public const string VersionGetVersion = "~/version";

            #endregion

            #region Scripts

            public const string Scripts = "~/scripts";

            #endregion


        }
    }
}