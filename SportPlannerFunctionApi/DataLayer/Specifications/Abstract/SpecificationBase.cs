using SportPlannerIngestion.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SportPlannerFunctionApi.DataLayer.Specifications
{
    public abstract class SpecificationBase<T> : ISpecification<T> where T : BaseEntity
    {
        protected SpecificationBase(Expression<Func<T, bool>> filter)
        {
            Filter = filter;
            Includes = new List<string>();
        }

        protected void AddInclude(string include)
        {
            Includes.Add(include);
        }

        public Expression<Func<T, bool>> Filter { get; }
        public List<string> Includes { get; }
    }
}
