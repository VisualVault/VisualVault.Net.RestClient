// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VaultSetUpFixture.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// <summary>
//   The vault set up fixture.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace VVRestApiTests
{
    using System;

    using NUnit.Framework;

    using VVRestApi;

    [SetUpFixture]
    public class VaultSetUpFixture
    {
        #region Public Events

        public static event TestExceptionEventHandler OnTestException;

        #endregion

        #region Public Methods and Operators

        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Running Setup in SetUpFixture");
            SetupTestListeners();

            OnTestException += TestSetupFixtureHelper_OnTestException;
        }

        [TearDown]
        public void Teardown()
        {
        }

        #endregion

        #region Methods


        /// <summary>
        /// Demonstration on how to attach to events raised by the client library
        /// </summary>
        private static void SetupTestListeners()
        {
            GlobalEvents.IsRunningUnitTests = true;
            GlobalEvents.IncludeDeclaringTypeInGlobalEventArgs = false;

            GlobalEvents.ClearListeners();

            GlobalEvents.OnVerbose += TestResultLogger.OnLogEventHandler;
            GlobalEvents.OnDebug += TestResultLogger.OnLogEventHandler;
            GlobalEvents.OnError += TestResultLogger.OnLogEventHandler;
            GlobalEvents.OnFatal += TestResultLogger.OnLogEventHandler;
            GlobalEvents.OnInfo += TestResultLogger.OnLogEventHandler;
            GlobalEvents.OnWarn += TestResultLogger.OnLogEventHandler;

            TestResultLogger.OnTestException += TestResultLogger_OnTestException;
        }

        private static void TestResultLogger_OnTestException(TestExceptionEventArgs args)
        {
            if (args != null)
            {
                if (OnTestException != null)
                {
                    OnTestException(args);
                }
            }
        }

        private static void TestSetupFixtureHelper_OnTestException(TestExceptionEventArgs args)
        {
            if (args != null)
            {
                Assert.Fail(args.Message);
            }
        }

        #endregion
    }
}