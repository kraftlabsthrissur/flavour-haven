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
    
    public partial class SpGetXrayList_Result
    {
        public int PatientLabTestMasterID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int PatientID { get; set; }
        public string PatientCode { get; set; }
        public string Doctor { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Patient { get; set; }
        public int IPID { get; set; }
        public string Xray { get; set; }
        public string TransNo { get; set; }
        public Nullable<int> totalRecords { get; set; }
        public Nullable<int> recordsFiltered { get; set; }
    }
}
