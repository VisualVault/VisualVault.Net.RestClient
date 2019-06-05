// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestExceptionEventArgs.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// <summary>
//   The test exception event args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace VVRestApiTests
{
    using System;

    public class TestExceptionEventArgs : EventArgs
    {
        #region Constructors and Destructors

        public TestExceptionEventArgs(string message, Exception ex)
        {
            this.Exception = ex;
            this.Message = message;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     The exception that may have been raised
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        ///     The message
        /// </summary>
        public string Message { get; set; }

        #endregion
    }
}