using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;



namespace PresentationContractLayer
{
  public  interface IRoleContract
    {
        List<ActionBO> GetActions(int RoleId);

        int Save(RoleBO roleBO, List<ActionIDBO> Actions, List<ActionIDBO> Tabs);

        List<RoleBO> GetRoleList();

        List<RoleBO> GetRole(int ID);

        List<ActionBO> GetRoleActions(int ID);

        DatatableResultBO GetRoleListForDatatable(string Code,string RoleName,string Remarks,string Actions,string Tabs,string SortField,string SortOrder,int Offset,int Limit);

        List<ActionBO> GetAreas();

        List<ActionBO> GetActionsList(string Area, int RoleID);

        List<ActionBO> GetActionID(int ID);
    }
}
