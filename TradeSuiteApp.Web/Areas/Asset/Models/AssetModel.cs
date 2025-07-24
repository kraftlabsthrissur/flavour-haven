using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;


namespace TradeSuiteApp.Web.Areas.Asset.Models
{
    public class AssetModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string TransDateStr { get; set; }
        public DateTime? TransDate
        {
            get
            {
                return TransDateStr == "" || TransDateStr == null ? (DateTime?)null : General.ToDateTime(TransDateStr);
            }
            set
            {
                TransDate = value == null ? (DateTime?)null : General.ToDateTime(TransDateStr);

            }
        }
        public string ItemName { get; set; }
        public string SupplierName { get; set; }
        public string AssetName { get; set; }
        public string AssetUniqueNo { get; set; }
        public string ItemAccountCategory { get; set; }
        public bool? IsRepairable { get; set; }
        public decimal? CompanyDepreciationRate { get; set; }
        public decimal? IncomeTaxDepreciationRate { get; set; }
        public decimal AssetValue { get; set; }
        public string AdditionToAssetNo { get; set; }
        public string StatusChangeDateStr { get; set; }
        public DateTime? StatusChangeDate
        {
            get
            {
                return StatusChangeDateStr == "" || StatusChangeDateStr == null ? (DateTime?)null : General.ToDateTime(StatusChangeDateStr);
            }
            set { StatusChangeDate = value == null ? (DateTime?)null : General.ToDateTime(StatusChangeDateStr); }
        }
        public decimal? ResidualValue { get; set; }
        public string DepreciationStartDateStr { get; set; }
        public DateTime? DepreciationStartDate
        {
            get
            {
                return DepreciationStartDateStr == "" || DepreciationStartDateStr == null ? (DateTime?)null : General.ToDateTime(DepreciationStartDateStr);
            }
            set { DepreciationStartDate = value == null ? (DateTime?)null : General.ToDateTime(DepreciationStartDateStr); }
        }
        public string DepreciationEndDateStr { get; set; }
        public DateTime? DepreciationEndDate
        {
            get
            {
                return DepreciationEndDateStr == "" || DepreciationEndDateStr == null ? (DateTime?)null : General.ToDateTime(DepreciationEndDateStr);

            }
            set { DepreciationEndDate = value == null ? (DateTime?)null : General.ToDateTime(DepreciationEndDateStr); }
        }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Project { get; set; }
        public string Employee { get; set; }
        public string Remark { get; set; }
        public string AssetStatus { get; set; }
        public string Status { get; set; }
        public string AssetCode { get; set; }
        public decimal LifeInYears { get; set; }
        public string NewItemName { get; set; }
        public string FinancialAssetName { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCapital { get; set; }
        public decimal Qty { get; set; }
        public SelectList DepartmentList { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList EmployeeList { get; set; }
        public SelectList ProjectList { get; set; }
        public SelectList StatusList { get; set; }
    }
    public class AssetFilterModel
    {
        public int ID { get; set; }
        public string TransNoFrom { get; set; }
        public int? TransNoFromID { get; set; }
        public string TransNoTo { get; set; }
        public int? TransNoToID { get; set; }
        public string TransDateFromStr { get; set; }
        public DateTime? TransDateFrom
        {
            get
            {
                //   return TransDateFromStr == "" || TransDateFromStr == null ? (DateTime?)null : General.ToDateTime(TransDateFromStr);
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
        public string AssetNameFrom { get; set; }
        public string AssetNameTo { get; set; }
        public string AccountCategoryFrom { get; set; }
        public string AccountCategoryTo { get; set; }
        public string SupplierNameFrom { get; set; }
        public string SupplierNameTo { get; set; }
        public string ReceiptNoFrom { get; set; }
        public string ReceiptNoTo { get; set; }
        public int? ReceiptNoFromID { get; set; }
        public int? ReceiptNoToID { get; set; }
        public string AssetName { get; set; }
        public string SupplierName { get; set; }
        public decimal AssetValue { get; set; }
        public int? SupplierID { get; set; }
        public int AssetNameID { get; set; }
        public int AccountCategoryID { get; set; }
        public SelectList TransNoRangeList { get; set; }
        public SelectList AssetNameRangeList { get; set; }
        public SelectList AccountCategoryRangeList { get; set; }
        public SelectList AccountCategoryList { get; set; }
        public SelectList SupplierNameRangeList { get; set; }
        public List<AssetModel> PendingList { get; set; }
        public List<AssetModel> CapitalList { get; set; }
        public List<AssetModel> RevenueList { get; set; }

    }
}