using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class MachineModel
    {
        public int ID { get; set; }
        public  string MachineName { get; set; }
        public string MachineCode { get; set; }
        public string InsulationDate { get; set; }
        public decimal AverageCostPerHour { get; set; }
        public decimal  PowerConsumptionPerHour { get; set; }
        public string Manufacturer { get; set; }
        public int MaintenancePeriod { get; set; }
        public string Type{ get; set; }
        public int TypeID { get; set; }
        public int ProcessID { get; set; }
        public string Process { get; set; }
        public string Motor { get; set; }
        public int HelperCount { get; set; }
        public int OperatorCount { get; set; }
        public string MachineNumber { get; set; }
        public string SoftwareVersion { get; set; }
        public int LocationID { get; set; }
        public string Model { get; set; }
        public int? LoadedMouldID { get; set; }
        public string Location { get; set; }

        public SelectList MachineType { get; set; }
        public SelectList ProcessTypeList { get; set; }
        public SelectList LocationList { get; set; }
    }
}