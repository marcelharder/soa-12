using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using api.DTOs;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace api.Controllers
{
    [Authorize]

    public class TrainingController : BaseApiController
    {
        private IOptions<ComSettings> _com;
         private IUserRepository _rep;
        public TrainingController(IOptions<ComSettings> com, IUserRepository rep)
        {
            _com = com;
            _rep = rep;
        }
        #region <!--epa -->
        [HttpGet("getEpaDefinition")]
        public async Task<IActionResult> getEpaDef()
        {

            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "Drop/epadefinition";
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
      
        [HttpGet("getListEpaas/{userId}")]
        public async Task<IActionResult> getEpaList(int userId)
        {

            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "Epa/dapper/Epas/" + userId;
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
      
        [HttpGet("getEpaDetails/{id}")]
        public async Task<IActionResult> getEpaDetails(int Id)
        {
            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "Epa/dapper/EpaDetails/" + Id;
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
     
        [HttpPut("updateEpa")]
        public async Task<IActionResult> Update([FromBody] EpaDetailsDto ep)
        {
            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "Epa/dapper/UpdateEpa";
            comaddress = comaddress + st;
            var json = JsonConvert.SerializeObject(ep, Formatting.None);
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
      
        [HttpGet("deleteEpa/{id}")]
        public async Task<IActionResult> deleteEpaDetails(int Id)
        {

            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "Epa/dapper/DeleteEpa/" + Id;
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

        #region <!--courses-->
        [HttpGet("getListCourses/{userId}")]
        public async Task<IActionResult> getCourseList(int userId)
        {
            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "Course/dapper/files/" + userId;
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
        [HttpGet("getSpecificCourse/{CourseId}", Name = "GSC")]
        public async Task<IActionResult> getSpecCourse(int CourseId)
        {
            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "Course/dapper/specificCourse/" + CourseId;
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
       
        [HttpPost("createCourse/{userId}")]
        public async Task<IActionResult> createCourse(int userId)
        {
            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "Course/dapper/create_course/" + userId;
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
   
        [HttpPut("updateCourse")]
        public async Task<IActionResult> updateSpecCourse(int docId, [FromBody] CourseDetailsDto up)
        {
            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "Course/dapper/update_course";
            comaddress = comaddress + st;
            var json = JsonConvert.SerializeObject(up, Formatting.None);
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

        [HttpDelete("deleteCourse/{CourseId}")]
        public async Task<IActionResult> deleteItem(int CourseId){

             var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "Course/dapper/delete_course/" + CourseId;
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
      
        #region <!--documents-->
        [HttpGet("getListDocuments/{userId}")]
        public async Task<IActionResult> getDocumentList(int userId)
        {
            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "pdf/dapper/files/" + userId;
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

        [HttpGet("getSpecificFile/{docId}", Name = "GSP")]
        public async Task<IActionResult> getSpecDocument(int docId)
        {
            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "pdf/dapper/specificfile/" + docId;
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

        [HttpPut("updateDocument")]
        public async Task<IActionResult> updateSpecDocument([FromBody] PdfForCreationDto up)
        {
            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "pdf/dapper/update_document";
            comaddress = comaddress + st;
            var json = JsonConvert.SerializeObject(up, Formatting.None);
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

        [HttpPost("createDocument/{userId}")]
        public async Task<IActionResult> createDocument(int userId)
        {
            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "pdf/dapper/create_document/" + userId;
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
       
        [HttpGet("listDocument/{userId}")]
        public async Task<IActionResult> listDocument(int userId)
        {
            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "pdf/dapper/files/" + userId;
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

        [HttpPost("uploadPdf/{documentId}")] 
        public async Task<IActionResult> uploadPdf(int documentId, [FromForm]PhotoForCreationDto photoDto)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(photoDto.File.OpenReadStream()), photoDto.File.Name, photoDto.File.FileName);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = photoDto.File.Name, FileName = photoDto.File.FileName };

            var help = new photoResult();
            var comaddress = _com.Value.trainingURL;
            var st = "pdf/dapper/upload-pdf/" + documentId;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(comaddress, content))
                {
                   // var ger = await response.Content.ReadAsStringAsync();
                    help = await response.Content.ReadFromJsonAsync<photoResult>();
                }
            }
            return Ok(help.document_url); 
        }
        
        
        [HttpDelete("deleteDocument/{docId}")]
        public async Task<IActionResult> deleteDocument(int docId){

            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "pdf/dapper/delete_document/" + docId;
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
     

        #region <!--procedures-->

        [HttpGet("getProcedures/{surgeonId}")]
        public async Task<IActionResult> getProcedureList(int surgeonId)
        {
            // get the loggedin surgeon so we can get the hospital number
            var logged_in_surgeon = await _rep.GetUser(surgeonId);

            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "Procedure/procedures/" + surgeonId + "/" + logged_in_surgeon.hospital_id;
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
      
        [HttpGet("getProcedureDetails/{id}")]
        public async Task<IActionResult> getProcedureDetails(int Id)
        {
            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "Procedure/procedureDetails/" + Id;
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
    }
    class photoResult{
        public string document_url { get; set; }
        public string image { get; set; }
        public string publicId { get; set; }
    }
}