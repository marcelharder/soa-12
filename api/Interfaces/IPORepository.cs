using System.Threading.Tasks;
using api.Entities;
using api.Helpers;

namespace api.Interfaces
{
    public interface IPORepository
    {
        Task<PagedList<Class_PostOp>> GetPostOps(ProcedureParams procParams);
        Task<Class_PostOp> GetSpecificPostOp(int id);
        Task<int> addPostOp(Class_PostOp p);
        Task<int> updatePostOp(Class_PostOp p);

        Task<bool> SaveAll();
    }
}
