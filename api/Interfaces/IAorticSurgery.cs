using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;

namespace api.Interfaces
{
    public interface IAorticSurgery
    {
        Task<Class_Aortic_Surgery> getSpecificCAS(int id);
        Task<int> updateCAS(Class_Aortic_Surgery p);
        Task<int> addCAS(Class_Aortic_Surgery p);
        Task<int> deleteCAS(int id);
        Task<bool> SaveAll();
        Task<int> DeleteAsync<T>(T entity) where T : class;
    }
    }
