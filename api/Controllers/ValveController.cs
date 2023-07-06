using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
    [Authorize]
    public class ValveController : BaseApiController
    {

        private IValveRepository _valve;
        private readonly IOptions<ComSettings> _com;

        private SpecialMaps _special;
        public ValveController(IValveRepository valve, SpecialMaps special, IOptions<ComSettings> com)
        {
            _valve = valve;
            _special = special;
            _com = com;
        }

        #region <!-- manage valves in procedures -->
       
        [HttpGet("valvesFromProcedure/{id}")]
        public async Task<IActionResult> getValvesfromProcedure(int id)
        {
            var p = new List<ValveForReturnDTO>();
            var t = await _valve.getValvesFromProcedure(id);
            foreach (Class_Valve cv in t)
            {
                p.Add(_special.mapToValveForReturn(cv));
            }
            return Ok(p);
        }
        // read
        [HttpGet("{serial}/{procedure_id}", Name = "GetValve")]
        public async Task<IActionResult> Get(string serial, int procedure_id)
        {
            var p = await _valve.GetSpecificValve(serial, procedure_id);

            var result = _special.mapToValveForReturn(p);

           var help = "";
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
        [HttpPost("{serial}/{procedure_id}")]
        public async Task<IActionResult> Post(string serial, int procedure_id)
        {
            var x = await _valve.addValve(serial, procedure_id);
            return Ok(_special.mapToValveForReturn(x));
        }
        //update
        [HttpPut("updateProcedureValve")]
        public async Task<IActionResult> Put(ValveForReturnDTO v)
        {
            var p = await _valve.GetSpecificValve(v.SERIAL_IMP, v.Id);

            var test = _special.mapToClassValve(v,p);
            test.Id = v.Id;
            test.ProcedureId = v.procedure_id;
            

            var x = await _valve.updateValve(test);

            if (x == 1) { return Ok("Valve updated"); }
            return BadRequest("Error updating valve ...");
        }
        // delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string deleteResult;
            var p = await _valve.deleteSpecificValve(id);
            if (p == 1) { deleteResult = "Successfully removed ..."; } else { deleteResult = "Not removed ..."; }
            return Ok(deleteResult);
        }
        #endregion
        [HttpGet("models/{type}/{position}")]
        public async Task<IActionResult> GetM(string type, string position)
        {
            var result = await _valve.getProductCodesInHospital(type, position);
            return Ok(result);
        }


        #region <!-- manage valves in hospital if OVI is not available -->

        [HttpGet("hospitalValves/{type}/{position}")]
        public async Task<IActionResult> GetMH(string type, string position)
        {
            var result = await _valve.getValvesInHospital(type, position);
            return Ok(result);
        }

        [HttpPost("createhospitalValve")]
        public async Task<IActionResult> GetMHC(valveDTO code)
        {
            var result = await _valve.createValveInHospital(code);
            return Ok(result);
        }

       /*  [HttpGet("readHospitalValve/{code}")]
        public async Task<IActionResult> GetMHR(string code)
        {
            if(code != null){var result = await _valve.readValveInHospital(code);
            return Ok(result);}
            return BadRequest("code undefined");
            
        } */

        [HttpPut("updateHospitalValve")]
        public async Task<IActionResult> GetMHU(valveDTO code)
        {
            var result = await _valve.updateValveInHospital(code);
            return Ok(result);
        }

        [HttpDelete("deleteHospitalValve/{code}")]
        public async Task<IActionResult> GetMHD(int code)
        {
            if(code != 0){var result = await _valve.deleteValveInHospital(code);
            if(result == 1) { return Ok("item deleted ..."); }}
            return BadRequest("item could not be deleted ...");
        }
        #endregion

        #region <!-- get valves from the OVI -->

        [HttpGet("valvesfromOVIforSOA/{hospitalNo}/{soort}/{implant_Position}")]
        public async Task<IActionResult> getOVIValves(string hospitalNo,string soort, string implant_Position){
            string result = "";
            var help = "";
           
            help = _com.Value.valveURL + "valvesForSOA" + 
            "?HospitalNo=" + hospitalNo + 
            "&Soort=" +  soort +
            "&Position=" + implant_Position;
                       
           
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(help))
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            } 
          return Ok(result);
          
        }
        #endregion


    }
}