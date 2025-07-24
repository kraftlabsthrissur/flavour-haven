using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class TreatmentModel
    {
        public int ID { get; set; }
        public string TreatmentCode { get; set; }
        public string TreatmentName { get; set; }
        public string AddedDate { get; set; }
        public string Description { get; set; }
        public int TreatmentGroupID { get; set; }
        public string TreatmentGroup { get; set; }
        public SelectList TreatmentGroupList { get; set; }
        public List<TreatmentItemModel> Items { get; set; }
    }
    public class TreatmentItemModel
    {
        public string TreatmentCode { get; set; }
        public string TreatmentName { get; set; }
        public SelectList TreatmentGroupList { get; set; }
        public string AddedDate { get; set; }
        public string Description { get; set; }


    }
}