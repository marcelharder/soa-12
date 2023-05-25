using System.Threading.Tasks;
using api.Entities;

namespace api.interfaces.reports
{
    public interface IOperativeReportPdf
    {
        Task<int> getPdf(int report_code, Class_Final_operative_report fr);
    }
}