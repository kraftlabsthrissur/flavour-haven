using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CounterSalesReturnBO
    {

        public int ID { get; set; }
        public string ReturnNo { get; set; }
        public DateTime ReturnDate { get; set; }
        public string PartyName { get; set; }
        public int PartyID { get; set; }
        public bool IsDraft { get; set; }
        public decimal NetAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public List<CounterSalesReturnItemBO> Items { get; set; }
        public List<CounterSalesBO> ReturnList { get; set; }
        public int BankID { get; set; }
        public int PaymentModeID { get; set; }
        public string BankName { get; set; }
        public string PaymentMode { get; set; }
        public int InvoiceID { get; set; }
        public string InvoiceNo { get; set; }

        public string Reason { get; set; }
        public decimal BillDiscount { get; set; }
        public decimal VATAmount { get; set; }
        public string AmountInWords { get; set; }
        public int DecimalPlaces { get; set; }
        public string currencyCode { get; set; }
    }

    public class CounterSalesReturnItemBO : CounterSalesItemsBO
    {
        public decimal ReturnQty { get; set; }        
        public int SalesTransID { get; set; }
        public int PrimaryUnitID { get; set; }
        public int CounterSalesTransID { get; set; }
    }
}