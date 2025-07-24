using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SRNBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public int? LocationID { get; set; }    //Check spGetUnProcessedGRN_Result
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public string ReceiptStore { get; set; }
        public string WarehouseName { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string DeliveryChallanNo { get; set; }
        public DateTime DeliveryChallanDate { get; set; }
        public int WarehouseID { get; set; }
        public bool PurchaseCompleted { get; set; }
        public bool Cancelled { get; set; }
        public Nullable<DateTime> CancelledDate { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int FinYear { get; set; }
        public int ApplicationID { get; set; }
    }
}
