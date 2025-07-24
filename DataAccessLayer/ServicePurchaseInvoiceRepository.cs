using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DataAccessLayer
{
    public class ServicePurchaseInvoiceRepository
    {
        private readonly PurchaseEntities _entity;


        public ServicePurchaseInvoiceRepository()
        {
            _entity = new PurchaseEntities();
        }

        public DatatableResultBO GetPurchaseInvoiceList(string Type, string TransNoHint, string TransDateHint, string InvoiceNoHint, string InvoiceDateHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetServicePurchaseInvoiceList(Type, TransNoHint, TransDateHint, InvoiceNoHint, InvoiceDateHint, SupplierNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                InvoiceNo = item.InvoiceNo,
                                InvoiceDate = item.InvoiceDate == null ? "" : ((DateTime)item.InvoiceDate).ToString("dd-MMM-yyyy"),
                                SupplierName = item.SupplierName,
                                NetAmount = item.NetAmount,
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


        /// <summary>
        /// Get UnProcessed SRN by supplier
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public List<SRNBO> GetUnProcessedSRNBySupplier(int supplierID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetUnProcessedSRN(supplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SRNBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Date = a.Date,
                        SupplierID = a.SupplierID,
                        SupplierName = a.SupplierName,
                        LocationID = a.LocationID,
                        Location = a.Location,
                        PurchaseOrderDate = (DateTime)a.PurchaseOrderDate
                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get TransItems by grn
        /// </summary>
        /// <param name="grnID"></param>
        /// <returns></returns>
        public List<SRNTransItemBO> GetUnProcessedSRNItems(int srnID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetUnProcessedSRNTrans(srnID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SRNTransItemBO
                    {
                        SRNID = srnID,            //SRNID not returning from sp
                        SRNTransID = a.SRNTransID,
                        ItemID = a.ItemID,
                        IsInclusiveGST = a.InclusiveGST,
                        ItemName = a.ItemName,
                        UnitID = a.UnitID,
                        Unit = a.Unit,
                        AcceptedQty = a.ApprovedQty,
                        ApprovedQty = a.ApprovedQty,
                        UnMatchedQty = (decimal)a.UnMatchedQty,
                        PORate = (decimal)a.PORate,
                        ApprovedValue = (decimal)(a.ApprovedQty * a.PORate),
                        SGSTPercent = a.SGSTPercent,
                        CGSTPercent = a.CGSTPercent,
                        IGSTPercent = a.IGSTPercent,
                        SGSTAmt = a.SGSTAmt,
                        CGSTAmt = a.CGSTAmt,
                        IGSTAmt = a.IGSTAmt,
                        ServiceLocationID = a.ServiceLocationID ?? 0,
                        DepartmentID = a.DepartmentID ?? 0,
                        EmployeeID = a.EmployeeID ?? 0,
                        CompanyID = a.CompanyID ?? 0,
                        ProjectID = a.ProjectID ?? 0,
                        //PurchaseOrderID = a.POServiceID,
                        POServiceID = a.POServiceID,
                        TransportMode = a.ModeOfTransport,
                        TravelDate = a.TravelDate,
                        TravelFrom = a.TravelFrom,
                        TravelTo = a.TravelTo,
                        TravelingRemarks = a.TravelingRemarks,
                        CategoryID = a.TravelCategoryID,

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Projects 
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetProjects()
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                return dbEntity.SpGetProject().Select(x => new KeyValuePair<int, string>(x.ID, x.Name)).ToList();
            }
        }

        /// <summary>
        /// Get Company 
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetCompanies()
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                return dbEntity.SpGetInterCompany().Select(x => new KeyValuePair<int, string>(x.ID, x.Name)).ToList();
            }
        }

        /// <summary>
        /// Get Department 
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetDepartments()
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                return dbEntity.SpGetDepartment().Select(x => new KeyValuePair<int, string>(x.ID, x.Name)).ToList();
            }
        }

        /// <summary>
        /// Get Location 
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetLocations()
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                return dbEntity.SpGetLocation().Select(x => new KeyValuePair<int, string>(x.ID, x.Place)).ToList();
            }
        }

        /// <summary>
        /// Get Employee 
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetEmployees()
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                return dbEntity.SpGetEmployee().Select(x => new KeyValuePair<int, string>(x.ID, x.Name)).ToList();
            }
        }

        /// <summary>
        /// Save Purchase Invoice
        /// </summary>
        /// <param name="servicePurchaseInvoiceBO"></param>
        /// <returns></returns>
        public int Save(ServicePurchaseInvoiceBO servicePurchaseInvoiceBO)
        {
            int purchaseInvoiceID;
            if (servicePurchaseInvoiceBO.ServicePurchaseInvoiceID <= 0)
            {
                purchaseInvoiceID = Create(servicePurchaseInvoiceBO);
            }
            else
            {
                purchaseInvoiceID = Update(servicePurchaseInvoiceBO);
            }
            UpdatePayable(purchaseInvoiceID);
            return purchaseInvoiceID;
        }

        /// <summary>
        /// Create new PurchaseInvoice
        /// </summary>
        /// <param name="servicePurchaseInvoiceBO"></param>
        /// <returns></returns>
        private int Create(ServicePurchaseInvoiceBO servicePurchaseInvoiceBO)
        {
            int purchaseInvoiceID = 0;
            if (servicePurchaseInvoiceBO != null)
            {
                using (var transaction = _entity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "PurchaseInvoiceForService";
                        ObjectParameter purchaseInvoiceIDOut = new ObjectParameter("purchaseInvoiceID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(string));

                        if (servicePurchaseInvoiceBO.IsDraft)
                        {
                            FormName = "DraftPurchaseInvoiceForService";
                        }

                        var j = _entity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var i = _entity.SpCreatePurchaseInvoiceForService(
                            SerialNo.Value.ToString(),
                            servicePurchaseInvoiceBO.PurchaseDate,
                            servicePurchaseInvoiceBO.SupplierID,
                            servicePurchaseInvoiceBO.LocalSupplierName,
                            servicePurchaseInvoiceBO.InvoiceNo,
                            servicePurchaseInvoiceBO.InvoiceDate,
                            servicePurchaseInvoiceBO.SGSTAmount,
                            servicePurchaseInvoiceBO.CGSTAmount,
                            servicePurchaseInvoiceBO.IGSTAmount,
                            servicePurchaseInvoiceBO.Discount,
                            servicePurchaseInvoiceBO.InvoiceAmount,
                            servicePurchaseInvoiceBO.DifferenceAmount,
                            servicePurchaseInvoiceBO.AcceptedAmount,
                            servicePurchaseInvoiceBO.TDS,
                            servicePurchaseInvoiceBO.TDSOnAdvance,
                            servicePurchaseInvoiceBO.NetTDS,
                            servicePurchaseInvoiceBO.NetAmountPayable,
                            servicePurchaseInvoiceBO.OtherDeductions,
                            servicePurchaseInvoiceBO.IsDraft,
                            servicePurchaseInvoiceBO.IsCanceled,
                            null,
                            GeneralBO.CreatedUserID,
                            servicePurchaseInvoiceBO.Status,
                            servicePurchaseInvoiceBO.TDSID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            purchaseInvoiceIDOut);

                        _entity.SaveChanges();

                        if (purchaseInvoiceIDOut.Value != null && Convert.ToInt32(purchaseInvoiceIDOut.Value) > 0)
                        {
                            purchaseInvoiceID = Convert.ToInt32(purchaseInvoiceIDOut.Value);

                            if (servicePurchaseInvoiceBO.TaxDetails != null)
                            {
                                foreach (var tax in servicePurchaseInvoiceBO.TaxDetails)
                                {
                                    var taxDetails = _entity.SpCreatePurchaseInvoiceTaxDetailsForService(
                                        purchaseInvoiceID,
                                        tax.Particular,
                                        tax.TaxPercentage,
                                        tax.POValue,
                                        tax.InvoiceValue,
                                        tax.DifferenceValue,
                                        tax.Remarks,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID);
                                }
                            }

                            if (servicePurchaseInvoiceBO.InvoiceTransItems != null)
                            {
                                foreach (var transItem in servicePurchaseInvoiceBO.InvoiceTransItems)
                                {
                                    var result = _entity.SpCreatePurchaseInvoiceTransForService(
                                        purchaseInvoiceID,
                                        transItem.SRNID,
                                        transItem.SRNTransID,
                                        transItem.ItemID,
                                        transItem.AcceptedQty,
                                        transItem.UnMatchedQty,
                                        transItem.PORate,
                                        transItem.InvoiceQty,
                                        transItem.InvoiceRate,
                                        transItem.InvoiceAmount,
                                        transItem.Difference,
                                        transItem.TDSCode,
                                        transItem.TDSAmount,
                                        transItem.Remarks,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID,
                                        transItem.ServiceLocationID,
                                        transItem.DepartmentID,
                                        transItem.EmployeeID,
                                        transItem.CompanyID,
                                        transItem.ProjectID,
                                        transItem.SGSTPercent,
                                        transItem.CGSTPercent,
                                        transItem.IGSTPercent,
                                        transItem.InvoiceGSTPercent,
                                        transItem.InclusiveGST,
                                        ReturnValue
                                        );
                                    if (Convert.ToInt32(ReturnValue.Value) == -2)
                                    {
                                        throw new DuplicateEntryException("Some of the items in the purchase invoice has been processed already");
                                    }
                                    if (Convert.ToInt32(ReturnValue.Value) == -1)
                                    {
                                        throw new QuantityExceededException("Invoice quantity exceeded for some of the items");
                                    }
                                }
                            }
                            if (servicePurchaseInvoiceBO.TDSOnAdvance > 0 && !servicePurchaseInvoiceBO.IsDraft)
                            {
                                _entity.SpUpdateTDSOnAdvanceMetAmount(
                                    purchaseInvoiceID,
                                    servicePurchaseInvoiceBO.TDSOnAdvance,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                );
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        purchaseInvoiceID = 0;
                        throw ex;
                    }
                }
            }
            return purchaseInvoiceID;
        }

        /// <summary>
        /// Update existing Purchase Invoice
        /// </summary>
        /// <param name="servicePurchaseInvoiceBO"></param>
        /// <returns></returns>
        private int Update(ServicePurchaseInvoiceBO servicePurchaseInvoiceBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("RetValue", typeof(string));

            if (servicePurchaseInvoiceBO != null)
            {
                using (var transaction = _entity.Database.BeginTransaction())
                {
                    try
                    {
                        var i = _entity.SpUpdatePurchaseInvoiceForService(
                            servicePurchaseInvoiceBO.ServicePurchaseInvoiceID,
                            servicePurchaseInvoiceBO.SupplierID,
                            servicePurchaseInvoiceBO.LocalSupplierName,
                            servicePurchaseInvoiceBO.InvoiceNo,
                            servicePurchaseInvoiceBO.InvoiceDate,
                            servicePurchaseInvoiceBO.SGSTAmount,
                            servicePurchaseInvoiceBO.CGSTAmount,
                            servicePurchaseInvoiceBO.IGSTAmount,
                            servicePurchaseInvoiceBO.Discount,
                            servicePurchaseInvoiceBO.InvoiceAmount,
                            servicePurchaseInvoiceBO.DifferenceAmount,
                            servicePurchaseInvoiceBO.AcceptedAmount,
                            servicePurchaseInvoiceBO.NetAmountPayable,
                            servicePurchaseInvoiceBO.TDS,
                            servicePurchaseInvoiceBO.TDSOnAdvance,
                            servicePurchaseInvoiceBO.NetTDS,
                            servicePurchaseInvoiceBO.IsDraft,
                            servicePurchaseInvoiceBO.IsCanceled,
                            null,
                            GeneralBO.CreatedUserID,
                            DateTime.Now,
                            servicePurchaseInvoiceBO.Status,
                            servicePurchaseInvoiceBO.TDSID,
                            servicePurchaseInvoiceBO.OtherDeductions,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            );


                        _entity.SaveChanges();


                        if (servicePurchaseInvoiceBO.TaxDetails != null)
                            foreach (var tax in servicePurchaseInvoiceBO.TaxDetails)
                            {
                                var taxDetails = _entity.SpCreatePurchaseInvoiceTaxDetailsForService(
                                   servicePurchaseInvoiceBO.ServicePurchaseInvoiceID,
                                    tax.Particular,
                                    tax.TaxPercentage,
                                    tax.POValue,
                                    tax.InvoiceValue,
                                    tax.DifferenceValue,
                                    tax.Remarks,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                    );
                            }

                        if (servicePurchaseInvoiceBO.InvoiceTransItems != null)
                            foreach (var transItem in servicePurchaseInvoiceBO.InvoiceTransItems)
                            {
                                var result = _entity.SpCreatePurchaseInvoiceTransForService(
                                   servicePurchaseInvoiceBO.ServicePurchaseInvoiceID,
                                    transItem.SRNID,
                                    transItem.SRNTransID,
                                    transItem.ItemID,
                                    transItem.AcceptedQty,
                                    transItem.UnMatchedQty,
                                    transItem.PORate,
                                    transItem.InvoiceQty,
                                    transItem.InvoiceRate,
                                    transItem.InvoiceAmount,
                                    transItem.Difference,
                                    transItem.TDSCode,
                                    transItem.TDSAmount,
                                    transItem.Remarks,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    transItem.ServiceLocationID,
                                    transItem.DepartmentID,
                                    transItem.EmployeeID,
                                    transItem.CompanyID,
                                    transItem.ProjectID,
                                    transItem.SGSTPercent,
                                    transItem.CGSTPercent,
                                    transItem.IGSTPercent,
                                    transItem.InvoiceGSTPercent,
                                    transItem.IsInclusiveGST,
                                    ReturnValue
                                    );
                                if (Convert.ToInt32(ReturnValue.Value) == -2)
                                {
                                    throw new DuplicateEntryException("Some of the items in the purchase invoice has been processed already");
                                }
                                if (Convert.ToInt32(ReturnValue.Value) == -1)
                                {
                                    throw new QuantityExceededException("Invoice quantity exceeded for some of the items");
                                }
                            }

                        if (servicePurchaseInvoiceBO.TDSOnAdvance > 0 && !servicePurchaseInvoiceBO.IsDraft)
                        {
                            _entity.SpUpdateTDSOnAdvanceMetAmount(
                                servicePurchaseInvoiceBO.ServicePurchaseInvoiceID,
                                servicePurchaseInvoiceBO.TDSOnAdvance,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID
                                );
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            return servicePurchaseInvoiceBO.ServicePurchaseInvoiceID;

            //throw new NotImplementedException("Service Purchase Invoice Update has not been impletemted");
        }
        /// <summary>
        /// Get Sales Purchase Invoice
        /// </summary>
        /// <param name="invoiceID"></param>

        /// <returns></returns>
        /// 


        public ServicePurchaseInvoiceBO GetPurchaseInvoice(int invoiceID)
        {
            ServicePurchaseInvoiceBO servicePurchaseInvoiceBO = new ServicePurchaseInvoiceBO();

            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                try
                {
                    var poForService = dbEntity.SpGetPurchaseInvoiceForService(invoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).FirstOrDefault();

                    servicePurchaseInvoiceBO = new ServicePurchaseInvoiceBO()
                    {
                        ServicePurchaseInvoiceID = poForService.ID,
                        PurchaseNo = poForService.PurchaseNo,
                        SupplierID = poForService.SupplierID ?? 0,
                        SupplierName = poForService.Supplier,
                        InvoiceNo = poForService.InvoiceNo,
                        InvoiceDate = poForService.InvoiceDate ?? new DateTime(),
                        SGSTAmount = poForService.SGSTAmount ?? 0,
                        CGSTAmount = poForService.CGSTAmount ?? 0,
                        IGSTAmount = poForService.IGSTAmount ?? 0,
                        Discount = poForService.Discount ?? 0,
                        InvoiceAmount = poForService.InvoiceAmount ?? 0,
                        DifferenceAmount = poForService.DifferenceAmount ?? 0,
                        AcceptedAmount = poForService.AcceptedAmount ?? 0,
                        TDS = poForService.TDS ?? 0,
                        TDSOnAdvance = poForService.TDSOnAdvance ?? 0,
                        NetTDS = poForService.NetTDS ?? 0,
                        NetAmountPayable = poForService.NetAmountPayable ?? 0,
                        IsDraft = poForService.IsDraft ?? false,
                        IsCanceled = poForService.IsCancelled ?? false,
                        TransDate = poForService.PurchaseDate ?? new DateTime(),
                        Status = poForService.Status,
                        IsGSTRegistered = (bool)poForService.IsGSTRegistered,
                        PurchaseOrderDate = (DateTime)poForService.PurchaseOrderDate,
                        GSTNo = poForService.GSTNo,
                        SupplierLocation = poForService.SupplierLocation,
                        TDSCode = poForService.TDSCode,
                        TDSID = poForService.TDSID,
                        Rate = string.Concat(poForService.TDSID, "#", poForService.TDSRate),
                        StateID = poForService.StateID,
                        OtherDeductions = poForService.OtherDeductions ?? 0,
                    };
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return servicePurchaseInvoiceBO;
        }

        public decimal GetTDsAmountForUnProcessedSRNItems(string IDS)
        {
            ObjectParameter TDSAmount = new ObjectParameter("TDSAmount", typeof(decimal));
            using (PurchaseEntities dbEntity = new PurchaseEntities())
            {
                var po = dbEntity.SpGetUnProcessedTDSAmountForServicePurchaseInvoice(IDS, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, TDSAmount);
                if (TDSAmount != null)
                {
                    return (decimal)TDSAmount.Value;
                }
            }
            return 0;
        }

        /// <summary>
        /// Get ServicePurchase Invoice items for edit
        /// </summary>
        /// <param name="servicePurchaseInvoiceID"></param>
        /// <returns></returns>
        public List<ServicePurchaseInvoiceTransItemBO> GetServicePurchaseInvoiceTransItems(int servicePurchaseInvoiceID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetPurchaseInvoiceTransForService(servicePurchaseInvoiceID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(t => new ServicePurchaseInvoiceTransItemBO()
                    {

                        PurchaseInvoiceID = (int)t.PurchaseInvoiceID,
                        SRNID = (int)t.SRNID,
                        SRNTransID = (int)t.SRNTransID,
                        ItemID = t.ItemID ?? 0,
                        ItemName = t.ItemName,
                        AcceptedQty = t.AcceptedQty ?? 0,
                        UnMatchedQty = t.UnMatchedQty ?? 0,
                        PORate = t.PORate ?? 0,
                        InvoiceQty = t.InvoiceQty ?? 0,
                        InvoiceRate = t.InvoiceRate ?? 0,
                        InvoiceAmount = t.InvoiceAmount ?? 0,
                        Difference = t.Difference ?? 0,
                        TDSCode = t.TDSCode,
                        TDSAmount = t.TDSAmount ?? 0,
                        Remarks = t.Remarks,
                        AcceptedValue = (decimal)t.AcceptedQty * (decimal)t.PORate,
                        InvoiceValue = (decimal)t.InvoiceQty * (decimal)t.InvoiceRate,
                        ServiceLocationID = (int)t.ServiceLocationID,
                        DepartmentID = (int)t.DepartmentID,
                        ProjectID = t.ProjectID,
                        CompanyID = t.CompanyID,
                        EmployeeID = (int)t.EmployeeID,
                        SGSTPercent = t.SGSTPercentage ?? 0,
                        CGSTPercent = (decimal)t.CGSTPercentage,
                        IGSTPercent = (decimal)t.IGSTPercentage,
                        InvoiceGSTPercent = (decimal)t.InvoiceGSTPercent,
                        IsInclusiveGST = (bool)t.IsInclusiveGST,
                        POServiceID = (int)t.PurchaseInvoiceID,
                        TransportMode = t.ModeOfTransport,
                        TravelDate = t.TravelDate,
                        TravelFrom = t.TravelFrom,
                        TravelTo = t.TravelTo,
                        TravelingRemarks = t.TravelingRemarks,
                        CategoryID = t.TravelCategoryID,
                        GSTPercent = (int)((t.SGSTPercentage ?? 0) + t.CGSTPercentage + t.IGSTPercentage),
                        InclusiveGST = (bool)t.IsInclusiveGST

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }



        /// <summary>
        /// Get al PurchaseOrderTrans Details for Service
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<PurchaseOrderTransBOService> GetPurchaseOrderTransDetails_Service(int ID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetPurchaseOrderTransDetailsForService(ID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PurchaseOrderTransBOService
                    {
                        ID = a.ID,
                        POServiceID = a.POServiceID,
                        PurchaseRequisitionID = a.PurchaseRequisitionID,
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitID = a.UnitID,
                        Unit = a.Unit,
                        Quantity = (decimal)a.Quantity,
                        Rate = a.Rate ?? 0,
                        Amount = a.Amount ?? 0,
                        SGSTPercent = a.SGSTPercent ?? 0,
                        CGSTPercent = a.CGSTPercent ?? 0,
                        IGSTPercent = a.IGSTPercent ?? 0,
                        SGSTAmt = a.SGSTAmt ?? 0,
                        CGSTAmt = a.CGSTAmt ?? 0,
                        IGSTAmt = a.IGSTAmt ?? 0,
                        NetAmount = (decimal)a.NetAmount,
                        ServiceLocationID = a.ServiceLocationID ?? 0,
                        DepartmentID = a.DepartmentID ?? 0,
                        EmployeeID = a.EmployeeID ?? 0,
                        CompanyID = a.CompanyID ?? 0,
                        ProjectID = a.ProjectID ?? 0,
                        IsPurchased = a.IsPurchased ?? false,
                        QtyMet = a.QtyMet ?? 0,
                        Remarks = a.Remarks,
                        FinYear = a.FinYear,
                        LocationID = a.LocationID,
                        ApplicationID = a.ApplicationID,
                        ServiceLocation = a.ServiceLocation,
                        Department = a.Department,
                        Employee = a.Employee,
                        InterCompany = a.InterCompany,
                        Project = a.Project
                    }).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Service PurchaseInvoice Tax Details
        /// </summary>
        /// <param name="servicePurchaseInvoiceID"></param>
        /// <returns></returns>
        public List<ServicePurchaseInvoiceTaxDetailsBO> GetServicePurchaseInvoiceTaxDetails(int servicePurchaseInvoiceID)
        {

            try
            {               //Valid servicePurchaseInvoiceID
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetPurchaseInvoiceTaxDetailsForService(servicePurchaseInvoiceID).Select(t => new ServicePurchaseInvoiceTaxDetailsBO()
                    {
                        PurchaseInvoiceID = t.PurchaseInvoiceID ?? 0,
                        Particular = t.Particular,
                        TaxPercentage = t.TaxPercentage ?? 0,
                        POValue = t.POAmount ?? 0,
                        InvoiceValue = t.InvoiceAmount ?? 0,
                        DifferenceValue = t.DifferenceAmount ?? 0,
                        Remarks = t.Remarks,

                    }).ToList();

                }
            }
            catch (Exception e)
            {
                throw;
            }

        }

        public bool UpdatePayable(int ID)
        {
            using (PurchaseEntities entity = new PurchaseEntities())
            {
                try
                {
                    entity.SpUpdatePayableDueDate(ID, "Service", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
                catch (Exception ex)
                {
                    return false;

                }
            }
            return true;

        }
        public bool Approve(int ID, String Status)
        {
            using (PurchaseEntities entity = new PurchaseEntities())
            {
                try
                {
                    entity.SpUpdatePurchaseInvoiceStatusForService(ID, Status, GeneralBO.FinYear,
                         GeneralBO.LocationID, GeneralBO.ApplicationID);
                    entity.SpUpdatePayableDueDate(ID, "Stock", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
        public int Cancel(int ID, string Table)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())

                    return dbEntity.SpCancelTransaction(ID, Table, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
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

    }

}
