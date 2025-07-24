using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IFleetContract
    {
        int Save(FleetBO fleet);
        int UpdateFleet(FleetBO fleet);
        List<FleetBO> GetFleetList();
        List<FleetBO> GetFleetDetails(int id);

    }
}
