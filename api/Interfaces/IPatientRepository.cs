using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Helpers;

namespace api.Interfaces
{
    public interface IPatientRepository
    {
        Task<FullPatientDTO> GetPatient(int id);
        Task<Class_Patient> GetPatientClass(int id);
        Task<PagedList<Class_Patient>> GetPatients(PatientParams patParams);
        Task<bool> SaveAll();
        void Delete<T>(T entity) where T : class;
        Task<PatientForReturnDTO> GetPatientFromMRN(string id);
        Task<string> GetPatientInDatabase (string id);
        Task<Class_Patient> AddPatientFromMRN(string id);
        Task<int> addPatient(int id, Class_Patient p);
        Task<int> updatePatient(Class_Patient p);
        Task<FullPatientDTO> GetPatientFromProcedureId(int id);
        Task<Class_Patient> GetPatientFromPatientId(int patient_id);
    }
}