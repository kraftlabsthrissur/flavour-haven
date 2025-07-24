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
    public class AppointmentScheduleDAL
    {
        public int Save(AppointmentScheduleBO appointmentScheduleBO, List<AppointmentScheduleItemBO> Items)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    ObjectParameter TransID = new ObjectParameter("TransID", typeof(int));
                    ObjectParameter returnValue = new ObjectParameter("ReturnValue", typeof(int));
                    dbEntity.SpCreateAppointmentSchedule(
                        appointmentScheduleBO.DoctorID,
                        appointmentScheduleBO.FromDate,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID,
                        TransID,
                        returnValue
                    );

                    foreach (var item in Items)
                    {
                        if (item.AppointmentScheduleItemID == 0)
                        {
                            dbEntity.SpCreateAppointmentScheduleItems(
                                Convert.ToInt32(TransID.Value),
                                item.PatientID,
                                item.Time,
                                item.TokenNo,
                                item.DepartmentID
                                );
                        }
                        else
                        {
                            dbEntity.SpUpdateAppointmentScheduleItems(
                            item.AppointmentScheduleItemID,
                            item.PatientID,
                            item.Time,
                            item.TokenNo
                            );
                        }
                    }
                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AppointmentScheduleItemBO> GetAppointmentItems(int DoctorID, DateTime FromDate)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetAppointmentSchedule(DoctorID, FromDate, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AppointmentScheduleItemBO()
                    {
                        Time = a.Time,
                        PatientID = (int)a.PatientID,
                        PatientName = a.PatientName,
                        TokenNo = (int)a.TokenNo,
                        AppointmentScheduleItemID = a.AppointmentScheduleItemID,
                        AppointmentProcessID = a.AppointmentProcessID,
                        DepartmentID = a.DepartmentID,
                        DepartmentName = a.Department
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsDeletable(int DoctorID, int PatientID, DateTime Date)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                dbEntity.SpIsDeletableAppointmentSchedule(
                   DoctorID, PatientID, Date);
            }
            return true;
        }

        public DatatableResultBO GetAppointmentScheduleList(string Type, string DoctorNameHint, string DateHint, string PatientCodeHint, string PatientHint, string TimeHint, string TokenNoHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    var result = dbEntity.SpGetAppointmentScheduleList(Type, DoctorNameHint, DateHint, PatientCodeHint, PatientHint, TimeHint, TokenNoHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Doctor = item.Doctor,
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                PatientCode = item.PatientCode,
                                Patient = item.Patient,
                                Time = item.Time,
                                TokenNo = item.TokenNo,
                                Status = item.DayText,
                                DoctorID = item.DoctorID,
                                PatientID = item.PatientID,
                                AppointmentItemID = item.AppointmentItemID,
                                FromDate = ((DateTime)item.Date).ToString(),
                                BillablesID = item.BillablesID,
                                IsConfirmed = item.IsConfirmed,
                                ConsultationStatus = item.ConsultationStatus,
                                Department = item.Department,
                                DepartmentID = item.DepartmentID,
                                IsProcessed = item.IsProcessed
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

        public bool IsAppointmentProcessed(int DoctorID, int PatientID)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    ObjectParameter IsAppointmentProcessed = new ObjectParameter("IsAppointmentProcessed", typeof(bool));
                    dbEntity.SpIsAppointmentProcessed(DoctorID, PatientID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, IsAppointmentProcessed);
                    return Convert.ToBoolean(IsAppointmentProcessed.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool GetAppointmentConfirmation(int DoctorID, int PatientID, DateTime Date, int AppointmentScheduleItemID, int BillablesID)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    ObjectParameter IsConfirmed = new ObjectParameter("IsConfirmed", typeof(bool));
                    dbEntity.SpCreateAppointmentConfirmation(BillablesID, DoctorID, PatientID, Date, AppointmentScheduleItemID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, IsConfirmed);
                    return ((Convert.ToBoolean(IsConfirmed.Value)));
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public AppointmentScheduleBO GetAppointmentFee(int AppointmentScheduleItemID)
        {
            try
            {
                AppointmentScheduleBO casesheet = new AppointmentScheduleBO();

                using (AHCMSEntities dEntity = new AHCMSEntities())
                {
                    return dEntity.SpGetAppointmentFee(AppointmentScheduleItemID).Select(a => new AppointmentScheduleBO
                    {
                        BillableID = a.ID,
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        Quantity = a.Qty,
                        Rate = a.Rate,
                        PatientID = (int)a.PatientID,
                        Patient = a.Patient,
                        TokenNo = (int)a.TokenNo,
                        Time = a.Time,
                        ConsultationMode = a.PaymentType

                    }).FirstOrDefault();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ConsultationBO> GetConsulationItem(int AppointmentScheduleItemID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetAppointmentFee(AppointmentScheduleItemID).Select(a => new ConsultationBO()
                    {
                        ItemName = a.ItemName,
                        Rate = a.Rate,
                        ItemID = a.ItemID
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int SaveAppointment(AppointmentScheduleBO Appointment, List<ConsultationBO> Items)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter SalesOrderID = new ObjectParameter("SalesOrderID", typeof(string));
                        ObjectParameter SalesInvoiceID = new ObjectParameter("SalesInvoiceID", typeof(string));
                        GeneralDAL generalDAL = new GeneralDAL();

                        foreach (var item in Items)
                        {
                            //if (item.Rate == 0)
                            //{
                            //    var j = dbEntity.SpUpdateSerialNo("ZeroSalesInvoice", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                            //}
                            //else

                            var j = dbEntity.SpUpdateSerialNo("ServiceSalesOrder", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);


                        }
                        dbEntity.SpCreateSalesOrderAndInvoice(SerialNo.Value.ToString(), Appointment.PatientID,
                        Appointment.NetAmount, Appointment.BillableID, Appointment.AppointmentScheduleItemID, Appointment.PaymentModeID,
                        Appointment.BankID, Appointment.ConsultationMode, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, Appointment.Remarks, SalesOrderID, SalesInvoiceID);

                        foreach (var item in Items)
                        {
                            dbEntity.SpCreateSalesOrderAndSalesInvoiceForAppointment(SerialNo.Value.ToString(), Appointment.PatientID,
                            item.Rate, item.ItemID, Appointment.BillableID, Appointment.AppointmentScheduleItemID, Appointment.PaymentModeID,
                            Appointment.BankID, Appointment.ConsultationMode, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, Appointment.Remarks, Convert.ToInt32(SalesOrderID.Value), Convert.ToInt32(SalesInvoiceID.Value));
                        }
                        transaction.Commit();
                        return 1;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;

                    }
                }
            }
        }
        public int UpdateAppointment(AppointmentScheduleBO Appointment)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {

                        dbEntity.SpUpdateAppointmentSchedule(Appointment.AppointmentScheduleItemID, Appointment.PatientID,
                            Appointment.DoctorID, Appointment.DepartmentID, Appointment.AppointmentDate, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);


                        transaction.Commit();
                        return 1;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;

                    }
                }
            }
        }
        public List<AppointmentScheduleBO> GetAppointmentScheduleDetailsForPrint(int ID)
        {
            try
            {
                List<AppointmentScheduleBO> AppointmentSchedule = new List<AppointmentScheduleBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    AppointmentSchedule = dbEntity.SpGetAppointmentScheduleDetailsForPrint(ID, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new AppointmentScheduleBO()
                    {
                        PatientID = (int)a.PatientID,
                        Time = a.Time,
                        FromDate = (DateTime)a.ValidFromDate,
                        ToDate = (DateTime)a.ValidToDate,
                        Patient = a.PatientName,
                        HIN = a.HIN,
                        Addressline1 = a.Addressline1,
                        Addressline2 = a.Addressline2,
                        Addressline3 = a.Addressline3,
                        Place = a.Place,
                        District = a.District,
                        Rate = (decimal)a.Rate,
                        CreatedDate = (DateTime)a.ValidDate,
                        Quantity = a.Qty,
                        ItemName = a.ItemName,
                        TransNo = a.SalesOrderNo,
                        Mode = a.Mode,
                        DoctorName = a.DoctorName,
                        TokenNo = (int)a.TokenNo
                    }
                    ).ToList();

                    return AppointmentSchedule;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AppointmentScheduleBO> GetPatientDetailsForPatientCard(int ID)
        {
            try
            {
                List<AppointmentScheduleBO> AppointmentSchedule = new List<AppointmentScheduleBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    AppointmentSchedule = dbEntity.SpGetPatientDetailsForPatientCard(ID, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new AppointmentScheduleBO()
                    {
                        PatientID = (int)a.ID,
                        Patient = a.Name,
                        Age = (int)a.Age,
                        PatientCode = a.Code,
                        Gender = a.Gender
                    }
                    ).ToList();

                    return AppointmentSchedule;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<AppointmentScheduleBO> GetPatientDetails(int ID)
        {
            try
            {
                List<AppointmentScheduleBO> AppointmentSchedule = new List<AppointmentScheduleBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    AppointmentSchedule = dbEntity.SpGetPatientDetails(ID, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new AppointmentScheduleBO()
                    {
                        PatientID = (int)a.ID,
                        Patient = a.Name,
                        Age = (int)a.Age,
                        PatientCode = a.Code,
                        Gender = a.Gender
                    }
                    ).ToList();

                    return AppointmentSchedule;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AppointmentScheduleBO> GetPatientDetailsWithImage(int ID)
        {
            try
            {
                List<AppointmentScheduleBO> AppointmentSchedule = new List<AppointmentScheduleBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    AppointmentSchedule = dbEntity.SpGetPatientDetailsWithImage(ID, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new AppointmentScheduleBO()
                    {
                        PatientID = (int)a.ID,
                        Patient = a.Name,
                        Age = (int)a.Age,
                        PatientCode = a.Code,
                        Gender = a.Gender,
                        PhotoID = (int)a.PhotoId,
                        PhotoName = a.PhotoName,
                        PhotoPath = a.Path
                    }
                    ).ToList();

                    return AppointmentSchedule;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AppointmentScheduleBO> GetPatientForBarCodeGenerator(int ID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetPatientDetails(ID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AppointmentScheduleBO()
                    {
                        PatientID = (int)a.ID,
                        Patient = a.Name,
                        Age = (int)a.Age,
                        PatientCode = a.Code,
                        Gender = a.Gender
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppointmentScheduleBO> GetPatientForBarCodeGeneratorWithImage(int ID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetPatientDetailsWithImage(ID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AppointmentScheduleBO()
                    {
                        PatientID = (int)a.ID,
                        Patient = a.Name,
                        Age = (int)a.Age,
                        PatientCode = a.Code,
                        Gender = a.Gender,
                        PhotoID = (int)a.PhotoId,
                        PhotoName = a.PhotoName,
                        PhotoPath = a.Path
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool CreateAppointmentCancellation(int AppointmentScheduleItemID, int PatientID, DateTime Date)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {

                //DateTime date = DateTime.ParseExact(Date, "yyyy/MM/dd", null);
                try
                {
                    ObjectParameter IsCancelled = new ObjectParameter("IsCancelled", typeof(bool));
                    dbEntity.SpCreateAppointmentCancellation(AppointmentScheduleItemID, PatientID, Date, GeneralBO.LocationID, GeneralBO.ApplicationID, GeneralBO.FinYear, IsCancelled);
                    return ((Convert.ToBoolean(IsCancelled.Value)));
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public int SaveAndConfirmAppointment(AppointmentScheduleBO Appointment)
        {
            try
            {
                ObjectParameter AppointmentScheduleItemID = new ObjectParameter("AppointmentScheduleItemID", typeof(int));
                ObjectParameter IsConfirmed = new ObjectParameter("IsConfirmed", typeof(bool));
                GeneralDAL generalDAL = new GeneralDAL();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    dbEntity.SpSaveAppointmentScheduleDirectly(Appointment.DoctorID, Appointment.FromDate,
                   Appointment.PatientID, Appointment.Time, Appointment.TokenNo, Appointment.BillableID, Appointment.DepartmentID, GeneralBO.CreatedUserID,
                   GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, IsConfirmed, AppointmentScheduleItemID);
                    return Convert.ToInt16(AppointmentScheduleItemID.Value);
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        public bool DeleteAppointmentScheduleItems(int AppointmentScheduleItemID)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {

                //DateTime date = DateTime.ParseExact(Date, "yyyy/MM/dd", null);
                try
                {
                    ObjectParameter IsCancelled = new ObjectParameter("IsCancelled", typeof(bool));
                    dbEntity.SpDeleteItemsFromAppointmentSchedule(AppointmentScheduleItemID);
                    return ((Convert.ToBoolean(IsCancelled.Value)));
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }


    }
}
