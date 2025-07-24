using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class PrivilegeCardBL : IPrivilegeCardContract
    {
        PrivilegeCardDAL cardDAL;
        public PrivilegeCardBL ()
        {
            cardDAL = new PrivilegeCardDAL();
        }
        public List<PrivilegeCardBO> GetPriviledgeCards()
        {
            return cardDAL.GetPriviledgeCards();
        }
        public List<PrivilegeCardBO> GetPriviledgeCardbyID(int id)
        {
            return cardDAL.GetPriviledgeCardbyID(id);
        }
    }
}
