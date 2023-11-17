using System;
using System.Threading.Tasks;
using api.DTOs;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using api.Data;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace api.Controllers
{
    [Authorize]
    public class PreviewReportController : BaseApiController
    {

        private IOptions<ComSettings> _com;
        private IValveRepository _valve;
        private ICABGRepository _cabg;
        private IProcedureRepository _proc;
        private IWebHostEnvironment _env;

        public PreviewReportController(
            IOptions<ComSettings> com,
            ICABGRepository cabg,
            IValveRepository valve,
           IProcedureRepository proc, IWebHostEnvironment env)
        {
            _proc = proc;
            _cabg = cabg;
            _valve = valve;
            _com = com;
            _env = env;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var help = "";
            var comaddress = _com.Value.reportURL;
            var st = "PreViewReport/" + id;
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    help = await response.Content.ReadAsStringAsync();
                    return Ok(help);


                }
            }

        }

        [HttpGet("reset/{id}")]
        public async Task<IActionResult> Reset(int id)
        {
            var help = "";
            var comaddress = _com.Value.reportURL;
            var st = "PreViewReport/reset/" + id;
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

        [HttpGet("isFinalReportReady/{id}")]
        public async Task<int> IsReportReady(int id)
        {
            var result = 0;
            var procedure = await _proc.GetProcedure(id); if (procedure == null) { return result; }
            var reportcode = Convert.ToInt32(getReportCode(procedure.fdType));


            if (reportcode == 1 || reportcode == 2) // these are the cabg procedures
            {
                var selectedCabg = await _cabg.GetSpecificCABG(id);
                if (selectedCabg.art01 != null) { result = 1; } else { result = 2; };
            }
            if (reportcode == 3 || reportcode == 4 || reportcode == 5)// these are the valve procedures
            {
                var selectedValveArray = await _valve.getValvesFromProcedure(id);
                if (selectedValveArray.Any(cv => cv.SERIAL_IMP != null)) { result = 3; } else { result = 4; }
            }
            // so
            // 1 means cabg properly filled, 2 means cabg not available
            // 3 means valve properly filled, 4 means valve not available
            // 0 means no procedure found
            return result;

        }

        [HttpPost]
        public async Task<IActionResult> Post(PreviewForReturnDTO pvfr)
        { // this comes from the save and print button

            var help = "";
            var comaddress = _com.Value.reportURL;
            var st = "PreViewReport";
            comaddress = comaddress + st;
            var json = JsonConvert.SerializeObject(pvfr, Formatting.None);
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
        private string getReportCode(int fdType)
        {
            var result = "";
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "conf/procedure.xml");
            XDocument order = XDocument.Load(filename);
            IEnumerable<XElement> help = from d in order.Descendants("Code")
                                         where d.Element("ID").Value == fdType.ToString()
                                         select d;
            foreach (XElement x in help) { result = x.Element("report_code").Value; }
            return result;
        }
    }
}