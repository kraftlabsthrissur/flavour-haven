using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
namespace DataAccessLayer
{
    public class DriverDAL
    {
        public int SaveDriver(DriverBO driver)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpCreateDriver(
                        driver.Name,
                        driver.Code,
                        driver.Address,
                        driver.LicenseNo,
                        driver.PhoneNo,
                        driver.IsActive,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID,
                        GeneralBO.CreatedUserID
                        );
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<DriverBO> GetDriverList()
        {
            try
            {
                List<DriverBO> driver = new List<DriverBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    driver = dbEntity.SpGetDriverList().Select(a => new DriverBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Address = a.Address,
                        PhoneNo = a.PhoneNo,
                        LicenseNo = a.LicenseNo
                    }).ToList();
                }
                return (driver);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<DriverBO> GetDriverDetails(int id)
        {
            try
            {
                List<DriverBO> driver = new List<DriverBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    driver = dbEntity.SpGetDriverDetails(id).Select(a => new DriverBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Address = a.Address,
                        LicenseNo = a.LicenseNo,
                        PhoneNo = a.PhoneNo,
                        IsActive = (bool)a.IsActive

                    }).ToList();
                }
                return driver;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int UpdateDriver(DriverBO driver)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateDriver(
                         driver.ID,
                         driver.Name,
                         driver.Code,
                         driver.Address,
                         driver.LicenseNo,
                         driver.PhoneNo,
                         driver.IsActive,
                         GeneralBO.LocationID,
                         GeneralBO.ApplicationID,
                         GeneralBO.CreatedUserID
                         );

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
