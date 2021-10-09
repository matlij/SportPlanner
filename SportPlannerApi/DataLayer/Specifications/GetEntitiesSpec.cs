using SportPlannerIngestion.DataLayer.Models;
using System;
using System.Linq.Expressions;

namespace SportPlannerApi.DataLayer.Specifications
{
    public class GetEntitiesSpec : ISpecification<Event>
    {
        public GetEntitiesSpec()
        {
            Filter = e => true;
        }

        public Expression<Func<Event, bool>> Filter { get; }

        public string[] Includes =>
            new string[]
            {
                nameof(Event.Address),
                $"{nameof(Event.Users)}.{nameof(EventUser.User)}"
            };
    }
}
