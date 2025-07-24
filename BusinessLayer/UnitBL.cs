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
    public class UnitBL : IUnitContract
    {
        UnitDAL unitDAL;
        public UnitBL()
        {
            unitDAL = new UnitDAL();
        }
        public int CreateUnit(UnitBO unitBo)
        {
            return unitDAL.CreateUnit(unitBo);
        }

        
        //public int DeleteState(StateBO stateBO)
        //{
        //    throw new NotImplementedException();
        //}

        public int EditUnit(UnitBO unitBO)
        {
            return unitDAL.UpdateUnit(unitBO);
        }

        public UnitBO GetUnitDetails(int UnitID)
        {
            return unitDAL.GetUnitDetails(UnitID);
        }



        public List<UnitBO> GetUnitsByItemID(int ItemID, string Type)
        {
            return unitDAL.GetUnitsByItemID(ItemID, Type);
        }
        public List<UnitBO> GetUnitList()
        {
            return unitDAL.GetUnitList();
        }
        public List<UnitBO> GetUnitGroupList()
        {
            return unitDAL.GetUnitGroupList();
        }
        

        public List<UnitBO> GetUnitsList()
        {
            return unitDAL.GetUnitsList();
        }
        public List<UnitBO> GetUnitListForAPI(int ItemID)
        {
            return unitDAL.GetUnitListForAPI(ItemID);
        }
        public int CreateSecondaryUnit(SecondaryUnitBO secondaryUnitBO)
        {
            return unitDAL.CreateSecondaryUnit(secondaryUnitBO);
        }
        public int UpdateSecondaryUnit(SecondaryUnitBO secondaryUnitBO)
        {
            return unitDAL.UpdateSecondaryUnit(secondaryUnitBO);
        }
        //
        public SecondaryUnitBO GetSecondaryUnitDetails(int UnitID)
        {
            return unitDAL.GetSecondaryUnitDetails(UnitID);
        }
        public List<SecondaryUnitBO> GetSecondaryUnitList()
        {
            return unitDAL.GetSecondaryUnitList();
        }
        public List<SecondaryUnitBO> GetSecondarytUnitListByUnitID(int UnitID)
        {
            return unitDAL.GeSecondarytUnitListByUnitID(UnitID);
        }
        
    }
}
