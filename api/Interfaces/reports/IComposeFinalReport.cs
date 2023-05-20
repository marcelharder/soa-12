using System.Threading.Tasks;
using api.Data;

namespace api.interfaces.reports
{
    public interface IComposeFinalReport
    {
        Task composeAsync(int procedure_id);
        int deletePDF(int id);
        Task<int> getReportCode(int procedure_id);
        int deleteExpiredReports();
        int addToExpiredReports(ReportTiming rt);
        Task<bool> isReportExpired(int id);
    }
}