using ABE.LTRIC.Core.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Core.Specifications
{
    public class DocByIdSp : Specification<Doc>
    {
        public DocByIdSp(int id)
        {
            Query.Where(x => x.Id == id).Take(1);
        }
    }
}
