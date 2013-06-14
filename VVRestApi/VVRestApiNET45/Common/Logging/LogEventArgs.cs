// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEventArgs.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VVRestApi.Common.Logging
{
    using System;

    public class LogEventArgs : EventArgs
    {
        #region Constructors (1)

        public LogEventArgs(string message, Exception ex, BaseApi api, LogLevelType logLevel, Type declaringType)
        {
            this.Exception = ex;
            this.Message = message;
            this.Api = api;
            this.LogLevel = logLevel;
            this.DateTime = DateTime.Now;
            this.DeclaringType = declaringType;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        ///     The Vault that was in use when the event was raised.
        /// </summary>
        public BaseApi Api { get; set; }

        /// <summary>
        ///     The date time that the event fired
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        ///     The Type that raised the event. You must have GlobalConfig.IncludeDeclaringTypeInGlobalEventArgs set to True to have this populated. This will make a call to the StackTrace and can cause performance hits.
        /// </summary>
        public Type DeclaringType { get; set; }

        /// <summary>
        ///     The exception that may have been raised
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        ///     The log level of the message
        /// </summary>
        public LogLevelType LogLevel { get; set; }

        /// <summary>
        ///     The message
        /// </summary>
        public string Message { get; set; }

        #endregion
    }
}