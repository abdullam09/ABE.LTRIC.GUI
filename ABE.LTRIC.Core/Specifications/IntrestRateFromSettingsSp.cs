using ABE.LTRIC.Core.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Core.Specifications
{
    public class IntrestRateFromSettingsSp : Specification<Setting>
    {
        public IntrestRateFromSettingsSp(DateTime effectiveDate)
        {
            Query.Where(x => x.EffectiveDate <= effectiveDate && x.Key == "Intrest").OrderByDescending(x => x.EffectiveDate).Take(1);
        }
    }
}
