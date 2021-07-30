using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelsCore;
using ModelsCore.Interfaces;
using System.Collections.Generic;

namespace SportPlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _log;
        private readonly IUserDataAccess _dataAccess;

        public UserController(ILogger<UserController> log, IUserDataAccess dataAccess)
        {
            _log = log;
            _dataAccess = dataAccess;
        }

        [HttpGet]
        public IEnumerable<UserDto> GetAll()
        {
            var events = _dataAccess.GetAll();

            return events;
        }
    }
}
