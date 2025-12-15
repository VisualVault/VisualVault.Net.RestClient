using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using VVRestApi.Common.Serialize;

namespace VVRestApi.Objects.Enums
{
    [JsonConverter(typeof(FlexibleEnumConverter<SearchCriteriaType>))]
    public enum SearchCriteriaType
    {
        [XmlEnum("1")]
        IsEqual = 1,

        [XmlEnum("2")]
        NotEqual,

        [XmlEnum("3")]
        GreaterThan,

        [XmlEnum("4")]
        GreaterThanEqual,

        [XmlEnum("5")]
        LessThan,

        [XmlEnum("6")]
        LessThanEqual,

        [XmlEnum("7")]
        BeginWith,

        [XmlEnum("8")]
        NotBeginWith,

        [XmlEnum("9")]
        EndWith,

        [XmlEnum("10")]
        NotEndWith,

        [XmlEnum("11")]
        Contain,

        [XmlEnum("12")]
        NotContain,

        [XmlEnum("13")]
        Null,

        [XmlEnum("14")]
        NotNull,

        [XmlEnum("15")]
        Between,

        [XmlEnum("16")]
        NotBetween,

        [XmlEnum("17")]
        In,

        [XmlEnum("18")]
        NotIn
    }
}
