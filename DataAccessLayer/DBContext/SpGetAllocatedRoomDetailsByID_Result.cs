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
    
    public partial class SpGetAllocatedRoomDetailsByID_Result
    {
        public System.DateTime ActualFromDate { get; set; }
        public System.DateTime ActualToDate { get; set; }
        public int RoomID { get; set; }
        public Nullable<int> PatientID { get; set; }
        public Nullable<int> IPID { get; set; }
        public string PatientName { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public string DoctorName { get; set; }
        public string RoomName { get; set; }
        public string RoomType { get; set; }
        public int RoomTypeID { get; set; }
        public string AttenderName { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<int> DoctorID { get; set; }
    }
}
