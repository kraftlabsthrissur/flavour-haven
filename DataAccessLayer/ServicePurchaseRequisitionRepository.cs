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
    public class ServicePurchaseRequisitionRepository
    {
        private readonly PurchaseEntities dbContext;

        public DatatableResultBO GetPurchaseRequisitionList(string Type, string TransNoHint, string TransDateHint, string PurchaseOrderNumberHint, string CategoryNameHint, string ItemNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetServicePurchaseRequisitionList(Type, TransNoHint, TransDateHint, PurchaseOrderNumberHint, CategoryNameHint, ItemNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                PurchaseOrderNumber = item.PurchaseOrderNumber,
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

        public ServicePurchaseRequisitionRepository()
        {
            dbContext = new PurchaseEntities();
        }

        #region purchase requisition for Non-stock item(service)

        public bool SavePurchaseRequisitionForService(RequisitionBO _masterPr, List<RequisitionServiceItemBO> _prdetails)
        {

            using (var transaction = dbContext.Database.BeginTransaction())
            {

                try
                {
                    string FormName = "PurchaseRequisitionForService";
                    ObjectParameter PrId = new ObjectParameter("purRequisitionServiceID", typeof(int));

                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                    if (_masterPr.IsDraft)
                    {
                        FormName = "DraftPurchaseRequisitionForService";
                    }

                    var j = dbContext.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                    dbContext.SaveChanges();

                    int i = dbContext.SpCreatePurchaseRequisitionForService(
                        SerialNo.Value.ToString(),
                        _masterPr.Date,
                        _masterPr.FullyOrdered,
                        _masterPr.IsDraft,
                        _masterPr.Cancelled,
                        _masterPr.CancelledDate,
                        GeneralBO.CreatedUserID,
                        _masterPr.FromDeptID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID,
                        PrId);
                    dbContext.SaveChanges();

                    if (PrId.Value != null)//TODO
                    {
                        foreach (var itm in _prdetails)
                        {
                            dbContext.SpCreatePurchaseRequisitionTransForService(Convert.ToInt32(PrId.Value),
                                itm.ItemID,
                                itm.Quantity,
                                itm.ExpectedDate,
                                itm.RequiredLocationID,
                                itm.RequiredDepartmentID,
                                itm.RequiredEmployeeID,
                                itm.RequiredInterCompanyID,
                                itm.RequiredProjectID,
                                itm.Remarks,
                                //code below by prama
                                GeneralBO.CreatedUserID,
                                itm.TravelFromID,
                                itm.TravelToID,
                                itm.TransportModeID,
                                itm.TravelingRemarks,
                                itm.TravelDate,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID);
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

        public bool UpdatePurchaseRequisitionForService(RequisitionBO _masterPr, List<RequisitionServiceItemBO> _prdetails)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.SpUpdatePurchaseRequisitionForService(
                        _masterPr.ID,
                        _masterPr.Date,
                        _masterPr.FullyOrdered,
                        _masterPr.IsDraft,
                        _masterPr.FromDeptID,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID);
                    foreach (var itm in _prdetails)
                    {

                        dbContext.SpCreatePurchaseRequisitionTransForService(Convert.ToInt32(_masterPr.ID),
                            itm.ItemID,
                            itm.Quantity,
                     itm.ExpectedDate,
                     itm.RequiredLocationID,
                     itm.RequiredDepartmentID,
                     itm.RequiredEmployeeID,
                     itm.RequiredInterCompanyID,
                     itm.RequiredProjectID,
                     itm.Remarks,
                     itm.CreatedUserID,
                     itm.TravelFromID,
                     itm.TravelToID,
                     itm.TransportModeID,
                     itm.TravelingRemarks,
                     itm.TravelDate,
                     GeneralBO.FinYear,
                     GeneralBO.LocationID,
                     GeneralBO.ApplicationID);
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

        public RequisitionBO PurchaseRequisitionDetailsForService(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetPurchaseRequisitionForService(ID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new RequisitionBO
                    {
                        ApplicationID = k.ApplicationID,
                        ID = k.ID,
                        Code = k.Code,
                        Date = k.Date,
                        RequisitionNo = k.Code,
                        ItemCategory = k.ItemType,
                        ItemName = k.ItemName,
                        Cancelled = k.Cancelled,
                        CancelledDate = k.CancelledDate,
                        CreatedUserID = k.CreatedUserID,
                        CreatedDate = k.CreatedDate,
                        FinYear = k.FinYear,
                        LocationID = k.LocationID,
                        POSNumber = k.PONumber,
                        FullyOrdered = k.IsFullyOrdered,
                        IsDraft = (bool)k.IsDraft,
                        FromDeptID = (int)k.FromDeptID
                    }).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RequisitionServiceItemBO> PurchaseRequisitionTransDetailsForService(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    return dbEntity.SpGetPurchaseRequisitionTransDetailsForService(ID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID).Select(ServicePRTrans => new RequisitionServiceItemBO
                    {
                        ID = ServicePRTrans.ID,
                        ApplicationID = ServicePRTrans.ApplicationID,
                        PurchaseRequisitionServiceID = ServicePRTrans.PurchaseRequisitionID,
                        ItemID = ServicePRTrans.ItemID,
                        Item = ServicePRTrans.Item,

                        Quantity = (decimal)ServicePRTrans.Quantity,
                        ExpectedDate = ServicePRTrans.ExpectedDate,
                        Unit = ServicePRTrans.Unit,
                        UnitID = ServicePRTrans.UnitID,

                        RequiredLocationID = ServicePRTrans.LocationID,
                        Location = ServicePRTrans.Location,

                        Department = ServicePRTrans.Department,
                        RequiredDepartmentID = (ServicePRTrans.RequiredDepartmentID) ?? 0,

                        Employee = ServicePRTrans.Employee,
                        RequiredEmployeeID = (ServicePRTrans.RequiredEmployeeID) ?? 0,

                        InterCompany = ServicePRTrans.InterCompany,
                        RequiredInterCompanyID = (ServicePRTrans.RequiredInterCompanyID) ?? 0,

                        FinYear = ServicePRTrans.FinYear,
                        LocationID = ServicePRTrans.LocationID,

                        Project = ServicePRTrans.Project,
                        RequiredProjectID = ServicePRTrans.RequiredProjectID ?? 0,

                        Remarks = ServicePRTrans.Remarks,
                        TransportMode = ServicePRTrans.ModeOfTransport,
                        TravelDate = ServicePRTrans.TravelDate,
                        TravelFromID = (int)ServicePRTrans.TravelFromID,
                        TravelToID = (int)ServicePRTrans.TravelToID,
                        TransportModeID = (int)ServicePRTrans.TransportModeID,
                        TravelFrom = ServicePRTrans.TravelFrom,
                        TravelTo = ServicePRTrans.TravelTo,
                        TravelingRemarks = ServicePRTrans.TravelingRemarks,
                        TravelCategoryID = ServicePRTrans.TravelCategoryID
                    }).ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RequisitionBO> GetUnProcessedPurchaseRequisitionForService()
        {
            List<RequisitionBO> itm = new List<RequisitionBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    itm = dbEntity.spGetUnProcessedPurchaseRequisitionForService(GeneralBO.FinYear, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new RequisitionBO
                    {
                        Code = k.Code,
                        ID = k.PurchaseRequisitionID,
                        RequisitionNo = k.Code,
                        Date = (DateTime)k.PurchaseRequisitionDate,
                        FromDepartment = k.RequestedBy
                    }).ToList();

                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
