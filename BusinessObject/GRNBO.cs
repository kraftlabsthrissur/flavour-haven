using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class GRNBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string CountryName { get; set; }
        public string CurrencyName { get; set; }
        public string BAddressLine1 { get; set; }
        public string BAddressLine2 { get; set; }
        public string BAddressLine3 { get; set; }
        public int? LocationID { get; set; }    //Check spGetUnProcessedGRN_Result
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public DateTime? PurchaseOrderDate { get; set; }
        public string ReceiptStore { get; set; }
        public string WarehouseName { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string DeliveryChallanNo { get; set; }
        public DateTime? DeliveryChallanDate { get; set; }
        public int WarehouseID { get; set; }
        public bool PurchaseCompleted { get; set; }
        public bool IsCancelled { get; set; }
        public Nullable<DateTime> CancelledDate { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int FinYear { get; set; }
        public int ApplicationID { get; set; }
        public string SupplierCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public bool IsDraft { get; set; }

        public decimal SGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }

        public int BatchID { get; set; }
        public int ItemID { get; set; }
        public string QRCode { get; set; }
        public string currencycode { get; set; }
        public bool IsGSTRegistered { get; set; }
        public int StateID { get; set; }

        public bool IsCheckedDirectInvoice { get; set; }
        public decimal LocalLandinngCost { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public string PurchaseOrderNo { get; set; }
        public decimal VATAmount { get; set; }
        public decimal SuppDocAmount { get; set; }
        public decimal SuppShipAmount { get; set; }
        public decimal PackingForwarding { get; set; }
        public string Remarks { get; set; }
        public decimal SuppOtherCharges { get; set; }
        public decimal SuppFreight { get; set; }
        public decimal LocalCustomsDuty { get; set; }
        public decimal LocalMiscCharge { get;set; }
        public decimal LocalOtherCharges { get; set; }
        public decimal LocalFreight { get; set; }
        public decimal LandingCost { get; set; }
        public decimal SuuplierCurrencyconverion {  get; set; }
        public string CurrencyCodeL { get; set; }
    }
    
}
