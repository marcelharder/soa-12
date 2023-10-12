﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using api.Entities;
using api.Helpers;
using api.interfaces.reports;
using api.Interfaces;
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
        private IProcedureRepository _proc;
        private SpecialReportMaps _sprep;
        private IOptions<ComSettings> _com;
        private IPV _previewReport;
        
        private SpecialMaps _sp;
        public SuggestionController(
            SpecialReportMaps sprep,
            IProcedureRepository proc,
            ISuggestion repo, 
            SpecialMaps sp, 
            IPV previewReport, 
            IOptions<ComSettings> com)
        {
            _repo = repo;
            _sp = sp;
            _previewReport = previewReport;
            _com = com;
            _proc = proc;
            _sprep = sprep;
           
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
        }
       
        [HttpGet("{soort}")] // gets recorded suggestion for this user by the soort
        public async Task<IActionResult> GetSuggestion(int soort)
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
            return Ok(help);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Class_Preview_Operative_report cp) {
            var currentprocedure = await _proc.GetProcedure(cp.procedure_id);
            var help = "";
            var currentUserId = _sp.getCurrentUserId();
            var comaddress = _com.Value.reportURL;
            var st = "Suggestion/" + currentUserId + "/" + currentprocedure.fdType;
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

        [HttpPost]
        public async Task<IActionResult> Post(Class_Suggestion c)
        {
            var help = "";
            var comaddress = _com.Value.reportURL;
            comaddress = comaddress + "Suggestion";
            var json = JsonConvert.SerializeObject(c, Formatting.None);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(comaddress,content))
                {
                    help = await response.Content.ReadAsStringAsync();
                }
            }
            return Ok();
        }

    }
}