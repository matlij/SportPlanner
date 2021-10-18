using SportPlannerIngestion.DataLayer.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SportPlannerFunctionApi.DataLayer.Specifications
{
    public class GetEventsSpecification : SpecificationBase<Event>
    {
        public GetEventsSpecification(Expression<Func<Event, bool>> filter) : base(filter)
        {
            AddInclude(nameof(Event.Address));
            AddInclude($"{nameof(Event.Users)}.{nameof(EventUser.User)}");
        }
    }

    public class GetEventsByUserIdSpecification : GetEventsSpecification
    {
        public GetEventsByUserIdSpecification(Guid userId) : base(e => e.Users.Any(eu => eu.UserId == userId))
        {
        }
    }

    public class GetEventByIdSpecification : GetEventsSpecification
    {
        public GetEventByIdSpecification(Guid id) : base(e => e.Id == id)
        {
        }
    }
}
