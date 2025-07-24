using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessObject
{
    public class GatePassBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Salesman { get; set; }
        public string VehicleNo { get; set; }
        public int VehicleNoID { get; set; }
        public DateTime? DespatchDateTime { get; set; }
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
        public bool IsCancelled { get; set; }
        public string Type { get; set; }

        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string InvoiceArea { get; set; }
        public int PPSNO { get; set; }
        public decimal TotalAmount { get; set; }
        public string DeliveryDate { get; set; }
    }
    public class GatePassItemsBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime TransDate { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Area { get; set; }
        public string Type { get; set; }
        public int PPSNO { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int GatePassTransID { get; set; }
        public int NoOfboxes { get; set; }
        public int NoOfCans { get; set; }
        public int NoOfBags { get; set; }
    }

}
