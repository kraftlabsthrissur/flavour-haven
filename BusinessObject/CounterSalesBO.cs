using System;
using System.Collections.Generic;

namespace BusinessObject
{
    //Counter Sales Bussiness Oject Class   created on 6/29/2018
    public class CounterSalesBO
    {
        public long ID { get; set; }
        public int Counts { get; set; }
        public string TransNo { get; set; }
        public DateTime TransDate { get; set; }
        public int PatientID { get; set; }
        public string PartyName { get; set; }
        public string ContactName { get; set; }
        
        public string CivilID { get; set; }
        public string MobileNumber { get; set; }
        public int CustomerID { get; set; }
        public int ContactID { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public int WarehouseID { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal DiscountPercentage { get; set; }
        public bool PrintWithItemCode { get; set; }
        public decimal TaxableAmt { get; set; }
        public bool IsDraft { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal TotalVATAmount { get; set; }
        public int IsVAT { get; set; }
        public int IsGST { get; set; }
        public int CurrencyID { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string WarehouseName { get; set; }
        public decimal PackingPrice { get; set; }
        public string Remarks { get; set; }
        public List<CounterSalesItemsBO> Items { get; set; }
        public List<CounterSalesAmountDetailsBO> CounterSalesAmountDetails { get; set; }
        public int PaymentModeID { get; set; }
        public decimal TotalAmountReceived { get; set; }
        public decimal BalanceToBePaid { get; set; }
        public decimal CessAmount { get; set; }
        public string Type { get; set; }
        public int TypeID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string PatientName { get; set; }
        public int BankID { get; set; }
        public string CashSalesName { get; set; }
        public decimal OutstandingAmount { get; set; }
        public int DiscountCategoryID { get; set; }
        public string DiscountCategory { get; set; }

        public int Count { get; set; }
        public string StartingBillNo { get; set; }
        public string EndingBillNo { get; set; }
        public decimal TotalCash { get; set; }
        public decimal CashOnCard { get; set; }
        public int BusinessCategoryID { get; set; }
        public string currencyCode { get; set; }
        public string AmountInWords { get; set; }
        public string MinimumCurrency { get; set; }
        public decimal AmountRecieveds { get; set; }
        public string ReferenceNo { get; set; }
        public int VATPercentageID { get; set; }
        public decimal VATPercentage { get; set; }
        public int DecimalPlaces { get; set; }

    }
    //Counter Sales Item Trans Bussiness Objet class created on 6/29/2018
    public class CounterSalesItemsBO : ItemBO
    {

        public int ID { get; set; }
        public int CounterSalesID { get; set; }
        public string TransNo { get; set; }
        public string FullOrLoose { get; set; }
        public int BatchID { get; set; }
        public decimal Quantity { get; set; }
        public decimal MRP { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal VATAmount { get; set; }
        public int IsVAT { get; set; }
        public int IsGST { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryReturnQty { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public decimal SecondaryRate { get; set; }
        public decimal SecondaryQty { get; set; }
        public decimal SecondaryOfferQty { get; set; }
        public int CurrencyID { get; set; }
        public decimal VATPercntage { get; set; }
        public decimal NetAmount { get; set; }
        public string Batch { get; set; }
        public int WarehouseID { get; set; }
        public decimal ConvertedQuantity { get; set; }
        public decimal CounterSalesQty { get; set; }
        public decimal CessAmount { get; set; }
        public decimal CessPercentage { get; set; }
        public decimal BasicPrice { get; set; }
        public decimal MinimumSalesQty { get; set; }
        public decimal MaximumSalesQty { get; set; }
        public decimal TotalMRP { get; set; }
        public bool IsGSTRegisteredLocation { get; set; }
        public decimal VATPercentage { get; set; }
        public string CurrencyName { get; set; }
        public string TaxType { get; set; }
        public bool PrintWithItemName { get; set; }

        public int DecimalPlaces { get; set; }

    }
    //Counter Sales Amount  Bussiness Objet class created on 6/29/2018
    public class CounterSalesAmountDetailsBO
    {
        public string Particulars { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
    }
    public class CurrencyClassBO
    {
        public string normalclass { get; set; }
        public string largeclass { get; set; }
        public int DecimalPlaces { get; set; }
    }

}

