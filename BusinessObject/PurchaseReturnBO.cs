using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PurchaseReturnBO
    {
        public int Id { get; set; }
        public string ReturnNo { get; set; }
        public string ReturnDateStr { get; set; }
        public DateTime ReturnDate { get; set; }
        public List<SupplierBO> SupplierList { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public decimal Freight { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal PackingCharges { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        public bool IsDraft { get; set; }
        public string GRNno { get; set; }
        public string ItemName { get; set; }
        public decimal ReturnQty { get; set; }
        public string SaveType { get; set; }
        public string PremisesName { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public bool IsProcessed { get; set; }
        public int StateID { get; set; }
        public bool IsGSTRegistred { get; set; }
        public decimal Discount { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public int CurrencyID { get; set; }
        public int IsVat { get; set; }
        public int IsGST { get; set; }
        public string GSTNo { get; set; }
        public string Addresses1 { get; set; }
        public string Addresses2 { get; set; }
        public string State { get; set; }
        public List<SupplierBO> Suppliers { get; set; }
        public List<PurchaseReturnTransItemBO> PurchaseReturnTrnasItemBOList { get; set; }
    }


    public class PurchaseReturnTransItemBO
    {
        public int PurchaseReturnID { get; set; }
        public int GRNID { get; set; }
        public int ItemID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }
        public int BatchTypeID { get; set; }
        public int UnitID { get; set; }
        public int PurchaseReturnTransID { get; set; }
        public string ReturnNo { get; set; }
        public decimal Stock { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal AcceptedQty { get; set; }
        public int InvoiceID { get; set; }
        public int WarehouseID { get; set; }
        public int PurchaseReturnOrderTransID { get; set; }
        public int PurchaseReturnOrderID { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal GRNQty { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryRate { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public decimal SecondaryReturnQty { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
    }
}
