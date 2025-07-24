//File created by prama on 19-4-2018

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;


namespace DataAccessLayer
{
    public class StockIssueDAL
    {

        public bool SaveStockIssue(StockIssueBO stockIssueBO, List<StockIssueItemBO> stockIssueItems, List<StockIssuePackingDetailsBO> PackingDetails)

        {
            using (StockEntities dEntity = new StockEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "StockIssue";
                        ObjectParameter SiId = new ObjectParameter("StockIssueID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));
                        if (stockIssueBO.IsDraft)
                        {
                            FormName = "DraftStockIssue";
                        }
                        var j = dEntity.SpUpdateSerialNo(FormName, stockIssueBO.IssueType, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var CreatedUserID = GeneralBO.CreatedUserID;

                        var i = dEntity.SpCreateStockIssue(
                            SerialNo.Value.ToString(),
                            stockIssueBO.Date,
                            stockIssueBO.RequestNo,
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
                            stockIssueBO.Remark,
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
                                dEntity.SpCreateStockIssueTrans(
                                    Convert.ToInt32(SiId.Value),
                                    item.StockRequestTransID,
                                    item.StockRequestID,
                                    item.ItemID,
                                    item.BatchID,
                                    item.BatchTypeID,
                                    item.IssueQty,
                                    item.RequestedQty,
                                    item.SecondaryIssueQty,
                                    item.SecondaryQty,
                                    item.SecondaryUnit,
                                    item.SecondaryUnitSize,
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
                                    item.Remark,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    ReturnValue);
                                if (Convert.ToInt32(ReturnValue.Value) == -1)
                                {
                                    throw new Exception("Stock requsition is already processed");
                                }
                                if (Convert.ToInt32(ReturnValue.Value) == -3)
                                {
                                    throw new Exception("Required quantity of " + item.Name + " already met");
                                }
                                if (Convert.ToInt32(ReturnValue.Value) <= 0)
                                {
                                    throw new OutofStockException("Item " + item.Name + " - " + item.BatchName + " is out of stock");
                                }
                            };

                            if (PackingDetails != null)
                            {
                                foreach (var item in PackingDetails)
                                {
                                    dEntity.SpCreatePackingDetails(
                                        Convert.ToInt32(SiId.Value),
                                        "StockIssue",
                                        item.PackSize,
                                        item.PackUnitID,
                                        item.Quantity,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID
                                        );

                                };
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        //transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public bool UpdateStockIssue(StockIssueBO stockIssueBO, List<StockIssueItemBO> stockIssueItems, List<StockIssuePackingDetailsBO> PackingDetails)

        {
            using (StockEntities dEntity = new StockEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {

                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));

                        var i = dEntity.SpUpdateStockIssue(
                            stockIssueBO.ID,
                            DateTime.Now,
                            stockIssueBO.RequestNo,
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
                            stockIssueBO.Remark,
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
                                dEntity.SpCreateStockIssueTrans(
                                    stockIssueBO.ID,
                                    item.StockRequestTransID,
                                    item.StockRequestID,
                                    item.ItemID,
                                    item.BatchID,
                                    item.BatchTypeID,
                                    item.IssueQty,
                                    item.RequestedQty,
                                    item.SecondaryIssueQty,
                                    item.SecondaryQty,
                                    item.SecondaryUnit,
                                    item.SecondaryUnitSize,
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
                                    item.Remark,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    ReturnValue);
                                if (Convert.ToInt32(ReturnValue.Value) == -1)
                                {
                                    throw new Exception("Stock requsition is already processed");
                                }
                                if (Convert.ToInt32(ReturnValue.Value) == -3)
                                {
                                    throw new Exception("Required quantity of " + item.Name + " already met");
                                }
                                if (Convert.ToInt32(ReturnValue.Value) <= 0)
                                {
                                    throw new OutofStockException("Item " + item.Name + " - " + item.BatchName + " is out of stock");
                                }
                            };
                            foreach (var item in PackingDetails)
                            {
                                dEntity.SpCreatePackingDetails(
                                      stockIssueBO.ID,
                                    "StockIssue",
                                    item.PackSize,
                                    item.PackUnitID,
                                    item.Quantity,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                    );

                            };
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

        public List<StockIssueBO> GetStockIssueList()

        {
            List<StockIssueBO> list = new List<StockIssueBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.spGetStockIssue(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockIssueBO
                {
                    ID = a.ID,
                    IssueNo = a.IssueNo,
                    Date = (DateTime)a.IssueDate,
                    IssuePremiseName = a.IssuePremises,
                    ReceiptPremiseName = a.ReceiptPremises,
                    IssueLocationName = a.IssueLocation,
                    ReceiptLocationName = a.ReceiptLocation,
                    IsCancelled = (bool)a.Cancelled,
                    IsDraft = (bool)a.IsDraft,
                    IsProcessed = (bool)a.IsProcessed

                }).ToList();


                return list;
            }

        }

        public List<StockIssueBO> GetStockIssueDetail(int ID)
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
                    Batch = a.BatchNo,
                    Remark = a.Remark,
                    VehicleNo = a.VehicleNo,
                    IssueLocationGSTNo = a.IssueLocationGSTNo,
                    ReceiptLocationGSTNo = a.ReceiptLocationGSTNo
                }).ToList();
                return list;
            }
        }

        public List<StockIssueItemBO> GetIssueTrans(int ID)
        {
            List<StockIssueItemBO> list = new List<StockIssueItemBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                var dataList = dEntity.SpGetStockIssueTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();

                return dataList.Select(a => new StockIssueItemBO
                {
                    Name = a.ItemName,
                    Code = a.Code,
                    PartsNo = a.PatentNo,
                    Make = a.Make,
                    Model = a.Model,
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
                    Category = a.Category,
                    PackSize = a.PackSize.HasValue ? a.PackSize.Value : 0,
                    SecondaryUnit = a.SecondaryUnit,
                    SecondaryQty = a.SecondaryQty,
                    SecondaryUnitSize = a.SecondaryUnitSize,
                    SecondaryIssueQty = a.SecondaryIssueQty,
                    MalayalamName = a.MalayalamName,
                    PrimaryUnit = a.PrimaryUnit,
                    PrimaryUnitID = a.PrimaryUnitID.HasValue ? a.PrimaryUnitID.Value : 0,
                    ExpiryDate = a.ExpiryDate,
                    Remark = a.Remark
                }).ToList();
            }
        }

