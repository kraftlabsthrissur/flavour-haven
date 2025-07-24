using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IRoomContract
    {
        int Save(RoomBO room);
        List<RoomBO> GetRoomList();
        List<RoomBO> GetRoomDetails(int ID);
        int UpdateRoom(RoomBO room);
    }
}
