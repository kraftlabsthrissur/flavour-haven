using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AdvancePaymentBO
    {
        public int ID { get; set; }
        public string AdvancePaymentNo { get; set; }
        public DateTime AdvancePaymentDate { get; set; }
        public string AdvancePaymentDateStr { get; set; }
        public int ModeOfPaymentID { get; set; }
        public string ModeOfPaymentName { get; set; }
        public int BankID { get; set; }
        public string BankDetail { get; set; }
        public int SelectedID { get; set; }     //SupplierID or EmployeeID 
        public string SelectedName { get; set; }     //Supplier or Employee Name
        public string AccountNo { get; set; }
        public string ReferenceNo { get; set; }
        public string SupplierName { get; set; }
        public string Purpose { get; set; }
        public decimal Amt { get; set; }
        public bool Draft { get; set; }
        public decimal? NetAmount { get; set; }
        public int SupplierID { get; set; }
        public int EmployeeID { get; set; }
        public bool IsOfficial
        {
            get
            {
                return Purpose != null && Purpose.ToLower().Equals("official");
            }
        }
        public bool IsPayment
        {
            get
            {
                return Purpose != null && Purpose.ToLower().Equals("personal");
            }
        }

        public string SaveType { get; set; }
        public bool IsDraft { get { return SaveType != null && SaveType.ToLower().Equals("draft"); } }
        public string Category { get; set; }
        public decimal Amount
        {
            get
            {
                return AdvancePaymentPurchaseOrders == null || AdvancePaymentPurchaseOrders.Count() <= 0 ? 0 :
                    AdvancePaymentPurchaseOrders.Sum(x => x.Amount);
            }
        }
        public string SupplierOrEmployeeBankName { get; set; }
        public string SupplierOrEmployeeBankACNo { get; set; }
        public string SupplierOrEmployeeIFSCNo { get; set; }
        public List<AdvancePaymentPurchaseOrderBO> AdvancePaymentPurchaseOrders { get; set; }
        public List<AdvancePaymentTransBO> PaymentTrans { get; set; }
    }


    public class AdvancePaymentTransBO
    {
        public int ID { get; set; }
        public string AdvancePaymentNo { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public string PurchaseOrderTerms { get; set; }
        public string ItemName { get; set; }
        public decimal Amount { get; set; }
        public string TDSCode { get; set; }
        public Decimal TDSAmount { get; set; }
        public string Remarks { get; set; }
        public int ItemID { get; set; }

    }
    public class AdvancePaymentPurchaseOrderBO
    {
        public int ID { get; set; }
        public int PurchaseOrderID { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public string PurchaseOrderDateStr { get; set; }
        public string TransNo { get; set; }
        public string PurchaseOrderTerms { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal Amount { get; set; }
        public int TDSID { get; set; }
        public string TDSCode { get; set; }
        public decimal TDSAmount { get; set; }
        public string Remarks { get; set; }
        public int PaymentWithin { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal POAmount { get; set; }
        public decimal TDSRate { get; set; }
        public decimal Advance { get; set; }
    }
}
