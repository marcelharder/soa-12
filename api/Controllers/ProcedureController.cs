using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace api.Controllers
{

    [ServiceFilter(typeof(LogUserActivity))] // this records the last user activity
    [Authorize]

    public class ProcedureController : BaseApiController
    {
        private IProcedureRepository _rep;
        private Cloudinary _cloudinary;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private UserManager<AppUser> _manager;
        private IPatientRepository _pat;
        private SpecialMaps _special;
        private IOptions<ComSettings> _com;

        public ProcedureController(IProcedureRepository rep,
            IOptions<CloudinarySettings> cloudinaryConfig,
            UserManager<AppUser> manager,
             SpecialMaps special,
             IOptions<ComSettings> com,
            IPatientRepository pat)
        {
            _rep = rep;
            _manager = manager;
            _pat = pat;
            _special = special;
            _com = com;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
               _cloudinaryConfig.Value.CloudName,
               _cloudinaryConfig.Value.ApiKey,
               _cloudinaryConfig.Value.ApiSecret
           );
            _cloudinary = new Cloudinary(acc);

        }

        [HttpGet("refPhysEmailHash/{id}")]
        public async Task<IActionResult> gethashAsync(int id)
        {
            // now calculate the hash which is the way the ref phys can find the report
            var p = await _rep.refPhysEmailHash(id);
            if (p != "") { return Ok(p); }
            return BadRequest("No Hash made, unfortunately");
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProcedureParams p)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _manager.Users.SingleOrDefaultAsync(x => x.Id == currentUserId);
            // show only the procedures done in the current selected hospital
            if (p.selectedHospital == 0) { p.selectedHospital = Convert.ToInt32(userFromRepo.hospital_id); }
            p.selectedSurgeon = userFromRepo.Id; // this will get only the procedures done by this surgeon

            var values = await _rep.GetProcedures(p);

            var l = new List<ProcedureListDTO>();
            foreach (Class_Procedure us in values)
            {
                l.Add(await _special.mapToProcedureListDTOAsync(us, 1));
            }
            Response.AddPagination(values.Currentpage, values.PageSize, values.TotalCount, values.TotalPages);
            return Ok(l);
        }

        [HttpGet("assistedProcedures")]
        public async Task<IActionResult> GetAssisted([FromQuery] ProcedureParams p)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _manager.Users.SingleOrDefaultAsync(x => x.Id == currentUserId);
            // show only the procedures done in the current selected hospital
            if (p.selectedHospital == 0) { p.selectedHospital = Convert.ToInt32(userFromRepo.hospital_id); }
            p.selectedSurgeon = userFromRepo.Id; // this will get only the procedures done by this surgeon

            var values = await _rep.GetAssistedProcedures(p);

            var l = new List<ProcedureListDTO>();
            foreach (Class_Procedure us in values)
            {
                l.Add(await _special.mapToProcedureListDTOAsync(us, 0));
            }
            Response.AddPagination(values.Currentpage, values.PageSize, values.TotalCount, values.TotalPages);
            return Ok(l);
        }

        [HttpGet("aioProcedures")]
        public async Task<IActionResult> GetAioProcedures([FromQuery] ProcedureParams p)
        {
            var values = await _rep.GetAioProcedures(p);
            var l = new List<ProcedureListDTO>();
            foreach (Class_Procedure us in values)
            {
                l.Add(await _special.mapToProcedureListDTOAsync(us, 0));
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

            var user = await _manager.Users.SingleOrDefaultAsync(x => x.Id == id);
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
                return CreatedAtRoute("GetProcedure", new { id = ncp.ProcedureId }, ProcedureForReturnDTO);
            }
            else { throw new Exception($"Adding procedure {id} failed on save"); };
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProcedure(ProcedureDTO up, int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var hlp = _special.mapToClassProcedureFromDTO(up, await _rep.GetProcedure(up.procedureId));
            var result = await _rep.updateProcedure(hlp);
            if (result == 1)
            {
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

        private async Task<IActionResult> syncTimingAsync(int id, int timing, int SelectedUrgentTiming, int SelectedEmergencyTiming, int patientId)
        {
            var selectedPatient = await _pat.GetPatientClass(patientId);
            selectedPatient.timing = timing.ToString();
            selectedPatient.reason_urgent = SelectedUrgentTiming.ToString();
            selectedPatient.reason_emergent = SelectedEmergencyTiming.ToString();
            await _pat.updatePatient(selectedPatient);
            return Ok();
        }

        [HttpGet("loadButtonCapAndActions/{id}")]
        public async Task<IActionResult> getBAASoort(int id)
        {
            var help = await _rep.getButtonsAndActions(id);
            return Ok(help);
        }

        #region ProcedurePhotos

        [HttpGet("photosAvailable/{id}")]
        public async Task<IActionResult> AreThereProcedurePhotos(int id)
        {
            // ask the PFSOA
            var h = "";
            var comaddress = _com.Value.pfsoaURL;
            var st = "photosAvailable/" + id;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    h = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok(h);
        }

        [HttpPost("addProcedurePhoto/{'id'}")]
        public async Task<IActionResult> AddPhotoForUser(int id, [FromForm] PhotoForCreationDto photoDto)
        {
            var file = photoDto.File;
            photoDto.procedureId = id;
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                    photoDto.Url = uploadResult?.SecureUrl?.AbsoluteUri;
                    photoDto.PublicId = uploadResult.PublicId;
                }
                return Ok(photoDto);
            }
            return BadRequest("Could not add the photo ...");
        }

        [HttpPost("addPhotoToPfSoa")]
        public async Task<IActionResult> AddPhotoForUserToPFSoa(PhotoForCreationDto photoDto)
        {
            var help = "";
            var comaddress = _com.Value.pfsoaURL;
            var st = "addPhoto";
            comaddress = comaddress + st;
            var json = JsonConvert.SerializeObject(photoDto, Formatting.None);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(comaddress, content))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok(help);
        }


        [HttpGet("getAllPhotos/{id}")]
        public async Task<IActionResult> GetProcedurePhotos(int id)
        {
            // ask the PFSOA
            var help = "";
            var comaddress = _com.Value.pfsoaURL;
            var st = "getPhotos/" + id;
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

        [HttpDelete("deletePhoto/{publicId}")]
        public async Task<IActionResult> deletePhoto(int publicId)
        {
            var help = "";
            var comaddress = _com.Value.pfsoaURL;
            var st = "deletePhoto/" + publicId;
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

        #endregion

    }
}