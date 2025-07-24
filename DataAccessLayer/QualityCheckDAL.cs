using System;
using System.Collections.Generic;
using System.Linq;

using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class QualityCheckDAL
    {
        public IList<QCTestBO> GetQCTestDetailsByID(int ID, string Type, int FinYear, int LocationID, int ApplicationID)
        {
            try
            {
                using (PurchaseEntities db = new PurchaseEntities())
                {
                    return db.spGetQCMaterialsTrans(ID, Type, FinYear, LocationID, ApplicationID).Select(a => new QCTestBO
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

        public IList<QCItemBO> GetQCItemDetailsByID(int ID, int FinYear, int LocationID, int ApplicationID)
        {
            try
            {
                using (PurchaseEntities db = new PurchaseEntities())
                {
                    return db.spGetQCMaterialByID(ID, FinYear, LocationID, ApplicationID).Select(a => new QCItemBO
                    {
                        ID = a.ID,
                        QCNo = a.QCNo,
                        QCDate = a.QCDate,
                        GRNID = a.GRNID,
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitName = a.UnitName,
                        ReferenceNo = a.Code,
                        SupplierName = a.SupplierName,
                        AcceptedQty = a.AcceptedQty,
                        ApprovedQty = a.ApprovedQty,
                        QCStatus = a.QCStatus,
                        GRNDate = a.GRNDate,
                        ToWareHouseID = a.ToWarehouseID,
                        Remarks = a.Remarks,
                        DeliveryChallanNo = a.DeliveryChallanNo,
                        DeliveryChallanDate = (DateTime)a.DeliveryChallanDate
                    }).ToList();

                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public DatatableResultBO GetQualityCheckList(string Type, string TransNoHint, string TransDateHint, string GRNNoHint, string ReceiptDateHint, string ItemNameHint, string UnitNameHint, string SupplierNameHint,string DeliveryChallanNoHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetQualityCheckList(Type, TransNoHint, TransDateHint, GRNNoHint, ReceiptDateHint, ItemNameHint, UnitNameHint, SupplierNameHint, DeliveryChallanNoHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                GRNNo = item.GRNNo,
                                ReceiptDate = ((DateTime)item.ReceiptDate).ToString("dd-MMM-yyyy"),
                                ItemName = item.ItemName,
                                UnitName = item.UnitName,
                                AcceptedQty = item.AcceptedQty,
                                SupplierName = item.SupplierName,
                                Status = item.Status,
                                ApprovedQty = item.ApprovedQty,
                                DeliveryChallanDate = ((DateTime)item.DeliveryChallanDate).ToString("dd-MMM-yyyy"),
                                DeliveryChallanNo = item.DeliveryChallanNo
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

        public IList<QCItemBO> GetQCList(string Status, int FinYear, int LocationID, int ApplicationID, int Offset, int Limit)
        {
            try
            {
                using (PurchaseEntities db = new PurchaseEntities())
                {

                    return db.spGetQCMaterials(Status, FinYear, LocationID, ApplicationID, Offset, Limit).Select(a => new QCItemBO
                    {
                        ID = a.ID,
                        QCNo = a.QCNo,
                        QCDate = a.QCDate,
                        GRNID = a.GRNID,
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitName = a.UnitName,
                        ReferenceNo = a.Code,
                        SupplierName = a.SupplierName,
                        AcceptedQty = a.AcceptedQty,
                        ApprovedQty = a.ApprovedQty,
                        QCStatus = a.QCStatus,
                        GRNDate = a.GRNDate,
                        IsDraft = (bool)a.IsDraft
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool UpdateQC(QCItemBO QCItem, List<QCTestBO> QCTestResults, int FinYear, int LocationID, int ApplicationID)
        {
            try
            {
                using (PurchaseEntities db = new PurchaseEntities())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        db.SpUpdateQC(QCItem.ID,
                           QCItem.QCDate,
                           QCItem.QCStatus,
                           QCItem.ToWareHouseID,
                           QCItem.ApprovedQty,
                           QCItem.IsCancelled,
                           QCItem.CancelledDate,
                           QCItem.Remarks,
                           QCItem.IsDraft,
                           GeneralBO.CreatedUserID,
                           FinYear,
                           LocationID,
                           ApplicationID);

             
                        db.SaveChanges();
                        foreach (var item in QCTestResults)
                        {
                            db.SpUpdateQCTrans(item.ID,
                                item.ActualValue,
                                item.ActualResult,
                                item.Remarks,
                                GeneralBO.CreatedUserID,
                                FinYear,
                                LocationID,
                                ApplicationID);
                        }

                        transaction.Commit();
                    }

                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}

