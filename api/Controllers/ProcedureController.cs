using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.interfaces.reports;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
   
    [ServiceFilter(typeof(LogUserActivity))] // this records the last user activity
    [Authorize]
    
    public class ProcedureController : BaseApiController
    {
        private IProcedureRepository _rep;
        private IComposeFinalReport _fr;
        private UserManager<AppUser> _manager;
        private IPatientRepository _pat;
        private SpecialMaps _special;
        
        public ProcedureController(IProcedureRepository rep, 
            UserManager<AppUser> manager, 
            SpecialMaps special,
            IPatientRepository pat,
            IComposeFinalReport fr)
        {
            _rep = rep;
            _manager = manager;
            _pat = pat;
            _special = special;
            _fr = fr;
        }

        [HttpGet("refPhysEmailHash/{id}")]
        public async Task<IActionResult> gethashAsync(int id){

             // add item to timingsRefReport this happens when the report sms is send

             // first remove any previous record with this id in the timingsRefReport XML file
             await _fr.removeIdFromXML(id);

             ReportTiming r = new ReportTiming();
             r.id = id;
             r.publishTime = DateTime.Now;
             r.fileLocation = "";
             _fr.addToExpiredReports(r);

             // now calculate the hash which is the way the ref phys can find the report
             var p = await _rep.refPhysEmailHash(id);
             if(p != ""){return Ok(p);}
             return BadRequest("No Hash made, unfortunately");
        }
       [HttpGet]
        public async Task<IActionResult> Get([FromQuery]ProcedureParams p)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _manager.Users.SingleOrDefaultAsync( x => x.Id == currentUserId);
            // show only the procedures done in the current selected hospital
            if (p.selectedHospital == 0) {  p.selectedHospital = Convert.ToInt32(userFromRepo.hospital_id); }
                p.selectedSurgeon = userFromRepo.Id; // this will get only the procedures done by this surgeon
                
                var values = await _rep.GetProcedures(p);

                var l = new List<ProcedureListDTO>();
                foreach (Class_Procedure us in values) 
            { 
                l.Add(await _special.mapToProcedureListDTOAsync(us,1)); 
            }
                Response.AddPagination(values.Currentpage, values.PageSize, values.TotalCount, values.TotalPages);
                return Ok(l);
            }
        
        [HttpGet("assistedProcedures")]
        public async Task<IActionResult> GetAssisted([FromQuery]ProcedureParams p)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _manager.Users.SingleOrDefaultAsync( x => x.Id == currentUserId);
            // show only the procedures done in the current selected hospital
            if (p.selectedHospital == 0) {  p.selectedHospital = Convert.ToInt32(userFromRepo.hospital_id); }
                p.selectedSurgeon = userFromRepo.Id; // this will get only the procedures done by this surgeon
                
                var values = await _rep.GetAssistedProcedures(p);

                var l = new List<ProcedureListDTO>();
                foreach (Class_Procedure us in values) 
            { 
                l.Add(await _special.mapToProcedureListDTOAsync(us,0)); 
            }
                Response.AddPagination(values.Currentpage, values.PageSize, values.TotalCount, values.TotalPages);
                return Ok(l);
            }
            
        [HttpGet("aioProcedures")]
        public async Task<IActionResult> GetAioProcedures([FromQuery]ProcedureParams p)
        {
                var values = await _rep.GetAioProcedures(p);
                var l = new List<ProcedureListDTO>();
                foreach (Class_Procedure us in values){ 
                l.Add(await _special.mapToProcedureListDTOAsync(us,0)); 
            }
                Response.AddPagination(values.Currentpage, values.PageSize, values.TotalCount, values.TotalPages);
                return Ok(l);
            }

        [HttpGet("{id}", Name = "GetProcedure")]
        public async Task<ActionResult> Get(int id)
        {
            var p = await _rep.GetProcedure(id);
            return Ok(_special.mapToDTOFromClassProcedure(p));
        }

        [HttpPost("{id}/{patient_id}")]
         public async Task<ActionResult> PostProcedure(ProcedureDTO up, int id, int patient_id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

            var user = await _manager.Users.SingleOrDefaultAsync( x => x.Id == id);
            var ncp = new Class_Procedure();
            ncp.PatientId = patient_id;
            ncp.DateOfSurgery = DateTime.UtcNow;
            ncp.SelectedSurgeon = user.Id;
            ncp.refPhys = Convert.ToInt32(up.refPhys);
            ncp.hospital = user.hospital_id;
            ncp.fdType = up.fdType;
            ncp.Description = await _rep.getProdedureDescription(up.fdType);
            ncp.TotalTime = _special.CalculateTotalTime(DateTime.UtcNow, up.selectedStartHr, up.selectedStartMin, up.selectedStopHr, up.selectedStopMin);

            var selectedPatient = await _pat.GetPatientFromPatientId(patient_id);
            selectedPatient.procedures.Add(ncp);
            var result = await _pat.updatePatient(selectedPatient);
           
            if (result == 1)
             { 
                 var ProcedureForReturnDTO = _special.mapToDTOFromClassProcedure(ncp);
                 return CreatedAtRoute("GetProcedure", new { id = ncp.ProcedureId}, ProcedureForReturnDTO);
             }
            else { throw new Exception($"Adding procedure {id} failed on save");};
       }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProcedure(ProcedureDTO up, int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var hlp = _special.mapToClassProcedureFromDTO(up, await _rep.GetProcedure(up.procedureId));
            var result = await _rep.updateProcedure(hlp);
            if (result == 1) {
                //sync the procedure timing to patients.timing which is the euroscore
               await this.syncTimingAsync(id, hlp.SelectedTiming, hlp.SelectedUrgentTiming, hlp.SelectedEmergencyTiming, hlp.PatientId);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProcedure(string id)
        {
           var _id = Convert.ToInt32(id);
           return Ok(await _rep.DeleteAsync(_id));
        }

        private async Task<IActionResult> syncTimingAsync(int id, int timing, int SelectedUrgentTiming, int SelectedEmergencyTiming, int patientId) {
            var selectedPatient = await _pat.GetPatientClass(patientId);
            selectedPatient.timing = timing.ToString();
            selectedPatient.reason_urgent = SelectedUrgentTiming.ToString();
            selectedPatient.reason_emergent = SelectedEmergencyTiming.ToString();
            await _pat.updatePatient(selectedPatient);
            return Ok();
        }

        [HttpGet("loadButtonCapAndActions/{id}")]
        public async Task<IActionResult> getBAASoort(int id){
            var help = await _rep.getButtonsAndActions(id);
            return Ok(help); }

        
    }
}