﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VVRestApi.Common;
using VVRestApi.Common.Logging;
using VVRestApi.Common.Messaging;
using VVRestApi.Vault;
using VVRestApi.Vault.Meta;
using VVRestApi.Vault.PersistedData;
using VVRestApiTests.Core.Tests.RestApiTests.Mocks;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class RestClientTests : TestBase
    {
        [Test]
        public void RequestOptionTest()
        {
            VVRestApi.Common.RequestOptions options = new RequestOptions();
            options.Query = "name eq 'world' AND id eq 'whatever'";

            var request = options.GetQueryString("q=userid eq 'vault.config'&stuff=things");
            Assert.IsNotEmpty(request);
            Assert.IsTrue(request.Contains("q=(userid eq 'vault.config') AND (name eq 'world' AND id eq 'whatever')"));
        }

        [Test]
        public void RequestOptionsGiveBackGoodQuery()
        {
            var reqOpts = new VVRestApi.Common.RequestOptions
            {
                Fields = @"VV Provider ID, CPR Number, EI Last Name, EI First Name, ADSA Client ID, ESIT Client ID, VisualVault Individual ID, Authorization Number DDA, Authorization Number ESIT, Authorization Number School Dist, Fund Source, RAC Code, CPR Number,
             Date of Record, Service Year Month, Program Type, Service Type, Status",
                Query = $"[VV Provider ID] eq 'pro_001' AND [CPR Number] = 'WrongNumber'",
                Take = 2000
            };


            var queryString = reqOpts.GetQueryString("");

            Assert.IsTrue(queryString.Contains("q=[VV Provider ID] eq 'pro_001' AND [CPR Number] = 'WrongNumber'"));
        }

        [Test]
        public void CallScheduledProcessCompleteUsingQueryString()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var spToken = new Guid("C1647446-4417-4341-B1F2-D82FAAEE20EA");

            vaultApi.ScheduledProcess.CallCompleteScheduledProcessUsingQueryString(spToken, "Test Message", true);

            //Assert.IsNotNull(data);
        }

        [Test]
        public void CallScheduledProcessCompleteUsingPostedData()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var spToken = new Guid("C1647446-4417-4341-B1F2-D82FAAEE20EA");

            vaultApi.ScheduledProcess.CallCompleteScheduledProcessUsingPostedData(spToken, "Test Message", true);

            //Assert.IsNotNull(data);
        }

        [Test]
        public void RunScheduledProcesses()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            vaultApi.ScheduledProcess.RunScheduledProcesses();

            //Assert.IsNotNull(data);
        }


        [Test]
        public void PersistedDataTest()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            const string dataToStore = "{\"invoice no\":\"272255\",\"formname\":\"orderform\",\"Address\":\"ds\"}";

            string persistedDataUniqueName = ShortGuid.NewGuid();

            PersistedClientData data = vaultApi.PersistedData.CreateData(persistedDataUniqueName, ScopeType.Global, dataToStore, "text/JSON", "", LinkedObjectType.None, null);

            Assert.IsNotNull(data);

            //get the persisted data by Id
            data = vaultApi.PersistedData.GetData(data.Id);

            Assert.IsNotNull(data);

            //Note:  The list of field names below was generated by running the GetApiObjectFieldNames test 
            //which calls the MetaData APi endpoint

            //PersistedData fields: 

            //CreateByUsId
            //CreateDateUtc
            //DataLength
            //DataMimeType
            //ExpirationDateUtc
            //Id
            //LinkedObjectId
            //LinkedObjectType
            //ModifiedByUsId
            //ModifiedDateUtc
            //Name
            //PersistedData
            //Scope

            //example of getting the persisted data by name (vs. using the Id) using a query
            //this api call returns a page of data matching the query parameters
            //in this example we only expect to get one item

            //basic query syntax (note field names must be enclosed in square brackets)
            //
            //[field name] {logical operator} '{predicate}'
            //
            //

            Page<PersistedClientData> dataPage = vaultApi.PersistedData.GetAllData(new RequestOptions() { Query = string.Format("[Name] eq '{0}'", persistedDataUniqueName) });

            Assert.IsNotNull(dataPage);

            var user = vaultApi.Users.GetUser(_ResourceOwnerUserName);

            if (user != null)
            {
                string loginToken = user.GetWebLoginToken();

                Assert.IsNotEmpty(loginToken);

                string url = string.Format("{0}/v1/en/Customer412/Main/vvlogin?token={1}&returnurl=userportal%3fportalname={2}%26persistedId={3}", _VaultApiBaseUrl, loginToken, "InvoiceData", data.Id);

                LogEventManager.Info(string.Format("Token login URL with Persisted Data Id query string parameter:{0} {1}", Environment.NewLine, url));
            }

        }

        [Test]
        public void CreateCustomerTest()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var newCustomer = vaultApi.Customer.CreateCustomer("Customer2", "Customer2", "Main", "Customer2.Admin", "p",
                                             "username@company.com", 1, 5, true);

            Assert.IsNotNull(newCustomer);
        }

        [Test]
        public void GetApiObjectFieldNames()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            List<MetaDataType> ApiObjectTypes = vaultApi.Meta.GetDataTypes();

            StringBuilder sbFieldList = new StringBuilder();

            sbFieldList.AppendLine("API Object Field List");

            foreach (MetaDataType apiObjectType in ApiObjectTypes)
            {
                sbFieldList.AppendLine("-----------------------------------------------------------------");

                sbFieldList.AppendLine(string.Format("{0} fields: {1}", apiObjectType.Name, Environment.NewLine));

                foreach (string fieldName in apiObjectType.AvailableFields)
                {
                    sbFieldList.AppendLine(string.Format("{0}", fieldName));
                }
            }

            LogEventManager.Info(sbFieldList.ToString());
        }

        [Test]
        public async Task TestRetryHandlerWithMock()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://example.com/");
            var handler = new RetryHandler(new RetryHttpMessageHandlerMock());

            var invoker = new HttpMessageInvoker(handler);
            var result = await invoker.SendAsync(httpRequestMessage, new CancellationToken());
        }


        [Test]
        public async Task TestRetryHandlerWithThreeRequestMax()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var formTemplate1 = vaultApi.FormTemplates.GetFormTemplate("Cell Format");
            var formTemplate2 = vaultApi.FormTemplates.GetFormTemplate("Cell Format");
            var formTemplate3 = vaultApi.FormTemplates.GetFormTemplate("Cell Format");
            var formTemplate4 = vaultApi.FormTemplates.GetFormTemplate("Cell Format");

            Assert.IsNotNull(formTemplate1);
            Assert.IsNotNull(formTemplate2);
            Assert.IsNotNull(formTemplate3);
            Assert.IsNotNull(formTemplate4);
        }
    }
}