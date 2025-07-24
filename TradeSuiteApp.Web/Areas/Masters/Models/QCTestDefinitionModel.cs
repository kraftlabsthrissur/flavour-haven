using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class QCTestDefinitionModel
    {
        public int ID { get; set; }
        public int QCID { get; set; }
        public int QCTestID { get; set; }
        public string TestName { get; set; }
        public decimal RangeFrom { get; set; }
        public decimal RangeTo { get; set; }
        public string DefinedResult { get; set; }
        public decimal? ActualValue { get; set; }
        public string ActualResult { get; set; }
        public string Remarks { get; set; }
        public bool IsMandatory { get; set; }
        public string Type { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Result { get; set; }
        public string QCTest { get; set; }
        public List<QCTestItemModel> Items { get; set; }
        public SelectList QCTestList { get; set; }
        public List<QCTestIsDeletedModel> IsDeletedItem { get; set; }
        public int count { get; set; }
    }

    public class QCTestItemModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string TestName { get; set; }
        public int QCTestID { get; set; }
        public bool IsMandatory { get; set; }
        public decimal RangeFrom { get; set; }
        public decimal RangeTo { get; set; }
        public string Result { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class QCTestIsDeletedModel
    {
        public int ID { get; set; }
    }



}
