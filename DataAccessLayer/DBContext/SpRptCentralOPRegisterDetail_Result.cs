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
    
    public partial class SpRptCentralOPRegisterDetail_Result
    {
        public Nullable<long> AnnualSerialNo { get; set; }
        public Nullable<long> MonthlySerialNo { get; set; }
        public Nullable<long> DailySerialNo { get; set; }
        public string OLDMRDNo { get; set; }
        public string NewMRDNo { get; set; }
        public string Transno { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public string PatientName { get; set; }
        public string Addressline1 { get; set; }
        public Nullable<int> AddressLine2 { get; set; }
        public Nullable<int> AddressLine3 { get; set; }
        public string Place { get; set; }
        public string MobileNo { get; set; }
        public Nullable<int> DistrictID { get; set; }
        public string District { get; set; }
        public string DoctorName { get; set; }
        public Nullable<int> Age { get; set; }
        public string Gender { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string Department { get; set; }
        public string Remarks { get; set; }
    }
}
