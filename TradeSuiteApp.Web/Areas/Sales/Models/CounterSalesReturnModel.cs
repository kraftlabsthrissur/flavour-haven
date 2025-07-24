using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Sales.Models
{
    public class CounterSalesReturnModel
    {
        public int ID { get; set; }
        public int PaymentModeID { get; set; }
        public string ReturnNo { get; set; }
        public string ReturnDate { get; set; }
        public string PartyName { get; set; }
        public int PartyID { get; set; }
        public string Status { get; set; }
        public decimal NetAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public bool IsDraft { get; set; }
        public List<CounterSalesReturnItemModel> Items { get; set; }
        public List<CounterSalesModel> ReturnList { get; set; }
        public SelectList PaymentModeList { get; set; }
        public SelectList BankList { get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }
        public string PaymentMode { get; set; }
        public int InvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public string Reason { get; set; }
        public decimal BillDiscount { get; set; }
        public string normalclass { get; set; }
        public decimal VATAmount { get; set; }
    }

    public class CounterSalesReturnItemModel : CountrSalesItemsModel
    {
        public decimal ReturnQty { get; set; }
        public int CounterSalesTransID { get; set; }
        public SelectList UnitList { get; set; }
        public int UnitID { get; set; }
        public int PrimaryUnitID { get; set; }
        public int CounterSalesTransUnitID { get; set; }
        public decimal ConvertedQuantity { get; set; }
        public decimal CounterSalesQty { get; set; }
    }
}