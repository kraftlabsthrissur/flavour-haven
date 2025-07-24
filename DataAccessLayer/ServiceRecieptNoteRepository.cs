using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ServiceRecieptNoteRepository
    {
        private readonly PurchaseEntities dbContext;

        public ServiceRecieptNoteRepository()
        {
            dbContext = new PurchaseEntities();
        }

        public bool SaveSRN(ServiceRecieptNoteBO _masterSRN, List<SRNTransBO> _SRNtrans)
        {
            bool _response = false;
            using (var transaction = dbContext.Database.BeginTransaction())
            {

                try
                {
                    string FormName = "ServiceReceiptNote";
                    dbContext.SaveChanges();
                    ObjectParameter sRNID = new ObjectParameter("sRNID", typeof(int));

                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                    ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));

                    if (_masterSRN.IsDraft)
                    {
                        FormName = "DraftServiceReceiptNote";
                    }
                    var j = dbContext.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);


                    var i = dbContext.SpCreateSRN(SerialNo.Value.ToString(),
                        _masterSRN.Date,
                        _masterSRN.SupplierID,
                        _masterSRN.ReceiptDate,
                        _masterSRN.DeliveryChallanNo,
                        _masterSRN.DeliveryChallanDate,
                        _masterSRN.IsDraft,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID,
                        sRNID);

                    dbContext.SaveChanges();

                    if (sRNID.Value != null)
                    {
                        foreach (var itm in _SRNtrans)
                        {
                            dbContext.SpCreateSRNTrans(Convert.ToInt32(sRNID.Value),
                                itm.POServiceID,
                                itm.POServiceTransID,
                                itm.ItemID,
                                itm.PurchaseOrderQty,
                                itm.ReceivedQty,
                                itm.AcceptedQty,
                                itm.ServiceLocationID,
                                itm.DepartmentID,
                                itm.EmployeeID,
                                itm.CompanyID,
                                itm.ProjectID,
                                itm.Remarks,
                                itm.TolaranceQty,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                ReturnValue);
                            if (Convert.ToInt32(ReturnValue.Value) <= 0)
                            {
                                throw new DuplicateEntryException("Some of the items in the purchase order has been received already");

                            }
                        }

                    };

                    transaction.Commit();
                    return _response = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

        }

        public bool UpdateSRN(ServiceRecieptNoteBO _masterSRN, List<SRNTransBO> _SRNtrans)
        {
            bool _response = false;
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));
                    var i = dbContext.SpUpdateServiceReceiptNote(
                        _masterSRN.ID,
                        _masterSRN.DeliveryChallanDate,
                        _masterSRN.DeliveryChallanNo,
                        _masterSRN.IsDraft,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                        );

                    dbContext.SaveChanges();


                    foreach (var itm in _SRNtrans)
                    {
                        dbContext.SpCreateSRNTrans(_masterSRN.ID,
                            itm.POServiceID,
                            itm.POServiceTransID,
                            itm.ItemID,
                            itm.PurchaseOrderQty,
                            itm.ReceivedQty,
                            itm.AcceptedQty,
                            itm.ServiceLocationID,
                            itm.DepartmentID,
                            itm.EmployeeID,
                            itm.CompanyID,
                            itm.ProjectID,
                            itm.Remarks,
                            itm.TolaranceQty,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            ReturnValue);
                        if (Convert.ToInt32(ReturnValue.Value) <= 0)
                        {
                            throw new DuplicateEntryException("Some of the items in the purchase order has been received already");

                        }
                    }



                    transaction.Commit();
                    return _response = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

        }

        public List<PurchaseOrderBO> GetUnProcessedPurchaseOrderServiceForSRN(Nullable<int> supplierID)
        {
            List<PurchaseOrderBO> itm = new List<PurchaseOrderBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    itm = dbEntity.SpGetUnProcessedPurchaseOrderForService(supplierID,GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(trans => new PurchaseOrderBO
                    {
                        ID = trans.ID,
                        PurchaseOrderNo = trans.PurchaseOrderNo,
                        PurchaseOrderDate = trans.PurchaseOrderDate,
                        //RequestedBy=trans.cre
                        SupplierID = trans.SupplierID,
                        SupplierName = trans.SupplierName,
                        NetAmt = (decimal)trans.NetAmt,
                        RequestedBy = trans.RequestedBy
                    }).ToList();

                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //this is not giving full details of product
        public List<SRNTransBO> GetUnProcessedPurchaseOrderServiceTransForSRN(Nullable<int> iD)
        {

            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetUnProcessedPurchaseOrderTransForService(iD, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(itm => new SRNTransBO
                    {
                        //Id=itm.ID ,
                        ItemID = itm.ItemID,
                        ItemName = itm.ItemName,
                        POServiceID = itm.POServiceID,
                        POServiceTransID = itm.ID,
                        PurchaseOrderNo = itm.PurchaseOrderNo,
                        Unit = itm.Unit,
                        AcceptedQty = 0,  //itm.Quantity,
                        PurchaseOrderQty = itm.PendingPOQty ?? 0,
                        ReceivedQty = 0,
                        QtyTolerancePercent = (decimal)itm.QtyTolerancePercent,
                        ServiceLocationID = itm.ServiceLocationID ?? 0,
                        ProjectID = itm.ProjectID ?? 0,
                        CompanyID = itm.CompanyID ?? 0,
                        DepartmentID = itm.DepartmentID ?? 0,
                        EmployeeID = itm.EmployeeID ?? 0,
                        TransportMode = itm.ModeOfTransport,
                        TravelDate = itm.TravelDate,
                        TravelFrom = itm.TravelFrom,
                        TravelTo = itm.TravelTo,
                        CategoryID = (int)itm.TravelCategoryID,
                        TravelingRemarks = itm.TravelingRemarks,
                        TolaranceQty = (decimal)(itm.PendingPOQty * itm.QtyTolerancePercent / 100),
                        PORate=(decimal)itm.PORate
                    }).ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ServiceRecieptNoteBO> GetAllSRN(Nullable<int> iD)
        {
            List<ServiceRecieptNoteBO> itm = new List<ServiceRecieptNoteBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    itm = dbEntity.SpGetSRN(iD, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(SRN => new ServiceRecieptNoteBO
                    {
                        ID = SRN.ID,
                        Code = SRN.Code,
                        Date = SRN.Date,
                        SupplierID = SRN.SupplierID,
                        SupplierName = SRN.SupplierName,
                        LocationID = SRN.LocationID ?? 0,
                        Location = SRN.Location,
                        DeliveryChallanNo = SRN.DeliveryChallanNo,
                        DeliveryChallanDate = SRN.DeliveryChallanDate,
                        IsDraft = (bool)SRN.IsDraft,
                        ServicePODate = SRN.PurchaseOrderDate,
                        IsCancelled = SRN.Cancelled

                    }).ToList();
                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SRNTransBO> GetAllSRNTrans(Nullable<int> sRNID)
        {
            List<SRNTransBO> itm = new List<SRNTransBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    itm = dbEntity.SpGetSRNTrans(sRNID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(SRNTrans => new SRNTransBO
                    {
                        SRNID = SRNTrans.SRNID,
                        ItemID = SRNTrans.ItemID,
                        POServiceID = SRNTrans.POServiceID ?? 0,
                        ItemName = SRNTrans.ItemName,
                        Unit = SRNTrans.Unit,
                        PurchaseOrderQty = SRNTrans.PurchaseOrderQty,
                        AcceptedQty = SRNTrans.AcceptedQty,
                        ReceivedQty = SRNTrans.ReceivedQty,
                        ServiceLocationID = SRNTrans.ServiceLocationID ?? 0,
                        DepartmentID = SRNTrans.DepartmentID ?? 0,
                        EmployeeID = SRNTrans.EmployeeID ?? 0,
                        CompanyID = SRNTrans.CompanyID ?? 0,
                        ProjectID = SRNTrans.ProjectID ?? 0,
                        Remarks = SRNTrans.Remarks,
                        PurchaseOrderNo = SRNTrans.PurchaseOrderNo,
                        ServiceLocation = SRNTrans.ServiceLocation,
                        Department = SRNTrans.Department,
                        Company = SRNTrans.InterCompany,
                        Employee = SRNTrans.Employee,
                        Project = SRNTrans.Project,
                        TransportMode = SRNTrans.ModeOfTransport,
                        TravelDate = SRNTrans.TravelDate,
                        TravelFrom = SRNTrans.TravelFrom,
                        TravelTo = SRNTrans.TravelTo,
                        TravelingRemarks = SRNTrans.TravelingRemarks,
                        CategoryID = SRNTrans.TravelCategoryID,
                        QtyTolerancePercent = (decimal)SRNTrans.QtyTolerancePercent,
                        POServiceTransID = SRNTrans.POServiceTransID,
                        PORate= (decimal)SRNTrans.PORate

                    }).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return itm;
        }

        public int Cancel(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())

                    return dbEntity.SpCancelSRN(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetInvoiceNumberCount(string Hint, string Table, int SupplierID)
        {
            try
            {
                ObjectParameter count = new ObjectParameter("count", typeof(int));
                int value;
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    dbEntity.SpGetInvoiceNoCount(Table, Hint, SupplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, count);
                    value = (int)count.Value;
                    return value;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DatatableResultBO GetSRNList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string DeliveryChallanNoHint, string DeliveryChallanDateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetSRNList(Type, TransNoHint, TransDateHint, SupplierNameHint, DeliveryChallanNoHint, DeliveryChallanDateHint, SortField, SortOrder, Offset, Limit, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                SupplierName = item.SupplierName,
                                DeliveryChallanNo = item.DeliveryChallanNo,
                                DeliveryChallanDate = item.DeliveryChallanDate == null ? "" : ((DateTime)item.DeliveryChallanDate).ToString("dd-MMM-yyyy"),
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
    }
}
