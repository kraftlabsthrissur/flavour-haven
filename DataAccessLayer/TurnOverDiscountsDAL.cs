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
   public class TurnOverDiscountsDAL
    {
        public DatatableResultBO GetCustomerListForLocation(int CustomerLocationID, string CodeHint, string NameHint, string LocationHint, string CustomerCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetCustomerListForLocation(CustomerLocationID, CodeHint, NameHint, LocationHint, CustomerCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Name = (item.Name).Trim(),
                                Location = item.Location,
                                StateID = item.StateID,
                                IsGSTRegistered = item.IsGSTRegistered,
                                PriceListID = item.PriceListID,
                                CustomerCategory = item.CustomerCategory,
                                Address = item.Address,
                                DistrictID = item.DistrictID,
                                CountryID = item.CountryID,
                                SchemeID = item.SchemeID

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

        public int Save(List<DiscountItemBO> Items, TurnOverDiscountsBO turnOverDiscountBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ObjectParameter ReturnValue = new ObjectParameter("TODID", typeof(int));
                    var i = dbEntity.SpCreateTurnOverDiscount(
                       turnOverDiscountBO.Date,
                       GeneralBO.LocationID,
                       GeneralBO.ApplicationID,
                       ReturnValue
                       );
                    foreach (var item in Items)
                    {
                        dbEntity.SpCreateTurnOverDiscountDetails(
                       Convert.ToInt32(ReturnValue.Value),
                       item.Code,
                       item.TurnOverDiscount,
                       item.FromDate,
                       item.ToDate,
                       item.Location,
                       item.Month,
                       GeneralBO.CreatedUserID,
                       GeneralBO.FinYear,
                       GeneralBO.LocationID,
                       GeneralBO.ApplicationID
                      );
                    }
                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TurnOverDiscountsBO> GetTurnOverDiscountList()
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                return dEntity.SpGetTurnOverDiscount().Select(a => new TurnOverDiscountsBO
                {
                    ID = a.ID,
                    Date = a.Date
                }).ToList();

            }
        }

        public List<TurnOverDiscountsBO> GetTurnOverDiscountDetails(int ID)
        {
            try
            {
                List<TurnOverDiscountsBO> TurnOverDiscounts = new List<TurnOverDiscountsBO>();

                using (MasterEntities dEntity = new MasterEntities())
                {
                    TurnOverDiscounts = dEntity.SpGetTurnOverDiscountsByID(ID).Select(a => new TurnOverDiscountsBO
                    {
                        ID = a.ID,
                        Date = a.Date

                    }).ToList();
                    return TurnOverDiscounts;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DiscountItemBO> GetTurnOverDiscountTransDetails(int ID)
        {
            try
            {
                List<DiscountItemBO> DiscountList = new List<DiscountItemBO>();

                using (MasterEntities dEntity = new MasterEntities())
                {
                    DiscountList = dEntity.SpGetTurnOverDiscountTransDetails(ID).Select(a => new DiscountItemBO
                    {
                        CustomerID = a.CustomerID,
                        CustomerName = a.CustomerName,
                        TurnOverDiscount = a.Amount,
                        FromDate = a.StartDate,
                        ToDate = a.EndDate,
                        Location = a.LocationName,
                        Code=a.Code,
                        Month=a.Month
                    }).ToList();
                    return DiscountList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(List<DiscountItemBO> Items, TurnOverDiscountsBO turnOverDiscountBO)
        {
            try
            {


                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var i = dbEntity.SpUpdateTurnOverDiscount(
                  turnOverDiscountBO.ID,
                  GeneralBO.LocationID,
                  GeneralBO.ApplicationID,
                  GeneralBO.CreatedUserID

                  );
                    foreach (var item in Items)
                    {
                       dbEntity.SpCreateTurnOverDiscountDetails(
                       turnOverDiscountBO.ID,
                       item.Code,
                       item.TurnOverDiscount,
                       item.FromDate,
                       item.ToDate,
                       item.Location,
                       item.Month,
                       GeneralBO.CreatedUserID,
                       GeneralBO.FinYear,
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
