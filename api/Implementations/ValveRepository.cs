
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
    public class ValveRepository: IValveRepository
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
            if(result != null){
                return result;
            }
            return null;
        }
        public async Task<Class_Valve> addValve(string serial, int procedure_id)
        {
            // check if this serial already exists, if it does then return that file

            var test = await _context.Valves.AnyAsync(x => x.SERIAL_IMP == serial);
            if(!test){
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
            } else {
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

        #region <!-- These are the valves that show up in the hospital configuration -->
        public async Task<List<Class_Item>> getProductCodesInHospital(string type, string position)
        {
            var productCodes = new List<Class_Item>();
            var counter = 0;

            var currentUserId = _special.getCurrentUserId();
            var currentUser = await _usermanager.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
            var currentHospitalId = currentUser.hospital_id.ToString().makeSureTwoChar();

            var selectedHospital = await _context.Hospitals
                                     .Include(vs => vs.valvecodes)
                                     .FirstOrDefaultAsync(a => a.HospitalNo == currentHospitalId);

            foreach (Class_Valve_Code el in selectedHospital.valvecodes)
            {
                counter++;
                var cl = new Class_Item();
                cl.description = el.description;
                cl.value = counter;
                productCodes.Add(cl);
            }
            return productCodes;
        }

        public async Task<List<valveDTO>> getValvesInHospital(string type, string position)
        {
            var productCodes = new List<valveDTO>();

            var currentUserId = _special.getCurrentUserId();
            var currentUser = await _usermanager.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
            var currentHospitalId = currentUser.hospital_id.ToString().makeSureTwoChar();

            var selectedHospital = await _context.Hospitals
                                     .Include(vs => vs.valvecodes)
                                     .FirstOrDefaultAsync(a => a.HospitalNo == currentHospitalId);

            foreach (Class_Valve_Code el in selectedHospital.valvecodes)
            {
                if (el.type == type && el.position == position)
                {
                    productCodes.Add(_special.mapClassValveToDTO(el));
                }

            }
            return productCodes;
        }
       



        public async Task<valveDTO> createValveInHospital(valveDTO tes)
        {
            var currentUserId = _special.getCurrentUserId();
            var currentUser = await _usermanager.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
            var currentHospitalId = currentUser.hospital_id.ToString().makeSureTwoChar();


            var selectedHospital = await _context.Hospitals
                                     .Include(vs => vs.valvecodes)
                                     .FirstOrDefaultAsync(a => a.HospitalNo == currentHospitalId);
                                     
            var listOfValvesForThisHospital = new List<Class_Valve_Code>();
            listOfValvesForThisHospital = selectedHospital.valvecodes.ToList();
            // check to see if this valve already exists in the valvecodes of this hospital
            if (listOfValvesForThisHospital.Exists(x => x.valveTypeId == tes.valveTypeId)) { return null; }
            else
            {
                Class_Valve_Code valve = new Class_Valve_Code();
                valve.code = tes.code;
                valve.valveTypeId = tes.valveTypeId;
                valve.position = tes.implant_Position;
                valve.description = tes.description;
                valve.soort = tes.soort;
                valve.type = tes.type;

                selectedHospital.valvecodes.Add(valve);
                _context.Update(selectedHospital);
                if (await SaveAll())
                {
                    tes.codeId = valve.codeId;
                    return tes;
                }
                else return null;
            }

        }
        public async Task<valveDTO> readValveInHospital(string code)
        {
            valveDTO vd = new valveDTO();
            var currentUserId = _special.getCurrentUserId();
            var currentUser = await _usermanager.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
            var currentHospitalId = currentUser.hospital_id.ToString().makeSureTwoChar();

            var selectedHospital = await _context.Hospitals
                                     .Include(vs => vs.valvecodes)
                                     .FirstOrDefaultAsync(a => a.HospitalNo == currentHospitalId);

            foreach (Class_Valve_Code el in selectedHospital.valvecodes)
            {
                if (el.code == code)
                {
                    vd.hospitalNo = currentUser.hospital_id;
                    vd.soort = el.soort;
                    vd.codeId = el.codeId;
                    vd.code = el.code;
                    vd.type = el.type;
                    vd.valveTypeId = el.valveTypeId;
                    vd.implant_Position = el.position;
                    vd.description = el.description;
                };
            };
            return vd;
        }
        public async Task<valveDTO> updateValveInHospital(valveDTO tes)
        {
            var currentUserId = _special.getCurrentUserId();
            var currentUser = await _usermanager.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
            var currentHospitalId = currentUser.hospital_id.ToString().makeSureTwoChar();

            var selectedHospital = await _context.Hospitals
                                     .Include(vs => vs.valvecodes)
                                     .FirstOrDefaultAsync(a => a.HospitalNo == currentHospitalId);
            var el = selectedHospital.valvecodes.Where(a => a.codeId == tes.codeId).ToList();
            el[0].code = tes.code;
            el[0].type = tes.type;
            el[0].position = tes.implant_Position;
            el[0].description = tes.description;

            _context.Update(selectedHospital);
            if (await SaveAll()) { return await readValveInHospital(tes.code); }
            return null;

        }
      /*   public async Task<int> deleteValveInHospital(int codeId)
        {
            var deleteResult = 0;
            var currentUserId = _special.getCurrentUserId();
            var currentUser = await _usermanager.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
            var currentHospitalId = currentUser.hospital_id.ToString().makeSureTwoChar();

            var selectedHospital = await _context.Hospitals
                                     .Include(vs => vs.valvecodes)
                                     .FirstOrDefaultAsync(a => a.HospitalNo == currentHospitalId);

            var el = selectedHospital.valvecodes.Where(a => a.codeId == codeId).ToList();
            selectedHospital.valvecodes.Remove(el[0]);
            _context.Update(selectedHospital);

            if (await SaveAll()) { deleteResult = 1; }
            return deleteResult;
        } */

public async Task<int> deleteValveInHospital(int codeId){
            await Task.Run(()=>{ return 1;});
            return 0;
   
}



        public async Task<string> getValveDescriptionFromModel(string code)
        {
            valveDTO vf = new valveDTO();
            vf = await readValveInHospital(code);
            return vf.description;
        }
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
    }
}