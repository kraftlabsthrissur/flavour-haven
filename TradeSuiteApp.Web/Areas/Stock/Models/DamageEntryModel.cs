using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Stock.Models
{
    public class DamageEntryModel
    {
        public int ID { get; set; }
        public String Date { get; set; }
        public string TransNo { get; set; }
        public SelectList WarehouseList { get; set; }
        public int ItemCategoryID { get; set; }
        public SelectList CategoryList { get; set; }
        public int? WarehouseID { get; set; }
        public string Warehouse { get; set; }
        public string DamageType { get; set; }
        public int? DamageTypeID { get; set; }
        public string Status { get; set; }
        public bool IsDraft { get; set; }

        public SelectList DamageTypeList { get; set; }
        public List<DamageEntryItem> Items { get; set; }
    }
    public class DamageEntryItem
    {
        public int ID { get; set; }
        public int? ItemID { get; set; }
        public string ItemName { get; set; }
        public int? UnitID { get; set; }
        public string UnitName { get; set; }
        public string Batch { get; set; }
        public int? BatchID { get; set; }
        public string BatchType { get; set; }
        public int? BatchTypeID { get; set; }
        public decimal? CurrentQty { get; set; }
        public decimal? DamageQty { get; set; }
        public String Category { get; set; }
        public int? WarehouseID { get; set; }
        public string Warehouse { get; set; }
        public string ExpiryDate { get; set; }
        public string DamageType { get; set; }
        public int? DamageTypeID { get; set; }
        public string Remarks { get; set; }
    }
}