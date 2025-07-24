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
    public class SchemeAllocationDAL
    {

        public void Save(SchemeAllocationBO schemeAllocationBO, string Items, string Customers, string Categories, string States, string Districts)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        dbEntity.SpCreateOrUpdateSchemeXMLMethod(
                            schemeAllocationBO.ID,
                            schemeAllocationBO.SchemeName,
                            schemeAllocationBO.StartDate,
                            schemeAllocationBO.EndDate,
                            Items,
                            Customers,
                            Categories,
                            States,
                            Districts,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            RetValue);
                        if (Convert.ToInt16(RetValue.Value) == -1)
                        {
                            throw new Exception("Scheme Allocation already exists");
                        }
                        if (Convert.ToInt16(RetValue.Value) == -2)
                        {
                            throw new Exception("Repeating item - offer quantity");
                        }
                        if (Convert.ToInt16(RetValue.Value) == -3)
                        {
                            throw new Exception("Different offer quantity for same item quantity");
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public DatatableResultBO GetSchemeAllocationList(string CodeHint, string NameHint, string CustomerNameHint, string CustomerCategoryHint, string CustomerStateHint, string CustomerDistrictHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetSchemeAllocationList(CodeHint, NameHint, CustomerNameHint, CustomerCategoryHint, CustomerStateHint, CustomerDistrictHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Name = item.Scheme,
                                Code = item.Code,
                                CustomerName = item.CustomerName,
                                CustomerCategory = item.CustomerCategory,
                                CustomerState = item.CustomerState,
                                CustomerDistrict = item.CustomerDistrict,
                                StartDate = ((DateTime)item.StartDate).ToString("dd-MMM-yyyy"),
                                EndDate = ((DateTime)item.EndDate).ToString("dd-MMM-yyy")
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

        public DatatableResultBO GetSchemeAllocationTransList(int ID, string NameHint, string SalesCategoryHint, string StartDate, string EndDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.GetSchemeAllocationTransList(ID, NameHint, SalesCategoryHint, StartDate, EndDate, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ItemName = item.ItemName,
                                OfferItem = item.OfferItem,
                                SalesCategory = item.SalesCategory,
                                InvoiceQty = item.InvoiceQty,
                                OfferQty = item.OfferQty,
                                StartDate = ((DateTime)item.StartDate).ToString("dd-MMM-yyyy"),
                                EndDate = ((DateTime)item.EndDate).ToString("dd-MMM-yyy")
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

        public SchemeAllocationBO GetSchemeAllocationDetails(int ID)
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                try
                {
                    return dEntity.SpGetSchemeAllocationDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SchemeAllocationBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        SchemeName = a.Scheme,
                        CountryID = (int)a.CountryID,
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,

                    }).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }

        public List<SchemeItemBO> GetSchemeItemList(int ID)
        {
            List<SchemeItemBO> list = new List<SchemeItemBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                list = dEntity.SpGetSchemeItems(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SchemeItemBO
                {
                    ID = (int)a.ID,
                    OfferItem = a.OfferItem,
                    OfferQty = (decimal)a.OfferQty,
                    InvoiceQty = (decimal)a.InvoiceQty,
                    Item = a.ItemName,
                    ItemID = (int)a.ItemID,
                    OfferItemID = (int)a.OfferItemID,
                    BusinessCategory = a.BusinessCategoty,
                    BusinessCategoryID = (int)a.BusinessCategoryID,
                    SalesCategory = a.SalesCategory,
                    SalesCategoryID = (int)a.SalesCategoryID,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    IsEnded = (int)a.IsEnd

                }).ToList();
            }
            return list;
        }

        public List<SchemeCustomerBO> GetSchemeCustomerList(int ID)
        {
            List<SchemeCustomerBO> list = new List<SchemeCustomerBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                list = dEntity.SpGetSchemeCustomerList(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SchemeCustomerBO
                {
                    CustomerID = (int)a.CustomerID,
                    Customer = a.CustomerName
                }).ToList();
            }
            return list;
        }

        public List<SchemeCustomerCategoryBO> GetSchemeCategoryList(int ID)
        {
            List<SchemeCustomerCategoryBO> list = new List<SchemeCustomerCategoryBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                list = dEntity.SpGetSchemeCategoryList(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SchemeCustomerCategoryBO
                {
                    CustomerCategory = a.CategoryName,
                    CustomerCategoryID = (int)a.CategoryID,
                    SchemeCategoryID = (int)a.SchemeCategoryID
                }).ToList();
            }
            return list;
        }

        public List<SchemeStateBO> GetSchemeStateList(int ID)
        {
            List<SchemeStateBO> list = new List<SchemeStateBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                list = dEntity.SpGetSchemeStateList(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SchemeStateBO
                {
                    State = a.StateName,
                    StateID = a.StateID,
                    CountryID = (int)a.CountryID,
                    SchemeStateID = a.SchemeStateID
                }).ToList();
            }
            return list;
        }

        public List<SchemeDistrictBO> GetSchemeDistrictList(int ID)
        {
            List<SchemeDistrictBO> list = new List<SchemeDistrictBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                list = dEntity.SpGetSchemeDistrictList(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SchemeDistrictBO
                {
                    District = a.DistrictName,
                    DistrictID = a.DistrictID,
                    StateID = (int)a.StateID,
                    SchemeDistrictID = a.SchemeDistrictID,
                    CountryID = (int)a.CountryID
                }).ToList();
                return list;
            }
        }
    }
}



