using System;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Helpers;
using api.Interfaces;

namespace api.Implementations
{
    public class Discharge : IDischarge
    {
        private DataContext _context;
        private SpecialMaps _maps;
        private IPORepository _ipo;
        private IPatientRepository _pat;




        public Discharge(DataContext context, 
        SpecialMaps maps,
            IPORepository ipo, 
            IPatientRepository patient)
        {
            _context = context;
            _ipo = ipo;
            _pat = patient;
            _maps = maps;
        }
        public Task<int> addDischarge(DischargeForUpdateDTO p)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<int> deleteDischarge(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DischargeForReturnDTO> getSpecificDischarge(int id)
        {
           var patient = await _pat.GetPatientFromProcedureId(id);
           var postop = await _ipo.GetSpecificPostOp(id);
           return _maps.mapToDischargeDTO(patient, postop);
        }

        public Task<bool> SaveAll()
        {
            throw new NotImplementedException();
        }

        public async Task<int> updateDischarge(DischargeForUpdateDTO p)
        {
            var pat = await _pat.GetPatientFromProcedureId(p.procedure_id);
            var patient = await _pat.GetPatientClass(pat.patientId);

            
            var postop = await _ipo.GetSpecificPostOp(p.procedure_id);
            var updatedPatient = _maps.mapFromDischargeForUpdate_1(p,patient);
            
            var updatedPostop = _maps.mapFromDischargeForUpdate_2(p, postop);
            
            var result_1 = await _ipo.updatePostOp(updatedPostop);

            var result_2 = await _pat.updatePatient(updatedPatient);
            return result_1;
        }
    }
}
