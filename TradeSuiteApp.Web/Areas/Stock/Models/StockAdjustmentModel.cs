using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TradeSuiteApp.Web.Areas.Stock.Models
{
    public class StockAdjustmentModel
    {
        public int ID { get; set; }
        public String Date { get; set; }
        public string TransNo { get; set; }
        public SelectList WarehouseList { get; set; }
       public int ItemCategoryID { get; set; }
        public SelectList CategoryList { get; set; }
        public int? WarehouseID { get; set; }
        public string Warehouse { get; set; }
        public string Status { get; set; }
        public SelectList DamageTypeList { get; set; }
        //public int ItemID { get; set; }
        //public string ItemName { get; set; }
        //public String ItemCategory { get; set; }
        ////public string Batch { get; set; }
        //public int BatchID { get; set; }
        //public string Unit { get; set; }
        //public int UnitID { get; set; }
        //public string BatchType { get; set; }
        //public int BatchTypeID { get; set; }
        public bool IsDraft { get; set; }
        public int SalesCategoryID { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public SelectList BatchTypeList { get; set; }
        public SelectList UnitList { get; set; }
        public List<StockAdjustmentItem> BatchList { get; set; }
        public int BatchID { get; set; }
        public int BatchTypeID { get; set; }
        public int DamageTypeID { get; set; }
        public int UnitID { get; set; }
        public int ItemID { get; set; }
        public decimal PhysicalQty { get; set; }
        public List<StockAdjustmentItem> Items { get; set; }
    }

    public class StockAdjustmentItem
    {
        public int ID { get; set; }
        public int? ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int? UnitID { get; set; }
        public string UnitName { get; set; }
        public string Batch { get; set; }
        public int? BatchID { get; set; }
        public string BatchType { get; set; }
        public int? BatchTypeID { get; set; }
        public decimal CurrentQty { get; set; }
        public decimal PhysicalQty { get; set; }
        public String Category { get; set; }
        public int? WarehouseID { get; set; }
        public string Warehouse { get; set; }
        public string ExpiryDate { get; set; }
        public string Remark { get; set; }
        public int DamageTypeID { get; set; }
        public string DamageType { get; set; }
        public decimal Rate { get; set; }
        public decimal LooseRate { get; set; }
        public decimal ExcessQty { get; set; }
        public decimal ExcessValue { get; set; }
        public int InventoryUnitID { get; set; }
        public string InventoryUnit { get; set; }       
        public decimal FullRate { get; set; }
        public string PrimaryUnit { get; set; }
        public int PrimaryUnitID { get; set; }
    }

}
