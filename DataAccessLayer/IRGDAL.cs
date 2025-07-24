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
    public class IRGDAL
    {

        public string SaveIRG(PurchaseReturnBO purchaseReturnBO, List<PurchaseReturnTransItemBO> TransItems)

        {
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {
                        string FormName = "IRG";
                        ObjectParameter PrId = new ObjectParameter("IRGID", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        if (purchaseReturnBO.IsDraft)
                        {
                            FormName = "DraftIRG";
                        }

                        var j = dEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var CreatedUserID = GeneralBO.CreatedUserID;
                        dEntity.SaveChanges();

                        var i = dEntity.SpCreateIRG(
                            SerialNo.Value.ToString(),
                            purchaseReturnBO.ReturnDate,
                            purchaseReturnBO.SupplierID,
                            purchaseReturnBO.SGSTAmount,
                            purchaseReturnBO.CGSTAmount,
                            purchaseReturnBO.IGSTAmount,
                            0,
                            0,
                            0,
                            purchaseReturnBO.NetAmount,
                            purchaseReturnBO.IsDraft,
                            false,
                            purchaseReturnBO.ReturnDate,
                            GeneralBO.CreatedUserID,
                             purchaseReturnBO.ReturnDate,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID,
                             PrId);

                        dEntity.SaveChanges();

                        if (PrId.Value != null)
                        {
                            foreach (var itm in TransItems)
                            {

                                dEntity.SpCreateIRGTrans(
                                    Convert.ToInt32(PrId.Value),
                                    itm.InvoiceID,
                                    itm.ItemID,
                                    itm.Quantity,
                                    itm.Rate,
                                    itm.SGSTPercent,
                                    itm.CGSTPercent,
                                    itm.IGSTPercent,
                                    itm.SGSTAmount,
                                    itm.CGSTAmount,
                                    itm.IGSTAmount,
                                    itm.Amount,
                                    itm.Remarks,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    itm.WarehouseID,
                                    itm.BatchTypeID,
                                    itm.UnitID,
                                    itm.PurchaseReturnID,
                                    itm.PurchaseReturnTransID,
                                    itm.PurchaseReturnOrderID,
                                    itm.PurchaseReturnOrderTransID
                                        );
                            }

                        };
                        transaction.Commit();
                        return PrId.Value.ToString();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
        public string UpdateIRG(PurchaseReturnBO purchaseReturnBO, List<PurchaseReturnTransItemBO> TransItems)

        {
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {

                    try
                    {


                        var i = dEntity.SpUpdateIRG(
                           purchaseReturnBO.Id,
                            purchaseReturnBO.ReturnDate,
                            purchaseReturnBO.SGSTAmount,
                            purchaseReturnBO.CGSTAmount,
                            purchaseReturnBO.IGSTAmount,
                            purchaseReturnBO.NetAmount,
                            purchaseReturnBO.IsDraft,
                            GeneralBO.CreatedUserID,
                             purchaseReturnBO.ReturnDate,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID

                );


                        foreach (var itm in TransItems)
                        {

                            dEntity.SpCreateIRGTrans(
                                purchaseReturnBO.Id,
                                itm.InvoiceID,
                                itm.ItemID,
                                itm.Quantity,
                                itm.Rate,
                                itm.SGSTPercent,
                                itm.CGSTPercent,
                                itm.IGSTPercent,
                                itm.SGSTAmount,
                                itm.CGSTAmount,
                                itm.IGSTAmount,
                                itm.Amount,
                                itm.Remarks,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                itm.WarehouseID,
                                itm.BatchTypeID,
                                itm.UnitID,
                                itm.PurchaseReturnID,
                                itm.PurchaseReturnTransID,
                                itm.PurchaseReturnOrderID,
                                itm.PurchaseReturnOrderTransID
                                    );
                        }


                        transaction.Commit();
                        return purchaseReturnBO.Id.ToString(); ;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<PurchaseReturnBO> GetIRGList()
        {
            List<PurchaseReturnBO> list = new List<PurchaseReturnBO>();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                list = dEntity.SpGetIRG(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PurchaseReturnBO
                {
                    Id = a.ID,
                    ReturnNo = a.Code,
                    ReturnDate = (DateTime)a.CreatedDate,
                    SupplierName = a.suppliername,
                    Freight = (decimal)a.FreightAmount,
                    PackingCharges = (decimal)a.PackingCahrge,
                    OtherCharges = (decimal)a.OtherCharge,
                    NetAmount = (decimal)a.NetAmount,
                    IsDraft=(bool)a.IsDraft
                }).ToList();
                return list;
            }

        }
        public List<PurchaseReturnBO> GetIRGDetail(int ID)
        {
            List<PurchaseReturnBO> list = new List<PurchaseReturnBO>();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                list = dEntity.SpGetIRGDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PurchaseReturnBO
                {
                    Id = a.ID,
                    ReturnNo = a.Code,
                    ReturnDate = (DateTime)a.CreatedDate,
                    SupplierName = a.suppliername,
                    Freight = (decimal)a.FreightAmount,
                    PackingCharges = (decimal)a.PackingCahrge,
                    OtherCharges = (decimal)a.OtherCharge,
                    NetAmount = (decimal)a.NetAmount,
                    IsDraft=(bool)a.IsDraft,
                    SupplierID=(int)a.SupplierID
                }).ToList();
                return list;
            }

        }
        public List<PurchaseReturnTransItemBO> GetIRGTransList(int ID)
        {
            List<PurchaseReturnTransItemBO> list = new List<PurchaseReturnTransItemBO>();
            using (PurchaseEntities dEntity = new PurchaseEntities())
            {
                list = dEntity.SpGetIRGTrans(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PurchaseReturnTransItemBO
                {
                    ItemName = a.ITEMNAME,                 
                    ReturnNo = a.PurchaseReturnNo,                 
                    Unit = a.unit,
                    AcceptedQty = (decimal)a.AcceptedQty,
                    Quantity = a.QTY,
                    Rate = a.Rate,
                    SGSTAmount =(decimal) a.SGSTAmount,
                    SGSTPercent =(decimal) a.SGSTPercent,
                    IGSTAmount =(decimal) a.IGSTAmount,
                    IGSTPercent =(decimal) a.IGSTPercent,
                    CGSTAmount = (decimal)a.CGSTAmount,
                    CGSTPercent = (decimal)a.CGSTPercent,
                    Amount = a.Amount,
                    Remarks = a.Remarks,
                    UnitID=(int)a.UnitID,
                    ItemID=a.ItemID,
                    WarehouseID=(int)a.WareHouseID,
                    PurchaseReturnID=(int)a.ReturnID,
                    PurchaseReturnTransID=(int)a.ReturnTransID,
                    InvoiceID=(int)a.InvoiceID,
                    PurchaseReturnOrderTransID=(int)a.POReturnTransID,
                    PurchaseReturnOrderID=(int)a.POReturnID,
                    InvoiceQty=(decimal)a.InvoiceQty
                }).ToList();


                return list;
            }

        }

        public DatatableResultBO GetIRGList(string Type, string TransNoHint, string TransDateHint, string SupplierHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetIRGListForDatatable(Type, TransNoHint, TransDateHint, SupplierHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                RETURNNO = item.RETURNNO,
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                suppliername = item.suppliername,
                                Status = item.Status,
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

