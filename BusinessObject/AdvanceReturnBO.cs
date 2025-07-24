using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class AdvanceReturnBO
    {
        public int ID { get; set; }
        public string ReturnNo { get; set; }
        public DateTime Date { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public decimal NetAmount { get; set; }
        public string Category { get; set; }
        public bool IsOfficial { get; set; }
        public bool IsDraft { get; set; }
        // public List<AdvanceReturnTransBO> UnProcessedAPList { get; set; }

        public List<AdvanceReturnTransBO> Items { get; set; }

        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public string BankName { get; set; }
        public int BankID { get; set; }
        public string ReferenceNumber { get; set; }
        public string Remarks { get; set; }

    }
    public class AdvanceReturnTransBO
    {
        public int ID { get; set; }
        public int AdvanceID { get; set; }
        public DateTime PODate { get; set; }
        public string TransNo { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal Amount { get; set; }
        public decimal ReturnAmount { get; set; }
        public decimal AdvancePaidAmount { get; set; }

    }
}