using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SRNTransBO
    {

        public int SRNID { get; set; }
        public int POServiceID { get; set; }
        public int? POServiceTransID { get; set; }
        public int ItemID { get; set; }
        public decimal PurchaseOrderQty { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal AcceptedQty { get; set; }
        public int? ServiceLocationID { get; set; }
        public string ServiceLocation { get; set; }
        public int? DepartmentID { get; set; }
        public string Department { get; set; }

        public int? EmployeeID { get; set; }
        public string Employee { get; set; }
        public int? CompanyID { get; set; }
        public string Company { get; set; }
        public int? ProjectID { get; set; }
        public string Project { get; set; }
        public string Remarks { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }

        public string PurchaseOrderNo { get; set; }
        public string Unit { get; set; }
        public string ItemName { get; set; }
        public decimal? QtyTolerancePercent { get; set; }
          //code below by prama on 12-6-18
        public string TravelFrom { get; set; }
        public string TravelTo { get; set; }
        public string TransportMode { get; set; }
        public string TravelingRemarks { get; set; }
        public DateTime? TravelDate { get; set; }
        public string TravelDateString { get; set; }
        public int CategoryID { get; set; }
        public decimal TolaranceQty { get; set; }
        public decimal PORate { get; set; }

    }
}
