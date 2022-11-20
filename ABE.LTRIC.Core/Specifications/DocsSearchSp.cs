using ABE.LTRIC.Core.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Core.Specifications
{
    public class DocsSearchSp : Specification<Doc>
    {
        private bool? isComplete;
        private bool? isOdComplete;

        public DocsSearchSp(int? companyId, string docNum, bool enablePaymentDateSearch, DateTime fromPaymentDate, DateTime toPaymentDate, decimal? amount, string isCompleteValue, string isOdCompleteValue)
        {
            isComplete = ConvertIsCompleteValueToBool(isCompleteValue);
            isOdComplete = ConvertIsCompleteValueToBool(isOdCompleteValue);

            Query.Where(x =>
                (companyId == null ? true : x.CompanyId == companyId) &&
                (string.IsNullOrEmpty(docNum) ? true : x.DocNumber.Contains(docNum)) &&
                (enablePaymentDateSearch ? x.PaymentDate >= fromPaymentDate : true) &&
                (enablePaymentDateSearch ? x.PaymentDate <= toPaymentDate : true) &&
                (amount == null ? true : x.PrincipleAmount == amount) &&
                (isComplete == null ? true : x.IsEnded == isComplete) &&
                (isOdComplete == null ? true : x.IsODEnded == isOdComplete)
                );
        }

        private bool? ConvertIsCompleteValueToBool(string isCompleteValue)
        {
            switch (isCompleteValue)
            {
                case "Yes":
                    return true;
                case "No":
                    return false;
                default:
                    return null;
            }
        }
    }
}
