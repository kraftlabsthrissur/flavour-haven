using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
namespace DataAccessLayer
{
    public class FleetDAL
    {
        public int Save(FleetBO fleet)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpCreateFleet(
                            fleet.VehicleNo,
                            fleet.VehicleName,
                            fleet.TravellingAgency,
                            fleet.TestExpairyDate,
                            fleet.TaxExpairyDate,
                            fleet.PurchaseDate,
                            fleet.PolicyNo,
                            fleet.PermitExpairyDate,
                            fleet.OwnerName,
                            fleet.OtherDetails,
                            fleet.LicenseNo,
                            fleet.InsuranceExpairyDate,
                            fleet.InsuranceCompany,
                            fleet.DriverName,
                            fleet.CanCapacity,
                            fleet.BoxCapacity,
                            fleet.BagCapacity,
                            GeneralBO.CreatedUserID,
                            GeneralBO.ApplicationID,
                            GeneralBO.LocationID);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<FleetBO> GetFleetList()
        {
            try
            {
                List<FleetBO> Fleet = new List<FleetBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Fleet = dbEntity.SpGetFleetList().Select(a => new FleetBO
                    {
                        ID = a.ID,
                        VehicleNo = a.VehicleNo,
                        VehicleName = a.VehicleName,
                        DriverName = a.DriverName,
                        OwnerName = a.OwnerName,
                        TravellingAgency = a.TravellingAgency
                    }).ToList();
                }
                return (Fleet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<FleetBO> GetFleetDetails(int id)
        {
            try
            {
                List<FleetBO> Fleet = new List<FleetBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Fleet = dbEntity.SpGetFleetDetails(id).Select(a => new FleetBO
                    {
                        ID = (int)a.ID,
                        VehicleNo = a.VehicleNo,
                        VehicleName = a.VehicleName,
                        DriverName = a.DriverName,
                        TravellingAgency = a.TravellingAgency,
                        TestExpairyDate = Convert.ToDateTime(a.TestExpairyDate),
                        TaxExpairyDate = Convert.ToDateTime(a.TaxExpairyDate),
                        PurchaseDate = Convert.ToDateTime(a.PurchasedDate),
                        PermitExpairyDate = Convert.ToDateTime(a.PermitExpairyDate),
                        PolicyNo = a.PolicyNo,
                        BagCapacity = a.BagCapacity,
                        CanCapacity = a.CanCapacity,
                        BoxCapacity = a.BoxCapacity,
                        InsuranceCompany = a.InsuranceCompany,
                        InsuranceExpairyDate = Convert.ToDateTime(a.InsuranceExpairyDate),
                        LicenseNo = a.LicenseNo,
                        OtherDetails = a.OtherDetails,
                        OwnerName = a.OwnerName
                    }).ToList();
                }
                return Fleet;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int UpdateFleet(FleetBO fleet)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateFleet(
                            fleet.ID,
                            fleet.VehicleNo,
                            fleet.VehicleName,
                            fleet.TravellingAgency,
                            fleet.TestExpairyDate,
                            fleet.TaxExpairyDate,
                            fleet.PurchaseDate,
                            fleet.PolicyNo,
                            fleet.PermitExpairyDate,
                            fleet.OwnerName,
                            fleet.OtherDetails,
                            fleet.LicenseNo,
                            fleet.InsuranceExpairyDate,
                            fleet.InsuranceCompany,
                            fleet.DriverName,
                            fleet.CanCapacity,
                            fleet.BoxCapacity,
                            fleet.BagCapacity,
                            GeneralBO.CreatedUserID,
                            GeneralBO.ApplicationID,
                            GeneralBO.LocationID);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
