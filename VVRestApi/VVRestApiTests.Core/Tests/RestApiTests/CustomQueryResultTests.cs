using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using VVRestApi.Common;
using VVRestApi.Vault;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class CustomQueryResultTests : TestBase
    {

        [Test]
        public void GetCustomQueryByQueryName()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "Department";
            var queryId = new Guid("AEB2F858-7B96-E111-972B-14FEB5F06078");

            var results = vaultApi.CustomQueryManager.GetCustomQueryResults(queryName);

            var count = results;
        }

        [Test]
        public void GetCustomQueryByQueryId()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "Department";
            var queryId = new Guid("AEB2F858-7B96-E111-972B-14FEB5F06078");

            var results = vaultApi.CustomQueryManager.GetCustomQueryResults(queryId);
        }

        [Test]
        public void GetCustomQueryByQueryNameWithFilter()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            string filter = "ususerid = 'vault.config'";

            var results = vaultApi.CustomQueryManager.GetCustomQueryResults(queryName, null, filter);

            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);
        }

        [Test]
        public void GetCustomQueryByQueryIdWithFilter()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            string filter = "ususerid = 'vault.config'";

            var results = vaultApi.CustomQueryManager.GetCustomQueryResults(queryId, null, filter);
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);
        }

        [Test]
        public void GetCustomQueryByQueryNameWithSortAsc()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            string sortBy = "uspkey";
            string sortDirection = "asc";

            var results = vaultApi.CustomQueryManager.GetCustomQueryResults(queryName, null, null, sortBy, sortDirection);

            Assert.IsNotNull(results);
            var firstValue = results[0]["uspKey"].ToString();
            var lastValue = results[1]["uspKey"].ToString();
            Assert.Greater(Int32.Parse(lastValue), Int32.Parse(firstValue));

        }

        [Test]
        public void GetCustomQueryByQueryIdWithSortAsc()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            string sortBy = "uspkey";
            string sortDirection = "asc";

            var results = vaultApi.CustomQueryManager.GetCustomQueryResults(queryId, null, null, sortBy, sortDirection);
            Assert.IsNotNull(results);
            var firstValue = ((JArray)results)[0]["uspKey"].ToString();
            var lastValue = ((JArray)results)[1]["uspKey"].ToString();
            Assert.Greater(Int32.Parse(lastValue), Int32.Parse(firstValue));
        }

        [Test]
        public void GetCustomQueryByQueryNameWithSortDesc()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            string sortBy = "uspkey";
            string sortDirection = "desc";

            var results = vaultApi.CustomQueryManager.GetCustomQueryResults(queryName, null, null, sortBy, sortDirection);

            Assert.IsNotNull(results);
            var firstValue = results[0]["uspKey"].ToString();
            var lastValue = results[1]["uspKey"].ToString();
            Assert.Greater(Int32.Parse(firstValue), Int32.Parse(lastValue));

        }

        [Test]
        public void GetCustomQueryByQueryIdWithSortDesc()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            string sortBy = "uspkey";
            string sortDirection = "desc";

            var results = vaultApi.CustomQueryManager.GetCustomQueryResults(queryId, null, null, sortBy, sortDirection);
            Assert.IsNotNull(results);
            var firstValue = ((JArray)results)[0]["uspKey"].ToString();
            var lastValue = ((JArray)results)[1]["uspKey"].ToString();
            Assert.Greater(Int32.Parse(firstValue), Int32.Parse(lastValue));
        }

        [Test]
        public void GetCustomQueryByNamePaged()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            RequestOptions options = new RequestOptions();
            options.Take = 10;
            options.Skip = 0;

            var results = vaultApi.CustomQueryManager.GetCustomQueryPagedResults(queryName, options);

            Assert.IsNotNull(results);
            Assert.Greater(results.Items.Count, 0);
            Assert.LessOrEqual(results.Items.Count, 10);
        }

        [Test]
        public void GetCustomQueryByIdPaged()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            RequestOptions options = new RequestOptions();
            options.Take = 10;
            options.Skip = 0;

            var results = vaultApi.CustomQueryManager.GetCustomQueryPagedResults(queryId, options);

            Assert.IsNotNull(results);
            Assert.Greater(results.Items.Count, 0);
            Assert.LessOrEqual(results.Items.Count, 10);
        }


        [Test]
        public void GetCustomQueryByQueryNamePagedWithSortAsc()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            string sortBy = "uspkey";
            string sortDirection = "asc";

            RequestOptions options = new RequestOptions();
            options.Take = 10;
            options.Skip = 0;

            var results = vaultApi.CustomQueryManager.GetCustomQueryPagedResults(queryName, options, null, sortBy, sortDirection);

            Assert.IsNotNull(results);
            var firstValue = results.Items.ElementAt(0).Data.Find(x => x.Key.ToLower() == "uspkey").Value;
            var lastValue = results.Items.ElementAt(1).Data.Find(x => x.Key.ToLower() == "uspkey").Value;
            Assert.Greater(Int32.Parse(lastValue), Int32.Parse(firstValue));

        }

        [Test]
        public void GetCustomQueryByQueryIdPagedWithSortAsc()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            string sortBy = "uspkey";
            string sortDirection = "asc";

            RequestOptions options = new RequestOptions();
            options.Take = 10;
            options.Skip = 0;

            var results = vaultApi.CustomQueryManager.GetCustomQueryPagedResults(queryId, options, null, sortBy, sortDirection);
            Assert.IsNotNull(results);
            var firstValue = results.Items.ElementAt(0).Data.Find(x => x.Key.ToLower() == "uspkey").Value;
            var lastValue = results.Items.ElementAt(1).Data.Find(x => x.Key.ToLower() == "uspkey").Value;
            Assert.Greater(Int32.Parse(lastValue), Int32.Parse(firstValue));
        }

        [Test]
        public void GetCustomQueryByQueryNamePagedWithSortDesc()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            string sortBy = "uspkey";
            string sortDirection = "desc";

            RequestOptions options = new RequestOptions();
            options.Take = 10;
            options.Skip = 0;

            var results = vaultApi.CustomQueryManager.GetCustomQueryPagedResults(queryName, options, null, sortBy, sortDirection);

            Assert.IsNotNull(results);
            var firstValue = results.Items.ElementAt(0).Data.Find(x => x.Key.ToLower() == "uspkey").Value;
            var lastValue = results.Items.ElementAt(1).Data.Find(x => x.Key.ToLower() == "uspkey").Value;
            Assert.Greater(Int32.Parse(firstValue), Int32.Parse(lastValue));

        }

        [Test]
        public void GetCustomQueryByQueryIdPagedWithSortDesc()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            string sortBy = "uspkey";
            string sortDirection = "desc";

            RequestOptions options = new RequestOptions();
            options.Take = 10;
            options.Skip = 0;

            var results = vaultApi.CustomQueryManager.GetCustomQueryPagedResults(queryId, options, null, sortBy, sortDirection);
            Assert.IsNotNull(results);
            var firstValue = results.Items.ElementAt(0).Data.Find(x => x.Key.ToLower() == "uspkey").Value;
            var lastValue = results.Items.ElementAt(1).Data.Find(x => x.Key.ToLower() == "uspkey").Value;
            Assert.Greater(Int32.Parse(firstValue), Int32.Parse(lastValue));
        }

        [Test]
        public void GetCustomQueryByNamePagedWithFilter()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            RequestOptions options = new RequestOptions();
            options.Take = 10;
            options.Skip = 0;

            string filter = "ususerid = 'vault.config'";

            var results = vaultApi.CustomQueryManager.GetCustomQueryPagedResults(queryName, options, filter);

            Assert.IsNotNull(results);
            Assert.AreEqual(results.Items.Count, 1);
            Assert.LessOrEqual(results.Items.Count, 10);
        }

        [Test]
        public void GetCustomQueryByIdPagedWithFilter()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            RequestOptions options = new RequestOptions();
            options.Take = 10;
            options.Skip = 0;

            string filter = "ususerid = 'vault.config'";

            var results = vaultApi.CustomQueryManager.GetCustomQueryPagedResults(queryId, options, filter);

            Assert.IsNotNull(results);
            Assert.AreEqual(results.Items.Count, 1);
            Assert.LessOrEqual(results.Items.Count, 10);
        }

        [Test]
        public void GetCustomQueryByNamePagedWithQuery()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            RequestOptions options = new RequestOptions();
            options.Take = 10;
            options.Skip = 0;

            options.Query = "ususerid eq 'vault.config'";

            var results = vaultApi.CustomQueryManager.GetCustomQueryPagedResults(queryName, options);

            Assert.IsNotNull(results);
            Assert.AreEqual(results.Items.Count, 1);
            Assert.LessOrEqual(results.Items.Count, 10);
        }

        [Test]
        public void GetCustomQueryByIdPagedWithQuery()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "users";
            var queryId = new Guid("83f38ad8-53a4-ea11-ae3d-5048494f4e43");

            RequestOptions options = new RequestOptions();
            options.Take = 10;
            options.Skip = 0;

            options.Query = "ususerid eq 'vault.config'";

            var results = vaultApi.CustomQueryManager.GetCustomQueryPagedResults(queryId, options);

            Assert.IsNotNull(results);
            Assert.AreEqual(results.Items.Count, 1);
            Assert.LessOrEqual(results.Items.Count, 10);
        }

        [Test]
        public void GetCustomQueryByQueryNameWithParams()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "usersparams";
            var queryId = new Guid("3ff21e68-263c-ec11-aecd-34298f90235b");

            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("Param", "vault");

            var results = vaultApi.CustomQueryManager.GetCustomQueryResults(queryName, null, null, null, null, queryParameters);

            Assert.IsNotNull(results);
            Assert.Greater(results.Count, 0);
        }

        [Test]
        public void GetCustomQueryByQueryIdWithParams()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "usersparams";
            var queryId = new Guid("3ff21e68-263c-ec11-aecd-34298f90235b");

            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("Param", "vault");

            var results = vaultApi.CustomQueryManager.GetCustomQueryResults(queryId, null, null, null, null, queryParameters);
            Assert.IsNotNull(results);
            Assert.Greater(results.Count, 0);
        }

        [Test]
        public void GetCustomQueryByNamePagedWithParams()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "usersparams";
            var queryId = new Guid("3ff21e68-263c-ec11-aecd-34298f90235b");

            RequestOptions options = new RequestOptions();
            options.Take = 10;
            options.Skip = 0;

            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("Param", "vault");

            var results = vaultApi.CustomQueryManager.GetCustomQueryPagedResults(queryName, options, null, null, null, queryParameters);

            Assert.IsNotNull(results);
            Assert.Greater(results.Items.Count, 0);
            Assert.LessOrEqual(results.Items.Count, 10);
        }

        [Test]
        public void GetCustomQueryByIdPagedWithParams()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var queryName = "usersparams";
            var queryId = new Guid("3ff21e68-263c-ec11-aecd-34298f90235b");

            RequestOptions options = new RequestOptions();
            options.Take = 10;
            options.Skip = 0;

            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("Param", "vault");

            var results = vaultApi.CustomQueryManager.GetCustomQueryPagedResults(queryId, options, null, null, null, queryParameters);

            Assert.IsNotNull(results);
            Assert.Greater(results.Items.Count, 0);
            Assert.LessOrEqual(results.Items.Count, 10);
        }
    }
}
