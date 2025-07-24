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
    public class IPCaseSheetDAL
    {

        public List<PatientDiagnosisBO> GetPatientDetails(int ID)
        {
            try
            {
                List<PatientDiagnosisBO> patient = new List<PatientDiagnosisBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    patient = dbEntity.SpGetPatientDetailsByID(ID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID).Select(a => new PatientDiagnosisBO()
                    {
                        PatientID = a.PatientID,
                        PatientName = a.PatientName,
                        PatientCode = a.PatientCode,
                        Age = (int)a.Age,
                        DOB = (DateTime)a.DOB,
                        Place = a.Place,
                        Mobile = a.MobileNo,
                        Month = (int)a.Month,
                        Gender = a.Gender
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

        public List<DetailedExaminationBO> GetDetailedExaminationList(string Type)
        {
            try
            {
                List<DetailedExaminationBO> examination = new List<DetailedExaminationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    examination = dbEntity.SpGetDetailedExamination(Type, GeneralBO.ApplicationID).Select(a => new DetailedExaminationBO()
                    {
                        ID = a.ID,
                        Name = a.Text
                    }
                    ).ToList();

                    return examination;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Save(PatientDiagnosisBO diagnosis, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<VitalChartBO> VitalChartItems, List<ReportBO> ReportItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<LabtestsBO> LabTestItems, List<PhysiotherapyBO> PhysiotherapyItems, List<XrayBO> XrayItem, List<RoundsBO> RoundsList, List<MedicineBO> InternalMedicinesList, List<MedicineItemBO> InternalMedicinesItems, List<DischargeSummaryBO> DischargeSummary, List<DoctorListBO> DoctorList)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter AppointmentProcessID = new ObjectParameter("TransID", typeof(int));

                        if (diagnosis.IPID != 0)
                        {
                            dbEntity.SpUpdateInPatient(
                                   diagnosis.IPID,
                                   diagnosis.IsDischargeAdvice,
                                   GeneralBO.LocationID,
                                   GeneralBO.ApplicationID

                                    );
                        }

                        if (Items != null)
                        {
                            foreach (var item in Items)
                            {
                                dbEntity.SpCreateIPExamination(
                                   diagnosis.IPID,
                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.ID,
                                   item.Name,
                                   item.Description,
                                   item.Value,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                            }
                        }

                        //dbEntity.SpDeleteAllBaseLineItems(
                        //diagnosis.AppointmentProcessID,
                        //diagnosis.PatientID,
                        //GeneralBO.ApplicationID,
                        //GeneralBO.LocationID
                        //);

                        dbEntity.SpDeleteAllDataByID(
                               diagnosis.IPID,
                               diagnosis.PatientID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID
                               );
                        if (BaseLineItems != null)
                        {
                            foreach (var item in BaseLineItems)
                            {
                                dbEntity.SpCreateIPBaseLineInformation(
                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.Name,
                                   item.Description,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   diagnosis.IPID
                                    );
                            }
                        }

                        if (DoctorList != null)
                        {
                            foreach (var item in DoctorList)
                            {

                                dbEntity.SpCreateIPDoctorList(
                                   diagnosis.IPID,
                                   item.DoctorNameID,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                            }
                        }

                        if (VitalChartItems != null)
                        {
                            foreach (var item in VitalChartItems)
                            {
                                dbEntity.SpCreateIPVitalChart(
                                diagnosis.IPID,
                                diagnosis.PatientID,
                                item.Date,
                                item.BP,
                                item.Pulse,
                                item.Temperature,
                                item.HR,
                                item.RR,
                                item.Height,
                                item.Weight,
                                item.Others,
                                item.Time,
                                GeneralBO.CreatedUserID,
                                GeneralBO.FinYear,
                                GeneralBO.ApplicationID,
                                GeneralBO.LocationID
                                  );
                            }
                        }

                        if (ReportItems != null)
                        {
                            foreach (var item in ReportItems)
                            {
                                if (item.ID == 0)
                                {
                                    dbEntity.SpCreateIPPatientReports(
                                       diagnosis.IPID,
                                       diagnosis.PatientID,
                                       diagnosis.Date,
                                       item.DocumentID,
                                       item.Name,
                                       item.Description,
                                       GeneralBO.CreatedUserID,
                                       GeneralBO.FinYear,
                                       GeneralBO.ApplicationID,
                                       GeneralBO.LocationID
                                        );
                                }
                            }
                        }

                        ObjectParameter TransID = new ObjectParameter("PatientTreatmentID", typeof(int));
                        if (Treatments != null)
                        {
                            foreach (var item in Treatments)
                            {
                                if (item.PatientTreatmentID == 0)
                                {
                                    dbEntity.SpCreateIPPatientTreatments(
                                   diagnosis.IPID,
                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.TreatmentID,
                                   item.TherapistID,
                                   item.TreatmentRoomID,
                                   item.TreatmentNo,
                                   item.Instructions,
                                   item.StartDate,
                                   item.EndDate,
                                   item.IsMorning,
                                   item.MorningTimeID,
                                   item.IsNoon,
                                   item.Isevening,
                                   item.NoonTimeID,
                                   item.EveningTimeID,
                                   item.IsNight,
                                   item.NightTimeID,
                                   item.NoofDays,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   TransID
                                    );
                                    foreach (var items in TreatmentMedicines.Where(x => x.TreatmentID == item.TreatmentID))
                                    {
                                        dbEntity.SpCreatePatientTreatmentMedicines(
                                             Convert.ToInt32(TransID.Value),
                                             items.MedicineID,
                                             items.StandardMedicineQty,
                                             items.TreatmentMedicineUnitID
                                            );
                                    }
                                }
                                else
                                {
                                    dbEntity.SpUpdatePatientTreatments(
                                   diagnosis.AppointmentProcessID,
                                   diagnosis.PatientID,
                                   item.PatientTreatmentID,
                                   item.TreatmentID,
                                   item.TherapistID,
                                   item.TreatmentRoomID,
                                   item.TreatmentNo,
                                   item.Instructions,
                                   item.StartDate,
                                   item.EndDate,
                                    item.IsMorning,
                                    item.MorningTimeID,
                                    item.IsNoon,
                                   item.Isevening,
                                    item.NoonTimeID,
                                    item.EveningTimeID,
                                    item.IsNight,
                                    item.NightTimeID,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                                    foreach (var items in TreatmentMedicines.Where(x => x.TreatmentID == item.TreatmentID))
                                    {
                                        dbEntity.SpCreatePatientTreatmentMedicines(
                                            item.PatientTreatmentID,
                                             items.MedicineID,
                                             items.StandardMedicineQty,
                                             items.TreatmentMedicineUnitID
                                            );
                                    }
                                }
                            }
                        }
                        ObjectParameter MedicineID = new ObjectParameter("TransID", typeof(int));
                        if (MedicinesItemsList != null)
                        {
                            foreach (var item in MedicinesItemsList)
                            {
                                
                                    dbEntity.SpCreateIPPatientMedicines(
                                   diagnosis.IPID,
                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.EndDate,
                                   item.EveningTime,
                                   item.InstructionsID,
                                   item.Isevening,
                                   item.IsMorning,
                                   item.IsNight,
                                   item.IsNoon,
                                   item.MorningTime,
                                   item.NightTime,
                                   item.NoonTime,
                                   item.StartDate,
                                   item.NoofDays,
                                   item.Description,
                                   item.IsEmptyStomach,
                                   item.IsAfterFood,
                                   item.IsBeforeFood,
                                   item.Frequency,
                                   item.ModeOfAdministrationID,
                                   item.IsMiddleOfFood,
                                   item.IsWithFood,
                                   item.MedicineInstruction,
                                   item.QuantityInstruction,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   MedicineID
                                    );
                                    foreach (var items in MedicinesList.Where(x => x.GroupID == item.GroupID))
                                    {
                                        dbEntity.SpCreateIPPatientMedicinesItems(
                                             Convert.ToInt32(MedicineID.Value),
                                             items.MedicineID,
                                             items.Quantity,
                                             items.UnitID
                                            );
                                    }

                                    dbEntity.SpCreateIPMedicineConsumptionChart(
                                        diagnosis.PatientID,
                                        diagnosis.AppointmentProcessID,
                                        Convert.ToInt32(MedicineID.Value),
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID
                                        );
                                
                                //else
                                //{
                                //    dbEntity.SpUpdateIPPatientMedicines(
                                //    item.PatientMedicineID,
                                //    diagnosis.IPID,
                                //    diagnosis.PatientID,
                                //    diagnosis.Date,
                                //    item.EndDate,
                                //    item.EveningTime,
                                //    item.InstructionsID,
                                //    item.Isevening,
                                //    item.IsMorning,
                                //    item.IsNight,
                                //    item.IsNoon,
                                //    item.MorningTime,
                                //    item.NightTime,
                                //    item.NoonTime,
                                //    item.StartDate,
                                //    item.NoofDays,
                                //    item.Description,
                                //    item.IsEmptyStomach,
                                //    item.IsAfterFood,
                                //    item.IsBeforeFood,
                                //    item.Frequency,
                                //    GeneralBO.CreatedUserID,
                                //    GeneralBO.FinYear,
                                //    GeneralBO.ApplicationID,
                                //    GeneralBO.LocationID
                                //     );
                                //}
                            }
                        }

                        //if (LabTestItems != null)
                        //{
                        //    foreach (var item in LabTestItems)
                        //    {
                        //        if (item.ID == 0)
                        //        {
                        //            dbEntity.SpCreatePatientLabItems(
                        //               item.TestDate,
                        //               diagnosis.AppointmentProcessID,
                        //               diagnosis.IPID,
                        //               item.LabTestID,
                        //               null,
                        //               GeneralBO.CreatedUserID,
                        //               GeneralBO.FinYear,
                        //               GeneralBO.ApplicationID,
                        //               GeneralBO.LocationID
                        //                );
                        //        }
                        //    }
                        //}

                        //dbEntity.SpDeleteAllLabItemsByID(
                        //   diagnosis.AppointmentProcessID,
                        //   GeneralBO.FinYear,
                        //   GeneralBO.ApplicationID,
                        //   GeneralBO.LocationID
                        //   );

                        if (LabTestItems != null)
                        {
                            foreach (var item in LabTestItems)
                            {
                                int OPID;
                                if (diagnosis.AppointmentProcessID == 0)
                                {
                                    OPID = Convert.ToInt32(AppointmentProcessID.Value);
                                }
                                else
                                {
                                    OPID = diagnosis.AppointmentProcessID;
                                }
                                //if (item.ID == 0)
                                //{
                                dbEntity.SpCreatePatientLabItems(
                                   item.TestDate,
                                   OPID,
                                   diagnosis.IPID,
                                   item.LabTestID,
                                   null,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                                //}
                            }
                        }

                        if (PhysiotherapyItems != null)
                        {
                            foreach (var item in PhysiotherapyItems)
                            {
                                dbEntity.SpCreatePatientPhysiotherapy(
                                   diagnosis.Date,
                                   diagnosis.PatientID,
                                   diagnosis.IPID,
                                   item.PhysiotherapyID,
                                   item.StartDate,
                                   item.EndDate,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                            }
                        }

                        if (XrayItem != null)
                        {
                            foreach (var item in XrayItem)
                            {
                                if (item.ID == 0)
                                {
                                    dbEntity.SpCreatePatientXrayDetails(
                                       item.XrayDate,
                                       diagnosis.AppointmentProcessID,
                                       diagnosis.IPID,
                                       item.XrayID,
                                       item.ID,
                                       GeneralBO.CreatedUserID,
                                       GeneralBO.FinYear,
                                       GeneralBO.ApplicationID,
                                       GeneralBO.LocationID
                                        );
                                }
                            }
                        }

                        if (RoundsList != null)
                        {
                            foreach (var item in RoundsList)
                            {
                                dbEntity.SpCreatePatientRoundsDetails(
                                   item.RoundsDate,
                                   diagnosis.PatientID,
                                   diagnosis.IPID,
                                   item.Remarks,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                            }
                        }
                        if (DischargeSummary != null)
                        {
                            foreach (var item in DischargeSummary)
                            {
                                dbEntity.SpCreateDischarageSummary(
                                   diagnosis.IPID,
                                   item.CourseInTheHospital,
                                   item.ConditionAtDischarge,
                                   item.DietAdvice
                                    );
                            }
                        }

                        ObjectParameter DischargeSummaryMedicineID = new ObjectParameter("DischargeSummaryMedicineID", typeof(int));
                        if (InternalMedicinesItems != null)
                        {
                            foreach (var item in InternalMedicinesItems)
                            {
                                if (item.PatientMedicineID == 0)
                                {
                                    dbEntity.SpCreateDischargePatientMedicines(
                                       diagnosis.IPID,
                                       diagnosis.PatientID,
                                       diagnosis.Date,
                                       item.EndDate,
                                       item.EveningTime,
                                       item.InstructionsID,
                                       item.Isevening,
                                       item.IsMorning,
                                       item.IsNight,
                                       item.IsNoon,
                                       item.MorningTime,
                                       item.NightTime,
                                       item.NoonTime,
                                       item.StartDate,
                                       item.NoofDays,
                                       item.Description,
                                       item.IsEmptyStomach,
                                       item.IsAfterFood,
                                       item.IsBeforeFood,
                                       item.Frequency,
                                       GeneralBO.CreatedUserID,
                                       GeneralBO.FinYear,
                                       GeneralBO.ApplicationID,
                                       GeneralBO.LocationID,
                                       DischargeSummaryMedicineID
                                        );
                                    foreach (var items in InternalMedicinesList.Where(x => x.GroupID == item.GroupID))
                                    {
                                        dbEntity.SpCreateIPPatientMedicinesItems(
                                             Convert.ToInt32(DischargeSummaryMedicineID.Value),
                                             items.MedicineID,
                                             items.Quantity,
                                             items.UnitID
                                            );
                                    }
                                }
                                else
                                {
                                    dbEntity.SpUpdateDischargeMedicines(
                                   item.DischargeSummaryID,
                                   item.GroupID,
                                   diagnosis.IPID,
                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.EndDate,
                                   item.EveningTime,
                                   item.InstructionsID,
                                   item.Isevening,
                                   item.IsMorning,
                                   item.IsNight,
                                   item.IsNoon,
                                   item.MorningTime,
                                   item.NightTime,
                                   item.NoonTime,
                                   item.StartDate,
                                   item.NoofDays,
                                   item.Description,
                                   item.IsEmptyStomach,
                                   item.IsAfterFood,
                                   item.IsBeforeFood,
                                   item.Frequency,
                                   item.ModeOfAdministrationID,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                                    foreach (var items in InternalMedicinesList.Where(x => x.GroupID == item.GroupID))
                                    {
                                        dbEntity.SpCreateIPPatientMedicinesItems(
                                             item.PatientMedicineID,
                                             items.MedicineID,
                                             items.Quantity,
                                             items.UnitID
                                            );
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

        public List<ExaminationBO> GetExaminationList(int ID, int IPID)
        {
            try
            {
                List<ExaminationBO> patient = new List<ExaminationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    patient = dbEntity.SpGetIPExaminationDetailsByID(ID, IPID, GeneralBO.ApplicationID).Select(a => new ExaminationBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        GroupName = a.GroupName,
                        Description = a.Description,
                        GeneralOptionID = (int)a.Value,
                        Text = a.Text
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

        public List<DetailedExaminationBO> GetOptionList(string Type)
        {
            try
            {
                List<DetailedExaminationBO> examination = new List<DetailedExaminationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    examination = dbEntity.SpGetDetailedExamination(Type, GeneralBO.ApplicationID).Select(a => new DetailedExaminationBO()
                    {
                        ID = a.ID,
                        Name = a.Text
                    }
                    ).ToList();

                    return examination;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<DropDownListBO> GetTherapistDetails()
        {
            List<DropDownListBO> item = new List<DropDownListBO>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    item = dbEntity.SpGetTherapistList().Select(a => new DropDownListBO
                    {
                        ID = a.ID,
                        Name = a.Name

                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PatientDiagnosisBO> GetDateListByID(int ID)
        {
            List<PatientDiagnosisBO> item = new List<PatientDiagnosisBO>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    item = dbEntity.SpGetAdmitDateByID(ID).Select(a => new PatientDiagnosisBO
                    {
                        Date = (DateTime)a.AdmissionDate

                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PatientDiagnosisBO> GetTreatmentNumberList()
        {
            List<PatientDiagnosisBO> item = new List<PatientDiagnosisBO>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    item = dbEntity.SpGetTreatmentNumberList(GeneralBO.ApplicationID).Select(a => new PatientDiagnosisBO
                    {
                        ID = a.ID,
                        Text = (int)a.Text

                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DropDownListBO> GetInstructionsList()
        {
            List<DropDownListBO> item = new List<DropDownListBO>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    item = dbEntity.SpGetTreatmentInstructionList(GeneralBO.ApplicationID).Select(a => new DropDownListBO
                    {
                        ID = a.ID,
                        Name = a.Name

                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DropDownListBO> GetMedicineTimeList(string Type)
        {
            List<DropDownListBO> item = new List<DropDownListBO>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    item = dbEntity.SpGetMedicineTimeList(Type, GeneralBO.ApplicationID).Select(a => new DropDownListBO
                    {
                        ID = a.ID,
                        Name = a.Time

                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VitalChartBO> GetVitalChart(int PatientID, int IPID)
        {
            try
            {
                PatientDiagnosisBO vitalchart = new PatientDiagnosisBO();

                using (AHCMSEntities dEntity = new AHCMSEntities())
                {
                    return dEntity.SpGetIPVitalChart(PatientID, IPID).Select(a => new VitalChartBO
                    {
                        Date = (DateTime)a.Date,
                        BP = a.BP,
                        Pulse = a.Pulse,
                        Temperature = a.Temperature,
                        HR = a.HR,
                        RR = a.RR,
                        Height = a.Height,
                        Weight = a.Weight,
                        Others = a.Others,
                        Time = a.Time
                    }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TreatmentBO> GetTreatmentListByID(int PatientID, int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetIPTreatmentListByID(PatientID, IPID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new TreatmentBO()
                    {
                        TreatmentID = a.TreatmentID,
                        Name = a.TreatmentName,
                        TreatmentRoomID = a.TreatmentRoomID,
                        TreatmentRoomName = a.TreatmentRoom,
                        TherapistID = a.TherapistID,
                        TherapistName = a.Therapist,
                        StartDate = (DateTime)a.StartDate,
                        EndDate = (DateTime)a.EndDate,
                        NoOfTreatment = (int)a.NoOfTreatment,
                        Status = a.Status,
                        Instructions = a.Instructions,
                        TreatmentNo = (int)a.TreatmentNo
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ReportBO> GetReportListByID(int PatientID, int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetIPReportListByID(PatientID, IPID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ReportBO()
                    {
                        Name = a.Name,
                        DocumentID = (int)a.DocumentID,
                        Description = a.Description,
                        Date = (DateTime)a.Date,
                        ID = a.ID
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ReportBO> GetReportListByIDV5(int PatientID, int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetIPReportListByIDV5(PatientID, IPID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ReportBO()
                    {
                        Name = a.Name,
                        DocumentID = (int)a.DocumentID,
                        Description = a.Description,
                        Date = (DateTime)a.Date,
                        ID = a.ID,
                        IsBeforeAdmission=a.IsBeforeAdmission
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TreatmentItemBO> GetTreatmentMedicineListByID(int PatientID, int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetIPTreatmentMedicneList(PatientID, IPID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new TreatmentItemBO()
                    {
                        TreatmentID = a.TreatmentID,
                        MedicineID = (int)a.Medicines,
                        Medicine = a.ProductionGroup,
                        StandardMedicineQty = a.Quantity,
                        TreatmentMedicineUnitID = a.UnitID,
                        TreatmentMedicineUnit= a.Unit
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MedicineBO> GetMedicinesDetails(int DischargeSumaryID, int PatientID, int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetMedicineDetailsByID(DischargeSumaryID, PatientID, IPID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MedicineBO()
                    {
                        MedicineID = (int)a.MedicineID,
                        TransID = (int)a.TransID,
                        Quantity = (decimal)a.Quantity,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
                        Medicine = a.Medicine
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MedicineBO> GetMedicineListByID(int DischargeSumaryID, int PatientID, int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetIPMedicneListByID(DischargeSumaryID, PatientID, IPID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MedicineBO()
                    {
                        Medicine = a.ProductionGroupName,
                        Prescription = a.Prescription,
                        PrescriptionID = (int)a.PrescriptionID,
                        StartDate = (DateTime)a.StartDate,
                        EndDate = (DateTime)a.EndDate,
                        TransID = a.TransID,
                        DischargeSummaryID = a.DischargeSummaryID,
                        DoctorName = a.DoctorName,
                        Qty = a.Quantity,
                        Status = a.Status,
                        MedicineInstruction = a.MedicineInstruction,
                        QuantityInstruction = a.QuantityInstruction,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MedicineItemBO> GetMedicinesItemsList(int DischargeSummaryID, int PatientID, int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetIPMedicinesItemsList(DischargeSummaryID, PatientID, IPID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MedicineItemBO()
                    {
                        GroupID = a.ID,
                        NoofDays = (int)a.NoOfDays,
                        IsMorning = (bool)a.IsMorning,
                        IsNoon = (bool)a.IsNoon,
                        Isevening = (bool)a.Isevening,
                        IsNight = (bool)a.IsNight,
                        MorningTime = a.MorningTime,
                        NoonTime = a.NoonTime,
                        EveningTime = a.EveningTime,
                        NightTime = a.NightTime,
                        IsEmptyStomach = (bool)a.IsEmptyStomach,
                        IsBeforeFood = (bool)a.IsBeforeFood,
                        IsAfterFood = (bool)a.IsAfterFood,
                        Description = a.Description,
                        MorningTimeID = a.MorningID,
                        EveningTimeID = a.EveningID,
                        NoonTimeID = a.NoonID,
                        NightTimeID = a.NightID,
                        StartDate = (DateTime)a.StartDate,
                        EndDate = (DateTime)a.EndDate,
                        ModeOfAdministrationID = a.ModeOfAdministrationID,
                        IsMiddleOfFood = a.IsMiddleOfFood,
                        IsWithFood = a.IsWithFood,
                        MedicineInstruction = a.MedicineInstruction,
                        QuantityInstruction = a.QuantityInstruction,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PatientDiagnosisBO GetCaseSheet(int PatientID, DateTime FromDate)
        {
            try
            {
                PatientDiagnosisBO casesheet = new PatientDiagnosisBO();

                using (AHCMSEntities dEntity = new AHCMSEntities())
                {
                    return dEntity.SpGetCaseSheetByDate(PatientID, FromDate, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PatientDiagnosisBO
                    {
                        Remark = a.Remarks,
                        NextVisitDate = (DateTime)a.NextVisitedDate

                    }).FirstOrDefault();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ExaminationBO> GetExaminationByDate(int PatientID, DateTime FromDate)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetExaminationByDate(PatientID, FromDate).Select(a => new ExaminationBO()
                    {
                        Value = (int)a.Value,
                        Description = a.Description,
                        Text = a.Text,
                        GroupName = a.GroupName,
                        Name = a.Name

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatatableResultBO GetIPPatientList(string Type, string CodeHint, string NameHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetIPPatientList(Type, CodeHint, NameHint, DateHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                PatientID = item.PatientID,
                                Code = item.PatientCode,
                                Name = item.Patient,
                                IPID = item.IPID,
                                Date = ((DateTime)item.AdmissionDate).ToString("dd-MMM-yyyy"),
                                AppointmentProcessID = item.AppointmentProcessID,
                                IsDischarged = (bool)item.IsDischarged,
                                IsDischargeAdviced = item.IsDischargeAdviced,
                                DischargeSummaryID = item.DischargeSummaryID
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

        public List<TreatmentBO> GetTreatmentList(int PatientID, int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetIPTreatmentList(PatientID, IPID).Select(a => new TreatmentBO()
                    {
                        TreatmentID = a.TreatmentID,
                        Name = a.TreatmentName,
                        TreatmentRoomID = a.TreatmentRoomID,
                        TreatmentRoomName = a.TreatmentRoom,
                        TherapistID = a.TherapistID,
                        TherapistName = a.Therapist,
                        StartDate = (DateTime)a.StartDate,
                        EndDate = (DateTime)a.EndDate,
                        NoOfTreatment = (int)a.NoOfTreatment,
                        Status = a.Status,
                        Instructions = a.Instructions,
                        FinishedTreatment = (int)a.FinishedTreatment,
                        PatientTreatmentID = a.ID,
                        DoctorName = a.DoctorName,
                        IsMorning = a.IsMorning,
                        MorningTime = a.MorningTime,
                        IsNoon = a.IsNoon,
                        NoonTime = a.NoonTime,
                        Isevening = a.IsEvening,
                        EveningTime = a.EveningTime,
                        IsNight = a.IsNight,
                        NightTime = a.NightTime,
                        MorningTimeID=Convert.ToString(a.MorningID),
                        NoonTimeID=Convert.ToString(a.NoonID),
                        EveningTimeID=Convert.ToString(a.EveningID),
                        NightTimeID=Convert.ToString(a.NightID)

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DropDownListBO> GetLabTestAutoComplete(string Hint)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetLabTestAutoComplete(Hint).Select(a => new DropDownListBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Category = a.Category,
                        Price=a.Price
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DropDownListBO> GetPhysiotherapyAutoComplete(string Hint)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetPhysiotherapyAutoComplete(Hint).Select(a => new DropDownListBO()
                    {
                        ID = a.ID,
                        Name = a.Name

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DropDownListBO> GetXrayAutoComplete(string Hint)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetXrayItemsAutoComplete(Hint).Select(a => new DropDownListBO()
                    {
                        ID = a.ID,
                        Name = a.Name

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RoundsBO> GetRoundsList(int PatientID, int IPID)
        {
            try
            {

                using (AHCMSEntities dEntity = new AHCMSEntities())
                {
                    return dEntity.SpGetIPRounds(PatientID, IPID).Select(a => new RoundsBO
                    {
                        RoundsDate = (DateTime)a.Date,
                        Remarks = a.Remarks
                    }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<RoundsBO> GetRoundsListV5(int PatientID, int IPID)
        {
            try
            {

                using (AHCMSEntities dEntity = new AHCMSEntities())
                {
                    return dEntity.SpGetIPRoundsV5(PatientID, IPID).Select(a => new RoundsBO
                    {
                        RoundsDate = (DateTime)a.Date,
                        RoundsTime = a.RoundsTime,
                        Remarks = a.Remarks,
                        ClinicalNote=a.clinicalNote,
                        Doctor=a.Doctor,
                        UserID=(int)a.UserID
                    }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<LabtestsBO> GetLabItems(int OPID)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    return dbEntity.SpGetLabTestItems(OPID).Select(
                    a => new LabtestsBO()
                    {
                        LabTestID = a.ID,
                        Labtest = a.Name,
                        Status = a.Selected,
                        State = a.Status
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<LabtestsBO> GetLabTestResultList(int OPID)
        {
            using (AHCMSEntities dEntity = new DBContext.AHCMSEntities())
            {
                return dEntity.SpGetLabTestResultList(OPID, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new LabtestsBO
                {
                    Labtest = a.LabTest,
                    BiologicalReference = a.BiologicalReference,
                    ObservedValue = a.ObservedValue,
                    Unit = a.Unit
                }).ToList();

            }
        }

        public List<LabtestsBO> GetCheckedTest(int OPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetPrescribedLabtests(OPID).Select(
                   a => new LabtestsBO()
                   {
                       LabTestID = (int)a.ItemID,
                       State = a.Status
                   }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<XrayBO> GetXrayItemList(int OPID)
        {
            using (AHCMSEntities dEntity = new DBContext.AHCMSEntities())
            {
                return dEntity.SpGetXrayItems(OPID, GeneralBO.ApplicationID).Select(a => new XrayBO
                {
                    XrayID = a.ID,
                    XrayName = a.Name,
                    Status = a.Selected,
                    State = a.Status
                }).ToList();

            }
        }

        public List<XrayBO> GetPrescribedXrayTest(int OPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetPrescribedXrayTest(OPID, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(
                   a => new XrayBO()
                   {
                       XrayID = (int)a.ItemID,
                       State = a.Status
                   }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<XrayBO> GetXrayResultItems(int OPID)
        {
            using (AHCMSEntities dEntity = new DBContext.AHCMSEntities())
            {
                return dEntity.SpGetXrayResult(OPID, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new XrayBO
                {
                    XrayName = a.Name,
                    Path = a.Path
                }).ToList();

            }
        }

        public List<LabTestItemBO> GetLabItems(int PatientID, int IPID)
        {
            using (AHCMSEntities dEntity = new AHCMSEntities())
            {
                return dEntity.SpGetLabAndXrayDetails(PatientID, IPID, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new LabTestItemBO
                {
                    ID = a.ID,
                    Date = (DateTime)a.Date,
                    ItemID = (int)a.ItemID,
                    ObserveValue = a.ObservedValue,
                    ItemName = a.ItemName,
                    Category = a.CategoryName,
                    BiologicalReference = a.BiologicalReference,
                    Unit = a.Unit,
                    DocumentID = (int)a.DocumentID,
                    Path = a.Path,
                }).ToList();

            }
        }
        public List<LabTestItemBO> GetXrayItems(int PatientID, int IPID)
        {
            using (AHCMSEntities dEntity = new AHCMSEntities())
            {
                return dEntity.SpGetXrayDetails(PatientID, IPID, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new LabTestItemBO
                {
                    ID = a.ID,
                    Date = (DateTime)a.Date,
                    ItemID = (int)a.ItemID,                   
                    ItemName = a.ItemName,
                    Category = a.CategoryName,                   
                    DocumentID = (int)a.DocumentID,
                    Path = a.Path,
                }).ToList();

            }
        }
        public DischargeSummaryBO GetDischargeSummary(int PatientID, int IPID)
        {
            try
            {
                using (AHCMSEntities dEntity = new AHCMSEntities())
                {
                    return dEntity.SpGetDischargeSummaryDetails(PatientID, IPID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new DischargeSummaryBO
                    {
                        CourseInTheHospital = a.CourseInHospital,
                        DietAdvice = a.DietAdvice,
                        ConditionAtDischarge = a.ConditionAtDischarge

                    }).FirstOrDefault();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ExaminationBO> GetIPBaseLineInformationDetails(int ID, int IPID)
        {
            try
            {
                List<ExaminationBO> patient = new List<ExaminationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    patient = dbEntity.SpGetIPBaseLineInformationDetails(ID, IPID, GeneralBO.ApplicationID).Select(a => new ExaminationBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        GroupName = a.GroupName,
                        IsChecked = Convert.ToBoolean(a.CheckedValue),
                        IsParent = (bool)a.IsParent,
                        Description = a.Description
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

        public List<ExaminationBO> GetIPBaseLineInformationDetailsByID(int ID, int IPID)
        {
            try
            {
                List<ExaminationBO> baseline = new List<ExaminationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    baseline = dbEntity.SpGetIPBaseLineInformationDetails(ID, IPID, GeneralBO.ApplicationID).Select(a => new ExaminationBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        GroupName = a.GroupName,
                        IsChecked = Convert.ToBoolean(a.CheckedValue),
                        IsParent = (bool)a.IsParent,
                        Description = a.Description,
                    }
                    ).ToList();

                    return baseline;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<DoctorListBO> GetDoctorList(int PatientID, DateTime FromDate, int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetIPReferenceDoctorList(PatientID, FromDate, IPID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new DoctorListBO()
                    {
                        DoctorNameID = (int)a.DoctorID,
                        DoctorName = a.Doctor,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //for ilaj by priyanka

        public List<ExaminationBO> GetCaseSheetList(int ID, int OPID)
        {
            try
            {
                List<ExaminationBO> patient = new List<ExaminationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    patient = dbEntity.SpGetExaminationDetailsByIDV2(ID, OPID, GeneralBO.ApplicationID).Select(a => new ExaminationBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        GroupName = a.GroupName,
                        Description = a.Description,
                        //GeneralOptionID = (int)a.Value,
                        //Text = a.Text,
                        //Diagnosis = a.Diagnosis,
                        IsParent = (bool)a.IsParent,
                        IsChecked = Convert.ToBoolean(a.CheckedValue)

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

        public List<ExaminationBO> GetBaseLineInformationList(int ID, int IPID)
        {
            try
            {
                List<ExaminationBO> patient = new List<ExaminationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    patient = dbEntity.SpGetBaseLineInformationDetailsByID(ID, IPID, GeneralBO.ApplicationID).Select(a => new ExaminationBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        GroupName = a.GroupName,
                        IsChecked = Convert.ToBoolean(a.CheckedValue),
                        IsParent = (bool)a.IsParent,
                        Description = a.Description

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

        public List<ExaminationBO> GetRogaPareekshaList(int ID, int OPID)
        {
            try
            {
                List<ExaminationBO> patient = new List<ExaminationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    patient = dbEntity.SpGetRogaPareekshaList(ID, OPID, GeneralBO.ApplicationID).Select(a => new ExaminationBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        GroupName = a.GroupName,
                        IsChecked = Convert.ToBoolean(a.CheckedValue),
                        IsParent = (bool)a.IsParent,
                        Description = a.Description

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

        public List<ExaminationBO> GetRogaNirnayamList(int ID, int OPID)
        {
            try
            {
                List<ExaminationBO> patient = new List<ExaminationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    patient = dbEntity.SpGetRogaNirnayamListItems(ID, OPID, GeneralBO.ApplicationID).Select(a => new ExaminationBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        GroupName = a.GroupName,
                        //IsChecked = Convert.ToBoolean(a.CheckedValue),
                        IsParent = (bool)a.IsParent,
                        Description = a.Description

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

        public List<ExaminationBO> GetIPExaminationList(int ID, int IPID)
        {
            try
            {
                List<ExaminationBO> patient = new List<ExaminationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    patient = dbEntity.SpGetIPCaseSheetDetailsByIDV2(ID, IPID, GeneralBO.ApplicationID).Select(a => new ExaminationBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        GroupName = a.GroupName,
                        Description = a.Description,
                        GeneralOptionID = (int)a.Value,
                        Text = a.Text,
                        Diagnosis = a.Diagnosis,
                        IsParent = (bool)a.IsParent

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

        public int SaveV2(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList, List<BaseLineBO> RogaPareekshaItems, List<BaseLineBO> CaseSheetItems, List<ExaminationBO> RogaNirnayamItem, List<RoundsBO> RoundsList, List<DischargeSummaryBO> DischargeSummary, List<MedicineBO> InternalMedicinesList, List<MedicineItemBO> InternalMedicinesItems)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter AppointmentProcessID = new ObjectParameter("TransID", typeof(int));

                        if (diagnosis.IPID != 0)
                        {
                            dbEntity.SpUpdateInPatient(
                                   diagnosis.IPID,
                                   diagnosis.IsDischargeAdvice,
                                   GeneralBO.LocationID,
                                   GeneralBO.ApplicationID

                                    );
                        }

                        if (Items != null)
                        {
                            foreach (var item in Items)
                            {
                                dbEntity.SpCreateIPExamination(
                                   diagnosis.IPID,
                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.ID,
                                   item.Name,
                                   item.Description,
                                   item.Value,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                            }
                        }

                        //if (Items != null)
                        //{
                        //    foreach (var item in Items)
                        //    {
                        //        int OPID;
                        //        if (diagnosis.AppointmentProcessID == 0)
                        //        {
                        //            OPID = Convert.ToInt32(AppointmentProcessID.Value);
                        //        }
                        //        else
                        //        {
                        //            OPID = diagnosis.AppointmentProcessID;
                        //        }

                        //        dbEntity.SpCreatePatientDiagnosis(
                        //           OPID,
                        //           diagnosis.PatientID,
                        //           diagnosis.Date,
                        //           item.ID,
                        //           item.Name,
                        //           item.Description,
                        //           item.Value,
                        //           GeneralBO.CreatedUserID,
                        //           GeneralBO.FinYear,
                        //           GeneralBO.ApplicationID,
                        //           GeneralBO.LocationID
                        //            );
                        //    }
                        //}

                        //dbEntity.SpDeleteAllBaseLineItems(
                        //     diagnosis.AppointmentProcessID,
                        //     diagnosis.PatientID,
                        //     GeneralBO.ApplicationID,
                        //     GeneralBO.LocationID
                        //     );

                        dbEntity.SpDeleteAllDataByID(
                                   diagnosis.IPID,
                                   diagnosis.PatientID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                   );

                        if (BaseLineItems != null)
                        {
                            foreach (var item in BaseLineItems)
                            {
                                dbEntity.SpCreateIPBaseLineInformation(
                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.Name,
                                   item.Description,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   diagnosis.IPID
                                    );
                            }
                        }

                        if (DoctorList != null)
                        {
                            foreach (var item in DoctorList)
                            {

                                dbEntity.SpCreateIPDoctorList(
                                   diagnosis.IPID,
                                   item.DoctorNameID,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                            }
                        }


                       

                        if (VitalChartItems != null)
                        {
                            foreach (var item in VitalChartItems)
                            {
                                dbEntity.SpCreateIPVitalChart(
                                diagnosis.IPID,
                                diagnosis.PatientID,
                                item.Date,
                                item.BP,
                                item.Pulse,
                                item.Temperature,
                                item.HR,
                                item.RR,
                                item.Height,
                                item.Weight,
                                item.Others,
                                item.Time,
                                GeneralBO.CreatedUserID,
                                GeneralBO.FinYear,
                                GeneralBO.ApplicationID,
                                GeneralBO.LocationID
                                  );
                            }
                        }

                        if (ReportItems != null)
                        {
                            foreach (var item in ReportItems)
                            {
                                if (item.ID == 0)
                                {
                                    dbEntity.SpCreateIPPatientReports(
                                       diagnosis.IPID,
                                       diagnosis.PatientID,
                                       diagnosis.Date,
                                       item.DocumentID,
                                       item.Name,
                                       item.Description,
                                       GeneralBO.CreatedUserID,
                                       GeneralBO.FinYear,
                                       GeneralBO.ApplicationID,
                                       GeneralBO.LocationID
                                        );
                                }
                            }
                        }

                        ObjectParameter TransID = new ObjectParameter("PatientTreatmentID", typeof(int));
                        if (Treatments != null)
                        {
                            foreach (var item in Treatments)
                            {
                                if (item.PatientTreatmentID == 0)
                                {
                                    dbEntity.SpCreateIPPatientTreatments(
                                   diagnosis.IPID,
                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.TreatmentID,
                                   item.TherapistID,
                                   item.TreatmentRoomID,
                                   item.TreatmentNo,
                                   item.Instructions,
                                   item.StartDate,
                                   item.EndDate,
                                   item.IsMorning,
                                   item.MorningTimeID,
                                   item.IsNoon,
                                   item.Isevening,
                                   item.NoonTimeID,
                                   item.EveningTimeID,
                                   item.IsNight,
                                   item.NightTimeID,
                                   item.NoofDays,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   TransID
                                    );
                                    foreach (var items in TreatmentMedicines.Where(x => x.TreatmentID == item.TreatmentID))
                                    {
                                        dbEntity.SpCreatePatientTreatmentMedicines(
                                             Convert.ToInt32(TransID.Value),
                                             items.MedicineID,
                                             items.StandardMedicineQty,
                                             items.TreatmentMedicineUnitID
                                            );
                                    }
                                }
                                else
                                {
                                    dbEntity.SpUpdatePatientTreatments(
                                   diagnosis.AppointmentProcessID,
                                   diagnosis.PatientID,
                                   item.PatientTreatmentID,
                                   item.TreatmentID,
                                   item.TherapistID,
                                   item.TreatmentRoomID,
                                   item.TreatmentNo,
                                   item.Instructions,
                                   item.StartDate,
                                   item.EndDate,
                                   item.IsMorning,
                               item.MorningTimeID,
                               item.IsNoon,
                               item.Isevening,
                               item.NoonTimeID,
                               item.EveningTimeID,
                               item.IsNight,
                               item.NightTimeID,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );

                                    foreach (var items in TreatmentMedicines.Where(x => x.TreatmentID == item.TreatmentID))
                                    {
                                        dbEntity.SpCreatePatientTreatmentMedicines(
                                            item.PatientTreatmentID,
                                             items.MedicineID,
                                             items.StandardMedicineQty,
                                             items.TreatmentMedicineUnitID
                                            );
                                    }
                                }
                            }
                        }
                        ObjectParameter PatientMedicineID = new ObjectParameter("TransID", typeof(int));
                        if (MedicinesItemsList != null)
                        {
                            foreach (var item in MedicinesItemsList)
                            {
                               
                                dbEntity.SpCreateIPPatientMedicines(
                               diagnosis.IPID,
                               diagnosis.PatientID,
                               diagnosis.Date,
                               item.EndDate,
                               // item.PatientMedicineID,
                               item.EveningTime,
                               item.InstructionsID,
                               item.Isevening,
                               item.IsMorning,
                               item.IsNight,
                               item.IsNoon,
                               item.MorningTime,
                               item.NightTime,
                               item.NoonTime,
                               item.StartDate,
                               item.NoofDays,
                               item.Description,
                               item.IsEmptyStomach,
                               item.IsAfterFood,
                               item.IsBeforeFood,
                               item.Frequency,
                               item.ModeOfAdministrationID,
                               item.IsMiddleOfFood,
                               item.IsWithFood,
                               item.MedicineInstruction,
                               item.QuantityInstruction,
                               //item.IsMultipleTimes,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID,
                               PatientMedicineID
                                );
                                    foreach (var items in MedicinesList.Where(x => x.GroupID == item.GroupID))
                                    {
                                        dbEntity.SpCreateIPPatientMedicinesItems(
                                             Convert.ToInt32(PatientMedicineID.Value),
                                                 items.MedicineID,
                                                 items.Quantity,
                                                 items.UnitID
                                            );
                                    }
                                    dbEntity.SpCreateIPMedicineConsumptionChart(
                                            diagnosis.PatientID,
                                            diagnosis.AppointmentProcessID,
                                            Convert.ToInt32(PatientMedicineID.Value),
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID
                                            );
                                
                                //}
                                //int PatientMedicinesID;
                                //PatientMedicinesID = Convert.ToInt32(PatientMedicineID.Value);

                                //if (item.PatientMedicineID == 0)
                                //{
                                //    PatientMedicinesID = Convert.ToInt32(PatientMedicineID.Value);                            
                            }
                            int AppointmentProcess;
                            if (diagnosis.AppointmentProcessID == 0)
                            {
                                AppointmentProcess = Convert.ToInt32(AppointmentProcessID.Value);
                            }
                            else
                            {
                                AppointmentProcess = diagnosis.AppointmentProcessID;
                            }
                            //using (SalesEntities dEntity = new SalesEntities())
                            //{
                            //    dEntity.SpCreateDirectSalesOrder(
                            //         diagnosis.PatientID,
                            //         AppointmentProcess,
                            //         GeneralBO.CreatedUserID,
                            //         GeneralBO.FinYear,
                            //         GeneralBO.LocationID,
                            //         GeneralBO.ApplicationID
                            //        );
                            //}
                        }



                        //dbEntity.SpDeleteAllLabItemsByID(
                        //    diagnosis.AppointmentProcessID,
                        //    GeneralBO.FinYear,
                        //    GeneralBO.ApplicationID,
                        //    GeneralBO.LocationID
                        //    );

                        if (LabTestItems != null)
                        {
                            foreach (var item in LabTestItems)
                            {
                                int OPID;
                                if (diagnosis.AppointmentProcessID == 0)
                                {
                                    OPID = Convert.ToInt32(AppointmentProcessID.Value);
                                }
                                else
                                {
                                    OPID = diagnosis.AppointmentProcessID;
                                }
                                //if (item.ID == 0)
                                //{
                                dbEntity.SpCreatePatientLabItems(
                                   item.TestDate,
                                   OPID,
                                   diagnosis.IPID,
                                   item.LabTestID,
                                   null,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                                //}
                            }
                        }

                        if (XrayItem != null)
                        {
                            foreach (var item in XrayItem)
                            {
                                int OPID;
                                if (diagnosis.AppointmentProcessID == 0)
                                {
                                    OPID = Convert.ToInt32(AppointmentProcessID.Value);
                                }
                                else
                                {
                                    OPID = diagnosis.AppointmentProcessID;
                                }
                                //if (item.ID == 0)
                                //{
                                dbEntity.SpCreatePatientXrayDetails(
                                   item.XrayDate,
                                   OPID,
                                   diagnosis.IPID,
                                   item.XrayID,
                                   item.ID,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                                //}
                            }
                        }

                        if (RoundsList != null)
                        {
                            foreach (var item in RoundsList)
                            {
                                dbEntity.SpCreatePatientRoundsDetails(
                                   item.RoundsDate,
                                   diagnosis.PatientID,
                                   diagnosis.IPID,
                                   item.Remarks,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                            }
                        }


                        int AppointmentID;
                        if (diagnosis.AppointmentProcessID == 0)
                        {
                            AppointmentID = Convert.ToInt32(AppointmentProcessID.Value);
                        }
                        else
                        {
                            AppointmentID = diagnosis.AppointmentProcessID;
                        }
                        dbEntity.SpCreatePatientCaseSheet(
                                  AppointmentID,
                                  diagnosis.PatientID,
                                  diagnosis.Date,
                                  diagnosis.Remark,
                                  diagnosis.NextVisitDate,
                                  GeneralBO.CreatedUserID,
                                  GeneralBO.FinYear,
                                  GeneralBO.ApplicationID,
                                  GeneralBO.LocationID
                                   );

                        dbEntity.SpCreatePatientHistory(
                           diagnosis.AppointmentProcessID,
                           diagnosis.PatientID,
                           diagnosis.Date,
                           diagnosis.Remark,
                           diagnosis.NextVisitDate,
                           GeneralBO.CreatedUserID,
                           GeneralBO.FinYear,
                           GeneralBO.LocationID,
                           GeneralBO.ApplicationID
                            );

                        dbEntity.SpDeleteAllRogaPareekshaItems(
                            diagnosis.AppointmentProcessID,
                            diagnosis.PatientID,
                            GeneralBO.ApplicationID,
                            GeneralBO.LocationID
                            );

                        if (RogaPareekshaItems != null)
                        {
                            foreach (var item in RogaPareekshaItems)
                            {
                                int OPID;
                                if (diagnosis.AppointmentProcessID == 0)
                                {
                                    OPID = Convert.ToInt32(AppointmentProcessID.Value);
                                }
                                else
                                {
                                    OPID = diagnosis.AppointmentProcessID;
                                }

                                dbEntity.SpCreateRogaPareekshaItems(

                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.Name,
                                   item.Description,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   OPID,
                                   diagnosis.IPID
                                    );
                            }
                        }

                        dbEntity.SpDeleteCaseSheetItemsByID(
                          diagnosis.AppointmentProcessID,
                          diagnosis.PatientID,
                          GeneralBO.ApplicationID,
                          GeneralBO.LocationID
                          );

                        if (CaseSheetItems != null)
                        {
                            foreach (var item in CaseSheetItems)
                            {
                                int OPID;
                                if (diagnosis.AppointmentProcessID == 0)
                                {
                                    OPID = Convert.ToInt32(AppointmentProcessID.Value);
                                }
                                else
                                {
                                    OPID = diagnosis.AppointmentProcessID;
                                }

                                dbEntity.SpCreateCaseSheet(

                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.Name,
                                   item.Description,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   OPID,
                                   diagnosis.IPID
                                    );
                            }
                        }

                        if (RogaNirnayamItem != null)
                        {
                            foreach (var item in RogaNirnayamItem)
                            {
                                int OPID;
                                if (diagnosis.AppointmentProcessID == 0)
                                {
                                    OPID = Convert.ToInt32(AppointmentProcessID.Value);
                                }
                                else
                                {
                                    OPID = diagnosis.AppointmentProcessID;
                                }


                                dbEntity.SpCreatePatientDiagnosis(
                                   OPID,
                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.ID,
                                   item.Name,
                                   item.Description,
                                   item.Value,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                            }
                        }

                      
                            if (DischargeSummary != null)
                            {
                                foreach (var item in DischargeSummary)
                                {
                                    dbEntity.SpCreateDischarageSummary(
                                       diagnosis.IPID,
                                       item.CourseInTheHospital,
                                       item.ConditionAtDischarge,
                                       item.DietAdvice
                                        );
                                }
                            }

                            ObjectParameter DischargeSummaryMedicineID = new ObjectParameter("DischargeSummaryMedicineID", typeof(int));
                            if (InternalMedicinesItems != null)
                            {
                                foreach (var item in InternalMedicinesItems)
                                {
                                if (item.PatientMedicineID == 0)
                                {
                                    dbEntity.SpCreateDischargePatientMedicines(
                                       diagnosis.IPID,
                                       diagnosis.PatientID,
                                       diagnosis.Date,
                                       item.EndDate,
                                       item.EveningTime,
                                       item.InstructionsID,
                                       item.Isevening,
                                       item.IsMorning,
                                       item.IsNight,
                                       item.IsNoon,
                                       item.MorningTime,
                                       item.NightTime,
                                       item.NoonTime,
                                       item.StartDate,
                                       item.NoofDays,
                                       item.Description,
                                       item.IsEmptyStomach,
                                       item.IsAfterFood,
                                       item.IsBeforeFood,
                                       item.Frequency,
                                       GeneralBO.CreatedUserID,
                                       GeneralBO.FinYear,
                                       GeneralBO.ApplicationID,
                                       GeneralBO.LocationID,
                                       DischargeSummaryMedicineID
                                        );
                                    foreach (var items in InternalMedicinesList.Where(x => x.GroupID == item.GroupID))
                                    {
                                        dbEntity.SpCreateIPPatientMedicinesItems(
                                             Convert.ToInt32(DischargeSummaryMedicineID.Value),
                                             items.MedicineID,
                                             items.Quantity,
                                             items.UnitID
                                            );
                                    }
                                }
                                   
                                    //else
                                    //{
                                    //    dbEntity.SpUpdateDischargeMedicines(
                                    //   item.DischargeSummaryID,
                                    //   item.PatientMedicineID,
                                    //   diagnosis.IPID,
                                    //   diagnosis.PatientID,
                                    //   diagnosis.Date,
                                    //   item.EndDate,
                                    //   item.EveningTime,
                                    //   item.InstructionsID,
                                    //   item.Isevening,
                                    //   item.IsMorning,
                                    //   item.IsNight,
                                    //   item.IsNoon,
                                    //   item.MorningTime,
                                    //   item.NightTime,
                                    //   item.NoonTime,
                                    //   item.StartDate,
                                    //   item.NoofDays,
                                    //   item.Description,
                                    //   item.IsEmptyStomach,
                                    //   item.IsAfterFood,
                                    //   item.IsBeforeFood,
                                    //   item.Frequency,
                                    //   item.ModeOfAdministrationID,
                                    //   GeneralBO.CreatedUserID,
                                    //   GeneralBO.FinYear,
                                    //   GeneralBO.ApplicationID,
                                    //   GeneralBO.LocationID
                                    //    );
                                    //    foreach (var items in InternalMedicinesList.Where(x => x.GroupID == item.GroupID))
                                    //    {
                                    //        dbEntity.SpCreateIPPatientMedicinesItems(
                                    //             item.PatientMedicineID,
                                    //             items.MedicineID,
                                    //             items.Quantity,
                                    //             items.UnitID
                                    //            );
                                    //    }
                                    //}
                                }
                            }


                    }

                    catch (Exception e)
                    {
                       
                       // transaction.Rollback();
                        throw e;
                    }
                    transaction.Commit();
                   
                }
                return 1;
            }
        }
        public int SaveV5(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<ExaminationBO> NewItems, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList, List<BaseLineBO> RogaPareekshaItems, List<BaseLineBO> CaseSheetItems, List<ExaminationBO> RogaNirnayamItem, List<RoundsBO> RoundsList, List<DischargeSummaryBO> DischargeSummary, List<MedicineBO> InternalMedicinesList, List<MedicineItemBO> InternalMedicinesItems)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter AppointmentProcessID = new ObjectParameter("TransID", typeof(int));

                        if (diagnosis.IPID != 0)
                        {
                            dbEntity.SpUpdateInPatient(
                                   diagnosis.IPID,
                                   diagnosis.IsDischargeAdvice,
                                   GeneralBO.LocationID,
                                   GeneralBO.ApplicationID

                                    );
                        }
                        dbEntity.SpDeleteDashaVidhaPareekhsaByID(
                         0,
                         diagnosis.IPID,
                         diagnosis.PatientID,
                         GeneralBO.ApplicationID,
                         GeneralBO.LocationID
                         );
                        if (Items != null)
                        {
                            foreach (var item in Items)
                            {
                                dbEntity.SpCreateIPExamination(
                                   diagnosis.IPID,
                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.ID,
                                   item.Name,
                                   item.Description,
                                   item.Value,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                            }
                        }
                        if (NewItems != null)
                        {
                            foreach (var item in NewItems)
                            {
                                dbEntity.SpCreateIPDashaVidhaPareekhsa(
                                   diagnosis.IPID,
                                   diagnosis.PatientID,
                                   diagnosis.Date,                                   
                                   item.Name,
                                   item.Area,
                                   item.Description,                                   
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                            }
                        }
                        //if (Items != null)
                        //{
                        //    foreach (var item in Items)
                        //    {
                        //        int OPID;
                        //        if (diagnosis.AppointmentProcessID == 0)
                        //        {
                        //            OPID = Convert.ToInt32(AppointmentProcessID.Value);
                        //        }
                        //        else
                        //        {
                        //            OPID = diagnosis.AppointmentProcessID;
                        //        }

                        //        dbEntity.SpCreatePatientDiagnosis(
                        //           OPID,
                        //           diagnosis.PatientID,
                        //           diagnosis.Date,
                        //           item.ID,
                        //           item.Name,
                        //           item.Description,
                        //           item.Value,
                        //           GeneralBO.CreatedUserID,
                        //           GeneralBO.FinYear,
                        //           GeneralBO.ApplicationID,
                        //           GeneralBO.LocationID
                        //            );
                        //    }
                        //}

                        //dbEntity.SpDeleteAllBaseLineItems(
                        //     diagnosis.AppointmentProcessID,
                        //     diagnosis.PatientID,
                        //     GeneralBO.ApplicationID,
                        //     GeneralBO.LocationID
                        //     );

                        dbEntity.SpDeleteAllDataByID(
                                   diagnosis.IPID,
                                   diagnosis.PatientID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                   );

                        if (BaseLineItems != null)
                        {
                            foreach (var item in BaseLineItems)
                            {
                                dbEntity.SpCreateIPBaseLineInformation(
                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.Name,
                                   item.Description,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   diagnosis.IPID
                                    );
                            }
                        }

                        if (DoctorList != null)
                        {
                            foreach (var item in DoctorList)
                            {

                                dbEntity.SpCreateIPDoctorList(
                                   diagnosis.IPID,
                                   item.DoctorNameID,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                            }
                        }




                        if (VitalChartItems != null)
                        {
                            foreach (var item in VitalChartItems)
                            {
                                dbEntity.SpCreateIPVitalChart(
                                diagnosis.IPID,
                                diagnosis.PatientID,
                                item.Date,
                                item.BP,
                                item.Pulse,
                                item.Temperature,
                                item.HR,
                                item.RR,
                                item.Height,
                                item.Weight,
                                item.Others,
                                item.Time,
                                GeneralBO.CreatedUserID,
                                GeneralBO.FinYear,
                                GeneralBO.ApplicationID,
                                GeneralBO.LocationID
                                  );
                            }
                        }

                        if (ReportItems != null)
                        {
                            foreach (var item in ReportItems)
                            {
                                if (item.ID == 0)
                                {
                                    dbEntity.SpCreateIPPatientReportsV5(
                                       diagnosis.IPID,
                                       diagnosis.PatientID,
                                       diagnosis.Date,
                                       item.DocumentID,
                                       item.Name,
                                       item.Description,
                                       item.IsBeforeAdmission,
                                       GeneralBO.CreatedUserID,
                                       GeneralBO.FinYear,
                                       GeneralBO.ApplicationID,
                                       GeneralBO.LocationID
                                        );
                                }
                            }
                        }

                        ObjectParameter TransID = new ObjectParameter("PatientTreatmentID", typeof(int));
                        if (Treatments != null)
                        {
                            foreach (var item in Treatments)
                            {
                                if (item.PatientTreatmentID == 0)
                                {
                                    dbEntity.SpCreateIPPatientTreatments(
                                   diagnosis.IPID,
                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.TreatmentID,
                                   item.TherapistID,
                                   item.TreatmentRoomID,
                                   item.TreatmentNo,
                                   item.Instructions,
                                   item.StartDate,
                                   item.EndDate,
                                   item.IsMorning,
                                   item.MorningTimeID,
                                   item.IsNoon,
                                   item.Isevening,
                                   item.NoonTimeID,
                                   item.EveningTimeID,
                                   item.IsNight,
                                   item.NightTimeID,
                                   item.NoofDays,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   TransID
                                    );
                                    foreach (var items in TreatmentMedicines.Where(x => x.TreatmentID == item.TreatmentID))
                                    {
                                        dbEntity.SpCreatePatientTreatmentMedicines(
                                             Convert.ToInt32(TransID.Value),
                                             items.MedicineID,
                                             items.StandardMedicineQty,
                                             items.TreatmentMedicineUnitID
                                            );
                                    }
                                }
                                else
                                {
                                    dbEntity.SpUpdatePatientTreatments(
                                   diagnosis.AppointmentProcessID,
                                   diagnosis.PatientID,
                                   item.PatientTreatmentID,
                                   item.TreatmentID,
                                   item.TherapistID,
                                   item.TreatmentRoomID,
                                   item.TreatmentNo,
                                   item.Instructions,
                                   item.StartDate,
                                   item.EndDate,
                                   item.IsMorning,
                               item.MorningTimeID,
                               item.IsNoon,
                               item.Isevening,
                               item.NoonTimeID,
                               item.EveningTimeID,
                               item.IsNight,
                               item.NightTimeID,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );

                                    foreach (var items in TreatmentMedicines.Where(x => x.TreatmentID == item.TreatmentID))
                                    {
                                        dbEntity.SpCreatePatientTreatmentMedicines(
                                            item.PatientTreatmentID,
                                             items.MedicineID,
                                             items.StandardMedicineQty,
                                             items.TreatmentMedicineUnitID
                                            );
                                    }
                                }
                            }
                        }
                        ObjectParameter PatientMedicineID = new ObjectParameter("TransID", typeof(int));
                        if (MedicinesItemsList != null)
                        {
                            foreach (var item in MedicinesItemsList)
                            {

                                dbEntity.SpCreateIPPatientMedicines(
                               diagnosis.IPID,
                               diagnosis.PatientID,
                               diagnosis.Date,
                               item.EndDate,
                               // item.PatientMedicineID,
                               item.EveningTime,
                               item.InstructionsID,
                               item.Isevening,
                               item.IsMorning,
                               item.IsNight,
                               item.IsNoon,
                               item.MorningTime,
                               item.NightTime,
                               item.NoonTime,
                               item.StartDate,
                               item.NoofDays,
                               item.Description,
                               item.IsEmptyStomach,
                               item.IsAfterFood,
                               item.IsBeforeFood,
                               item.Frequency,
                               item.ModeOfAdministrationID,
                               item.IsMiddleOfFood,
                               item.IsWithFood,
                               item.MedicineInstruction,
                               item.QuantityInstruction,
                               //item.IsMultipleTimes,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID,
                               PatientMedicineID
                                );
                                foreach (var items in MedicinesList.Where(x => x.GroupID == item.GroupID))
                                {
                                    dbEntity.SpCreateIPPatientMedicinesItems(
                                         Convert.ToInt32(PatientMedicineID.Value),
                                             items.MedicineID,
                                             items.Quantity,
                                             items.UnitID
                                        );
                                }
                                dbEntity.SpCreateIPMedicineConsumptionChart(
                                        diagnosis.PatientID,
                                        diagnosis.AppointmentProcessID,
                                        Convert.ToInt32(PatientMedicineID.Value),
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID
                                        );

                                //}
                                //int PatientMedicinesID;
                                //PatientMedicinesID = Convert.ToInt32(PatientMedicineID.Value);

                                //if (item.PatientMedicineID == 0)
                                //{
                                //    PatientMedicinesID = Convert.ToInt32(PatientMedicineID.Value);                            
                            }
                            int AppointmentProcess;
                            if (diagnosis.AppointmentProcessID == 0)
                            {
                                AppointmentProcess = Convert.ToInt32(AppointmentProcessID.Value);
                            }
                            else
                            {
                                AppointmentProcess = diagnosis.AppointmentProcessID;
                            }
                            //using (SalesEntities dEntity = new SalesEntities())
                            //{
                            //    dEntity.SpCreateDirectSalesOrder(
                            //         diagnosis.PatientID,
                            //         AppointmentProcess,
                            //         GeneralBO.CreatedUserID,
                            //         GeneralBO.FinYear,
                            //         GeneralBO.LocationID,
                            //         GeneralBO.ApplicationID
                            //        );
                            //}
                        }



                        //dbEntity.SpDeleteAllLabItemsByID(
                        //    diagnosis.AppointmentProcessID,
                        //    GeneralBO.FinYear,
                        //    GeneralBO.ApplicationID,
                        //    GeneralBO.LocationID
                        //    );

                        if (LabTestItems != null)
                        {
                            foreach (var item in LabTestItems)
                            {
                                int OPID;
                                if (diagnosis.AppointmentProcessID == 0)
                                {
                                    OPID = Convert.ToInt32(AppointmentProcessID.Value);
                                }
                                else
                                {
                                    OPID = diagnosis.AppointmentProcessID;
                                }
                                //if (item.ID == 0)
                                //{
                                dbEntity.SpCreatePatientLabItems(
                                   item.TestDate,
                                   OPID,
                                   diagnosis.IPID,
                                   item.LabTestID,
                                   null,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                                //}
                            }
                        }

                        if (XrayItem != null)
                        {
                            foreach (var item in XrayItem)
                            {
                                int OPID;
                                if (diagnosis.AppointmentProcessID == 0)
                                {
                                    OPID = Convert.ToInt32(AppointmentProcessID.Value);
                                }
                                else
                                {
                                    OPID = diagnosis.AppointmentProcessID;
                                }
                                //if (item.ID == 0)
                                //{
                                dbEntity.SpCreatePatientLabItems(
                                   item.XrayDate,
                                   OPID,
                                   diagnosis.IPID,
                                   item.XrayID,
                                   null,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                                //}
                            }
                        }

                        if (RoundsList != null)
                        {
                            foreach (var item in RoundsList)
                            {
                                dbEntity.SpCreatePatientRoundsDetailsV5(
                                   item.RoundsDate,
                                   item.RoundsTime,
                                   diagnosis.PatientID,
                                   diagnosis.IPID,
                                   item.ClinicalNote,
                                   item.Remarks,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                            }
                        }


                        int AppointmentID;
                        if (diagnosis.AppointmentProcessID == 0)
                        {
                            AppointmentID = Convert.ToInt32(AppointmentProcessID.Value);
                        }
                        else
                        {
                            AppointmentID = diagnosis.AppointmentProcessID;
                        }
                        dbEntity.SpCreatePatientCaseSheet(
                                  AppointmentID,
                                  diagnosis.PatientID,
                                  diagnosis.Date,
                                  diagnosis.Remark,
                                  diagnosis.NextVisitDate,
                                  GeneralBO.CreatedUserID,
                                  GeneralBO.FinYear,
                                  GeneralBO.ApplicationID,
                                  GeneralBO.LocationID
                                   );

                        dbEntity.SpCreatePatientHistory(
                           diagnosis.AppointmentProcessID,
                           diagnosis.PatientID,
                           diagnosis.Date,
                           diagnosis.Remark,
                           diagnosis.NextVisitDate,
                           GeneralBO.CreatedUserID,
                           GeneralBO.FinYear,
                           GeneralBO.LocationID,
                           GeneralBO.ApplicationID
                            );

                        dbEntity.SpDeleteAllRogaPareekshaItems(
                            diagnosis.AppointmentProcessID,
                            diagnosis.PatientID,
                            GeneralBO.ApplicationID,
                            GeneralBO.LocationID
                            );

                        if (RogaPareekshaItems != null)
                        {
                            foreach (var item in RogaPareekshaItems)
                            {
                                int OPID;
                                if (diagnosis.AppointmentProcessID == 0)
                                {
                                    OPID = Convert.ToInt32(AppointmentProcessID.Value);
                                }
                                else
                                {
                                    OPID = diagnosis.AppointmentProcessID;
                                }

                                dbEntity.SpCreateRogaPareekshaItems(

                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.Name,
                                   item.Description,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   OPID,
                                   diagnosis.IPID
                                    );
                            }
                        }

                        dbEntity.SpDeleteCaseSheetItemsByID(
                          diagnosis.AppointmentProcessID,
                          diagnosis.PatientID,
                          GeneralBO.ApplicationID,
                          GeneralBO.LocationID
                          );

                        if (CaseSheetItems != null)
                        {
                            foreach (var item in CaseSheetItems)
                            {
                                int OPID;
                                if (diagnosis.AppointmentProcessID == 0)
                                {
                                    OPID = Convert.ToInt32(AppointmentProcessID.Value);
                                }
                                else
                                {
                                    OPID = diagnosis.AppointmentProcessID;
                                }

                                dbEntity.SpCreateCaseSheet(

                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.Name,
                                   item.Description,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   OPID,
                                   diagnosis.IPID
                                    );
                            }
                        }

                        if (RogaNirnayamItem != null)
                        {
                            foreach (var item in RogaNirnayamItem)
                            {
                                int OPID;
                                if (diagnosis.AppointmentProcessID == 0)
                                {
                                    OPID = Convert.ToInt32(AppointmentProcessID.Value);
                                }
                                else
                                {
                                    OPID = diagnosis.AppointmentProcessID;
                                }


                                dbEntity.SpCreatePatientDiagnosis(
                                   OPID,
                                   diagnosis.PatientID,
                                   diagnosis.Date,
                                   item.ID,
                                   item.Name,
                                   item.Description,
                                   item.Value,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID
                                    );
                            }
                        }


                        if (DischargeSummary != null)
                        {
                            foreach (var item in DischargeSummary)
                            {
                                dbEntity.SpCreateDischarageSummary(
                                   diagnosis.IPID,
                                   item.CourseInTheHospital,
                                   item.ConditionAtDischarge,
                                   item.DietAdvice
                                    );
                            }
                        }

                        ObjectParameter DischargeSummaryMedicineID = new ObjectParameter("DischargeSummaryMedicineID", typeof(int));
                        if (InternalMedicinesItems != null)
                        {
                            foreach (var item in InternalMedicinesItems)
                            {
                                if (item.PatientMedicineID == 0)
                                {
                                    dbEntity.SpCreateDischargePatientMedicines(
                                       diagnosis.IPID,
                                       diagnosis.PatientID,
                                       diagnosis.Date,
                                       item.EndDate,
                                       item.EveningTime,
                                       item.InstructionsID,
                                       item.Isevening,
                                       item.IsMorning,
                                       item.IsNight,
                                       item.IsNoon,
                                       item.MorningTime,
                                       item.NightTime,
                                       item.NoonTime,
                                       item.StartDate,
                                       item.NoofDays,
                                       item.Description,
                                       item.IsEmptyStomach,
                                       item.IsAfterFood,
                                       item.IsBeforeFood,
                                       item.Frequency,
                                       GeneralBO.CreatedUserID,
                                       GeneralBO.FinYear,
                                       GeneralBO.ApplicationID,
                                       GeneralBO.LocationID,
                                       DischargeSummaryMedicineID
                                        );
                                    foreach (var items in InternalMedicinesList.Where(x => x.GroupID == item.GroupID))
                                    {
                                        dbEntity.SpCreateIPPatientMedicinesItems(
                                             Convert.ToInt32(DischargeSummaryMedicineID.Value),
                                             items.MedicineID,
                                             items.Quantity,
                                             items.UnitID
                                            );
                                    }
                                }

                                //else
                                //{
                                //    dbEntity.SpUpdateDischargeMedicines(
                                //   item.DischargeSummaryID,
                                //   item.PatientMedicineID,
                                //   diagnosis.IPID,
                                //   diagnosis.PatientID,
                                //   diagnosis.Date,
                                //   item.EndDate,
                                //   item.EveningTime,
                                //   item.InstructionsID,
                                //   item.Isevening,
                                //   item.IsMorning,
                                //   item.IsNight,
                                //   item.IsNoon,
                                //   item.MorningTime,
                                //   item.NightTime,
                                //   item.NoonTime,
                                //   item.StartDate,
                                //   item.NoofDays,
                                //   item.Description,
                                //   item.IsEmptyStomach,
                                //   item.IsAfterFood,
                                //   item.IsBeforeFood,
                                //   item.Frequency,
                                //   item.ModeOfAdministrationID,
                                //   GeneralBO.CreatedUserID,
                                //   GeneralBO.FinYear,
                                //   GeneralBO.ApplicationID,
                                //   GeneralBO.LocationID
                                //    );
                                //    foreach (var items in InternalMedicinesList.Where(x => x.GroupID == item.GroupID))
                                //    {
                                //        dbEntity.SpCreateIPPatientMedicinesItems(
                                //             item.PatientMedicineID,
                                //             items.MedicineID,
                                //             items.Quantity,
                                //             items.UnitID
                                //            );
                                //    }
                                //}
                            }
                        }


                    }

                    catch (Exception e)
                    {

                        // transaction.Rollback();
                        throw e;
                    }
                    transaction.Commit();

                }
                return 1;
            }

        }
        public List<ExaminationBO> GetDashaVidhaPareekhsalist(int PatientID, int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetIPDashaVidhaPareekhsaByID(PatientID, IPID, GeneralBO.ApplicationID).Select(a => new ExaminationBO()
                    {
                        GroupName = a.GroupName,
                        Description = a.Description

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

