using System;
using System.Collections.Generic;
using System.Linq;

using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity.Core.Objects;
namespace DataAccessLayer
{
    public class QCProductionDAL
    {
        public List<QCTestBO> GetQCTestDetails(int ID, string Type)
        {
            try
            {
                using (ProductionEntities db = new ProductionEntities())
                {
                    return db.spGetQCProductionTrans(ID, Type, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new QCTestBO
                    {
                        ID = a.ID,
                        QCID = a.QCID,
                        QCTestID = Convert.ToInt32(a.QCTestID),
                        TestName = a.Name,
                        RangeFrom = (decimal)a.RangeFrom,
                        RangeTo = a.RangeTo,
                        DefinedResult = a.DefinedResult,
                        ActualResult = a.ActualResult,
                        ActualValue = a.ActualValue,
                        Remarks = a.Remarks,
                        IsMandatory = (bool)a.IsMandatory
                    }).ToList();

                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<QCItemBO> GetQCItemDetails(int ID)
        {
            try
            {
                using (ProductionEntities db = new ProductionEntities())
                {
                    return db.spGetQCProductionMaterial(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new QCItemBO
                    {
                        ID = a.ID,
                        QCNo = a.QCNo,
                        QCDate = a.QCDate,
                        ProductionID = (int)a.ProductionID,
                        ProductionIssueID = (int)a.ProductionIssueID,
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitName = a.UnitName,
                        ReferenceNo = a.Code,
                        AcceptedQty = a.AcceptedQty,
                        ApprovedQty = a.ApprovedQty,
                        QCStatus = a.QCStatus,
                        ProductionIssueDate = (DateTime)a.IssueDate,
                        ToWareHouseID = a.ToWarehouseID,
                        Remarks = a.Remarks,
                        BatchNo = a.BatchNo,
                        StandardOutput=(decimal)a.StandardOutput
                    }).ToList();

                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<QCItemBO> GetQCList(string Status, int Offset, int Limit)
        {
            try
            {
                using (ProductionEntities db = new ProductionEntities())
                {

                    return db.spGetQCProductionList(Status, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, Offset, Limit).Select(a => new QCItemBO
                    {
                        ID = a.ID,
                        QCNo = a.QCNo,
                        QCDate = a.QCDate,
                        ProductionID = a.ProductionID,
                        ProductionIssueID = (int)a.ProductionIssueID,
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitName = a.UnitName,
                        ReferenceNo = a.Code,
                        AcceptedQty = a.AcceptedQty,
                        ApprovedQty = a.ApprovedQty,
                        QCStatus = a.QCStatus,
                        ProductionIssueDate = (DateTime)a.IssueDate,
                        BatchNo=a.BatchNo,
                        BatchSize=(decimal)a.ActualBatchSize
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public bool UpdateQC(QCItemBO QCItem, List<QCTestBO> QCTestResults)
        {
            try
            {
                ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(bool));
                using (ProductionEntities db = new ProductionEntities())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {


                        db.SpUpdateQCProduction(QCItem.ID, QCItem.QCDate, QCItem.QCStatus, QCItem.ToWareHouseID, QCItem.ApprovedQty, QCItem.IsCancelled, QCItem.CancelledDate, QCItem.Remarks, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID,GeneralBO.CreatedUserID, ReturnValue);
                        db.SaveChanges();
                        if ((bool)ReturnValue.Value)
                        {
                            foreach (var item in QCTestResults)
                            {
                                db.SpUpdateQCProductionTrans(item.ID, item.ActualValue, item.ActualResult, item.Remarks, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID,GeneralBO.CreatedUserID);
                            }
                        }
                        transaction.Commit();
                    }

                }

                return ((bool)ReturnValue.Value);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetQCList(string Type, string ProductionNoHint, string ReceiptDateHint, string ItemHint, string BatchNoHint, string UnitHint, string AcceptedQuantityHint, string ApprovedQuantityHint, string BatchsizeHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (ProductionEntities dbEntity = new ProductionEntities())
                {
                    var result = dbEntity.SpGetQCListForDatatable(Type, ProductionNoHint, ReceiptDateHint, ItemHint, BatchNoHint, UnitHint, AcceptedQuantityHint, ApprovedQuantityHint, BatchsizeHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                ProductionNo = item.ProductionNo,
                                ReceiptDate = ((DateTime)item.ReceiptDate).ToString("dd-MMM-yyyy"),
                                Item = item.Item,
                                BatchNo = item.BatchNo,
                                Unit = item.Unit,
                                BatchSize = item.BatchSize,
                                AcceptedQuantity=item.AcceptedQty,
                                ApprovedQuantity=item.ApprovedQty,
                                Status = item.status,
                                QCStatus=item.QCStatus
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