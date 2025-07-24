//file Created by prama on 9-4-2018
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
    public class StockRequestDAL
    {
        public bool SaveStockRequest(StockRequestBO stockRequestBO, List<StockRequestItemBO> stockRequestItems)
        {
            using (StockEntities dEntity = new StockEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "StockRequsitionIntra";
                        ObjectParameter SrId = new ObjectParameter("StockRequisitionID", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        if (stockRequestBO.IsDraft)
                        {
                            FormName = "DraftStockRequsitionIntra";
                        }

                        var j = dEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dEntity.SaveChanges();

                        var i = dEntity.SpCreateStockRequisition(
                            SerialNo.Value.ToString(),
                            stockRequestBO.Date,
                            stockRequestBO.IssueLocationID,
                            stockRequestBO.IssuePremiseID,
                            stockRequestBO.ReceiptLocationID,
                            stockRequestBO.ReceiptPremiseID,
                            GeneralBO.CreatedUserID,
                            stockRequestBO.IsDraft,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            SrId);

                        dEntity.SaveChanges();

                        if (SrId.Value != null)
                        {
                            foreach (var itm in stockRequestItems)
                            {
                                DateTime req = Convert.ToDateTime(itm.RequiredDate);
                                dEntity.SpCreateStockRequisitionTrans(
                                    Convert.ToInt32(SrId.Value),
                                    itm.ItemID,
                                    Convert.ToDecimal(itm.RequiredQty),
                                    itm.RequiredDate,
                                    itm.RequiredDate,
                                    itm.Remarks,
                                    itm.BatchTypeID,
                                    itm.Stock,
                                    itm.AverageSales,
                                    itm.SuggestedQty,
                                    itm.UnitID,
                                    itm.SecondaryUnit,
                                    itm.SecondaryUnitSize,
                                    itm.SecondaryQty,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID);
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

        public bool UpdateStockRequest(StockRequestBO stockRequestBO, List<StockRequestItemBO> stockRequestItems)
        {
            using (StockEntities dEntity = new StockEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {

                        var i = dEntity.SpUpdateStockRequisition(
                                stockRequestBO.ID,
                                DateTime.Now,
                                stockRequestBO.IssueLocationID,
                                stockRequestBO.IssuePremiseID,
                                stockRequestBO.ReceiptLocationID,
                                stockRequestBO.ReceiptPremiseID,
                                GeneralBO.CreatedUserID,
                                stockRequestBO.IsDraft,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID);


                        foreach (var itm in stockRequestItems)
                        {
                            DateTime req = Convert.ToDateTime(itm.RequiredDate);
                            dEntity.SpCreateStockRequisitionTrans(
                                stockRequestBO.ID,
                                itm.ItemID,
                                Convert.ToDecimal(itm.RequiredQty),
                                itm.RequiredDate,
                                itm.RequiredDate,
                                itm.Remarks,
                                itm.BatchTypeID,
                                itm.Stock,
                                itm.AverageSales,
                                itm.SuggestedQty,
                                itm.UnitID,
                                itm.SecondaryUnit,
                                itm.SecondaryUnitSize,
                                itm.SecondaryQty,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID);
                        }

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

        public List<StockRequestBO> GetStockRequestList()
        {
            List<StockRequestBO> list = new List<StockRequestBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.spGetStockRequisition(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockRequestBO
                {
                    ID = a.ID,
                    RequestNo = a.RequestNo,
                    Date = (DateTime)a.CreatedDate,
                    IssuePremiseName = a.IssuePremises,
                    ReceiptPremiseName = a.ReceiptPremises,
                    IssueLocationName = a.IssueLocation,
                    ReceiptLocationName = a.ReceiptLocation,
                    IsCancelled = (bool)a.Cancelled,
                    IsDraft = (bool)a.IsDraft,
                    IsProcessed = (bool)a.IsProcessed,
                    IsSuspended = (bool)a.IsSuspended
                }).ToList();
                return list;
            }

        }

        public List<StockRequestBO> GetStockRequestDetail(int ID)
        {
            List<StockRequestBO> list = new List<StockRequestBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.spGetStockRequisitionDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockRequestBO
                {
                    ID = a.ID,
                    RequestNo = a.RequestNo,
                    Date = (DateTime)a.CreatedDate,
                    IssuePremiseName = a.IssuePremises,
                    ReceiptPremiseName = a.ReceiptPremises,
                    IssueLocationName = a.IssueLocation,
                    ReceiptLocationName = a.ReceiptLocation,
                    ReceiptLocationID = (int)a.ReceiptLocationID,
                    ReceiptPremiseID = (int)a.ReceiptPremiseID,
                    IssueLocationID = (int)a.IssueLocationID,
                    IssuePremiseID = (int)a.IssuePremiseID,
                    IsCancelled = (bool)a.Cancelled,
                    IsDraft = (bool)a.IsDraft,
                    IsProcessed = (bool)a.IsProcessed,
                    IsSuspended = (bool)a.IsSuspended,
                    ProductionGroup = a.ProductionGroupName,
                    Batch = a.BatchNo
                }).ToList();
                return list;
            }

        }

        public List<StockRequestItemBO> GetStockRequestTrans(int ID)
        {
            List<StockRequestItemBO> list = new List<StockRequestItemBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                try
                {
                    list = dEntity.SpGetStockRequisitionTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockRequestItemBO
                    {
                        Name = a.ItemName,
                        Code = a.Code,
                        PartsNo = a.PatentNo,
                        Make = a.Make,
                        Model = a.Model,
                        Unit = a.Unit,
                        RequiredQty = (decimal)a.RequiredQty,
                        SecondaryQty = a.SecondaryQty,
                        SecondaryUnit = a.SecondaryUnit,
                        SecondaryUnits = a.SecondaryUnits,
                        RequiredDate = a.RequiredTime,
                        Remarks = a.Remarks,
                        BatchName = a.BatchName,
                        Stock = a.Stock,
                        AverageSales = a.AverageSales,
                        ItemID = (int)a.ItemID,
                        BatchTypeID = a.BatchTypeID == null ? 0 : (int)a.BatchTypeID,
                        UnitID = (int)a.UnitID,
                        SalesCategory = a.SalesCategory,
                        SuggestedQty = (decimal)a.SuggestedQty,
                        MalayalamName = a.MalayalamName
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
                return list;
            }

        }

        public List<StockRequestBO> GetUnProcessedSRList(int IssueLocationID, int IssuePremiseID, int ReceiptLocationID, int ReceiptPremiseID)
        {
            List<StockRequestBO> list = new List<StockRequestBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.SpGetUnProcessedStockRequisition(IssueLocationID, IssuePremiseID, ReceiptLocationID, ReceiptPremiseID,
                    GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockRequestBO
                    {
                        RequestNo = a.RequestNO,
                        IssueLocationName = a.IssueLocation,
                        IssuePremiseName = a.IssuePremises,
                        ReceiptLocationName = a.ReceiptLocation,
                        ReceiptPremiseName = a.ReceiptPremises,
                        IssueLocationID = a.IssueLocationID,
                        IssuePremiseID = a.IssuePremiseID,
                        ReceiptLocationID = a.ReceiptLocationID,
                        ReceiptPremiseID = a.ReceiptPremiseID,
                        Date = (DateTime)a.RequestedDate,
                        ID = a.ID,
                        ProductionGroup = a.ItemName,
                        Batch = a.Batch
                    }).ToList();
                return list;
            }
        }

        public List<StockRequestItemBO> GetUnProcessedSRTransList(string CommaSeperatedIDs)
        {
            List<StockRequestItemBO> list = new List<StockRequestItemBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                try
                {
                    var dataList = dEntity.SpGetBatchwiseItemsOfStockRequest(CommaSeperatedIDs, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    return dataList.Select(a => new StockRequestItemBO
                    {
                        Name = a.ItemName,
                        Code = a.Code,
                        PartsNo = a.PartsNo,
                        Make = a.Make,
                        Model = a.Model,
                        Unit = a.Unit,
                        BatchType = a.BatchTypeName,
                        BatchName = a.BatchNo,
                        RequestTransID = (int)a.StockRequisitionTransID,
                        RequestID = (int)a.StockRequisitionID,
                        ItemID = (int)a.ItemID,
                        BatchTypeID = (int)a.BatchTypeID,
                        BatchID = (int)a.BatchID,
                        RequestedQty = (decimal)a.RequestedQty,
                        Stock = (decimal)a.Stock,
                        Rate = (decimal)a.Rate,
                        IssueQty = (decimal)a.IssueQty,
                        CGSTPercentage = (decimal)a.CGSTPercentage,
                        SGSTPercentage = (decimal)a.SGSTPercentage,
                        IGSTPercentage = (decimal)a.IGSTPercentage,
                        TradeDiscountPercentage = (decimal)a.TradeDiscountPercentage,
                        StockRequisitionNo = a.StockRequisitionNo,
                        UnitID = (int)a.UnitID,
                        PrimaryUnit = a.PrimaryUnit,
                        SecondaryQty = (decimal)a.SecondaryQty,
                        SecondaryUnit = a.SecondaryUnit,
                        SecondaryUnitSize = (decimal)a.SecondaryUnitSize,
                        PackSize = a.PackSize.HasValue ? (int)a.PackSize.Value : 0,
                        PrimaryUnitID = a.PrimaryUnitID,

                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }

            }

        }

        public int Suspend(int ID, String Table)
        {
            using (StockEntities dbEntity = new StockEntities())
            {

                ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));
                int output;
                dbEntity.SpSuspendTransaction(ID, Table, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, ReturnValue);
                output = (int)ReturnValue.Value;
                return output;
            }
        }

        public int Cancel(int ID, string Table)
        {
            StockEntities dEntity = new StockEntities();
            return dEntity.SpCancelStockRequisition(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
        }

        public DatatableResultBO GetStockRequisitionList(string Type, string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    var result = dbEntity.SpGetStockRequisitionList(Type, TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                IsSuspendable = (bool)item.IsProcessed || (bool)item.IsSuspended || (bool)item.IsCancelled ? 0 : 1,
                                IsClonable = (bool)item.IsSuspended || (bool)item.IsCancelled ? 0 : 1
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
