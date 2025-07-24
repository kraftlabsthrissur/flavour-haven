using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Models
{
    public class ManageRolePermissionViewModel
    {
        public int RoleID { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public List<RolePermissionViewModel> RolePermissionViewModelList { get; set; }
    }

    public class RolePermissionViewModel
    {
        public int RoleModuleID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public int ModuleID { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Key { get; set; }
        public bool IsHaveAccess { get; set; }
    }

    public static partial class Mapper
    {
        public static List<RolePermissionViewModel> MapToModelList(this List<RolePermissionBO> boList)
        {
            if (boList != null)
                return boList.Select(x => x.MapToModel()).ToList();
            else
                return new List<RolePermissionViewModel>();
        }
        public static RolePermissionViewModel MapToModel(this RolePermissionBO bo)
        {
            if (bo != null)
                return new RolePermissionViewModel()
                {
                    RoleModuleID = bo.RoleModuleID,
                    RoleID = bo.RoleID,
                    RoleName = bo.RoleName,
                    ModuleID = bo.ModuleID,
                    Area = bo.Area,
                    Controller = bo.Controller,
                    Action = bo.Action,
                    Key = bo.Key,
                    IsHaveAccess = bo.IsHaveAccess
                };
            else
                return new RolePermissionViewModel();
        }

        public static List<RolePermissionBO> MapToBOList(this List<RolePermissionViewModel> modelList)
        {
            return (from m in modelList
                    select new RolePermissionBO
                    {
                        RoleModuleID = m.RoleModuleID,
                        RoleID = m.RoleID,
                        RoleName = m.RoleName,
                        ModuleID = m.ModuleID,
                        Area = m.Area,
                        Controller = m.Controller,
                        Action = m.Action,
                        Key = m.Key,
                        IsHaveAccess = m.IsHaveAccess
                    }).ToList();
        }
    }
}