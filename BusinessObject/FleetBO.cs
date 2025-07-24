using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class FleetBO
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
        public DateTime PurchaseDate { get; set; }
        public DateTime PermitExpairyDate { get; set; }
        public DateTime TaxExpairyDate { get; set; }
        public DateTime TestExpairyDate { get; set; }
        public DateTime InsuranceExpairyDate { get; set; }
        public int? BagCapacity { get; set; }
        public int? BoxCapacity { get; set; }
        public int? CanCapacity { get; set; }
        public int CreatetedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ApplicationID { get; set; }
        public int LocationID { get; set; }
    }
}
