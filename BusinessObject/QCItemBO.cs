using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class QCItemBO
    {
        public int ID { get; set; }
        public string QCNo { get; set; }
        public DateTime QCDate { get; set; }

        public int GRNID { get; set; }
        public int GRNTransID { get; set; }
        public DateTime GRNDate { get; set; }

        public int ProductionID { get; set; }
        public int ProductionIssueID { get; set; }
        public DateTime ProductionIssueDate { get; set; }

        public string ReferenceNo { get; set; }
        public int? WareHouseID { get; set; }
        public int? ItemID { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string SupplierName { get; set; }
        public string BatchNo { get; set; }
        public string QCStatus { get; set; }
        public int? ToWareHouseID { get; set; }
        public decimal? AcceptedQty { get; set; }
        public decimal? ApprovedQty { get; set; }
        public decimal StandardOutput { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string Remarks { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }
        public bool IsDraft { get; set; }
        public decimal BatchSize { get; set; }
        public string DeliveryChallanNo { get; set; }
        public DateTime DeliveryChallanDate { get; set; }
    }
}
