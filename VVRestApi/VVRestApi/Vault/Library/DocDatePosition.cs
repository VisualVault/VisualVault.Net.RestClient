namespace VVRestApi.Vault.Library
{
    public enum DocDatePosition
    {
        NoDateInsert = 0,
        InsertDateBeforePrefix = 1,
        InsertDateBeforeSequence = 2,
        InsertDateBeforeSuffix = 3,
        InsertDateAfterSuffix = 4
    }
}