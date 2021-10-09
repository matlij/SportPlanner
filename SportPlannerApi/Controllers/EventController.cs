using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelsCore;
using ModelsCore.Enums;
using SportPlannerApi.DataLayer.DataAccess;
using SportPlannerApi.DataLayer.Specifications;
using SportPlannerIngestion.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportPlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _log;
        private readonly IRepository<Event> _dataAccess;

        public EventController(ILogger<EventController> log, IRepository<Event> dataAccess)
        {
            _log = log;
            _dataAccess = dataAccess;
        }

        [HttpGet]
        public async Task<IEnumerable<EventDto>> GetAll([FromQuery] Guid userId, [FromQuery] int limit = 100)
        {
            ISpecification<Event> spec = userId != default && userId != Guid.Empty
                ? new GetEntitiesByUserIdSpecification(userId)
                : new GetEntitiesSpec();

            return await _dataAccess.Get<EventDto>(spec, limit);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetById([Required] Guid id)
        {
            var result = await _dataAccess.Get<EventDto>(new GetEntitiesByIdSpecification(id));

            var @event = result.SingleOrDefault();
            if (@event != null)
            {
                return Ok(@event);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody, Required] EventDto value)
        {
            var (crudResult, eventDto) = await _dataAccess.Add(value);
            if (crudResult != CrudResult.Ok)
            {
                _log.LogError($"FAILED: Create event: ${JsonSerializer.Serialize(value)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction(nameof(GetById), new { id = eventDto.Id }, null);
        }

        // PUT api/<EventController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([Required] Guid id, [FromBody] EventDto value)
        {
            var includes = new string[]
            {
                nameof(Event.Address),
                $"{nameof(Event.Users)}.{nameof(EventUser.User)}"
            };

            var result = await _dataAccess.Update(new GetByIdSpecification<Event>(id, includes), value);
            if (result == CrudResult.NotFound)
            {
                return BadRequest("Entity not found");
            }
            if (result == CrudResult.Ok)
            {
                return NoContent();
            }

            _log.LogError($"FAILED: Update event: {JsonSerializer.Serialize(value)}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // DELETE api/<EventController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([Required] Guid id)
        {
            var result = await _dataAccess.Delete(id);
            if (result == CrudResult.NotFound)
            {
                return BadRequest("Entity not found");
            }
            if (result == CrudResult.Ok)
            {
                return NoContent();
            }

            _log.LogError($"FAILED: Delete event with ID: {id}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
