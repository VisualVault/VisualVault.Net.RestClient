// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalEvents.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VVRestApi
{
    using System;

    using VVRestApi.Common.Logging;

    /// <summary>
    ///     Global events that you can subscribe to which any of the APIs will call to when an error occurs.
    /// </summary>
    public static class GlobalEvents
    {
        #region Public Events

        /// <summary>
        ///     The on debug.
        /// </summary>
        public static event LogEventHandler OnDebug;

        /// <summary>
        ///     The on error.
        /// </summary>
        public static event LogEventHandler OnError;

        /// <summary>
        ///     The on fatal.
        /// </summary>
        public static event LogEventHandler OnFatal;

        /// <summary>
        ///     The on info.
        /// </summary>
        public static event LogEventHandler OnInfo;

        /// <summary>
        ///     The on verbose.
        /// </summary>
        public static event LogEventHandler OnVerbose;

        /// <summary>
        ///     The on warn.
        /// </summary>
        public static event LogEventHandler OnWarn;

        #endregion

        #region Public Properties

        /// <summary>
        ///     If set to true, the declaring type will be determined via a stacktrace
        /// </summary>
        public static bool IncludeDeclaringTypeInGlobalEventArgs { get; set; }

        /// <summary>
        ///     Set to true to have the REST API provide more diagnostic inspection of the messaging
        /// </summary>
        public static bool IsRunningUnitTests { get; set; }

        
        /// <summary>
        /// If set to true, wired events ignored when raised
        /// </summary>
        public static bool Mute { get; set; }

        #endregion

        #region Public Methods and Operators

        public static void ClearListeners()
        {
            try
            {
                if (OnDebug != null)
                {
                    foreach (Delegate invoker in OnDebug.GetInvocationList())
                    {
                        OnDebug -= (LogEventHandler)invoker;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                if (OnError != null)
                {
                    foreach (Delegate invoker in OnError.GetInvocationList())
                    {
                        OnError -= (LogEventHandler)invoker;
                    }    
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                if (OnFatal != null)
                {
                    foreach (Delegate invoker in OnFatal.GetInvocationList())
                    {
                        OnFatal -= (LogEventHandler)invoker;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                if (OnInfo != null)
                {
                    foreach (Delegate invoker in OnInfo.GetInvocationList())
                {
                    OnInfo -= (LogEventHandler)invoker;
                }

                }
                            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                if (OnVerbose != null)
                {
                    foreach (Delegate invoker in OnVerbose.GetInvocationList())
                    {
                        OnVerbose -= (LogEventHandler)invoker;
                    }    
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                if (OnWarn != null)
                {
                    foreach (Delegate invoker in OnWarn.GetInvocationList())
                    {
                        OnWarn -= (LogEventHandler)invoker;
                    }    
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The is listening.
        /// </summary>
        /// <param name="logLevel">
        ///     The log level.
        /// </param>
        /// <returns>
        ///     The System.Boolean.
        /// </returns>
        internal static bool IsListening(LogLevelType logLevel)
        {
            if (Mute)
            {
                return false;
            }

            bool listening = false;

            switch (logLevel)
            {
                case LogLevelType.Verbose:
                    if (OnVerbose != null)
                    {
                        listening = true;
                    }

                    break;
                case LogLevelType.Debug:
                    if (OnDebug != null)
                    {
                        listening = true;
                    }

                    break;
                case LogLevelType.Info:
                    if (OnInfo != null)
                    {
                        listening = true;
                    }

                    break;
                case LogLevelType.Warn:
                    if (OnWarn != null)
                    {
                        listening = true;
                    }

                    break;
                case LogLevelType.Error:
                    if (OnError != null)
                    {
                        listening = true;
                    }

                    break;
                case LogLevelType.Fatal:
                    if (OnFatal != null)
                    {
                        listening = true;
                    }

                    break;
                default:
                    if (OnDebug != null)
                    {
                        listening = true;
                    }

                    break;
            }

            return listening;
        }

        /// <summary>
        ///     The raise exception.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        internal static void RaiseException(object sender, LogEventArgs e)
        {
            if (Mute)
            {
                return;
            }

            if (e != null)
            {
                switch (e.LogLevel)
                {
                    case LogLevelType.Verbose:
                        if (OnVerbose != null)
                        {
                            OnVerbose(sender, e);
                        }

                        break;
                    case LogLevelType.Debug:
                        if (OnDebug != null)
                        {
                            OnDebug(sender, e);
                        }

                        break;
                    case LogLevelType.Info:
                        if (OnInfo != null)
                        {
                            OnInfo(sender, e);
                        }

                        break;
                    case LogLevelType.Warn:
                        if (OnWarn != null)
                        {
                            OnWarn(sender, e);
                        }

                        break;
                    case LogLevelType.Error:
                        if (OnError != null)
                        {
                            OnError(sender, e);
                        }

                        break;
                    case LogLevelType.Fatal:
                        if (OnFatal != null)
                        {
                            OnFatal(sender, e);
                        }

                        break;
                    default:
                        if (OnDebug != null)
                        {
                            OnDebug(sender, e);
                        }

                        break;
                }
            }
        }

        #endregion
    }
}