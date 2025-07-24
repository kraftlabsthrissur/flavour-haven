using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class LaboratoryTestResultBO
    {
        public int PatientLabTestsID { get; set; }
        public int PatientLabTestTransID { get; set; }
        public int ItemID { get; set; }
        public string ObservedValue { get; set; }
        public int BillablesID { get; set; }
        public string BiologicalReference { get; set; }
        public string Unit { get; set; }
        public string Item { get; set; }
        public string Status { get; set; }
        public bool IsProcessed { get; set; }
        public string NormalHighLevel { get; set; }
        public string NormalLowLevel { get; set; }
        public int DocumentID { get; set; }
        public int SpecimenID { get; set; }
        public string Specimen { get; set; }
        public string CollectedTime { get; set; }
        public string ReportedTime { get; set; }
        public DateTime CollectedDate { get; set; }
        public DateTime ReportedDate { get; set; }
        public string Type { get; set; }
        public string Method { get; set; }
        public string Test { get; set; }
        public DateTime CollectTime { get; set; }
        public DateTime ReportTime { get; set; }
    }
}
