using System.IO;
using System.Threading.Tasks;
using api.interfaces.reports;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [Authorize]
    public class FinalOperativeReport : BaseApiController
    {
        private readonly IWebHostEnvironment _env;
        private IProcedureRepository _proc;
        private IManageFinalReport _impdf;


        public FinalOperativeReport(
            IWebHostEnvironment env,
            IManageFinalReport impdf,

            IProcedureRepository proc)
        {
            _env = env;
            _impdf = impdf;
            _proc = proc;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
           return File(this.GetStream(id.ToString()), "application/pdf", $"{id}.pdf");
        }

        [AllowAnonymous]
        [HttpGet("getRefReport/{hash}")]
        public async Task<IActionResult> getPdfForRefPhys(string hash)
        {
            _impdf.deleteExpiredReports(); // delete expired reports 
            var id = await _proc.getProcedureIdFromHash(hash);
            if (id == 0 || await _impdf.pdfDoesNotExists(id.ToString()))
            {
                return BadRequest("Your operative report is not found or expired ...");
            }
            return File(this.GetStream(id.ToString()), "application/pdf", $"{id}.pdf");
        }

        [AllowAnonymous]
        [HttpGet("deleteExpiredReports")]
        public IActionResult deleteExpiredReports()
        {
            var help = 0;
            help = _impdf.deleteExpiredReports();
            if (help == 2) { return BadRequest(new { message = "Something went wrong in removing the expired reports" }); }
            return Ok("Success");
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
