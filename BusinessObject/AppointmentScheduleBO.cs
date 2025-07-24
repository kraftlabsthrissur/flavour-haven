using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AppointmentScheduleBO
    {
        public int ID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public List<AppointmentScheduleItemBO> Items { get; set; }
        public List<ConsultationBO> ConsultationItems { get; set; }
        public int BillableID { get; set; }
        public int ItemID { get; set; }
        public decimal Quantity { get; set; }
        public string ItemName { get; set; }
        public decimal Rate { get; set; }
        public int PatientID { get; set; }
        public string Patient { get; set; }
        public string HIN { get; set; }
        public int TokenNo { get; set; }
        public int Age { get; set; }
        public string PatientCode { get; set; }
        public string Gender { get; set; }
        public string Time { get; set; }
        public int DepartmentID { get; set; }
        public int PhotoID { get; set; }
        public string PhotoName { get; set; }
        public string PhotoPath { get; set; }
        public string Addressline2 { get; set; }
        public string Addressline1 { get; set; }
        public string Addressline3 { get; set; }
        public string Place { get; set; }
        public string District { get; set; }

        public string TransNo { get; set; }
        public string Mode { get; set; }
        public int AppointmentScheduleItemID { get; set; }
        public string Remarks { get; set; }
        public decimal NetAmount { get; set; }
        public int PaymentModeID { get; set; }
        public int BankID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ConsultationMode { get; set; }
        public DateTime AppointmentDate { get; set; }
    }

    public class AppointmentScheduleItemBO
    {
        public string FromDate { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string Time { get; set; }
        public int TokenNo { get; set; }
        public int AppointmentScheduleItemID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }

    }

    public class DateLisBO
    {
        public DateTime day1 { get; set; }
        public DateTime day2 { get; set; }
        public DateTime day3 { get; set; }
        public DateTime day4 { get; set; }
        public DateTime day5 { get; set; }
        public DateTime day6 { get; set; }
        public DateTime day7 { get; set; }
    }
    public class ConsultationBO
    {
        public string ItemName { get; set; }
        public decimal Rate { get; set; }
        public int ItemID { get; set; }
    }
}



