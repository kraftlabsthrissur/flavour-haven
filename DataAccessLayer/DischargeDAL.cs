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
   public class DischargeDAL
    {
        public DatatableResultBO GetDischargeAdvicedInpatientList(string Patient, string Room, string AdmissionDate,string Doctor,string Type, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetDischargeAdvicedInpatientList(Patient, Room, AdmissionDate, Doctor, Type, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                RoomName = item.RoomName,
                                RoomStatusID = item.RoomStatusID,
                                IPID = item.IPID,
                                ConditionAtDischarge=item.ConditionAtDischarge,
                                CourseInHospital=item.CourseInHospital,
                                DietAdvice=item.DietAdvice,
                                DischargeSummaryID=item.DischargeSummaryID,
                                Doctor=item.Doctor
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

        public List<DischargeBO> GetMedicineList(int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetDischargeMedicine(IPID,GeneralBO.ApplicationID,GeneralBO.LocationID,GeneralBO.FinYear).Select(a => new DischargeBO()
                    {
                       Medicine=a.Medicines,
                       Unit=a.Unit,
                       Qty=a.Quantity,
                       Instructions=a.Instructions
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DischargeBO GetDischargeSummaryDetails(int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetDischargeSummaryDetailsByID(IPID).Select(a => new DischargeBO()
                    {
                        Course=a.CourseInHospital,
                        Condition=a.ConditionAtDischarge,
                        Diet=a.DietAdvice,
                        IsDischarged=(bool)a.IsDischarged
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<DischargePatientBO> GetDischargePatientList(int IPID)
        {
            try
            {
                List<DischargePatientBO> patient = new List<DischargePatientBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    patient = dbEntity.SpGetDischargedPatientDetailsForPrint(IPID, GeneralBO.ApplicationID,GeneralBO.LocationID,GeneralBO.FinYear).Select(a => new DischargePatientBO()
                    {
                        Patient=a.PatientName,
                        Age=(int)a.Age,
                        Gender=a.Gender,
                        AddressLine1=a.AddressLine1,
                        AddressLine2=a.AddressLine2,
                        AddressLine3=a.AddressLine3,
                        OPNo=a.OPNO,
                        IPNo=a.IPNO,
                        DischargeDate=a.DischargeDate,
                        AdmissionDate =(DateTime)a.AdmissionDate,
                        Phone=a.MobileNo,
                        Diagnosis=a.Diagnosis
                    }
                    ).ToList();
                    return patient;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<DischargeMedicineBO> GetInternalMedicineList(int IPID)
        {
            try
            {
                List<DischargeMedicineBO> medicine = new List<DischargeMedicineBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    medicine = dbEntity.SpGetInternalMedicines(IPID, GeneralBO.ApplicationID, GeneralBO.LocationID, GeneralBO.FinYear).Select(a => new DischargeMedicineBO()
                    {
                       Medicine=a.InternalMedicines,
                       Instructions=a.InternalMedicineDescription,
                       NoOfDays=(int)a.InNoOfDays
                    }
                    ).ToList();
                    return medicine;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<DischargeMedicineBO> GetDischargeMedicineList(int IPID)
        {
            try
            {
                List<DischargeMedicineBO> medicine = new List<DischargeMedicineBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    medicine = dbEntity.SpGetDischargeMedicineList(IPID, GeneralBO.ApplicationID, GeneralBO.LocationID, GeneralBO.FinYear).Select(a => new DischargeMedicineBO()
                    {
                        Medicine = a.DischargeMedicines,
                        Instructions = a.MedicineDescription,
                        NoOfDays = (int)a.NoOfDays
                    }
                    ).ToList();
                    return medicine;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<DischargeMedicineBO> GetTreatmentList(int IPID)
        {
            try
            {
                List<DischargeMedicineBO> Treatment = new List<DischargeMedicineBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    Treatment = dbEntity.SpGetTreatmentDetailsForPrint(IPID, GeneralBO.ApplicationID, GeneralBO.LocationID, GeneralBO.FinYear).Select(a => new DischargeMedicineBO()
                    {
                       Treatment=a.Treatment,
                       Complaint1=a.Complaint,
                       Complaint2=a.Complaint2,
                       NoOfDays=(int)a.TreatmentNo,
                       Doctor=a.Doctor
                    }
                    ).ToList();
                    return Treatment;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool IsBillPaid(int IPID)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    ObjectParameter IsBillPaid = new ObjectParameter("IsBillPaid", typeof(bool));
                    dbEntity.SpIsBillPaid(IPID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, IsBillPaid);
                    return Convert.ToBoolean(IsBillPaid.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public int Discharge(int IPID)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    dbEntity.SpDischargeByIPID(IPID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                    return 1;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public List<DischargeBO> GetDischargeAdviceList(int IPID)
        {
            try
            {
                List<DischargeBO> AdviceList = new List<DischargeBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    AdviceList = dbEntity.SpGetDischargeSummaryDetailsByID(IPID).Select(a => new DischargeBO()
                    {
                       Condition=a.ConditionAtDischarge,
                       Course=a.CourseInHospital,
                       Diet=a.DietAdvice
                    }
                    ).ToList();
                    return AdviceList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
