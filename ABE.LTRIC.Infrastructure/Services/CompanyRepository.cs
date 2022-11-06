using ABE.LTRIC.Infrastructure.Entities;
using ABE.LTRIC.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Infrastructure.Services
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(LTRIC_Context context) : base(context)
        {
        }
    }
}
