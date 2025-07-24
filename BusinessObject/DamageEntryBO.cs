using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DamageEntryBO
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string TransNo { get; set; }
        public int? WarehouseID { get; set; }
        public string Warehouse { get; set; }
        public int DamageTypeID { get; set; }
        public string DamageType { get; set; }
        public bool IsDraft { get; set; }
        public List<DamageEntryItemBO> ItemList { get; set; }
    }
    public class DamageEntryItemBO
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string TransNo { get; set; }
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
        public bool IsDraft { get; set; }
        public int? WarehouseID { get; set; }
        public string WareHouse { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ExpiryDateString { get; set; }
        public int? DamageTypeID { get; set; }
        public string DamageType { get; set; }
        public string Remarks { get; set; }
    }
}
