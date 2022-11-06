using ABE.LTRIC.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Core.Entities
{
    public class Setting : EntityBase
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
