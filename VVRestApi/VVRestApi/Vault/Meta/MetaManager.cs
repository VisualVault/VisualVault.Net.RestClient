// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaManager.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VVRestApi.Common;
using VVRestApi.Common.Logging;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Newtonsoft.Json.Linq;
    using VVRestApi.Common;
    using VVRestApi.Common.Extensions;
    using VVRestApi.Common.Logging;

    /// <summary>
    /// 
    /// </summary>
    public class MetaManager : BaseApi
    {
        #region Fields

        private List<string> DataTypeNames { get; set; }

        private List<MetaDataType> DataTypes { get; set; }

        #endregion

        #region Constructors and Destructors

        internal MetaManager(VaultApi api)
        {
            this.Populate(api.ClientSecrets, api.ApiTokens);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the current user based on the CurrentToken
        /// </summary>
        /// <returns></returns>
        public List<string> GetDataTypeNames()
        {
            // Let's reuse the list of names once we have gotten them
            if (this.DataTypeNames == null)
            {
                try
                {
                    JObject result = HttpHelper.Get(GlobalConfiguration.Routes.Meta, string.Empty, null, GetUrlParts(), this.ApiTokens, this.ClientSecrets);
                    if (result.IsHttpStatus(HttpStatusCode.OK))
                    {
                        string json = result["data"]["dataTypes"].ToString();
                        JArray jArray = JArray.Parse(json);
                        this.DataTypeNames = jArray.ToObject<List<string>>();
                    }
                }
                catch (Exception e)
                {
                    LogEventManager.Error("Error getting data type names", e);
                }
            }

            return this.DataTypeNames;
        }

        /// <summary>
        ///     Gets the current user based on the CurrentToken
        /// </summary>
        /// <returns></returns>
        public List<MetaDataType> GetDataTypes()
        {
            // Let's reuse the list of names once we have gotten them
            if (this.DataTypes == null)
            {
                try
                {
                    RequestOptions options = new RequestOptions { Expand = true };
                    this.DataTypes = new List<MetaDataType>(HttpHelper.GetPagedResult<MetaDataType>(GlobalConfiguration.Routes.Meta, string.Empty, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens).Items);
                }
                catch (Exception e)
                {
                    LogEventManager.Error("Error getting data types", e);
                }
            }

            return this.DataTypes;
        }

        /// <summary>
        ///     Gets the current user based on the CurrentToken
        /// </summary>
        /// <param name="dataTypeName"> </param>
        /// <returns></returns>
        public MetaDataType GetDataType(string dataTypeName)
        {
            MetaDataType dataType = null;
            try
            {
                dataType = HttpHelper.Get<MetaDataType>(GlobalConfiguration.Routes.MetaId, string.Empty, null, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dataTypeName);
            }
            catch (Exception e)
            {
                LogEventManager.Error("Error getting data type", e);
            }

            return dataType;
        }

        #endregion
    }
}