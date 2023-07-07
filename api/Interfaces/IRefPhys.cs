
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Entities;

namespace api.Interfaces
{
    public interface IRefPhys
    {
        Task<Class_Ref_Phys> getSpecificRefPhys(int id);
        Task<List<Class_Item>> getAllRefPhysInThisHospital(int hospital_id);
        Task<int> updateRefPhys(Class_Ref_Phys p);
        Task<Class_Ref_Phys> addRefPhys();
        Task<int> deleteRefPhys(int id);
        Task<bool> SaveAll();
    }
}