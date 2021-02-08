using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ModelsCore.Interfaces;

namespace SportPlannerIngestion.API
{
    public class UserController
    {
        private readonly IUserDataAccess _userDataAccess;

        public UserController(IUserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }

        [FunctionName("GetAllUsers")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "user")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting all users.");

            var users = _userDataAccess.GetAll();

            return new OkObjectResult(users);
        }
    }
}
