using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.AHCMS.Models
{
    public class XraysModel
    {
        public string Date { get; set; }
        public string Doctor { get; set; }
        public string PatientCode { get; set; }
        public string Patient { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Mobile { get; set; }
        public int PatientID { get; set; }
        public int AppointmentProcessID { get; set; }
        public List<XraysItemModel> Items { get; set; }
        public List<FileBO> SelectedQuotation { get; set; }
    }

    public class XraysItemModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Status { get; set; }
        public SelectList StatusList { get; set; }
        public string Remark { get; set; }
        public string Date { get; set; }
        public int DocumentID { get; set; }
        public string Category { get; set; }
        public List<FileBO> SelectedQuotation { get; set; }
        public string Path { get; set; }
    }
}