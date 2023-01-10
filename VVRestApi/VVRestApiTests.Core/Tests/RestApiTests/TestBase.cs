using VVRestApi.Common;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    public class TestBase : IClientSecrets
    {

        //Base URL to VisualVault.  Copy URL string preceding the version number ("/v1")
        protected const string _VaultApiBaseUrl = "http://localhost/visualvault";

        //API version number (number following /v in the URL).  Used to provide backward compatitiblity.
        protected const string _ApiVersion = "1";

        //OAuth2 token endpoint, exchange credentials for api access token
        //typically the VaultApiBaseUrl + /oauth/token unless using an external OAuth server
        private const string _OAuthServerTokenEndPoint = "http://localhost/visualvault/oauth/token";

        //your customer alias value.  Visisble in the URL when you log into VisualVault
        protected const string _CustomerAlias = "test";

        //your customer database alias value.  Visisble in the URL when you log into VisualVault
        protected const string _DatabaseAlias = "FDTest";

        //Copy "API Key" value from User Account Property Screen
        protected const string _ClientId = "d3b6a24f-ed02-4780-9d40-ef53483453c3";


        //Copy "API Secret" value from User Account Property Screen
        protected const string _ClientSecret = "jPW5++77PT6PXZJaqaFIZu8iyM2GOkM90+yCdems12Q=";

        //VisualVault FolderStore Constants
        public const string GeneralFolderDefaultName = "General";
        public string AttachmentsFolderDefaultName = "Attachments";



        // Scope is used to determine what resource types will be available after authentication.  If unsure of the scope to provide use
        // either 'vault' or no value.  'vault' scope is used to request access to a specific customer vault (aka customer database). 
        protected const string _Scope = "app";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        protected const string _ResourceOwnerUserName = "";
        protected const string _ResourceOwnerUsID = "";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        protected const string _ResourceOwnerPassword = "";


        public string BaseUrl
        {
            get { return _VaultApiBaseUrl; }
        }
        public string ApiKey
        {
            get { return _ClientId; }
        }
        public string ApiSecret
        {
            get { return _ClientSecret; }
        }
        public string CustomerAlias
        {
            get { return _CustomerAlias; }
        }
        public string DatabaseAlias
        {
            get { return _DatabaseAlias; }
        }
        public string ApiVersion
        {
            get { return _ApiVersion; }
        }
        public string OAuthTokenEndPoint
        {
            get { return _OAuthServerTokenEndPoint; }
        }
        public string Scope
        {
            get { return _Scope; }
        }
    }
}
