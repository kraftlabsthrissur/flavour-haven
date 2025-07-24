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
    public class RoomReservationDAL
    {
        public List<RoomAllocationBO> GetAvailableRooms(int ID, DateTime FromDate, DateTime ToDate,int PatientID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetAvailableRooms(ID, (DateTime)FromDate, (DateTime)ToDate,PatientID).Select(a => new RoomAllocationBO
                    {
                        RoomID = a.ID,
                        RoomName = a.Name,
                        Rate = (decimal)a.Rate
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<RoomAllocationBO> GetRoomDetailsByID(int ID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetRoomDetailsByID(ID).Select(a => new RoomAllocationBO
                    {
                        Descriptions = a.Description,
                        Rate = (decimal)a.Rate
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Save(RoomAllocationBO RoomReservation)
        {
            try
            {
                ObjectParameter ReservationID = new ObjectParameter("ReservationID", typeof(int));
                GeneralDAL generalDAL = new GeneralDAL();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpCreateRoomReservation(RoomReservation.Date, RoomReservation.PatientID,
                        RoomReservation.RoomTypeID, RoomReservation.RoomID, RoomReservation.FromDate, RoomReservation.ToDate,
                        RoomReservation.Rate, RoomReservation.FromDate, RoomReservation.ToDate, GeneralBO.CreatedUserID, GeneralBO.FinYear,
                        GeneralBO.LocationID, GeneralBO.ApplicationID, ReservationID);
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public DatatableResultBO GetRoomReservationList(string FromDate, string ToDate, string Patient, string Room, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetRoomReservationList(FromDate, ToDate, Patient, Room, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID=item.ID,
                                Patient = item.Patient,
                                PatientID = item.PatientID,
                                FromDate = ((DateTime)item.FromDate).ToString("dd-MMM-yyyy"),
                                ToDate = ((DateTime)item.ToDate).ToString("dd-MMM-yyyy"),
                                Room = item.Room
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return DatatableResult;
        }

        public List<RoomAllocationBO> GetRoomReservationDetails(int ID)
        {
            try
            {
                List<RoomAllocationBO> RoomReservation = new List<RoomAllocationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    RoomReservation = dbEntity.SpGetRoomReservationByID(ID).Select(a => new RoomAllocationBO
                    {
                        ID = a.ID,
                        PatientName = a.Patient,
                        PatientID = a.PatientID,
                        Date = (DateTime)a.CreateDate,
                        FromDate = (DateTime)a.FromDate,
                        ToDate = (DateTime)a.ToDate,
                        RoomType = a.RoomType,
                        RoomTypeID=a.RoomTypeID,
                        RoomName = a.Room,
                        RoomID=a.RoomID,
                        Rate = (decimal)a.Rate,
                        IPID=(int)a.IPID
                    }).ToList();
                    return RoomReservation;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public int Update(RoomAllocationBO RoomReservation)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpUpdateRoomReservation(RoomReservation.ID,RoomReservation.Date, RoomReservation.PatientID,
                        RoomReservation.RoomTypeID, RoomReservation.RoomID, RoomReservation.FromDate, RoomReservation.ToDate,
                        RoomReservation.Rate, RoomReservation.FromDate, RoomReservation.ToDate, GeneralBO.CreatedUserID, GeneralBO.FinYear,
                        GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<RoomAllocationBO> GetAllRooms()
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetAllRooms().Select(a => new RoomAllocationBO
                    {
                        RoomID = a.ID,
                        RoomName = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<RoomAllocationBO> GetRoomByID(int ReservationID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetRoomByID(ReservationID).Select(a => new RoomAllocationBO
                    {
                        RoomID = a.RoomID,
                        RoomName = a.RoomName,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<RoomAllocationBO> GetAllocatedRoom(int RoomStatusID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetAllocatedRoom(RoomStatusID).Select(a => new RoomAllocationBO
                    {
                        RoomID = a.RoomID,
                        RoomName = a.RoomName,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
