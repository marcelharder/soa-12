using System.Net.Http;
using System.Threading.Tasks;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api.Controllers
{
    [Authorize]

    public class TrainingController : BaseApiController
    {
        private IOptions<ComSettings> _com;
        public TrainingController( IOptions<ComSettings> com)
        {
            _com = com;
            
        }

        [HttpGet("getEpaDefinition")]
        public async Task<IActionResult> getEpaDef(){

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
        
    }
}