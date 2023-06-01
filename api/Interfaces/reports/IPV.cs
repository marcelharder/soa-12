
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;

namespace api.interfaces.reports
{
    public interface IPV
    {
        Task<Class_Preview_Operative_report> getPreViewAsync(int procedure_id);
        Task<int> updatePVR(Class_Preview_Operative_report cp);
        Task<bool> SaveAll();
        Task<int> DeleteAsync<T>(T entity) where T : class;
        Task<Class_Preview_Operative_report> resetPreViewAsync(int procedure_id);
        Task<InstitutionalDTO> getInstitutionalReport(int hospitalNo, int soort);
        Task<string> createInstitutionalReport(int hospitalNo);
        Task<string> updateInstitutionalReport(InstitutionalReport rep);
    }
}
