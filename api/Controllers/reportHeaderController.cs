using System.Threading.Tasks;
using api.DTOs;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace api.Controllers
{
    [Authorize]
    public class reportHeaderController : BaseApiController
    {
        public SpecialReportMaps _mapper;
        private IProcedureRepository _proc;
        public reportHeaderController(SpecialReportMaps mapper, IProcedureRepository proc)
        {
            _mapper = mapper;
            _proc = proc;
        }
       
       
        [HttpGet("{id}")]
        public async Task<ReportHeaderDTO> GetAsync(int id)
        {
            var pr = await _proc.GetProcedure(id);
            return await _mapper.mapToReportHeaderAsync(pr);
        }

       
    }
}
