using VVRestApi.Common.Messaging;

namespace VVRestApi.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Page<T> : RestObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientSecrets"> </param>
        /// /// <param name="apiTokens"></param>
        public Page(IClientSecrets clientSecrets,Tokens apiTokens)
        {
            base.Populate(clientSecrets, apiTokens);
            ItemType = typeof(T);
            Items = new List<T>();
            First = string.Empty;
            Last = string.Empty;
            Limit = 0;
            Next = string.Empty;
            Offset = 0;
            Previous = string.Empty;
            TotalRecords = 0;
            
        }

        /// <summary>
        /// The result set
        /// </summary>
        public Type ItemType { get; private set; }

        /// <summary>
        /// The result set
        /// </summary>
        public ICollection<T> Items { get; set; }

        /// <summary>
        ///     The URI for the first page of results
        /// </summary>
        public string First { get; set; }

        /// <summary>
        ///     The URI for the last page of results
        /// </summary>
        public string Last { get; set; }

        /// <summary>
        ///     The row limit for this request
        /// </summary>
        public long Limit { get; set; }

        /// <summary>
        ///     The resource URI for the next set of results
        /// </summary>
        public string Next { get; set; }

        /// <summary>
        ///     THe number of records skipped for this request
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        ///     The resource URI for a previous set of results
        /// </summary>
        public string Previous { get; set; }
        
        /// <summary>
        ///     The total number of records
        /// </summary>
        public long TotalRecords { get; set; }

    }
}