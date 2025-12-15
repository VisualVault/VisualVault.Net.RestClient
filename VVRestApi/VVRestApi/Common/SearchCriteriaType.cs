using System;

namespace VVRestApi.Common
{
    public enum SearchCriteriaType
    {
        IsEqual = 1,
        NotEqual,
        GreaterThan,
        GreaterThanEqual,
        LessThan,
        LessThanEqual,
        BeginWith,
        NotBeginWith,
        EndWith,
        NotEndWith,
        Contain,
        NotContain,
        Null,
        NotNull,
        Between,
        NotBetween,
        In,
        NotIn
    }
}
