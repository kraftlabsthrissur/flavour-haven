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
    public class ServicePurchaseOrderDAL
    {
        public PurchaseOrderBO GetPurchaseOrder(int ID)
        {
            PurchaseOrderBO itm = new PurchaseOrderBO();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    itm = dbEntity.SpGetPurchaseOrderForService(ID,GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new PurchaseOrderBO
                    {
                        AdvanceAmount =  Convert.ToDecimal(k.AdvanceAmount),
                        AdvancePercentage =  Convert.ToInt32(k.AdvancePercentage),
                        BillingAddressID = k.BillingAddressID,
                        BillingLocation = k.BillingAddress,
                        Cancelled = k.Cancelled,
                        CancelledDate = k.CancelledDate,
                        CGSTAmt = k.CGSTAmt,
                        CreatedDate = k.CreatedDate,
                        DeliveryWithin = k.DeliveryWithin,
                        DirectInvoice = (bool)k.IsDirectInvoice,
                        IGSTAmt = k.IGSTAmt,
                        NetAmt =(decimal) k.NetAmt,
                        SGSTAmt = k.SGSTAmt,
                        ShippingAddressID = k.ShippingAddressID,

                        ID = k.ID,
                        ItemName = k.ItemName,
                        ItemCatagory = k.ItemType,
                        InclusiveGST = k.InclusiveGST,
                        IsDraft = (bool)k.IsDraft,
                        OrderMet = k.OrderMet,
                        OtherQuotationIDS = k.OtherQuotationIDS,
                        PurchaseOrderDate = k.PurchaseOrderDate,
                        PurchaseOrderNo = k.PurchaseOrderNo,
                        Remarks = k.Remarks,
                        SelectedQuotationID = k.SelectedQuotationID,

                        ShipplingLocation = k.ShippingAddress,
                        ShippingStateID = k.ShippingStateID,

                        SupplierID = k.SupplierID,
                        SupplierName = k.Supplier,
                        SupplierLocation = k.SupplierLocation,
                        StateId = (int)k.ShippingStateID,
                        IsGSTRegistred = (bool)k.IsGSTRegistered,
                        PaymentModeID = k.PaymentModeID,
                        PaymentMode = k.PaymentMode,
                        PaymentWithin = k.PaymentWithIn,
                        PaymentWithinID = k.PaymentWithinID,
                        SupplierReferenceNo = k.SupplierReferenceNo,
                        TermsOfPrice = k.TermsOfPrice,
                        IsSuspended = (bool)k.IsSuspended,
                        InvoiceNo=k.PurchaseInvoiceNo,
                        InvoiceDate=(DateTime)k.PurchaseInvoiceDate,
                        Discount=(decimal)k.PurchaseInvoiceDiscount,
                        OtherDeductions=(decimal)k.PurchaseInvoiceDeductions
                    }).FirstOrDefault();
                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DatatableResultBO GetPurchaseOrderList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string ItemNameHint, string CategoryNameHint,string NetAmtHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetServicePurchaseOrderList(Type, TransNoHint, TransDateHint, SupplierNameHint, CategoryNameHint, ItemNameHint ,NetAmtHint, SortField, SortOrder, Offset, Limit,GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                NetAmount = item.NetAmt,
                                ItemName = item.ItemName,
                                CategoryName = item.CategoryName,
                                Status = item.Status,
                                IsCancellable = (item.Status.ToLower() != "processed" && item.Status.ToLower() != "cancelled" && item.Status.ToLower() != "suspended") ? 1 : 0,
                                IsSuspendable = (item.Status.ToLower() != "cancelled" && item.Status.ToLower() != "suspended" && item.Status.ToLower() != "processed") ? 1 : 0,
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

        public JSONOutputBO SavePurchaseOrder(PurchaseOrderBO PO, List<PurchaseOrderTransBO> POTrans)
        {
            JSONOutputBO output = new JSONOutputBO();
            using (PurchaseEntities entity = new PurchaseEntities())
            {
                using (var transaction = entity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "PurchaseOrderForService";

                        ObjectParameter POId = new ObjectParameter("PurchaseOrderID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
                        if (PO.IsDraft)
                        {
                            FormName = "DraftPurchaseOrderForService";
                        }

                        var j = entity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        entity.SaveChanges();

                        var i = entity.SpCreatePurchaseOrderForService(
                            SerialNo.Value.ToString(),
                            PO.PurchaseOrderDate,
                            PO.SupplierID,
                            PO.AdvancePercentage,
                            PO.AdvanceAmount,
                            PO.PaymentModeID,
                            PO.ShippingAddressID,
                            PO.BillingAddressID,
                            PO.InclusiveGST,
                            PO.SelectedQuotationID,
                            PO.OtherQuotationIDS,
                            PO.DeliveryWithin,
                            PO.PaymentWithinID,
                            PO.SGSTAmt,
                            PO.CGSTAmt,
                            PO.IGSTAmt,
                            PO.NetAmt,
                            PO.OrderMet,
                            PO.IsDraft,
                            PO.Remarks,
                            PO.SupplierReferenceNo,
                            PO.TermsOfPrice,
                            PO.Cancelled,
                            PO.CancelledDate,
                            GeneralBO.CreatedUserID,
                            DateTime.Now,
                            PO.DirectInvoice,
                            PO.InvoiceNo,
                            PO.InvoiceDate,
                            PO.Discount,
                            PO.OtherDeductions,
                            POTrans.Select(e => e.ItemID).FirstOrDefault(),
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            POId);

                        entity.SaveChanges();

                        if (POId.Value != null)
                        {
                            foreach (var item in POTrans)
                            {

                                entity.SpCreatePurchaseOrderTransForService(
                                    Convert.ToInt32(POId.Value),
                                    item.PurchaseReqID,
                                    item.PRTransID,
                                    PO.IsDraft,
                                    item.ItemID,
                                    item.Quantity,
                                    item.Rate,
                                    item.Amount,
                                    item.SGSTPercent,
                                    item.CGSTPercent,
                                    item.IGSTPercent,
                                    item.SGSTAmt,
                                    item.CGSTAmt,
                                    item.IGSTAmt,
                                    item.NetAmount,
                                    //item.ServiceLocationID,
                                    //item.DepartmentID,
                                    //item.EmployeeID,
                                    //item.CompanyID,
                                    //item.ProjectID,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    false,
                                    item.Remarks,                                  
                                    //item.TravelFromID,
                                    //item.TravelToID,
                                    //item.TransportModeID,
                                    //item.TravelingRemarks,
                                    0,0,0,"",
                                    item.TravelDate,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    ReturnValue);
                                if (Convert.ToInt16(ReturnValue.Value) == -1)
                                {
                                    transaction.Rollback();
                                    output.Message = "Some of the items in the purchase requisition has been processed";
                                    output.Status = "failure";
                                    return output;
                                }

                            }
                            output.Data = new OutputDataBO
                            {
                                ID = Convert.ToInt32(POId.Value),
                                IsDraft = PO.IsDraft,
                                TransNo = SerialNo.Value.ToString()
                            };

                        }
                        else
                        {
                            output.Message = "Failed To Save Service Purchase Order";
                            output.Status = "failure";
                        };
                        if (PO.DirectInvoice)
                        {
                            entity.SpCreateAutomaticSRNAndInvoice(
                                Convert.ToInt32(POId.Value),
                                PO.InvoiceNo,
                                PO.InvoiceDate,
                                PO.Discount,
                                PO.OtherDeductions,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID
                                );
                        }
                        transaction.Commit();
                        return output;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        output.Message = "Failed To Save Service Purchase Order " + ex.ToString();
                        output.Status = "failure";
                        return output;
                    }
                }
            }
        }

        public JSONOutputBO UpdatePurchaseOrder(PurchaseOrderBO _masterPO, List<PurchaseOrderTransBO> _pOdetails)
        {
            JSONOutputBO output = new JSONOutputBO();
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (PurchaseEntities entity = new PurchaseEntities())
            {
                using (var transaction = entity.Database.BeginTransaction())
                {
                    try
                    {
                        //
                        //_masterPO.PurchaseOrderDate,
                        entity.SpUpdatePurchaseOrderForService(
                            _masterPO.ID,
                            _masterPO.SupplierID,
                            _masterPO.AdvancePercentage,
                            _masterPO.AdvanceAmount,
                            _masterPO.PaymentModeID,
                            _masterPO.ShippingAddressID,
                            _masterPO.BillingAddressID,
                            _masterPO.InclusiveGST,
                            _masterPO.SelectedQuotationID,
                            _masterPO.OtherQuotationIDS,
                            _masterPO.DeliveryWithin,
                            _masterPO.PaymentWithinID,
                            _masterPO.SGSTAmt,
                            _masterPO.CGSTAmt,
                            _masterPO.IGSTAmt,
                            _masterPO.NetAmt,
                            _masterPO.IsDraft,
                            _masterPO.Remarks,
                            _masterPO.SupplierReferenceNo,
                            _masterPO.TermsOfPrice,
                            _masterPO.InvoiceNo,
                            _masterPO.InvoiceDate,
                           _masterPO.Discount,
                           _masterPO.OtherDeductions,
                            _pOdetails.Select(e => e.ItemID).FirstOrDefault(),
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            _masterPO.DirectInvoice
                        );

                        entity.SaveChanges();


                        foreach (var item in _pOdetails)
                        {
                            entity.SpCreatePurchaseOrderTransForService(_masterPO.ID,
                                item.PurchaseReqID,
                                item.PRTransID,
                                _masterPO.IsDraft,
                                item.ItemID,
                                item.Quantity,
                                item.Rate,
                                item.Amount,
                                item.SGSTPercent,
                                item.SGSTPercent,
                                item.IGSTPercent,
                                item.SGSTAmt,
                                item.CGSTAmt,
                                item.IGSTAmt,
                                item.NetAmount,
                                //item.ServiceLocationID,
                                //item.DepartmentID,
                                //item.EmployeeID,
                                //item.CompanyID,
                                //item.ProjectID,
                                0,0,0,0,0,
                                item.QtyMet,
                                item.Purchased,
                                item.Remarks,
                                //code below by prama
                                //item.TravelFromID,
                                //item.TravelToID,
                                //item.TransportModeID,
                                //item.TravelingRemarks,
                                0,0,0,"",
                                item.TravelDate,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                ReturnValue
                                );
                            if (Convert.ToInt16(ReturnValue.Value) == -1)
                            {
                                transaction.Rollback();
                                output.Message = "Some of the items in the purchase requisition has been processed";
                                output.Status = "failure";
                                throw new Exception("Some of the items in the purchase requisition has been processed");
                            }
                        }

                        transaction.Commit();
                        output.Data = new OutputDataBO
                        {
                            ID = _masterPO.ID,
                            IsDraft = _masterPO.IsDraft,

                        };
                        return output;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        output.Message = "Failed To Update Service Purchase Order " + ex.ToString();
                        output.Status = "failure";
                        throw ex;
                    }
                }
            }
        }

        public List<PurchaseOrderTransBO> GetUnProcessedPurchaseRequisitionTransForPO(int ID, int SupplierID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {

                    return dbEntity.spGetUnProcessedPurchaseRequisitionTransForService(ID, SupplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(item => new PurchaseOrderTransBO
                    {
                        ID = item.ItemID,
                        PurchaseReqID = item.PurchaseRequisitionID,
                        PRTransID = item.ID,
                        Name = item.Item,
                        UnitID = item.UnitID,
                        Unit = item.Unit,
                        Quantity = Convert.ToDecimal(item.Quantity),
                        Remarks = item.Remarks,
                       // ServiceLocationID = item.RequiredLocationID,
                        ServiceLocation = item.Location,
                        //DepartmentID = item.RequiredDepartmentID,
                        Department = item.Department,
                       // EmployeeID = item.RequiredEmployeeID,
                        Employee = item.Employee,
                        //CompanyID = item.RequiredInterCompanyID,
                        Company = item.InterCompany,
                        //ProjectID = item.RequiredProjectID,
                        Project = item.Project,
                        GSTPercentage = item.GSTPercentage,

                        //code below by prama
                        //TransportMode = item.ModeOfTransport,
                        //TransportModeID = (int)item.TransportModeID,
                        TravelDate = item.TravelDate,
                        //TravelFrom = item.TravelFrom,
                        //TravelFromID = (int)item.TravelFromID,
                        //TravelTo = item.TravelTo,
                        //TravelToID = (int)item.TravelToID,
                        //TravelCategoryID = (int)item.TravelCategoryID,
                        //TravelingRemarks = item.TravelingRemarks
                    }).ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<PurchaseOrderTransBO> GetPurchaseOrderTransDetails(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetPurchaseOrderTransDetailsForService(ID,GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PurchaseOrderTransBO
                    {
                        Amount = a.Amount,
                        ApplicationID = a.ApplicationID,
                        CGSTAmt = a.CGSTAmt,
                        IGSTAmt = a.IGSTAmt,
                        SGSTAmt = a.SGSTAmt,
                        CGSTPercent = a.CGSTPercent,
                        FinYear = a.FinYear,
                        IGSTPercent = a.IGSTPercent,
                        ItemID = a.ItemID,
                        LocationID = a.LocationID,
                        Name = a.ItemName,
                        Purchased = a.IsPurchased,
                        PurchaseOrderID = a.POServiceID,
                        QtyMet = a.QtyMet,
                        Quantity = a.Quantity,
                        QtyOrdered = a.Quantity,
                        Rate = a.Rate,
                        Remarks = a.Remarks,
                        SGSTPercent = a.SGSTPercent,
                        ID = a.ItemID,
                        Unit = a.Unit,
                        PurchaseReqID = a.PurchaseRequisitionID,
                        PRTransID = a.PRServiceTransID,
                        ServiceLocation = a.ServiceLocation,
                        Department = a.Department,
                        Employee = a.Employee,
                        Company = a.InterCompany,
                        Project = a.Project,
                        NetAmount = (decimal)a.NetAmount,
                        //ServiceLocationID = a.ServiceLocationID,
                        //DepartmentID = a.DepartmentID,
                        //EmployeeID = a.EmployeeID,
                        //CompanyID = a.CompanyID,
                        //ProjectID = a.ProjectID,
                        //TransportMode = a.ModeOfTransport,
                        //TravelDate = a.TravelDate,
                        //TravelFrom = a.TravelFrom,
                        //TravelTo = a.TravelTo,
                        //TravelingRemarks = a.TravelingRemarks,
                        //TravelCategoryID = a.TravelCategoryID,
                        //TravelFromID = a.TravelFromID ?? 0,
                        //TravelToID = a.TravelToID ?? 0,
                        //TransportModeID = a.TransportModeID ?? 0
                    }).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsPOSCancellable(int POSID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpIsPOSCancellable(POSID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).FirstOrDefault().Value;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CancelServicePurchaseOrder(int ServicePurchaseOrderID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpCancelServicePurchaseOrder(ServicePurchaseOrderID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).FirstOrDefault().Value;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CreateAutomaticSRNAndInvoice(int PurchaseOrderID, string InvoiceNo, DateTime InvoiceDate,decimal Discount,decimal OtherDeductions)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    dbEntity.SpCreateAutomaticSRNAndInvoice(PurchaseOrderID, InvoiceNo, InvoiceDate,Discount,OtherDeductions, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public int SuspendPurchaseOrder(int ID, string Table)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(int));
                    int output;
                    dbEntity.SpSuspendTransaction(ID, Table, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID,ReturnValue);
                    output = (int)ReturnValue.Value;
                    return output;
                }

            }
            catch (Exception ex)
            {
                throw ex;
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
    }
}
