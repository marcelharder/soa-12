using System.Threading.Tasks;
using api.DTOs;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
 
    [Authorize]
    public class AorticSurgeryController : BaseApiController
    {
        private IAorticSurgery _aos;
        private SpecialMaps _special;
        public AorticSurgeryController( IAorticSurgery aos, SpecialMaps special)
         { 
             _aos=aos;
             _special = special; 
        }
        [HttpGet("{id}", Name = "GetAorticSurgery")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _aos.getSpecificCAS(id);
            return Ok(_special.mapToAOSForReturn(p));
        }

        [HttpPost]
        public async Task<IActionResult> Post(AoSurgeryForReturnDTO c)
        {
            var p = await _aos.getSpecificCAS(c.procedure_id);

            var x = await _aos.updateCAS(_special.mapToCAS(c,p));
            return Ok(x);
        }

    }

}