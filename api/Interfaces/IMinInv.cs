using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;

namespace api.Interfaces
{
    public interface IMinInv
    {
        Task<Class_minInv> getSpecificMIN(int id);
        Task<int> updateMIN(Class_minInv p);
        Task<int> addMIN(Class_minInv p);
        Task<int> deleteMIN(int id);
        Task<bool> SaveAll();
        Task<int> DeleteAsync<T>(T entity) where T : class;
    }
}