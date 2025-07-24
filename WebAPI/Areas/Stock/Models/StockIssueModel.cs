using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Areas.Stock.Models
{
    public class StockIssueModel
    {
        public int ID { get; set; }
        public string IssueNo { get; set; }
        public string Date { get; set; }
        public string RequestNo { get; set; }
        public int RequestID { get; set; }

        public string IssueLocationName { get; set; }
        public string ReceiptLocationName { get; set; }
        public string IssuePremiseName { get; set; }
        public string ReceiptPremiseName { get; set; }

        public int IssueLocationID { get; set; }
        public int ReceiptLocationID { get; set; }
        public int IssuePremiseID { get; set; }
        public int ReceiptPremiseID { get; set; }

        public string Status { get; set; }
        public int ItemCategoryID { get; set; }
        public int BatchTypeID { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCancelled { get; set; }
        public string CancelledDate { get; set; }
        public int UnitID { get; set; }

        public string ProductionGroup { get; set; }
        public string Batch { get; set; }
        public List<StockIssueItem> Items { get; set; }

        public List<StockIssuePackingDetailsModel> PackingDetails { get; set; }



        public int LocationStateID { get; set; }

        public decimal GrossAmount { get; set; }
        public decimal TradeDiscount { get; set; }
        public decimal TradeDiscountPercent { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }

        public string IssueType { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string GSTNo { get; set; }
        public bool IsService { get; set; }
        public string Remark { get; set; }
    }

    public class StockIssuePackingDetailsModel
    {
        public string PackSize { get; set; }
        public string PackUnit { get; set; }
        public decimal Quantity { get; set; }
        public int PackUnitID { get; set; }
    }
    public class StockIssueItem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
        public decimal RequestedQty { get; set; }
        public decimal IssueQty { get; set; }
        public string IssueDate { get; set; }
        public int StockRequestTransID { get; set; }
        public int StockRequestID { get; set; }
        public decimal Rate { get; set; }
        public decimal NetAmount { get; set; }
        public int ItemID { get; set; }
        public decimal Stock { get; set; }
        public int RequestTransID { get; set; }
        public string StockRequisitionNo { get; set; }
        public string Remark { get; set; }

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

        public decimal GSTPercentage
        {
            get
            {
                return IGSTPercentage;
            }

            set
            {
                GSTPercentage = value;
            }
        }

        public decimal GSTAmount
        {
            get
            {
                return CGSTAmount + SGSTAmount + IGSTAmount;
            }

            set
            {
                GSTAmount = value;
            }
        }

        public decimal PackSize { get; set; }
        public string Category { get; set; }
        public string SecondaryUnit { get; set; }
        public int PrimaryUnitID { get; set; }
        public string PrimaryUnit { get; set; }
    }
}