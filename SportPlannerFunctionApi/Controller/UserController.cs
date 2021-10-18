using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ModelsCore;
using SportPlannerFunctionApi.DataLayer.DataAccess;
using SportPlannerIngestion.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportPlannerFunctionApi
{
    public class UserController
    {
        private readonly IRepository<User> _dataAccess;

        public UserController(IRepository<User> dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        [FunctionName("GetAllUsers")]
        public async Task<IEnumerable<UserDto>> GetAllUsers(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "User")] HttpRequest req,
            ILogger log
            )
        {
            log.LogInformation($"C# HTTP trigger function processed a request for {nameof(GetAllUsers)}");

            return await _dataAccess.GetAll<UserDto>();
        }
    }
}
