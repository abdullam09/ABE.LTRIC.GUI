using ABE.LTRIC.Core.Entities;
using ABE.LTRIC.Core.Interfaces;
using ABE.LTRIC.Core.Specifications;
using ABE.LTRIC.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.WpfGui.Services
{
    public class DocumentService : IDocumentService
    {
        private DateTime Today;
        private IDocDtlRepository _docDtlRepository;
        private ISettingRepository _settingRepository;
        private IDocRepository _docRepository;

        public DocumentService(IDocDtlRepository docDtlRepository, ISettingRepository settingRepository, IDocRepository docRepository)
        {
            _docDtlRepository = docDtlRepository;
            _settingRepository = settingRepository;
            _docRepository = docRepository;
        }

        public async Task ProcessDocument(Doc doc)
        {
            if (!doc.IsEnded)
            {
                Today = DateTime.Today.Date;
            }
            else
            {
                if (doc.IsODEnded)
                {
                    if (doc.ExpectedDueDate > doc.ODDueDate)
                    {
                        Today = doc.ExpectedDueDate;
                    }
                    else
                    {
                        Today = doc.ODDueDate;
                    }
                }
                else
                {
                    Today = DateTime.Today.Date;
                }
            }

            var exccededDate = doc.DocDtls.Where(x => x.RecordDate > Today);
            await _docDtlRepository.DeleteRange(exccededDate.ToList());
            var processDate = doc.PaymentDate;
            var numberOfDaysToProcess = Today.Subtract(processDate).Days + 1;
            var docDtls = doc.DocDtls.OrderBy(x => x.RecordDate).ToList();

            for (var i = 0; i < numberOfDaysToProcess; i++)
            {
                var firstDay = i == 0;
                processDate = firstDay ? processDate : processDate.AddDays(1);
                var docDtl = docDtls.FirstOrDefault(x => x.RecordDate.Date == processDate.Date);
                if (docDtl == null)
                {
                    if (firstDay)
                    {
                        docDtl = new DocDtl
                        {
                            CompanyId = doc.CompanyId,
                            DocId = doc.Id,
                            InsertDate = DateTime.Now,
                            RecordDate = processDate.Date,
                            RefundDate = processDate.Date,
                            EarlySattleToBank = 0,
                            RecoveredFromCompany = 0,
                            LTR_Period = 0,
                            OD_Period = 0,
                            LTR_Intrest = 0,
                            OD_Interest = 0,
                            LTR_Total = 0,
                            OD_Total = 0,
                            Total_Intrest = 0,
                            TotalChargeToBank = Math.Round(doc.PrincipleAmount, 4),
                            TotalChargeToCompany = Math.Round(doc.PrincipleAmount, 4),
                        };
                        doc.DocDtls.Add(docDtl);

                    }
                    else
                    {
                        var preDate = processDate.AddDays(-1);
                        var preDocDtl = doc.DocDtls.FirstOrDefault(x => x.RecordDate == preDate);
                        var intrestRateFromSettingsSp = new IntrestRateFromSettingsSp(processDate);
                        var interestSetting = (List<Setting>)await _settingRepository.Get(intrestRateFromSettingsSp);
                        var interest = decimal.Parse(interestSetting[0].Value);

                        var ltr_i = (interest * (doc.PrincipleAmount - preDocDtl.EarlySattleToBank + preDocDtl.LTR_Total)) / 365;
                        var total_ltr_i = ltr_i + preDocDtl.LTR_Total;
                        if (processDate.Date > doc.ExpectedDueDate.Date && doc.IsEnded)
                        {
                            ltr_i = preDocDtl.LTR_Intrest;
                            total_ltr_i = preDocDtl.LTR_Total;
                        }

                        var od_i = (interest * (preDocDtl.EarlySattleToBank - preDocDtl.RecoveredFromCompany + preDocDtl.OD_Total)) / 365;
                        od_i = od_i < 0 ? 0 : od_i;
                        docDtl = new DocDtl
                        {
                            CompanyId = doc.CompanyId,
                            DocId = doc.Id,
                            InsertDate = DateTime.Now,
                            RecordDate = processDate.Date,
                            RefundDate = processDate.Date,
                            EarlySattleToBank = preDocDtl.EarlySattleToBank,
                            RecoveredFromCompany = preDocDtl.RecoveredFromCompany,
                            LTR_Period = 1,
                            OD_Period = 1,
                            LTR_Intrest = Math.Round(ltr_i, 4),
                            OD_Interest = Math.Round(od_i, 4),
                            LTR_Total = Math.Round(total_ltr_i, 4),
                            OD_Total = Math.Round(od_i + preDocDtl.OD_Total, 4),
                            Total_Intrest = Math.Round(total_ltr_i + od_i + preDocDtl.OD_Total, 4),
                            TotalChargeToBank = Math.Round(preDocDtl.TotalChargeToBank + ltr_i + od_i - preDocDtl.EarlySattleToBank, 4),
                            TotalChargeToCompany = Math.Round(preDocDtl.TotalChargeToCompany - preDocDtl.RecoveredFromCompany + od_i + ltr_i, 4),
                        };

                        doc.DocDtls.Add(docDtl);

                    }
                    if (doc.Id == null || doc.Id == 0)
                    {
                        await _docRepository.Create(doc);
                    }
                    else
                    {
                        await _docRepository.Update(doc);
                    }
                }
                else
                {
                    if (firstDay)
                    {
                        docDtl.CompanyId = doc.CompanyId;
                        docDtl.DocId = doc.Id;
                        docDtl.RecordDate = processDate.Date;
                        docDtl.RefundDate = processDate.Date;
                        docDtl.EarlySattleToBank = 0;
                        docDtl.RecoveredFromCompany = 0;
                        docDtl.LTR_Period = 0;
                        docDtl.OD_Period = 0;
                        docDtl.LTR_Intrest = 0;
                        docDtl.OD_Interest = 0;
                        docDtl.LTR_Total = 0;
                        docDtl.OD_Total = 0;
                        docDtl.Total_Intrest = 0;
                        docDtl.TotalChargeToBank = Math.Round(doc.PrincipleAmount, 4) < 0 ? 0 : Math.Round(doc.PrincipleAmount, 4);
                        docDtl.TotalChargeToCompany = Math.Round(doc.PrincipleAmount, 4);
                    }
                    else
                    {
                        var preDate = processDate.AddDays(-1);
                        var preDocDtl = doc.DocDtls.FirstOrDefault(x => x.RecordDate == preDate);
                        var earlySattleToBank = preDocDtl.EarlySattleToBank == 0 || preDocDtl.EarlySattleToBank < docDtl.EarlySattleToBank ? docDtl.EarlySattleToBank : preDocDtl.EarlySattleToBank;
                        var recoveredFromCompany = preDocDtl.RecoveredFromCompany == 0 || preDocDtl.RecoveredFromCompany < docDtl.RecoveredFromCompany ? docDtl.RecoveredFromCompany : preDocDtl.RecoveredFromCompany;
                        var intrestRateFromSettingsSp = new IntrestRateFromSettingsSp(processDate);
                        var interestSetting = (List<Setting>)await _settingRepository.Get(intrestRateFromSettingsSp);
                        var interest = decimal.Parse(interestSetting[0].Value);

                        var ltr_i = (interest * (doc.PrincipleAmount - earlySattleToBank + preDocDtl.LTR_Total)) / 365;
                        var total_ltr_i = ltr_i + preDocDtl.LTR_Total;
                        if (processDate.Date > doc.ExpectedDueDate.Date && doc.IsEnded)
                        {
                            ltr_i = preDocDtl.LTR_Intrest;
                            total_ltr_i = preDocDtl.LTR_Total;
                        }

                        var od_i = (interest * (earlySattleToBank - recoveredFromCompany + preDocDtl.OD_Total)) / 365;
                        od_i = od_i < 0 ? 0 : od_i;
                        docDtl.CompanyId = doc.CompanyId;
                        docDtl.DocId = doc.Id;
                        docDtl.RecordDate = processDate.Date;
                        docDtl.RefundDate = processDate.Date;
                        docDtl.EarlySattleToBank = Math.Round(earlySattleToBank, 4);
                        docDtl.RecoveredFromCompany = Math.Round(recoveredFromCompany, 4);
                        docDtl.LTR_Period = 1;
                        docDtl.OD_Period = 1;
                        docDtl.LTR_Intrest = Math.Round(ltr_i, 4);
                        docDtl.OD_Interest = Math.Round(od_i, 4);
                        docDtl.LTR_Total = Math.Round(total_ltr_i, 4);
                        docDtl.OD_Total = Math.Round(od_i + preDocDtl.OD_Total, 4);
                        docDtl.Total_Intrest = Math.Round(total_ltr_i + od_i + preDocDtl.OD_Total, 4);
                        docDtl.TotalChargeToBank = Math.Round(doc.PrincipleAmount + docDtl.LTR_Total + docDtl.OD_Total - earlySattleToBank, 4) < 0 ? 0 : Math.Round(doc.PrincipleAmount + docDtl.LTR_Total + docDtl.OD_Total - earlySattleToBank, 4);
                        docDtl.TotalChargeToCompany = Math.Round(doc.PrincipleAmount - recoveredFromCompany + docDtl.LTR_Total + docDtl.OD_Total, 4);
                    }
                    if (doc.Id == null || doc.Id == 0)
                    {
                        await _docRepository.Create(doc);
                    }
                    else
                    {
                        await _docRepository.Update(doc);
                    }
                }
            }
        }
    }
}
