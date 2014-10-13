namespace VVRestApiCOM
{
    public interface IComLibrary
    {
        ApiResponse GetVaultUserWebLoginToken(string userName, string apiKey, string developerSecret, string oAuthServerTokenEndPoint, string baseVaultUrl, string apiVersion, string customerAlias, string databaseAlias);

        ApiResponse StorePersistedData(string valuesInJsonFormat, int minutesToStoreData, string apiKey, string developerSecret, string oAuthServerTokenEndPoint, string baseVaultUrl, string apiVersion, string customerAlias, string databaseAlias);
    }
}
