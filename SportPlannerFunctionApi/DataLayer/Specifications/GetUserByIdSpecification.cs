using SportPlannerIngestion.DataLayer.Models;
using System;

namespace SportPlannerFunctionApi.DataLayer.Specifications
{
    public class GetUserByIdSpecification : SpecificationBase<User>
    {
        public GetUserByIdSpecification(Guid id) : base(e => e.Id == id)
        {
        }
    }
}
