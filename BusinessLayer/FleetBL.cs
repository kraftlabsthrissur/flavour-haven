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
    public class FleetBL : IFleetContract
    {
        FleetDAL fleetDAL;
        public FleetBL()
        {
            fleetDAL = new FleetDAL();
        }
        public int Save(FleetBO fleet)
        {
            return fleetDAL.Save(fleet);
        }
        public List<FleetBO> GetFleetList()
        {
            return fleetDAL.GetFleetList();
        }
        public List<FleetBO> GetFleetDetails(int id)
        {
            return fleetDAL.GetFleetDetails(id);
        }
        public int UpdateFleet(FleetBO fleet)
        {
            return fleetDAL.UpdateFleet(fleet);
        }

    }
}


