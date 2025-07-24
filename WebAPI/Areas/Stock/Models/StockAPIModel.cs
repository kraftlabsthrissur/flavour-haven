using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Areas.Masters.Models;

namespace WebAPI.Areas.Stock.Models
{

    public class StockAPIModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int UnitID { get; set; }
        public int BatchID { get; set; }
        public int WarehouseID { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string Batch { get; set; }
        public string WareHouse { get; set; }
        public string Status { get; set; }
        public string ExpiryDate { get; set; }
        public decimal CurrentQty { get; set; }
        public decimal PhysicalQty { get; set; }
        public List<StockAPIItemsModel> Items { get; set; }
    }
    public class StockAPIItemsModel : ItemModel
    {
        public string TransNo { get; set; }
        public string FullOrLoose { get; set; }
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        public decimal PhysicalQty { get; set; }
        public decimal CurrentQty { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
        public string Unit { get; set; }
        public string BatchNo { get; set; }
        public string ExpiryDateString { get; set; }
        public int WareHouseID { get; set; }
    }
}