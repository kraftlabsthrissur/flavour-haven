using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Asset.Models
{
    public class RetirementModel:AssetModel
    {
        public string RetirementTransNo { get; set; }
        public string DateStr { get; set; }
        public DateTime? Date
        {
            get
            {
                return DateStr == "" || DateStr == null ? (DateTime?)null : General.ToDateTime(DateStr);
            }
            set
            {
                Date = value == null ? (DateTime?)null : General.ToDateTime(DateStr);

            }

        }
        public string CapitalisationDateStr { get; set; }
        public DateTime? CapitalisationDate
        {
            get
            {
                return CapitalisationDateStr == "" || CapitalisationDateStr == null ? (DateTime?)null : General.ToDateTime(CapitalisationDateStr);
            }
            set
            {
                CapitalisationDate = value == null ? (DateTime?)null : General.ToDateTime(CapitalisationDateStr);

            }
            
        }
        public string EndDateStr { get; set; }
        public DateTime? EndDate
        {
            get
            {
                return EndDateStr == "" || EndDateStr == null ? (DateTime?)null : General.ToDateTime(EndDateStr);
            }
            set { EndDate = value == null ? (DateTime?)null : General.ToDateTime(EndDateStr); }
        }
        public decimal ClosingGrossBlockValue { get; set; }
        public decimal ClosingAccumulatedDepreciation { get; set; }
        public decimal ClosingWDV { get; set; }
        public decimal SaleQty { get; set; }
        public decimal SaleValue { get; set; }          
        public decimal AssetQty { get; set; }
    }
    public class RetirementFilterModel
    {
        public string AssetCodeFrom { get; set; }
        public string AssetCodeTo { get; set; }
        public string AssetNameFrom { get; set; }
        public string AssetNameTo { get; set; }
        public string AssetName { get; set; }
        public SelectList AssetNameRangeList { get; set; }
        public string CapitalisationDateFromStr { get; set; }
        public string CapitalisationDateToStr { get; set; }
        public DateTime? CapitalisationDateFrom
        {
            get
            {           
                return CapitalisationDateFromStr == "" ? (DateTime?)null : General.ToDateTime(CapitalisationDateFromStr);
            }
            set { CapitalisationDateFrom = value == null ? (DateTime?)null : General.ToDateTime(CapitalisationDateFromStr); }
        }
        public DateTime? CapitalisationDateTo
        {
            get
            {
                return CapitalisationDateToStr == "" ? (DateTime?)null : General.ToDateTime(CapitalisationDateToStr);
            }
            set { CapitalisationDateTo = value == null ? (DateTime?)null : General.ToDateTime(CapitalisationDateToStr); }
        }

    }
}