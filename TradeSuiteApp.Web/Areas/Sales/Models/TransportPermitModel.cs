using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Sales.Models
{
    public class TransportPermitModel
    {
        public string DriverName { get; set; }
        public string VehicleNumber { get; set; }
        public int SalesInvoiceNoFromID { get; set; }
        public int StockTransferNoFromID { get; set; }
        public int StockTransferNoToID { get; set; }
        public int SalesInvoiceNoToID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }       
        public string Type { get; set; }
        public string ValidFromDate { get; set; }
        public string ValidToDate { get; set; }
        public string TransNo { get; set; }
        public int ID { get; set; }
        public List<TransportPermitItemModel> Items { get; set; }
    }
    public class TransportPermitItemModel
    {
        public string TransNo { get; set; }
        public int TransID { get; set; }
        public string TransDate { get; set; }
        public string CustomerName { get; set; }
        public string LocationName { get; set; }
        public string District { get; set; }
        public string Type { get; set; }
        public int CustomerID { get; set; }
        public int LocationID { get; set; }
        public int DistrictID { get; set; }
        public int ItemID { get; set; }
        public int BatchTypeID { get; set; }
        public decimal Quantity { get; set; }
    }
}