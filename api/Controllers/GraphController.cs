using api.Entities;
using api.Interfaces.statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace api.Controllers
{

    [Authorize]
    public class GraphController : BaseApiController
    {

        private IStatistics _st;
        private IElementaryStatistics _el;
        public GraphController(IStatistics st, IElementaryStatistics el)
        {

            _st = st;
            _el = el;
        }

       
        [Route("vladGraphPerHospital/{userId}/{hospitalId}")]
        [HttpGet]
        public async Task<IActionResult> gettwoAsync(int userId,int hospitalId)
        {
            ClassVlad result = await _st.getVladAsync(userId,hospitalId);
            return Ok(result);
        }
        
        [Route("cmGraphPerHospital/{userId}/{hospitalId}")]
        [HttpGet]
        public async Task<IActionResult> getfourAsync(int userId, int hospitalId)
        {
            ClassVlad result = await _el.getCaseMixPerHospital(userId, hospitalId);
            return Ok(result);
        }

       
        [Route("ageGraphPerHospital/{userId}/{hospitalId}")]
        [HttpGet]
        public async Task<IActionResult> getsixAsync(int userId, int hospitalId)
        {
            ClassVlad result = await _el.getAgeDistributionPerHospital(userId, hospitalId);
             if(result.caption != "n/a"){return Ok(result);}
            return BadRequest();
        }
       
        [Route("euroGraphPerHospital/{userId}/{hospitalId}")]
        [HttpGet]
        public async Task<IActionResult> geteightAsync(int userId, int hospitalId)
        {
            ClassVlad result = await _el.getRiskBandsPerHospital(userId,hospitalId);
            return Ok(result);
        }
        
        [Route("proceduresPerMonthGraphPerHospital/{userId}/{hospitalId}")]
        [HttpGet]
        public async Task<IActionResult> geteightAsync1(int userId,int hospitalId)
        {
            DateTime now = DateTime.UtcNow.Date;
            var currentYear = now.Year;
            ClassVlad result = await _el.getCasesPerMonthPerHospital(currentYear, userId, hospitalId);
            return Ok(result);
        }
        
        [Route("proceduresPerYearGraphPerHospital/{userId}/{hospitalId}")]
        [HttpGet]
        public async Task<IActionResult> getNine01Async(int userId, int hospitalId)
        {
            ClassVlad result = await _el.getCasesPerYearPerHospital(userId,hospitalId);
            return Ok(result);
        }





    }
}