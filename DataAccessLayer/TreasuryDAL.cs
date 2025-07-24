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
    public class TreasuryDAL
    {
        public List<TreasuryBO> GetTreasuryList()
        {
            try
            {

                using (AccountsEntities dbEntity = new AccountsEntities())
                {

                    return dbEntity.SPGetTreasuryList().Select(a => new TreasuryBO
                    {
                        ID = a.ID,
                        Type = a.Type,
                        AccountCode = a.AccountCode,
                        BankName = a.BankName,
                        AliasName = a.AliasName,
                        CoBranchName = a.CoBranchName,
                        BankBranchName = a.BankBranchName,
                        AccountType1 = a.AccountType1
                    }
                ).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TreasuryBO GetTreasuryDetails(int TreasuryID)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpTreasuryDetails(TreasuryID).Select(a => new TreasuryBO
                    {
                        ID = a.ID,
                        Type = a.Type,
                        BankCode = a.BankCode,
                        AccountCode = a.AccountCode,
                        AccountName = a.AccountName,
                        BankName = a.BankName,
                        AliasName = a.AliasName,
                        CoBranchName = a.CoBranchName,
                        BankBranchName = a.BankBranchName,
                        AccountType1 = a.AccountType1,
                        AccountType2 = a.AccountType2,
                        AccountNo = a.AccountNo,
                        IFSC = a.IFSC,
                        LocationMappingID = a.LocationMappingID ?? 0,
                        LocationMapping = a.LocationMapping,
                        StartDate = (DateTime)a.StartDate,
                        EndDate = (DateTime)a.EndDate,
                        remarks = a.remarks,
                        IsPayment = a.IsPayment ?? false,
                        IsReceipt = a.IsReceipt ?? false
                    }).FirstOrDefault();
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public int CreateTreasury(TreasuryBO treasuryBO)
        {

            try
            {
                ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(string));
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    ObjectParameter ID = new ObjectParameter("ID", typeof(int));
                    string Type = "Treasury";
                    var i = dbEntity.SPCreateTreasury(
                        treasuryBO.Type,
                        treasuryBO.BankCode,
                        treasuryBO.AccountCode,
                        treasuryBO.BankName,
                        treasuryBO.AliasName,
                        treasuryBO.CoBranchName,
                        treasuryBO.BankBranchName,
                        treasuryBO.AccountType1,
                        treasuryBO.AccountType2,
                        treasuryBO.AccountNo,
                        treasuryBO.IFSC,
                        treasuryBO.LocationMappingID,
                        treasuryBO.StartDate,
                        treasuryBO.EndDate,
                        treasuryBO.remarks,
                        treasuryBO.IsPayment,
                        treasuryBO.IsReceipt,
                        treasuryBO.OpeningAmount,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID,
                        ID
                        );
                    int PartyID = Convert.ToInt32(ID.Value);
                    return dbEntity.SpCreateAccountHeadByType(
                                 Type,
                                 PartyID,
                                 GeneralBO.CreatedUserID,
                                 GeneralBO.ApplicationID,
                                 GeneralBO.LocationID,
                                 GeneralBO.FinYear
                             );
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public int UpdateTreasury(TreasuryBO treasuryBO)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    string Type = "Treasury";
                    var i = dbEntity.SpUpdateTreasury(
                        treasuryBO.ID,
                        treasuryBO.Type,
                        treasuryBO.BankCode,
                        treasuryBO.AccountCode,
                        treasuryBO.BankName,
                        treasuryBO.AliasName,
                        treasuryBO.CoBranchName,
                        treasuryBO.AccountType1,
                        treasuryBO.AccountType2,
                        treasuryBO.BankBranchName,
                        treasuryBO.AccountNo,
                        treasuryBO.IFSC,
                        treasuryBO.LocationMappingID,
                        treasuryBO.StartDate,
                        treasuryBO.EndDate,
                        treasuryBO.remarks,
                        treasuryBO.IsPayment,
                        treasuryBO.IsReceipt,
                        GeneralBO.CreatedUserID,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID);

                    int PartyID = Convert.ToInt32(treasuryBO.ID);
                    return dbEntity.SpUpdateAccountHeadByType(
                                 Type,
                                 PartyID,
                                 treasuryBO.AliasName,
                                 GeneralBO.CreatedUserID,
                                 GeneralBO.ApplicationID,
                                 GeneralBO.LocationID,
                                 GeneralBO.FinYear
                             );

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<TypeBO> GetTreasuryType()
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetTreasuryType().Select(a => new TypeBO
                    {
                        Name = a.Name
                    }).ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<TreasuryBO> GetBank(string ModuleName, String Mode)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetBankName(ModuleName, Mode, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new TreasuryBO
                    {
                        BankName = a.Bankname,
                        ID = a.ID,
                        CreditBalance = a.CreditBalance


                    }).ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<TreasuryBO> GetReceiverBankList()
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetReceiverBankName( GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new TreasuryBO
                    {
                       ReceiverBankName = a.Bankname,
                        ReceiverBankID =(int) a.ID
                       // CreditBalance = a.CreditBalance


                    }).ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<TreasuryBO> GetBank()
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetBanklist(GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new TreasuryBO
                    {
                        BankName = a.Bankname,
                        ID = a.ID,
                        CreditBalance = a.CreditBalance


                    }).ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<TreasuryBO> GetBankList()
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetBanklist(GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new TreasuryBO
                    {
                        BankName = a.Bankname,
                        ID = a.ID,
                        CreditBalance = a.CreditBalance


                    }).ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<TreasuryBO> GetAccountCodeAutoComplete(string CodeHint)
        {
            List<TreasuryBO> AccountHead = new List<TreasuryBO>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    AccountHead = dbEntity.SpGetAccountCodeAutoComplete(CodeHint).Select(a => new TreasuryBO
                    {
                        ID = a.ID,
                        Code = a.AccountID,
                        AccountName = a.AccountName,

                    }).ToList();
                    return AccountHead;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TreasuryBO> GetTreasuryDetailsForAutoComplete(string codeHint)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetTreasuryDetailsForAutocomplete(codeHint).Select(a => new TreasuryBO
                    {
                        ID = a.ID,
                        AccountNo = a.AccountID,
                        AccountName = a.BankName
                    }
                    ).ToList();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TreasuryBO> GetBank(int LocationID)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetBanklist(GeneralBO.CreatedUserID, LocationID, GeneralBO.ApplicationID).Select(a => new TreasuryBO
                    {
                        BankName = a.Bankname,
                        ID = a.ID


                    }).ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<TreasuryBO> GetBankForCounterSales(string mode)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetBankForCounterSales(mode, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new TreasuryBO
                    {
                        BankName = a.Bankname,
                        ID = a.ID


                    }).ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DatatableResultBO GetTreasuryList(string Type, string AccountCode, string BankName, string AliasName, string CoBranchName, string BankBranchName, string AccountType, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {

                    var result = dbEntity.SpGetListForTreasury(Type, AccountCode, BankName, AliasName, CoBranchName, BankBranchName, AccountType, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Type = item.Type,
                                AccountCode = item.AccountCode,
                                BankName = item.BankName,
                                AliasName = item.AliasName,
                                CoBranchName = item.CoBranchName,
                                BankBranchName = item.BankBranchName,
                                AccountType1 = item.AccountType1
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




