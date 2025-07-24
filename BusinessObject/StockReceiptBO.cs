//File created by prama on 29-6-18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class StockReceiptBO
    {

        public int ID { get; set; }
        public string ReceiptNo { get; set; }
        public DateTime Date { get; set; }
        public string IssueLocationName { get; set; }
        public string IssuePremiseName { get; set; }
        public string ReceiptLocationName { get; set; }
        public string ReceiptPremiseName { get; set; }

        public string ReceiptType { get; set; }
        public string BatchType { get; set; }
        public int BatchTypeID { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsDraft { get; set; }
        public bool IsService { get; set; }
        public decimal NetAmount { get; set; }
        public List<StockReceiptItemBO> Item { get; set; }
    }

    public class StockReceiptItemBO
    {
        public int ItemID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string BatchName { get; set; }
        public int UnitID { get; set; }
        public string BatchType { get; set; }
        public decimal RequestedQty { get; set; }
        public decimal IssueQty { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int StockIssueTransID { get; set; }
        public int StockIssueID { get; set; }
        public decimal ReceiptQty { get; set; }
        public decimal SecondaryReceiptQty { get; set; }
        public decimal SecondaryIssueQty { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public string SecondaryUnit { get; set; }
        public int BatchID { get; set; }
        public int BatchTypeID { get; set; }
        public int IssuePremiseID { get; set; }
        public int ReceiptPremiseID { get; set; }
        public int IssueLocationID { get; set; }
        public int ReceiptLocationID { get; set; }
        public decimal Rate { get; set; }
        public decimal NetAmount { get; set; }       
        public decimal TradeDiscount { get; set; }
        public decimal TradeDiscountPercent { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal BasicPrice { get; set; }

    }
}