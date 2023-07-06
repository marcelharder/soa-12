

using System.Threading.Tasks;
using api.DTOs;

namespace api.Interfaces
{
    public interface IDischarge
    {
        Task<DischargeForReturnDTO> getSpecificDischarge(int id);
        Task<int> updateDischarge(DischargeForUpdateDTO p);
        Task<int> addDischarge(DischargeForUpdateDTO p);
        Task<int> deleteDischarge(int id);
        Task<bool> SaveAll();
        Task<int> DeleteAsync<T>(T entity) where T : class;
    }
}
