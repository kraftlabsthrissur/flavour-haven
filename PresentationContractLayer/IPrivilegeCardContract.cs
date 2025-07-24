using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IPrivilegeCardContract
    {
        List<PrivilegeCardBO> GetPriviledgeCards();
        List<PrivilegeCardBO> GetPriviledgeCardbyID(int id);
    }
}
