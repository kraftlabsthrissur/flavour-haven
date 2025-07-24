using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using DataAccessLayer;
using BusinessObject;

namespace BusinessLayer
{
    public class UnitGroupBL : IUnitGroupContract
    {
        UnitGroupDAL unitGroupDAL;
        public UnitGroupBL()
        {
            unitGroupDAL = new UnitGroupDAL();
        }


        public List<UnitGroupBO> GetUnitGroupList()
        {
            return unitGroupDAL.GetUnitGroupList();
        }


    }
}
