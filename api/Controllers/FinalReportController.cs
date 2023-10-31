using System.Threading.Tasks;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api.Controllers
{

    [Authorize]
    public class FinalReport : BaseApiController
    {
       
        private IOptions<ComSettings> _com;
      


        public FinalReport(IOptions<ComSettings> com) {_com = com;}

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var comaddress = _com.Value.reportURL;
            var st = "FinalReport/" + id;
            comaddress = comaddress + st;
            return Redirect(comaddress);
        }

        [AllowAnonymous]
        [HttpGet("getRefReport/{hash}")]
        public async Task<IActionResult> getPdfForRefPhys(string hash)
        {
           var comaddress = _com.Value.reportURL;
           var st = "FinalReport/getRefReport/" + hash;
           comaddress = comaddress + st;
           return Redirect(comaddress);
          
        }

        

        


    }
}
