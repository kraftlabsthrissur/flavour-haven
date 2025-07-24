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
    public class PatientDiagnosisDAL
    {
        public DatatableResultBO GetManagePatientList(string Type, string CodeHint, string NameHint, string TimeHint, string TokenHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetManagerPatientList(Type, CodeHint, NameHint, TimeHint, TokenHint, DateHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID).ToList();
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
                                Code = item.Code,
                                Name = item.Name,
                                TransID = item.ScheduleItemID,
                                Time = item.Time,
                                TokenNo = item.TokenNo,
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                AppointmentProcessID = item.AppointmentProcessID,
                                IsCompleted = item.IsCompleted,
                                IsReferedIP = item.IsReferedIP,
                                ReviewID = item.ReviewID,
                                IsWalkin = item.IsWalkin
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
                        AdmissionDate = a.AdmissionDate,
                        Doctor = a.Doctor,
                        IPDoctor = a.IPDoctorName,
                        RoomName = a.RoomName,
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

        public int Save(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    ObjectParameter AppointmentProcessID = new ObjectParameter("TransID", typeof(int));

                    if (diagnosis.AppointmentProcessID == 0)
                    {
                        dbEntity.SpCreateAppointmentProcess(
                                   diagnosis.Date,
                                   diagnosis.PatientID,
                                   diagnosis.AppointmentScheduleItemID,
                                   diagnosis.AppointmentType,
                                   diagnosis.VisitType,
                                   diagnosis.IsCompleted,
                                   diagnosis.IsReferedIP,
                                   diagnosis.IswalkIn,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   AppointmentProcessID
                                    );
                    }
                    else
                    {
                        dbEntity.SpUpdateAppointmentProcess(
                               diagnosis.AppointmentProcessID,
                               diagnosis.PatientID,
                               diagnosis.Date,
                               diagnosis.VisitType,
                               diagnosis.IsCompleted,
                               diagnosis.IsReferedIP,
                               diagnosis.IswalkIn,
                               diagnosis.ParentID,
                               diagnosis.ReviewID,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID
                                );
                    }
                    if (Items != null)
                    {
                        foreach (var item in Items)
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

                    if (DoctorList != null)
                    {
                        foreach (var item in DoctorList)
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

                            dbEntity.SpCreateDoctorList(
                               OPID,
                               item.DoctorNameID,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID
                                );
                        }
                    }

                    //dbEntity.SpDeleteAllBaseLineItems(
                    //     diagnosis.AppointmentProcessID,
                    //     diagnosis.PatientID,
                    //     GeneralBO.ApplicationID,
                    //     GeneralBO.LocationID
                    //     );
                    if (BaseLineItems != null)
                    {
                        foreach (var item in BaseLineItems)
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

                            dbEntity.SpCreateBaseLineInformation(

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


                    dbEntity.SpDeleteVitalchartByID(
                          diagnosis.AppointmentProcessID,
                          diagnosis.PatientID,
                          GeneralBO.ApplicationID,
                          GeneralBO.LocationID
                          );

                    if (VitalChartItems != null)
                    {
                        foreach (var item in VitalChartItems)
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
                            dbEntity.SpCreateVitalChartForPatientDiagnosis(
                            OPID,
                            diagnosis.PatientID,
                            diagnosis.Date,
                            item.BP,
                            item.Pulse,
                            item.Temperature,
                            item.HR,
                            item.RR,
                            item.Height,
                            item.Weight,
                            item.Others,
                            item.Unit,
                            item.BMI,
                            item.RespiratoryRate,
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
                            int ProcessID;
                            if (diagnosis.AppointmentProcessID == 0)
                            {
                                ProcessID = Convert.ToInt32(AppointmentProcessID.Value);
                            }
                            else
                            {
                                ProcessID = diagnosis.AppointmentProcessID;
                            }
                            dbEntity.SpCreatePatientReports(
                               ProcessID,
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
                               diagnosis.Date,//Error
                               OPID,
                               diagnosis.IPID,
                               item.LabTestID,
                              diagnosis.ParentID,
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

                    ObjectParameter TransID = new ObjectParameter("TransID", typeof(int));
                    if (Treatments != null)
                    {
                        foreach (var item in Treatments)
                        {
                            int ProcessID;
                            if (diagnosis.AppointmentProcessID == 0)
                            {
                                ProcessID = Convert.ToInt32(AppointmentProcessID.Value);
                            }
                            else
                            {
                                ProcessID = diagnosis.AppointmentProcessID;
                            }
                            //if (item.PatientTreatmentID == 0)
                            //{
                            dbEntity.SpCreatePatientTreatments(
                               ProcessID,
                               diagnosis.PatientID,
                               diagnosis.Date,
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
                               item.NoofDays,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID,
                               TransID
                                );

                            int PatientTreatmentID;
                            PatientTreatmentID = Convert.ToInt32(TransID.Value);
                            foreach (var items in TreatmentMedicines.Where(x => x.TreatmentID == item.TreatmentID))
                            {
                                dbEntity.SpCreatePatientTreatmentMedicines(
                                PatientTreatmentID,
                                items.MedicineID,
                                items.StandardMedicineQty,
                                items.MedicineUnitID
                               );
                            }
                            //}
                            //else
                            //{
                            //    dbEntity.SpUpdatePatientTreatments(
                            //       diagnosis.AppointmentProcessID,
                            //       diagnosis.PatientID,
                            //       item.PatientTreatmentID,
                            //       item.TreatmentID,
                            //       item.TherapistID,
                            //       item.TreatmentRoomID,
                            //       item.TreatmentNo,
                            //       item.Instructions,
                            //       item.StartDate,
                            //       item.EndDate,
                            //       GeneralBO.CreatedUserID,
                            //       GeneralBO.FinYear,
                            //       GeneralBO.ApplicationID,
                            //       GeneralBO.LocationID
                            //        );
                            //}
                        }
                    }

                    ObjectParameter PatientMedicineID = new ObjectParameter("TransID", typeof(int));
                    if (MedicinesItemsList != null)
                    {
                        foreach (var item in MedicinesItemsList)
                        {
                            int ProcessID;
                            if (diagnosis.AppointmentProcessID == 0)
                            {
                                ProcessID = Convert.ToInt32(AppointmentProcessID.Value);
                            }
                            else
                            {
                                ProcessID = diagnosis.AppointmentProcessID;
                            }
                            //if (item.PatientMedicineID == 0)
                            //{
                            dbEntity.SpCreatePatientMedicines(
                               ProcessID,
                               diagnosis.PatientID,
                               diagnosis.Date,
                               item.EndDate,
                               item.PatientMedicineID,
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
                               item.IsMultipleTimes,
                               item.MedicineInstruction,
                               item.QuantityInstruction,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID,
                               PatientMedicineID
                                );
                            //}

                            //else
                            //{
                            //    dbEntity.SpUpdatePatientMedicines(
                            //       diagnosis.AppointmentProcessID,
                            //       item.PatientMedicineID,
                            //       item.EndDate,
                            //       item.EveningTime,
                            //       item.InstructionsID,
                            //       item.Isevening,
                            //       item.IsMorning,
                            //       item.IsNight,
                            //       item.IsNoon,
                            //       item.MorningTime,
                            //       item.NightTime,
                            //       item.NoonTime,
                            //       item.StartDate,
                            //       item.NoofDays,
                            //       item.Description,
                            //       item.IsEmptyStomach,
                            //       item.IsAfterFood,
                            //       item.IsBeforeFood,
                            //       item.Frequency,
                            //       item.ModeOfAdministrationID,
                            //       GeneralBO.CreatedUserID,
                            //       GeneralBO.FinYear,
                            //       GeneralBO.ApplicationID,
                            //       GeneralBO.LocationID,
                            //       PatientMedicineID
                            //        );
                            //}
                            int PatientMedicinesID;
                            PatientMedicinesID = Convert.ToInt32(PatientMedicineID.Value);

                            //if (item.PatientMedicineID == 0)
                            //{
                            //    PatientMedicinesID = Convert.ToInt32(PatientMedicineID.Value);
                            foreach (var items in MedicinesList.Where(x => x.GroupID == item.GroupID))
                            {
                                dbEntity.SpCreatePatientMedicinesItems(
                                     PatientMedicinesID,
                                     items.MedicineID,
                                     items.Quantity,
                                     items.UnitID
                                    );
                            }
                            //}
                            //else
                            //{
                            //PatientMedicinesID = item.PatientMedicineID;
                            //foreach (var items in MedicinesList.Where(x => x.PatientMedicinesID == item.PatientMedicineID))
                            //{
                            //    dbEntity.SpCreatePatientMedicinesItems(
                            //         PatientMedicinesID,
                            //         items.MedicineID,
                            //         items.Quantity,
                            //         items.UnitID
                            //        );
                            //}
                            //}

                            //foreach (var items in MedicinesList.Where(x => x.GroupID == item.GroupID && x.PatientMedicinesID==item.PatientMedicineID))
                            //{
                            //    dbEntity.SpCreatePatientMedicinesItems(
                            //         PatientMedicinesID,
                            //         items.MedicineID,
                            //         items.Quantity,
                            //         items.UnitID
                            //        );
                            //}
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
                        using (SalesEntities dEntity = new SalesEntities())
                        {
                            dEntity.SpCreateDirectSalesOrder(
                                 diagnosis.PatientID,
                                 AppointmentProcess,
                                 GeneralBO.CreatedUserID,
                                 GeneralBO.FinYear,
                                 GeneralBO.LocationID,
                                 GeneralBO.ApplicationID
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
                }
                catch (Exception e)
                {
                    throw e;
                }
                return 1;
            }
        }

        public List<ExaminationBO> GetExaminationList(int ID, int OPID)
        {
            try
            {
                List<ExaminationBO> patient = new List<ExaminationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    patient = dbEntity.SpGetExaminationDetailsByID(ID, OPID, GeneralBO.ApplicationID).Select(a => new ExaminationBO()
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

        public List<PatientDiagnosisBO> GetDateListByID(int ID, int OPID)
        {
            List<PatientDiagnosisBO> item = new List<PatientDiagnosisBO>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    item = dbEntity.SpGetAppointmentDateByID(ID, OPID, GeneralBO.CreatedUserID).Select(a => new PatientDiagnosisBO
                    {
                        Date = (DateTime)a.Date

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

        public PatientDiagnosisBO GetVitalChart(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            try
            {
                PatientDiagnosisBO vitalchart = new PatientDiagnosisBO();

                using (AHCMSEntities dEntity = new AHCMSEntities())
                {
                    return dEntity.SpGetVitalChartByAppointmentProcessID(AppointmentProcessID, PatientID, FromDate).Select(a => new PatientDiagnosisBO
                    {
                        BP = a.BP,
                        Pulse = a.Pulse,
                        Temperature = a.Temperature,
                        HR = a.HR,
                        RR = a.RR,
                        Height = a.Height,
                        Weight = a.Weight,
                        Others = a.Others,
                        BMI = (decimal)a.BMI,
                        RespiratoryRate = a.RespiratoryRate,
                        Unit = a.Unit

                    }).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TreatmentBO> GetTreatmentListByID(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetTreatmentListByID(PatientID, FromDate, AppointmentProcessID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new TreatmentBO()
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
                        NoonTimeID = Convert.ToString(a.NoonID),
                        NightTimeID = Convert.ToString(a.NightID),
                        EveningTimeID = Convert.ToString(a.EveningID),

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ReportBO> GetReportListByID(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetReportListByID(PatientID, FromDate, AppointmentProcessID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ReportBO()
                    {
                        Name = a.Name,
                        DocumentID = (int)a.DocumentID,
                        Description = a.Description,
                        Date = (DateTime)a.Date                   
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ReportBO> GetReportListByIDV5(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetReportListByIDv5(PatientID, FromDate, AppointmentProcessID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ReportBO()
                    {
                        Name = a.Name,
                        DocumentID = (int)a.DocumentID,
                        Description = a.Description,
                        Date = (DateTime)a.Date,
                        IsBeforeAdmission=a.IsBeForeAdmission
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TreatmentItemBO> GetTreatmentMedicineListByID(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetTreatmentMedicneListByID(PatientID, FromDate, AppointmentProcessID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new TreatmentItemBO()
                    {
                        TreatmentID = a.TreatmentID,
                        MedicineID = (int)a.Medicines,
                        Medicine = a.ProductionGroup,
                        StandardMedicineQty = a.Quantity,
                        MedicineUnitID=a.UnitID,
                        MedicineUnit = a.Unit
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DoctorListBO> GetDoctorList(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetReferenceDoctorList(PatientID, FromDate, AppointmentProcessID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new DoctorListBO()
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

        public List<MedicineBO> GetMedicineListByID(int OPID, int PatientID, DateTime FromDate)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetMedicneListByID(OPID, PatientID, FromDate, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MedicineBO()
                    {
                        Medicine = a.ProductionGroupName,
                        Prescription = a.Prescription,
                        PrescriptionID = (int)a.PrescriptionID,
                        TransID = a.TransID,
                        PatientMedicinesID = a.ID,
                        Status = a.Status,
                        DoctorName = a.DoctorName,
                        Qty=(a.Quantity),
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

        public PatientDiagnosisBO GetCaseSheet(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            try
            {
                PatientDiagnosisBO casesheet = new PatientDiagnosisBO();

                using (AHCMSEntities dEntity = new AHCMSEntities())
                {
                    return dEntity.SpGetCaseSheetByID(PatientID, FromDate, AppointmentProcessID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PatientDiagnosisBO
                    {
                        Remark = a.Remarks,
                        NextVisitDate = (DateTime)a.NextVisitedDate
                    }).FirstOrDefault();
                    //return dEntity.SpGetCaseSheetByDate(PatientID, FromDate, AppointmentProcessID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PatientDiagnosisBO
                    //{
                    //    Remark = a.Remarks,
                    //    NextVisitDate = (DateTime)a.NextVisitedDate
                    //}).FirstOrDefault();
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
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        GroupName = a.GroupName,
                        Description = a.Description,
                        GeneralOptionID = (int)a.Value,
                        Text = a.Text

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ExaminationBO> GetExamination(int PatientID, DateTime FromDate, int AppointmentProcessID, int ReviewID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetExaminations(PatientID, FromDate, ReviewID, AppointmentProcessID).Select(a => new ExaminationBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        GroupName = a.GroupName,
                        Description = a.Description,
                        GeneralOptionID = (int)a.Value,
                        Text = a.Text

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MedicineBO> GetMedicinePrescriptionByID(int PatientID, DateTime FromDate)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetMedicineDetailsForPrint(PatientID, FromDate, GeneralBO.ApplicationID).Select(a => new MedicineBO()
                    {
                        PrescriptionNo = a.TransNo,
                        Medicine = a.MedicineName,
                        Unit = a.Unit,
                        Quantity = (decimal)a.Quantity,
                        Instructions = a.Instructions

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DatatableResultBO GetTreatmentDetailsListForPrint(string TransNo, string Date, string Patient, string Doctor, string Time, string TokenNo, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetTreatmentDetailsListForPrint(TransNo, Date, Patient, Doctor, Time, TokenNo, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                TransNo = item.TransNo,
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                Patient = item.PatientName,
                                Doctor = item.DoctorName,
                                Time = item.Time,
                                TokenNo = item.TokenNo,
                                AppointmentProcessID = item.AppointmentProcessID,
                                PatientID=item.PatientID

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

        public List<MedicineBO> GetMedicineListItemsByID(int ID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    return dbEntity.SpGetMedicineListByID(ID, GeneralBO.FinYear, GeneralBO.ApplicationID).Select(a => new MedicineBO()
                    {
                        Medicine = a.MedicineName,
                        Quantity = (decimal)a.Quantity,
                        Unit = a.Unit,
                        Instructions = a.Instructions,
                        TransID = (int)a.TransID,
                        Description = a.Description,
                        MorningTime = a.MorningTime,
                        NoonTime = a.NoonTime,
                        EveningTime = a.EveningTime,
                        NightTime = a.NightTime,
                        MedicineTime = a.MedicineTime,
                        StartDate = (DateTime)a.StartDate,
                        EndDate = (DateTime)a.EndDate
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TreatmentBO> GetTreatmentListItemsByID(int ID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    return dbEntity.SpGetTreatmentListItemByID(ID, GeneralBO.FinYear, GeneralBO.ApplicationID).Select(a => new TreatmentBO()
                    {
                        Name = a.TreatmentName,
                        TherapistName = a.Therapist,
                        TreatmentRoomName = a.TreatmentRoom,
                        StartDate = (DateTime)a.StartDate,
                        EndDate = (DateTime)a.EndDate,
                        NoOfTreatment = (int)a.TreatmentNo
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PatientDiagnosisBO> GetPatientDetailsItemsByID(int ID)
        {
            try
            {
                List<PatientDiagnosisBO> patient = new List<PatientDiagnosisBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    patient = dbEntity.SpGetPatientDetailsListByID(ID, GeneralBO.ApplicationID).Select(a => new PatientDiagnosisBO()
                    {
                        PatientID = a.PatientID,
                        HIN = a.HIN,
                        PatientName = a.PatientName,
                        TransNo = a.TransNo,
                        Date = (DateTime)a.Date,
                        Doctor = a.DoctorName,
                        Time = a.Time,
                        TokenNumber = (int)a.TokenNo,
                        AppointmentProcessID = a.AppointmentProcessID,
                        Address = a.Address,
                        Age = (int)a.Age,
                        Gender = a.Gender,
                        Mobile = a.MobileNo,
                        NextVisitDate = (DateTime)a.NextVisitedDate,
                        DoctorID = a.DoctorID
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

        public DatatableResultBO GetIPPatientList(string CodeHint, string NameHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetIPPatientList("", CodeHint, NameHint, DateHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID).ToList();
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

        public List<MedicineBO> GetMedicinesDetails(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetOPMedicineDetailsByID(PatientID, FromDate, AppointmentProcessID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MedicineBO()
                    {
                        MedicineID = (int)a.MedicineID,
                        TransID = (int)a.TransID,
                        Quantity = (decimal)a.Quantity,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
                        Medicine = a.Medicine,
                        PatientMedicinesID = (int)a.TransID
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<MedicineBO> GetAllMedicinesbyProductionGroup(int ProductionGroupID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetAllMedicinesbyProductionGroup(ProductionGroupID,GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MedicineBO()
                    {
                        ItemID=a.ItemID,
                        ItemName=a.ItemName,
                        ProductionGroup=a.ProductionGroup,
                        Stock=(decimal)a.Stock,
                        Unit=a.Unit

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ExaminationBO> GetDashaVidhaPareekhsalist(int PatientID,int AppointmentProcessID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetDashaVidhaPareekhsaByID(PatientID, AppointmentProcessID,  GeneralBO.ApplicationID).Select(a => new ExaminationBO()
                    {
                        GroupName=a.GroupName,
                        Description=a.Description

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<MedicineItemBO> GetMedicinesItemsList(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetOPMedicinesItemsList(PatientID, FromDate, AppointmentProcessID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MedicineItemBO()
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
                        PatientMedicineID = a.ID,
                        ModeOfAdministrationID = a.ModeOfAdministrationID,
                        Frequency = a.Frequency,
                        IsMiddleOfFood = a.IsMiddleOfFood,
                        IsWithFood = a.IsWithFood,
                        IsMultipleTimes=a.IsMultipleTimes,
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

        public int GetAppointmentProcessID(int PatientID, int AppointmentScheduleItemID)
        {

            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                ObjectParameter ID = new ObjectParameter("ID", typeof(int));
                dbEntity.SpGetAppointmentProcessID(PatientID, AppointmentScheduleItemID, GeneralBO.LocationID, ID);

                if (Convert.ToInt32(ID.Value) != 0)
                {
                    return Convert.ToInt32(ID.Value);
                }
                else
                {
                    return 0;
                }

            }

        }

        public List<LabTestItemBO> GetLabItems(int PatientID, int OPID, DateTime FromDate)
        {
            using (AHCMSEntities dEntity = new AHCMSEntities())
            {
                return dEntity.SpGetOPLabAndXrayDetails(PatientID, OPID, FromDate, GeneralBO.CreatedUserID, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new LabTestItemBO
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
        public List<XraysItemBO> GetXrayItems(int PatientID, int OPID, DateTime FromDate)
        {
            using (AHCMSEntities dEntity = new AHCMSEntities())
            {
                return dEntity.SpGetOPXrayDetails(PatientID, OPID, FromDate, GeneralBO.CreatedUserID, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new XraysItemBO
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
        public List<LabTestItemBO> GetCategoryWiseLabItems(string LabTestCategoryID)
        {
            using (AHCMSEntities dEntity = new AHCMSEntities())
            {
                return dEntity.SpGetCategoryWiseLabItems(LabTestCategoryID,GeneralBO.LocationID,GeneralBO.ApplicationID).Select(a => new LabTestItemBO
                {
                    ID = a.ItemID,                   
                    ItemName = a.ItemName,
                    Type=a.Type,
                    Price=a.Price
                }).ToList();

            }
        }

        public bool IsInPatient(int PatientID)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    ObjectParameter IsInPatient = new ObjectParameter("IsInPatient", typeof(bool));
                    dbEntity.SpIsInPatient(PatientID, GeneralBO.LocationID, GeneralBO.ApplicationID, IsInPatient);
                    return Convert.ToBoolean(IsInPatient.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
        }

        public List<MedicineBO> GetMedicines(int PatientID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetMedicines(PatientID, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MedicineBO()
                    {
                        Unit = a.UnitName,
                        Medicine = a.ProductionGroupName,
                        StartDate = (DateTime)a.StartDate,
                        EndDate = (DateTime)a.EndDate,
                        NoofDays = (int)a.NoOfDays,
                        Description = a.Description,
                        Qty = a.Quantity,
                        PatientMedicinesID = a.TransID
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MedicineBO> GetPreviousMedicineListByID(int PatientMedicinesID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetPreviousMedicineListByID(PatientMedicinesID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MedicineBO()
                    {
                        Medicine = a.ProductionGroupName,
                        Prescription = a.Prescription,
                        PrescriptionID = (int)a.PrescriptionID,
                        TransID = a.TransID,
                        PatientMedicinesID = a.ID
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MedicineBO> GetPreviousMedicinesList(int PatientMedicinesID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetPreviousMedicinesList(PatientMedicinesID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MedicineBO()
                    {
                        MedicineID = (int)a.MedicineID,
                        TransID = (int)a.TransID,
                        Quantity = (decimal)a.Quantity,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
                        Medicine = a.Medicine,
                        PatientMedicinesID = (int)a.TransID
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MedicineItemBO> GetPreviousMedicinesItemsList(int PatientMedicinesID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetPreviousMedicinesItemsList(PatientMedicinesID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MedicineItemBO()
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
                        PatientMedicineID = a.ID,
                        ModeOfAdministrationID = a.ModeOfAdministrationID,
                        Frequency = a.Frequency
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ExaminationBO> GetPreviousExamination(int PatientID, int ReviewID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetLatestExaminationDetails(PatientID, ReviewID).Select(a => new ExaminationBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        GroupName = a.GroupName,
                        Description = a.Description,
                        GeneralOptionID = (int)a.Value,
                        Text = a.Text
                        
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DatatableResultBO GetAllLabTestList(string CodeHint, string TypeHint, string ServiceHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetLabTestListItems(CodeHint, TypeHint, ServiceHint, NameHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Code = item.Code,
                                Name = item.Name,
                                GroupName = item.GroupName,
                                ServiceName = item.ProductionGroup,
                                Type= item.Type,
                                Category = item.Category,
                                Price = item.Price
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

        public List<HistoryBO> GetHistoryListByID(int OPID, int PatientID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetHistoryListByID(OPID, PatientID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new HistoryBO()
                    {
                        ParentID = (int)a.ParentID,
                        AppointmentProcessID = (int)a.AppointmentProcessID,
                        IPID = (int)a.IPID,
                        PatientID = (int)a.PatientID,
                        AppointmentType = a.AppointmentType,
                        Disease = a.Disease,
                        ReportedDate = (DateTime)a.ReportedDate,
                        CaseSheet = a.CaseSheet,
                        Remarks = a.Remarks,
                        Doctor = a.Doctor,
                        SuggestedReviewDate = (DateTime)a.SuggestedReviewDate,
                        Patient = a.Patient,
                        TransNo = a.TransNo,
                        DischargedDate= (DateTime)a.DischargeDate,
                        PresentingComplaints = a.PresentingComplaints,
                        Associatedcomplaints = a.Associatedcomplaints,
                        ContemporaryDiagnosis = a.Contemporarydiagnosis,
                        AyurvedicDiagnosis = a.Ayurvedicdiagnosis
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<HistoryBO> GetHistoryByID(int ParentID, int OPID, int PatientID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetHistoryByID(ParentID, OPID, PatientID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new HistoryBO()
                    {
                        ParentID = (int)a.ParentID,
                        AppointmentProcessID = (int)a.AppointmentProcessID,
                        IPID = (int)a.IPID,
                        PatientID = (int)a.PatientID,
                        AppointmentType = a.AppointmentType,
                        Disease = a.Disease,
                        ReportedDate = (DateTime)a.ReportedDate,
                        CaseSheet = a.CaseSheet,
                        Remarks = a.Remarks,
                        Doctor = a.Doctor,
                        SuggestedReviewDate = (DateTime)a.SuggestedReviewDate,
                        VitalChartID = (int)a.VitalChartID,
                        Medicines = a.Medicines,
                        BP = a.BP,
                        Pulse = a.Pulse,
                        Temperature = a.Temperature,
                        HR = a.HR,
                        RR = a.RR,
                        Height = a.Height,
                        Weight = a.Weight,
                        Others = a.Others,
                        Patient = a.Patient,
                        TransNo = a.TransNo,
                        DischargedDate = (DateTime)a.DischargeDate,
                        PresentingComplaints = a.PresentingComplaints,
                        Associatedcomplaints = a.Associatedcomplaints,
                        ContemporaryDiagnosis = a.Contemporarydiagnosis,
                        AyurvedicDiagnosis = a.Ayurvedicdiagnosis,

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MedicineBO> GetMedicinesHistory(int ParentID, int OPID, int IPID, int PatientID, string AppointmentType)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetMedicinesHistory(ParentID, OPID, IPID, PatientID, AppointmentType, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MedicineBO()
                    {
                        ParentID = (int)a.ParentID,
                        AppointmentProcessID = (int)a.AppointmentProcessID,
                        PatientID = (int)a.PatientID,
                        Medicine = a.Medicine,
                        StartDate = (DateTime)a.StartDate,
                        EndDate = (DateTime)a.EndDate,
                        NoofDays = (int)a.NoOfDays,
                        IsNoon = (bool)a.IsNoon,
                        IsEvening = (bool)a.IsEvening,
                        IsNight = (bool)a.IsNight,
                        IsMultipleTimes = (bool)a.IsMultipleTimes,
                        IsEmptyStomach = (bool)a.IsEmptyStomach,
                        IsBeforeFood = (bool)a.IsBeforeFood,
                        IsAfterFood = (bool)a.IsAfterFood,
                        MorningTime = a.MorningTime,
                        NoonTime = a.NoonTime,
                        EveningTime = a.EveningTime,
                        Description=a.Description
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TreatmentBO> GetTreatmentHistory(int ParentID, int OPID, int IPID, int PatientID, string AppointmentType)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetTreatmentHistory(ParentID, OPID, IPID, PatientID, AppointmentType, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new TreatmentBO()
                    {
                        ParentID = (int)a.ParentID,
                        AppointmentProcessID = (int)a.AppointmentProcessID,
                        PatientID = (int)a.PatientID,
                        Medicine = a.Medicine,
                        Name = a.Treatment,
                        TherapistName = a.Therapist,
                        TreatmentRoomName = a.TreatmentRoom,
                        StartDate = (DateTime)a.StartDate,
                        EndDate = (DateTime)a.EndDate,
                        NoofDays = (int)a.NoOfDays,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VitalChartBO> GetVitalChartHistory(int ParentID, int OPID, int IPID, int PatientID, string AppointmentType)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetVitalChartHistory(ParentID, OPID, IPID, PatientID, AppointmentType, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new VitalChartBO()
                    {
                        ParentID = (int)a.ParentID,
                        AppointmentProcessID = (int)a.AppointmentProcessID,
                        PatientID = (int)a.PatientID,
                        AppointmentType = a.AppointmentType,
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RoundsBO> GetRoundsHistory(int ParentID, int OPID, int IPID, int PatientID, string AppointmentType)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetRoundsHistory(ParentID, OPID, IPID, PatientID, AppointmentType, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new RoundsBO()
                    {
                        ParentID = (int)a.ParentID,
                        AppointmentProcessID = (int)a.AppointmentProcessID,
                        PatientID = (int)a.PatientID,
                        AppointmentType = a.AppointmentType,
                        RoundsDate = (DateTime)a.RoundsDate,
                        Remarks = a.Remarks
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PatientDiagnosisBO GetDepartmentItems(int AppointmentScheduleItemID)
        {
            try
            {
                PatientDiagnosisBO department = new PatientDiagnosisBO();

                using (AHCMSEntities dEntity = new AHCMSEntities())
                {
                    return dEntity.SpGetPatientDepartmentDetails(AppointmentScheduleItemID).Select(a => new PatientDiagnosisBO
                    {
                        PatientID = (int)a.PatientID,
                        PatientName = a.PatientName,
                        DepartmentID = (int)a.DepartmentID,
                        DepartmentName = a.DepartmentName,
                        Date = (DateTime)a.Date
                    }).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int EditDepartment(PatientDiagnosisBO EditDepartment)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpUpdateDepartment(EditDepartment.DepartmentID, EditDepartment.AppointmentScheduleItemID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ExaminationBO> GetBaseLineInformationDetails(int ID, int OPID)
        {
            try
            {
                List<ExaminationBO> patient = new List<ExaminationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    patient = dbEntity.SpGetBaseLineInformationDetails(ID, OPID, GeneralBO.ApplicationID).Select(a => new ExaminationBO()
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

        public List<ExaminationBO> GetBaseLineInformationList(int ID, int OPID)
        {
            try
            {
                List<ExaminationBO> patient = new List<ExaminationBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    patient = dbEntity.SpGetBaseLineInformationDetailsByID(ID, OPID, GeneralBO.ApplicationID).Select(a => new ExaminationBO()
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

        public bool IsPatientHistory(int AppointmentProcessID, int PatientID)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    ObjectParameter IsPatientHistory = new ObjectParameter("IsPatientHistory", typeof(bool));
                    dbEntity.SpIsCaseSheetHistory(PatientID, AppointmentProcessID, IsPatientHistory);
                    return Convert.ToBoolean(IsPatientHistory.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool StopMedicine(int PatientMedicinesID)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    ObjectParameter IsCancelled = new ObjectParameter("IsCancelled", typeof(bool));
                    dbEntity.SpStopMedicine(PatientMedicinesID);
                    return ((Convert.ToBoolean(IsCancelled.Value)));
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

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

        public int SaveV2(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList, List<BaseLineBO> RogaPareekshaItems, List<BaseLineBO> CaseSheetItems, List<ExaminationBO> RogaNirnayamItem, List<QuestionnaireBO> QuestionnaireItems)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    ObjectParameter AppointmentProcessID = new ObjectParameter("TransID", typeof(int));

                    if (diagnosis.AppointmentProcessID == 0)
                    {
                        dbEntity.SpCreateAppointmentProcess(
                                   diagnosis.Date,
                                   diagnosis.PatientID,
                                   diagnosis.AppointmentScheduleItemID,
                                   diagnosis.AppointmentType,
                                   diagnosis.VisitType,
                                   diagnosis.IsCompleted,
                                   diagnosis.IsReferedIP,
                                   diagnosis.IswalkIn,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   AppointmentProcessID
                                    );
                    }
                    else
                    {
                        dbEntity.SpUpdateAppointmentProcess(
                               diagnosis.AppointmentProcessID,
                               diagnosis.PatientID,
                               diagnosis.Date,
                               diagnosis.VisitType,
                               diagnosis.IsCompleted,
                               diagnosis.IsReferedIP,
                               diagnosis.IswalkIn,
                               diagnosis.ParentID,
                               diagnosis.ReviewID,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID
                                );
                    }
                    if (Items != null)
                    {
                        foreach (var item in Items)
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

                    if (DoctorList != null)
                    {
                        foreach (var item in DoctorList)
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

                            dbEntity.SpCreateDoctorList(
                               OPID,
                               item.DoctorNameID,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID
                                );
                        }
                    }

                    //dbEntity.SpDeleteAllBaseLineItems(
                    //     diagnosis.AppointmentProcessID,
                    //     diagnosis.PatientID,
                    //     GeneralBO.ApplicationID,
                    //     GeneralBO.LocationID
                    //     );
                    if (BaseLineItems != null)
                    {
                        foreach (var item in BaseLineItems)
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

                            dbEntity.SpCreateBaseLineInformation(

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


                    dbEntity.SpDeleteVitalchartByID(
                          diagnosis.AppointmentProcessID,
                          diagnosis.PatientID,
                          GeneralBO.ApplicationID,
                          GeneralBO.LocationID
                          );

                    if (VitalChartItems != null)
                    {
                        foreach (var item in VitalChartItems)
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
                            dbEntity.SpCreateVitalChartForPatientDiagnosis(
                            OPID,
                            diagnosis.PatientID,
                            diagnosis.Date,
                            item.BP,
                            item.Pulse,
                            item.Temperature,
                            item.HR,
                            item.RR,
                            item.Height,
                            item.Weight,
                            item.Others,
                            item.Unit,
                            item.BMI,
                            item.RespiratoryRate,
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
                            int ProcessID;
                            if (diagnosis.AppointmentProcessID == 0)
                            {
                                ProcessID = Convert.ToInt32(AppointmentProcessID.Value);
                            }
                            else
                            {
                                ProcessID = diagnosis.AppointmentProcessID;
                            }
                            dbEntity.SpCreatePatientReports(
                               ProcessID,
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

                    ObjectParameter TransID = new ObjectParameter("TransID", typeof(int));
                    if (Treatments != null)
                    {
                        foreach (var item in Treatments)
                        {
                            int ProcessID;
                            if (diagnosis.AppointmentProcessID == 0)
                            {
                                ProcessID = Convert.ToInt32(AppointmentProcessID.Value);
                            }
                            else
                            {
                                ProcessID = diagnosis.AppointmentProcessID;
                            }
                            //if (item.PatientTreatmentID == 0)
                            //{
                            dbEntity.SpCreatePatientTreatments(
                               ProcessID,
                               diagnosis.PatientID,
                               diagnosis.Date,
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
                               item.NoofDays,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID,
                               TransID
                                );

                            int PatientTreatmentID;
                            PatientTreatmentID = Convert.ToInt32(TransID.Value);
                            foreach (var items in TreatmentMedicines.Where(x => x.TreatmentID == item.TreatmentID))
                            {
                                dbEntity.SpCreatePatientTreatmentMedicines(
                                PatientTreatmentID,
                                items.MedicineID,
                                items.StandardMedicineQty,
                                items.MedicineUnitID
                               );
                            }
                            //}
                            //else
                            //{
                            //    dbEntity.SpUpdatePatientTreatments(
                            //       diagnosis.AppointmentProcessID,
                            //       diagnosis.PatientID,
                            //       item.PatientTreatmentID,
                            //       item.TreatmentID,
                            //       item.TherapistID,
                            //       item.TreatmentRoomID,
                            //       item.TreatmentNo,
                            //       item.Instructions,
                            //       item.StartDate,
                            //       item.EndDate,
                            //       GeneralBO.CreatedUserID,
                            //       GeneralBO.FinYear,
                            //       GeneralBO.ApplicationID,
                            //       GeneralBO.LocationID
                            //        );
                            //}
                        }
                    }

                    ObjectParameter PatientMedicineID = new ObjectParameter("TransID", typeof(int));
                    if (MedicinesItemsList != null)
                    {
                        foreach (var item in MedicinesItemsList)
                        {
                            int ProcessID;
                            if (diagnosis.AppointmentProcessID == 0)
                            {
                                ProcessID = Convert.ToInt32(AppointmentProcessID.Value);
                            }
                            else
                            {
                                ProcessID = diagnosis.AppointmentProcessID;
                            }
                            //if (item.PatientMedicineID == 0)
                            //{
                            dbEntity.SpCreatePatientMedicines(
                               ProcessID,
                               diagnosis.PatientID,
                               diagnosis.Date,
                               item.EndDate,
                               item.PatientMedicineID,
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
                               item.IsMultipleTimes,
                               item.MedicineInstruction,
                               item.QuantityInstruction,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID,
                               PatientMedicineID
                                );
                            //}

                            //else
                            //{
                            //    dbEntity.SpUpdatePatientMedicines(
                            //       diagnosis.AppointmentProcessID,
                            //       item.PatientMedicineID,
                            //       item.EndDate,
                            //       item.EveningTime,
                            //       item.InstructionsID,
                            //       item.Isevening,
                            //       item.IsMorning,
                            //       item.IsNight,
                            //       item.IsNoon,
                            //       item.MorningTime,
                            //       item.NightTime,
                            //       item.NoonTime,
                            //       item.StartDate,
                            //       item.NoofDays,
                            //       item.Description,
                            //       item.IsEmptyStomach,
                            //       item.IsAfterFood,
                            //       item.IsBeforeFood,
                            //       item.Frequency,
                            //       item.ModeOfAdministrationID,
                            //       GeneralBO.CreatedUserID,
                            //       GeneralBO.FinYear,
                            //       GeneralBO.ApplicationID,
                            //       GeneralBO.LocationID,
                            //       PatientMedicineID
                            //        );
                            //}
                            int PatientMedicinesID;
                            PatientMedicinesID = Convert.ToInt32(PatientMedicineID.Value);

                            //if (item.PatientMedicineID == 0)
                            //{
                            //    PatientMedicinesID = Convert.ToInt32(PatientMedicineID.Value);
                            foreach (var items in MedicinesList.Where(x => x.GroupID == item.GroupID))
                            {
                                dbEntity.SpCreatePatientMedicinesItems(
                                     PatientMedicinesID,
                                     items.MedicineID,
                                     items.Quantity,
                                     items.UnitID
                                    );
                            }
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
                        using (SalesEntities dEntity = new SalesEntities())
                        {
                            dEntity.SpCreateDirectSalesOrder(
                                 diagnosis.PatientID,
                                 AppointmentProcess,
                                 GeneralBO.CreatedUserID,
                                 GeneralBO.FinYear,
                                 GeneralBO.LocationID,
                                 GeneralBO.ApplicationID
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

                    dbEntity.SpDeleteQuestionnaireItems(
                         diagnosis.AppointmentProcessID,
                         diagnosis.PatientID,
                         GeneralBO.ApplicationID,
                         GeneralBO.LocationID
                         );

                    if (QuestionnaireItems != null)
                    {
                        foreach (var item in QuestionnaireItems)
                        {
                            int ProcessID;
                            if (diagnosis.AppointmentProcessID == 0)
                            {
                                ProcessID = Convert.ToInt32(AppointmentProcessID.Value);
                            }
                            else
                            {
                                ProcessID = diagnosis.AppointmentProcessID;
                            }
                            dbEntity.SpCreateQuestionnaireForScreening(
                               diagnosis.PatientID,
                               ProcessID,
                               item.Question,
                               item.Answer,
                               diagnosis.IPID,
                               GeneralBO.CreatedUserID,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID,
                               GeneralBO.FinYear
                                );
                        }
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                return 1;
            }
        }
        public int SaveV5(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<ExaminationBO> NewItems, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList, List<BaseLineBO> RogaPareekshaItems, List<BaseLineBO> CaseSheetItems, List<ExaminationBO> RogaNirnayamItem, List<QuestionnaireBO> QuestionnaireItems)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    ObjectParameter AppointmentProcessID = new ObjectParameter("TransID", typeof(int));

                    if (diagnosis.AppointmentProcessID == 0)
                    {
                        dbEntity.SpCreateAppointmentProcess(
                                   diagnosis.Date,
                                   diagnosis.PatientID,
                                   diagnosis.AppointmentScheduleItemID,
                                   diagnosis.AppointmentType,
                                   diagnosis.VisitType,
                                   diagnosis.IsCompleted,
                                   diagnosis.IsReferedIP,
                                   diagnosis.IswalkIn,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.ApplicationID,
                                   GeneralBO.LocationID,
                                   AppointmentProcessID
                                    );
                    }
                    else
                    {
                        dbEntity.SpUpdateAppointmentProcess(
                               diagnosis.AppointmentProcessID,
                               diagnosis.PatientID,
                               diagnosis.Date,
                               diagnosis.VisitType,
                               diagnosis.IsCompleted,
                               diagnosis.IsReferedIP,
                               diagnosis.IswalkIn,
                               diagnosis.ParentID,
                               diagnosis.ReviewID,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID
                                );
                    }
                   
                    if (Items != null)
                    {
                        foreach (var item in Items)
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
                    dbEntity.SpDeleteDashaVidhaPareekhsaByID(
                         diagnosis.AppointmentProcessID,
                         0,
                         diagnosis.PatientID,
                         GeneralBO.ApplicationID,
                         GeneralBO.LocationID
                         );
                    if (NewItems != null)
                    {
                        foreach (var item in NewItems)
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

                            dbEntity.SpCreateDashaVidhaPareekhsa(
                               OPID,
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
                    if (DoctorList != null)
                    {
                        foreach (var item in DoctorList)
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

                            dbEntity.SpCreateDoctorList(
                               OPID,
                               item.DoctorNameID,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID
                                );
                        }
                    }

                    //dbEntity.SpDeleteAllBaseLineItems(
                    //     diagnosis.AppointmentProcessID,
                    //     diagnosis.PatientID,
                    //     GeneralBO.ApplicationID,
                    //     GeneralBO.LocationID
                    //     );
                    if (BaseLineItems != null)
                    {
                        foreach (var item in BaseLineItems)
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

                            dbEntity.SpCreateBaseLineInformation(

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


                    dbEntity.SpDeleteVitalchartByID(
                          diagnosis.AppointmentProcessID,
                          diagnosis.PatientID,
                          GeneralBO.ApplicationID,
                          GeneralBO.LocationID
                          );

                    if (VitalChartItems != null)
                    {
                        foreach (var item in VitalChartItems)
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
                            dbEntity.SpCreateVitalChartForPatientDiagnosis(
                            OPID,
                            diagnosis.PatientID,
                            diagnosis.Date,
                            item.BP,
                            item.Pulse,
                            item.Temperature,
                            item.HR,
                            item.RR,
                            item.Height,
                            item.Weight,
                            item.Others,
                            item.Unit,
                            item.BMI,
                            item.RespiratoryRate,
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
                            int ProcessID;
                            if (diagnosis.AppointmentProcessID == 0)
                            {
                                ProcessID = Convert.ToInt32(AppointmentProcessID.Value);
                            }
                            else
                            {
                                ProcessID = diagnosis.AppointmentProcessID;
                            }
                            dbEntity.SpCreatePatientReportsv5(
                               ProcessID,
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
                            if (item.ID == 0)
                            {
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
                            }
                        }
                    }

                    ObjectParameter TransID = new ObjectParameter("TransID", typeof(int));
                    if (Treatments != null)
                    {
                        foreach (var item in Treatments)
                        {
                            int ProcessID;
                            if (diagnosis.AppointmentProcessID == 0)
                            {
                                ProcessID = Convert.ToInt32(AppointmentProcessID.Value);
                            }
                            else
                            {
                                ProcessID = diagnosis.AppointmentProcessID;
                            }
                            //if (item.PatientTreatmentID == 0)
                            //{
                            dbEntity.SpCreatePatientTreatments(
                               ProcessID,
                               diagnosis.PatientID,
                               diagnosis.Date,
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
                               item.NoofDays,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID,
                               TransID
                                );

                            int PatientTreatmentID;
                            PatientTreatmentID = Convert.ToInt32(TransID.Value);
                            foreach (var items in TreatmentMedicines.Where(x => x.TreatmentID == item.TreatmentID))
                            {
                                dbEntity.SpCreatePatientTreatmentMedicines(
                                PatientTreatmentID,
                                items.MedicineID,
                                items.StandardMedicineQty,
                                items.MedicineUnitID
                               );
                            }
                            //}
                            //else
                            //{
                            //    dbEntity.SpUpdatePatientTreatments(
                            //       diagnosis.AppointmentProcessID,
                            //       diagnosis.PatientID,
                            //       item.PatientTreatmentID,
                            //       item.TreatmentID,
                            //       item.TherapistID,
                            //       item.TreatmentRoomID,
                            //       item.TreatmentNo,
                            //       item.Instructions,
                            //       item.StartDate,
                            //       item.EndDate,
                            //       GeneralBO.CreatedUserID,
                            //       GeneralBO.FinYear,
                            //       GeneralBO.ApplicationID,
                            //       GeneralBO.LocationID
                            //        );
                            //}
                        }
                    }

                    ObjectParameter PatientMedicineID = new ObjectParameter("TransID", typeof(int));
                    if (MedicinesItemsList != null)
                    {
                        foreach (var item in MedicinesItemsList)
                        {
                            int ProcessID;
                            if (diagnosis.AppointmentProcessID == 0)
                            {
                                ProcessID = Convert.ToInt32(AppointmentProcessID.Value);
                            }
                            else
                            {
                                ProcessID = diagnosis.AppointmentProcessID;
                            }
                            //if (item.PatientMedicineID == 0)
                            //{
                            dbEntity.SpCreatePatientMedicines(
                               ProcessID,
                               diagnosis.PatientID,
                               diagnosis.Date,
                               item.EndDate,
                               item.PatientMedicineID,
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
                               item.IsMultipleTimes,
                               item.MedicineInstruction,
                               item.QuantityInstruction,
                               GeneralBO.CreatedUserID,
                               GeneralBO.FinYear,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID,
                               PatientMedicineID
                                );
                            //}

                            //else
                            //{
                            //    dbEntity.SpUpdatePatientMedicines(
                            //       diagnosis.AppointmentProcessID,
                            //       item.PatientMedicineID,
                            //       item.EndDate,
                            //       item.EveningTime,
                            //       item.InstructionsID,
                            //       item.Isevening,
                            //       item.IsMorning,
                            //       item.IsNight,
                            //       item.IsNoon,
                            //       item.MorningTime,
                            //       item.NightTime,
                            //       item.NoonTime,
                            //       item.StartDate,
                            //       item.NoofDays,
                            //       item.Description,
                            //       item.IsEmptyStomach,
                            //       item.IsAfterFood,
                            //       item.IsBeforeFood,
                            //       item.Frequency,
                            //       item.ModeOfAdministrationID,
                            //       GeneralBO.CreatedUserID,
                            //       GeneralBO.FinYear,
                            //       GeneralBO.ApplicationID,
                            //       GeneralBO.LocationID,
                            //       PatientMedicineID
                            //        );
                            //}
                            int PatientMedicinesID;
                            PatientMedicinesID = Convert.ToInt32(PatientMedicineID.Value);

                            //if (item.PatientMedicineID == 0)
                            //{
                            //    PatientMedicinesID = Convert.ToInt32(PatientMedicineID.Value);
                            foreach (var items in MedicinesList.Where(x => x.GroupID == item.GroupID))
                            {
                                dbEntity.SpCreatePatientMedicinesItems(
                                     PatientMedicinesID,
                                     items.MedicineID,
                                     items.Quantity,
                                     items.UnitID
                                    );
                            }
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
                        using (SalesEntities dEntity = new SalesEntities())
                        {
                            dEntity.SpCreateDirectSalesOrder(
                                 diagnosis.PatientID,
                                 AppointmentProcess,
                                 GeneralBO.CreatedUserID,
                                 GeneralBO.FinYear,
                                 GeneralBO.LocationID,
                                 GeneralBO.ApplicationID
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

                    dbEntity.SpDeleteQuestionnaireItems(
                         diagnosis.AppointmentProcessID,
                         diagnosis.PatientID,
                         GeneralBO.ApplicationID,
                         GeneralBO.LocationID
                         );

                    if (QuestionnaireItems != null)
                    {
                        foreach (var item in QuestionnaireItems)
                        {
                            int ProcessID;
                            if (diagnosis.AppointmentProcessID == 0)
                            {
                                ProcessID = Convert.ToInt32(AppointmentProcessID.Value);
                            }
                            else
                            {
                                ProcessID = diagnosis.AppointmentProcessID;
                            }
                            dbEntity.SpCreateQuestionnaireForScreening(
                               diagnosis.PatientID,
                               ProcessID,
                               item.Question,
                               item.Answer,
                               diagnosis.IPID,
                               GeneralBO.CreatedUserID,
                               GeneralBO.ApplicationID,
                               GeneralBO.LocationID,
                               GeneralBO.FinYear
                                );
                        }
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                return 1;
            }
        }
        public bool CancelPaitentTreatment(int PatientTreatmentID, int TreatmentID, DateTime Date)
        {
            try
            {
               
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    dbEntity.SpCancelPaitentTreatment(PatientTreatmentID, TreatmentID, Date,GeneralBO.ApplicationID );

                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

