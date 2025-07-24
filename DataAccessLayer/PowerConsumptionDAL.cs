using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PowerConsumptionDAL
    {

        public List<PowerConsumptionBO> GetLocationList()
        {
            List<PowerConsumptionBO> LocationList = new List<PowerConsumptionBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                LocationList = dEntity.SpGetListForLocation().Select(a => new PowerConsumptionBO
                {
                    Location = a.ID,
                    LocationName = a.Name,

                }).ToList();
                return LocationList;
            }
        }

        public int Save(List<PowerConsumptionItemBO> Items, PowerConsumptionBO powerConsumptionBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    foreach (var item in Items)
                    {
                        if (item.ID == 0)
                        {
                            dbEntity.SpCreatePowerConsumption(
                                    powerConsumptionBO.Location,
                                    item.Time,
                                    item.Amount,
                                    GeneralBO.CreatedUserID,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                );
                        }
                    }
                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<PowerConsumptionBO> GetPowerConsumptionDetails(int ID)
        {
            try
            {
                List<PowerConsumptionBO> PowerConsumption = new List<PowerConsumptionBO>();

                using (MasterEntities dEntity = new MasterEntities())
                {
                    PowerConsumption = dEntity.SpGetPowerConsumptionByID(ID).Select(a => new PowerConsumptionBO
                    {
                        ID=a.ID,
                        Location=a.LocationID,
                        LocationName=a.LocationName
                    }).ToList();
                    return PowerConsumption;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PowerConsumptionBO> GetPowerConsumptionTransDetails(int ID)
        {
            try
            {
                List<PowerConsumptionBO> PowerConsumption = new List<PowerConsumptionBO>();

                using (MasterEntities dEntity = new MasterEntities())
                {
                    PowerConsumption = dEntity.SpGetPowerConsumptionTransDetails(ID).Select(a => new PowerConsumptionBO
                    {
                        Amount = (int)a.Amount,
                        Time = a.Time
                    }).ToList();
                    return PowerConsumption;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(List<PowerConsumptionItemBO> Items, PowerConsumptionBO powerConsumptionBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var i = dbEntity.SpDeletePowerConsumption(
                                     powerConsumptionBO.Location,
                                     GeneralBO.ApplicationID
                                     );
                    foreach (var item in Items)
                    {
                        dbEntity.SpCreatePowerConsumption(
                                    powerConsumptionBO.Location,
                                    item.Time,
                                    item.Amount,
                                    GeneralBO.CreatedUserID,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                );
                    }
                    return 1;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
