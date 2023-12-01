using System.Threading.Tasks;
using api.Data;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations
{
    public class CABGRepository : ICABGRepository
    {
        private DataContext _context;
        private SpecialMaps _special;

        public CABGRepository(DataContext context, SpecialMaps special)
        {
            _context = context;

            _special = special;
        }

        public Task<int> addCABG(Class_CABG p)
        {
            throw new System.NotImplementedException();
        }


        public Task<PagedList<Class_CABG>> GetCABGS(ProcedureParams procParams)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Class_CABG> GetSpecificCABG(int id)
        {
            var result = new Class_CABG();
            if (await _context.CABGS.AnyAsync(u => u.PROCEDURE_ID == id))// check if there is a record for this procedure
            {
                return await _context.CABGS.FirstOrDefaultAsync(u => u.PROCEDURE_ID == id);
            }
            else
            {
                var cabg = new Class_CABG();
                cabg.PROCEDURE_ID = id;
                cabg.B1_SITE = "0"; cabg.B2_SITE = "0"; cabg.B3_SITE = "0"; cabg.B4_SITE = "0";
                cabg.B5_SITE = "0"; cabg.B6_SITE = "0";
                cabg.CAB = "1"; cabg.UNPLANNED_CAB = "1";

                _context.Add(cabg);
                if (await this.SaveAll())
                {
                    return await _context.CABGS.FirstOrDefaultAsync(u => u.PROCEDURE_ID == id);
                }
                else { return null; }
            }

        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<int> updateCABG(Class_CABG p)
        {
            var result = 0;
            _context.Update(p);
            if (await _context.SaveChangesAsync() > 0) { result = 1; }
            return result;
        }
        public async Task<bool> showVSMHarvestAsync(int procedure_id)
        {
            var help = false;
            var selectedProcedure = await _context.Procedures.FirstOrDefaultAsync(x => x.ProcedureId == procedure_id);
            var soort = selectedProcedure.fdType;
            if (_special.getSoortWithVSM().Contains(soort.ToString())) { help = true; }
            return help;
        }
        public async Task<bool> showRadialHarvestAsync(int procedure_id)
        {
            var help = false;
            var selectedProcedure = await _context.Procedures.FirstOrDefaultAsync(x => x.ProcedureId == procedure_id);
            var soort = selectedProcedure.fdType;
            if (_special.getSoortWithRadial().Contains(soort.ToString())) { help = true; }
            return help;
        }
        public async Task<bool> show80Async(int procedure_id)
        {
            var help = false;
            var selectedProcedure = await _context.Procedures.FirstOrDefaultAsync(x => x.ProcedureId == procedure_id);
            var soort = selectedProcedure.fdType;
            if (_special.getSoortWith80().Contains(soort.ToString())) { help = true; }
            return help;
        }


    }

}