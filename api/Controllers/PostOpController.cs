using System.Threading.Tasks;
using api.DTOs;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    
    public class PostOpController : BaseApiController
    {
        
        private IPORepository _ipo;
        private SpecialMaps _special;
        public PostOpController(IPORepository ipo, SpecialMaps special)
        {
            _ipo = ipo; 
            _special = special;
        }

        [HttpGet("{id}", Name = "GetPostOp")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _ipo.GetSpecificPostOp(id);
            return Ok(_special.mapToPostOpForReturn(p));
        }
        [HttpPost]
        public async Task<IActionResult> Post(PostOpDetailsDTO c)
        {
             var p = await _ipo.GetSpecificPostOp(c.procedure_id);
             var x = await _ipo.updatePostOp(_special.mapToClassPostop(c,p));
            return Ok(x);
        }

    }
}