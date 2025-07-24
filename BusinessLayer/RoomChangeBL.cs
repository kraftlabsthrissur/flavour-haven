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
   public class RoomChangeBL:IRoomChangeContract
    {
        RoomChangeDAL roomChangeDAL;

        public RoomChangeBL()
        {
            roomChangeDAL = new RoomChangeDAL();
        }
        public int Save(List<IpRoomBO> Items)
        {
            return roomChangeDAL.Save(Items);
        }
    }
}
