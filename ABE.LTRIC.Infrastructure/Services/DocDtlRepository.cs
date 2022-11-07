using ABE.LTRIC.Core.Entities;
using ABE.LTRIC.Core.Interfaces;
using ABE.LTRIC.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Infrastructure.Services
{
    public class DocDtlRepository : Repository<DocDtl>, IDocDtlRepository
    {
        public DocDtlRepository(LTRIC_Context context) : base(context)
        {

        }
    }
}
