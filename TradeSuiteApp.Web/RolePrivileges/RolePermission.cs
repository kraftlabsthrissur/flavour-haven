using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.RolePrivileges
{
    public static class RolePermission
    {
        public static bool HasPermission(this ControllerBase controller, string permission)
        {
            bool Found = false;
            try
            {
                Found = new CustomUserRole(controller.ControllerContext
                                     .HttpContext.User.Identity.Name).HasPermission(permission);
            }
            catch { }
            return Found;
        }

        public static bool IsSysAdmin(this ControllerBase controller)
        {
            bool IsSysAdmin = false;
            try
            {
                IsSysAdmin = new CustomUserRole(controller.ControllerContext
                                          .HttpContext.User.Identity.Name).IsSysAdmin;
            }
            catch { }
            return IsSysAdmin;
        }
    }

    public class CustomUserRole
    {
        private IRolePermissionContract _rolePermission;
        #region Constructor

        public CustomUserRole(IRolePermissionContract rolePermission)
        {
            _rolePermission = rolePermission;

        }

        #endregion

        public bool IsSysAdmin { get; set; }
        public string Username { get; set; }
        private List<RolePermissionBO> Roles = new List<RolePermissionBO>();
        private string RoleName { get; set; }

        public CustomUserRole(string username)
        {
            this.Username = username;
            this.IsSysAdmin = false;
            GetDatabaseUserRolesPermissions();
            _rolePermission = new RolePermissionRepository();
        }

        private void GetDatabaseUserRolesPermissions()
        {


        }

        public bool HasPermission(string requiredPermission)
        {
            return _rolePermission.CheckPermissionExists(Username, requiredPermission);
        }
    }

}