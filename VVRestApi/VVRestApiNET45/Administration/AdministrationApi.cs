// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdministrationApi.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace VVRestApi.Administration
{
    using VVRestApi.Administration.Customers;
    using VVRestApi.Administration.Licenses;
    using VVRestApi.Common;
    using VVRestApi.Common.Messaging;

    /// <summary>
    ///     AdministrationApi is for working with an instance of the VisualVault administration database. Obtain an instance of the AdministrationApi by logging in with a configuration user's username and secret.
    /// </summary>
    public class AdministrationApi : BaseApi
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AdministrationApi" /> class.
        /// </summary>
        /// <param name="currentToken">
        ///     The current token.
        /// </param>
        internal AdministrationApi(SessionToken currentToken)
        {
            if (currentToken.IsValid() && currentToken.TokenType == TokenType.Administration)
            {
                this.CurrentToken = currentToken;

                // Once all of the BaseApi settings have been created, pass the AdministrationApi into the constructor of all of the sub API managers
                this.REST = new RestManager(this);
                this.Licenses = new LicenseManager(this);
                this.Customers = new CustomerManager(this);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Operations for managing customers
        /// </summary>
        public CustomerManager Customers { get; private set; }

        /// <summary>
        ///     Operations for managing licensing
        /// </summary>
        public LicenseManager Licenses { get; private set; }

        /// <summary>
        ///     Allows you to make authenticated administration REST API calls to the VisualVault server you are currently authenticated to.
        /// </summary>
        public RestManager REST { get; private set; }

        #endregion
    }
}