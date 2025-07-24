using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IStateContract
    {
        //string SaveStates(StateBO , List<ItemBO> _prdetails);
        List<StateBO> GetStateList();
        int CreateState(StateBO stateBO);
        int EditState(StateBO stateBO);
        StateBO GetStateDetails(int StateID);
        List<StateBO> GetStateListCountryWise(int CountryID);
    }
}
