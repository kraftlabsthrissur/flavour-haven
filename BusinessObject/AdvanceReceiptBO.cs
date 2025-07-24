using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class AdvanceReceiptBO
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public string BankName { get; set; }
        public int BankID { get; set; }
        public decimal? NetAmount { get; set; }
        public string CustomerName { get; set; }
        public int CustomerID { get; set; }
        public int StateID { get; set; }
        public int PriceListID { get; set; }
        public int SchemeID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public string AdvanceReceiptNo { get; set; }
        public DateTime AdvanceReceiptDate { get; set; }
        public string ReferenceNo { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int LocationStateID { get; set; }
        public int BatchTypeID { get; set; }
        public decimal Amount { get; set; }
        public Decimal TDSAmount { get; set; }
        public string TDSCode { get; set; }
        public string TransNo { get; set; }
        public string Remarks { get; set; }
        public DateTime SalesOrderDate { get; set; }
        public string TDSValue { get; set; }



        List<AdvanceReceiptItemBO> ReceiptItems { get; set; }
    }

    public class AdvanceReceiptItemBO
    {
        public int ID { get; set; }
        public string AdvancePaymentNo { get; set; }
        public DateTime SalesOrderDate { get; set; }
        public string ItemName { get; set; }
        public string TDSCode { get; set; }
        public string Remarks { get; set; }
        public string TransNo { get; set; }
        public int ItemID { get; set; }
        public int TDSID { get; set; }
        public Decimal TDSAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal ItemAmount { get; set; }

    }
}
