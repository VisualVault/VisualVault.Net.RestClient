using System;
using VVRestApi.Common;

namespace VVRestApi.Administration.Customers
{
    public class CustomerBilling : RestObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public CustomerBillingAccountStatus AccountStatus { get; set; }

        public string VendorSubscriptionId { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public string CardNumber { get; set; }

        public int SubscriptionPlanId { get; set; }
        public string SubscriptionPlanName { get; set; }
        public string SubscriptionPlanCode { get; set; }

        public DateTime TrialStartDate { get; set; }
        public DateTime TrialEndDate { get; set; }

        public DateTime BillingStartDate { get; set; }

        public DateTime? DateCanceled { get; set; }
        public Guid CanceledById { get; set; }

        public DateTime? DateToExpire { get; set; }

        public DateTime? DateSuspended { get; set; }
        public Guid SuspendedById { get; set; }

        public DateTime? DateToClose { get; set; }
        public DateTime? DateClosed { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public Guid ModifyById { get; set; }
    }
}