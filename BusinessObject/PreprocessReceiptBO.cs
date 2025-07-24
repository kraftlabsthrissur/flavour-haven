using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PreprocessReceiptBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime TransDate { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCancelled { get; set; }
        //public List<PreprocessReceiptItemBO> Items { get; set; }

        public List<PreProcessReceiptPurificationItemBO> PreProcessReceiptPurificationItemBOList { get; set; }
    }
    public class PreprocessReceiptItemBO
    {
        public int ID { get; set; }
        public int PurificationIssueID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int PurifiedItemID { get; set; }
        public string PurifiedItemName { get; set; }
        public string PurifiedItemUnit { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public decimal Quantity { get; set; }
        public decimal QtyMet { get; set; }
        public decimal BalanceQty { get; set; }
        public int ProcessID { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime TransTime { get; set; }
        public string ProcessName { get; set; }
        public string TransNo { get; set; }
        public int PurifiedItemUnitID { get; set; }
    }

    public class PreProcessReceiptPurificationItemBO
    {
        public int ReceiptItemID { get; set; }
        public string ReceiptItem { get; set; }
        public string ReceiptItemUnit { get; set; }
        public int ReceiptItemUnitID { get; set; }
        public int IssuedItemID { get; set; }
        public string IssuedItemName { get; set; }
        public string IssuedItemUnit { get; set; }
        public decimal IssuedQuantity { get; set; }
        public DateTime IssuedDate { get; set; }
        public string ProcessName { get; set; }
        public decimal ReceiptQuantity { get; set; }
        public decimal BalanceQty { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int MaterialPurificationIssueTransID { get; set; }
        public bool IsCompleted { get; set; }
        public decimal QtyMet { get; set; }
    }

    /// <summary>
    /// This is used in Index
    /// </summary>
    public class PreProcessReceiptDisplayBO
    {
        public int ID { get; set; }
        public string IssuedItem { get; set; }
        public string IssuedItemUnit { get; set; }
        public decimal IssuedQuantity { get; set; }
        public string ReceiptItem { get; set; }
        public decimal ReceiptQuantity { get; set; }
        public string ReceiptItemUnit { get; set; }
        public string Activity { get; set; }
        public decimal QuantityLoss { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCancelled { get; set; }
        public string ReceiptCode { get; set; }
        public int ReceiptItemUnitID { get; set; }
    }
}
