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
   public class ServiceItemReceiptDAL
    {
        public int Save(StockReceiptBO stockReceiptBO, List<StockReceiptItemBO> stockReceiptItems)
        {
            using (StockEntities dEntity = new StockEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {
                        ObjectParameter StockReceiptID = new ObjectParameter("StockReceiptID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        var j = dEntity.SpUpdateSerialNo("ServiceItemReceipt", stockReceiptBO.ReceiptType, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

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
                                dEntity.SpCreateServiceItemReceiptTrans(
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
                                    GeneralBO.ApplicationID);
                            }

                        };
                        transaction.Commit();
                        return Convert.ToInt32(StockReceiptID.Value);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<StockReceiptBO> GetServiceItemReceiptDetail(int ID)
        {
            List<StockReceiptBO> list = new List<StockReceiptBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.spGetServiceItemReceiptDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockReceiptBO
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

        public List<StockReceiptItemBO> GetServiceItemReceiptTrans(int ID)
        {
            List<StockReceiptItemBO> list = new List<StockReceiptItemBO>();
            using (StockEntities dEntity = new StockEntities())
            {
                list = dEntity.SpGetServiceItemReceiptTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockReceiptItemBO
                {
                    Name = a.ItemName,
                    Unit = a.Unit,
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

        public bool IsServiceOrStockReceipt(int ID, string Type)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {

                    ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
                    dbEntity.SpIsServiceOrStockReceipt(
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

        public DatatableResultBO GetServiceItemReceiptList(string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    var result = dbEntity.SpGetServiceItemReceiptList(TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
