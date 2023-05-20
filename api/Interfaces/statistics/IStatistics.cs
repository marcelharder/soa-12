using System.Threading.Tasks;
using api.Entities;

namespace api.Interfaces.statistics
{
    public interface IStatistics
    {
       Task<ClassVlad> getVladAsync(int userId, int hospitalId);
    }
}
