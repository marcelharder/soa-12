using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Valved_ConduitController : BaseApiController
    {
       private IValveRepository _valve;
        private SpecialMaps _special;
        public Valved_ConduitController(IValveRepository valve, SpecialMaps special)
        {
            _valve = valve;
            _special = special;
        }


        [HttpGet("valvedConduitsFromProcedure/{id}")]
        public async Task<IActionResult> getValverepairsfromProcedure(int id)
        {
            var p = new List<ValveForReturnDTO>();
            var t = await _valve.getValvedConduitsFromProcedure(id);
            foreach (Class_Valve cv in t)
            {
                p.Add(_special.mapToValveForReturn(cv));
            }
            return Ok(p);
        }
        
        
         // read Valved_Conduit
        [HttpGet("{id}", Name = "GetValvedConduit")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _valve.GetSpecificValvedConduit(id);
            return Ok(_special.mapToValveForReturn(p));
        }

        //Create Valved_Conduit
        
        [HttpPost("{procedure_id}")]
        public async Task<IActionResult> Post(int procedure_id)
        {
            var x = await _valve.addValvedConduit(procedure_id);
            return Ok(_special.mapToValveForReturn(x));
        }
        //update Valved_Conduit
        [HttpPut]
        public async Task<IActionResult> Put(ValveForReturnDTO v)
        {
            var p = await _valve.GetSpecificValvedConduit(v.Id);
            var x = await _valve.updateValve(_special.mapToClassValve(v, p));
            if (x == 1) { return Ok("Valved_Conduit updated"); }
            return BadRequest("Error updating valvedConduit ...");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id){

            var x = await _valve.deleteSpecificValve(id);
            if (x == 1) { return Ok("Valved_Conduit deleted"); }
            return BadRequest("Error deleting valvedConduit ...");
        }
        


    }
}