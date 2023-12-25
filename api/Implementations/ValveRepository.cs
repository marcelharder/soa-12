
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations
{
    public class ValveRepository : IValveRepository
    {
        private DataContext _context;
        private UserManager<AppUser> _usermanager;
        private SpecialMaps _special;
        public ValveRepository(
            IWebHostEnvironment env,
            DataContext context,
            SpecialMaps special,
            UserManager<AppUser> usermanager)
        {
            _context = context;
            _special = special;
            _usermanager = usermanager;
        }

        #region <!-- CRUD for valves associated with procedures-->
        public async Task<int> DeleteAsync<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Deleted;
            if (await SaveAll()) { return 1; } else { return 0; }
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Class_Valve> GetSpecificValve(string serial, int procedure_id)
        {
            var result = await _context.Valves.FirstOrDefaultAsync(u => u.SERIAL_IMP == serial);
            if (result != null)
            {
                return result;
            }
            return null;
        }
        public async Task<Class_Valve> addValve(string serial, int procedure_id)
        {
            // check if this serial already exists, if it does then return that file

            var test = await _context.Valves.AnyAsync(x => x.SERIAL_IMP == serial);
            if (!test)
            {
                var result = new Class_Valve();
                var selectedProcedure = await _context.Procedures.Include(c => c.ValvesUsed).FirstOrDefaultAsync(x => x.ProcedureId == procedure_id);
                var valve = new Class_Valve();
                // put some standard initializations here.
                valve.SERIAL_IMP = serial;
                valve.ProcedureId = procedure_id;

                selectedProcedure.ValvesUsed.Add(valve);
                _context.Update(selectedProcedure);
                if (await SaveAll())
                {
                    result = selectedProcedure.ValvesUsed.Where(a => a.SERIAL_IMP == serial).FirstOrDefault();
                    return result;
                }
            }
            else
            {
                return await _context.Valves.FirstOrDefaultAsync(x => x.SERIAL_IMP == serial);
            }
            return null;
        }
        public async Task<int> updateValve(Class_Valve p)
        {
            _context.Update(p);
            if (await SaveAll()) { return 1; } else { return 2; }
        }
        public async Task<List<Class_Valve>> getValvesFromProcedure(int id)
        {
            var p = await _context.Procedures.Include(vs => vs.ValvesUsed).FirstOrDefaultAsync(r => r.ProcedureId == id);

            return p.ValvesUsed.ToList();
        }

        #endregion
        #region <!-- CRUD for valveRepair-->
        public async Task<Class_Valve> GetSpecificValveRepair(int id, int procedure_id)
        {
            var selectedValve = await _context.Valves.FirstOrDefaultAsync(x => x.Id == id);
            return selectedValve;
        }
        public async Task<Class_Valve> addValveRepair(string position, int procedure_id)
        {
            var result = new Class_Valve();
            var selectedProcedure = await _context.Procedures.Include(c => c.ValvesUsed).FirstOrDefaultAsync(x => x.ProcedureId == procedure_id);
            var valve = new Class_Valve();
            // put some standard initializations here.
            valve.Implant_Position = position;
            valve.TYPE = "Annuloplasty_Ring";
            valve.ProcedureId = procedure_id;
            selectedProcedure.ValvesUsed.Add(valve);
            _context.Update(selectedProcedure);
            if (await SaveAll())
            {
                result = selectedProcedure.ValvesUsed.Last();
                return result;
            }
            return null;
        }

        public async Task<List<Class_Valve>> getValveRepairsFromProcedure(int id)
        {
            var result = new List<Class_Valve>();
            var p = await _context.Procedures.Include(vs => vs.ValvesUsed).FirstOrDefaultAsync(r => r.ProcedureId == id);
            foreach (Class_Valve cv in p.ValvesUsed)
            {
                if (cv.TYPE == "Annuloplasty_Ring") { result.Add(cv); }
            }
            return result;
        }

        #endregion
        #region <!-- CRUD for ValvedConduits -->
        public async Task<List<Class_Valve>> getValvedConduitsFromProcedure(int id)
        {
            var result = new List<Class_Valve>();
            var p = await _context.Procedures.Include(vs => vs.ValvesUsed).FirstOrDefaultAsync(r => r.ProcedureId == id);
            foreach (Class_Valve cv in p.ValvesUsed)
            {
                if (cv.TYPE == "Valved_Conduit") { result.Add(cv); }
            }
            return result;
        }
        public async Task<Class_Valve> GetSpecificValvedConduit(int id)
        {
            var selectedValve = await _context.Valves.FirstOrDefaultAsync(x => x.Id == id);
            return selectedValve;
        }
        public async Task<Class_Valve> addValvedConduit(int procedure_id)
        {
            var result = new Class_Valve();
            var selectedProcedure = await _context.Procedures.Include(c => c.ValvesUsed).FirstOrDefaultAsync(x => x.ProcedureId == procedure_id);
            var valve = new Class_Valve();
            // put some standard initializations here.
            valve.Implant_Position = "Aortic";
            valve.TYPE = "Valved_Conduit";
            valve.ProcedureId = procedure_id;
            selectedProcedure.ValvesUsed.Add(valve);
            _context.Update(selectedProcedure);
            if (await SaveAll())
            {
                result = selectedProcedure.ValvesUsed.Last();
                return result;
            }
            return null;
        }





        #endregion

       /*  private IEnumerable<Class_Valve_Code> getValvesInHospital(int hospitalId)
        {
            // get this from the valveinventory
            var result = _context.Class_Valve_Code.OrderByDescending(u => u.hospitalId).AsQueryable();
            if (result != null) { return result; } else { return null; }


        } */
        public async Task<int> deleteSpecificValve(int id)
        {
            var selectedValve = await _context.Valves.FirstOrDefaultAsync(x => x.Id == id);
            if (selectedValve != null)
            {
                if (await this.DeleteAsync(selectedValve) == 1)
                {
                    return 1;
                };
                return 0;
            }
            return 0;
        }
       
    }
}