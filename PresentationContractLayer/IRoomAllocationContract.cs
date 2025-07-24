using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IRoomAllocationContract
    {
        DatatableResultBO GetReferedToIPList(string TransNo, string Patient, string Doctor, string AdmissionDate, string SortField, string SortOrder, int Offset, int Limit);
        List<RoomAllocationBO> GetRoomDetailsByID(int ID);
        int Save(RoomAllocationBO IP);
        RoomAllocationBO GetPatientDetailsByID(int ID);
        RoomAllocationBO GetReservationDetailsByID(int ReservationID,int AppointmentProcessID);
        RoomAllocationBO GetAllocatedRoomDetails(int ID);
        DatatableResultBO GetInPatientList(string Patient, string Room, string AdmissionDate, string SortField, string SortOrder, int Offset, int Limit);
        List<RoomAllocationBO> GetRoomType();
        List<IpRoomBO> GetAllocatedRoomDetailsByID(int RoomStatusID);
    }
}
