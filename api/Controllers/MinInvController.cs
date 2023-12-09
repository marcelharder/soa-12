
using System.Threading.Tasks;
using api.DTOs;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    public class MinInvController : BaseApiController
    {
        private IMinInv _min;
        private SpecialMaps _sm;
      
        public MinInvController(IMinInv min, SpecialMaps sm)
        {
            _min = min;
            _sm = sm;
           
        }
        [HttpGet("{id}", Name = "GetMinInv")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _min.getSpecificMIN(id);
            return Ok(_sm.mapToMinvForReturn(p));
        }
        [HttpPost]
        public async Task<IActionResult> Post(MinInvForReturn c)
        {
            var help = await _min.getSpecificMIN(c.procedure_id);
            var p = await _min.updateMIN(_sm.mapFromMinvForReturn(c, help));
            return Ok(p);
        }
    }
}