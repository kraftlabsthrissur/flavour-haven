using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class UnSettledPurchaseInoviceBO
    {
        public string Description { get; set; }
        public string SupplierName { get; set; }
        public DateTime CreatedDate { get; set; }
        public double InvoiceAmount { get; set; }
        public double AmountToBePaid { get; set; }
        public int PayableID { get; set; }
    }
}
