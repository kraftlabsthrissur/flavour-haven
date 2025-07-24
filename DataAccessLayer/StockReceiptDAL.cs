//File created by prama on 29-6-18
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
    public class StockReceiptDAL
    {

        public string SaveStockReceipt(StockReceiptBO stockReceiptBO, List<StockReceiptItemBO> stockReceiptItems)
        {
            using (StockEntities dEntity = new StockEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {
                        ObjectParameter StockReceiptID = new ObjectParameter("StockReceiptID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));

                        var j = dEntity.SpUpdateSerialNo("StockReceipt", stockReceiptBO.ReceiptType, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var i = dEntity.SpCreateStockReceipt(
                                SerialNo.Value.ToString(),
                                stockReceiptBO.Date,
                                stockReceiptBO.NetAmount,
                                stockReceiptBO.IsService,
                                GeneralBO.CreatedUserID,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                StockReceiptID);

                        if (StockReceiptID.Value != null)
                        {
                            foreach (var itm in stockReceiptItems)
                            {
                                dEntity.SpCreateStockReceiptTrans(
                                    Convert.ToInt32(StockReceiptID.Value),
                                    itm.StockIssueTransID,
                                    itm.StockIssueID,
                                    itm.ReceiptLocationID,
                                    itm.ReceiptPremiseID,
                                    itm.IssueLocationID,
                                    itm.IssuePremiseID,
                                    itm.ItemID,
                                    itm.BatchID,
                                    itm.BatchTypeID,
                                    itm.ReceiptQty,
                                    itm.IssueQty,
                                    itm.SecondaryIssueQty,
                                    itm.SecondaryUnit,
                                    itm.SecondaryUnitSize,
                                    itm.SecondaryReceiptQty,
                                    itm.Rate,
                                    itm.NetAmount,
                                    itm.BasicPrice,
                                    itm.GrossAmount,
                                    itm.TradeDiscountPercent,
                                    itm.TradeDiscount,
                                    itm.TaxableAmount,
                                    itm.SGSTPercentage,
                                    itm.CGSTPercentage,
                                    itm.IGSTPercentage,
                                    itm.SGSTAmount,
                                    itm.CGSTAmount,
                                    itm.IGSTAmount,
                                    itm.UnitID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    ReturnValue
                                    );
                                if (Convert.ToInt32(ReturnValue.Value) == -1)
                                {
                                    throw new Exception("Stock issue is already processed");
                                }
                            }

                        };

                        transaction.Commit();
                        return StockReceiptID.Value.ToString();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<StockReceiptBO> GetStockReceiptList()
        {
            List<StockReceiptBO> list = new List<StockReceiptBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.spGetStockReceipt(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockReceiptBO
                {
                    ID = a.ID,
                    ReceiptNo = a.ReceiptNo,
                    Date = (DateTime)a.ReceiptDate,
                    IsCancelled = a.Cancelled,
                    //   IsDraft = (bool)a.d,
                    //    IsProcessed = (bool)a.IsProcessed
                }).ToList();
                return list;
            }
        }

        public List<StockReceiptBO> GetStockReceiptDetail(int ReturnID)
        {
            List<StockReceiptBO> list = new List<StockReceiptBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.spGetStockReceiptDetail(ReturnID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockReceiptBO
                {
                    ID = a.ID,
                    ReceiptNo = a.ReceiptNo,
                    Date = (DateTime)a.ReceiptDate,
                    IsCancelled = a.Cancelled,
                    ReceiptLocationName = a.ReceiptLocation,
                    ReceiptPremiseName = a.ReceiptPremisesName,
                    IssueLocationName = a.IssueLocation,
                    IssuePremiseName = a.IssuePremisesName,
                    NetAmount = a.NetAmount
                }).ToList();
                return list;
            }
        }

        public List<StockReceiptItemBO> GetStockReceiptTrans(int ID)
        {
            List<StockReceiptItemBO> list = new List<StockReceiptItemBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.SpGetStockReceiptTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockReceiptItemBO
                {
                    Name = a.ItemName,
                    SecondaryReceiptQty = a.SecondaryReceiptQty,
                    SecondaryUnitSize = a.SecondaryUnitSize,
                    SecondaryIssueQty = a.SecondaryIssueQty,
                    SecondaryUnit = a.SecondaryUnit,
                    Unit = a.Unit,
                    BatchType = a.BatchTypeName,
                    BatchName = a.BatchName,
                    IssueQty = (decimal)a.IssueQty,
                    ReceiptQty = (decimal)a.ReceiptQty,
                    BasicPrice = (decimal)a.BasicPrice,
                    GrossAmount = (decimal)a.GrossAmount,
                    TradeDiscountPercent = (decimal)a.TradeDiscountPercentage,
                    TradeDiscount = (decimal)a.TradeDiscount,
                    TaxableAmount = (decimal)a.TaxableAmount,
                    CGSTPercentage = (decimal)a.CGSTPercentage,
                    SGSTPercentage = (decimal)a.SGSTPercentage,
                    IGSTPercentage = (decimal)a.IGSTPercentage,
                    CGSTAmount = (decimal)a.CGSTAmount,
                    SGSTAmount = (decimal)a.SGSTAmount,
                    IGSTAmount = (decimal)a.IGSTAmount,
                    NetAmount = (decimal)a.NetAmount,
                }).ToList();
                return list;
            }
        }

        public DatatableResultBO GetStockReceiptList(string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    var result = dbEntity.SpGetStockReceiptList(TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
    }
}
