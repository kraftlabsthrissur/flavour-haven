using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class PrescriptionFormatModel
    {
        public int ID { get; set; }
        public string MedicineCategory { get; set; }
        public int MedicineCategoryID { get; set; }
        public string Prescription { get; set; }
        public int PrescriptionID { get; set; }
        public SelectList MedicineCategoryGroupList { get; set; }
        public List<PrescriptionFormatItemModel> Items { get; set; }
        public int count { get; set; }
    }
    public class PrescriptionFormatItemModel
    {
        public int ID { get; set; }
        public string MedicineCategory { get; set; }
        public int MedicineCategoryID { get; set; }
        public string Prescription { get; set; }
        public SelectList MedicineCategoryGroupList { get; set; }
    }
}