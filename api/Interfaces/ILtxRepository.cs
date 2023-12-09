using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;

namespace api.Interfaces
{
    public interface ILtxRepository
    {
        List<Class_LTX> GetLTX(int id);
        Task<Class_LTX> GetSpecificLTX(int id);
        Task<int> updateLTX(Class_LTX p);
        Task<int> addLTX(Class_LTX p);
        Task<int> deleteLTX(int id);
        Task<bool> SaveAll();
        Task<int> DeleteAsync<T>(T entity) where T : class;
    }
}