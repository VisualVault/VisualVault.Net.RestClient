using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using Newtonsoft.Json;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Vault.Users;
using VVRestApi.Vault;


namespace VVRestApi.Administration.Customers
{
    using System;
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class CustomerManager : BaseApi
    {
        

        internal CustomerManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="name">Name of the customer</param>
        /// <param name="alias">Alias of the customer. This will be used in the URL and for logging into the VaultApi</param>
        /// <param name="databaseAlias">Alias to give to the initial database that will be created for the customer. This will be used in the URL and for logging into the VaultApi.</param>
        /// <param name="newAdminUsername">A new unique username for the Vault.Access account on the administrator</param>
        /// <param name="newAdminPassword">A password for the new user.</param>
        /// <param name="newAdminEmailAddress">A valid e-mail address for the new user account. Password reset requests will be sent to this account.</param>
        /// <param name="databaseCount">The number of databases to license to this customer. At minimum, 1 database license will be granted.</param>
        /// <param name="userCount">The number of user accounts to license to the customer. At minimum, 5 user licenses will be granted.</param>
        /// <param name="addCurrentUser">If set to true, the current user will be added as a vault.access user.</param>
        /// <returns></returns>
        public Customer CreateCustomer(string name, string alias, string databaseAlias, string newAdminUsername, string newAdminPassword, string newAdminEmailAddress, int databaseCount, int userCount, bool addCurrentUser)
        {
            return HttpHelper.Post<Customer>(GlobalConfiguration.Routes.Customers, string.Empty, GetUrlParts(), this.ClientSecrets,this.ApiTokens, new { name = name, alias = alias, databaseAlias = databaseAlias, newAdminUsername = newAdminUsername, newAdminPassword = newAdminPassword, newAdminEmailAddress = newAdminEmailAddress, databaseCount = databaseCount, userCount = userCount, addCurrentUser = addCurrentUser });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public CustomerDatabaseInfo GetCustomerDatabaseInfo(RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.Get<CustomerDatabaseInfo>(GlobalConfiguration.Routes.CustomersCustomerDatabases, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }


        public CustomerOrganization GetCustomerOrganization(RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.Get<CustomerOrganization>(GlobalConfiguration.Routes.CustomersOrganization, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        public CustomerOrganization UpdateCustomerOrganizationName(string name)
        {
            dynamic postData = new ExpandoObject();
            postData.name = name;

            return HttpHelper.Put<CustomerOrganization>(GlobalConfiguration.Routes.CustomersOrganization, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData);
        }

        public CustomerOrganization UpdateCustomerOrganizationAddress(string address1, string address2, string city, string state, string zipcode, string phone1, string phone2)
        {
            dynamic postData = new ExpandoObject();
            postData.address1 = address1;
            postData.address2 = address2;
            postData.city = city;
            postData.state = state;
            postData.zipcode = zipcode;
            postData.phone1 = phone1;
            postData.phone2 = phone2;

            return HttpHelper.Put<CustomerOrganization>(GlobalConfiguration.Routes.CustomersOrganization, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData);
        }


        public CustomerBilling GetCustomerBillingInformation(RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.Get<CustomerBilling>(GlobalConfiguration.Routes.CustomersBilling, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }
        
        public CustomerBilling UpdateCustomerBillingAddress(string address1, string address2, string city, string state, string zipcode)
        {
            dynamic postData = new ExpandoObject();
            postData.address1 = address1;
            postData.address2 = address2;
            postData.city = city;
            postData.state = state;
            postData.zipcode = zipcode;

            return HttpHelper.Put<CustomerBilling>(GlobalConfiguration.Routes.CustomersBillingAddress, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData);
        }


        public CustomerSubscriptionPlan GetCustomerSubscriptionPlan(RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.Get<CustomerSubscriptionPlan>(GlobalConfiguration.Routes.CustomersBillingPlan, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        public CustomerBilling UpdateCustomerSubscriptionPlan(int subscriptionPlanId)
        {
            dynamic postData = new ExpandoObject();
            postData.subscriptionPlanId = subscriptionPlanId;

            return HttpHelper.Put<CustomerBilling>(GlobalConfiguration.Routes.CustomersBillingPlan, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData);
        }

        public static ApiMetaData CreateNewTrialCustomerAccount(CustomerOrganization customerOrganization, int subscriptionPlanId, string emailAddress, string firstName, string lastName, IClientEndPoints clientEndPoints)
        {
            dynamic postData = new ExpandoObject();

            postData.baseUrl = BaseApi.EmailBaseUrl;

            postData.emailAddress = emailAddress;
            postData.firstName = firstName;
            postData.lastName = lastName;

            postData.subscriptionPlanId = subscriptionPlanId;
            postData.isTrial = true;

            postData.orgName = customerOrganization.Name;
            postData.orgAddress1 = customerOrganization.Address1;
            postData.orgAddress2 = customerOrganization.Address2;
            postData.orgCity = customerOrganization.City;
            postData.orgState = customerOrganization.State;
            postData.orgZipcode = customerOrganization.ZipCode;
            postData.orgPhone1 = customerOrganization.Phone1;
            postData.orgPhone2 = customerOrganization.Phone2;


            var resultData = HttpHelper.PostPublicNoCustomerAliases(GlobalConfiguration.Routes.CustomersAccounts, "", GetUrlParts(clientEndPoints), postData);
            var metaNode = resultData["meta"];
            if (metaNode != null)
            {
                return JsonConvert.DeserializeObject<ApiMetaData>(metaNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());
            }

            return new ApiMetaData();
        }

        public static ApiMetaData CreateNewCustomerAccount(CustomerOrganization customerOrganization, CustomerBillingAddress billingAddress, bool isTrial, int subscriptionPlanId, string emailAddress, string firstName, string lastName, IClientEndPoints clientEndPoints)
        {
            dynamic postData = new ExpandoObject();

            if (!isTrial)
            {
                postData.authToken = "dfindfjkfdiafkadfkadlkfladkfnvlkadflkvandlvknadaahifha7afhadfhi4hoh";
            }
            
            postData.baseUrl = BaseApi.EmailBaseUrl;

            postData.emailAddress = emailAddress;
            postData.firstName = firstName;
            postData.lastName = lastName;

            postData.subscriptionPlanId = subscriptionPlanId;
            postData.isTrial = isTrial;

            postData.orgName = customerOrganization.Name;
            postData.orgAddress1 = customerOrganization.Address1;
            postData.orgAddress2 = customerOrganization.Address2;
            postData.orgCity = customerOrganization.City;
            postData.orgState = customerOrganization.State;
            postData.orgZipcode = customerOrganization.ZipCode;
            postData.orgPhone1 = customerOrganization.Phone1;
            postData.orgPhone2 = customerOrganization.Phone2;

            postData.billingAddress1 = billingAddress.Address1;
            postData.billingAddress2 = billingAddress.Address2;
            postData.billingCity = billingAddress.City;
            postData.billingState = billingAddress.State;
            postData.billingZipcode = billingAddress.ZipCode;


            var resultData = HttpHelper.PostPublicNoCustomerAliases(GlobalConfiguration.Routes.CustomersAccounts, "", GetUrlParts(clientEndPoints), postData);
            var metaNode = resultData["meta"];
            if (metaNode != null)
            {
                return JsonConvert.DeserializeObject<ApiMetaData>(metaNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());
            }

            return new ApiMetaData();
        }

        public CustomerBilling CancelCustomerAccount()
        {
            dynamic postData = new ExpandoObject();
            postData.cancelAccount = true;

            return HttpHelper.Put<CustomerBilling>(GlobalConfiguration.Routes.CustomersAccounts, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData);
        }

        public List<CustomerSubscriptionPlan> GetCustomerSubscriptionPlans(RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.GetListResult<CustomerSubscriptionPlan>(GlobalConfiguration.Routes.ListsSubscriptionplans, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        public ApiMetaData AssignUser(Guid customerId, string userId, bool allowMultipleCustomers = false)
        {
            if (customerId.Equals(Guid.Empty))
            {
                throw new ArgumentException("Customer ID is required but was an empty Guid", "customerId");
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("UserId is required", "UserId");
            }

            dynamic putData = new ExpandoObject();
            putData.userId = userId;
            if (allowMultipleCustomers)
            {
                putData.allowMultipleCustomers = allowMultipleCustomers;
            }

            return HttpHelper.PutNoCustomerAliasReturnMeta(GlobalConfiguration.Routes.CustomerAssignUser, string.Empty, GetUrlParts(), this.ApiTokens, ClientSecrets, putData, customerId);
        }


    }
}