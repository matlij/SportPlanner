using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelsCore;
using ModelsCore.Enums;
using ModelsCore.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly IEventDataAccess _dataAccess;
        private readonly IUserDataAccess _userDataAccess;

        public EventController(ILogger<EventController> log, IEventDataAccess eventDataAccess, IUserDataAccess userDataAccess)
        {
            _log = log;
            _dataAccess = eventDataAccess;
            _userDataAccess = userDataAccess;
        }

        [HttpGet]
        public IEnumerable<EventDto> GetAll([FromQuery] string userId)
        {
            var events = _dataAccess.GetAll(userId);

            return events;
        }

        [HttpGet("{id}")]
        public ActionResult<EventDto> GetById([Required] string id)
        {
            var @event = _dataAccess.GetById(id);
            if (@event != null)
            {
                return Ok(@event);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody, Required] EventDto value)
        {
            var (crudResult, eventDto) = await _dataAccess.Store(value);
            if (crudResult != CrudResult.Ok)
            {
                _log.LogError($"FAILED: Create event: ${JsonSerializer.Serialize(value)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction(nameof(GetById), new { id = eventDto.Id }, null);
        }

        // PUT api/<EventController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([Required] string id, [FromBody] EventDto value)
        {
            var result = await _dataAccess.Update(id, value);
            if (result != CrudResult.Ok)
            {
                _log.LogError($"FAILED: Update event: {JsonSerializer.Serialize(value)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        // DELETE api/<EventController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([Required] string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Event identifier is null or empty");

            var result = await _dataAccess.Delete(id);
            if (result != CrudResult.Ok)
            {
                _log.LogError($"FAILED: Delete event: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}
