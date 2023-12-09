using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using api.Interfaces.signalR;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace api.Controllers
{

    [ServiceFilter(typeof(LogUserActivity))] // this records the last user activity
    [Authorize]

    public class UsersController : BaseApiController
    {
        private readonly IOptions<ComSettings> _com;

        private IUserRepository _rep;
        private IUserOnline _online;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        private SpecialMaps _mapper;
        private readonly UserManager<AppUser> _manager;


        public UsersController(
            IOptions<ComSettings> com,
            UserManager<AppUser> manager,
            IUserOnline online,
            IUserRepository rep,
            SpecialMaps map,
            DataContext context,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _rep = rep;
            _mapper = map;
            _online = online;
            _cloudinaryConfig = cloudinaryConfig;
            _manager = manager;
            _com = com;




            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] UserParams userParams)
        {
            var values = await _rep.GetUsers();
            var l = new List<UserForReturnDto>();
            foreach (AppUser us in values)
            {
                l.Add(_mapper.mapToUserForReturn(us));
            }
            //Response.AddPagination(values.Currentpage, values.PageSize, values.TotalCount, values.TotalPages);
            return Ok(l);
        }


        [HttpGet("getUsersByHospital")]
        public async Task<IActionResult> GetByHospital([FromQuery] UserParams userParams)
        {
            var values = await _rep.GetUsersByHospital(userParams);
            var l = new List<UserForReturnDto>();
            foreach (AppUser us in values)
            {
                l.Add(_mapper.mapToUserForReturn(us));
            }
            Response.AddPagination(values.Currentpage, values.PageSize, values.TotalCount, values.TotalPages);
            return Ok(l);
        }
        [HttpGet("getAiosByHospital")]
        public async Task<IActionResult> GetAByHospital([FromQuery] UserParams userParams)
        {
            var values = await _rep.GetAiosByHospital(userParams);
            if (values.Count != 0)
            {
                var l = new List<UserForReturnDto>();
                foreach (AppUser us in values)
                {
                    l.Add(_mapper.mapToUserForReturn(us));
                }
                return Ok(l);
            }
            return BadRequest("No trainees found");
        }
        [HttpGet("getSurgeonsByHospital")]
        public async Task<IActionResult> GetSurgeonsByHospital([FromQuery] UserParams userParams)
        {
            var values = await _rep.GetSurgeonsByHospital(userParams);
            var l = new List<UserForReturnDto>();
            foreach (AppUser us in values)
            {
                l.Add(_mapper.mapToUserForReturn(us));
            }
            return Ok(l);
        }
        [HttpGet("getChefsByHospital/{centerId}")]
        public async Task<IActionResult> GetChefsByHospital(int centerId)
        {
            var chef = await _rep.GetChefsByHospital(centerId);
            if (chef.hospital_id != 9999)
            {
                return Ok(_mapper.mapToUserForReturn(chef));
            }
            return BadRequest("No chef found in this hospital ...");
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> addUser(UserForRegisterDto ufr)
        {
            var user = await _manager.Users.SingleOrDefaultAsync(x => x.UserName == ufr.UserName.ToLower());
            if (user != null) { return BadRequest("User already exists ..."); }

            user = new AppUser
            {

                UserName = ufr.UserName.ToLower(),
                Country = ufr.country,
                City = ufr.city,
                hospital_id = Convert.ToInt32(ufr.hospital_id),
                worked_in = ufr.hospital_id.ToString().makeSureTwoChar(),
                KnownAs = ufr.knownAs,
                Created = DateTime.Now,
                LastActive = DateTime.Now,
                PaidTill = DateTime.Now.AddDays(30),
                Email = ufr.UserName.ToLower(),
                Gender = "Male",
                Mobile = ufr.mobile,
                active = ufr.active,
                ltk = ufr.ltk,
                PhotoUrl = "https://randomuser.me/api/portraits/men/75.jpg"
            };

            var result = await _manager.CreateAsync(user, ufr.password);
            if (!result.Succeeded) { return BadRequest(result.Errors); }

            var roleResult = await _manager.AddToRoleAsync(user, "Surgery");
            if (!roleResult.Succeeded) { return BadRequest(roleResult.Errors); }

            UserForReturnDto ufre = _mapper.mapToUserForReturn(user);
            return CreatedAtRoute("GetUser", new { id = user.Id }, ufre);
        }

        private async Task<string> GetSpecificHospitalAsync(string currentHospital)
        {
            var test = "";
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital/getHospitalNameFromId";
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    test = await response.Content.ReadAsStringAsync();
                }
            }
            return test;
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult> GetUser(int id)
        {

            var user = await _rep.GetUser(id);
            return Ok(_mapper.mapToUserForReturn(user));
        }

        [HttpGet("ltk/{id}")]
        public async Task<ActionResult> GetUserLtk(int id)
        {
            if (id == 0) { return BadRequest("Id can not be zero ..."); }
            return Ok(await _rep.GetUserLtk(id));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(UserForUpdateDto up, int id)
        {
            // if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

            var user = await _rep.GetUser(up.Id);
            if (user.Country == null)
            {
                // get the country from the hospital_id
            }
            var userupdated = _mapper.mapToUser(up, user);
            _rep.Update(userupdated);
            if (await _rep.SaveAll()) return NoContent();
            throw new Exception($"Updating user {up.Id} failed on save");

        }

        [HttpPost("addUserPhoto/{id}")]
        public async Task<IActionResult> AddPhotoForUser(int id, [FromForm] PhotoForCreationDto photoDto)
        {
            var user = await _rep.GetUser(id);

            var file = photoDto.File;
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
                user.PhotoUrl = uploadResult?.SecureUrl?.AbsoluteUri;


                if (await _rep.SaveAll())
                {
                    UserForReturnDto ufr = _mapper.mapToUserForReturn(user);
                    return CreatedAtRoute("GetUser", new { id = user.Id }, ufr);
                }

            }
            return BadRequest("Could not add the photo ...");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _rep.GetUser(id);
            _rep.Delete(user);
            if (await _rep.SaveAll()) return Ok("User deleted ...");
            return BadRequest("Deleting failed ...");
        }

        [HttpGet("users-online")]
        public async Task<IActionResult> getUsersOnline()
        {
            return Ok(await _online.getOnlineUsers());
        }

        [HttpPost("addPayment/{id}")]
        public async Task<IActionResult> addPayment(int id, [FromQuery] DateTime d)
        {
            var user = await _rep.GetUser(id);
            if (await _rep.UpdatePayment(d, id)) { return Ok("Payment updated ..."); }
            return BadRequest("Updating payment went wrong");
        }

        [HttpGet("newUser/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> getNewUser(int id)
        {
            return Ok(await GetUser(id));
        }








    }
}