using System.Collections.Generic;
using System.Threading.Tasks;


namespace api.interfaces.reports
{
    public interface IInstitutionalText
    {
        Task<List<string>> getText(string hospital, string soort, int procedure_id);
    }
}