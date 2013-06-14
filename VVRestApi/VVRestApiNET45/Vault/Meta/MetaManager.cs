// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaManager.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VVRestApi.Vault.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using Newtonsoft.Json.Linq;

    using VVRestApi.Common;
    using VVRestApi.Common.Extensions;
    using VVRestApi.Common.Logging;

    public class MetaManager : BaseApi
    {
        #region Fields

        private List<string> DataTypeNames { get; set; }

        private List<MetaDataType> DataTypes { get; set; }

        #endregion

        #region Constructors and Destructors

        internal MetaManager(VaultApi api)
        {
            this.Populate(api.CurrentToken);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the current user based on the CurrentToken
        /// </summary>
        /// <param name="refresh">If set to true, the a call will be made to the server to refresh the current user properties.</param>
        /// <returns></returns>
        public List<string> GetDataTypeNames()
        {
            // Let's reuse the list of names once we have gotten them
            if (this.DataTypeNames == null)
            {
                try
                {
                    JObject result = HttpHelper.Get(GlobalConfiguration.Routes.Meta, string.Empty, false, string.Empty, this.CurrentToken);
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
        /// <param name="refresh">If set to true, the a call will be made to the server to refresh the current user properties.</param>
        /// <returns></returns>
        public List<MetaDataType> GetDataTypes()
        {
            // Let's reuse the list of names once we have gotten them
            if (this.DataTypes == null)
            {
                try
                {
                    this.DataTypes = HttpHelper.GetListResult<MetaDataType>(GlobalConfiguration.Routes.Meta, string.Empty, true, string.Empty, this.CurrentToken);
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
        /// <param name="refresh">If set to true, the a call will be made to the server to refresh the current user properties.</param>
        /// <returns></returns>
        public MetaDataType GetDataType(string dataTypeName)
        {
            MetaDataType dataType = null;
                try
                {
                    dataType = HttpHelper.Get<MetaDataType>(GlobalConfiguration.Routes.MetaId, string.Empty, false, string.Empty, this.CurrentToken, dataTypeName);
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