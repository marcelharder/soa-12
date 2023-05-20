using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValveRepairController : ControllerBase
    {

        private IValveRepository _valve;

        private readonly IOptions<ComSettings> _com;
        private SpecialMaps _special;
        public ValveRepairController(IValveRepository valve, SpecialMaps special, IOptions<ComSettings> com)
        {
            _valve = valve;
            _special = special;
            _com = com;
        }

        [HttpGet("valveRepairsFromProcedure/{id}")]
        public async Task<IActionResult> getValverepairsfromProcedure(int id)
        {
            var p = new List<ValveForReturnDTO>();
            var t = await _valve.getValveRepairsFromProcedure(id);
            foreach (Class_Valve cv in t)
            {
                p.Add(_special.mapToValveForReturn(cv));
            }
            return Ok(p);
        }
        // read
        [HttpGet("{id}/{procedure_id}", Name = "GetValveRepair")]
        public async Task<IActionResult> Get(int id, int procedure_id)
        {
            var p = await _valve.GetSpecificValveRepair(id, procedure_id);


            var help = "";
            var result = _special.mapToValveForReturn(p);

            var comaddress = _com.Value.valveURL;
            var st = "getValveDescriptionFromModel/" + result.MODEL;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            result.valveDescription = help;

            return Ok(result);
        }
        //create
        [HttpPost("{position}/{procedure_id}")]
        public async Task<IActionResult> Post(string position, int procedure_id)
        {
            var x = await _valve.addValveRepair(position, procedure_id);
            return Ok(_special.mapToValveForReturn(x));
        }
        //update
        [HttpPut]
        public async Task<IActionResult> Put(ValveForReturnDTO v)
        {
            var p = await _valve.GetSpecificValveRepair(v.Id, v.procedure_id);
            var x = await _valve.updateValve(_special.mapToClassValve(v, p));
            if (x == 1) { return Ok("ValveRepair updated"); }
            return BadRequest("Error updating valve ...");
        }
        // delete


    }

}