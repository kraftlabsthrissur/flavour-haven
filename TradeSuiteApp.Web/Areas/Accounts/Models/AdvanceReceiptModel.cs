using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class AdvanceReceiptModel
    {
        public int ID { get; set; }
        public SelectList BankList { get; set; }
        public SelectList PaymentTypeList { get; set; }
       
        public int PaymentTypeID { get; set; }
        public SelectList CustomerCategory { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string PaymentTypeName { get; set; }
        public string BankName { get; set; }
        public int BankID { get; set; }
        public decimal? NetAmount { get; set; }
        public string CustomerName { get; set; }
        public int PriceListID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public int StateID { get; set; }
        public int SchemeID { get; set; }
        public int CustomerID { get; set; }
        public string AdvanceReceiptNo { get; set; }
        public string AdvanceReceiptDate { get; set; }
        public string ReferenceNo { get; set; }
        public int LocationStateID { get; set; }
        public int BatchTypeID { get; set; }
        //public string SalesOrderDate { get; set; }
        public string ItemName { get; set; }
        public decimal Amount { get; set; }
        public string TDSCode { get; set; }
        public int TDSID { get; set; }
        public Decimal TDSAmount { get; set; }
        public string Remarks { get; set; }
        public string TransNo { get; set; }
        public int ItemID { get; set; }
        public string Status { get; set; }
        public SelectList TDSList { get; set; }
        public string Rate { get; set; }
       


        public List<AdvanceReceiptItem> Items { get; set; }

    }
    public class AdvanceReceiptItem
    {
        public int ID { get; set; }
        public string AdvancePaymentNo { get; set; }
        public string SalesOrderDate { get; set; }
        public string ItemName { get; set; }
        public decimal Amount { get; set; }
        public string TDSCode { get; set; }
        public Decimal TDSAmount { get; set; }
        public string Remarks { get; set; }
        public string TransNo { get; set; }
        public int ItemID { get; set; }
        public decimal NetAmount { get; set; }
        public int TDSID { get; set; }
        public string Rate { get; set; }
        public SelectList TDSList { get; set; }
        public string Description { get; set; }
        public decimal ItemAmount { get; set; }

    }



}