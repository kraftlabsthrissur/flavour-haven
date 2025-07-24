using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ServiceRecieptNoteBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public int SupplierID { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string DeliveryChallanNo { get; set; }
        public DateTime? DeliveryChallanDate { get; set; }
        public bool IsDraft { get; set; }
        public int CreatedUserID { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }
        public DateTime ServicePODate { get; set; }
        public string SupplierName { get; set; }
        public string Location { get; set; }
        public bool IsCancelled { get; set; }

    }
}
