using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Manufacturing.Models
{
    public class JobWorkIssueViewModel
    {
        public int ID { get; set; }
        public string IssueNo { get; set; }
        public string IssueDate { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCancelled { get; set; }
        public string Date { get; set; }
        public int CreatedUserID { get; set; }
        public string CreatedUser { get; set; }
        public string Status { get; set; }
        public string Supplier { get; set; }
        public int SupplierID { get; set; }
        public SelectList WarehouseList { get; set; }
        public string Warehouse { get; set; }
        public int WarehouseID { get; set; }
        public List<JobWorkIssueItemModel> Items { get; set; }
    }
    public class JobWorkIssueItemModel
    {
        public string IssueItemName { get; set; }
        public int IssueItemID { get; set; }
        public string IssueUnit { get; set; }
        public decimal IssueQty { get; set; }
        public int IssueTransID { get; set; }
        public decimal QtyMet { get; set; }
        public int WarehouseID { get; set; }
        public decimal Stock { get; set; }
    }


}