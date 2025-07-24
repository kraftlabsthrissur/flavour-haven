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
    public class ChequeStatusDAL
    {
        public List<ChequeStatusTransBO> getChequeStatus(string InstrumentStatus, DateTime FromReciptDate, DateTime ToReciptDate)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {


                return dbEntity.SpGetInstrumentStatusList(FromReciptDate, ToReciptDate, InstrumentStatus, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ChequeStatusTransBO
                {
                    InstrumentNumber = a.InstrumentNo,
                    InstrumentDate = (DateTime)a.InstrumentDate,
                    ChequeStatus = a.InstrumentStatus,
                    BankCharges = Convert.ToDecimal(a.BankCharges),
                    CustomerName = a.CustomerName,
                    CustomerID = (int)a.CustomerID,
                    InstrumentAmount = Convert.ToDecimal(a.InstrumentAmount),
                    StatusChangeDate = (DateTime)a.StatusChangeDate,
                    TotalAmount = (decimal)a.NetAmount,
                    ChargesToCustomer = Convert.ToDecimal(a.ChargesToCustomer),
                    Remarks = a.Remarks,
                    VoucherNo = a.ReceiptVoucherNo,
                    VoucherID = (int)a.ReceiptVoucherID,
                    CGST=(decimal)a.CGST,
                    SGST =(decimal)a.SGST,
                    IGST =(decimal)a.IGST,
                    StateID=(int)a.StateID,
                    ChequeReceivedDate=(DateTime)a.ChequeReceivedDate
                }).ToList();

            }
        }
        public string Save(ChequeStatusBO Master, List<ChequeStatusTransBO> Details)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {

                    try
                    {
                        string FormName = "ChequeStatus";
                        ObjectParameter chequeStatusID = new ObjectParameter("ChequeStatusID", typeof(int));

                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));

                        if (Master.IsDraft)
                        {
                            FormName = "DraftChequeStatus";
                        }

                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);


                        var i = dbEntity.SpCreateChequeStatus(SerialNo.Value.ToString(),
                            Master.Date,
                            Master.ReceiptDateFrom,
                            Master.ReceiptDateTo,
                            Master.InstrumentStatus,
                            Master.IsDraft,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            chequeStatusID);                       

                        if (chequeStatusID.Value != null)
                        {
                            foreach (var itm in Details)
                            {
                                dbEntity.SpCreateChequeStatusTrans(Convert.ToInt32(chequeStatusID.Value),
                                    itm.InstrumentNumber,
                                    itm.InstrumentDate,
                                    itm.ChequeStatus,
                                    itm.StatusChangeDate,
                                    itm.CustomerID,
                                    itm.InstrumentAmount,
                                    itm.BankCharges,
                                    itm.TotalAmount,
                                    itm.ChargesToCustomer,
                                    itm.Remarks,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    itm.IsActive,
                                    itm.VoucherNo,
                                    itm.VoucherID,
                                    itm.CGST,
                                    itm.SGST,
                                    itm.IGST);
                            }

                        };
                        transaction.Commit();
                        return chequeStatusID.Value.ToString();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }

            }
        }
        public List<ChequeStatusBO> getChequeStatusList()
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                return dbEntity.SpGetChequeStatusList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ChequeStatusBO
                {
                    ID = (int)a.ID,
                    TransNo = a.TransNo,
                    Date = Convert.ToDateTime(a.Date),
                    InstrumentStatus = a.InstrumentStatus,
                    ReceiptDateFrom = (DateTime)a.FromReceiptDate,
                    ReceiptDateTo = (DateTime)a.ToReceiptDate,
                    IsDraft = (bool)a.IsDraft,
                    CustomerName = a.CustomerName

                }).ToList();
            }
        }

        public List<ChequeStatusBO> getChequeStatusDetails(int ChequeStatusID)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                return dbEntity.SpGetChequeStatusDetails(ChequeStatusID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ChequeStatusBO
                {
                    ID = a.ID,
                    TransNo = a.TransNo,
                    Date = Convert.ToDateTime(a.Date),
                    InstrumentStatus = a.InstrumentStatus,
                    ReceiptDateFrom = (DateTime)a.FromReceiptDate,
                    ReceiptDateTo = (DateTime)a.ToReceiptDate,
                    IsDraft = (bool)a.IsDraft

                }).ToList();
            }
        }

        public List<ChequeStatusTransBO> getChequeStatusTransDetails(int ChequeStatusID)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                return dbEntity.SpGetChequeStatusTransDetails(ChequeStatusID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ChequeStatusTransBO
                {
                    InstrumentNumber = a.InstrumentNo,
                    InstrumentDate = (DateTime)a.InstrumentDate,
                    ChequeStatus = a.InstrumentStatus,
                    StatusChangeDate = (DateTime)a.StatusChangeDate,
                    CustomerID = (int)a.CustomerID,
                    CustomerName = a.CustomerName,
                    InstrumentAmount = (decimal)a.InstrumentAmount,
                    BankCharges = (decimal)a.BankCharges,
                    TotalAmount = (decimal)a.NetAmount,
                    ChargesToCustomer = (decimal)a.ChargesToCustomer,
                    Remarks = a.Remarks,
                    VoucherNo = a.ReceiptvoucherNo,
                    VoucherID=(int)a.ReceiptvoucherID,
                    CGST=(decimal)a.CGST,
                    SGST= (decimal)a.SGST,
                    IGST= (decimal)a.IGST,
                    StateID=(int)a.StateID,
                    ChequeReceivedDate=(DateTime)a.ChequeReceivedDate
                }).ToList();
            }
        }
        public bool Update(ChequeStatusBO Master, List<ChequeStatusTransBO> Details)
        {
            bool IsSuccess = false;
            using (AccountsEntities entity = new AccountsEntities())
            {
                using (var transaction = entity.Database.BeginTransaction())
                {

                    try
                    {

                        var j = entity.SpUpdateChequeStatus(Master.ID, Master.Date, Master.InstrumentStatus, Master.ReceiptDateFrom, Master.ReceiptDateTo,
                          Master.IsDraft, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                        foreach (var itm in Details)
                        {
                            entity.SpCreateChequeStatusTrans(Master.ID,
                                    itm.InstrumentNumber,
                                    itm.InstrumentDate,
                                    itm.ChequeStatus,
                                    itm.StatusChangeDate,
                                    itm.CustomerID,
                                    itm.InstrumentAmount,
                                    itm.BankCharges,
                                    itm.TotalAmount,
                                    itm.ChargesToCustomer,
                                    itm.Remarks,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    itm.IsActive,
                                    itm.VoucherNo,
                                    itm.VoucherID,
                                    itm.CGST,itm.SGST,itm.IGST);
                        }
                        entity.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
                return IsSuccess;
            }
        }

        public DatatableResultBO GetChequeStatusList(string Type, string StatusNoHint, string TransDateHint, string InstrumentStatusHint, string FromDateHint, string ToDateHint, string CustomerNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetChequeStatusListForDatatable(Type, StatusNoHint, TransDateHint, InstrumentStatusHint, FromDateHint, ToDateHint, CustomerNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                InstrumentStatus = item.InstrumentStatus,
                                FromReceiptDate = ((DateTime)item.FromReceiptDate).ToString("dd-MMM-yyyy"),
                                ToReceiptDate = ((DateTime)item.ToReceiptDate).ToString("dd-MMM-yyyy"),
                                CustomerName = item.CustomerName,
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
