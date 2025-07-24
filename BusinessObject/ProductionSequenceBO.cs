using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ProductionSequenceBO
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public int ProductionSequence { get; set; }
        public string ProcessStage { get; set; }
        public decimal BatchSize { get; set; }
        public decimal StandardOutput { get; set; }
        public bool IsQCRequired { get; set; }
        public string Unit { get; set; }
    }
}
