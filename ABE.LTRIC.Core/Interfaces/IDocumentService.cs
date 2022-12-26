using ABE.LTRIC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Core.Interfaces
{
    public interface IDocumentService
    {
        Task ProcessDocument(Doc doc);
    }
}
