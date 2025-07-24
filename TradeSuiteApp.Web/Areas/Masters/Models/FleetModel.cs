using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class FleetModel
    {
        public int ID { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleName { get; set; }
        public string DriverName { get; set; }
        public string LicenseNo { get; set; }
        public string PolicyNo { get; set; }
        public string OwnerName { get; set; }
        public string TravellingAgency { get; set; }
        public string InsuranceCompany { get; set; }
        public string OtherDetails { get; set; }
        public string PurchaseDate { get; set; }
        public string PermitExpairyDate { get; set; }
        public string TaxExpairyDate { get; set; }
        public string TestExpairyDate { get; set; }
        public string InsuranceExpairyDate { get; set; }
        public int? BagCapacity { get; set; }
        public int? BoxCapacity { get; set; }
        public int? CanCapacity { get; set; }
        public int CreatetedUserID { get; set; }
        public string CreatedDate { get; set; }
        public int ApplicationID { get; set; }
        public int LocationID { get; set; }
    }
}