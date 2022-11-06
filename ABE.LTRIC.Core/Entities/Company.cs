using ABE.LTRIC.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Core.Entities
{
    public class Company : EntityBase
    {
        public string Name { get; set; }
        public ICollection<Doc> Docs { get; set; }
    }
}
