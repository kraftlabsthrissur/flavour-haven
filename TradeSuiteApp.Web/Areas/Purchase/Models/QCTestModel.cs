using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class QCTestModel
    {
        public int ID { get; set; }
        public int QCID { get; set; }
        public int QCTestID { get; set; }
        public string TestName { get; set; }
        public decimal RangeFrom { get; set; }
        public decimal RangeTo { get; set; }
        public string DefinedResult { get; set; }

        //[Required]
       // [Display(Name = "Actual Value")]
        public decimal? ActualValue { get; set; }

      //  [Required]
     //   [Display(Name = "Actual Value")]
        public string ActualResult { get; set; }
        public string Remarks { get; set; }
        public bool IsMandatory { get; set; }



        public string Type { get; set; }
    }
}