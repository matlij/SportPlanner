using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModelsCore;
using ModelsCore.Enums;
using ModelsCore.Interfaces;
using SportPlannerIngestion.DataLayer.Data;
using SportPlannerIngestion.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportPlannerIngestion.DataLayer.DataAccess
{
    public class EventDataAccess : IEventDataAccess
    {
        private readonly SportPlannerContext _context;
        private readonly IMapper _mapper;

        public EventDataAccess(SportPlannerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICollection<EventDto> GetAll(string userId = null)
        {
            var events = userId == null
                ? GetEvents(_context)
                : GetEvents(_context)
                    .Where(e => e.Users.Any(eu => eu.UserId == userId));

            return _mapper.Map<ICollection<EventDto>>(events.ToList());
        }

        public EventDto GetById(string identifier)
        {
            var @event = GetEvents(_context).FirstOrDefault(e => e.Id == identifier);
            return _mapper.Map<EventDto>(@event);
        }

        public async Task<(CrudResult crudResult, EventDto eventDto)> Store(EventDto input)
        {
            return await StoreEvent(_context, input);
        }

        public async Task<CrudResult> Update(string id, EventDto input)
        {
            var existing = _context.Events
                .Include(e => e.Users)
                .SingleOrDefault(e => e.Id == id);

            if (input.Id is null)
            {
                input.Id = id;
            }

            if (existing == null)
            {
                var (crudResult, eventDto) = await StoreEvent(_context, input);
                return crudResult;
            }
            else
            {
                _mapper.Map(input, existing);
                await _context.SaveChangesAsync();
                return CrudResult.Ok;
            }
        }

        public async Task<CrudResult> Delete(string identifier)
        {
            var eventToRemove = _context.Events.Single(e => e.Id == identifier);
            if (eventToRemove == null)
            {
                return CrudResult.NotFound;
            }

            _context.Remove(eventToRemove);
            var result = await _context.SaveChangesAsync();

            return result > 0 ? CrudResult.Ok : CrudResult.NoAction;
        }

        private async Task<(CrudResult crudResult, EventDto eventDto)> StoreEvent(SportPlannerContext context, EventDto input)
        {
            if (string.IsNullOrEmpty(input.Id))
            {
                input.Id = Guid.NewGuid().ToString();
            }

            var newEvent = _mapper.Map<Event>(input);

            context.Add(newEvent);
            var result = await context.SaveChangesAsync();
            var crudResult = result > 0 ? CrudResult.Ok : CrudResult.NoAction;

            return (crudResult, _mapper.Map<EventDto>(newEvent));
        }

        private static IIncludableQueryable<Event, User> GetEvents(SportPlannerContext context)
        {
            return context.Events
                .Include(e => e.Users)
                .ThenInclude(eu => eu.User);
        }
    }
}
