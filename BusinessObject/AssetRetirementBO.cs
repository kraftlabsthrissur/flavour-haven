using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace BusinessObject
{
    public class AssetRetirementBO : AssetBO
    {

        public DateTime? CapitalisationDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Date { get; set; }
        public decimal ClosingGrossBlockValue { get; set; }
        public decimal ClosingAccumulatedDepreciation { get; set; }
        public decimal ClosingWDV { get; set; }
        public decimal SaleQty { get; set; }
        public decimal SaleValue { get; set; }
        public decimal AssetQty { get; set; }

    }
    public class RetirementFilterBO
    {
        public string AssetCodeFrom { get; set; }
        public string AssetCodeTo { get; set; }
        public string AssetNameFrom { get; set; }
        public string AssetNameTo { get; set; }
        public string AssetName { get; set; }
        public DateTime? CapitalisationDateFrom { get; set; }
        public DateTime? CapitalisationDateTo { get; set; }

    }
}