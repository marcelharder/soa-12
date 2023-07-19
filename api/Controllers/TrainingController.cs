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
        public TrainingController(IOptions<ComSettings> com)
        {
            _com = com;
        }
        #region <!--epa -->
        [HttpGet("getEpaDefinition")]
        public async Task<IActionResult> getEpaDef()
        {

            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "drop/epadefinition";
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
        public async Task<IActionResult> updateSpecDocument(int docId, [FromBody] PdfForCreationDto up)
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
            content.Add(new StreamContent(photoDto.file.OpenReadStream()), photoDto.file.Name, photoDto.file.FileName);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = photoDto.file.Name, FileName = photoDto.file.FileName };

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
        public async Task<IActionResult> deleteItem(int docId){

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
     
    }
    class photoResult{
        public string document_url { get; set; }
        public string publicId { get; set; }
    }
}