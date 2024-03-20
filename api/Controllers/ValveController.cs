using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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

            var test = _special.mapToClassValve(v, p);
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

        [HttpGet("models/{type}/{position}")] //gives the list of drop_items
        public async Task<IActionResult> GetM(string type, string position)
        {
            var currentHospitalId = await _special.getCurrentHospitalIdAsync();
            var help = "";
            var comaddress = _com.Value.productURL;
            var st = "ValveCode/getValveCodesInHospital/" + type + "/" + position + "/" + currentHospitalId;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            //var result = await _valve.getValvesInHospital(type, position);
            return Ok(help);
        }

        #region <!-- manage valves in hospital if OVI is not available -->

        [HttpGet("getValveCodeSizes/{type}")]
        public async Task<IActionResult> getCodeSizes(int type)
        {
            var help = "";
            var comaddress = _com.Value.productURL;
            var st = "ValveSize/" + type;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok(help);
        }


        [HttpGet("hospitalValves/{type}/{position}")]//gives the list of ValveCodes
        public async Task<IActionResult> GetMH(string type, string position)
        {
            var currentHospitalId = await _special.getCurrentHospitalIdAsync();
            var help = "";
            var comaddress = _com.Value.productURL;
            var st = "ValveCode/getValveCodesInHospital/" + type + "/" + position + "/" + currentHospitalId;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            //var result = await _valve.getValvesInHospital(type, position);
            return Ok(help);
        }

        [HttpGet("hospitalValvesNotInHospital/{type}/{position}")]//gives the list of valveCodes to choose from
        public async Task<IActionResult> GetMH2(string type, string position)
        {
            var currentHospitalId = await _special.getCurrentHospitalIdAsync();
            var help = "";
            var comaddress = _com.Value.productURL;
            var st = "ValveCode/getValve_CodeAsItemsNotInHospital/" + type + "/" + position + "/" + currentHospitalId;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            //var result = await _valve.getValvesInHospital(type, position);
            return Ok(help);
        }

        [HttpGet("createHospitalValve")]
        public async Task<IActionResult> GetMHC()
        {
            var help = "";
            var comaddress = _com.Value.productURL;
            var st = "ValveCode";
            comaddress = comaddress + st;
            var h = new Valve_Code();
            h.Type = "0";
            var json = JsonConvert.SerializeObject(h, Formatting.None);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(comaddress, content))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok(help);// this gives a new Valve_Code with ValveTypeId
        }

        [HttpGet("vendors")]
        public async Task<IActionResult> getVendors()
        {
            var help = "";
            var comaddress = _com.Value.valveURL;
            var st = "vendors";
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok(help);
        }

        [HttpGet("readHospitalValve/{code}")]
        public async Task<IActionResult> GetMHR(string code)
        {
            var help = "";
            var comaddress = _com.Value.productURL;
            var st = "ValveCode/detailsByValveId/" + code;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    help = await response.Content.ReadAsStringAsync();
                    return Ok(help);
                }
            }
            

        }

        [HttpPut("updateHospitalValve")]
        public async Task<IActionResult> GetMHU([FromBody] Valve_Code code)
        {
            //code.hospitalId = await _special.getCurrentHospitalIdAsync();
            code.Valve_size = null;
            var help = "";
            var comaddress = _com.Value.productURL;
            var st = "ValveCode";
            comaddress = comaddress + st;
            var json = JsonConvert.SerializeObject(code, Formatting.None);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PutAsync(comaddress, content))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok(help);
        }

        [HttpDelete("deleteHospitalValve/{code}")]
        public async Task<IActionResult> GetMHD(int code)
        {
            var help = "";
            var comaddress = _com.Value.productURL;
            var st = "ValveCode/" + code;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(comaddress))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok(help);
        }
       
        [HttpGet("writeHospitalIdToValveCode/{code}")]
        public async Task<IActionResult> WriteHosId(int code)
        {
            var help = "";
            var t = code.ToString().makeSureTwoChar();
            var comaddress = _com.Value.productURL;
            var st = "ValveCode/writeHospitalIdToValveCode/" + t + "/" + await _special.getCurrentHospitalIdAsync();
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok(help);
        }
         
       [HttpPost("addValveTypePhoto/{id}")]
        public async Task<IActionResult> AddPhotoForValveType(int id, [FromForm] PhotoForCreationDto photoDto)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(photoDto.File.OpenReadStream()), photoDto.File.Name, photoDto.File.FileName);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = photoDto.File.Name, FileName = photoDto.File.FileName };

            var help = new photoResult();
            var comaddress = _com.Value.hospitalURL;
            var st = "ValveCode/addPhoto/" + id;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(comaddress, content))
                {
                    help = await response.Content.ReadFromJsonAsync<photoResult>();
                }
            }
            return Ok(help.document_url); 
        }

        #endregion

        #region <!-- get valves from the OVI -->

        [HttpGet("valvesfromOVIforSOA/{hospitalNo}/{Type}/{implant_Position}")]
        public async Task<IActionResult> getOVIValves(string hospitalNo, string Type, string implant_Position)
        {
            string result = "";
            var help = "";

            help = _com.Value.valveURL + "valvesForSOA" +
            "?HospitalNo=" + hospitalNo +
            "&Soort=" + Type +
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