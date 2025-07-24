using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class RoomChangeDAL
    {
        public int Save(List<IpRoomBO> Items)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    foreach (var item in Items)
                    {
                        dbEntity.SpCreateRoomChange(
                            item.FromDate,
                            item.ToDate,
                            item.PatientID,
                            item.DoctorID,
                            item.RoomID,
                            item.RoomStatusID
                            );
                    }
                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
