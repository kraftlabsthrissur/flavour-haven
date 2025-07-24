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
    public class RoomAllocationBL: IRoomAllocationContract
    {
        private RoomAllocationDAL roomAllocationDAL;
        public RoomAllocationBL()
        {
            roomAllocationDAL = new RoomAllocationDAL();
        }
        public DatatableResultBO GetReferedToIPList(string TransNo, string Patient, string Doctor, string AdmissionDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            return roomAllocationDAL.GetReferedToIPList(TransNo, Patient, Doctor, AdmissionDate, SortField, SortOrder, Offset, Limit);
        }
        public List<RoomAllocationBO> GetRoomDetailsByID(int ID)
        {
            return roomAllocationDAL.GetRoomDetailsByID(ID);
        }
        public int Save(RoomAllocationBO IP)
        {
            return roomAllocationDAL.Save(IP);
        }
        public RoomAllocationBO GetPatientDetailsByID(int ID)
        {
            return roomAllocationDAL.GetPatientDetailsByID(ID);
        }
        public RoomAllocationBO GetReservationDetailsByID(int RoomReservationID, int AppointmentProcessID )
        {
            return roomAllocationDAL.GetReservationDetailsByID(RoomReservationID,AppointmentProcessID);
        }
        public DatatableResultBO GetInPatientList(string Patient, string Room, string AdmissionDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            return roomAllocationDAL.GetInPatientList(Patient, Room, AdmissionDate, SortField, SortOrder, Offset, Limit);
        }
        public RoomAllocationBO GetAllocatedRoomDetails(int ID)
        {
            return roomAllocationDAL.GetAllocatedRoomDetails(ID);
        }
        public List<RoomAllocationBO> GetRoomType()
        {
            return roomAllocationDAL.GetRoomType();
        }
        public List<IpRoomBO> GetAllocatedRoomDetailsByID(int IPID)
        {
            return roomAllocationDAL.GetAllocatedRoomDetailsByID(IPID);
        }
    }
}
