using System;
using VVRestApi.Common;

namespace VVRestApi.Administration.Customers
{
    public class CustomerOrganization : RestObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public DateTime ModifyDate { get; set; }
        public Guid ModifyById { get; set; }
    }
}