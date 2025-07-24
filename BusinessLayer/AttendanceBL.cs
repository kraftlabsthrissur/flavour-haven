using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class AttendanceBL : IAttendanceContract
    {
        AttendanceDAL attendanceDAL;
        public AttendanceBL()
        {
            attendanceDAL = new AttendanceDAL();

        }

        public List<MonthBO> GetMonthList()
        {
            return attendanceDAL.GetMonthList();
        }
    }
}
