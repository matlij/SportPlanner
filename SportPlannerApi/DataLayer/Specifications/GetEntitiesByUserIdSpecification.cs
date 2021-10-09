using SportPlannerIngestion.DataLayer.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SportPlannerApi.DataLayer.Specifications
{
    public class GetEntitiesByUserIdSpecification : ISpecification<Event>
    {
        public GetEntitiesByUserIdSpecification(Guid userId)
        {
            Filter = e => e.Users.Any(eu => eu.UserId == userId);
        }

        public Expression<Func<Event, bool>> Filter { get; }

        public string[] Includes => new string[]
        {
            nameof(Event.Address),
            $"{nameof(Event.Users)}.{nameof(EventUser.User)}"
        };
    }

    public class GetEntitiesByIdSpecification : ISpecification<Event>
    {
        public GetEntitiesByIdSpecification(Guid id)
        {
            Filter = e => e.Id == id;
        }

        public Expression<Func<Event, bool>> Filter { get; }

        public string[] Includes => new string[]
        {
            nameof(Event.Address),
            $"{nameof(Event.Users)}.{nameof(EventUser.User)}"
        };
    }

    public class GetUsersByIdSpecification : ISpecification<User>
    {
        public GetUsersByIdSpecification(Guid id)
        {
            Filter = e => e.Id == id;
        }

        public Expression<Func<User, bool>> Filter { get; }

        public string[] Includes => Array.Empty<string>();
    }

    public class GetAddresssByIdSpecification : ISpecification<Address>
    {
        public GetAddresssByIdSpecification(Guid id)
        {
            Filter = e => e.Id == id;
        }

        public Expression<Func<Address, bool>> Filter { get; }

        public string[] Includes => Array.Empty<string>();
    }

    public class GetByIdSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public GetByIdSpecification(Guid id, string[] includes)
        {
            Filter = e => e.Id == id;
            Includes = includes;
        }

        public Expression<Func<T, bool>> Filter { get; }
        public string[] Includes { get; }
    }
}
