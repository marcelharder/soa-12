using System;
using System.Threading.Tasks;
using api.Entities;
using api.Helpers;
using api.interfaces.reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    
    [Authorize]

    public class SuggestionController : BaseApiController
    {
        private ISuggestion _repo;
        private IPV _previewReport;
        
        private SpecialMaps _sp;
        public SuggestionController(ISuggestion repo, SpecialMaps sp, IPV previewReport)
        {
            _repo = repo;
            _sp = sp;
            _previewReport = previewReport;
           
        }

        [HttpGet] // get all recorded suggestions for this user as class_items
        public async Task<IActionResult> Get()
        { 
            var p = await _repo.GetAllIndividualSuggestions();
            return Ok(p);
        }
       
        [HttpGet("{id}", Name = "GetSuggestion")] // gets recorded suggestion for this user by the soort
        public async Task<IActionResult> GetA(int id)
        {
            var p = await _repo.GetIndividualSuggestion(id);

            return Ok(p);
        }

        [HttpPut("{soort}")]
        public async Task<IActionResult> Put(Class_Preview_Operative_report cp, int soort) {

            // Save the preview report first
            int pvr_result = await _previewReport.updatePVR(cp);

            // get the current suggestion, if not available a new one is generated for this user and soort                     
            var current_suggestion = await _repo.GetIndividualSuggestion(soort );
            
            Class_Suggestion c = _sp.mapToSuggestionFromPreview(current_suggestion, cp, soort);
            
            var result = await _repo.updateSuggestion(c);
            return Ok(result);
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