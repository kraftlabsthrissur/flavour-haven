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
    public class TreatmentProcessDAL
    {
        public List<TreatmentScheduleItemBO> GetTreatmentScheduleList(DateTime fromDate)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    return dbEntity.SpGetDataForTreatmentIssue(fromDate, GeneralBO.FinYear, GeneralBO.ApplicationID, GeneralBO.CreatedUserID).Select(a => new TreatmentScheduleItemBO()
                    {
                        TreatmentID = a.TreatmentID,
                        TreatmentName = a.TreatmentName,
                        PatientID = a.PatientID,
                        PatientName = a.PatientName,
                        DoctorID = a.DoctorID,
                        DoctorName = a.DoctorName,
                        TotalTreatmentNo = (int)a.TreatmentNo,
                        NoOfTreatment = a.NoOfTreatment,
                        TreatmentRoom = a.TreatmentRoomName,
                        TreatmentRoomID = a.TreatmentRoomID,
                        StartTime = a.StartTime,
                        DurationID = (int)a.DurationID,
                        Duration = a.Duration,
                        EndTime = a.EndTime,
                        TherapistID = a.TherapistID,
                        TherapistName = a.Therapist,
                        Status = a.Status,
                        TreatmentScheduleItemID = a.ID,
                        StatusID = a.StatusID,
                        AppointmentProcessID = a.AppointmentProcessID,
                        TreatmentProcessID = a.TreatmentProcessID
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TreatmentScheduleBO> GetDropDownDetails()
        {
            List<TreatmentScheduleBO> item = new List<TreatmentScheduleBO>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    item = dbEntity.SpGetDropDownDetailsForStatus().Select(a => new TreatmentScheduleBO
                    {

                        TreatmentStatusID = a.ID,
                        TreatmentStatus = a.Name
                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Save(List<TreatmentScheduleItemBO> Items, List<TreatmentMedicineItemBO> Medicines, string CommaSeparatedScheduleID)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter IsTreatmentRoomExist = new ObjectParameter("IsTreatmentRoomExist", typeof(bool));
                        ObjectParameter IsPatientExist = new ObjectParameter("IsPatientExist", typeof(bool));
                        ObjectParameter ErrorMessage = new ObjectParameter("ErrorMessage", typeof(string));
                        ObjectParameter IsStok = new ObjectParameter("IsStok", typeof(bool));
                        foreach (var item in Items)
                        {
                            dbEntity.SpCreateTreatmentProcess(
                                   item.TreatmentProcessID,
                                   item.TreatmentScheduleItemID,
                                   item.AppointmentProcessID,
                                   item.Date,
                                   item.PatientID,
                                   item.TreatmentID,
                                   item.TreatmentRoomID,
                                   item.DurationID,
                                   item.TherapistID,
                                   item.TotalTreatmentNo,
                                   item.NoOfTreatment,
                                   item.StartTime,
                                   item.EndTime,
                                   item.Remarks,
                                   item.Status,
                                   CommaSeparatedScheduleID,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.LocationID,
                                   GeneralBO.ApplicationID,
                                   IsTreatmentRoomExist,
                                   IsPatientExist,
                                   ErrorMessage
                              );
                            if (
                                   Convert.ToBoolean(IsTreatmentRoomExist.Value) == true ||
                                   Convert.ToBoolean(IsPatientExist.Value) == true
                                   )
                            {
                                throw new Exception(Convert.ToString(ErrorMessage.Value));
                            }
                            if (item.Status == "Completed")
                            {
                                foreach (var items in Medicines.Where(x => x.TreatmentScheduleID == item.TreatmentScheduleItemID))
                                {
                                    dbEntity.SpCreateUsedTreatmentMedicines(
                                         items.TreatmentScheduleID,
                                         items.ItemID,
                                         items.UnitID,
                                         items.Qty,
                                         GeneralBO.CreatedUserID,
                                         GeneralBO.FinYear,
                                         GeneralBO.LocationID,
                                         GeneralBO.ApplicationID,
                                         IsStok
                                        );
                                    if (Convert.ToBoolean(IsStok.Value) == false)
                                    {
                                        throw new Exception(Convert.ToString("Out of Stock"));
                                    }
                                }

                            }

                        }
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                    transaction.Commit();
                }
            }
            return 1;
        }

        public DatatableResultBO GetTreatmentProcessList(string Type, string DateHint, string StartTimeHint, string EndTimeHint, string TreatmentHint, string PatientHint, string DoctorHint, string MedicineHint, string TreatmentRoomHint, string StatusHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {///ERRPR   

                    var result = dbEntity.SpGetTreatmentProcessList(Type, DateHint, StartTimeHint, EndTimeHint, TreatmentRoomHint, DoctorHint, MedicineHint, PatientHint, TreatmentHint, StatusHint, SortField, SortOrder, Offset, Limit, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                Date = ((DateTime)item.ScheduledDate).ToString("dd-MMM-yyyy"),
                                StartTime = item.StartTime,
                                EndTime = item.Endtime,
                                Patient = item.Patient,
                                Doctor = item.Doctor,
                                Treatment = item.Treatment,
                                TreatmentRoom = item.TreatmentRoom,
                                //Therapist = item.Therapist,
                                Medicines = item.Medicines,
                                Status = item.Status
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

        public List<TreatmentMedicineItemBO> GetTreatmentMedicines(int TreatmentScheduleID, int TreatmentProcessID)
        {
            List<TreatmentMedicineItemBO> Medicines = new List<TreatmentMedicineItemBO>();

            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    Medicines = dbEntity.SpGetTreatmentMedicinesByScheduleID(
                        TreatmentScheduleID,
                        TreatmentProcessID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID).Select(
                        a => new TreatmentMedicineItemBO
                        {
                            ItemID = (int)a.ItemID,
                            Item = a.Item,
                            Stock = (decimal)a.Stock,
                            Qty = a.Qty,
                            UnitID = a.UnitID,
                            Unit = a.Unit,
                            ProductionGroupID = a.ProductionGroupID
                        }
                        ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Medicines;
        }

        public decimal GetItemStockByBatchID(int ItemID, int BatchID, int TreatmentScheduleID)
        {
            try
            {
                ObjectParameter Stock = new ObjectParameter("Stock", typeof(decimal));
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    //dbEntity.SpGetItemStockByBatchID(
                    //               ItemID,
                    //               BatchID,
                    //               TreatmentScheduleID,
                    //               GeneralBO.LocationID,
                    //               GeneralBO.ApplicationID,
                    //               Stock
                    //          );
                }
                return Convert.ToDecimal(Stock.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
