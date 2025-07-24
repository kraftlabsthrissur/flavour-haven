using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using BusinessObject;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class GeneralBL : IGeneralContract
    {
        GeneralDAL generalDAL;

        public GeneralBL()
        {
            generalDAL = new GeneralDAL();
        }

        public string GetConfig(string ConfigName)
        {
            return generalDAL.GetConfig(ConfigName);
        }

        public string PostDummy(string Name)
        {
             return generalDAL.PostDummy(Name);
            
        }

        public bool IsGSTRegisteredLocation(int LocationID)
        {
            return generalDAL.GetIsGSTRegisteredLocation(LocationID);
        }

        public string GetConfig(string ConfigName, int UserID)
        {
            return generalDAL.GetConfig(ConfigName, UserID);
        }

        public FinYearBO GetCurrentFinYear(int ApplicationID)
        {
            return generalDAL.GetCurrentFinYear(ApplicationID);
        }

        public int GetStateIDByLocation()
        {
            return generalDAL.GetStateIDByLocation();
        }

        /**
        IDictionary<int, string> dict = new Dictionary<int, string>();
        dict.Add(0, "SONo");
        dict.Add(1, "CustomerName");
        dict.Add(2, "CustomerID");
        dict.Add(3, "CustomerCategory");

        SalesOrderModel SalesOrder = new SalesOrderModel();

        try
        {
	        List<SalesOrderModel> SalesOrders = generalBL.ReadExcel("C:\\Users\\Ajith\\Source\\Workspaces\\Ayurware\\TradeSuiteApp.Web\\Uploads\\new.xlsx", SalesOrder, dict);
            }
        catch (Exception e){
	        ViewBag.ErrorMessage = e.Message;
        }
        */

        public List<T> ReadExcel<T>(string FilePath, T DynamicObject, IDictionary<int, string> Dictionary)
        {
            List<T> records = new List<T>();

            string connString;
            string Extention = Path.GetExtension(FilePath);
            if (Extention == ".xlsx")
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
            }
            else
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            }
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();

            try
            {
                oledbConn.Open();

                var DbSchema = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                string SheetName = DbSchema.Rows[0]["TABLE_NAME"].ToString();
                string Query = String.Format("select * from [{0}]", SheetName);
                using (OleDbCommand cmd = new OleDbCommand(Query, oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);
                    dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == "")
                        {
                            break;
                        }
                        T NewDynamicObject = (T)Activator.CreateInstance(DynamicObject.GetType());
                        foreach (var item in Dictionary)
                        {
                            try
                            {
                                var Type = NewDynamicObject.GetType().GetProperty(item.Value).PropertyType.Name;
                                switch (Type)
                                {
                                    case "String":
                                        string s;
                                        s = row[item.Key].ToString();
                                        NewDynamicObject.GetType().GetProperty(item.Value).SetValue(NewDynamicObject, s);
                                        break;
                                    case "Int32":
                                    case "Int16":
                                    case "Int":
                                        int i = ToInt(row[item.Key].ToString());
                                        NewDynamicObject.GetType().GetProperty(item.Value).SetValue(NewDynamicObject, i);
                                        break;
                                    case "Boolean":
                                        bool b = ToBoolean(row[item.Key].ToString());
                                        NewDynamicObject.GetType().GetProperty(item.Value).SetValue(NewDynamicObject, b);
                                        break;
                                    case "Decimal":
                                        decimal d = ToDecimal(row[item.Key].ToString());
                                        NewDynamicObject.GetType().GetProperty(item.Value).SetValue(NewDynamicObject, d);
                                        break;
                                    case "DateTime":
                                        if (row[item.Key].ToString() != "1/1/0001 12:00:00 AM" && row[item.Key].ToString() != "")
                                        {
                                            DateTime t = Convert.ToDateTime(row[item.Key].ToString());
                                            NewDynamicObject.GetType().GetProperty(item.Value).SetValue(NewDynamicObject, t);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            catch (FormatException e)
                            {
                                // the FormatException is thrown when the string text does not represent a valid boolean.
                                string a = dt.Columns[1].Caption;
                                throw new FormatException("Invalid value " + row[item.Key].ToString() + " for " + item.Value + ". Make sure the excel is in correct format.");
                            }
                            catch (OverflowException e)
                            {
                                // the OverflowException is thrown when the string is a valid integer, 
                                // but is too large for a 32 bit integer.  Use Convert.ToInt64 in
                                // this case.
                                throw new OverflowException("Value out of bound " + row[item.Key].ToString() + " for " + item.Value);
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                        records.Add(NewDynamicObject);
                    }
                }
                return records;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                oledbConn.Close();
            }
        }

        private decimal ToDecimal(string s)
        {
            try
            {
                return s == "" ? 0 : s.Trim() == "" ? 0 : Convert.ToDecimal(s);
            }
            catch (FormatException e)
            {
                // the FormatException is thrown when the string text does not represent a valid decimal.
                throw e;
            }
        }

        private bool ToBoolean(string s)
        {
            try
            {
                return s == "" ? false : s.Trim() == "" ? false : Convert.ToBoolean(s);
            }
            catch (FormatException e)
            {
                // the FormatException is thrown when the string text does not represent a valid boolean.
                throw e;
            }
        }

        private int ToInt(string s)
        {
            try
            {
                return s == "" ? 0 : s.Trim() == "" ? 0 : Convert.ToInt32(s);
            }
            catch (FormatException e)
            {
                // the FormatException is thrown when the string text does not represent a valid integer.
                throw e;
            }
            catch (OverflowException e)
            {
                // the OverflowException is thrown when the string is a valid integer, 
                // but is too large for a 32 bit integer.  Use Convert.ToInt64 in
                // this case.
                throw e;
            }
        }

        public void InsertActions(List<ActionBO> Actions)
        {
            Actions.Select(a =>
            {
                a.Controller = a.Controller.Replace("Controller", "");
                a.Key = a.Area + "_" + a.Controller + "_" + a.Action;

                switch (a.Action)
                {
                    case "Index":
                    case "index":
                        a.Name = "View list of " + this.Split(a.Controller);
                        break;
                    case "Details":
                    case "details":
                        a.Name = "View details of " + this.Split(a.Controller);
                        break;
                    default:
                        if (a.Area == "Reports" && a.ReturnType == "ActionResult")
                        {
                            a.Name = "View " + Split(a.Action) + " Report";
                        }
                        else
                        {
                            a.Name = Split(a.Action) + " " + this.Split(a.Controller);
                        }
                        break;
                }

                return a;
            }).ToList();

            string XMLActions = XMLHelper.Serialize(Actions);
            generalDAL.InsertActions(XMLActions);
        }

        private string Split(string s)
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            return r.Replace(s, " ");
        }

        public string NumberToText(int n)
        {
            if (n < 0)
                return "Minus " + NumberToText(-n);
            else if (n == 0)
                return "";
            else if (n <= 19)
                return new string[] {"One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight",
                "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
                "Seventeen", "Eighteen", "Nineteen"}[n - 1] + " ";
            else if (n <= 99)
                return new string[] {"Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy",
                  "Eighty", "Ninety"}[n / 10 - 2] + " " + NumberToText(n % 10);
            else if (n <= 199)
                return "One Hundred " + NumberToText(n % 100);
            else if (n <= 999)
                return NumberToText(n / 100) + "Hundred " + NumberToText(n % 100);
            else if (n <= 1999)
                return "One Thousand " + NumberToText(n % 1000);
            else if (n <= 99999)
                return NumberToText(n / 1000) + "Thousand " + NumberToText(n % 1000);
            else if (n <= 199999)
                return "One Lakh " + NumberToText(n % 100000);
            else if (n <= 9999999)
                return NumberToText(n / 100000) + "Lakh " + NumberToText(n % 100000);
            else if (n <= 19999999)
                return "One Million " + NumberToText(n % 1000000);
            else if (n <= 999999999)
                return NumberToText(n / 1000000) + "Million " + NumberToText(n % 1000000);
            else if (n <= 1999999999)
                return "One Billion " + NumberToText(n % 1000000000);
            else
                return NumberToText(n / 1000000000) + "Billion " + NumberToText(n % 1000000000);
        }

        public string GetSerialNo(string Form, string Field)
        {
            return generalDAL.GetSerialNo(Form, Field);
        }

        public string UpdateSerialNo(string Form, string Field)
        {
            return generalDAL.UpdateSerialNo(Form, Field);
        }

        public void LogError(string Area, string Controller, string Action, int ID, string Message, string StackTrace, string InnerException)
        {
            generalDAL.LogError(Area, Controller, Action, ID, Message, StackTrace, InnerException);
        }

        public void LogError(string Area, string Controller, string Action, int ID, Exception e)
        {
            string Message = e.Message;
            string StackTrace = e.StackTrace;
            string InnerException = e.InnerException?.ToString() ?? "";

            generalDAL.LogError(Area, Controller, Action, ID, Message, StackTrace, InnerException);
        }

        public List<ModeOfTransportBO> GetModeOfTransport()
        {
            return generalDAL.GetModeOfTransport();
        }

        public int Cancel(int ID, string Table)
        {
            return generalDAL.Cancel(ID, Table);
        }

        public List<MenuItemBO> GetMenuItems()
        {
            return generalDAL.GetMenuItems();
        }

        public List<ActionBO> GetRolePrivileges()
        {
            return generalDAL.GetRolePrivileges();
        }

        public List<ConfigBO> GetGroupConfig(string GroupName, int UserID)
        {
            List<ConfigBO> UserPref = generalDAL.GetGroupConfig(GroupName, UserID);
            List<ConfigBO> AppPref = generalDAL.GetGroupConfig(GroupName, 0);

            return AppPref.Concat(UserPref).ToLookup(a => a.Name).Select(g => g.Aggregate((a1, a2) => new ConfigBO()
            {
                Name = a1.Name,
                Description = a1.Description,
                StringValue = a2.StringValue ?? a1.StringValue

            })).ToList();
        }

        public void LogPerformance(string Area, string Controller, string Action, string ID, int Duration)
        {
            generalDAL.LogPerformance(Area, Controller, Action, ID, Duration);
        }

        public bool IsAuthorised(string Area, string Controller, string Action, string Tab, string Type)
        {
            return generalDAL.IsAuthorised(Area, Controller, Action, Tab, Type);
        }

        public int[] GetUsersWithRole(string RoleName)
        {
            return generalDAL.GetUsersWithRole(RoleName);
        }

        public bool IsRoleExistForUser(string RoleName)
        {
            return generalDAL.IsRoleExistForUser(RoleName);
        }

        public int[] GetUsersWithPermission(string Area, string Controller, string Action, string Type)
        {
            return generalDAL.GetUsersWithPermission(Area, Controller, Action, Type);
        }

        public DatatableResultBO GetPrintList(DateTime DateFrom, string TransNo, string Form, string SortField, string SortOrder, int Offset, int Limit)
        {
            return generalDAL.GetPrintList(DateFrom, TransNo, Form, SortField, SortOrder, Offset, Limit);
        }
        public List<MailBO> GetMailSenderDetails(string key)
        {
            return generalDAL.GetMailSenderDetails(key);
        }

        public List<MailBO> GetMailReceiverDetails(string UI, int ID)
        {
            return generalDAL.GetMailReceiverDetails(UI, ID);
        }
      public   bool UpdateMailSent(string UI, int ID)
        {
            return generalDAL.UpdateMailSent(UI, ID);
        }

    }
}
