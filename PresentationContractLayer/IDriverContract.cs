using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IDriverContract
    {
        int SaveDriver(DriverBO driver);
        List<DriverBO> GetDriverList();
        List<DriverBO> GetDriverDetails(int id);
        int UpdateDriver(DriverBO driver);


    }
}
