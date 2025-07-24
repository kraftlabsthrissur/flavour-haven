using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PackingProcessBO
    {
        public int PackingIssueID { get; set; }
        public int BatchTypeID { get; set; }
        public string Stage { get; set; }
        public string ProcessName { get; set; }
        public DateTime? StartTime { get; set; }
        public int InputQuantity { get; set; }
        public DateTime? EndTime { get; set; }
        public int OutputQty { get; set; }
        public decimal SkilledLaboursStandard { get; set; }
        public decimal UnSkilledLabourStandard { get; set; }
        public decimal MachineHoursStandard { get; set; }
        public decimal BatchSize { get; set; }
        public decimal SkilledLaboursActual { get; set; }
        public decimal UnSkilledLabourActual { get; set; }
        public decimal MachineHoursActual { get; set; }

        public String DoneBy { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public int PackingProcessDefinitionTransID { get; set; }


    }
}
