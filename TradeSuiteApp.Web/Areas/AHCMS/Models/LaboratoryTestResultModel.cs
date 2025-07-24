using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.AHCMS.Models
{
    public class LaboratoryTestResultModel
    {
        public int OPID { get; set; }
        public int SalesInvoiceID { get; set; }
        public int PatientID { get; set; }
        public List<LaboratoryTestResultItemsModel> Items { get; set; }
    }
    public class LaboratoryTestResultItemsModel
    {
        
        public int PatientLabTestsID { get; set; }
        public int PatientLabTestTransID { get; set; }
        public SelectList StatusList { get; set; }
        public int ItemID { get; set; }
        public string ObservedValue { get; set; }
        public int BillablesID { get; set; }
        public string BiologicalReference { get; set; }
        public string Unit { get; set; }
        public string Item { get; set; }
        public string Status { get; set; }
        public bool IsProcessed { get; set; }
        public string NormalHighLevel { get; set; }
        public string NormalLowLevel { get; set; }
        public int DocumentID { get; set; }
        public List<FileBO> SelectedQuotation { get; set; }
        public string CollectedTime { get; set; }
        public string ReportedTime { get; set; }
    }
}