using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class StockAdjustmentBO
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string TransNo { get; set; }
        public int? WarehouseID { get; set; }
        public string Warehouse { get; set; }
        public bool IsDraft { get; set; }
        public List<StockAdjustmentItemBO> ItemList { get; set; }
        public bool IsClone { get; set; }
        public int IsPending { get; set; }
        public int ItemID { get; set; }
        public int BatchID { get; set; }
        public string ItemCode { get; set; }
        public string Batch { get; set; }
        public decimal PhysicalQty { get; set; }
    }

    public class StockAdjustmentItemBO
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string TransNo { get; set; }
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
        public bool IsDraft { get; set; }
        public int? WarehouseID { get; set; }
        public string WareHouse { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ExpiryDateString { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public int DamageTypeID { get; set; }
        public string DamageType { get; set; }
        public decimal Rate { get; set; }
        public int InventoryUnitID { get; set; }
        public string InventoryUnit { get; set; }
        public decimal LooseRate { get; set; }
        public decimal ExcessQty { get; set; }
        public decimal ExcessValue { get; set; }
        public decimal FullRate { get; set; }
        public string PrimaryUnit { get; set; }
        public int PrimaryUnitID { get; set; }
    }
}
