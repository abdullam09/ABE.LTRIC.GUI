using ABE.LTRIC.Core.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Core.Specifications
{
    public class DocDtlsByDocId : Specification<DocDtl>
    {
        public DocDtlsByDocId(int docId)
        {
            Query.Where(x => x.DocId == docId).OrderBy(x => x.RecordDate);
        }
    }
}
