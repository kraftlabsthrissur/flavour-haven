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
   public class ServiceItemIssueDAL
    {
        public decimal GetTradeDiscountPercent()
        {
            try
            {
                ObjectParameter TradeDiscountPercent = new ObjectParameter("TradeDiscountPercent", typeof(decimal));
                using (StockEntities dbEntity = new StockEntities())
                {
                    var result = dbEntity.SpGetTradeDiscountPercent(
                                  GeneralBO.ApplicationID,
                                  TradeDiscountPercent
                                  );
                    return Convert.ToDecimal(TradeDiscountPercent.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Save(StockIssueBO stockIssueBO, List<StockIssueItemBO> stockIssueItems)
        {
            using (StockEntities dEntity = new StockEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "ServiceItemIssue";
                        ObjectParameter SiId = new ObjectParameter("StockIssueID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));

                        if (stockIssueBO.IsDraft)
                        {
                            FormName = "DraftServiceItemIssue";
                        }
                        var j = dEntity.SpUpdateSerialNo(FormName, stockIssueBO.IssueType, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var CreatedUserID = GeneralBO.CreatedUserID;

                        var i = dEntity.SpCreateServiceItemIssue(
                            SerialNo.Value.ToString(),
                            stockIssueBO.Date,
                            stockIssueBO.IssueLocationID,
                            stockIssueBO.IssuePremiseID,
                            stockIssueBO.ReceiptLocationID,
                            stockIssueBO.ReceiptPremiseID,
                            stockIssueBO.GrossAmount,
                            stockIssueBO.TradeDiscount,
                            stockIssueBO.TaxableAmount,
                            stockIssueBO.SGSTAmount,
                            stockIssueBO.CGSTAmount,
                            stockIssueBO.IGSTAmount,
                            stockIssueBO.RoundOff,
                            stockIssueBO.NetAmount,
                            GeneralBO.CreatedUserID,
                            stockIssueBO.IsDraft,
                            stockIssueBO.IsService,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            SiId);

                        if (SiId.Value != null)
                        {
                            foreach (var item in stockIssueItems)
                            {
                                dEntity.SpCreateServiceItemIssueTrans(
                                    Convert.ToInt32(SiId.Value),
                                    item.ItemID,
                                    item.IssueQty,
                                    item.Rate,
                                    item.BasicPrice,
                                    item.GrossAmount,
                                    item.TradeDiscountPercentage,
                                    item.TradeDiscount,
                                    item.TaxableAmount,
                                    item.SGSTPercentage,
                                    item.CGSTPercentage,
                                    item.IGSTPercentage,
                                    item.SGSTAmount,
                                    item.CGSTAmount,
                                    item.IGSTAmount,
                                    item.NetAmount,
                                    item.UnitID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                    );
                            }
                        };

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public bool Update(StockIssueBO stockIssueBO, List<StockIssueItemBO> stockIssueItems)
        {
            using (StockEntities dEntity = new StockEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));

                        var i = dEntity.SpUpdateServiceItemIssue(
                            stockIssueBO.ID,
                            stockIssueBO.Date,
                            stockIssueBO.IssueLocationID,
                            stockIssueBO.IssuePremiseID,
                            stockIssueBO.ReceiptLocationID,
                            stockIssueBO.ReceiptPremiseID,
                            stockIssueBO.GrossAmount,
                            stockIssueBO.TradeDiscount,
                            stockIssueBO.TaxableAmount,
                            stockIssueBO.SGSTAmount,
                            stockIssueBO.CGSTAmount,
                            stockIssueBO.IGSTAmount,
                            stockIssueBO.RoundOff,
                            stockIssueBO.NetAmount,
                            GeneralBO.CreatedUserID,
                            stockIssueBO.IsDraft,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                             );

                        if (stockIssueBO.ID != 0)
                        {
                            foreach (var item in stockIssueItems)
                            {
                                dEntity.SpCreateServiceItemIssueTrans(
                                    stockIssueBO.ID,
                                     item.ItemID,
                                    item.IssueQty,
                                    item.Rate,
                                    item.BasicPrice,
                                    item.GrossAmount,
                                    item.TradeDiscountPercentage,
                                    item.TradeDiscount,
                                    item.TaxableAmount,
                                    item.SGSTPercentage,
                                    item.CGSTPercentage,
                                    item.IGSTPercentage,
                                    item.SGSTAmount,
                                    item.CGSTAmount,
                                    item.IGSTAmount,
                                    item.NetAmount,
                                    item.UnitID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                     );
                            }
                        };

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public DatatableResultBO GetServiceItemIssueList(string Type, string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    var result = dbEntity.SpGetServiceItemIssueList(Type, TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransNo = item.TransNo,
                                TransDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                IssueLocation = item.IssueLocation,
                                IssuePremise = item.IssuePremise,
                                ReceiptLocation = item.ReceiptLocation,
                                ReceiptPremise = item.ReceiptPremise,
                                Status = item.Status,
                                Amount = item.Amount
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

        public List<StockIssueBO> GetServiceItemIssueDetail(int ID)
        {
            List<StockIssueBO> list = new List<StockIssueBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.spGetStockIssueDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockIssueBO
                {
                    ID = a.ID,
                    IssueNo = a.IssueNo,
                    Date = (DateTime)a.IssueDate,
                    IssuePremiseName = a.IssuePremises,
                    ReceiptPremiseName = a.ReceiptPremises,
                    IssueLocationName = a.IssueLocation,
                    ReceiptLocationName = a.ReceiptLocation,
                    IssueLocationID = a.IssueLocationID,
                    ReceiptLocationID = a.ReceiptLocationID,
                    IssuePremiseID = a.IssuePremiseID,
                    ReceiptPremiseID = a.ReceiptPremiseID,
                    GrossAmount = a.GrossAmount,
                    TradeDiscount = a.TradeDiscount,
                    TaxableAmount = a.TaxableAmount,
                    CGSTAmount = a.CGSTAmount,
                    SGSTAmount = a.SGSTAmount,
                    IGSTAmount = a.IGSTAmount,
                    RoundOff = a.RoundOff,
                    NetAmount = a.NetAmount,
                    RequestNo = a.RequestNo,
                    IsCancelled = (bool)a.Cancelled,
                    IsDraft = (bool)a.IsDraft,
                    IsProcessed = (bool)a.IsProcessed,
                    ProductionGroup = a.ProductionGroupName,
                    Batch = a.BatchNo
                }).ToList();
                return list;
            }
        }

        public List<StockIssueItemBO> GetServiceItemIssueTrans(int ID)
        {
            List<StockIssueItemBO> list = new List<StockIssueItemBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.SpGetStockIssueTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a =>

                new StockIssueItemBO
                {
                    Name = a.ItemName,
                    Unit = a.Unit,
                    BatchType = a.BatchTypeName,
                    IssueQty = (decimal)a.IssueQty,
                    BatchName = a.BatchName,
                    RequestedQty = (decimal)a.RequestedQty,
                    IssueDate = (DateTime)a.IssueDate,
                    ItemID = (int)a.ItemID,
                    BatchID = (int)a.BatchID,
                    BatchTypeID = (int)a.BatchTypeID,
                    Stock = (decimal)a.Stock,
                    StockRequestTransID = (int)a.StockRequestTransID,
                    StockRequestID = a.StockRequestID,
                    Rate = (decimal)a.Rate,
                    BasicPrice = a.BasicPrice,
                    GrossAmount = a.GrossAmount,
                    TradeDiscountPercentage = a.TradeDiscountPercentage,
                    TradeDiscount = a.TradeDiscount,
                    TaxableAmount = a.TaxableAmount,
                    CGSTPercentage = a.CGSTPercentage,
                    SGSTPercentage = a.SGSTPercentage,
                    IGSTPercentage = a.IGSTPercentage,
                    CGSTAmount = a.CGSTAmount,
                    SGSTAmount = a.SGSTAmount,
                    IGSTAmount = a.IGSTAmount,
                    NetAmount = (decimal)a.NetAmount,
                    StockRequisitionNo = a.StockRequisitionNo,
                    UnitID = (int)a.UnitID,
                    MalayalamName = a.MalayalamName
                }).ToList();
                return list;
            }
        }


        public bool IsServiceOrStockIssue(int ID, string Type)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {

                    ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
                    dbEntity.SpIsServiceOrStockIssue(
                            ID,
                            Type,
                            GeneralBO.ApplicationID,
                            ReturnValue
                        );
                    return Convert.ToBoolean(ReturnValue.Value);
                };

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<StockIssueBO> GetUnProcessedServiceItemIssueList(int IssueLocationID, int IssuePremiseID, int ReceiptLocationID, int ReceiptPremiseID)
        {
            List<StockIssueBO> list = new List<StockIssueBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.SpGetUnprocessedServiceItemIssue(IssueLocationID, IssuePremiseID, ReceiptLocationID, ReceiptPremiseID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockIssueBO
                {
                    IssueNo = a.IssueNo,
                    IssueLocationName = a.IssueLocation,
                    IssuePremiseName = a.IssuePremises,
                    ReceiptLocationName = a.ReceiptLocation,
                    ReceiptPremiseName = a.ReceiptPremises,
                    IssueLocationID = a.IssueLocationID,
                    IssuePremiseID = a.IssuePremiseID,
                    ReceiptLocationID = a.ReceiptLocationID,
                    ReceiptPremiseID = a.ReceiptPremiseID,
                    Date = (DateTime)a.IssueDate,
                    ID = a.ID,
                    ProductionGroup = a.ItemName,
                    NetAmount = a.NetAmount
                }).ToList();
                return list;
            }
        }

    }
}
