using System.Threading.Tasks;
using api.Data;

namespace api.interfaces.reports
{
    public interface IManageFinalReport
    {
        int deletePDF(int id);
        int deleteExpiredReports();
        int addToExpiredReports(ReportTiming rt);
        Task<bool> isReportExpired(int id);
        Task<bool> pdfDoesNotExists(string id_string);
       
    }
}