using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class RoomAllocationDAL
    {
        public DatatableResultBO GetReferedToIPList(string TransNo, string Patient, string Doctor, string AdmissionDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetPatientsReferedToIP(TransNo, Patient, Doctor, AdmissionDate, SortField, SortOrder, Offset, Limit,GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {

                                AppointmentProcessID = item.ID,
                                TransNo=item.TransNo,
                                Patient=item.PatientName,
                                PatientID=item.PatientID,
                                Doctor=item.DoctorName,
                                DoctorID=item.DoctorID,
                                ReferedDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                ReservationID=item.ReservationID,
                                RoomStatusID=item.RoomStatusID
                                
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
        public List<RoomAllocationBO> GetRoomDetailsByID(int ID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetRoomDetailsByID(ID).Select(a => new RoomAllocationBO
                    {
                        Descriptions=a.Description,
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

        public int Save(RoomAllocationBO IP)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpCreateInpatient(IP.AppointmentProcessID,IP.AdmissionDate,IP.AdmissionDateTill, IP.PatientID, IP.DoctorID,IP.RoomID,
                    IP.ReservationID,IP.ByStander,IP.MobileNumber,IP.IsRoomChange,IP.RoomChangeDate,IP.RoomStatusID,GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public RoomAllocationBO GetPatientDetailsByID(int ID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetPatientDetailsListByID(ID, GeneralBO.ApplicationID).Select(a => new RoomAllocationBO()
                    {
                        PatientID = a.PatientID,
                        PatientName = a.PatientName,
                        TransNo = a.TransNo,
                        AdmissionDate = (DateTime)a.Date,
                        DoctorID = a.DoctorID,
                        AppointmentProcessID = a.AppointmentProcessID,
                        RoomStatusID = 0

                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public RoomAllocationBO GetReservationDetailsByID(int ReservationID, int AppointmentProcessID )
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetReservationDetailsByID(ReservationID,AppointmentProcessID, GeneralBO.ApplicationID).Select(a => new RoomAllocationBO()
                    {
                      RoomTypeID=a.RoomTypeID,
                      RoomID=a.RoomID,
                      RoomName=a.RoomName,
                      RoomType=a.RoomType,
                      Rate=(decimal)a.Rate,
                      FromDate=(DateTime)a.FromDate,
                      ToDate=(DateTime)a.ToDate,
                      ReservationID=a.ID,
                      PatientID=a.PatientID,
                      PatientName=a.PatientName,
                      TransNo=a.TransNO,
                      AdmissionDate=(DateTime)a.AdmissionDate,
                      AdmissionDateTill=(DateTime)a.ToDate,
                      AppointmentProcessID=(int)a.AppointmentProcessID,
                      DoctorID=(int)a.DoctorID
                    }
                    ).FirstOrDefault();

                   
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetInPatientList(string Patient, string Room, string AdmissionDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetInPatientList(Patient,Room,AdmissionDate,SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {

                                AppointmentProcessID = item.AppointmentProcessID,
                                Patient = item.PatientName,
                                PatientID = item.PatientID,
                                AdmissionDate = ((DateTime)item.AdmissionDate).ToString("dd-MMM-yyyy"),
                                ReservationID = item.ReservationID,
                                RoomName=item.RoomName,
                                RoomStatusID=item.RoomStatusID,
                                IPID=item.IPID
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

        public RoomAllocationBO GetAllocatedRoomDetails(int ID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetAllocatedRoomDetailsByID(ID).Select(a => new RoomAllocationBO()
                    {
                        RoomTypeID = a.RoomTypeID,
                        RoomID = a.RoomID,
                        RoomName = a.RoomName,
                        RoomType = a.RoomType,
                        AdmissionDate = (DateTime)a.ActualFromDate,
                        AdmissionDateTill = (DateTime)a.ActualToDate,
                        PatientID=(int)a.PatientID,
                        PatientName=a.PatientName,
                        ByStander=a.AttenderName,
                        MobileNumber=a.MobileNumber,
                        DoctorID=(int)a.DoctorID,
                        IPID=(int)a.IPID
                        
                    }
                    ).FirstOrDefault();


                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<RoomAllocationBO> GetRoomType()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetRoomTypeList().Select(a => new RoomAllocationBO
                    {
                        ID = a.ID,
                        RoomType = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<IpRoomBO> GetAllocatedRoomDetailsByID(int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetAllocatedRoomDetailsList(IPID).Select(a => new IpRoomBO()
                    {
                        FromDate = (DateTime)a.ActualFromDate,
                        ToDate = (DateTime)a.ActualToDate,
                        RoomTypeID = a.RoomTypeID,
                        RoomID = a.RoomID,
                        RoomName = a.RoomName,
                        RoomType = a.RoomType,
                        Rate = (int)a.Rate

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
