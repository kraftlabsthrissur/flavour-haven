using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DepreciationBO : AssetBO
    {

        public decimal GrossBlockValue { get; set; }
        public decimal OpeningWDV { get; set; }
        public decimal DepreciationValue { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal WDVValue { get; set; }
        public string Remarks { get; set; }
        public decimal DepreciationRate { get; set; }
        public string DepreciationType { get; set; }
    }
    public class DepreciationFilterBO
    {

        public DateTime? TransDateFrom { get; set; }
        public DateTime? TransDateTo { get; set; }
        public decimal FromCompanyDepreciationRate { get; set; }
        public decimal ToCompanyDepreciationRate { get; set; }
        public decimal FromIncomeTaxDepreciationRate { get; set; }
        public decimal ToIncomeTaxDepreciationRate { get; set; }

    }
}