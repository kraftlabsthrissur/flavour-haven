using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class GeneralDAL
    {
        public string GetConfig(string ConfigName)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    return dbEntity.SpGetConfig(ConfigName, GeneralBO.LocationID, GeneralBO.ApplicationID).FirstOrDefault().ConfigValue;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ConfigName + " parameter is not set in the configuration for the current location");
            }
        }

        public string PostDummy(string Name)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                     dbEntity.spCreatePostDummy(Name);
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Name + " ABCD");
            }
        }

        public bool GetIsGSTRegisteredLocation(int LocationID)
        {
            ObjectParameter IsGSTRegisteredLocation = new ObjectParameter("IsGSTRegisteredLocation", typeof(bool));
            try
            {

                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SPGetIsGSTRegisteredLocation(LocationID, IsGSTRegisteredLocation);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return Convert.ToBoolean(IsGSTRegisteredLocation.Value);
        }

        public string GetConfig(string ConfigName, int UserID)
        {

            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    return dbEntity.SpGetConfigByUser(ConfigName, UserID, GeneralBO.LocationID, GeneralBO.ApplicationID).FirstOrDefault().ConfigValue;
                }
            }
            catch (Exception e)
            {
                throw new Exception(ConfigName + " parameter is not set in the configuration for the current user");
            }
        }

        public List<ConfigBO> GetGroupConfig(string GroupName, int UserID)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    return dbEntity.SpGetConfigGroup(GroupName, UserID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ConfigBO()
                    {
                        Name = a.ConfigName,
                        StringValue = a.ConfigValue,
                        Description = a.Description
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public FinYearBO GetCurrentFinYear(int ApplicationID)
        {

            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    return dbEntity.SpGetCurrentFinYear(ApplicationID).Select(a => new FinYearBO()
                    {
                        FinancialYearID = a.FinancialYearID,
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        Title = a.Title,
                        Year = a.Year,
                    }).FirstOrDefault();

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetStateIDByLocation()
        {

            try
            {
                ObjectParameter ID = new ObjectParameter("ID", typeof(int));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpGetStateByLocation(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, ID);
                    return Convert.ToInt32(ID.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public bool IsCodeAlreadyExists(string Table, string Field, string Code, int ID = 0)
        {
            try
            {
                ObjectParameter IsExists = new ObjectParameter("IsExists", typeof(int));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpIsCodeAlreadyExists(Table, Field, Code, ID, IsExists);
                    if (Convert.ToInt16(IsExists.Value) == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public void InsertActions(string XMLActions)
        {
            using (AyurwareEntities dbEntity = new AyurwareEntities())
            {
                try
                {
                    dbEntity.SpInsertActions(XMLActions);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void InsertActions(List<ActionBO> Actions)
        {

        }

        public string GetSerialNo(string Form, string Field)
        {
            string SerialNo = "";
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    List<string> items = dbEntity.SpSerialNoGenerator(Form, Field, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    SerialNo = items[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Serial no properties are not found for the form " + Form);
            }
            return SerialNo;
        }
        public string UpdateSerialNo(string Form, string Field)
        {

            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                    dbEntity.SpUpdateSerialNo(Form, Field, GeneralBO.FinYear, 0, GeneralBO.ApplicationID, SerialNo);
                    return SerialNo.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void LogError(string Area, string Controller, string Action, int ID, string Message, string StackTrace, string InnerException)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    dbEntity.SpLogError(Area, Controller, Action, ID, Message, StackTrace, InnerException, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
               
            }
        }

        public List<ModeOfTransportBO> GetModeOfTransport()
        {
            using (MasterEntities dbEntity = new DBContext.MasterEntities())
            {
                return dbEntity.SpGetModeOfTransport().Select(a => new ModeOfTransportBO()
                {
                    ID = a.ID,
                    Name = a.Name
                }).ToList();
            }
        }

        public int Cancel(int ID, string Table)
        {
            MasterEntities dEntity = new MasterEntities();
            return dEntity.SpCancelTransaction(ID, Table, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
        }

        public List<MenuItemBO> GetMenuItems()
        {
            using (AyurwareEntities dbEntity = new AyurwareEntities())
            {
                try
                {
                    return dbEntity.SpGetMenuItems(GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(
                    a => new MenuItemBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        ActionID = a.ActionID,
                        ParentID = a.ParentID,
                        SortOrder = a.SortOrder,
                        IconClass = a.IconClass,
                        BaseParentID = a.BaseParentID,
                        Area = a.Area,
                        Controller = a.Controller,
                        Action = a.Action,
                        URL = a.URL,
                        ReportURL = a.ReportURL
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool IsAuthorised(string Area, string Controller, string Action, string Tab, string Type)
        {
            using (AyurwareEntities dbEntity = new AyurwareEntities())
            {
                try
                {
                    ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(bool));
                    dbEntity.SpIsAuthorised(GeneralBO.CreatedUserID, Area, Controller, Action, Tab, Type, GeneralBO.LocationID, GeneralBO.ApplicationID, ReturnValue);
                    return Convert.ToBoolean(ReturnValue.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<ActionBO> GetRolePrivileges()
        {
            using (AyurwareEntities dbEntity = new AyurwareEntities())
            {
                try
                {
                    return dbEntity.SpGetRolePrivileges(GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(
                    a => new ActionBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Area = a.Area,
                        Controller = a.Controller,
                        Action = a.Action,
                        ReturnType = a.ReturnType,
                        Key = a.Key,
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
        }

        public void LogPerformance(string Area, string Controller, string Action, string ID, int Duration)
        {
            using (AyurwareEntities dbEntity = new AyurwareEntities())
            {
                try
                {
                    dbEntity.SpLogPerformance(Area, Controller, Action, ID, Duration, GeneralBO.CreatedUserID, GeneralBO.ApplicationID);
                }
                catch (Exception e)
                {

                }
            }
        }

        public int[] GetUsersWithRole(string RoleName)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    return dbEntity.SpGetUsersWithRole(RoleName, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => a.UserID).ToArray();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int[] GetUsersWithPermission(string Area, string Controller, string Action, string Type)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    return dbEntity.SpGetUsersWithPermission(Area, Controller, Action, Type, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => a.UserID).ToArray();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool IsRoleExistForUser(string RoleName)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(bool));
                    dbEntity.SpIsRoleExistForUser(RoleName, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID, ReturnValue);
                    return Convert.ToBoolean(ReturnValue.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetPrintList(DateTime DateFrom,string TransNo, string Form, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {

                    var result = dbEntity.SpGetPrintList(DateFrom,TransNo, Form, SortField, SortOrder, Offset, Limit, GeneralBO.CreatedUserID,GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Form = item.form.Trim(),
                                CreatedDate = ((DateTime)item.Createddate).ToString("dd-MMM-yyyy"),
                                ID = item.ID,
                                Area = item.Area
                               
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public List<MailBO> GetMailSenderDetails(string key)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    return dbEntity.SpGetMailSenderDetails(key).Select(k => new MailBO
                    {
                        SenderMailID=k.SenderMailID,
                        SenderMailPassword=k.SenderMailPassword

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<MailBO> GetMailReceiverDetails(string UI,int ID)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    return dbEntity.SpGetMailReceiverDetails(UI,ID).Select(k => new MailBO
                    {
                        ReceiverMailID = k.Email,
                        IsMailSent =(bool) k.IsMailSent

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateMailSent(string UI, int ID)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    dbEntity.SpUpDateMailSentDetails(UI, ID);
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
