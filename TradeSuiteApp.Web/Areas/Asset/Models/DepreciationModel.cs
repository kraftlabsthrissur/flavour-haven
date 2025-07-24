using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Asset.Models
{
    public class DepreciationModel : AssetModel
    {
        public decimal GrossBlockValue { get; set; }
        public decimal OpeningWDV { get; set; }
        public decimal DepreciationValue { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal WDVValue { get; set; }
        public string Remarks { get; set; }
        public decimal DepreciationPrecentage { get; set; }
        public string DepreciationType { get; set; }
        public string DateStr { get; set; }        
        public DateTime? Date
        {
            get
            {
                return DateStr == "" ? (DateTime?)null : General.ToDateTime(DateStr);
            }
            set { Date = value == null ? (DateTime?)null : General.ToDateTime(DateStr); }
        }
        public SelectList DepreciationTypeList { get; set; }
    }
    public class DepreciationFilterModel
    {
        public string TransDateFromStr { get; set; }
        public DateTime? TransDateFrom
        {
            get
            {
                return TransDateFromStr == "" ? (DateTime?)null : General.ToDateTime(TransDateFromStr);
            }
            set { TransDateFrom = value == null ? (DateTime?)null : General.ToDateTime(TransDateFromStr); }
        }
        public DateTime? TransDateTo
        {
            get
            {
                return TransDateToStr == "" ? (DateTime?)null : General.ToDateTime(TransDateToStr);
            }
            set { TransDateTo = value == null ? (DateTime?)null : General.ToDateTime(TransDateToStr); }
        }
        public string TransDateToStr { get; set; }
        public decimal FromCompanyDepreciationRate { get; set; }
        public decimal ToCompanyDepreciationRate { get; set; }
        public decimal FromIncomeTaxDepreciationRate { get; set; }
        public decimal ToIncomeTaxDepreciationRate { get; set; }

    }
}