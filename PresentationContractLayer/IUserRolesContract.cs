using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IUserRolesContract
    {
       
        DatatableResultBO GetRolesList(string CodeHint,string  NameHint, string SortField, string SortOrder, int Offset, int Limit);

        List<UserRolesBO> GetUserRoles();

        int Save(List<UserRolesBO> Items);

        bool IsUserhaveRoles(int UserID);

        List<UserRolesBO> GetUserRolesDetails(int ID);       
        
    }
}
