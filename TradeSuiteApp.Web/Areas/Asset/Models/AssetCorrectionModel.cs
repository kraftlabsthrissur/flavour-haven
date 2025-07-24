using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradeSuiteApp.Web.Utils;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Asset.Models
{
    public class AssetCorrectionModel : AssetModel
    {
        public string CorrectionTransNumber { get; set; }
        public string CorrectionDateStr { get; set; }
        public DateTime? CorrectionDate
        {
            get
            {
                return CorrectionDateStr == "" || CorrectionDateStr == null ? (DateTime?)null : General.ToDateTime(CorrectionDateStr);
            }
            set
            {
                CorrectionDate = value == null ? (DateTime?)null : General.ToDateTime(CorrectionDateStr);

            }
        }
        public string DebitACName { get; set; }
        public string DebitACCode { get; set; }
        public int DebitACID { get; set; }
        public string CreditACName { get; set; }
        public string CreditACCode { get; set; }
        public int CreditACID { get; set; }
        public bool IsAdditionDuringYear { get; set; }
        public bool IsDepreciation { get; set; }
        public string AssetSubLedger { get; set; }
        public decimal AmountValue { get; set; }
        public SelectList AdditionDuringYearList { get; set; }
        public SelectList DepreciationList { get; set; }
    }
    public class AssetCorrectionFilterModel
    {
        public string AssetCodeFrom { get; set; }
        public string AssetCodeTo { get; set; }
        public string AssetNameFrom { get; set; }
        public string AssetNameTo { get; set; }
        public string AssetName { get; set; }
        public SelectList AssetNameRangeList { get; set; }

    }
}