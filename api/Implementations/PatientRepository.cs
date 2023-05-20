using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations
{
    public class PatientRepository: IPatientRepository
    {
         private DataContext _context;
        private SpecialMaps _map;


        public PatientRepository(DataContext context, SpecialMaps map)
        {
            _context = context;
            _map = map;
        }

        public async Task<int> addPatient(int id, Class_Patient p)
        {
            await _context.Patients.AddAsync(p);
            if (await SaveAll()) { return 1; } else { return 0; }
        }
        public async Task<Class_Patient> AddPatientFromMRN(string id)
        {
            var p = await _context.Patients.FirstOrDefaultAsync(a => a.MRN == id);
            if (p == null)
            {
                var help = new Class_Patient();
                help.MRN = id;
                // add other start variables here
                help.NYHA = "1";
                help.poor_mobility = "0";
                help.systolic_pa_pressure = "1";
                help.timing = "1";
                help.weight_of_intervention = "1";
                help.dialysis = false;
                // and save to the database
                await _context.Patients.AddAsync(help);
                await _context.SaveChangesAsync();
                p = await _context.Patients.FirstOrDefaultAsync(a => a.MRN == id);
            }
            return p;
        }
        public void Delete<T>(T entity) where T : class
        {
            throw new System.NotImplementedException();
        }
        public async Task<FullPatientDTO> GetPatient(int id)
        {
            //NB there is always a patient record here.
            var test = await _context.Patients.Include(a => a.procedures).FirstOrDefaultAsync(a => a.PatientId == id);
            return _map.mapToFullPatientDto(test);
        }

        public async Task<Class_Patient> GetPatientFromPatientId(int patient_id)
        {
            var test = await _context.Patients.Include(a => a.procedures).FirstOrDefaultAsync(a => a.PatientId == patient_id);
            return test;
        }

        public async Task<Class_Patient> GetPatientClass(int id)
        {
            var test = await _context.Patients.FirstOrDefaultAsync(a => a.PatientId == id);
            return test;
        }
        
        public async Task<string> GetPatientInDatabase(string id)
        {
            var result = "1";
            var patient_in_database = await _context.Patients.AnyAsync(a => a.MRN == id);
            if (!patient_in_database) { result = "0"; }
            return result;
        }
        public async Task<PatientForReturnDTO> GetPatientFromMRN(string id)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(a => a.MRN == id);
            return _map.mapToPatientForReturn(patient);
        }
        public async Task<PagedList<Class_Patient>> GetPatients(PatientParams patParams)
        {
            var patients = _context.Patients.OrderByDescending(u => u.PatientId).AsQueryable();
            return await PagedList<Class_Patient>.CreateAsync(patients, patParams.PageNumber, patParams.PageSize);
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<int> updatePatient(Class_Patient p)
        {
            _context.Update(p);
            if (await this.SaveAll())
            {
                return 1;
            }
            return 0;
        }
        public async Task<FullPatientDTO> GetPatientFromProcedureId(int id)
        {
            var procedure = await _context.Procedures.FirstOrDefaultAsync(u => u.ProcedureId == id);
            return await GetPatient(procedure.PatientId);
        }
    }
}