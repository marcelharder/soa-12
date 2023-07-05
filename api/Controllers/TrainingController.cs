using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var st = "Epa/Epas/" + userId;
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
            var st = "Epa/EpaDetails/" + Id;
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
            var st = "Epa/UpdateEpa";
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
            var st = "Epa/DeleteEpa/" + Id;
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
            var st = "pdf/files/" + userId;
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

        [HttpGet("createDocument/{userId}")]
        public async Task<IActionResult> createDocument(int userId)
        {
            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "Pdf/create-document/" + userId;
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
        public async Task<IActionResult> uploadPdf(int documentId,IFormFile file)
        {

           var content = new MultipartFormDataContent();
           foreach (var prop in file.GetType().GetProperties()){
            var value = prop.GetValue(file);
            if(value is FormFile){
                var f = value as FormFile;
                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, f.FileName);
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = prop.Name, FileName = f.FileName };
            }
           }


            var help = "";
            var comaddress = _com.Value.trainingURL;
            var st = "pdf/upload-pdf/" + documentId;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(comaddress, content))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok();
        }














        #endregion






    }
}