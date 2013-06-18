// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginTests.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// <summary>
//   The login tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace VVRestApiTests
{
    using NUnit.Framework;

    using VVRestApi;
    using VVRestApi.Vault;

    /// <summary>
    ///     The login tests.
    /// </summary>
    [TestFixture]
    public class LoginTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// The login test.
        /// </summary>
        [Test]
        public void LoginTest()
        {
            string vaultServerUrl = "http://YOURSERVER/VISUALVAULT_VIRTUAL_DIRECTORY_IF_IT_EXISTS/";
            string customerAlias = "CustomerToLoginTo";
            string databaseAlias = "DatabaseAliasOfCustomer";
            string apiKey = "YOUR API KEY / DEVELOPER ID HERE";
            string developerId = "SAME AS API KEY OR ANOTHER DEVELOPER ID";
            string developerSecret = "DEVELOPER SECRET HERE";

            VaultApi vault = Authorize.GetVaultApi(apiKey, developerId, developerSecret, vaultServerUrl, customerAlias, databaseAlias);
            Assert.IsNotNull(vault);
        }

        #endregion
    }
}