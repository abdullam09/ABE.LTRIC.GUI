using ABE.LTRIC.Core.Entities;
using ABE.LTRIC.Core.Interfaces;
using ABE.LTRIC.Core.Specifications;
using ABE.LTRIC.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ABE.LTRIC.WpfGui.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IDocRepository _docRepository;
        private readonly IDocDtlRepository _docDtlRepository;
        public string folder = @"Reports\";
        public ReportsService(IDocRepository docRepository, IDocDtlRepository docDtlRepository)
        {
            _docRepository = docRepository;
            _docDtlRepository = docDtlRepository;
        }

        public async Task<string> DeleteReport(string reportName)
        {
            if (File.Exists($"{reportName}"))
            {
                try
                {
                    File.Delete($"{reportName}");
                    await Task.CompletedTask;
                }
                catch 
                {
                    return "Please close the file, or the file maybe doesn't exist";
                }
            }
            return "SUCCESS";
        }

        public async Task GenerateReport(int? companyId, string documentNumber, DateTime from, DateTime to, bool includeDate)
        {
            if (companyId <= 0)
            {
                companyId = null;
            }

            var report = "Company,Doc Num,Principal Amount,Payment Date,Refund Date,Due Date,Is Complete,Is OD Complete,Early Sattle To Bank,Recovered From Company,LTR Total,OD Total,Total Interest,Comments"
                + Environment.NewLine;
            var docsToGenerateReportsSp = new DocsToGenrateReportsSp(companyId, documentNumber);
            var docs = (List<Doc>)await _docRepository.Get(docsToGenerateReportsSp);

            foreach (var doc in docs)
            {
                var docDtl = new List<DocDtl>();
                if (includeDate)
                {
                    to = to.Date;
                    from = from.Date;
                    var docDtlsByDocIdRecordFromToSp = new DocDtlsByDocIdRecordFromToSp(doc.Id, from, to);
                    docDtl = ((List<DocDtl>)await _docDtlRepository.Get(docDtlsByDocIdRecordFromToSp)).OrderByDescending(x => x.RecordDate).ToList();
                }
                else
                {
                    var docDtlsByDocId = new DocDtlsByDocId(doc.Id);
                    docDtl = ((List<DocDtl>)await _docDtlRepository.Get(docDtlsByDocId)).OrderByDescending(x => x.RecordDate).ToList();
                }

                if (docDtl.Count > 0)
                {
                    var lastDocDtl = docDtl[0];
                    var preDocDtlDate = docDtl[docDtl.Count - 1].RecordDate.AddDays(-1).Date;
                    var preDocDtl = doc.DocDtls.FirstOrDefault(x => x.RecordDate == preDocDtlDate);
                    if (preDocDtl != null)
                    {
                        lastDocDtl.OD_Total = lastDocDtl.OD_Total - preDocDtl.OD_Total;
                        lastDocDtl.LTR_Total = lastDocDtl.LTR_Total - preDocDtl.LTR_Total;
                        lastDocDtl.Total_Intrest = lastDocDtl.Total_Intrest - preDocDtl.Total_Intrest;

                        lastDocDtl.OD_Total = lastDocDtl.OD_Total < 0 ? 0 : lastDocDtl.OD_Total;
                        lastDocDtl.LTR_Total = lastDocDtl.LTR_Total < 0 ? 0 : lastDocDtl.LTR_Total;
                        lastDocDtl.Total_Intrest = lastDocDtl.Total_Intrest < 0 ? 0 : lastDocDtl.Total_Intrest;
                        lastDocDtl.EarlySattleToBank = lastDocDtl.EarlySattleToBank < 0 ? 0 : lastDocDtl.EarlySattleToBank;
                        lastDocDtl.RecoveredFromCompany = lastDocDtl.RecoveredFromCompany < 0 ? 0 : lastDocDtl.RecoveredFromCompany;
                    }
                    var comments = doc.Comments?.Replace(",", string.Empty).Replace(Environment.NewLine, " ");
                    report += $"{doc.Company.Name},{doc.DocNumber},{doc.PrincipleAmount},{doc.PaymentDate.Date},{lastDocDtl.RefundDate.Date},{doc.ExpectedDueDate.Date},{doc.IsEnded},{doc.IsODEnded},{lastDocDtl.EarlySattleToBank},{lastDocDtl.RecoveredFromCompany},{lastDocDtl.LTR_Total},{lastDocDtl.OD_Total},{lastDocDtl.Total_Intrest},{comments}"
                        + Environment.NewLine;
                }
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var fileName = $"Report_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            File.AppendAllText($"{folder}{fileName}", report);
        }

        public List<string> GetAllReports()
        {
            if (!Directory.Exists(folder))
            {
                return new List<string>();
            }
            var files = Directory.GetFiles(folder, "*.csv");
            return files.ToList();
        }

        public async Task OpenReport(string reportName)
        {
            if (File.Exists($"{reportName}"))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = $"{reportName}",
                    UseShellExecute = true,
                });
            }
            await Task.CompletedTask;
        }
    }
}
