using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    public class ChefController : BaseApiController
    {
        private UserManager<AppUser> _manager;
        private readonly IWebHostEnvironment _env;
        private IProcedureRepository _rep;
        private IUserRepository _urep;
        private SpecialMaps _special;
      
        public ChefController(
            IWebHostEnvironment env,
            UserManager<AppUser> manager, 
            IProcedureRepository rep, 
            SpecialMaps special, 
            IUserRepository urep)
        {
            _manager = manager;
            _rep = rep;
            _special = special;
            _urep = urep;
             _env = env;
        }
            [Authorize(Policy = "RequireChefRole")]
            [HttpGet("procedures_from_trainee")]
            public async Task<IActionResult> Get([FromQuery]ProcedureParams p)
            {
                var values = await _rep.GetProcedures(p);

                var l = new List<ProcedureListDTO>();
                foreach (Class_Procedure us in values) 
            { 
                l.Add(await _special.mapToProcedureListDTOAsync(us,1)); 
            }
                Response.AddPagination(values.Currentpage, values.PageSize, values.TotalCount, values.TotalPages);
                return Ok(l);
            }
       
            [Authorize(Policy = "RequireChefRole")]
            [HttpGet("list_of_trainees")]
            public async Task<IActionResult> GetAByHospital([FromQuery] UserParams userParams)
        {
            var values = await _urep.GetAiosByHospital(userParams);
            var l = new List<UserForReturnDto>();
            foreach (AppUser us in values)
            {
                l.Add(_special.mapToUserForReturn(us));
            }
            return Ok(l);
        }

            [Authorize(Policy = "RequireChefRole")]
            [HttpGet("final_op_report/{id}")]
            
            public async Task<IActionResult> GetOpReport(int id)
            {
            await Task.Run(()=>{
                return File(this.GetStream(id.ToString()), "application/pdf", $"{id}.pdf");
            });
            return BadRequest();
             
      
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

