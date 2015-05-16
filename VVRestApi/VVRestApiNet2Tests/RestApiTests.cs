using System;
using System.IO;
using NUnit.Framework;
using VVRestAPINet2.Common;
using VVRestAPINet2.Vault;
using Assert = NUnit.Framework.Assert;

namespace VVRestApiNet2Tests
{
    [TestFixture]
    public class RestApiTests
    {
        #region Constants

        //Base URL to VisualVault.  Copy URL string preceding the version number ("/v1")
        const string VaultApiBaseUrl = "http://localhost/VisualVault4_1_10";

        //API version number (number following /v in the URL).  Used to provide backward compatitiblity.
        const string ApiVersion = "1";

        //OAuth2 token endpoint, exchange credentials for api access token
        //typically the VaultApiBaseUrl + /oauth/token unless using an external OAuth server
        private const string OAuthServerTokenEndPoint = "http://localhost/VisualVault4_1_10/oauth/token";

        //your customer alias value.  Visisble in the URL when you log into VisualVault
        const string CustomerAlias = "Customer412";

        //your customer database alias value.  Visisble in the URL when you log into VisualVault
        const string DatabaseAlias = "Main";

        //Copy "API Key" value from User Account Property Screen
        const string ClientId = "6abdcfa4-c45a-48d9-b21f-b8893c189cf9";

        //Copy "API Secret" value from User Account Property Screen
        const string ClientSecret = "SqjSsY2I4bDU0E5tj9HgHYxLV0ClJJCFf75ckg3fRKU=";

        // Scope is used to determine what resource types will be available after authentication.  If unsure of the scope to provide use
        // either 'vault' or no value.  'vault' scope is used to request access to a specific customer vault (aka customer database). 
        const string Scope = "vault";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string ResourceOwnerUserName = "vault.config";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string ResourceOwnerPassword = "p";

        #endregion

        [Test]
        public void ClientCredentialsGrantType_LoginTest()
        {
            try
            {
                ClientSecrets clientSecrets = new ClientSecrets
                                              {
                                                  ApiKey = RestApiTests.ClientId,
                                                  ApiSecret = RestApiTests.ClientSecret,
                                                  OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                                                  BaseUrl = RestApiTests.VaultApiBaseUrl,
                                                  ApiVersion = RestApiTests.ApiVersion,
                                                  CustomerAlias = RestApiTests.CustomerAlias,
                                                  DatabaseAlias = RestApiTests.DatabaseAlias,
                                                  Scope = RestApiTests.Scope
                                              };

                VaultApi vaultApi = new VaultApi(clientSecrets);

                Assert.IsNotNull(vaultApi);
            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
        }

        [Test]
        public void GetDocumentRevisionFile()
        {
            try
            {
                ClientSecrets clientSecrets = new ClientSecrets
                {
                    ApiKey = RestApiTests.ClientId,
                    ApiSecret = RestApiTests.ClientSecret,
                    OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                    BaseUrl = RestApiTests.VaultApiBaseUrl,
                    ApiVersion = RestApiTests.ApiVersion,
                    CustomerAlias = RestApiTests.CustomerAlias,
                    DatabaseAlias = RestApiTests.DatabaseAlias,
                    Scope = RestApiTests.Scope
                };

                VaultApi vaultApi = new VaultApi(clientSecrets);

                Assert.IsNotNull(vaultApi);

                Guid fileId = new Guid("8d57c716-40e5-e411-beee-93df0d4ae3b6");

                string filePath = string.Format(@"C:\Users\tod.olsen\Downloads\{0}", "test2.docx");

                File.Delete(filePath);

                using (Stream stream = vaultApi.Files.GetStream(fileId))
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    int count = 0;
                    do
                    {
                        byte[] buf = new byte[102400];
                        count = stream.Read(buf, 0, 102400);
                        fs.Write(buf, 0, count);
                    } while (count > 0);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
    }
}
