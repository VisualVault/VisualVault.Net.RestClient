// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogLevelType.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VVRestApi.Common.Logging
{
    using System;

    [Flags]
    public enum LogLevelType
    {
        None = 0, 

        Verbose = 1, 

        Debug = 2, 

        Info = 4, 

        Warn = 8, 

        Error = 16, 

        Fatal = 32, 
    }
}