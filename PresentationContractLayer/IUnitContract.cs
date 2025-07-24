using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using System.Web.UI.WebControls;

namespace PresentationContractLayer
{
    public interface IUnitContract
    {
        List<UnitBO> GetUnitList();
        List<UnitBO> GetUnitGroupList();
        int CreateUnit(UnitBO unitBO);
        int EditUnit(UnitBO unitBO);
        UnitBO GetUnitDetails(int UnitID);
        List<UnitBO> GetUnitsList();
        List<UnitBO> GetUnitListForAPI(int ItemID);
        List<SecondaryUnitBO> GetSecondaryUnitList();
        List<SecondaryUnitBO> GetSecondarytUnitListByUnitID(int UnitID);
        
        SecondaryUnitBO GetSecondaryUnitDetails(int UnitID);
        int CreateSecondaryUnit(SecondaryUnitBO secondaryUnitBO);
        int UpdateSecondaryUnit(SecondaryUnitBO secondaryUnitBO);


    }
}
