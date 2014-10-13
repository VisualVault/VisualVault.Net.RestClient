using System;
using System.Runtime.InteropServices;
using VVRestApi.Common;
using VVRestApi.Vault;
using VVRestApi.Vault.PersistedData;

namespace VVRestApiCOM
{
    [ClassInterface(ClassInterfaceType.None)]
    public class ComLibrary : IComLibrary
    {
        //default contructor for COM compatibility
        public ComLibrary()
        {

        }

        public ApiResponse GetVaultUserWebLoginToken(string userName, string apiKey, string developerSecret, string oAuthServerTokenEndPoint, string baseVaultUrl, string apiVersion, string customerAlias, string databaseAlias)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                ClientSecrets clientSecrets = new ClientSecrets
                {
                    ApiKey = apiKey,
                    ApiSecret = developerSecret,
                    OAuthTokenEndPoint = oAuthServerTokenEndPoint,
                    BaseUrl = baseVaultUrl,
                    ApiVersion = apiVersion,
                    CustomerAlias = customerAlias,
                    DatabaseAlias = databaseAlias
                };

                VaultApi vaultApi = new VaultApi(clientSecrets);

                var user = vaultApi.Users.GetUser(userName);

                if (user != null)
                {
                    response.Value = user.GetWebLoginToken();
                }
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public ApiResponse StorePersistedData(string valuesInJsonFormat, int minutesToStoreData, string apiKey, string developerSecret, string oAuthServerTokenEndPoint, string baseVaultUrl, string apiVersion, string customerAlias, string databaseAlias)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                ClientSecrets clientSecrets = new ClientSecrets
                {
                    ApiKey = apiKey,
                    ApiSecret = developerSecret,
                    OAuthTokenEndPoint = oAuthServerTokenEndPoint,
                    BaseUrl = baseVaultUrl,
                    ApiVersion = apiVersion,
                    CustomerAlias = customerAlias,
                    DatabaseAlias = databaseAlias
                };

                VaultApi vaultApi = new VaultApi(clientSecrets);

                DateTime expirationDate = DateTime.UtcNow.AddMinutes(minutesToStoreData);

                var data = vaultApi.PersistedData.CreateData(Guid.NewGuid().ToString(), ScopeType.Global, valuesInJsonFormat, "text/JSON", string.Empty, LinkedObjectType.None, expirationDate);

                if (data != null)
                {
                    response.Value = data.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
