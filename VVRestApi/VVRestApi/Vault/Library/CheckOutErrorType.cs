namespace VVRestApi.Vault.Library
{
    public enum CheckOutErrorType
    {
        None,
        CheckedOutRev,
        PendingRev,
        InsufficientSecurity,
        UnknownException,
        AnotherRevisionCheckedOut,
        ReadOnlyFolder,
        DocumentIsDeleted
    }
}