using System;
using System.Runtime.InteropServices;
using VVRestApi;
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

        public ApiResponse GetVaultUserWebLoginToken(string userName, string developerId, string developerSecret, string baseVaultUrl, string customerAlias, string databaseAlias)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                VaultApi vaultApi = Authentication.GetVaultApi(developerId, developerId, developerSecret, baseVaultUrl, customerAlias, databaseAlias);

                if (vaultApi != null)
                {
                    var user = vaultApi.Users.GetUser(userName);

                    if (user != null)
                    {
                        response.Value = user.GetWebLoginToken();
                    }
                }
            }catch(Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public ApiResponse StorePersistedData(string valuesInJsonFormat, int minutesToStoreData, string developerId, string developerSecret, string baseVaultUrl, string customerAlias, string databaseAlias)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                VaultApi vaultApi = Authentication.GetVaultApi(developerId, developerId, developerSecret, baseVaultUrl,customerAlias, databaseAlias);

                if (vaultApi != null)
                {
                    DateTime expirationDate = DateTime.UtcNow.AddMinutes(minutesToStoreData);

                    var data = vaultApi.PersistedData.CreateData(Guid.NewGuid().ToString(), ScopeType.Global, valuesInJsonFormat, "text/JSON", string.Empty, LinkedObjectType.None, expirationDate);

                    if (data != null)
                    {
                        response.Value = data.Id.ToString();
                    }
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
