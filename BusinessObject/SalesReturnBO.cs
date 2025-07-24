using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SalesReturnBO
    {
        public int ID { get; set; }
        public string SRNo { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime SRDate { get; set; }
        public string CustomerName { get; set; }
        public int CustomerID { get; set; }
        public int PriceListID { get; set; }
        public string Status { get; set; }
        public int CustomerCategoryID { get; set; }
        public string CustomerCategory { get; set; }
        public int PaymentModeID { get; set; }
        public int ItemCategoryID { get; set; }
        public int StateID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCancelled { get; set; }
        public Nullable<DateTime> CancelledDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsProcessed { get; set; }
        public int SalesCategoryID { get; set; }
        public int SchemeAllocationID { get; set; }
        public bool IsNewInvoice { get; set; }

        public decimal GrossAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }

        public int LogicCodeID { get; set; }
        public string LogicCode { get; set; }
        public string LogicName { get; set; }
        public int SalesInvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public string AmountInWords { get; set; }
        public int DecimalPlaces { get; set; }
    }
}
