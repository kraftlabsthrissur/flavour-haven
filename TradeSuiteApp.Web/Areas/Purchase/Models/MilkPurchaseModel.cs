using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class MilkPurchaseModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string Date { get; set; }
        public string DateString { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalQty { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDraft { get; set; }
        public string FatRange { get; set; }
        public string FatID { get; set; }
        public string Status { get; set; }
        public int MilkSupplierID { get; set; }
        public string SupplierName { get; set; }
        public string PurchaseRequisitionIDS { get; set; }
        public string RequiredQty { get; set; }
        public string RequisitionIDs { get; set; }
        public bool DirectInvoice { get; set; }
        public List<MilkPurchaseRequisitionModel> UnProcessedPrList { get; set; }
        public List<MilkFatRangeModel> FatList { get; set; }
        public List<MilkPurchseTransModel> MilkPurchaseTrans { get; set; }
    }
    public class MilkPurchaseRequisitionModel
    {
        public int ID { get; set; }
        public string ExpectedDate { get; set; }
        public decimal Qty { get; set; }
        public string PrNumber { get; set; }
        public string FromDept { get; set; }
    }
    public class MilkPurchseTransModel
    {
        public int ID { get; set; }
        public int MilkPurchaseID { get; set; }
        public int MilkSupplierID { get; set; }
        public string SlipNo { get; set; }
        public decimal Qty { get; set; }
        public int FatRangeID { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string MilkSupplier { get; set; }
        public decimal FatContentFrom { get; set; }
        public decimal FatContentTo { get; set; }
        public string SupplierCode { get; set; }
        public string Remarks { get; set; }
    }

    public class MilkFatRangeModel
    {
        public int ID { get; set; }
        public int SupplierID { get; set; }
        public string FatRange { get; set; }
        public decimal WaterFrom { get; set; }
        public decimal WaterTo { get; set; }
        public decimal Price { get; set; }
    }
}