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
   public class MedicineConsumptionDAL
    {

        public List<MedicineConsumptionBO> GetMedicineConsumptionList(DateTime Date, int StoreID, int RoomID = 0, string Time = null)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    return dbEntity.SpGetMedicinesForConsumption(Date, RoomID, Time, StoreID, GeneralBO.CreatedUserID).Select(a => new MedicineConsumptionBO()
                    {
                     DoctorID=a.DoctorID,
                     DoctorName=a.Doctor,
                     PatientID=a.PatientID,
                     PatientName=a.Patient,
                     Room=a.Room,
                     RoomID=(int)a.RoomID,
                     PatientMedicinesID=a.PatientMedicinesID,
                     AppointmentProcessID=(int)a.AppointmentProcessID,
                     IPID=(int)a.IPID,
                     Medicine=a.Medicines,
                     ModeOfAdminstration=a.ModeOfAdminstration,
                     BeforeOrAfterFood=a.BeforeOrAfterFood,
                     Date=(DateTime)a.Date,
                     Time=a.Time,
                     Description=a.Description,
                     Status=a.Status,
                     ActiualTime=a.ActualTime,
                    MedicineConsumptionID=a.MedicineConsumptionID
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MedicinesConsumptionListBO> MedicinesForConsumption(int PatientMedicinesID,int StoreID)
        {
            List<MedicinesConsumptionListBO> Medicines = new List<MedicinesConsumptionListBO>();

            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    Medicines = dbEntity.SpGetMedicineItemsForConsumption(
                        PatientMedicinesID,
                        StoreID,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID).Select(
                        a => new MedicinesConsumptionListBO
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

        public int Save(List<MedicineConsumptionBO> Items, List<MedicinesConsumptionListBO> Medicines)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter IsStok = new ObjectParameter("IsStok", typeof(bool));
                        foreach (var item in Items)
                        {
                            dbEntity.SpUpdateIPMedicineConsumption(
                                   item.MedicineConsumptionID,
                                   item.ActiualTime,
                                   item.Status
                              );
                            if (item.Status == "Completed")
                            {
                                foreach (var items in Medicines.Where(x => x.MedicineConsumptionID == item.MedicineConsumptionID))
                                {
                                    dbEntity.SpCreateConsumedIPMedicines(
                                         items.MedicineConsumptionID,
                                         items.StoreID,
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

        public DatatableResultBO GetMedicineConsumptionListForDataTable(string Type, string DateHint, string TimeHint, string PatientHint, string DoctorHint, string MedicineHint, string RoomHint, string StatusHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetMedicineConsumptionListForDataTable(Type, DateHint,TimeHint,RoomHint, DoctorHint, MedicineHint, PatientHint, StatusHint, SortField, SortOrder, Offset, Limit, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                Time = item.Time,
                                Patient = item.Patient,
                                Doctor = item.Doctor,
                                Room = item.Room,
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
    }
}
