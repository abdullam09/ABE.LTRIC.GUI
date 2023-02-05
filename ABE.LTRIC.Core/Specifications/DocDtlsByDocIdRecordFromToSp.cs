using ABE.LTRIC.Core.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ABE.LTRIC.Core.Specifications
{
    public class DocDtlsByDocIdRecordFromToSp : Specification<DocDtl>
    {
        public DocDtlsByDocIdRecordFromToSp(int docId, DateTime from, DateTime to)
        {
            Query.Where(x => x.DocId == docId && x.RecordDate >= from && x.RecordDate < to).OrderBy(x => x.RecordDate);
        }
    }
}
