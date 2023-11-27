
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace api.Controllers
{

    [Authorize]
    public class HospitalController : BaseApiController
    {

        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        private SpecialMaps _map;
        private UserManager<AppUser> _manager;
        private IOptions<ComSettings> _com;



        public HospitalController(

        IOptions<ComSettings> com,
        UserManager<AppUser> manager,
        SpecialMaps map,
        IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _com = com;
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


        #region <!--Hospitals-->


        [HttpGet("allFullHospitals")]
        public async Task<IActionResult> getAllHospitals()
        {
            var help = "";
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital/allFullHospitals";
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok(help);
            /*  var ret = new List<HospitalForReturnDTO>();
             var result = await _hos.getAllFullHospitals();
             foreach (Class_Hospital ch in result) { ret.Add(_map.mapToHospitalForReturn(ch)); }
             return Ok(ret); */
        }

        [HttpGet("allFullHospitalsPerCountry/{id}")]
        public async Task<IActionResult> getHospitalsperCountry(string id)
        {
            var help = "";
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital/allFullHospitalsPerCountry/" + id;
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

        [HttpGet("{id}", Name = "GetHospital")]// get specific hospital details
        public async Task<IActionResult> GetHospital(int id)
        {
            var help = "";
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital/getHospital/" + id;
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

       /*  [HttpGet("hospitalFromInventory/{id}")]// get specific hospital details from inventory
        public async Task<IActionResult> getHospitalNameFromInventory(int id)
        {
            var help = "";
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital/getHospitalById/" + id;
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
 */
       
        [HttpGet("getHospitalNameFromId/{id}")]// get specific hospital details
        public async Task<IActionResult> GetHospitalName(string id)
        {
            var help = "";
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital/getHospitalName/" + id;
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

        [HttpPut]
        public async Task<IActionResult> PutHospitalAsync([FromBody] HospitalForReturnDTO hr)
        {
            var help = "";
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital";
            comaddress = comaddress + st;
            var json = JsonConvert.SerializeObject(hr, Formatting.None);
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

        [HttpPost("{country}/{no}")]
        public async Task<IActionResult> PostHospitalAsync(string country, int no)
        {
            var help = "";
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital/" + country + "/" + no;
            comaddress = comaddress + st;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(comaddress, null))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok(help);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteHospitalAsync(string id)
        {
            var help = "";
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital/" + id;
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

        [HttpPost("addHospitalPhoto/{id}")]
        public async Task<IActionResult> AddPhotoForHospital(int id, [FromForm] PhotoForCreationDto photoDto)
        {
            var help = "";
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital/addHospitalPhoto/" + id;
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

        #endregion

        #region <!--Country -->

        [HttpPost("addCountryNow")]
        public async Task<IActionResult> AddCountryNow(CountryDto model)
        {
            var help = "";
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital/getHospitalById/";
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

        [HttpGet("hospitalByUser/{id}")]
        public async Task<IActionResult> getCurrentHospitalForUser(int id)
        {
            var help = "";
            var comaddress = _com.Value.hospitalURL;
            var st = "Hospital/getHospitalById/" + id;
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

        [HttpGet("IsThisHospitalImplementingOVI/{id}")]
        public async Task<IActionResult> getOVI(string id)
        {
            var help = "";
            var comaddress = _com.Value.reportURL;
            var st = "Hospital/isusigOVI/" + id;
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

        #endregion


        #region <!-- InstitutionalReports stuff -->

        [HttpGet("InstitutionalReport/{hospitalId}/{soort}")]
        public async Task<IActionResult> getIRepAsync(string hospitalId, int soort)
        {
            var help = "";
            var comaddress = _com.Value.reportURL;
            var st = "InstitutionalReport/" + hospitalId + "/" + soort;
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

        [HttpPut("InstitutionalReport/{hospitalId}/{soort}")]
        public async Task<IActionResult> updateIRepAsync([FromBody] InstitutionalDTO cp, string hospitalId, int soort)
        {
            var help = "";
            var comaddress = _com.Value.reportURL;
            var st = "InstitutionalReport/" + hospitalId + "/" + soort;
            comaddress = comaddress + st;
            var json = JsonConvert.SerializeObject(cp, Formatting.None);
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


        [HttpGet("AdditionalReportItems/{hospitalId}/{which}")]
        public async Task<IActionResult> getARIAsync(string hospitalId, int which)
        {
            var help = "";
            var comaddress = _com.Value.reportURL;
            var st = "InstitutionalReport/AdditionalReportitems/" + hospitalId + "/" + which;
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

        [HttpPut("UpdateAdditionalReportItems/{hospitalId}/{which}")]
        public async Task<IActionResult> updateIRepAsync([FromBody] AdditionalReportDTO cp, string hospitalId, int which)
        {
            var help = "";
            var comaddress = _com.Value.reportURL;
            var st = "InstitutionalReport/AdditionalReportitems/" + hospitalId + "/" + which;
            comaddress = comaddress + st;
            var json = JsonConvert.SerializeObject(cp, Formatting.None);
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

        #endregion

    }
}
