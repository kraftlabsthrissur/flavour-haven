using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class StockValueDAL
    {
        public List<StockValueBO> GetItemStockValue()
        {
            List<StockValueBO> list = new List<StockValueBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.SpGetItemStockValueList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockValueBO
                {
                    ID = a.ID,
                    ItemName = a.ItemName,
                    OpeningStock = (decimal)a.OpeningStock,
                    OpeningValue = (decimal)a.OpeningStockValue,
                    OpeningRate = (decimal)a.OpeningRate,
                    StockIn = (decimal)a.StockIn,
                    NetStockValueIn = (decimal)a.NetStockValueIn,
                    NetStockRateIn = (decimal)a.StockInRate,
                    ClosingStock = (decimal)a.ClosingStock,
                    ClosingRate = (decimal)a.ClosingRate,
                    ClosingValue = (decimal)a.ClosingValue,
                    IssueRate = (decimal)a.IssueRate,
                    IssueStock = (decimal)a.IssueStock,
                    IssueValue = (decimal)a.IssueValue,
                    LastUpdatedDate = (DateTime)a.LastUpdatedDate,
                    LastUpdatedRate = (decimal)a.LastUpdatedRate,
                }).ToList();
                return list;
            }

        }

        public List<StockValueBO> Execute()
        {
            List<StockValueBO> list = new List<StockValueBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                //list = dEntity.SpPostClosingStockValues(DateTime.Now, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockValueBO
                //{
                //    //ID = a.ID,
                //    //ItemName = a.ItemName,
                //    //OpeningStock = (decimal)a.OpeningStock,
                //    //StockIn =(decimal) a.StockIn,
                //    //NetStockValueIn =(decimal) a.NetStockValueIn,
                //    //ClosingStock = (decimal)a.ClosingStock,
                //    //ClosingRate = (decimal)a.ClosingRate,
                //    //LastUpdatedDate = (DateTime)a.LastUpdatedDate,
                //    //LastUpdatedRate =(decimal) a.LastUpdatedRate,
                //    //OpeningValue = (decimal)a.OpeningStockValue

                //}).ToList();

                // dEntity.SpPostClosingStockValues(DateTime.Today, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                dEntity.SpStartCostingActivities(DateTime.Today, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                return list;
            }

        }

        public DatatableResultBO GetStockValueList(string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    var result = dbEntity.SpGetStockValueList(NameHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransDate = item.TransDate,
                                ItemID = item.ItemID,
                                ItemName = item.ItemName,
                                OpeningStock = (decimal)item.OpeningStock,
                                OpeningRate = (decimal)item.OpeningRate,
                                OpeningStockValue = (decimal)item.OpeningStockValue,
                                StockIn = (decimal)item.StockIn,
                                StockInRate = (decimal)item.StockInRate,
                                NetStockValueIn = (decimal)item.NetStockValueIn,
                                IssueStock = (decimal)item.IssueStock,
                                IssueRate = (decimal)item.IssueRate,
                                IssueValue = (decimal)item.IssueValue,
                                ClosingStock = (decimal)item.ClosingStock,
                                ClosingRate = (decimal)item.ClosingRate,
                                ClosingValue = (decimal)item.ClosingValue,
                                LastUpdatedRate = (decimal)item.LastUpdatedRate,
                                LastUpdatedDate = ((DateTime)item.LastUpdatedDate).ToString("dd-MMM-yyyy"),
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
