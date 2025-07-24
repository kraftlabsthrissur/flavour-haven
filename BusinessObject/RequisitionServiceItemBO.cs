using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class RequisitionServiceItemBO
    {
        public int ID { get; set; }
        public int PurchaseRequisitionServiceID { get; set; }
        public int ItemID { get; set; }
        public string Item { get; set; }

        public decimal Quantity { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public Nullable<int> UnitID { get; set; }
        public string Unit { get; set; }

        public int RequiredLocationID { get; set; }
        public string Location { get; set; }

        public int RequiredDepartmentID { get; set; }
        public string Department { get; set; }

        public int RequiredEmployeeID { get; set; }
        public string Employee { get; set; }

        public int RequiredInterCompanyID { get; set; }
        public string InterCompany { get; set; }

        public int RequiredProjectID { get; set; }
        public string Project { get; set; }

        public string Remarks { get; set; }
        public int CreatedUserID { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; } 
        public int? TravelFromID { get; set; }
        public int? TravelToID { get; set; }
        public int? TransportModeID { get; set; }
        public string TravelFrom { get; set; }
        public string TravelTo { get; set; }
        public string TransportMode { get; set; }
        public string TravelingRemarks { get; set; }
        public DateTime? TravelDate { get; set; }
        public int CategoryID { get; set; }
        public int TravelCategoryID { get; set; }



    }
}
