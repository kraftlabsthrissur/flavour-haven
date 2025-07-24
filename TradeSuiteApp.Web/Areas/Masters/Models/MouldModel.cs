using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class MouldModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Date { get; set; }
        public string InceptionDate { get; set; }
        public string MouldName { get; set; }
        public string ExpairyDate { get; set;}
        public int MandatoryMaintenanceTime { get; set; }
        public string ManufacturedBy { get; set; }
        public List<MouldItemModel> Items { get; set; }
        public List<MouldMachinesModel> Machines { get; set; }
        public SelectList LocationList { get; set; }
        public int CurrentLocationID { get; set; }
        public string CurrentLocationName { get; set; }
    }
    public class MouldItemModel
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int NoOfCavity { get; set; }
        public int StdTime { get; set; }
        public decimal StdWeight { get; set; }
        public decimal StdRunningWaste { get; set; }
        public decimal StdShootingWaste { get; set; }
        public decimal StdGrindingWaste { get; set; }
    }
    public class MouldMachinesModel
    {
        public int ID { get; set; }
        public string Machine { get; set; }
        public string check { get; set; }
    }
}