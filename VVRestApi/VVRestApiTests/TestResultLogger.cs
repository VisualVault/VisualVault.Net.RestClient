// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestResultLogger.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace VVRestApiTests
{
    using System;
    using System.Text;

    using VVRestApi.Common.Logging;

    public delegate void TestExceptionEventHandler(TestExceptionEventArgs args);

    public static class TestResultLogger
    {
        #region Public Events

        public static event TestExceptionEventHandler OnTestException;

        #endregion

        #region Public Methods and Operators

        public static void LogTest(string message)
        {
            RecordMessage(message, LogLevelType.Verbose);
        }

        public static void LogTestException(string message)
        {
            RecordMessage(message, LogLevelType.Error);
        }
        public static void LogTestException(Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Message: " + ex.Message);
            sb.AppendLine("Stack Trace: " + ex.StackTrace);

            RecordMessage(sb.ToString(), LogLevelType.Error);

            if (OnTestException != null)
            {
                OnTestException(new TestExceptionEventArgs("Exception thrown: " + sb, ex));
            }
        }

        public static void LogTestFailure(string message)
        {
            RecordMessage(message, LogLevelType.Error);
        }

        public static void LogTestFailure(Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Message: " + ex.Message);
            sb.AppendLine("Stack Trace: " + ex.StackTrace);

            RecordMessage(sb.ToString(), LogLevelType.Error);
        }

        public static void OnLogEventHandler(object sender, LogEventArgs args)
        {
            RecordLogEvent(args);
        }

        #endregion

        #region Methods

        private static void RecordLogEvent(LogEventArgs args)
        {
            if (args != null)
            {
                var sb = new StringBuilder();
                sb.AppendLine(args.Message);
                if (args.Exception != null)
                {
                    sb.AppendLine(args.Exception.Message);
                }

                if (args.Api != null)
                {
                    // if (args.Api.VaultConfiguration != null)
                    // {
                    // sb.AppendLine(string.Format("Customer Alias: {0}", args.VaultApi.VaultConfiguration.CustomerAlias));
                    // sb.AppendLine(string.Format("Database Alias: {0}", args.VaultApi.VaultConfiguration.DatabaseAlias));
                    // if (args.VaultApi.CurrentUser != null)
                    // sb.AppendLine(string.Format("User ID: {0}", args.VaultApi.CurrentUser.GetCurrentUserID()));
                    // }
                }

                string recordedMessage = RecordMessage(sb.ToString(), args.LogLevel);
            }
        }

        private static string RecordMessage(string message, LogLevelType logLevel)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Time: " + DateTime.Now.ToString());
            switch (logLevel)
            {
                case LogLevelType.Warn:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevelType.Error:
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    break;
                case LogLevelType.Fatal:
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case LogLevelType.Debug:
                case LogLevelType.Info:
                default:

                    Console.BackgroundColor = ConsoleColor.White;
                    break;
            }

            sb.AppendLine("Message: " + message);

            string finalMessage = sb.ToString();
            Console.WriteLine(finalMessage);

            return finalMessage;
        }

        #endregion
    }
}