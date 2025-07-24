using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.FundTranferBO;

namespace DataAccessLayer
{
    public class FundTranferDAL
    {
        public List<FundTranferBO> FundTransferList()
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                return dbEntity.SpGetFundTransferList(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new FundTranferBO
                {
                    ID = a.ID,
                    TransNo = a.TransNo,
                    Date = (DateTime)a.Date,
                    FromLocation = a.FromLocation,
                    ToLocation = a.ToLocation,
                    ModeOfPayment = a.ModeOfPayment,
                    TotalAmount = a.TotalAmount,
                    IsDraft=(bool)a.IsDraft
                }).ToList();
            }
        }

        public List<FundTranferBO> GetFundTransferDetails(int FundTransferID)
        {
            List<FundTranferBO> list = new List<FundTranferBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetFundTransferDetails(FundTransferID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new FundTranferBO
                {
                    ID = a.ID,
                    TransNo = a.TransNo,
                    Date = (DateTime)a.Date,
                    TotalAmount = a.TotalAmount,
                    IsDraft=(bool)a.IsDraft
                }).ToList();
                return list;
            }
        }
        public List<FundTranferTransBO> GetFundTransferTransDetails(int FundTransferID)
        {
            List<FundTranferTransBO> list = new List<FundTranferTransBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {

                list = dEntity.SpGetFundTransferTransDetails(FundTransferID,GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new FundTranferTransBO
                {
                    FromLocation = a.FromLocation,
                    FromLocationID = (int)a.FromLocationID,
                    ToLocation = a.ToLocation,
                    ToLocationID = (int)a.ToLocationID,
                    FromBank = a.FromBank,
                    FromBankID = (int)a.FromBankID,
                    ToBank = a.ToBank,
                    ToBankID = (int)a.ToBankID,
                    ModeOfPayment = a.ModeOfPayment,
                    ModeOfPaymentID = (int)a.ModeOfPaymentID,
                    Amount = (decimal)a.Amount,
                    InstrumentNumber = a.InstrumentNumber,
                    InstrumentDate = (DateTime)a.InstrumentDate,
                    Remarks = a.Remarks,
                    CreditBalance=a.CreditBalance
                }).ToList();
                return list;
            }
        }
        public List<FundTranferBO> GetLocationWiseBank(int LocationID)
        {
            List<FundTranferBO> list = new List<FundTranferBO>();
            using (AccountsEntities dEntity = new AccountsEntities())
            {
                list = dEntity.SpGetLocationwiseBanks(GeneralBO.CreatedUserID,LocationID,GeneralBO.ApplicationID).Select(a => new FundTranferBO
                {
                    ID = a.ID,
                    BankName = a.Bankname,
                    CreditBalance=a.CreditBalance

                }).ToList();
                return list;
            }
        }
        public List<FundTranferBO> GetFundTransferToLocation(int LocationID)
        {
            List<FundTranferBO> list = new List<FundTranferBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                list = dEntity.SpGetLocationByLocationHead(LocationID).Select(a => new FundTranferBO
                {
                    ID = a.ID,
                    ToLocation = a.Name
                }).ToList();
                return list;
            }
        }
        public string Save(FundTranferBO Master, List<FundTranferTransBO> Details)
        {
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "FundTransfer";
                        ObjectParameter fundtransferID = new ObjectParameter("FundTransferID", typeof(int));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        if (Master.IsDraft)
                        {
                            FormName = "DraftFundTransfer";
                        }

                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        dbEntity.SaveChanges();

                        var i = dbEntity.SpCreateFundTransfer(SerialNo.Value.ToString(), Master.Date, Master.TotalAmount, Master.IsDraft, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, fundtransferID);

                        dbEntity.SaveChanges();

                        if (fundtransferID.Value != null)
                        {
                            foreach (var itm in Details)
                            {
                                dbEntity.SpCreateFundTransferTrans(Convert.ToInt32(fundtransferID.Value), itm.FromLocationID, itm.ToLocationID, itm.FromBankID, itm.ToBankID, itm.Amount, itm.ModeOfPaymentID, itm.InstrumentNumber, itm.InstrumentDate, itm.Remarks, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, RetValue);
                            }

                        };
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new DatabaseException("Total exceeds credit limit");
                        }
                        //dbEntity.SpCreateAutomaticFundTransferReceipt(Convert.ToInt32(fundtransferID.Value), GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);


                        transaction.Commit();
                        return fundtransferID.Value.ToString();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }

            }
        }
        public bool Update(FundTranferBO Master, List<FundTranferTransBO> Details)
        {
            bool IsSuccess = false;
            using (AccountsEntities entity = new AccountsEntities())
            {
                using (var transaction = entity.Database.BeginTransaction())
                {
                    ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                    try
                    {
                         var j = entity.SpUpdateFundTransfer(Master.ID, Master.Date, Master.TotalAmount, Master.IsDraft, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);
                        foreach (var itm in Details)
                        {
                            var i = entity.SpCreateFundTransferTrans(Master.ID, itm.FromLocationID, itm.ToLocationID, itm.FromBankID, itm.ToBankID, itm.Amount, itm.ModeOfPaymentID, itm.InstrumentNumber, itm.InstrumentDate, itm.Remarks, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, RetValue);
                        }
                        entity.SaveChanges();
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new DatabaseException("Total exceeds credit limit");
                        }
                        entity.SpCreateAutomaticFundTransferReceipt(Master.ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;

                    }
                }
                return IsSuccess;
            }

        }

        public DatatableResultBO GetFundTransferList(string Type, string FundTransferNo, string FundTransferDate, string FromLocation, string ToLocation, string ModeOfPayment, string TotalAmount, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetFundTransferListForDataTable(Type, FundTransferNo, FundTransferDate, FromLocation, ToLocation, ModeOfPayment, TotalAmount, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                FundTransferNo = item.TransNo,
                                FundTransferDate=((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                FromLocation=item.FromLocation,
                                ToLocation=item.ToLocation,
                                ModeOfPayment=item.ModeOfPayment,
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
                throw e;
            }
            return DatatableResult;
        }
    }

}