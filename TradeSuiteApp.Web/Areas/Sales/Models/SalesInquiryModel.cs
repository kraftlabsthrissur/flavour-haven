using BusinessObject;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Razor.Text;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Sales.Models
{
    public class SalesInquiryModel
    {
        public int ID { get; set; }
        public string SalesInquiryNo { get; set; }
        public string SalesInquiryDate { get; set; }
        public string RequestedCustomerName { get; set; }
        public string RequestedDelivaryDate { get; set; }
        public string RequestExpiryDate { get; set; }
        public string Status { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        public List<SalesInquiryItemModel> Items { get; set; }
        public int UnitID { get; set; }
        public SelectList UnitList { get; set; }
        public string RequestedCustomerAddress { get; set; }
        public string Remarks { get; set; }
        public int DecimalPlaces { get; set; }
        public string normalclass { get; set; }
        public string PhoneNo1 { get; set; }
        public string PhoneNo2 { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string SIOrVINNumber { get; set; }
        public bool IsDraft { get; set; }

    }
    public class SalesInquiryItemModel
    {
        public int SalesInquiryItemID { get; set; }
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public int UnitID { get; set; }
        public string PartsNumber { get; set; }
        public string DeliveryTerm { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string SIOrVINNumber { get; set; }
        public string Remarks { get; set; }

        public decimal CGSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
        public decimal Rate { get; set; }
        public decimal Qty { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public decimal SGST { get; set; }
        public decimal NetAmount { get; set; }
        //public decimal GSTAmount
        //{
        //    get
        //    {
        //        return CGST + SGST + IGST;
        //    }

        //    set
        //    {
        //        GSTAmount = value;
        //    }
        //}

    }

}