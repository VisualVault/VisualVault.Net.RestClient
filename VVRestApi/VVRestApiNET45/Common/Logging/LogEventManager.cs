// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEventManager.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace VVRestApi.Common.Logging
{
    using System;
    using System.Diagnostics;

    internal static class LogEventManager
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
        internal static void Debug(string message)
        {
            Publish(message, LogLevelType.Debug);
        }

        /// <summary>
        ///     Publish a error message
        /// </summary>
        internal static void Error(string message)
        {
            Publish(message, LogLevelType.Error);
        }


        internal static void Error(string message, Exception ex)
        {
            Publish(message, ex, LogLevelType.Error);
        }

        /// <summary>
        ///     Publish a fatal message
        /// </summary>
        internal static void Fatal(string message)
        {
            Publish(message, LogLevelType.Fatal);
        }

        /// <summary>
        ///     Publish a info message
        /// </summary>
        internal static void Info(string message)
        {
            Publish(message, LogLevelType.Info);
        }

        internal static void Publish(string message, LogLevelType logLevel)
        {
            if (GlobalEvents.IsListening(logLevel))
            {
                Type declaringType = GetDeclaringType();

                GlobalEvents.RaiseException(null, new LogEventArgs(message, null, null, logLevel, declaringType));
            }
        }

        internal static void Publish(string message, LogLevelType logLevel, Type declaringType)
        {
            if (GlobalEvents.IsListening(logLevel))
            {
                GlobalEvents.RaiseException(null, new LogEventArgs(message, null, null, logLevel, declaringType));
            }
        }

        internal static void Publish(string message, Exception ex, LogLevelType logLevel)
        {
            if (GlobalEvents.IsListening(logLevel))
            {
                Type declaringType = GetDeclaringType();
                GlobalEvents.RaiseException(null, new LogEventArgs(message, ex, null, logLevel, declaringType));
            }
        }

        internal static void Publish(string message, Exception ex, LogLevelType logLevel, Type declaringType)
        {
            if (GlobalEvents.IsListening(logLevel))
            {
                GlobalEvents.RaiseException(null, new LogEventArgs(message, ex, null, logLevel, declaringType));
            }
        }

        internal static void Publish(string message, Exception ex, BaseApi api, LogLevelType logLevel)
        {
            if (GlobalEvents.IsListening(logLevel))
            {
                Type declaringType = GetDeclaringType();
                GlobalEvents.RaiseException(null, new LogEventArgs(message, ex, api, logLevel, declaringType));
            }
        }

        internal static void Publish(string message, Exception ex, BaseApi vaultApi, LogLevelType logLevel, Type declaringType)
        {
            if (GlobalEvents.IsListening(logLevel))
            {
                GlobalEvents.RaiseException(null, new LogEventArgs(message, ex, vaultApi, logLevel, declaringType));
            }
        }

        /// <summary>
        ///     Publish a verbose message
        /// </summary>
        internal static void Verbose(string message)
        {
            Publish(message, LogLevelType.Verbose);
        }

        /// <summary>
        ///     Publish a warn message
        /// </summary>
        internal static void Warn(string message)
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