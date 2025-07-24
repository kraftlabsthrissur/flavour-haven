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
    public class SalesForecastDAL
    {
        public int Process(SalesForecastBO SalesForecast)
        {
            try
            {
                using (SnopEntities dbEntity = new SnopEntities())
                {
                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                    ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));

                    var j = dbEntity.SpUpdateSerialNo("SalesForecast", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    dbEntity.SpProcessSalesForecast(
                            SalesForecast.ID,
                            SerialNo.Value.ToString(),
                            SalesForecast.TransDate,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            ReturnValue
                        );
                    int SalesForecastID = Convert.ToInt32(ReturnValue.Value);
                    return SalesForecastID;
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetSalesForecasts(string TransNoHint, string MonthHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SnopEntities dbEntity = new SnopEntities())
                {
                    var result = dbEntity.SpGetSalesForecasts(TransNoHint, MonthHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = (int)item.ID,
                                TransNo = item.TransNO,
                                TransDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                IsFinalize = (bool)item.IsFinalize,
                                Month = item.MonthName,
                                Year = item.FinYear
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

        public DatatableResultBO GetSalesForecastItem(int SalesForecastID, string ItemNameHint, string CodeHint, string SalesCategoryHint, string LocationNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SnopEntities dbEntity = new SnopEntities())
                {

                    var result = dbEntity.SpGetSalesForecastItems(SalesForecastID, ItemNameHint, CodeHint, SalesCategoryHint, LocationNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ItemID = (int)item.ItemID,
                                ItemName = item.Name,
                                ItemCode = item.Code,
                                LocationName = item.LocationName,
                                SalesCategory = item.SalesCategory,
                                FinalForecast = item.FinalForecast,
                                ComputedForecast = item.ComputedForecast,
                                Unit = item.SalesUnit,
                                FinalForecastInkgs=item.FinalForecastInkgs,
                                ComputedForecastInKgs=item.ComputedForecastInKgs,
                                FinalForecastValue=item.FinalForecastValue,
                                ComputedForecastValue=item.ComputedForecastValue,
                                SalesForecastID = (int)item.SalesForecastID
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

        public int IsSalesForecastExist(int Month)
        {
            try
            {
                using (SnopEntities dbEntity = new SnopEntities())
                {

                    ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
                    dbEntity.SpIsSalesForecastExist(
                            Month,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            ReturnValue
                        );
                    int SalesForecastID = Convert.ToInt32(ReturnValue.Value);
                    return SalesForecastID;
                };

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int UploadSalesForecastItems(int ID, string XMLItems)
        {
            try
            {
                using (SnopEntities dbEntity = new SnopEntities())
                {
                    dbEntity.SpUpdateSalesForecast(
                                ID,
                                XMLItems,
                                GeneralBO.ApplicationID
                            );
                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public SalesForecastBO GetSalesForecast(int SalesForecastID)
        {
            try
            {
                using (SnopEntities dbEntity = new SnopEntities())
                {

                    return dbEntity.SpGetSalesForecast(SalesForecastID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new SalesForecastBO
                    {
                        TransDate = (DateTime)k.TransDate,
                        TransNo = k.TransNo,
                        ID = k.ID,
                        Month = k.MonthName,
                        IsFinalize = (bool)k.IsFinalize
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Save(int ID)
        {
            try
            {
                using (SnopEntities dbEntity = new SnopEntities())
                {
                    dbEntity.SpSaveSalesForecast(
                            ID,
                            GeneralBO.ApplicationID
                        );
                };
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
