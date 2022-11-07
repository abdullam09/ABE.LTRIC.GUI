using ABE.LTRIC.Core.Entities;
using ABE.LTRIC.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Infrastructure.Services
{
    public class DocRepository : Repository<Doc>, IDocRepository
    {
        public DocRepository(LTRIC_Context context) : base(context)
        {

        }
    }
}
