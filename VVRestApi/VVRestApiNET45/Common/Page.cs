namespace VVRestApi.Common
{
    using System;
    using System.Collections.Generic;

    using VVRestApi.Common.Messaging;

    public class Page<T> : RestObject
    {
        public Page(SessionToken token)
        {
            base.Populate(token);
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
        ///     The URI for the current resource
        /// </summary>
        public string Href { get; set; }

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