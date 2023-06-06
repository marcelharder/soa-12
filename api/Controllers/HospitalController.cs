
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
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace api.Controllers
{
   
    [Authorize]
    public class HospitalController : BaseApiController
    {
        public IHospitalRepository _hos;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        private SpecialMaps _map;
        private UserManager<AppUser> _manager;



        public HospitalController(
        IHospitalRepository hos, 
        UserManager<AppUser> manager,
        SpecialMaps map, 
        IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _hos = hos;
            _map = map;
            _manager = manager;


            _cloudinaryConfig = cloudinaryConfig;
             

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
           _cloudinary = new Cloudinary(acc);

        }

        [HttpGet("allFullHospitals")]
        public async Task<IActionResult> getAllHospitals(){
            var ret = new List<HospitalForReturnDTO>();
            var result = await _hos.getAllFullHospitals();
            foreach(Class_Hospital ch in result){ret.Add(_map.mapToHospitalForReturn(ch));}
            return Ok(ret);
        }

        [HttpGet("allFullHospitalsPerCountry/{id}")]
        public async Task<IActionResult> getHospitalsperCountry(string id){
            // id is now bv 31 en moet NL worden
           // var iso_land = _map.getCountryFromCode(id);

            var ret = new List<HospitalForReturnDTO>();
            var result = await _hos.getAllFullHospitalsPerCountry(id);
            foreach(Class_Hospital ch in result){ret.Add(_map.mapToHospitalForReturn(ch));}
            return Ok(ret);
        }
    
        [HttpGet("{id}", Name = "GetHospital")]// get specific hospital details
        public async Task<IActionResult> GetHospital(int id)
        {
            var result = await _hos.GetSpecificHospital(id.ToString().makeSureTwoChar());
            return Ok(result);
        }
       
        [HttpGet("hospitalFromInventory/{id}")]// get specific hospital details from inventory
        public async Task<IActionResult> getHospitalNameFromInventory(int id){
            var result = await _hos.GetSpecificHospitalFromInventory(id.ToString().makeSureTwoChar());
            return Ok(result);
        } 

        [HttpGet("getHospitalNameFromId/{id}")]// get specific hospital details
        public async Task<IActionResult> GetHospitalName(int id)
        {
            var result = await _hos.GetSpecificHospital(id.ToString().makeSureTwoChar());
            return Ok(result.hospitalName);
        }

        [HttpPut]
        public async Task<IActionResult> PutHospitalAsync([FromBody] HospitalForReturnDTO hr)
        {
            var h = await _hos.getClassHospital(hr.hospitalNo);
            
            Class_Hospital ch = _map.mapToHospital(hr, h);
            return Ok(await _hos.updateHospital(ch));
        }

        [HttpPost("{id}/{no}")]
        public async Task<IActionResult> PostHospitalAsync(string id, int no)
        {
            Class_Hospital ch = new Class_Hospital();
            ch.Country = id;
            ch.HospitalNo = no.ToString().makeSureTwoChar();
            var new_hospital_number = await _hos.addHospital(ch);
            return CreatedAtRoute("GetHospital", new { id = new_hospital_number }, ch);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteHospitalAsync(string id)
        {
            var h = await _hos.getClassHospital(id);
            if (h != null){return Ok(await _hos.DeleteAsync(h));}
            return BadRequest("Hospital not found");
            
        }

        [HttpPost("addHospitalPhoto/{id}")]
        public async Task<IActionResult> AddPhotoForHospital(int id, [FromForm] PhotoForCreationDto photoDto)
        {
            var h = await _hos.getClassHospital(id.ToString().makeSureTwoChar());

            var file = photoDto.file;
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
                }
                h.ImageUrl = uploadResult?.SecureUrl?.AbsoluteUri;
                // automap it to class-hospital before save
                var no = await _hos.updateHospital(h);
                if (no == 1)
                {
                    return CreatedAtRoute("GetHospital", new { id = h.hospitalId }, h);
                }
            }
            return BadRequest("Could not add the photo ...");
        }

        [HttpGet("hospitalByUser/{id}")]
        public async Task<IActionResult> getCurrentHospitalForUser(int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            // get the hospitalId from the user
            var us = await _manager.Users.SingleOrDefaultAsync(x => x.Id == id);
            var result = await _hos.GetSpecificHospital(us.hospital_id.ToString().makeSureTwoChar());
            return Ok(result.hospitalName);
        }

        [HttpGet("IsThisHospitalImplementingOVI/{id}")]
        public async Task<IActionResult> getOVI(string id){
            return Ok(await _hos.HospitalImplementsOVI(id));
        }

        #region <!-- InstitutionalReports stuff -->
        [HttpGet("InstitutionalReport/{id}/{soort}")]
        public async Task<IActionResult> getIRep(string id, int soort){
          var help = await _hos.getInstitutionalReport(Convert.ToInt32(id), soort);
          return Ok(help);
        }
        [HttpPut("InstitutionalReport/{id}/{soort}")]
        public async Task<IActionResult> updateIRep([FromBody] InstitutionalDTO rep, string id, int soort){
             var help = await _hos.updateInstitutionalReport(rep,soort,Convert.ToInt32(id));
          return Ok(help);
        }
        [HttpPost("InstitutionalReport/{id}")]
        public async Task<IActionResult> createIRep(string id){
             var help = await _hos.createInstitutionalReport(Convert.ToInt32(id));
          return Ok(help);
        }

        [HttpGet("AdditionalReportItems/{id}/{soort}/{which}")]
        public async Task<IActionResult> getARI(string id, int soort,int which){
          var help = await _hos.getAdditionalReportItems(Convert.ToInt32(id),soort,which);
          return Ok(help);
        }

        [HttpPut("UpdateAdditionalReportItems/{id}/{soort}/{which}")]
        public async Task<IActionResult> getARk([FromBody] AdditionalReportDTO l,string id, int soort,int which){
          var help = await _hos.updateAdditionalReportItem(l,Convert.ToInt32(id),soort,which);
          return Ok(help);
        }

        #endregion

    }
}
