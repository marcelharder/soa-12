using System;

using System.Security.Claims;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace api.Controllers
{
    
    [Authorize]
    public class RefPhysController : BaseApiController
    {
        
        private IRefPhys _ref;
        private IHttpContextAccessor _ht;

        SpecialMaps _mapper;
        private UserManager<AppUser> _manager;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public RefPhysController(
            SpecialMaps mapper,
            IRefPhys refphys, 
            IHttpContextAccessor ht,
            UserManager<AppUser> manager,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
             _ref = refphys;
            _mapper = mapper;
            _ht = ht;
            _manager = manager;
            _cloudinaryConfig = cloudinaryConfig;


            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);

        }

        [HttpGet]
        [Route("specificRefPhys/{id}", Name = "GetRefById")]
        public async Task<ActionResult> Get(int id)
        {
            var p = await _ref.getSpecificRefPhys(id);
            var dt = _mapper.mapToRefRhysForReturn(p);
            return Ok(p);
        }

        

        [HttpGet]
        [Route("AllRefPhys/{hospital_id}")]
        public async Task<ActionResult> GetAll(int hospital_id) //should return list of dropItems
        {
            var p = await _ref.getAllRefPhysInThisHospital(hospital_id);

            return Ok(p);
        }

        [HttpPut]
        [Route("updateRefPhys")]
        public async Task<ActionResult> updateRefPhys(refphysForUpdate cr) {
            var old = await _ref.getSpecificRefPhys(cr.Id);
            old = _mapper.mapToRefRhys(cr,old);
            var p = await _ref.updateRefPhys(old);
            return Ok(p);
        }

        [HttpDelete]
        [Route("deleteRefPhys/{id}")]
        public async Task<ActionResult> deleteRefPhys(int id)
        {
            var p = await _ref.deleteRefPhys(id);
            return Ok(p);
        }

        [HttpGet]
        [Route("addRefPhys")]
        [Authorize]
        public async Task<ActionResult> addRefPhys()
        {
            var p = await _ref.addRefPhys();
            var currentUserId = _ht.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var id = Convert.ToInt32(currentUserId);
            var currentUser = await _manager.Users.SingleOrDefaultAsync(x => x.Id == id);
                       
            p.hospital_id = currentUser.hospital_id;
            p.image = "https://res.cloudinary.com/marcelcloud/image/upload/v1559818775/user.png.jpg";

            p.send_email = false;
            p.active = false;

            if (await _ref.SaveAll()) {
                var refToReturn = _mapper.mapToRefRhysForReturn(p);
                return CreatedAtRoute("GetRefById", new { id = p.Id }, refToReturn);
            }

            return BadRequest("Could not add RefPhys");
        }

        [HttpPost("addPhoto/{id}")]
        public async Task<IActionResult> AddPhotoForRefPhys(int id, [FromForm]PhotoForCreationDto photoDto)
        {
            // if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var p = await _ref.getSpecificRefPhys(id);
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
               // p.image = uploadResult.Uri.ToString();
                 p.image = uploadResult.SecureUrl.AbsoluteUri;

               
                var r = await _ref.updateRefPhys(p);

                if (r == 1)
                { 
                    return CreatedAtRoute("GetRefById", new { id = p.Id }, p); 
                }

            }
            return BadRequest("Could not add the photo ...");
        }





    }
}