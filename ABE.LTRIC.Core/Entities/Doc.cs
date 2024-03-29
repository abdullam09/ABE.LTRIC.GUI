﻿using ABE.LTRIC.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Core.Entities
{
    public class Doc : EntityBase
    {
        public int CompanyId { get; set; }
        public Company Company {get;set;}
        public string DocNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime ExpectedDueDate { get; set; }
        public decimal PrincipleAmount { get; set; }
        public string Comments { get; set; }
        public bool IsEnded { get; set; }
        public bool IsODEnded { get; set; }
        public DateTime ODDueDate { get; set; }
        public DateTime InsertDate { get; set; }
        [ForeignKey("DocId")]
        public ICollection<DocDtl> DocDtls { get; set; }
    }
}
