using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVRestApi.Common;

namespace VVRestApi.Studio.Models
{
    public class StudioRequestOptions : IRequestOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SortDir { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Search { get; set; }

        public StudioRequestOptions() 
        { 
        
        }

        public void PrepForRequest()
        {
            if (this.Take <= 0)
                this.Take = 200;

            if (this.Skip <= 0)
                this.Skip = 0;
        }


        public string GetQueryString(string originalQueryString)
        {
            this.PrepForRequest();

            var queryDict = ParseQueryString(originalQueryString ?? string.Empty);

            // Always ensure take/skip are valid
            if (!queryDict.ContainsKey("take") && Take > 0)
                queryDict["take"] = Take.ToString();

            if (!queryDict.ContainsKey("skip") && Skip >= 0)
                queryDict["skip"] = Skip.ToString();

            // Optional filters / sorting
            if (!string.IsNullOrWhiteSpace(Sort))
                queryDict["sort"] = Sort;

            if (!string.IsNullOrWhiteSpace(SortDir))
                queryDict["sortDir"] = SortDir;

            if (!string.IsNullOrWhiteSpace(Search))
                queryDict["search"] = Search;

            // Construct query string
            var query = string.Join("&", queryDict.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value ?? string.Empty)}"));

            return query;
        }

        private static Dictionary<string, string> ParseQueryString(string query)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(query))
                return dict;

            foreach (var token in query.TrimStart('?').Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = token.Split(new[] { '=' }, 2);
                var key = parts[0].Trim();

                var value = parts.Length > 1 ? parts[1].Trim() : string.Empty;
                dict[key] = value;
            }

            return dict;
        }

    }
}
