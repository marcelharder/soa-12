using System.Collections.Generic;
using System.Threading.Tasks;
using api.Entities;

namespace api.Interfaces
{
    public interface IValveRepository
    {
        Task<Class_Valve> GetSpecificValve(string serial, int procedure_id);
        Task<int> updateValve(Class_Valve p);
        Task<int> DeleteAsync<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<List<Class_Valve>> getValvedConduitsFromProcedure(int id);
        Task<List<Class_Valve>> getValveRepairsFromProcedure(int id);
        Task<List<Class_Valve>> getValvesFromProcedure(int id);
        Task<Class_Valve> addValve(string serial, int procedure_id);
        Task<Class_Valve> GetSpecificValveRepair(int id, int procedure_id);
        Task<Class_Valve> addValveRepair(string position,int procedure_id);
        Task<Class_Valve> GetSpecificValvedConduit(int id);
        Task<Class_Valve> addValvedConduit(int procedure_id);
        Task<int> deleteSpecificValve(int id);
    }
}