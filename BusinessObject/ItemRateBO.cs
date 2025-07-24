using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ItemRateBO
    {
        public int ItemID { get; set; }
        public int BatchTypeID { get; set; }
        public decimal Rate { get; set; }
    }
}
