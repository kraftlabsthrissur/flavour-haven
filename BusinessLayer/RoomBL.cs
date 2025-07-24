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
   public class RoomBL: IRoomContract
    {
        RoomDAL roomDAL;
        public RoomBL()
        {
            roomDAL = new RoomDAL();
        }
        public int Save(RoomBO room)
        {
            return roomDAL.Save(room);
        }
        public List<RoomBO> GetRoomList()
        {
            return roomDAL.GetRoomList();
        }
        public List<RoomBO> GetRoomDetails(int ID)
        {
            return roomDAL.GetRoomDetails(ID);
        }
        public int UpdateRoom(RoomBO room)
        {
            return roomDAL.Update(room);
        }
    }
}
