using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class DatatableResultBO
    {
        public int draw { get; set; }
        public List<object> data { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
    }
}
