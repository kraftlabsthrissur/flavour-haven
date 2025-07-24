using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessObject;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class AdvanceRequestModel
    {
        public int? ID { get; set; }
        public string AdvanceRequestNo { get; set; }
        public string AdvanceRequestDate { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public string ExpectedDate { get; set; }
        public string Status { get; set; }
        public string ItemCatagory { get; set; }
        public int ItemCategoryID { get; set; }
        public decimal TotalAmount { get; set; }
        public string AdvanceCategory { get; set; }
        public string ItemName { get; set; }
        public bool IsSuspend { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public SelectList AdvanceCategoryList { get; set; }
        public List<AdvanceRequestTransModel> Item { get; set; }
        public List<FileBO> SelectedQuotation { get; set; }
        public int SelectedQuotationID { get; set; }
    }
    public class AdvanceRequestTransModel
    {
        public int EmployeeID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string EmployeeName { get; set; }
        public string ExpectedDate { get; set; }
        public string Remarks { get; set; }
        public decimal Amount { get; set; }
        public bool IsOfficial { get; set; }
        public string Category { get; set; }
        public int ID { get; set; }
        public string AdvanceRequestNo { get; set; }
        public string AdvanceRequestDate { get; set; }

    }
}