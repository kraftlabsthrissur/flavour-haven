using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class RoomDAL
    {
        public int Save(RoomBO room)
        {
            try
            {
                ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    string FormName = "Room";
                    var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    return dbEntity.SpCreateRoom(room.Code,room.RoomTypeID,room.RoomName,room.Rate,room.Description,room.StartDate,room.EndDate,room.StoreID,
                        GeneralBO.FinYear,GeneralBO.LocationID,GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public List<RoomBO> GetRoomList()
        {
            try
            {
                List<RoomBO> Room = new List<RoomBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Room = dbEntity.SpGetRoomList().Select(a => new RoomBO
                    {
                         ID = a.ID,
                         RoomName=a.Name,
                         Code=a.Code,
                         StartDate=(DateTime)a.StartDate,
                         EndDate=(DateTime)a.EndDate
                    }).ToList();

                    return Room;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<RoomBO> GetRoomDetails(int ID)
        {
            try
            {
                List<RoomBO> Room = new List<RoomBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Room = dbEntity.SpGetRoomDetails(ID).Select(a => new RoomBO
                    {
                        ID = a.ID,
                        Code=a.Code,
                        RoomType=a.RoomType,
                        RoomTypeID=a.RoomTypeID,
                        RoomName=a.Name,
                        StartDate=(DateTime)a.StartDate,
                        EndDate=(DateTime)a.EndDate,
                        Description=a.Description,
                        Rate=(decimal)a.Rate,
                        StoreID=(int)a.WarehouseID,
                        Store=a.Store

                    }).ToList();
                    return Room;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public int Update(RoomBO room)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateRoom(room.ID,room.Code,room.RoomTypeID,room.RoomName,room.Rate,room.Description,
                        room.StartDate,room.EndDate,room.StoreID,GeneralBO.CreatedUserID,GeneralBO.FinYear,GeneralBO.LocationID,GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

    }
}
