//File created by prama on 19-4-18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace BusinessObject
{
    public class StockIssueBO
    {
        public int ID { get; set; }
        public string IssueNo { get; set; }
        public DateTime Date { get; set; }
        public string RequestNo { get; set; }
        public int RequestID { get; set; }

        public string IssueLocationName { get; set; }
        public string IssuePremiseName { get; set; }
        public string ReceiptLocationName { get; set; }
        public string ReceiptPremiseName { get; set; }

        public string BatchTypeID { get; set; }
        public string BatchName { get; set; }

        public int IssueLocationID { get; set; }
        public int IssuePremiseID { get; set; }
        public int ReceiptLocationID { get; set; }
        public int ReceiptPremiseID { get; set; }

        public bool IsCancelled { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsDraft { get; set; }
        public bool IsService { get; set; }


        public decimal GrossAmount { get; set; }
        public decimal TradeDiscount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }

        public string IssueType { get; set; }

        public string ProductionGroup { get; set; }
        public string Batch { get; set; }
        public string Remark { get; set; }
        public string VehicleNo { get; set; }
        public string IssueLocationGSTNo { get; set; }
        public string ReceiptLocationGSTNo { get; set; }
        public DateTime CancelledDate { get; set; }
        public List<StockIssueItemBO> Items { get; set; }
        public List<StockIssuePackingDetailsBO> PackingDetails { get; set; }

    }
    public class StockIssuePackingDetailsBO
    {
        public string PackSize { get; set; }
        public string PackUnit { get; set; }
        public decimal Quantity { get; set; }
        public int PackUnitID { get; set; }
    }

    public class StockIssueItemBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PartsNo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
        public decimal RequestedQty { get; set; }
        public decimal IssueQty { get; set; }
        public DateTime IssueDate { get; set; }
        public int StockRequestTransID { get; set; }
        public int StockRequestID { get; set; }
        public int StockIssueTransID { get; set; }
        public int StockIssueID { get; set; }
        public decimal Rate { get; set; }
        public decimal NetAmount { get; set; }
        public int ItemID { get; set; }
        public decimal Stock { get; set; }
        public int RequestTransID { get; set; }
        public string StockRequisitionNo { get; set; }

        public decimal BasicPrice { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TradeDiscountPercentage { get; set; }
        public decimal TradeDiscount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal PackSize { get; set; }
        public string Category { get; set; }
        public decimal SecondaryQty { get; set; }
        public decimal SecondaryIssueQty { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public string SecondaryUnit { get; set; }
        public string SecondaryUnits { get; set; }

        public string MalayalamName { get; set; }
        public string PrimaryUnit { get; set; }
        public int PrimaryUnitID { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Remark { get; set; }

    }
}
