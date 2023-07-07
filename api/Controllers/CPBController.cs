using System.Threading.Tasks;
using api.DTOs;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace api.Controllers
{
    
    [Authorize]
    public class CPBController : BaseApiController
    {
        private ICPBRepository _cpb;
        private SpecialMaps _special;
        public CPBController( ICPBRepository cpb, SpecialMaps special)
         { 
             _cpb = cpb;
             _special = special; 
        }
       
       [HttpGet("{id}", Name = "GetCPB")]
        public async Task<IActionResult> Get(int id) 
        {  
            var p = await _cpb.GetSpecificCPB(id);  
            return Ok(_special.mapToCPBForReturn(p)); 
        }
        [HttpPost]
        public async Task<IActionResult> Post(CPBForReturnDTO c)
         { 
            var old_cpb = await _cpb.GetSpecificCPB(c.procedure_id);
            var p = await _cpb.updateCPB(_special.mapToClassCPB(c, old_cpb));  
            return Ok(p);  
        }

    }

}