using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
   public class MachineDAL
    {

        public int SaveMachine(MachineBO machine)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpCreateMachine(
                        machine.MachineCode,
                        machine.InsulationDate,
                        machine.TypeID,
                        machine.Model,
                        machine.LocationID,
                        machine.MachineName,
                        machine.ProcessID,
                        machine.Motor,
                        machine.PowerConsumptionPerHour,
                        machine.SoftwareVersion,
                        machine.MachineNumber,
                        machine.Manufacturer,
                        machine.OperatorCount,
                        machine.HelperCount,
                        machine.MaintenancePeriod,
                        machine.AverageCostPerHour,
                        GeneralBO.CreatedUserID,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                        );
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int UpdateMachineDetails(MachineBO machine)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateMachineDetails(
                        machine.ID,
                        machine.MachineCode,
                        machine.InsulationDate,
                        machine.Model,
                        machine.LocationID,
                        machine.MachineName,
                        machine.ProcessID,
                        machine.Motor,
                        machine.PowerConsumptionPerHour,
                        machine.SoftwareVersion,
                        machine.MachineNumber,
                        machine.Manufacturer,
                        machine.OperatorCount,
                        machine.HelperCount,
                        machine.MaintenancePeriod,
                        machine.AverageCostPerHour,
                        GeneralBO.CreatedUserID,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                        );

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<MachineBO> GetMachineList()
        {
            try
            {
                
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetMachineList().Select(a => new MachineBO
                    {
                        ID = a.ID,
                        MachineCode = a.MachineCode,
                        InsulationDate = (DateTime)a.InsulationDate,
                        MachineName = a.MachineName,
                        MachineNumber = a.MachineNumber
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<MachineBO> GetMachineDetails(int id)
        {
            try
            {
                List<MachineBO> machine = new List<MachineBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return machine =dbEntity.SpGetMachineDetails(id,GeneralBO.LocationID,GeneralBO.ApplicationID).Select(a => new MachineBO
                    {
                        ID = a.ID,
                        MachineCode = a.MachineCode,
                        InsulationDate =(DateTime)a.InsulationDate,
                        TypeID = (int)a.MachineTypeID,
                        Model = a.Model,
                        Location = a.CurrentLocation,
                        LocationID = a.CurrentLocationID,
                        MachineName = a.MachineName,
                        Process = a.Process,
                        ProcessID = a.ProcessID,
                        Motor = a.Motor,
                        PowerConsumptionPerHour = (decimal)a.PowerConsumptionPerHour,
                        SoftwareVersion = a.SoftwareVersion,
                        MachineNumber = a.MachineNumber,
                        Manufacturer = a.Manufacturer,
                        OperatorCount = (int)a.NumberOfOperators,
                        HelperCount = (int)a.NumberOfHelpers,
                        MaintenancePeriod = (int)a.MaintenancePeriod,
                        AverageCostPerHour = (decimal)a.AverageCostPerHour
                        }).ToList();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DatatableResultBO GetAllMachineList(string MachineCodeHint, string MachineNameHint, string LoadedMouldHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetAllMachineList(MachineCodeHint, MachineNameHint, LoadedMouldHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                MachineCode = item.MachineCode,
                                MachineName = item.MachineName,
                                LoadedMould = item.MouldName
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

    }
}
