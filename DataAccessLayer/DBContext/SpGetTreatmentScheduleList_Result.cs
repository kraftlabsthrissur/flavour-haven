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
    
    public partial class SpGetTreatmentScheduleList_Result
    {
        public System.DateTime ScheduledDate { get; set; }
        public int TreatmentID { get; set; }
        public int TherapistID { get; set; }
        public int DoctorID { get; set; }
        public int TreatmentRoomID { get; set; }
        public string StartTime { get; set; }
        public string Endtime { get; set; }
        public string Status { get; set; }
        public string Treatment { get; set; }
        public string Patient { get; set; }
        public string Doctor { get; set; }
        public string TreatmentRoom { get; set; }
        public string Medicines { get; set; }
        public string Therapist { get; set; }
        public Nullable<int> totalRecords { get; set; }
        public Nullable<int> recordsFiltered { get; set; }
    }
}
