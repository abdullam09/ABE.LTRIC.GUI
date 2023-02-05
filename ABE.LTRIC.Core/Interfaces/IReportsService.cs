using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Core.Interfaces
{
    public interface IReportsService
    {
        Task GenerateReport(int? companyId, string documentNumber, DateTime from, DateTime to, bool includeDate);
        List<string> GetAllReports();
        Task<string> DeleteReport(string reportName);
        Task OpenReport(string reportName);
    }
}
