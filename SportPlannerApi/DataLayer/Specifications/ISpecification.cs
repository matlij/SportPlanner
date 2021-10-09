using System;
using System.Linq.Expressions;

namespace SportPlannerApi.DataLayer.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Filter { get; }
        string[] Includes { get; }
    }
}
