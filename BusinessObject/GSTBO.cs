using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
  public  class GSTBO
    {
       public int ID { get; set; }
        public string GST { get; set; }
        public int GSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal IGST { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal CGST { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal SGST { get; set; }
    }
}
