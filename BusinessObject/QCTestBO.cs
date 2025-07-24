using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessObject
{
    public class QCTestBO
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
        public int ReferencesID { get; set; }
        public string QCTest { get; set; }

        public List<QCTestItemBO> QCTestItemBO { get; set; }
        public SelectList QCTestList { get; set; }
        public List<QCTestIsDeletedBO> IsDeletedItemBO { get; set; }
    }

    public class QCTestItemBO
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string TestName { get; set; }
        public int QCTestID { get; set; }
        public bool IsMandatory { get; set; }
        public decimal RangeFrom { get; set; }
        public decimal RangeTo { get; set; }
        public string Result { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class QCTestIsDeletedBO
    {
        public int ID { get; set; }
    }
}
