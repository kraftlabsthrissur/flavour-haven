using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class AdvanceReturnModel
    {
        public int ID { get; set; }
        public string ReturnNo { get; set; }
        public string Date { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public decimal NetAmount { get; set; }
        public string Category { get; set; }
        public bool IsOfficial { get; set; }
        public bool IsDraft { get; set; }
        public string Status { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public SelectList PaymentTypeList { get; set; }
        public string BankName { get; set; }
        public int BankID { get; set; }
        public SelectList BankList { get; set; }
        public string ReferenceNumber { get; set; }
        public string Remarks { get; set; }
        public decimal Amount { get; set; }

        public List<SelectListItem> Categories { get; set; }
        public List<AdvancePaymentModel> UnProcessedAPList { get; set; }
        public List<AdvanceReturnTransModel> Items { get; set; }


    }
    public class AdvanceReturnTransModel
    {
        public int ID { get; set; }
        public int AdvanceID { get; set; }
        public string PODate { get; set; }
        public string TransNo { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal Amount { get; set; }
        public decimal ReturnAmount { get; set; }
        public decimal AdvancePaidAmount { get; set; }

    }
}