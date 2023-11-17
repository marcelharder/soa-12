using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api.Controllers
{

    [Authorize]
    public class FinalReport : BaseApiController
    {
        IWebHostEnvironment _env;
        private IOptions<ComSettings> _com;


        public FinalReport(IWebHostEnvironment env, IOptions<ComSettings> com)
        {
            _env = env;
            _com = com;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var id_string = id.ToString();
            return File(this.GetStream(id_string), "application/pdf", id_string + ".pdf");
        }

        [AllowAnonymous]
        [HttpGet("getRefReport/{hash}")]
        public async Task<IActionResult> getPdfForRefPhys(string hash)
        {
            var comaddress = _com.Value.reportURL;
            var st = "FinalReport/getRefReport/" + hash; // calls the previewcontroller and this results in a pdf or not
            comaddress = comaddress + st;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(comaddress))
                {
                    var help = await response.Content.ReadAsStringAsync();
                    if (help == "0") { return BadRequest("This report is not available"); }
                    else
                    {
                        return File(this.GetStream(help), "application/pdf", help + ".pdf");
                    };
                }
            }
        }

        private Stream GetStream(string id_string)
        {
            var pathToFile = _env.ContentRootPath + "/assets/pdf/";
            var file_name = pathToFile + id_string + ".pdf";
            var stream = new FileStream(file_name, FileMode.Open, FileAccess.Read);
            stream.Position = 0;
            return stream;
        }
    }
}
