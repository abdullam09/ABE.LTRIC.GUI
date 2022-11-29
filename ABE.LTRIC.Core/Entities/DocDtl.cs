using ABE.LTRIC.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Core.Entities
{
    public class DocDtl : EntityBase
    {
        public int CompanyId { get; set; }
        public int DocId { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime RefundDate { get; set; }
        public decimal EarlySattleToBank { get; set; }
        public decimal RecoveredFromCompany { get; set; }
        public int LTR_Period { get; set; }
        public int OD_Period { get; set; }
        public decimal LTR_Intrest { get; set; }
        public decimal OD_Interest { get; set; }
        public decimal LTR_Total { get; set; }
        public decimal OD_Total { get; set; }
        public decimal Total_Intrest { get; set; }
        public decimal TotalChargeToBank { get; set; }
        public decimal TotalChargeToCompany { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
