using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using BusinessObject;
using DataAccessLayer;

namespace BusinessLayer
{
    public class StateBL : IStateContract
    {
        StateDAL stateDAL;
        public StateBL()
        {
            stateDAL = new StateDAL();
        }
        public int CreateState(StateBO stateBo)
        {
            return stateDAL.CreateState(stateBo);
        }
        public int DeleteState(StateBO stateBO)
        {
            throw new NotImplementedException();
        }

        public int EditState(StateBO stateBO)
        {
            return stateDAL.UpdateState(stateBO);
        }

        public StateBO GetStateDetails(int StateID)
        {
            return stateDAL.GetStateDetails(StateID);
        }

        public List<StateBO> GetStateList()
        {
            return stateDAL.GetStateList();
        }
        public List<StateBO> GetStateListCountryWise(int CountryID)
        {
            return stateDAL.GetStateListCountryWise(CountryID);
        }

    }
}
