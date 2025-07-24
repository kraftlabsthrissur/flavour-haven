using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity.Core.Objects;



namespace DataAccessLayer
{
  public class MouldDAL
    {
        public int Save(MouldBO mouldBO, List<MouldItemBO> mouldItems, List<MouldMachinesBO> mouldmachines)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ObjectParameter MouldID = new ObjectParameter("MouldID", typeof(int));
                    dbEntity.SpCreateMould(
                              mouldBO.Code,
                              mouldBO.MouldName,
                              mouldBO.InceptionDate,
                              mouldBO.ExpairyDate,
                              mouldBO.MandatoryMaintenanceTime,
                              mouldBO.ManufacturedBy,
                              mouldBO.CurrentLocationID,
                              GeneralBO.CreatedUserID,
                              GeneralBO.FinYear,
                              GeneralBO.LocationID,
                              GeneralBO.ApplicationID,
                              MouldID
                        );
                    if (MouldID.Value != null)
                    {
                        foreach (var item in mouldmachines)
                        {
                            dbEntity.SpCreateMouldMachines(
                                    Convert.ToInt16(MouldID.Value),
                                    item.ID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                );
                        }
                    }
                    if (MouldID.Value != null)
                    {
                        foreach (var item in mouldItems)
                        {
                            dbEntity.SpCreateMouldItems(
                                    Convert.ToInt16(MouldID.Value),
                                    item.ItemID,
                                    item.NoOfCavity,
                                    item.StdTime,
                                    item.StdWeight,
                                    item.StdRunningWaste,
                                    item.StdShootingWaste,
                                    item.StdGrindingWaste,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                );
                        }
                    }
                };
                    return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Update(MouldBO mouldBO, List<MouldItemBO> mouldItems, List<MouldMachinesBO> mouldmachines)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    
                    dbEntity.SpUpdateMould(
                              mouldBO.ID,
                              mouldBO.Code,
                              mouldBO.MouldName,
                              mouldBO.InceptionDate,
                              mouldBO.ExpairyDate,
                              mouldBO.MandatoryMaintenanceTime,
                              mouldBO.ManufacturedBy,
                              GeneralBO.CreatedUserID,
                              GeneralBO.FinYear,
                              GeneralBO.LocationID,
                              GeneralBO.ApplicationID
                        );
                    foreach (var item in mouldmachines)
                    {
                        dbEntity.SpCreateMouldMachines(
                                mouldBO.ID,
                                item.ID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID
                            );
                    }
                    foreach (var item in mouldItems)
                        {
                            dbEntity.SpCreateMouldItems(
                                    mouldBO.ID,
                                    item.ItemID,
                                    item.NoOfCavity,
                                    item.StdTime,
                                    item.StdWeight,
                                    item.StdRunningWaste,
                                    item.StdShootingWaste,
                                    item.StdGrindingWaste,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                );
                        }
                };
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<MouldBO> GetMould()
        {
            List<MouldBO> Mould = new List<MouldBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Mould = dbEntity.SpGetMould(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new MouldBO
                    {
                        ID = k.ID,
                        Code=k.Code,
                        MouldName=k.MouldName,
                        InceptionDate=(DateTime)k.InceptionDate,
                        ExpairyDate= (DateTime)k.ExpairyDate
                    }).ToList();
                }
                return Mould;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MouldItemBO> GetMouldItems(int MouldID)
        {
            List<MouldItemBO> item = new List<MouldItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetMouldItems(MouldID, GeneralBO.FinYear, GeneralBO.LocationID).Select(k => new MouldItemBO
                    {
                        ItemName = k.ItemName,
                        ItemID=k.ItemID,
                        NoOfCavity=(int)k.NoOfCavity,
                        StdTime=k.StdTime,
                        StdWeight=(decimal)k.StdWeight,
                        StdGrindingWaste=(int)k.StdGrindingWaste,
                        StdRunningWaste=(int)k.StdRunningWaste,
                        StdShootingWaste=(int)k.StdShootingWaste
                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MouldBO GetMouldDetails(int MouldID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetMouldDetails(MouldID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new MouldBO
                    {
                        ID = k.ID,
                        MouldName=k.MouldName,
                        Code=k.Code,
                        InceptionDate=(DateTime)k.InceptionDate,
                        ExpairyDate=(DateTime)k.ExpairyDate,
                        MandatoryMaintenanceTime = (int)k.MandatoryMaintenanceTime,
                        ManufacturedBy = k.ManufacturedBy,
                        CurrentLocationID = (int)k.CurrentLocationID,
                        CurrentLocationName = k.CurrentLocationName
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MouldMachinesBO> GetMachines(int MouldID)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    return dbEntity.SpGetMachinesForMould(MouldID,GeneralBO.ApplicationID).Select(
                    a => new MouldMachinesBO()
                    {
                        ID = a.ID,
                        Machine=a.MachineName,
                        check=a.Checked
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public DatatableResultBO GetMouldList(string CodeHint, string MouldNameHint, string ItemNameHint, string MachineNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetMouldList(CodeHint, MouldNameHint, ItemNameHint, MachineNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Code = item.Code,
                                MouldName = item.MouldName,
                                ItemName = item.ItemName,
                                MachineName = item.MachineName,
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
