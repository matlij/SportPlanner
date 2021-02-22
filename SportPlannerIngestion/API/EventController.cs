using System;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ModelsCore;
using ModelsCore.Interfaces;

namespace SportPlannerIngestion.API
{
    public class EventController
    {
        private readonly IEventDataAccess _dataAccess;

        public EventController(IEventDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        [FunctionName("GetAll")]
        public IActionResult GetAll(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "event")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting all events");

            var events = _dataAccess.Get();

            return new OkObjectResult(events);
        }

        [FunctionName("Get")]
        public IActionResult Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "event/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            if (string.IsNullOrEmpty(id))
                return new BadRequestObjectResult("Event identifier is null or empty");

            log.LogInformation("Getting Event with id " + id);

            var @event = _dataAccess.Get(id);

            if (@event != null)
            {
                return new OkObjectResult(@event);
            }

            return new NotFoundResult();
        }

        [FunctionName("GetByUserId")]
        public IActionResult GetByUserId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{userid}/events")] HttpRequest req,
            ILogger log, string userid)
        {
            if (string.IsNullOrEmpty(userid))
                return new BadRequestObjectResult("User identifier is null or empty");

            log.LogInformation("Getting Event for user " + userid);

            try
            {
                var events = _dataAccess.GetByUser(userid);
                return new OkObjectResult(events);
            }
            catch (Exception e)
            {
                log.LogError(e, $"Get {nameof(EventDto)}´s for user {userid} failed.");
                return new InternalServerErrorResult();
            }
        }

        [FunctionName("Create")]
        public async Task<IActionResult> CreateEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "event")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating a new Event.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
                return new BadRequestObjectResult("Request body is null or empty");

            var input = JsonConvert.DeserializeObject<EventDto>(requestBody);

            var result = await _dataAccess.Store(input);
            if (result == null)
                return new InternalServerErrorResult();

            return new OkObjectResult(result);
        }

        [FunctionName("Update")]
        public async Task<IActionResult> UpdateEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "event/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            if (string.IsNullOrEmpty(id))
                return new BadRequestObjectResult("Event identifier is null or empty");

            log.LogInformation("Updating Event");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<EventDto>(requestBody);

            input.Identifier = id;
            var result = await _dataAccess.Update(input);
            if (result == null)
                return new InternalServerErrorResult();

            return new OkObjectResult(result);
        }

        [FunctionName("Delete")]
        public async Task<IActionResult> DeleteEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "event/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            if (string.IsNullOrEmpty(id))
                return new BadRequestObjectResult("Event identifier is null or empty");

            log.LogInformation("deleting Event");

            var result = await _dataAccess.Delete(id);
            if (result == false)
            {
                return new NotFoundObjectResult(id);
            }

            return new OkResult();
        }
    }
}
