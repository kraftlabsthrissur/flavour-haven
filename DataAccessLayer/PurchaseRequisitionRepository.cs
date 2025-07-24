using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using DataAccessLayer.DBContext;
using BusinessObject;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class PurchaseRequisitionRepository
    {
        private readonly PurchaseEntities dbContext;

        public PurchaseRequisitionRepository()
        {
            dbContext = new PurchaseEntities();
        }

        public DatatableResultBO GetPurchaseRequisitionListForPurchaseOrder(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string CategoryNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetPurchaseRequisitionListForPurchaseOrder(Type, TransNoHint, TransDateHint, SupplierNameHint, CategoryNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                PurchaseRequisitionID = item.ID,
                                RequisitionNo = item.RequisitionNo,
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                RequisitedPhoneNumber1 = item.RequisitedPhoneNumber1,
                                RequisitedPhoneNumber2 = item.RequisitedPhoneNumber2,
                                SupplierName = item.SupplierName,
                                RequisitedCustomerAddress = item.RequisitedCustomerAddress,
                                Remarks = item.Remarks,
                               
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

        public DatatableResultBO GetPurchaseRequisitionList(string Type, string TransNoHint, string TransDateHint, string FromDepartmentHint, string ToDepartmentHint, string CategoryNameHint, string ItemNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetPurchaseRequisitionList(Type, TransNoHint, TransDateHint, FromDepartmentHint, ToDepartmentHint, CategoryNameHint, ItemNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                FromDepartment = item.FromDepartment,
                                ToDepartment = item.ToDepartment,
                                CategoryName = item.CategoryName,
                                ItemName = item.ItemName,
                                Status = item.Status
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

        #region purchase requisition for stock items
        /// <summary>
        /// constructor
        /// </summary>


        /// <summary>
        /// Save PR
        /// </summary>
        /// <param name="_masterPr"></param>
        /// <param name="_prdetails"></param>
        /// <returns></returns>
        public bool SavePurchaseRequisition(RequisitionBO _masterPr, List<ItemBO> _prdetails)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {

                try
                {
                    string FormName = "PurchaseRequisition";
                    ObjectParameter PrId = new ObjectParameter("PurRequisitionID", typeof(int));

                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                    if (_masterPr.IsDraft)
                    {
                        FormName = "DraftPurchaseRequisition";
                    }

                    var j = dbContext.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                    dbContext.SaveChanges();

                    var i = dbContext.SpCreatePurchaseRequisition(
                        SerialNo.Value.ToString(), _masterPr.Date, _masterPr.SalesInquiryID, SerialNo.Value.ToString(), _masterPr.PurchaseRequisitedCustomer,
                        _masterPr.RequisitedCustomerAddress, _masterPr.RequisitedPhoneNumber1, _masterPr.RequisitedPhoneNumber2, _masterPr.Remarks,
                        _masterPr.QuotationProcessed, _masterPr.FullyOrdered, _masterPr.FromDeptID, _masterPr.ToDeptID, _masterPr.Cancelled,
                        _masterPr.CancelledDate, GeneralBO.CreatedUserID, _masterPr.CreatedDate, _masterPr.IsDraft, _masterPr.SupplierID, _masterPr.SupplierName,
                        GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, PrId);

                    dbContext.SaveChanges();

                    if (PrId.Value != null)
                    {
                        foreach (var itm in _prdetails)
                        {

                            dbContext.SpCreatePurchaseRequisitionTrans(Convert.ToInt32(PrId.Value), itm.ItemID, itm.ItemCode, itm.ItemName, itm.PartsNumber,
                                itm.Unit, itm.SalesInquiryItemID, itm.ItemTypeID, itm.ReqQty, itm.QtyOrdered, itm.Stock, itm.QtyUnderQC, itm.Remarks, itm.RequiredStatus,
                                itm.ExpectedDate, itm.UnitID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                        }

                    };
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

        }
        /// <summary>
        /// Update PR
        /// </summary>
        /// <param name="_masterPr"></param>
        /// <param name="_prdetails"></param>
        /// <returns></returns>
        public bool UpdatePurchaseRequisition(RequisitionBO _masterPr, List<ItemBO> _prdetails)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.SpUpdatePurchaseRequisition(_masterPr.ID, _masterPr.FromDeptID, _masterPr.ToDeptID,
                        _masterPr.IsDraft, _masterPr.SupplierID, _masterPr.SupplierName, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                    foreach (var itm in _prdetails)
                    {
                        dbContext.SpCreatePurchaseRequisitionTrans(_masterPr.ID, itm.ItemID, itm.ItemCode, itm.ItemName, itm.PartsNumber, itm.Unit,
                          itm.SalesInquiryItemID, itm.ItemTypeID, itm.ReqQty, itm.QtyOrdered, itm.Stock, itm.QtyUnderQC, itm.Remarks, itm.RequiredStatus,
                          itm.ExpectedDate, itm.UnitID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                    }
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }

        }
        /// <summary>
        /// Get PR details
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="FinYear"></param>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        public List<RequisitionBO> PurchaseRequisitionDetails(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    return dbEntity.SpGetPurchaseRequisitionDetails(ID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new RequisitionBO
                    {
                        ApplicationID = k.ApplicationID,
                        ID = k.ID,
                        Code = k.Code,
                        Date = k.Date,
                        RequisitionNo = k.RequisitionNo,
                        FromDepartment = k.FromDepartment,
                        ToDepartment = k.ToDepartment,
                        PurchaseRequisitedCustomer = k.PurchaseRequisitedCustomer,
                        RequisitedCustomerAddress = k.RequisitedCustomerAddress,
                        RequisitedPhoneNumber1 = k.RequisitedPhoneNumber1,
                        RequisitedPhoneNumber2 = k.RequisitedPhoneNumber2,
                        Remarks = k.Remarks,
                        QuotationProcessed = k.QuotationProcessed,
                        FullyOrdered = k.FullyOrdered,
                        FromDeptID = k.FromDeptID ?? 0,
                        ItemCategory = k.ItemType,
                        ItemName = k.ItemName,
                        ToDeptID = k.ToDeptID ?? 0,
                        Cancelled = k.Cancelled,
                        CancelledDate = k.CancelledDate,
                        CreatedUserID = k.CreatedUserID,
                        CreatedDate = k.CreatedDate,
                        FinYear = k.FinYear,
                        LocationID = k.LocationID,
                        IsDraft = (bool)k.IsDraft,
                        SupplierID = k.SupplierID,
                        SupplierName = k.SupplierName
                    }).ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get PR Trans details
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="FinYear"></param>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        public List<ItemBO> PurchaseRequisitionTransDetails(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    var items = dbEntity.SpGetPurchaseRequisitionTransDetails(ID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new ItemBO
                    {
                        ItemID = k.ItemID,
                        ItemName = k.ItemName,
                        ItemCode = k.ItemCode,
                        PartsNumber = k.PartsNumber,
                        SalesInquiryItemID = k.SalesInquiryItemID.HasValue ? k.SalesInquiryItemID.Value : 0,
                        Unit = k.Unit,
                        UnitID = (int)k.UnitID,
                        Stock = (k.Stock != null) ? (int)k.Stock : 0,
                        QtyUnderQC = (k.QtyUnderQC != null) ? k.QtyUnderQC : 0,
                        ReqQty = k.Quantity,
                        QtyOrdered = (k.OrderedQty != null) ? k.OrderedQty : 0,
                        ItemCategoryID = k.ItemCategoryID,
                        Remarks = k.Remarks,
                        ExpectedDate = k.RequiredDate,
                        applicationID = k.ApplicationID,
                        LocationID = k.LocationID,
                        FinYear = k.FinYear,
                        RequiredStatus = k.RequiredStatus,
                    }).ToList();
                    return items;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int IsPurchaseRequisitionEditable(int PurchaseRequisitionID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    return dbEntity.SpIsPurchaseRequisitionEditable(PurchaseRequisitionID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).FirstOrDefault().Value;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }


        #endregion

    }
}
