
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessObject
{
    public class StockRequestBO
    {
        public int ID { get; set; }
        public string RequestNo { get; set; }
        public DateTime Date { get; set; }

        public string IssueLocationName { get; set; }
        public string IssuePremiseName { get; set; }
        public string ReceiptLocationName { get; set; }
        public string ReceiptPremiseName { get; set; }

        public int IssueLocationID { get; set; }
        public int IssuePremiseID { get; set; }
        public int ReceiptLocationID { get; set; }
        public int ReceiptPremiseID { get; set; }

        public bool IsCancelled { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsDraft { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsClone { get; set; }
        public string  ProductionGroup{get;set;}
        public string Batch { get; set; }
        public Nullable<DateTime> CancelledDate { get; set; }
        public List<StockRequestBO> UnProcessedSRList { get; set; }
        public List<StockRequestItemBO> UnProcesedSRTransList { get; set; }

    }


    public class StockRequestItemBO 
    {
        public int ItemID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        public string Remarks { get; set; }
        public DateTime? RequiredDate { get; set; }
        public decimal RequiredQty { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public string SecondaryUnit { get; set; }
        public string SecondaryUnits { get; set; }
        
        public int RequestTransID { get; set; }
        public int RequestID { get; set; }
        public decimal SecondaryQty { get; set; }
        public decimal RequestedQty { get; set; }
        public string BatchType { get; set; }
        public int BatchTypeID { get; set; }
        public decimal Stock { get; set; }
        public decimal AverageSales { get; set; }
        public decimal Rate { get; set; }
        public decimal IssueQty { get; set; }
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
        public decimal SuggestedQty { get; set; }
        public string SalesCategory { get; set; }
        public string MalayalamName { get; set; }
        public string PrimaryUnit { get; set; }
        public int PackSize { get; set; }
        public string PartsNo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int PrimaryUnitID { get; set; }
    }
}