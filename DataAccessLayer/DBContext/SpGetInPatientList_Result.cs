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
    
    public partial class SpGetInPatientList_Result
    {
        public int AppointmentProcessID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public Nullable<int> ReservationID { get; set; }
        public string RoomName { get; set; }
        public Nullable<System.DateTime> AdmissionDate { get; set; }
        public int RoomStatusID { get; set; }
        public int IPID { get; set; }
        public Nullable<int> totalRecords { get; set; }
        public Nullable<int> recordsFiltered { get; set; }
    }
}
