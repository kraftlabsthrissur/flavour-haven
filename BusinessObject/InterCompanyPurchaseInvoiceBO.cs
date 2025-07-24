

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
namespace BusinessObject
{
    public class InterCompanyPurchaseInvoiceBO : PurchaseInvoiceBO
    {
        public decimal TurnoverDiscountAvailable { get; set; }
        public bool CashDiscountEnabled { get; set; }
        public decimal CashDiscountPercentage { get; set; }
        public decimal CashDiscount { get; set; }
        public decimal TurnoverDiscount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AdditionalDiscount { get; set; }
        public decimal TaxableAmount { get; set; }
        public Decimal RoundOff { get; set; }
        public int PaymentModeID { get; set; }
        public string SalesInvoiceNo { get; set; }
        public DateTime SalesInvoiceDate { get; set; }
        public int SalesInvoiceID { get; set; }
        public int ShippingAddressID { get; set; }
        public int BillingAddressID { get; set; }
        public decimal TradeDiscount { get; set; }
        public string PaymentMode { get; set; }
        public List<InterCompanyPurchaseInvoiceItemBO> Trans { get; set; }
        public List<SalesAmountBO> AmountDetails { get; set; }
    }
    public class InterCompanyPurchaseInvoiceItemBO : GRNTransItemBO
    {

        public decimal BasicPrice { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal AdditionalDiscount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal GSTAmount { get; set; }           
        public decimal DiscPercentage { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal TradeDiscPercentage { get; set; }
        public decimal TradeDiscAmount { get; set; }
        public decimal TurnoverDiscount { get; set; }
        public decimal CashDiscount { get; set; }     
        public int BatchID { get; set; }
        public int SalesInvoiceTransID { get; set; }
        public int SalesInvoiceID { get; set; }

    }

}
