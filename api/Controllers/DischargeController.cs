using System.Threading.Tasks;
using api.DTOs;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
  [Authorize]
    public class DischargeController : BaseApiController
    {
        private IDischarge _dis;
        public DischargeController(IDischarge dis)
        {
            _dis = dis;
        }

        [HttpGet("{id}", Name = "GetDischarge")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _dis.getSpecificDischarge(id);
            return Ok(p);
        }
        [HttpPost]
        public async Task<IActionResult> Post(DischargeForUpdateDTO c)
        {
            var p = await _dis.updateDischarge(c);
            return Ok(p);
        }
    }
}