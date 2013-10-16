using System;

namespace VVRestApiCOM
{
    public interface IComLibrary
    {
        ApiResponse GetVaultUserWebLoginToken(string userName, string developerId, string developerSecret, string baseVaultUrl, string customerAlias, string databaseAlias);

        ApiResponse StorePersistedData(string valuesInJsonFormat, int minutesToStoreData, string developerId, string developerSecret, string baseVaultUrl, string customerAlias, string databaseAlias);

    }
}