        public List<StockIssuePackingDetailsBO> GetPackingDetails(int ID, string Type)
        {
            List<StockIssuePackingDetailsBO> list = new List<StockIssuePackingDetailsBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.SpGetPackingDetails(ID, Type, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a =>

                new StockIssuePackingDetailsBO
                {
                    PackSize = a.PackSize,
                    PackUnit = a.Unit,
                    PackUnitID = (int)a.UnitID,
                    Quantity = (decimal)a.Quantity
                }).ToList();
                return list;
            }



        }

        public List<StockIssueBO> GetUnProcessedSIList(int IssueLocationID, int IssuePremiseID, int ReceiptLocationID, int ReceiptPremiseID)
        {
            List<StockIssueBO> list = new List<StockIssueBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.SpGetUnprocessedStockIssue(IssueLocationID, IssuePremiseID, ReceiptLocationID, ReceiptPremiseID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockIssueBO
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
                    Batch = a.Batch,
                    NetAmount = a.NetAmount
                }).ToList();
                return list;
            }
        }

        public List<StockIssueItemBO> GetUnProcessedSITransList(int ID)
        {
            List<StockIssueItemBO> list = new List<StockIssueItemBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.SpGetStockIssueItemForStockReceipt(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockIssueItemBO
                {
                    Name = a.Name,
                    Unit = a.Unit,
                    StockIssueTransID = a.StockIssueTransID,
                    IssueQty = (decimal)a.IssueQty,
                    StockIssueID = (int)a.StockIssueID,
                    BatchType = a.BatchTypeName,
                    RequestedQty = (int)a.RequestedQty,
                    SecondaryQty = a.SecondaryQty,
                    SecondaryUnit = a.SecondaryUnit,
                    SecondaryUnitSize = a.SecondaryUnitSize,
                    SecondaryIssueQty = a.SecondaryIssueQty,
                    ID = a.ItemID,
                    BatchID = (int)a.BatchID,
                    BatchName = a.BatchNo,
                    BatchTypeID = (int)a.BatchTypeID,
                    Rate = (decimal)a.Rate,
                    NetAmount = (decimal)a.NetAmount,
                    GrossAmount = a.GrossAmount,
                    TradeDiscount = a.TradeDiscount,
                    TaxableAmount = a.TaxableAmount,
                    CGSTPercentage = a.CGSTPercentage,
                    IGSTPercentage = a.IGSTPercentage,
                    SGSTPercentage = a.SGSTPercentage,
                    IGSTAmount = a.IGSTAmount,
                    CGSTAmount = a.CGSTAmount,
                    SGSTAmount = a.SGSTAmount,
                    TradeDiscountPercentage = a.TradeDiscountPercentage,
                    BasicPrice = a.BasicPrice,
                    UnitID = (int)a.UnitID,


                }).ToList();
                return list;
            }

        }

        public int Cancel(int ID, string Table)
        {
            using (StockEntities dbEntity = new StockEntities())
            {
                return dbEntity.SpCancelStockIssue(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
            }
        }

        public List<StockIssueItemBO> GetBatchwiseItem(int ItemID, int BatchTypeID, decimal Qty, int WarehouseID, int UnitID)
        {
            using (StockEntities dbEntity = new StockEntities())
            {
                try
                {
                    var DataList = dbEntity.SpGetBatchwiseItemsForStockIssue(ItemID, BatchTypeID, WarehouseID, Qty, UnitID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();

                    return DataList.Select(a => new StockIssueItemBO()
                    {
                        ItemID = (int)a.ItemID,
                        Code = a.Code,
                        Name = a.ItemName,
                        PartsNo = a.PartsNo,
                        Make = a.Make,
                        Model = a.Model,
                        BatchID = (int)a.BatchID,
                        BatchName = a.BatchNo,
                        BatchTypeID = (int)a.BatchTypeID,
                        BatchType = a.BatchTypeName,
                        Stock = (decimal)a.Stock,
                        RequestedQty = (decimal)a.RequestedQty,
                        IssueQty = (decimal)a.IssueQty,
                        Rate = (decimal)a.Rate,
                        CGSTPercentage = (decimal)a.CGSTPercentage,
                        SGSTPercentage = (decimal)a.SGSTPercentage,
                        IGSTPercentage = (decimal)a.IGSTPercentage,
                        TradeDiscountPercentage = (decimal)a.TradeDiscountPercentage,
                        Unit = a.Unit,
                        PrimaryUnit = a.PrimaryUnit,
                        SecondaryUnits = a.SecondaryUnits,
                        PackSize = a.PackSize.HasValue ? a.PackSize.Value : 0,
                        PrimaryUnitID = a.PrimaryUnitID
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public DatatableResultBO GetStockIssueList(string Type, string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    var result = dbEntity.SpGetStockIssueList(Type, TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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

        public List<StockIssueItemBO> GetItemsToGrid(List<StockIssueItemBO> stockIssueItems, int IssuePremiseID)
        {
            using (StockEntities dbEntity = new StockEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in stockIssueItems)
                        {
                            dbEntity.SpCreateStockIssueTemplateItem(
                                item.ItemID,
                                IssuePremiseID,
                                item.BatchTypeID,
                                item.IssueQty,
                                item.UnitID
                                );
                        }

                        var list = dbEntity.SpGetBatchwiseItemsOfIssueTemplate(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockIssueItemBO
                        {
                            ItemID = (int)a.ItemID,
                            Name = a.ItemName,
                            BatchID = (int)a.BatchID,
                            BatchName = a.BatchNo,
                            BatchTypeID = (int)a.BatchTypeID,
                            BatchType = a.BatchTypeName,
                            Stock = (decimal)a.Stock,
                            RequestedQty = (decimal)a.RequestedQty,
                            IssueQty = (decimal)a.IssueQty,
                            Rate = (decimal)a.Rate,
                            CGSTPercentage = (decimal)a.CGSTPercentage,
                            SGSTPercentage = (decimal)a.SGSTPercentage,
                            IGSTPercentage = (decimal)a.IGSTPercentage,
                            TradeDiscountPercentage = (decimal)a.TradeDiscountPercentage,
                            Unit = a.Unit,
                            UnitID = (int)a.UnitID,
                            PackSize = (decimal)a.PackSize,
                            PrimaryUnit = a.PrimaryUnit,
                            PrimaryUnitID = (int)a.PrimaryUnitID
                        }).ToList();

                        transaction.Commit();
                        return list;
                    }

                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;

                    }
                }
            }
        }

        public List<StockIssueBO> GetIssueNoAutoCompleteForReport(string CodeHint, DateTime FromDate, DateTime ToDate)
        {
            List<StockIssueBO> StockIssue = new List<StockIssueBO>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    StockIssue = dbEntity.SpGetStockIssueCdeAutoComplete(CodeHint, FromDate, ToDate, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockIssueBO
                    {
                        ID = a.ID,
                        IssueNo = a.IssueNo,
                    }).ToList();
                    return StockIssue;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}