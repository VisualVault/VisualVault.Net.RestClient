namespace VVRestApi.Vault.PersistedData
{
    /// <summary>
    /// Helps to determine access to client data
    /// </summary>
    public enum ScopeType
    {
        /// <summary>
        /// Data only accessible by the creator
        /// </summary>
        User = 0,

        /// <summary>
        /// Data accessible by anyone
        /// </summary>
        Global = 1,
    }
}