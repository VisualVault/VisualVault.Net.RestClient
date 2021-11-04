using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVRestApi.Common;
using Newtonsoft.Json.Linq;

namespace VVRestApi.Vault.CustomQueries
{
    public class CustomQueryResult : RestObject
    {
        public List<KeyValuePair<string, string>> Data { get; set; }

        internal override void PopulateData(JToken data)
        {
            Data = new List<KeyValuePair<string, string>>();

            var jobject = data as JObject;
            if (jobject != null)
            {
                foreach (var dataProperty in jobject)
                {
                    Data.Add(new KeyValuePair<string, string>(dataProperty.Key, dataProperty.Value.ToString()));
                }
            }
        }
    }
}
