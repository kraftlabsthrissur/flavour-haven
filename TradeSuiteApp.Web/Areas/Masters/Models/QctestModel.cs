using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class QctestModel
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
        public string References { get; set; }
        public string Result { get; set; }



        public SelectList QCTestList { get; set; }
        public SelectList ReferenceList { get; set; }
        public SelectList ResultList { get; set; }
        public SelectList ItemList { get; set; }
    }
}