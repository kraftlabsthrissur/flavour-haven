using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class WarehouseDAL
    {

        public List<WareHouseBO> GetWareHouseList()
        {
            try
            {
                using (MasterEntities db = new MasterEntities())
                {
                    return db.SpGetWareHouseList(GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new WareHouseBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Place = a.Place,
                        ItemTypeID = a.ItemTypeID,
                        ItemTypeName = a.ItemTypeName,
                        Remarks = a.Remarks
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<WareHouseBO> GetWareHouses()
        {
            try
            {
                using (MasterEntities db = new MasterEntities())
                {
                    return db.SpGetWareHouse(GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new WareHouseBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Place = a.Place,
                        ItemTypeID = a.ItemTypeID,
                        ItemTypeName = a.ItemTypeName,
                        Remarks = a.Remarks
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<WareHouseBO> GetWareHousesForStockRequestReceipt(int ID)
        {
            try
            {
                using (MasterEntities db = new MasterEntities())
                {
                    return db.SpGetWareHouseByWareHouseID(ID,GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new WareHouseBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Place = a.Place,
                        ItemTypeID = a.ItemTypeID,
                        ItemTypeName = a.ItemTypeName,
                        Remarks = a.Remarks
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<WareHouseBO> GetWareHousesForStockRequestIssue(int ID)
        {
            try
            {
                using (MasterEntities db = new MasterEntities())
                {
                    return db.SpGetWareHouseByWareHouseID(ID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new WareHouseBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Place = a.Place,
                        ItemTypeID = a.ItemTypeID,
                        ItemTypeName = a.ItemTypeName,
                        Remarks = a.Remarks
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<WareHouseBO> GetWareHousesByLocation(int LocationID)
        {
            try
            {
                using (MasterEntities db = new MasterEntities())
                {
                    return db.SpGetWareHouse(LocationID, GeneralBO.ApplicationID).Select(a => new WareHouseBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Place = a.Place,
                        ItemTypeID = a.ItemTypeID,
                        ItemTypeName = a.ItemTypeName,
                        Remarks = a.Remarks
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int CreateWareHouse(WareHouseBO wareHouseBO)
        {
            ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
            try
            {
                var warehouseretvalue = 0;
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    warehouseretvalue = dbEntity.SpCreateWareHouse(
                        wareHouseBO.Code,
                        wareHouseBO.Name,
                        wareHouseBO.Place,
                        "False",
                        "False",
                        wareHouseBO.ItemTypeID,
                        wareHouseBO.Remarks,
                        wareHouseBO.LocationID,
                        GeneralBO.ApplicationID,
                        GeneralBO.CreatedUserID,
                        DateTime.Now,
                        RetValue);
                }
                return warehouseretvalue;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public WareHouseBO GetWareHouseDetails(int WareHouseID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetWareHouseByID(WareHouseID).Select(a => new WareHouseBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Place = a.Place,
                        Remarks = a.Remarks,
                        ItemTypeID = a.ItemTypeID,
                        ItemTypeName = a.ItemTypeName,
                        LocationID = (int)a.LocationID,
                        LocationName = a.LocationName
                    }
                      ).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int UpdateWareHouse(WareHouseBO wareHouseBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var data = dbEntity.SpUpdateWareHouse(wareHouseBO.ID, wareHouseBO.Code, wareHouseBO.Name, wareHouseBO.Place, wareHouseBO.Remarks, wareHouseBO.ItemTypeID, wareHouseBO.LocationID,GeneralBO.CreatedUserID,GeneralBO.LocationID,GeneralBO.ApplicationID);
                    return data;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public DatatableResultBO GetNursingStationAutoComplete(string Hint)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetNursingStationList(Hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Name = item.Name

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DatatableResult;
        }

    }
}
