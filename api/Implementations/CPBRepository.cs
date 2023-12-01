using System;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace api.Implementations
{
    public class CPBRepository : ICPBRepository
    {
        private DataContext _context;

        public CPBRepository(DataContext context){
            _context = context;
        }

        public Task<int> addCPB(Class_CPB p)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<Class_CPB>> GetCPBS(ProcedureParams procParams)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Class_CPB> GetSpecificCPB(int id)
        {
            var result = new Class_CPB();
            if (await _context.CPBS.AnyAsync(u => u.PROCEDURE_ID == id))// check if there is a record for this procedure
            {
                result = await _context.CPBS.FirstOrDefaultAsync(u => u.PROCEDURE_ID == id);
            }
            else
            {
                var cpb = new Class_CPB();
                // put clever initial values here
                cpb.PROCEDURE_ID = id;
                cpb.opcab_attempt = "1";
                cpb.cpb_used = "1";
                cpb.a1 = "1";
                cpb.v1 = "1";
                cpb.aoOCCL = "1";
                cpb.IABP = "0";
                cpb.IABP_IND = "0";
                cpb.VAD = "0";
                cpb.BVAD = "0";
                cpb.TAH = "0";
                cpb.myoplasty = "0";
                cpb.deep_hypo = "0";
                cpb.deep_hypo_rcp = "0";
                cpb.acp_circ = "0";
                cpb.other_cns_protect = "0";
                cpb.nonCMProtect = "0";
                cpb.LOWEST_CORE_TEMP = 34;

                _context.Add(cpb);

                if (await SaveAll()) { result = await _context.CPBS.FirstOrDefaultAsync(u => u.PROCEDURE_ID == id); }
                else { Console.WriteLine("saving ging mis"); }
            }
            return result;
        }

        public async Task<bool> SaveAll() { return await _context.SaveChangesAsync() > 0;}

        public async Task<int> updateCPB(Class_CPB p)
        {
            _context.Update(p);
            if (await this.SaveAll()) { return 1; }
            return 0;
        }
    }
}
