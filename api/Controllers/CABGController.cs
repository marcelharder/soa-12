using System.Threading.Tasks;
using api.DTOs;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
  
    [Authorize]
    public class CABGController : BaseApiController
    {


        private ICABGRepository _cabg;
        private SpecialMaps _special;
        public CABGController(ICABGRepository cabg, SpecialMaps special)
        {
            _special = special;
            _cabg = cabg;
        }

        [HttpGet("{id}", Name = "GetCABG")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _cabg.GetSpecificCABG(id);
            return Ok(_special.mapToCABGForReturn(p));
        }


        [HttpPost]
        public async Task<IActionResult> Post(CabgDetailsDTO c)
        {
            var p = await _cabg.GetSpecificCABG(c.procedure_id);
            var x = await _cabg.updateCABG(_special.mapToClassCABG(c, p));
            return Ok(x);
        }


        


    }

}