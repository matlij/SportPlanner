using Microsoft.AspNetCore.Mvc;
using ModelsCore;
using SportPlannerApi.DataLayer.DataAccess;
using SportPlannerApi.DataLayer.Specifications;
using SportPlannerIngestion.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportPlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IRepository<Address> _dataAccess;

        public AddressController(IRepository<Address> dataAccess)
        {
            _dataAccess = dataAccess;
        }

        // GET: api/<AddressController>
        [HttpGet]
        public async Task<IEnumerable<AddressDto>> Get()
        {
            return await _dataAccess.GetAll<AddressDto>();
        }

        // GET api/<AddressController>/5
        [HttpGet("{id}")]
        public async Task<AddressDto> Get(Guid id)
        {
            var result = await _dataAccess.Get<AddressDto>(new GetAddresssByIdSpecification(id));

            return result.SingleOrDefault();
        }

        // POST api/<AddressController>
        [HttpPost]
        public async Task<ActionResult<AddressDto>> Post([FromBody] AddressDto value)
        {
            var result = await _dataAccess.Add(value);

            if (result.result == ModelsCore.Enums.CrudResult.Ok)
            {
                return CreatedAtAction(nameof(Get), result.dto.Id, result.dto);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        // PUT api/<AddressController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AddressController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
