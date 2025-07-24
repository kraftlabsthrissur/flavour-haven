using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class LabTestBO
    {
        public DateTime Date { get; set; }
        public string Doctor { get; set; }
        public string PatientCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Place { get; set; }
        public string Patient { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Mobile { get; set; }
        public int PatientID { get; set; }
        public int PatientLabTestID { get; set; }
        public int IPID { get; set; }
        public int OPID { get; set; }
        public decimal NetAmount { get; set; }
        public int SalesTypeID { get; set; }
        public string SalesType { get; set; }
        public int PatientTypeID { get; set; }
        public int PaymentModeID { get; set; }
        public int BankID { get; set; }
        public decimal price { get; set; }
        public decimal MRP { get; set; }
        public string TransNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int AppointmentProcessID { get; set; }
        public int InvoiceID { get; set; }
        public decimal DiscountAmount { get; set; }
    }

    public class LabTestItemBO
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string BiologicalReference { get; set; }
        public string Unit { get; set; }
        public string Status { get; set; }
        public string ObserveValue { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public int DocumentID { get; set; }
        public string Path { get; set; }
        public DateTime CompletedDate { get; set; }
        public string GroupName { get; set; }
        public string ServiceName { get; set; }
        
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public bool IsBillGenerated { get; set; }
        public DateTime IssueDate { get; set; }
        public int SupplierID { get; set; }
        public int SalesInvoiceID { get; set; }
        public string NormalValueHigh { get; set; }
        public string NormalValueLow { get; set; }

        public string LabTestCategory { get; set; }
        public int LabTestCategoryID { get; set; }
    }
}
