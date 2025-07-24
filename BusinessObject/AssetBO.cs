using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AssetBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime? TransDate { get; set; }
        public string ItemName { get; set; }
        public string SupplierName { get; set; }
        public string AssetName { get; set; }
        public string AssetUniqueNo { get; set; }
        public string AssetCode { get; set; }
        public string ItemAccountCategory { get; set; }
        public bool? IsRepairable { get; set; }
        public decimal? CompanyDepreciationRate { get; set; }
        public decimal? IncomeTaxDepreciationRate { get; set; }
        public decimal LifeInYears { get; set; }
        public decimal AssetValue { get; set; }
        public string AdditionToAssetNo { get; set; }
        public DateTime? StatusChangeDate { get; set; }
        public decimal? ResidualValue { get; set; }
        public DateTime? DepreciationStartDate { get; set; }
        public DateTime? DepreciationEndDate { get; set; }
        public string AdditionToAssetNumber { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Employee { get; set; }
        public string Project { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCapital { get; set; }
        public decimal Qty { get; set; }

    }
    public class AssetFilterBO
    {
        public int ID { get; set; }
        public string TransNoFrom { get; set; }
        public int? TransNoFromID { get; set; }
        public string TransNoTo { get; set; }
        public int? TransNoToID { get; set; }
        public DateTime TransDateFrom { get; set; }
        public DateTime TransDateTo { get; set; }
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
        public int? AssetNameID { get; set; }
        public int AccountCategoryID { get; set; }
        public List<AssetBO> PendingList { get; set; }
        public List<AssetBO> CapitalList { get; set; }
        public List<AssetBO> RevenueList { get; set; }
    }
}
