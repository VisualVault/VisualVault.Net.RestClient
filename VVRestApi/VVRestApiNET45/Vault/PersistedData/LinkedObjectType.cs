namespace VVRestApi.Vault.PersistedData
{
    public enum LinkedObjectType
    {
        None = 0,
        Document = 1,
        DocumentRevision = 2,
        Folder = 3,
        FormTemplate = 4,
        FormInstance = 5,
        Project = 6,
        Group = 7,
        User = 8,
        UserSession = 9,
        Portal = 10,
        Page = 11,
        Search = 12,
        Site = 13,
        Report = 14,
        Workflow = 15,
        OutsideProcess = 16,

        Custom = 99,
    }
}