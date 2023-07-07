using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations
{
    public class PORepository : IPORepository
    {

        private DataContext _context;
        private SpecialMaps _special;

        public PORepository(DataContext context, SpecialMaps special)
        {
            _context = context;
            _special = special;
        }

        public Task<int> addPostOp(Class_PostOp p)
        {
            throw new NotImplementedException();
        }
        public Task<PagedList<Class_PostOp>> GetPostOps(ProcedureParams procParams)
        {
            throw new NotImplementedException();
        }
        public async Task<Class_PostOp> GetSpecificPostOp(int id)
        {
            var result = new Class_PostOp();
            if (await _context.PostOps.AnyAsync(u => u.PROCEDURE_ID == id))
            {
                return await _context.PostOps.FirstOrDefaultAsync(u => u.PROCEDURE_ID == id);
            }
            else
            {
                var cp = await _context.Procedures.FirstOrDefaultAsync(u => u.ProcedureId == id);
              
                var po = new Class_PostOp();
                po.PROCEDURE_ID = id;

                po.ICU_ARRIVAL_DATE = cp.DateOfSurgery.Date;
                po.ICU_ARRIVAL_DATE = po.ICU_ARRIVAL_DATE.AddHours(cp.SelectedStopHr);
                po.ICU_ARRIVAL_DATE = po.ICU_ARRIVAL_DATE.AddMinutes(cp.SelectedStopMin);
                po.ICU_ARRIVAL_DATE = po.ICU_ARRIVAL_DATE.AddMinutes(10);// add 10 minutes for walking from theatre to ICU

                po.ICU_DISCHARGE_DATE = po.ICU_ARRIVAL_DATE.AddDays(1);
                po.EXTUBATION_DATE = po.ICU_ARRIVAL_DATE.AddDays(1);
                po.readmitted = "1";
                po.reintubated = "1";

                _context.Add(po);

                if (await SaveAll())
                {

                    return await _context.PostOps.FirstOrDefaultAsync(u => u.PROCEDURE_ID == id);
                }
            }
            return result;
        }
        public async Task<int> updatePostOp(Class_PostOp p)
        {
            var res = 0;
            _context.Update(p);
            if (await _context.SaveChangesAsync() > 0){ res = 1; }
            return res;
        }
        public async Task<bool> SaveAll(){ return await _context.SaveChangesAsync() > 0; }
        
       

    }
}
