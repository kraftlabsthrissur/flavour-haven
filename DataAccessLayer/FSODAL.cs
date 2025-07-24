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
    public class FSODAL
    {
        public int Save(FSOBO fsoBO, string FSOIncentiveItems)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                ObjectParameter FSOID = new ObjectParameter("FSOID", typeof(int));
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        var i = dbEntity.SpCreateFSO(
                           fsoBO.FSOCode,
                           fsoBO.IsAreaManager,
                           fsoBO.IsZonalManager,
                           fsoBO.IsRegionalSalesManager,
                           fsoBO.IsSalesManager,
                           fsoBO.BusinessCategoryID,
                           fsoBO.SalesIncentiveCategoryID,
                           fsoBO.SalesCategoryID,
                           fsoBO.RouteCode,
                           fsoBO.RouteName,
                           fsoBO.ZoneCode,
                           fsoBO.ZoneName,
                           fsoBO.FSOName,
                           fsoBO.ReportingToID,
                           fsoBO.AreaManagerID,
                           fsoBO.ZonalManagerID,
                           fsoBO.SalesManagerID,
                           fsoBO.RegionalSalesManagerID,
                           fsoBO.FromDate,
                           fsoBO.ToDate,
                           fsoBO.EmployeeID,
                           fsoBO.IsActive,
                           GeneralBO.LocationID,
                           GeneralBO.ApplicationID,
                           FSOID
                            );
                        dbEntity.SaveChanges();
                        transaction.Commit();
                        int ID = Convert.ToInt32(FSOID.Value);
                        if (ID != 0)
                        {
                            dbEntity.SpCreateFSOIncentiveMapping(ID,FSOIncentiveItems);
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                        return 1;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public int Update(FSOBO fsoBO, string FSOIncentiveItems)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        var i = dbEntity.SpUpdateFSO(
                           fsoBO.ID,
                           fsoBO.FSOCode,
                           fsoBO.IsAreaManager,
                           fsoBO.IsZonalManager,
                           fsoBO.IsRegionalSalesManager,
                           fsoBO.IsSalesManager,
                           fsoBO.BusinessCategoryID,
                           fsoBO.SalesIncentiveCategoryID,
                           fsoBO.SalesCategoryID,
                           fsoBO.RouteCode,
                           fsoBO.RouteName,
                           fsoBO.ZoneCode,
                           fsoBO.ZoneName,
                           fsoBO.FSOName,
                           fsoBO.ReportingToID,
                           fsoBO.AreaManagerID,
                           fsoBO.ZonalManagerID,
                           fsoBO.SalesManagerID,
                           fsoBO.RegionalSalesManagerID,
                           fsoBO.FromDate,
                           fsoBO.ToDate,
                           fsoBO.EmployeeID,
                           fsoBO.IsActive,
                           GeneralBO.LocationID,
                           GeneralBO.ApplicationID,
                           GeneralBO.CreatedUserID
                            );                 
                        dbEntity.SpCreateFSOIncentiveMapping(fsoBO.ID,FSOIncentiveItems);
                        transaction.Commit();
                        return 1;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<FSOBO> GetFSOLst()
        {
            List<FSOBO> fso = new List<FSOBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    fso = dbEntity.SpGetFSOLst(GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new FSOBO
                    {
                        ID = k.ID,
                        FSOName = k.FSOName,
                        AreaManager = k.AreaManager,
                        ZonalManager = k.ZonalManager,
                        RouteCode = k.RouteCode,
                        SalesManager = k.SalesManager,
                        RegionalSalesManager = k.RegionalSalesManager
                    }).ToList();
                }
                return fso;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FSOBO GetFSODetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetFSODetails(ID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new FSOBO
                    {
                        ID = k.ID,
                        FSOCode = k.FSOCode,
                        FSOName = k.FSOName,
                        AreaManager = k.AreaManager,
                        ZonalManager = k.ZonalManager,
                        IsAreaManager = (bool)k.IsAreaManager,
                        IsZonalManager = (bool)k.IsZonalManager,
                        BusinessCategoryID = (int)k.BusinessCategoryID,
                        SalesCategoryID = (int)k.SalesCategoryID,
                        SalesIncentiveCategoryID = (int)k.SalesIncentiveCategoryID,
                        SalesCategoryName = k.SalesCategoryName,
                        SalesIncentiveCategoryName = k.SalesIncentiveCategoryName,
                        BusinessCategoryName = k.BusinessCategoryName,
                        ZoneCode = k.ZoneCode,
                        ZoneName = k.ZoneName,
                        RouteCode = k.RouteCode,
                        RouteName = k.RouteName,
                        FromDate = (DateTime)k.FromDate,
                        ToDate = (DateTime)k.ToDate,
                        EmployeeID = (int)k.EmployeeID,
                        IsActive = (bool)k.IsActive,
                        SalesManagerID = (int)k.SalesManagerID,
                        RegionalSalesManagerID = (int)k.RegionalSalesManagerID,
                        SalesManager = k.SalesManager,
                        RegionalSalesManager = k.RegionalSalesManager,
                        IsSalesManager = (bool)k.IsSalesManager,
                        IsRegionalSalesManager = (bool)k.IsRegionalSalesManager,
                        ZonalManagerID = (int)k.ZonalManagerID,
                        AreaManagerID = (int)k.AreaManagerID,
                        ReportingToID=(int)k.ReportingID,
                        ReportingToName=k.ReportingName
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsFSOExist(int ID)
        {
            try
            {
                ObjectParameter IsExist = new ObjectParameter("IsExist", typeof(bool));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpIsFSOExist(
                                  ID,
                                  GeneralBO.ApplicationID,
                                  IsExist
                                  );
                    return Convert.ToBoolean(IsExist.Value);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetFSOList(string CodeHint, string NameHint, string SalesManagerHint, string RegionalManagerHint, string ZonalManagerHint, string AreaManagerHint, string RouteCodeHint, string RouteNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetFSOList(CodeHint, NameHint, SalesManagerHint, RegionalManagerHint, ZonalManagerHint, AreaManagerHint, RouteCodeHint, RouteNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                FSOCode = item.FSOCode,
                                FSOName = item.FSOName,
                                AreaManager = item.AreaManager,
                                ZonalManager = item.ZonalManager,
                                RouteCode = item.RouteCode,
                                SalesManager = item.SalesManager,
                                RegionalSalesManager = item.RegionalSalesManager,
                                RouteName = item.RouteName,
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

        public List<FSOIncentiveItemBO> GetCustomersByFilters(int StateID, int DistrictID, int CustomerCategoryID, int FSOID, int SalesIncentiveCategoryID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCustomersByFilterForFSO(StateID, DistrictID, CustomerCategoryID, FSOID, SalesIncentiveCategoryID).Select(a => new FSOIncentiveItemBO()
                    {
                        CustomerID = a.CustomerID,
                        CustomerName = a.CustomerName,
                        FSOName = a.FSOName,
                        CustomerCategory = a.CustomerCategory,
                        StateID = (int)a.StateID,
                        DistrictID = (int)a.DistrictID,
                        SalesIncentiveCategoryID = (int)a.SalesIncentiveCategoryID,
                        SalesIncentiveCategory = a.SalesIncentiveCategory,
                        State = a.State,
                        District = a.District,
                        CustomerCode = a.CustomerCode,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FSOIncentiveItemBO> GetCustomersByFSO(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCustomersByFSO(ID).Select(a => new FSOIncentiveItemBO()
                    {
                        CustomerID = a.CustomerID,
                        CustomerName = a.CustomerName,
                        FSOName = a.FSOName,
                        CustomerCategory = a.CustomerCategory,
                        StateID = (int)a.StateID,
                        DistrictID = (int)a.DistrictID,
                        SalesIncentiveCategoryID = (int)a.SalesIncentiveCategoryID,
                        SalesIncentiveCategory = a.SalesIncentiveCategory,
                        State = a.State,
                        District = a.District,
                        CustomerCode = a.CustomerCode,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InactiveConfirm(int ID)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            bool result;
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpFSOInactiveConfirm(
                   ID,
                   ReturnValue);
                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }

        public DatatableResultBO GetFSOManagersList(string CodeHint, string NameHint, string DesignationHint, string RouteNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetFSOManagersList(CodeHint, NameHint, DesignationHint, RouteNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                FSOCode = item.FSOCode,
                                FSOName = item.FSOName,
                                AreaManager = item.AreaManager,
                                ZonalManager = item.ZonalManager,
                                RouteCode = item.RouteCode,
                                SalesManager = item.SalesManager,
                                RegionalSalesManager = item.RegionalSalesManager,
                                RouteName = item.RouteName,
                                Designation = item.Designation
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
