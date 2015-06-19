namespace VVRestApi.Vault.Library
{
    public enum CheckInStatusType
    {
        CheckedIn,
        DuplicateRevision,
        DocumentAlreadyCheckedIn,
        UnknownException,
        DocumentAlreadyCheckedOut,
    }
}