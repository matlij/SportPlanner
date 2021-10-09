using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelsCore;
using SportPlannerApi.DataLayer.DataAccess;
using SportPlannerIngestion.DataLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportPlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _log;
        private readonly IRepository<User> _dataAccess;

        public UserController(ILogger<UserController> log, IRepository<User> dataAccess)
        {
            _log = log;
            _dataAccess = dataAccess;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAll()
        {
            return await _dataAccess.GetAll<UserDto>();
        }
    }
}
