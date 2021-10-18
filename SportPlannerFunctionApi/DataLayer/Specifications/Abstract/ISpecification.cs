using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SportPlannerFunctionApi.DataLayer.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Filter { get; }
        List<string> Includes { get; }
    }
}
