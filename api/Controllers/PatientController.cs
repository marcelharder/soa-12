using System;
using System.Security.Claims;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
   
    [Authorize]
    public class PatientController : BaseApiController
    {
        private IPatientRepository _rep;
        private UserManager<AppUser> _manager;
        private SpecialMaps _special;

        public PatientController(
            IPatientRepository rep,
            UserManager<AppUser> manager,
            SpecialMaps special)
        {
            _rep = rep;
            _manager = manager;
            _special = special;

        }

        [HttpGet("{id}", Name = "GetPatient")]
        public async Task<ActionResult> Get(int id)  { return Ok(await _rep.GetPatient(id)); }
        [HttpGet("patientFromMRN/{mrn}")]
        public async Task<ActionResult> GetFromMRN(string mrn) { return Ok(await _rep.GetPatientFromMRN(mrn)); }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PatientParams p)
        {
            var values = await _rep.GetPatients(p);
            Response.AddPagination(values.Currentpage, values.PageSize, values.TotalCount, values.TotalPages);
            return Ok(values);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Post(PatientToAddDTO p, int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            Class_Patient cp = await _rep.AddPatientFromMRN(p.mrn);
            cp.MRN = p.mrn;
            cp.Age = p.age;
            cp.gender = p.gender;
            cp.creat_number = p.creat_number;
            cp.weight = p.weight.ToString();
            cp.height = p.height.ToString();
            
            var result = await _rep.updatePatient(cp);
            if (result == 1)
            {
                var patientToReturn = _special.mapToPatientForReturn(cp);
                return CreatedAtRoute("GetPatient", new { id = cp.PatientId }, patientToReturn);
            }
            else { throw new Exception($"Adding patient {id} failed on save"); };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(FullPatientDTO p, int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
                     
            var oldPatient = await _rep.GetPatientClass(p.patientId);
            var result = await _rep.updatePatient(_special.mapFromPatientDTOToPatient(p, oldPatient));

            return Ok(result);
        }
    
        [HttpGet]
        [Route("patientFromProcedureId/{id}")]
        public async Task<ActionResult> GetFromProc(int id)
        {
            var p = await _rep.GetPatientFromProcedureId(id);
            return Ok(p);
        }
    }

}