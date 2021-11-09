using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ModelsCore;
using ModelsCore.Enums;
using Newtonsoft.Json;
using SportPlannerFunctionApi.DataLayer.DataAccess;
using SportPlannerFunctionApi.DataLayer.Specifications;
using SportPlannerIngestion.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

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

        [FunctionName("GetUser")]
        public async Task<ActionResult<UserDto>> GetUser(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{id}")] HttpRequest req,
            ILogger log,
            Guid id)
        {
            log.LogInformation($"C# HTTP trigger function processed a request for {nameof(GetUser)}");

            var result = await _dataAccess.Get<UserDto>(new GetUserByIdSpecification(id));
            var entity = result.SingleOrDefault();
            if (entity is null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(entity);
        }

        [FunctionName("AddUser")]
        public async Task<IActionResult> AddUser(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "user")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request for {nameof(AddUser)}");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UserDto>(requestBody);

            var (crudResult, userDto) = await _dataAccess.Add(data);

            if (crudResult == CrudResult.AlreadyExists)
            {
                return new ConflictObjectResult(userDto);
            }
            else if (crudResult != CrudResult.Ok)
            {
                log.LogError($"FAILED: Create user: ${JsonConvert.SerializeObject(data)}");
                return new InternalServerErrorResult();
            }

            return new CreatedAtRouteResult(nameof(GetUser), new { id = userDto.Id }, null);
        }

        [FunctionName("UpdateUser")]
        public async Task<IActionResult> UpdateUser(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "user/{id}")] HttpRequest req,
            ILogger log,
            Guid id)
        {
            log.LogInformation($"C# HTTP trigger function processed a request for {nameof(UpdateUser)}");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UserDto>(requestBody);

            var crudResult = await _dataAccess.Update(new GetUserByIdSpecification(id), data);
            if (crudResult == CrudResult.NotFound)
            {
                var (addCrudResult, userDto) = await _dataAccess.Add(data);
                crudResult = addCrudResult;
            }

            if (crudResult != CrudResult.Ok)
            {
                log.LogError($"FAILED: Create user: ${JsonConvert.SerializeObject(data)}");
                return new InternalServerErrorResult();
            }

            return new NoContentResult();
        }
    }
}
