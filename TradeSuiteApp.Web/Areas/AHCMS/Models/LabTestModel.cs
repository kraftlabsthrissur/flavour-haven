using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.AHCMS.Models
{
    public class LabTestModel
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public string Doctor { get; set; }
        public string PatientCode { get; set; }
        public string Patient { get; set; }
        public int Age { get; set; }
        public int PatientTypeID { get; set; }
        public string SalesType { get; set; }
        public SelectList PatientTypeList { get; set; }
        public int SalesTypeID { get; set; }
        public SelectList SalesTypeList { get; set; }
        public int PaymentModeID { get; set; }
        public SelectList PaymentModeList { get; set; }
        public int BankID { get; set; }
        public SelectList BankList { get; set; }
        public string Sex { get; set; }
        public string Mobile { get; set; }
        public int PatientID { get; set; }
        public int? ConfigValue { get; set; }
        public int DoctorID { get; set; }
        public List<LabTestItemModel> Items { get; set; }
        public int AppointmentProcessID { get; set; }
        public SelectList LabTestType { get; set; }
        public int TypeID { get; set; }
        public int SupplierID { get; set; }
        public SelectList SupplierList { get; set; }
        public decimal NetAmount { get; set; }
        public int PatientLabTestID { get; set; }
        public int IPID { get; set; }
        public int OPID { get; set; }       
        public string IssueDate { get; set; }
        public int PatientLabTestMasterID { get; set; }
        public int SalesInvoiceID { get; set; }
        public int LabTestID { get; set; }
        public string LabTest { get; set; }
        public decimal Price { get; set; }
        public int InvoiceID { get; set; }
        public decimal DiscountAmount { get; set; }
    }

    public class LabTestItemModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Status { get; set; }
        public SelectList StatusList { get; set; }
        public string ObserveValue { get; set; }
        public string BiologicalReference { get; set; }
        public string Unit { get; set; }
        public string Category { get; set; }
        public string Date { get; set; }       
        public int DocumentID { get; set; }
        public string Path { get; set; }
        public string CompletedDate { get; set; }
        public List<FileBO> SelectedQuotation { get; set; }
        public string Code { get; set; }
        public string GroupName { get; set; }
        public string ServiceName { get; set; }
        
        public decimal Price { get; set; }
        public string LabtestType { get; set; }
        public string IssueDate { get; set; }
        public bool IsBillGenerated { get; set; }
        public int SupplierID { get; set; }
        public string NormalValueHigh { get; set; }
        public string NormalValueLow { get; set; }
        public int IPID { get; set; }
        public int OPID { get; set; }
        public int PatientID { get; set; }
        public int SalesInvoiceID { get; set; }

        public string LabTestCategory { get; set; }
        public int LabTestCategoryID { get; set; }

    }
}