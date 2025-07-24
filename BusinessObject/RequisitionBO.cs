using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class RequisitionBO
    {
        public int ID { get; set; }
        public int SalesInquiryID {  get; set; }
        public string Code { get; set; }
        public DateTime? Date { get; set; }
        public string RequisitionNo { get; set; }
        public bool QuotationProcessed { get; set; }
        public bool FullyOrdered { get; set; }
        public int FromDeptID { get; set; }
        public string FromDepartment { get; set; }
        public string PurchaseRequisitedCustomer { get; set; }
        public string RequisitedCustomerAddress { get; set; }
        public string RequisitedPhoneNumber1 { get; set; }
        public string RequisitedPhoneNumber2 { get; set; }
        public string Remarks { get; set; }
        public int ToDeptID { get; set; }
        public string ToDepartment { get; set; }
        public bool Cancelled { get; set; }
        public Nullable<System.DateTime> CancelledDate { get; set; }
        public Nullable<int> CreatedUserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }

        public string ItemCategory { get; set; }
        public string ItemName { get; set; }

        public bool IsDraft { get; set; }
        public List<ItemBO> Products { get; set; }

       
        public string POSNumber { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }

    }
}
