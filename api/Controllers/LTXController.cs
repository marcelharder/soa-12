using System.Threading.Tasks;
using api.DTOs;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
   
    [Authorize]
    public class LTXController : BaseApiController
    {


        private ILtxRepository _ltx;
        private SpecialMaps _special;
        public LTXController(ILtxRepository ltx, SpecialMaps special)
        {
            _special = special;
            _ltx = ltx;
        }

        [HttpGet("{id}", Name = "GetLTX")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _ltx.GetSpecificLTX(id);
            return Ok(_special.mapToLTXForReturn(p));
        }

        [HttpPost]
        public async Task<IActionResult> Post(LtxForReturnDTO c)
        {
            var p = await _ltx.GetSpecificLTX(c.PROCEDURE_ID);
            var x = await _ltx.updateLTX(_special.mapToLTX(c, p));
            return Ok(x);
        }


        


    }

}