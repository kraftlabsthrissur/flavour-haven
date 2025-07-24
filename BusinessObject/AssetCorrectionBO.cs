using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AssetCorrectionBO : AssetBO
    {
        public string CorrectionTransNumber { get; set; }
        public DateTime? CorrectionDate { get; set; }
        public string DebitACName { get; set; }
        public string DebitACCode { get; set; }
        public string CreditACName { get; set; }
        public string CreditACCode { get; set; }
        public int DebitACID { get; set; }
        public int CreditACID { get; set; }
        public bool IsAdditionDuringYear { get; set; }
        public bool IsDepreciation { get; set; }
        public string AssetSubLedger { get; set; }
        public decimal AmountValue { get; set; }
    }
    public class AssetCorrectionFilterBO
    {
        public string AssetCodeFrom { get; set; }
        public string AssetCodeTo { get; set; }
        public string AssetNameFrom { get; set; }
        public string AssetNameTo { get; set; }
        public string AssetName { get; set; }


    }
}