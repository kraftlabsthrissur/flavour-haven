using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessObject;
using System.ComponentModel.DataAnnotations;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class QCItemModel
    {
        [Required]
        [Display(Name = "QCID")]
        public int ID { get; set; }

        public string QCNo { get; set; }
        public string QCDate { get; set; }
        public int GRNID { get; set; }
        public string GRNDate { get; set; }
        public string ReferenceNo { get; set; }
        public int WareHouseID { get; set; }
        [Required]
        [Display(Name = "Item Name")]
        public int? ItemID { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string SupplierName { get; set; }
        public string BatchNo { get; set; }
        public string QCStatus { get; set; }
        [Required]
        [Display(Name = "Store")]
        public int? ToWareHouseID { get; set; }
        public decimal? AcceptedQty { get; set; }
        [Required]
        [Display(Name = "Approved Quantity")]
        public decimal? ApprovedQty { get; set; }
        public bool IsCancelled { get; set; }
        public string Remarks { get; set; }
        public string CancelledDate { get; set; }
        public int CreatedUserID { get; set; }
        public string CreatedDate { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }
        public string WareHouse { get; set; }
        public bool IsDraft { get; set; }
        public string Status { get; set; }
        public string DeliveryChallanNo { get; set; }
        public string DeliveryChallanDate { get; set; }

    }
}