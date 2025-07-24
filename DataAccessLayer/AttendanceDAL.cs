using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AttendanceDAL
    {
        public List<MonthBO> GetMonthList()
        {
            List<MonthBO> MonthList = new List<MonthBO>();
            using (PayrollEntities dEntity = new PayrollEntities())
            {
                MonthList = dEntity.SpGetMonthList().Select(a => new MonthBO
                {
                    ID = a.ID,
                    //Name =a.Month,
                }).ToList();
                return MonthList;
            }
        }

    }
}
