using ABE.LTRIC.Core.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Core.Specifications
{
    public class DocsToGenrateReportsSp : Specification<Doc>
    {
        public DocsToGenrateReportsSp(int? companyId, string documentNumber)
        {
            Query.Where(x =>
                (companyId == null ? true : x.CompanyId == companyId) &&
                (string.IsNullOrEmpty(documentNumber) ? true : x.DocNumber.Contains(documentNumber)));
        }
    }
}
