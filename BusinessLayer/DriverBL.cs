using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;

namespace BusinessLayer
{
    public class DriverBL : IDriverContract
    {
        DriverDAL driverDAL;
        public DriverBL()
        {
            driverDAL = new DriverDAL();
        }
        public int SaveDriver(DriverBO driver)
        {
            return driverDAL.SaveDriver(driver);
        }
        public List<DriverBO> GetDriverList()
        {
            return driverDAL.GetDriverList();
        }
        public List<DriverBO> GetDriverDetails(int id)
        {
            return driverDAL.GetDriverDetails(id);
        }
        public int UpdateDriver(DriverBO driver)
        {
            return driverDAL.UpdateDriver(driver);
        }
    }
}
