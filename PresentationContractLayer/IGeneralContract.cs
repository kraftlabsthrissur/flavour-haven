using BusinessObject;
using System;
using System.Collections.Generic;

namespace PresentationContractLayer
{
    public interface IGeneralContract
    {
        string GetConfig(string ConfigName);

        string PostDummy(string Name);

        string GetConfig(string ConfigName, int UserID);

        List<ConfigBO> GetGroupConfig(string GroupName, int UserID);

        FinYearBO GetCurrentFinYear(int ApplicationID);

        int GetStateIDByLocation();

        string GetSerialNo(string Form, string Field);

        string UpdateSerialNo(string Form, string Field);

        List<T> ReadExcel<T>(string FilePath, T Object, IDictionary<int, string> dict);

        void InsertActions(List<ActionBO> Actions);

        void LogError(string Area, string Controller, string Action, int ID, string Message, string StackTrace, string InnerException);

        void LogError(string Area, string Controller, string Action, int ID, Exception e);

        List<ModeOfTransportBO> GetModeOfTransport();
         
        int Cancel(int ID, string Table);

        List<MenuItemBO> GetMenuItems();

        List<ActionBO> GetRolePrivileges();

        bool IsAuthorised(string Area, string Controller, string Action, string Tab, string Type);

        void LogPerformance(string Area, string Controller, string Action, string ID, int Duration);

        string NumberToText(int n);

        int[] GetUsersWithRole(string RoleName);

        int[] GetUsersWithPermission(string Area, string Controller, string Action, string Type);

        bool IsRoleExistForUser(string RoleName);

        bool IsGSTRegisteredLocation(int LocationID);

        DatatableResultBO GetPrintList(DateTime DateFrom, string TransNo, string Form, string SortField, string SortOrder, int Offset, int Limit);
        List<MailBO> GetMailSenderDetails(string key);
        List<MailBO> GetMailReceiverDetails(string UI,int ID);
        bool UpdateMailSent(string UI,int ID);

    }
}
