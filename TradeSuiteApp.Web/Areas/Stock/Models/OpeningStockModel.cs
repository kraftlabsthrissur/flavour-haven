using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Stock.Models
{
    public class OpeningStockModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string Date { get; set; }
        public bool IsDraft { get; set; }
        public SelectList WarehouseList { get; set; }
        public int WarehouseID { get; set; }
        public int ItemCategoryID { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public int BatchTypeID { get; set; }
        public SelectList BatchTypeList { get; set; }
        public SelectList BatchList { get; set; }
        public List<OpeningStockItemModel> Items { get; set; }
        public int SlNo { get; set; }
        public String Store { get; set; }
        public int OpeningStockID { get; set; }
        public string Status { get; set; }
        public decimal MRP { get; set; }
        public List<UnitModel> UOMList { get; set; }
        public int UnitID { get; set; }
    }

    public class OpeningStockItemModel
    {
        public int WarehouseID { get; set; }
        public int ItemID { get; set; }
        public int BatchTypeID { get; set; }
        public string Batch { get; set; }
        public int UnitID { get; set; }
        public decimal Qty { get; set; }

        public string ItemName { get; set; }
        public string BatchType { get; set; }
        public string Unit { get; set; }
        public int BatchID { get; set; }
        public decimal Value { get; set; }

        public string ExpDate { get; set; }
    }

    public class OpeningStockMRPModel
    {
        public decimal MRP { get; set; }
    }
}



