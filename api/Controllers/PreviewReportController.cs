using System;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.interfaces.reports;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using api.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Authorize]
    public class PreviewReportController : BaseApiController
    {

        private IPV _repo;
        private IValveRepository _valve;
         private ICABGRepository _cabg;
        private IUserRepository _ur;
        private IHospitalRepository _hos;
        private IProcedureRepository _proc;
        private DataContext _context;
        private SpecialMaps _sm;
        private SpecialReportMaps _sprm;
        private IOperativeReportPdf _ioprep;

        public PreviewReportController(IPV repo,
            IUserRepository ur,
            DataContext context,
            IOperativeReportPdf ioprep,
            IHospitalRepository hos,
            SpecialMaps sm,
            ICABGRepository cabg,
            IValveRepository valve,
            SpecialReportMaps sprm,
            IProcedureRepository proc)
        {
            _repo = repo;
            _sm = sm;
            _proc = proc;
            _ur = ur;
            _hos = hos;
            _sprm = sprm;
            _cabg = cabg;
            _valve = valve;
            _ioprep = ioprep;
            _context = context;

        }


        [HttpGet("{id}", Name = "GetPreview")]
        public async Task<IActionResult> Get(int id) { return Ok(await _repo.getPreViewAsync(id)); }

        [HttpGet("reset/{id}", Name = "ResetView")]
        public async Task<IActionResult> Reset(int id) { return Ok(await _repo.resetPreViewAsync(id)); }

        [HttpGet("isFinalReportReady/{id}")]
        public async Task<int> IsReportReady(int id)
        {
            var result = 0;
            var procedure = await _proc.GetProcedure(id); if (procedure == null) { return result; }
            var reportcode = Convert.ToInt32(_sprm.getReportCode(procedure.fdType));
            
            
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
            try
            {  
                Class_privacy_model pm = _sprm.mapToClassPrivacyModel(pvfr);
                Class_Preview_Operative_report pv = await _repo.getPreViewAsync(pvfr.procedure_id);
                pv = _sprm.mapToClassPreviewOperativeReport(pvfr, pv);

                // save the Class_Preview_Operative_report to the database first
                var result = await _repo.updatePVR(pv);

                // generate final operative report Class
                var classFR = await _sprm.updateFinalReportAsync(pm, pv.procedure_id);

                // generate PDF and store for 3 days
                // first get the procedure so that we can get the fdtype and subsequent report_code
               try
               {
                     var current_procedure = await _context.Procedures.FirstOrDefaultAsync(x => x.ProcedureId == pvfr.procedure_id);
                     var report_code = Convert.ToInt32(_sprm.getReportCode(current_procedure.fdType));
                     await _ioprep.getPdf(report_code,classFR);
               }
               catch (Exception a)
               {
                   Console.WriteLine(a.InnerException); 
                   return BadRequest("Error creating the pdf");
               } 
               return Ok(result);
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); }
            return BadRequest("Error saving the preview report");

        }

       
       
       
       
        #region <!-- InstitutionalReports stuff -->
        
        [HttpGet("InstitutionalReport/{id}/{soort}")]
        public async Task<IActionResult> getIRep(int id, int soort){
          var help = await _repo.getInstitutionalReport(id, soort);
          return Ok(help);
        }
        [HttpPut("InstitutionalReport")]
        public async Task<IActionResult> updateIRep([FromBody] InstitutionalDTO rep, int id, int soort){
             var help = await _repo.updateInstitutionalReport(rep,soort,id);
          return Ok(help);
        }
        [HttpPost("InstitutionalReport")]
        public async Task<IActionResult> createIRep(int id){
             var help = await _repo.createInstitutionalReport(id);
          return Ok(help);
        }

         [HttpGet("AdditionalReportItems/{id}/{soort}/{which}")]
        public async Task<IActionResult> getARI(int id, int soort,int which){
          var help = await _repo.getAdditionalReportItems(id,soort,which);
          return Ok(help);
        }

        #endregion


    }
}