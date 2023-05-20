using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
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
        private IComposeFinalReport _fr;

      
        public FinalOperativeReport(
            IWebHostEnvironment env,
            IComposeFinalReport fr,
            
            IProcedureRepository proc)
        {
            _env = env;
            _fr = fr;
            _proc = proc;
          

        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _fr.deleteExpiredReports();// delete expired reports
            var help = _fr.deletePDF(id);
            var id_string = id.ToString();

            await _fr.composeAsync(id); //get the final report and composes a pdf, which is stored in assets/pdf/73764743.pdf
            return File(this.GetStream(id_string), "application/pdf", id_string + ".pdf");

        }

        [AllowAnonymous]
        [HttpGet("getRefReport/{hash}")]
        public async Task<IActionResult> getPdfForRefPhys(string hash)
        {
            _fr.deleteExpiredReports();// delete expired reports

            var id = await _proc.getProcedureIdFromHash(hash);
            if (id != 0)
            {
                

                if (await _proc.IsThisReportNotExpired(id))
                {
                    var id_string = id.ToString();
                    if (await _proc.pdfDoesNotExists(id_string))
                    {
                        await _fr.composeAsync(id); //get the final report and composes a pdf, which is stored in bv. assets/pdf/73764743.pdf
                    }
                    return File(this.GetStream(id_string), "application/pdf", id_string + ".pdf");
                }
                else
                {
                    var help = _fr.deletePDF(id);
                    return BadRequest("Your operative report is expired ...");
                }
            }
            else { return BadRequest("Your operative report is not found or expired ..."); }
        }

        [AllowAnonymous]
        [HttpGet("deleteExpiredReports")]
        public IActionResult deleteExpiredReports(){
            var help = 0;
            help = _fr.deleteExpiredReports();
            if (help == 2){return BadRequest(new {message = "Something went wrong in removing the expired reports"});}
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
