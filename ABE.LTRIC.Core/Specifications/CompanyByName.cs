using ABE.LTRIC.Core.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Core.Specifications
{
    public class CompanyByName : Specification<Company>
    {
        public CompanyByName(string name)
        {
            Query.Where(x => x.Name == name);
        }
    }
}
