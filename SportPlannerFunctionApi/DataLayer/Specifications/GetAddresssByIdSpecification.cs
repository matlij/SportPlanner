using SportPlannerIngestion.DataLayer.Models;
using System;

namespace SportPlannerFunctionApi.DataLayer.Specifications
{
    public class GetAddresssByIdSpecification : SpecificationBase<Address>
    {
        public GetAddresssByIdSpecification(Guid id) : base(e => e.Id == id)
        {
        }
    }
}
