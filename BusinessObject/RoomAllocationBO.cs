using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class RoomAllocationBO
    {
        public int ID { get; set; }
        public int RoomTypeID { get; set; }
        public int RoomID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public int ReservationID { get; set; }
        public int RoomStatusID { get; set; }
        public int IPID { get; set; }

        public string RoomName { get; set; }
        public string RoomType { get; set; }
        public string Descriptions { get; set; }
        public string ReferedDate { get; set; }
        public string PatientName { get; set; }
        public string TransNo { get; set; }
        public string ByStander { get; set; }
        public string MobileNumber { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime AdmissionDateTill { get; set; }
        public decimal Rate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime Date { get; set; }
        public DateTime RoomChangeDate { get; set; }

        public bool IsRoomChange { get; set; }

    }
    public class IpRoomBO
    {
        public int RoomID { get; set; }
        public int RoomTypeID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public int RoomStatusID { get; set; }
        public string RoomName { get; set; }
        public string RoomType { get; set; }
        public decimal Rate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
