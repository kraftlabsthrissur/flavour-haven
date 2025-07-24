using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Manufacturing.Models
{
    public class JobWorkReceiptViewModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string TransDate { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCancelled { get; set; }
        public string Supplier { get; set; }
        public int SupplierID { get; set; }
        public int IssueID { get; set; }
        public string IssueNo { get; set; }
        public List<JobWorkIssuedModel> IssuedItems { get; set; }
        public List<JobWorkReceiptModel> ReceiptItems { get; set; }
        public string Status { get; set; }
        public string IssueDate { get; set; }
        public SelectList WarehouseList { get; set; }
        public string Warehouse { get; set; }
        public int WarehouseID { get; set; }
    }
    public class JobWorkIssuedModel
    {
        public int IssueTransID { get; set; }
        public decimal PendingQuantity { get; set; }
        public bool IsCompleted { get; set; }
        public decimal IssuedQty { get; set; }
        public string IssuedItem { get; set; }
        public string IssuedUnit { get; set; }

    }
    public class JobWorkReceiptModel
    {
        public int ReceiptItemID { get; set; }
        public string ReceiptUnit { get; set; }
        public decimal ReceiptQty { get; set; }
        public string ReceiptDate { get; set; }
        public string ReceiptItemName { get; set; }
        public int WarehouseID { get; set; }
    }
}