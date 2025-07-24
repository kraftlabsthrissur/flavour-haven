using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using TradeSuiteApp.Web.Areas.Masters.Models;


namespace TradeSuiteApp.Web.Areas.Manufacturing.Models
{
    public class QCViewModel
    {
    }
    public class QCItemModel
    {
        [Required]
        [Display(Name = "QCID")]
        public int ID { get; set; }

        public string QCNo { get; set; }
        public string QCDate { get; set; }
        public int ProductionID { get; set; }
        public int ProductionTransID { get; set; }
        public string ProductionDate { get; set; }
        public string ReferenceNo { get; set; }
        public int WareHouseID { get; set; }
        [Required]
        [Display(Name = "Item Name")]
        public int? ItemID { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string SupplierName { get; set; }
        public string BatchNo { get; set; }
        public string QCStatus { get; set; }
        public decimal BatchSize { get; set; }
        [Required]
        [Display(Name = "Store")]
        public int? ToWareHouseID { get; set; }
        public decimal? AcceptedQty { get; set; }
        [Required]
        [Display(Name = "Approved Quantity")]
        public decimal? ApprovedQty { get; set; }
        public bool IsCancelled { get; set; }
        public string Remarks { get; set; }
        public string CancelledDate { get; set; }
        public int CreatedUserID { get; set; }
        public string CreatedDate { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }
        public string WareHouse { get; set; }
        public decimal StandardOutput { get; set; }

    }
    public class QCItemViewModel
    {
        public List<QCItemModel> pendingQCList { get; set; }
        public List<QCItemModel> onGoingQCList { get; set; }
        public List<QCItemModel> completedQCList { get; set; }
    }
    public class QCTestModel
    {
        public int ID { get; set; }
        public int QCID { get; set; }
        public int QCTestID { get; set; }
        public string TestName { get; set; }
        public decimal RangeFrom { get; set; }
        public decimal RangeTo { get; set; }
        public string DefinedResult { get; set; }
        public bool IsMandatory { get; set; }


        [Display(Name = "Actual Value")]
        public decimal? ActualValue { get; set; }

        [Display(Name = "Actual Result")]
        public string ActualResult { get; set; }
        public string Remarks { get; set; }
    }
    public class QCTestViewModel
    {
        public QCItemModel QCItem { get; set; }
        public List<QCTestModel> physicalTestDetails { get; set; }
        public List<QCTestModel> chemicalTestDetails { get; set; }
        public List<QCTestModel> organolepticTestDetails { get; set; }
        public List<WareHouseModel> wareHouse { get; set; }
        public List<QCTestModel> testResults { get; set; }
        public List<QCTestModel> pharmaceuticalTestDetails { get; set; }
        public List<QCTestModel> MicrobiologyTestDetails { get; set; }
    }
}