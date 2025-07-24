using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
  public  class MouldBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public DateTime InceptionDate { get; set; }
        public string MouldName { get; set; }
        public DateTime ExpairyDate { get; set; }
        public int MandatoryMaintenanceTime { get; set; }
        public string ManufacturedBy { get; set; }
        public int CurrentLocationID { get; set; }
        public string CurrentLocationName { get; set; }
    }
    public class MouldItemBO
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

    public class MouldMachinesBO
    {
        public int ID { get; set; }
        public string Machine { get; set; }
        public string check { get; set; }
    }
}
