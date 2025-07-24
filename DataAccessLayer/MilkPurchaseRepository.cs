using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DBContext;
using BusinessObject;
using PresentationContractLayer;
using System.Data.Entity.Core.Objects;
//conflict
namespace DataAccessLayer
{
    public class MilkPurchaseRepository 
    {
        private readonly AyurwareEntities _entity;
        #region Constructor
        public MilkPurchaseRepository()
        {
            _entity = new AyurwareEntities();
        }
        #endregion
        /// <summary>
        /// GetAllMilkPurchase
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="finYear"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public List<MilkPurchaseBO> GetAllMilkPurchase(int Id)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetMilkPurchase(Id, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(MP => new MilkPurchaseBO
                    {
                        ID = MP.ID,
                        TransNo = MP.TransNo,
                        TotalAmount = (decimal)MP.TotalAmount,
                        TotalQty = (decimal)MP.TotalQty,
                        Date = (DateTime)MP.Date,
                        IsDraft = (bool)MP.IsDraft,
                        SupplierName = MP.SupplierName,
                        DirectInvoice=(bool)MP.IsDirectInvoice
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MilkPurchseTransBO> GetAllMilkPurchaseTrans(int Id)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetMilkPurchaseTransDetails(Id, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(MPT => new MilkPurchseTransBO
                    {
                        MilkPurchaseID = (int)MPT.MilkPurchaseID,
                        MilkSupplierID = (int)MPT.MilkSupplierID,
                        MilkSupplier = MPT.MilkSupplier,
                        SupplierCode = MPT.MilkSupplierCode,
                        SlipNo = MPT.SlipNo,
                        Qty = (decimal)MPT.Qty,
                        Rate = (decimal)MPT.Rate,
                        Amount = (decimal)MPT.Amount,
                        FatRangeID = (int)MPT.FatRangeID,
                        FatContentFrom = (decimal)MPT.FatContentFrom,
                        FatContentTo = (decimal)MPT.FatContentTo,
                        Remarks=MPT.Remarks

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MilkSupplierBO> GetMilkSupplierList(string term)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetMilkSuppliersList(GeneralBO.LocationID, GeneralBO.ApplicationID, term).Select(a => new MilkSupplierBO
                    {
                        SupplierID = a.ID,
                        SupplierName = a.Name,
                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///GetMilkFatRange  for Milk purchase page
        /// </summary>
        /// <param name="hint"></param>
        /// <returns></returns>
        public List<MilkFatRangeBO> GetMilkFatRange(string hint)
        {
            List<MilkFatRangeBO> itm = new List<MilkFatRangeBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    itm = dbEntity.SpGetMilkFatRangeList(hint).Select(it => new MilkFatRangeBO
                    {
                        ID = it.ID,
                        FatRange = it.FatRange,
                        Price = it.Price ?? 0,
                        SupplierID = (int)it.SupplierID,
                        WaterFrom = it.WaterFrom ?? 0,
                        WaterTo = (int)it.WaterTo

                    }).ToList();
                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saving milk purchase order
        /// </summary>
        /// <param name="_milkMaster"></param>
        /// <param name="_milkTrans"></param>
        /// <returns></returns>
        public bool SaveMilkPurchase(MilkPurchaseBO _milkMaster, List<MilkPurchseTransBO> _milkTrans)
        {
            bool IsSuccess = false;
            using (PurchaseEntities entity = new PurchaseEntities())
            {
                using (var transaction = entity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "MilkPurchase";
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        if (_milkMaster.IsDraft)
                        {
                            FormName = "DraftMilkPurchase";
                        }

                        var j = entity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        entity.SaveChanges();

                        ObjectParameter POId = new ObjectParameter("PurchaseOrderID", typeof(int));

                        int? MPId = entity.SpCreateMilkPurchase(
                            SerialNo.Value.ToString(),
                            _milkMaster.Date,
                            _milkMaster.TotalAmount,
                            _milkMaster.TotalQty,
                            GeneralBO.CreatedUserID,
                            _milkMaster.IsDraft,
                            _milkMaster.DirectInvoice,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            _milkMaster.RequisitionIDs,
                            POId);

                        if (MPId != null)
                        {
                            foreach (var itm in _milkTrans)
                            {
                                entity.SpCreateMilkPurchaseTrans(
                                    Convert.ToInt32(POId.Value),
                                    itm.MilkSupplierID,
                                    itm.SlipNo,
                                    itm.Qty,
                                    itm.FatRangeID,
                                    itm.Rate,
                                    itm.Amount,
                                    itm.Remarks,
                                    GeneralBO.CreatedUserID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                    );
                            }
                            if (_milkMaster.IsDraft == false && _milkMaster.DirectInvoice)
                            {
                                entity.SpCreateMilkPurchaseInvoice(
                               Convert.ToInt32(POId.Value),
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID
                                );
                            }

                        }
                    
                        IsSuccess = true;
                        entity.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }

            return IsSuccess;
        }

        public bool UpdateMilkPurchase(MilkPurchaseBO _milkMaster, List<MilkPurchseTransBO> _milkTrans)
        {
            bool IsSuccess = false;
            using (PurchaseEntities entity = new PurchaseEntities())
            {
                using (var transaction = entity.Database.BeginTransaction())
                {
                    try
                    {
                        entity.SpUpdateMilkPurchase(_milkMaster.ID, _milkMaster.Date, _milkMaster.TotalAmount
                             , _milkMaster.TotalQty, GeneralBO.CreatedUserID, _milkMaster.IsDraft, _milkMaster.RequisitionIDs,_milkMaster.DirectInvoice, GeneralBO.FinYear,
                             GeneralBO.LocationID, GeneralBO.ApplicationID);
                        foreach (var itm in _milkTrans)
                        {
                            entity.SpCreateMilkPurchaseTrans(_milkMaster.ID, itm.MilkSupplierID, itm.SlipNo, itm.Qty, itm.FatRangeID,
                                itm.Rate, itm.Amount, itm.Remarks, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                        }
                        if (_milkMaster.IsDraft == false && _milkMaster.DirectInvoice)
                        {
                            entity.SpCreateMilkPurchaseInvoice(
                           _milkMaster.ID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            );
                        }
                        entity.SaveChanges();
                        transaction.Commit();
                        IsSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                    }
                }
                return IsSuccess;
            }
        }

        public MilkFatRangeBO GetMilkFatRangePrice(int ID)
        {
            MilkFatRangeBO itm = new MilkFatRangeBO();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetMilkFatRangePrice(ID).Select(it => new MilkFatRangeBO
                    {
                        Price = (decimal)it.Price,
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MilkPurchaseRequisitionBO> GetMilkPurchaseRqusition(DateTime Date)
        {
            List<MilkPurchaseRequisitionBO> itm = new List<MilkPurchaseRequisitionBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    itm = dbEntity.SpGetMilkPurchaseRequisition(Date, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(it => new MilkPurchaseRequisitionBO
                    {
                        ID = it.ID,
                        PrNumber = it.Code,
                        ExpectedDate = (DateTime)it.Date,
                        Qty =(decimal) it.Quantity,
                        FromDept = it.DEptName,
                       
                    }).ToList();
                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatatableResultBO GetMilkPurchaseList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string QuantityHint, string AmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var result = dbEntity.SpGetMilkPurchaseList(Type, TransNoHint, TransDateHint, SupplierNameHint, QuantityHint, AmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransDate = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                SupplierName = item.SupplierName,
                                TotalQty = item.TotalQty,
                                TotalAmount = item.TotalAmount,
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

