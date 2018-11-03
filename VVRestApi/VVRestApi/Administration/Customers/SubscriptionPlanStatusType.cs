namespace VVRestApi.Administration.Customers
{
    public enum SubscriptionPlanStatusType
    {
        Available,
        Prototype,
        Beta,
        ExistingOnly,
        Retired
    }

    public enum SelectSubscriptionPlanStatus
    {
        Available,
        Prototype,
        Beta,
        ExistingOnly,
        Retired,
        BetaAndAvailable,
        All
    }
}