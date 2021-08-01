using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelsCore;
using ModelsCore.Interfaces;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportPlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _log;
        private readonly IEventDataAccess _dataAccess;

        public EventController(ILogger<EventController> log, IEventDataAccess dataAccess)
        {
            _log = log;
            _dataAccess = dataAccess;
        }

        [HttpGet]
        public IEnumerable<EventDto> GetAll([FromQuery] string userId)
        {
            var events = _dataAccess.GetAll(userId);

            return events;
        }

        [HttpGet("{id}")]
        public ActionResult<EventDto> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Event identifier is null or empty");

            var @event = _dataAccess.GetById(id);

            if (@event != null)
            {
                return Ok(@event);
            }

            return NotFound();
        }

        // POST api/<EventController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EventDto value)
        {
            if (value == null)
                return BadRequest("Request body is null or empty");

            var result = await _dataAccess.Store(value);
            if (result == null)
            {
                _log.LogError($"FAILED: Create event: ${JsonSerializer.Serialize(value)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction(nameof(GetById), result.Identifier, null);
        }

        // PUT api/<EventController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] EventDto value)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Event identifier is null or empty");

            if (value.Identifier is null)
            {
                value.Identifier = id;
            }

            var result = await _dataAccess.Update(value);
            if (result == null)
            {
                _log.LogError($"FAILED: Update event: {JsonSerializer.Serialize(value)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(result);
        }

        // DELETE api/<EventController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Event identifier is null or empty");

            var succeded = await _dataAccess.Delete(id);
            if (!succeded)
            {
                _log.LogError($"FAILED: Delete event: id");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}
