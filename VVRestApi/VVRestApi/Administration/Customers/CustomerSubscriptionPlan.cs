using System;
using VVRestApi.Common;

namespace VVRestApi.Administration.Customers
{
    public class CustomerSubscriptionPlan : RestObject
    {
        public int Id { get; set; }
        public string PlanCode { get; set; }
        public decimal PlanPrice { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Features { get; set; }
        public SubscriptionPlanStatusType PlanStatus { get; set; }
        public int OrdinalPosition { get; set; }
        public bool IsDefault { get; set; }
        public DateTime RetireDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public Guid ModifyById { get; set; }
    }
}