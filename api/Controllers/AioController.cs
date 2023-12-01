using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
  
    [Authorize]

    public class AioController : BaseApiController
    {

        private IAioRepo _repo;
        private UserManager<AppUser> _manager;

        private IUserRepository _user;
        private SpecialMaps _spec;

        public AioController(IAioRepo repo, UserManager<AppUser> manager, SpecialMaps spec, IUserRepository user)
        {
            _repo = repo;
            _manager = manager;
            _spec = spec;
            _user = user;
        }


        [HttpGet("Courses/{id}", Name = "GetCourse")]
        public async Task<IActionResult> GetCourses(int id)
        {
            var l = new List<CourseDetailsDto>();
            l = await _repo.getCourses(id.ToString());
            return Ok(l);
        }
        [HttpGet("CourseDetails/{id}")]
        public async Task<IActionResult> GetCourseDetails(int id)
        {
            var l = await _repo.getSpecificCourse(id.ToString());
            return Ok(l);
        }

        [HttpPost("AddCourse")]
     
        public async Task<IActionResult> AddCourseDetails(CourseDetailsDto cd)
        {
            var currentUserId = _spec.getCurrentUserId();
            cd.CourseId = currentUserId;

            var currentUser = await _manager.Users.SingleOrDefaultAsync(x => x.Id == currentUserId);
            var course = _spec.mapToCourse(cd, new Class_Course());
            currentUser.Courses.Add(course);
            if (await _user.SaveAll())
            {
                var ret = _spec.mapToCoursedto(course);
                return CreatedAtRoute("GetCourse", new { id = course.CourseId }, ret);
            }
            return BadRequest("Could not add Course ...");
        }
        [HttpPut("UpdateCourse")]
     
        public async Task<IActionResult> EditDetails(CourseDetailsDto cd)
        {
            var result = "";
            var help = await _repo.UpdateAsync(cd);
            if (help == 1) { result = "Update succeeded"; }
            return Ok(result);
        }
        [HttpDelete("DeleteCourse/{id}")]
        public async Task<IActionResult> DeleteDetails(int id)
        {
            var result = "";
            var help = await _repo.DeleteCourse(id);
            if (help == 1) { result = "Delete succeeded"; }
            return Ok(result);
        }

        [HttpGet("Epas/{id}")]
        public async Task<IActionResult> GetEpas(int id)
        {
            var l = new List<EpaDetailsDto>();
            l = await _repo.getEpas(id.ToString());
            return Ok(l);
        }
        
        [HttpGet("EpaDetails/{id}", Name = "GetEpa")]
        public async Task<IActionResult> GetEpaDetails(int id)
        {
            var l = await _repo.getEpa(id.ToString());
            return Ok(l);
        }
        
        [HttpPost("AddEpa")]
        public async Task<IActionResult> AddEpaDetails(EpaDetailsDto epd)
        {
            var currentUserId = _spec.getCurrentUserId();
            if (currentUserId != epd.Id) return Unauthorized();
            var currentUser = await _manager.Users.SingleOrDefaultAsync(x => x.Id == currentUserId);

            var epa = _spec.mapToEpa(epd, new Class_Epa());
            currentUser.Epa.Add(epa);
            if (await _user.SaveAll())
            {
                var ret = _spec.mapToepadto(epa);
                return CreatedAtRoute("GetEpa", new { id = epa.EpaId }, ret);
            }
            return BadRequest("Could not add EPA ...");
        }
        
        [HttpPut("UpdateEpa")]
        public async Task<IActionResult> EditEpasDetails(EpaDetailsDto epd) {
             var result = "";
            var help = await _repo.UpdateAsync(epd);
            if (help == 1) { result = "Update succeeded"; }
            return Ok(result);
        }
        
        [HttpDelete("DeleteEpa/{id}")]
        public async Task<IActionResult> DeleteEpaDetails(int id)
        {
            var result = "";
            var help = await _repo.DeleteEpa(id);
            if (help == 1) { result = "Delete succeeded"; }
            return Ok(result);
        }

    }
}
