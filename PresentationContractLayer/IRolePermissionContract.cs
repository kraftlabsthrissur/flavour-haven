using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IRolePermissionContract
    {
        List<RolePermissionBO> GetRoleModuleByRole(string role);
        List<RolePermissionBO> GetRoleModuleByUser(string userName);
        List<RolePermissionBO> GetRoleModuleByCriteria(string userName, string roleName, string moduleName = "", string controllerName = "", string action = "", string permissionKey = "", int roleID = 0);
        bool CheckPermissionExists(string userName, string permissionKey);
        List<string> GetDistinctArea();
        List<string> GetControllerByModule(string module);
        void ModifyRolePermission(List<RolePermissionBO> boList, int roleID);
    }
}
