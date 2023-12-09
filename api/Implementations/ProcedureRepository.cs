using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations
{
    public class ProcedureRepository : IProcedureRepository
    {
        private DataContext _context;
        private IValveRepository _valve;
        private IAorticSurgery _cas;

        private IMinInv _inv;
        private ILtxRepository _ltx;

        private XElement _el;

        private IWebHostEnvironment _env;

        private SpecialMaps _spec;

        public ProcedureRepository(
            SpecialMaps spec,
            IWebHostEnvironment env,
            ILtxRepository ltx,
            DataContext context,
            IValveRepository valve,
            IAorticSurgery cas,
            IMinInv inv)
        {
            _context = context;
            _valve = valve;
            _cas = cas;
            _inv = inv;
            _ltx=ltx;
            _spec=spec;
         
            _env = env;
            var content = _env.ContentRootPath;
            var filename = "conf/procedure.xml";
            var test = Path.Combine(content, filename);
            XElement el = XElement.Load($"{test}");
            _el = el;

        }

        public async Task<int> DeleteAsync(int id)
        {
            await checkAndDeleteCollateralTables(id);
            var p = await GetProcedure(id);
            _context.Procedures.Remove(p);
            if (await SaveAll()) { return 1; } else { return 0; }
        }
        public async Task<Class_Procedure> GetProcedure(int id)
        {
            var result = await _context.Procedures.FirstOrDefaultAsync(u => u.ProcedureId == id);
            return result;
        }
        public async Task<PagedList<Class_Procedure>> GetProcedures(ProcedureParams procParams)
        {
            var procedures = _context.Procedures.OrderByDescending(u => u.ProcedureId).AsQueryable();
            //apply the filters here
            procedures = procedures.Where(s => s.hospital == procParams.selectedHospital);
            procedures = procedures.Where(s => s.SelectedSurgeon == procParams.selectedSurgeon);
            // procedures = procedures.Where(s => s.PatientId == procParams.selectedPatient); 

            return await PagedList<Class_Procedure>.CreateAsync(procedures, procParams.PageNumber, procParams.PageSize);
        }
        public async Task<PagedList<Class_Procedure>> GetAioProcedures(ProcedureParams procParams)
        {
            var selectedHospital = await _spec.getCurrentHospitalIdAsync();
            procParams.selectedHospital = Convert.ToInt32(selectedHospital);
            var procedures = _context.Procedures.OrderByDescending(u => u.ProcedureId).AsQueryable();
            //apply the filters here
            procedures = procedures.Where(s => s.hospital == procParams.selectedHospital);
            procedures = procedures.Where(s => s.SelectedAssistant == procParams.aioId);
            
            return await PagedList<Class_Procedure>.CreateAsync(procedures, procParams.PageNumber, procParams.PageSize);
  
        }
         public async Task<PagedList<Class_Procedure>> GetAssistedProcedures(ProcedureParams procParams)
        {
            var procedures = _context.Procedures.OrderByDescending(u => u.ProcedureId).AsQueryable();
            //apply the filters here
            procedures = procedures.Where(s => s.hospital == procParams.selectedHospital);
            procedures = procedures.Where(s => s.SelectedAssistant == procParams.selectedSurgeon);

            // procedures = procedures.Where(s => s.PatientId == procParams.selectedPatient); 

            return await PagedList<Class_Procedure>.CreateAsync(procedures, procParams.PageNumber, procParams.PageSize);
        }
        public async Task<int> addProcedure(Class_Procedure cp)
        {
            cp.Description = await getProdedureDescription(cp.fdType);
            _context.Procedures.Add(cp);
            if (await SaveAll()) { return 1; } else { return 0; }
        }
        public async Task<int> updateProcedure(Class_Procedure p)
        {
            _context.Procedures.Update(p);
            if (await SaveAll()) { return 1; } else { return 0; }
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<int> checkAndDeleteCollateralTables(int id)
        {
            while (await _context.AorticSurgeries.AnyAsync(u => u.procedure_id == id))
            {
                var help = await _cas.getSpecificCAS(id);
                var result = await _cas.DeleteAsync(help);
            }
            while (await _context.MinInvs.AnyAsync(u => u.PROCEDURE_ID == id))
            {
                var help = await _inv.getSpecificMIN(id);
                var result = await _inv.DeleteAsync(help);
            }
             while (await _context.LTXs.AnyAsync(u => u.PROCEDURE_ID == id))
            {
               var help = await _ltx.GetSpecificLTX(id);
               var result = await _ltx.DeleteAsync(help);
            }
             while (await _context.MinInvs.AnyAsync(u => u.PROCEDURE_ID == id))
            {
                var help = await _inv.getSpecificMIN(id);
                var result = await _inv.DeleteAsync(help);
            }


            return 1;
        }
        public async Task<List<int>> getProceduresFromPatientId(int id)
        {
            var result = new List<int>();
         
            var selectedPatient = await _context.Patients
            .Include(a => a.procedures)
            .FirstOrDefaultAsync(a => a.PatientId == id);

            foreach(Class_Procedure p in selectedPatient.procedures){
                result.Add(p.ProcedureId);}
           
            return result;
        }
        public async Task<List<int>> getProceduresFromMRN(string mrn)
        {
            var result = new List<int>();
         
            var selectedPatient = await _context.Patients
            .Include(a => a.procedures)
            .FirstOrDefaultAsync(a => a.MRN == mrn);

            foreach(Class_Procedure p in selectedPatient.procedures){
                result.Add(p.ProcedureId);}
           
            return result;
        }
       
        public async Task<ButtonPerProcedureSoortDTO> getButtonsAndActions(int soort)
        {

            IEnumerable<XElement> op = _el.Descendants("Code");
            ButtonPerProcedureSoortDTO _result = new ButtonPerProcedureSoortDTO();
            await Task.Run(() =>
            {
                foreach (XElement s in op)
                {
                    if (s.Element("ID").Value == soort.ToString())
                    {
                        _result.iD = soort;
                        _result.description = s.Element("Description").Value;
                        _result.aantal_buttons = s.Element("aantal_buttons").Value;
                        _result.cpb_used = s.Element("cpb_used").Value;
                        _result.weight_of_intervention = s.Element("weight_of_intervention").Value;
                        _result.report_code = s.Element("report_code").Value;
                        IEnumerable<XElement> cap = s.Descendants("button_caption");
                        foreach (XElement t in cap)
                        {
                            _result.button_caption = new List<string>();
                            _result.button_caption.Add(t.Element("button_caption_1").Value);
                            _result.button_caption.Add(t.Element("button_caption_2").Value);
                            _result.button_caption.Add(t.Element("button_caption_3").Value);
                            _result.button_caption.Add(t.Element("button_caption_4").Value);
                            _result.button_caption.Add(t.Element("button_caption_5").Value);
                            _result.button_caption.Add(t.Element("button_caption_6").Value);
                            _result.button_caption.Add(t.Element("button_caption_7").Value);
                            _result.button_caption.Add(t.Element("button_caption_8").Value);
                        }
                        IEnumerable<XElement> act = s.Descendants("button_action");
                        foreach (XElement r in act)
                        {
                            _result.button_action = new List<string>();
                            _result.button_action.Add(r.Element("action_1").Value);
                            _result.button_action.Add(r.Element("action_2").Value);
                            _result.button_action.Add(r.Element("action_3").Value);
                            _result.button_action.Add(r.Element("action_4").Value);
                            _result.button_action.Add(r.Element("action_5").Value);
                            _result.button_action.Add(r.Element("action_6").Value);
                            _result.button_action.Add(r.Element("action_7").Value);
                            _result.button_action.Add(r.Element("action_8").Value);
                        }
                    }
                }
            });

            return _result;
        }
        public async Task<string> getProdedureDescription(int soort)
        {
            var r = await this.getButtonsAndActions(soort);
            return r.description;
        }
        public async Task<string> refPhysEmailHash(int id)
        {
            var currentProcedure = await _context.Procedures.FirstOrDefaultAsync(x => x.ProcedureId == id);
            if (currentProcedure.emailHash == null)
            {
                // compose a hash now
                var newHash = Hash.Create("Mijn nieuwe hash wordt hier gemaakt", Salt.Create());
                // now check that there are no forward slashes, cause that will cause problem in the router
                newHash = newHash.Replace("/","7");
                currentProcedure.emailHash = newHash;
                if (await updateProcedure(currentProcedure) == 1) { return newHash; };
                return "";
            }
            else
            {
                return currentProcedure.emailHash;
            }
        }
        public async Task<int> getProcedureIdFromHash(string hash)
        {
            var result = 0;
            if (await _context.Procedures.AnyAsync(x => x.emailHash == hash))
            {
                var selectedprocedure = await _context.Procedures.FirstOrDefaultAsync(x => x.emailHash == hash);
                result = selectedprocedure.ProcedureId;
            }
            return result;
        }


    }

    public class Salt
    {
        public static string Create()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
    public class Hash
    {
        public static string Create(string value, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: value,
                                salt: Encoding.UTF8.GetBytes(salt),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        public static bool Validate(string value, string salt, string hash)
            => Create(value, salt) == hash;
    }
    }
