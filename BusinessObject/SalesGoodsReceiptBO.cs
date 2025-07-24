using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SalesGoodsReceiptBO
    {
      
        public string Make { get; set; }
         public bool PrintWithItemCode { get; set; }
        public string Name { get; set; }
        public string PartsNumber { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public String ItemName { get; set; }
        public string Itemcode { get; set; }
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime TransDate { get; set; }
        public int SalesTypeID { get; set; }
        public string SalesTypeName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLocation { get; set; }
        public int CustomerID { get; set; }
        public int PriceListID { get; set; }
        public DateTime? DespatchDate { get; set; }
        public string Status { get; set; }
        public int CustomerCategoryID { get; set; }
        public string CustomerCategory { get; set; }
        public int PaymentModeID { get; set; }
        public int PaymentTypeID { get; set; }
        public int ItemCategoryID { get; set; }
        public int StateID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsProcessed { get; set; }
        public int SalesCategoryID { get; set; }
        public int SchemeID { get; set; }
        public string SalesOrderNos { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TurnoverDiscount { get; set; }
        public decimal AdditionalDiscount { get; set; }
        public bool CheckStock { get; set; }
        public int? NoOfBoxes { get; set; }
        public int? NoOfCans { get; set; }
        public int? NoOfBags { get; set; }
        public int BillingAddressID { get; set; }
        public int ShippingAddressID { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string CheckedBy { get; set; }
        public string PackedBy { get; set; }
        public decimal CessAmount { get; set; }
        public decimal FreightAmount { get; set; }
        public decimal OutStandingAmount { get; set; }
        public decimal MaxCreditLimit { get; set; }
        public string Remarks { get; set; }
        public DateTime? SONO { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public string FullOrLoose { get; set; }
        public string Location { get; set; }


    }
}
