//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayer.DBContext
{
    using System;
    
    public partial class SpGetIPMedicneListByID_Result
    {
        public Nullable<int> PrescriptionID { get; set; }
        public string Prescription { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public int TransID { get; set; }
        public int DischargeSummaryID { get; set; }
        public string ProductionGroupName { get; set; }
        public string Quantity { get; set; }
        public string Status { get; set; }
        public string DoctorName { get; set; }
        public string MedicineInstruction { get; set; }
        public string QuantityInstruction { get; set; }
    }
}
