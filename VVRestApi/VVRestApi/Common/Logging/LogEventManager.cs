// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEventManager.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace VVRestApi.Common.Logging
{
    using System;
    using System.Diagnostics;

    public static class LogEventManager
    {
        #region Methods

        /// <summary>
        /// Returns true if an event is wired up the specified log level
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public static bool IsListening(LogLevelType logLevel)
        {
            return GlobalEvents.IsListening(logLevel);
        }

        /// <summary>
        ///     Publish a debug message
        /// </summary>
        public static void Debug(string message)
        {
            Publish(message, LogLevelType.Debug);
        }

        /// <summary>
        ///     Publish a error message
        /// </summary>
        public static void Error(string message)
        {
            Publish(message, LogLevelType.Error);
        }


        public static void Error(string message, Exception ex)
        {
            Publish(message, ex, LogLevelType.Error);
        }

        /// <summary>
        ///     Publish a fatal message
        /// </summary>
        public static void Fatal(string message)
        {
            Publish(message, LogLevelType.Fatal);
        }

        /// <summary>
        ///     Publish a info message
        /// </summary>
        public static void Info(string message)
        {
            Publish(message, LogLevelType.Info);
        }

        public static void Publish(string message, LogLevelType logLevel)
        {
            if (GlobalEvents.IsListening(logLevel))
            {
                Type declaringType = GetDeclaringType();

                GlobalEvents.RaiseException(null, new LogEventArgs(message, null, null, logLevel, declaringType));
            }
        }

        public static void Publish(string message, LogLevelType logLevel, Type declaringType)
        {
            if (GlobalEvents.IsListening(logLevel))
            {
                GlobalEvents.RaiseException(null, new LogEventArgs(message, null, null, logLevel, declaringType));
            }
        }

        public static void Publish(string message, Exception ex, LogLevelType logLevel)
        {
            if (GlobalEvents.IsListening(logLevel))
            {
                Type declaringType = GetDeclaringType();
                GlobalEvents.RaiseException(null, new LogEventArgs(message, ex, null, logLevel, declaringType));
            }
        }

        public static void Publish(string message, Exception ex, LogLevelType logLevel, Type declaringType)
        {
            if (GlobalEvents.IsListening(logLevel))
            {
                GlobalEvents.RaiseException(null, new LogEventArgs(message, ex, null, logLevel, declaringType));
            }
        }

        public static void Publish(string message, Exception ex, BaseApi api, LogLevelType logLevel)
        {
            if (GlobalEvents.IsListening(logLevel))
            {
                Type declaringType = GetDeclaringType();
                GlobalEvents.RaiseException(null, new LogEventArgs(message, ex, api, logLevel, declaringType));
            }
        }

        public static void Publish(string message, Exception ex, BaseApi vaultApi, LogLevelType logLevel, Type declaringType)
        {
            if (GlobalEvents.IsListening(logLevel))
            {
                GlobalEvents.RaiseException(null, new LogEventArgs(message, ex, vaultApi, logLevel, declaringType));
            }
        }

        /// <summary>
        ///     Publish a verbose message
        /// </summary>
        public static void Verbose(string message)
        {
            Publish(message, LogLevelType.Verbose);
        }

        /// <summary>
        ///     Publish a warn message
        /// </summary>
        public static void Warn(string message)
        {
            Publish(message, LogLevelType.Warn);
        }

        private static Type GetDeclaringType()
        {
            Type declaringType = null;
            if (GlobalEvents.IncludeDeclaringTypeInGlobalEventArgs)
            {
                var stackTrace = new StackTrace();
                try
                {
                    declaringType = stackTrace.GetFrame(1).GetMethod().DeclaringType;
                }
                catch (Exception)
                {
                    try
                    {
                        declaringType = stackTrace.GetFrame(1).GetMethod().ReflectedType;
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return declaringType;
        }

        #endregion
    }
}