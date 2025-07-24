using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Sales.Models
{
    public class GatePassModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string TransDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Salesman { get; set; }
        public string VehicleNo { get; set; }
        public int VehicleNoID { get; set; }
        public string DespatchDateTime { get; set; }
        public string DriverName { get; set; }
        public int DriverID { get; set; }
        public string DrivingLicense { get; set; }
        public string VehicleOwner { get; set; }
        public string TransportingAgency { get; set; }
        public string HelperName { get; set; }
        public string Area { get; set; }
        public decimal StartingKilometer { get; set; }
        public string IssuedBy { get; set; }
        public int BagCount { get; set; }
        public int CanCount { get; set; }
        public int BoxCount { get; set; }
        public string Time { get; set; }
        public bool IsDraft { get; set; }

        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string InvoiceArea { get; set; }
        public int PPSNO { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public bool IsCancelled { get; set; }
        public string DeliveryDate { get; set; }
        public SelectList TypeList { get; set; }
        public string Type { get; set; }
        public  List<SalesInvoiceModel> invoiceitems { get; set; }
        public SelectList DDLDriver { get; set; }
        public SelectList DDLVehicleNo { get; set; }

        public List<GatePassItems> GatepassItems { get; set; }
    }
    public class GatePassItems
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string TransDate { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Area { get; set; }
        public string Type { get; set; }
        public string DeliveryDate { get; set; }
        public int GatePassTransID { get; set; }
        public int NoOfboxes { get; set; }
        public int NoOfCans { get; set; }
        public int NoOfBags { get; set; }
    }
    
}