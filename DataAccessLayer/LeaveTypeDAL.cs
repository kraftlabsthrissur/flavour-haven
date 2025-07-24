using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class LeaveTypeDAL
    {
        public List<LeaveTypeBO> GetLeaveTypeList()
        {
            List<LeaveTypeBO> LeaveTypeList = new List<LeaveTypeBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    LeaveTypeList = dbEntity.SpGetLeaveTypeList(GeneralBO.FinYear,GeneralBO.LocationID,GeneralBO.ApplicationID).Select(a => new LeaveTypeBO
                    {
                        LeaveTypeID = a.ID,
                        Name = a.Name
                    }).ToList();
                    return LeaveTypeList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
