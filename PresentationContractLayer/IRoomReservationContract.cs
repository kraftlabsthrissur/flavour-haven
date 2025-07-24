using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IRoomReservationContract
    {
        List<RoomAllocationBO> GetAvailableRooms(int ID,DateTime FromDate, DateTime ToDate,int PatientID);
        List<RoomAllocationBO> GetAllRooms();
        List<RoomAllocationBO> GetRoomByID(int ReservationID);
        List<RoomAllocationBO> GetAllocatedRoom(int RoomStatusID);
        List<RoomAllocationBO> GetRoomDetailsByID(int ID);
        int Save(RoomAllocationBO RoomReservation);
        DatatableResultBO GetRoomReservationList(string FromDate, string ToDate, string Patient, string Room, string SortField, string SortOrder, int Offset, int Limit);
        List<RoomAllocationBO> GetRoomReservationDetails(int ID);
        int UpdateRoomReservation(RoomAllocationBO RoomReservation);
    }
}
