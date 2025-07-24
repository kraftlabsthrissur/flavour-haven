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
   public class ScreeningDAL
    {
        public DatatableResultBO GetOpPatientList(string Type, string CodeHint, string NameHint, string TimeHint, string TokenHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetOpPatientList(Type, CodeHint, NameHint, TimeHint, TokenHint, DateHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID).ToList();
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
                                IsReferedIP = item.IsReferedIP
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

        public int Save(PatientDiagnosisBO diagnosis,List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    ObjectParameter AppointmentProcessID = new ObjectParameter("TransID", typeof(int));
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
                }
                catch (Exception e)
                {
                    throw e;
                }
                return 1;
            }
        }

        public PatientDiagnosisBO GetVitalChart(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            try
            {
                PatientDiagnosisBO vitalchart = new PatientDiagnosisBO();

                using (AHCMSEntities dEntity = new AHCMSEntities())
                {
                    return dEntity.SpGetScreeningVitalChart(AppointmentProcessID, PatientID, FromDate).Select(a => new PatientDiagnosisBO
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

        public List<PatientDiagnosisBO> GetDateListByID(int ID, int OPID)
        {
            List<PatientDiagnosisBO> item = new List<PatientDiagnosisBO>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    item = dbEntity.SpGetScreeningAppointmentDateByID(ID, OPID, GeneralBO.CreatedUserID).Select(a => new PatientDiagnosisBO
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

        public int SaveV2(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<QuestionnaireBO> QuestionnaireItems)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    ObjectParameter AppointmentProcessID = new ObjectParameter("TransID", typeof(int));
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

                }
                catch (Exception e)
                {
                    throw e;
                }
                return 1;
            }
        }
        public List<QuestionnaireBO> GetQuestionnaireAndAnswers(int PatientID, int AppointmentProcessID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetQuestionsAndAnswersForScreening(PatientID, AppointmentProcessID, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new QuestionnaireBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        GroupName = a.Question,
                        IsChecked = Convert.ToBoolean(a.CheckedValue),
                        IsParent = (bool)a.IsParent,
                        Description = a.Description
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<QuestionnaireBO> GetQuestionnaireList(int PatientID, int AppointmentProcessID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetQuestionsForScreening(PatientID, AppointmentProcessID, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new QuestionnaireBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        GroupName = a.Question,
                        IsChecked = Convert.ToBoolean(a.CheckedValue),
                        IsParent = (bool)a.IsParent,
                        Description = a.Description
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QuestionnaireBO IsPatientExists(int OPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpCheckIsPatientExists(OPID,GeneralBO.LocationID,GeneralBO.ApplicationID,GeneralBO.FinYear).Select(a => new QuestionnaireBO()
                    {
                        IsExists=Convert.ToBoolean(a.PatientExist)
                    }
                    ).FirstOrDefault();


                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
