using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RolePermissionRepository : IRolePermissionContract
    {
        private readonly AyurwareEntities _entity;
        #region Constructor

        public RolePermissionRepository()
        {
            _entity = new AyurwareEntities();
        }

        #endregion

        /// <summary>
        /// GetRoleModules by Role
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<RolePermissionBO> GetRoleModuleByRole(string role)
        {
            using (AyurwareEntities dbEntity = new AyurwareEntities())
            {
                return dbEntity.SpGetRoleModulesByRole(role).Select(x => new RolePermissionBO
                {
                    RoleModuleID = x.ID,
                    RoleID = x.RoleID,
                    RoleName = x.RoleName,
                    ModuleID = x.ModuleID,
                    Area = x.Area,
                    Controller = x.Controller,
                    Action = x.Action,
                    Key = x.Key
                }).ToList();
            }

        }

        /// <summary>
        /// GetRoleModules by User
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<RolePermissionBO> GetRoleModuleByUser(string userName)
        {
            using (AyurwareEntities dbEntity = new AyurwareEntities())
            {
                return dbEntity.SpGetRoleModulesByUser(userName).Select(x => new RolePermissionBO
                {
                    RoleModuleID = x.ID,
                    RoleID = x.RoleID,
                    RoleName = x.RoleName,
                    ModuleID = x.ModuleID,
                    Area = x.Area,
                    Controller = x.Controller,
                    Action = x.Action,
                    Key = x.Key
                }).ToList();
            }
        }

        /// <summary>
        /// Get Role modules by criteria
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        /// <param name="moduleName"></param>
        /// <param name="controllerName"></param>
        /// <param name="action"></param>
        /// <param name="permissionKey"></param>
        /// <returns></returns>
        public List<RolePermissionBO> GetRoleModuleByCriteria(string userName, string roleName, string moduleName = "", string controllerName = "", string action = "", string permissionKey = "", int roleID = 0)
        {
            using (AyurwareEntities dbEntity = new AyurwareEntities())
            {
                return dbEntity.SpGetRoleModulesByCriteria(roleID, roleName, moduleName, controllerName, action, permissionKey).Select(x => new RolePermissionBO
                {
                    RoleModuleID = x.RoleModuleID ?? 0,
                    RoleID = x.RoleID ?? 0,
                    RoleName = x.RoleName,
                    ModuleID = x.ModuleID,
                    Area = x.Area,
                    Controller = x.Controller,
                    Action = x.Action,
                    Key = x.Key,
                    IsHaveAccess = x.IsHaveAccess ?? false
                }).ToList();
            }
        }

        /// <summary>
        /// Check permission exists by user name and permission key
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="permissionKey"></param>
        /// <returns></returns>
        public bool CheckPermissionExists(string userName, string permissionKey)
        {
            var roleModules = GetRoleModuleByCriteria(userName, string.Empty, string.Empty, string.Empty, permissionKey);
            return roleModules != null && roleModules.Count() > 0 && roleModules.FirstOrDefault().IsHaveAccess;
        }

        /// <summary>
        /// Get all modules
        /// </summary>
        /// <returns></returns>
        public List<ModuleBO> GetAllModules()
        {
            using (AyurwareEntities dbEntity = new AyurwareEntities())
            {
                return dbEntity.SpGetModules().Select(x => new ModuleBO
                {
                    ID = x.ID,
                    Name = x.Name,
                    Area = x.Area,
                    Controller = x.Controller,
                    Action = x.Action,
                    Key = x.Key,
                    TableName = x.TableName
                }).ToList();
            }
        }

        /// <summary>
        /// Get Distinct Areas
        /// </summary>
        /// <returns></returns>
        public List<string> GetDistinctArea()
        {
            return GetAllModules().Select(x => x.Area).Distinct().ToList();
        }

        /// <summary>
        /// Get controllers by Modules
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>

        public List<string> GetControllerByModule(string module)
        {
            //No Need to add distinct. Controller will be distinct for each and every module
            return GetRoleModuleByCriteria(string.Empty, string.Empty, module, string.Empty, string.Empty, string.Empty).Select(x => x.Controller).ToList();
        }

        /// <summary>
        /// Add update role permission
        /// </summary>
        /// <returns></returns>
        public void ModifyRolePermission(List<RolePermissionBO> boList, int roleID)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("RoleID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IsHaveAccess");

            foreach (var bo in boList)
            {
                dt.Rows.Add(roleID, bo.ModuleID, bo.IsHaveAccess);
            }


            using (AyurwareEntities dbEntity = new AyurwareEntities())
            {


                string sql = @"EXEC [dbo].[SpAddUpdateRolePermission] @rolePermission";

                SqlParameter param = new SqlParameter("@rolePermission", SqlDbType.Structured)
                {
                    TypeName = "dbo.AddUpdateRolePermissionTblType",
                    Value = dt
                };

                dbEntity.Database.ExecuteSqlCommand(sql,param);
            }
        }
    }



}
