using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class OpeningStockBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime Date { get; set; }
        public bool IsDraft { get; set; }
        public int ItemCategoryID { get; set; }
        public int ItemID { get; set; }
        public int UnitID { get; set; }
        public decimal Qty { get; set; }
        public string Batch { get; set; }
        public int BatchID { get; set; }
        public int BatchTypeID { get; set; }
        public int WarehouseID { get; set; }
        public int SlNo { get; set; }
        public String Store { get; set; }
        public int OpeningStockID { get; set; }
        public decimal MRP { get; set; }
    }

    public class OpeningStockItemBO
    {
        public int WarehouseID { get; set; }
        public int ItemID { get; set; }
        public int BatchTypeID { get; set; }
        public int BatchID { get; set; }
        public string Batch { get; set; }
        public int UnitID { get; set; }
        public decimal Qty { get; set; }
        public decimal Value { get; set; }
        public string ItemName { get; set; }
        public string BatchType { get; set; }
        public string Unit { get; set; }
        public DateTime ExpDate { get; set; }
    }

    public class OpeningStockMRPBO
    {
        public decimal MRP { get; set; }
    }



}

