namespace VVRestApi.Administration.Customers
{
    public enum CustomerBillingAccountStatus
    {
        InTrial,
        TrialExpired,
        Billing,
        BillingExpired,
        Suspended,
        Canceled,
        Closed
    }
}