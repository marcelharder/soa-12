using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using api.Entities;
using api.Helpers;
using api.interfaces.reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace api.Controllers
{
    
    [Authorize]

    public class SuggestionController : BaseApiController
    {
        private ISuggestion _repo;
        private IOptions<ComSettings> _com;
        private IPV _previewReport;
        
        private SpecialMaps _sp;
        public SuggestionController(
            ISuggestion repo, 
            SpecialMaps sp, 
            IPV previewReport, 
            IOptions<ComSettings> com)
        {
            _repo = repo;
            _sp = sp;
            _previewReport = previewReport;
            _com = com;
           
        }

        [HttpGet] // get all recorded suggestions for this user as class_items
        public async Task<IActionResult> Get()
        { 
            var help = "";
            var currentUserId = _sp.getCurrentUserId();
            var comaddress = _com.Value.reportURL;
            var st = "Suggestion/" + currentUserId;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok(help);
           // var p = await _repo.GetAllIndividualSuggestions();
           // return Ok(p);
        }
       
        [HttpGet("{soort}", Name = "GetSuggestion")] // gets recorded suggestion for this user by the soort
        public async Task<IActionResult> GetA(int soort)
        {
           
            var help = "";
            var currentUserId = _sp.getCurrentUserId();
            var comaddress = _com.Value.reportURL;
            var st = "Suggestion/" + currentUserId + "/" + soort;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
           // var p = await _repo.GetIndividualSuggestion(id);

            return Ok(help);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Class_Preview_Operative_report cp) {

            var help = "";
            var currentUserId = _sp.getCurrentUserId();
            var comaddress = _com.Value.reportURL;
            var st = "Suggestion";
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



           /*  // Save the preview report first
            int pvr_result = await _previewReport.updatePVR(cp);

            // get the current suggestion, if not available a new one is generated for this user and soort                     
            var current_suggestion = await _repo.GetIndividualSuggestion(soort );
            
            Class_Suggestion c = _sp.mapToSuggestionFromPreview(current_suggestion, cp, soort);
            
            var result = await _repo.updateSuggestion(c);
            return Ok(result); */
        }

        [HttpPost]
        public async Task<IActionResult> Post(Class_Suggestion c)
        {
             var p = await _repo.AddIndividualSuggestion(c);
             if (await _repo.SaveAll())
            {
               return CreatedAtRoute("GetSuggestion", new { id = c.user }, p);
            }
            else { throw new Exception($"Adding suggestion {c.user} failed on save"); };

        }

    }
}