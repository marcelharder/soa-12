
using System.Threading.Tasks;
using api.Entities;

namespace api.Interfaces.statistics
{
    public interface IElementaryStatistics
    {
        Task<ClassVlad> getAgeDistributionPerHospital(int userId, int hospitalId);

        Task<ClassVlad> getCasesPerYearPerHospital(int userId, int hospitalId);

        Task<ClassVlad> getCasesPerMonthPerHospital(int currentYear, int userId, int hospitalId);

        Task<ClassVlad> getCaseMixPerHospital(int userId, int hospitalId);

        Task<ClassVlad> getRiskBandsPerHospital(int userId, int hospitalId);
    }
}
