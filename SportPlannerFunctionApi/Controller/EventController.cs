namespace SportPlannerFunctionApi;

public class EventController
{
    private readonly IRepository<Event> _dataAccess;

    public EventController(IRepository<Event> dataAccess)
    {
        _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
    }

    [FunctionName("GetAllEvents")]
    public async Task<IEnumerable<EventDto>> GetAllEvents(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "event")] HttpRequest req,
        ILogger log
        )
    {
        log.LogInformation($"C# HTTP trigger function processed a request for {nameof(GetAllEvents)}");

        string limit = req.Query["limit"];
        string userId = req.Query["userId"];

        ISpecification<Event> spec = string.IsNullOrEmpty(userId)
            ? new GetEventsSpecification(e => true)
            : new GetEventsByUserIdSpecification(Guid.Parse(userId));

        return await _dataAccess.Get<EventDto>(spec, string.IsNullOrEmpty(limit) ? 100 : int.Parse(limit));
    }

    [FunctionName("GetEvent")]
    public async Task<ActionResult<EventDto>> GetEvent(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "event/{id}")] HttpRequest req,
        ILogger log,
        Guid id)
    {
        log.LogInformation($"C# HTTP trigger function processed a request for {nameof(GetEvent)}");

        var result = await _dataAccess.Get<EventDto>(new GetEventByIdSpecification(id));
        var entity = result.SingleOrDefault();
        if (entity is null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(entity);
    }

    [FunctionName("AddEvent")]
    public async Task<IActionResult> AddEvent(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "event")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation($"C# HTTP trigger function processed a request for {nameof(AddEvent)}");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<EventDto>(requestBody);

        var (crudResult, eventDto) = await _dataAccess.Add(data);

        if (crudResult != CrudResult.Ok)
        {
            log.LogError($"FAILED: Create event: ${JsonConvert.SerializeObject(data)}");
            return new ObjectResult(StatusCodes.Status500InternalServerError);
        }

        return new CreatedAtRouteResult(nameof(GetEvent), new { id = eventDto.Id }, null);
    }

    [FunctionName("UpdateEvent")]
    public async Task<IActionResult> UpdateEvent(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "event/{id}")] HttpRequest req,
        ILogger log,
        Guid id)
    {
        log.LogInformation($"C# HTTP trigger function processed a request for {nameof(UpdateEvent)}");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<EventDto>(requestBody);

        var crudResult = await _dataAccess.Update(new GetEventByIdSpecification(id), data);

        return CrudResultToStatus(log, id, crudResult, HttpMethods.Put);
    }

    [FunctionName("DeleteEvent")]
    public async Task<IActionResult> DeleteEvent(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "event/{id}")] HttpRequest req,
        ILogger log,
        Guid id)
    {
        log.LogInformation($"C# HTTP trigger function processed a request for {nameof(DeleteEvent)}");

        var crudResult = await _dataAccess.Delete(id);
        return CrudResultToStatus(log, id, crudResult, HttpMethods.Delete);
    }

    private static IActionResult CrudResultToStatus(ILogger log, Guid id, CrudResult crudResult, string method)
    {
        if (crudResult == CrudResult.NotFound)
        {
            return new NotFoundResult();
        }
        else if (crudResult != CrudResult.Ok)
        {
            log.LogError($"{method} event {id} failed. CrudResult: {crudResult}");
            return new InternalServerErrorResult();
        }
        return new NoContentResult();
    }
}
