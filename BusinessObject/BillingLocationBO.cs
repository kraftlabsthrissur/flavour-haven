using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class BillingLocationBO
    {
        public int LocationID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int BillingLocationID { get; set; }
        public string BillingLocation { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public int StateID { get; set; }
    }

}
