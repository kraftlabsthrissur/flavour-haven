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
   public class RoomReservationBL: IRoomReservationContract
    {
        private RoomReservationDAL roomReservationDAL;

        public RoomReservationBL()
        {
            roomReservationDAL = new RoomReservationDAL();
        }
        public List<RoomAllocationBO> GetAvailableRooms(int ID,DateTime FromDate,DateTime ToDate,int PatientID)
        {
            return roomReservationDAL.GetAvailableRooms(ID,FromDate, ToDate, PatientID);
        }
        public List<RoomAllocationBO> GetRoomDetailsByID(int ID)
        {
            return roomReservationDAL.GetRoomDetailsByID(ID);
        }
        public int Save(RoomAllocationBO RoomReservation)
        {
            return roomReservationDAL.Save(RoomReservation);
        }
        public DatatableResultBO GetRoomReservationList(string FromDate, string ToDate, string Patient, string Room, string SortField, string SortOrder, int Offset, int Limit)
        {
            return roomReservationDAL.GetRoomReservationList(FromDate, ToDate, Patient, Room, SortField, SortOrder, Offset, Limit);
        }
        public List<RoomAllocationBO> GetRoomReservationDetails(int ID)
        {
            return roomReservationDAL.GetRoomReservationDetails(ID);
        }
        public int UpdateRoomReservation(RoomAllocationBO RoomReservation)
        {
            return roomReservationDAL.Update(RoomReservation);
        }
        public List<RoomAllocationBO> GetAllRooms()
        {
            return roomReservationDAL.GetAllRooms();
        }
        public List<RoomAllocationBO> GetRoomByID(int ReservationID)
        {
            return roomReservationDAL.GetRoomByID(ReservationID);
        }

        public List<RoomAllocationBO> GetAllocatedRoom(int RoomStatusID)
        {
            return roomReservationDAL.GetAllocatedRoom(RoomStatusID);
        }
    }
}
