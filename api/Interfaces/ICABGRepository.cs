using System.Threading.Tasks;
using api.Entities;
using api.Helpers;

namespace api.Interfaces
{
    public interface ICABGRepository
    {
        Task<PagedList<Class_CABG>> GetCABGS(ProcedureParams procParams);
        Task<Class_CABG> GetSpecificCABG(int id);
        Task<int> addCABG(Class_CABG p);
        Task<int> updateCABG(Class_CABG p);
        Task<bool> SaveAll();

        Task<bool> showVSMHarvestAsync(int procedure_id);
        Task<bool> showRadialHarvestAsync(int procedure_id);
        Task<bool> show80Async(int procedure_id);
    }
}