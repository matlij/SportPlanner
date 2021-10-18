using SportPlannerIngestion.DataLayer.Models;
using System;

namespace SportPlannerFunctionApi.DataLayer.Specifications
{
    public class GetUsersByIdSpecification : SpecificationBase<User>
    {
        public GetUsersByIdSpecification(Guid id) : base(e => e.Id == id)
        {
        }
    }
}
