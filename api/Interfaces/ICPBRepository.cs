
using System.Threading.Tasks;
using api.Entities;
using api.Helpers;

namespace api.Interfaces
{
    public interface ICPBRepository
    {
        Task<PagedList<Class_CPB>> GetCPBS(ProcedureParams procParams);
        Task<Class_CPB> GetSpecificCPB(int id);
        Task<int> addCPB(Class_CPB p);
        Task<int> updateCPB(Class_CPB p);
        Task<bool> SaveAll();
    }
}
