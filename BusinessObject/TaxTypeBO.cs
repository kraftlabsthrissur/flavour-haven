using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class TaxTypeBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LocationName { get; set; }
        public int LocationID { get; set; }
        public string Description { get; set; }
    }
}
