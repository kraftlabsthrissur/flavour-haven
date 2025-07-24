using AutoMapper;
using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SLADAL
    {

        public List<SLAError> GetErrorsDuringSLAMapping()
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {

                    return dbEntity.SpGetErrorsDuringSLAMapping().Select(a => new SLAError()
                    {
                        SLAMappingID = (int)a.SLAMappingID,
                        TrnType = a.TrnType,
                        KeyValue = a.KeyValue,
                        TableName = a.TableName,
                        TransID = (int)a.TransID,
                        SupplierID = (int)a.SupplierID,
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        SupplierName = a.Supplier,
                        Description = a.Description,
                        Remarks = a.Remarks,
                        CreatedDate = (DateTime)a.CreatedDate,
                        TablePrimaryID = (int)a.TablePrimaryID,
                        Area = a.Area,
                        Controller = a.Controller,
                        Method = a.Action,
                        DocumentNo = a.DocumentNo,
                        DocumentTable = a.DocumentTable
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<SLAPostedBO> GetAccountsPostedValues(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {

                    return dbEntity.spGetAccountsPostedValues(FromDate, ToDate).Select(a => new SLAPostedBO()
                    {
                        AccountDebitID = a.AccountDebitID,
                        DebitAccount = a.Debit,
                        AccountCreditID = a.AccountCreditID,
                        CreditAccount = a.Credit,
                        Amount = (decimal)a.AccountAmt,
                        Date = (DateTime)a.Date,
                        VoucherDate = (DateTime)a.VoucherDate,
                        DocumentNo = a.DocumentNo,
                        DocumentTable = a.DocumentTable

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SLAToBePostedBO> GetAccountsToBePostedValues(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                                        return dbEntity.spGetAccountsToBePostedValues(FromDate, ToDate).Select(a => new SLAToBePostedBO()
                    {
                        AccountDebitID = (int)a.AccountDebitID,
                        DebitAccount = a.Debit,
                        AccountCreditID = (int)a.AccountCreditID,
                        CreditAccount = a.Credit,
                        Amount = (decimal)a.Amount,
                        ItemID = (int)a.ItemID,
                        ItemName = a.ItemName,
                        SupplierID = (int)a.SupplierID,
                        SupplierName = a.Supplier,
                        IsProcessed = (bool)a.ISProcessed,
                        SLAMappingItemID = (int)a.SLAMappingItemID,
                        Date = (DateTime)a.Date,
                        TablePrimaryID = (int)a.TablePrimaryID,
                        Area = a.Area,
                        Controller = a.Controller,
                        Method = a.Action,
                        DocumentNo = a.DocumentNo,
                        DocumentTable = a.DocumentTable
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SLAValuesBO> GetUnMappedValuesToSLA()
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetUnMappedValuesToSLA().Select(a => new SLAValuesBO()
                    {
                        TransationType = a.TrnType,
                        KeyValue = a.KeyValue,
                        Amount = (decimal)a.Amount,
                        Event = a.Event,
                        TransationID = (int)a.TransID,
                        Date = (DateTime)a.Date,
                        TablePrimaryID = (int)a.TablePrimaryID,
                        Area = a.Area,
                        Controller = a.Controller,
                        Method = a.Action,
                        DocumentNo = a.DocumentNo,
                        DocumentTable = a.DocumentTable

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CreateAccountEntryDetails()
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    dbEntity.SpCreateAccountEntryDetails();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GenerateAccountEntryDataUsingSLARules()
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    dbEntity.SpGenerateAccountEntryDataUsingSLARules();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SLAValuesBO> GetTransTypeAutoComplete(string hint)
        {
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    return dbEntity.SpGetTransTypeAutoComplete(hint).Select(a => new SLAValuesBO
                    {
                        TransationType = a.TransactionType
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SLABO> GetSLAKeyValueByTransactionType(string TransactionType)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetSLAKeyValueByTransactionType(TransactionType).Select(a => new SLABO
                    {
                        ID=a.ID,                
                        KeyValue = a.KeyValue

                    }).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SLABO> GetSLAFilterByType(string Type)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetSLAFilterByType(Type).Select(a => new SLABO
                    {
                        SLAFIlter = a.Filter,
                        ID = a.ID

                    }).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SLABO> GetTransactionTypeByProcessCycle(string ProcessCycle)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetTransactionTypeByProcessCycle(ProcessCycle).Select(a => new SLABO
                    {
                        ID=a.ID,
                        TransactionType = a.TransactionType
                  

                    }).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SLABO> GetProcessCycleList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetProcessCycleList().Select(a => new SLABO
                    {
                        ID=a.ID,
                        ProcessCycle=a.ProcessCycle
                        
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public int CreateSLA(SLABO slaBO)
        {

            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ObjectParameter Count = new ObjectParameter("Count", typeof(int));

                    var result = dbEntity.SpGetCheckSLARuleExist(slaBO.Cycle, slaBO.TransactionType, slaBO.KeyValue, slaBO.Item, slaBO.ItemAccountsCategory,
                        slaBO.ItemTaxCategory, slaBO.BatchPrefix, slaBO.Supplier, slaBO.SupplierAccountsCategory, slaBO.SupplierTaxCategory,
                        slaBO.Customer, slaBO.CustomerAccountsCategory, slaBO.CustomerCategory, slaBO.CustomerTaxCategory, slaBO.CostComponent,
                        slaBO.DepartmentCategory, slaBO.Capitilization, slaBO.Location, Count);
                    var RowCount = Convert.ToInt32(Count.Value);
                    if (RowCount == 0)
                    {
                        var config = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<SLABO, SLA>();
                        });
                        IMapper iMapper = config.CreateMapper();
                        var destination = iMapper.Map<SLABO, SLA>(slaBO); 
                        dbEntity.SLAs.Add(destination);
                        return dbEntity.SaveChanges();
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (DbEntityValidationResult item in e.EntityValidationErrors)
                {
                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;
                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int UpdateSLA(SLABO slaBO)
        {

            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<SLABO, SLA>();
                    });
                    IMapper iMapper = config.CreateMapper();
                    SLA sLA = dbEntity.SLAs.Find(slaBO.ID);
                    iMapper.Map(slaBO, sLA);
                    dbEntity.SpLogChange("sLA", "ID", slaBO.ID, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);
                    return dbEntity.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (DbEntityValidationResult item in e.EntityValidationErrors)
                {
                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;
                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public SLABO GetSLADetails(int id)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<SpGetSLADetails_Result, SLABO>().ForMember(d => d.ID, o => o.Ignore());
                    });

                    IMapper iMapper = config.CreateMapper();
                    var result = dbEntity.SpGetSLADetails(id).FirstOrDefault();
                    SLABO slaBO = iMapper.Map<SpGetSLADetails_Result, SLABO>(result);                   
                    return slaBO;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (DbEntityValidationResult item in e.EntityValidationErrors)
                {
                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;
                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetAllSLAList(string CycleHint, string TransactionTypeHint, string KeyValueHint, string ItemHint, string SupplierHint, string CustomerHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetAllSLAList(CycleHint, TransactionTypeHint, KeyValueHint, ItemHint, SupplierHint, CustomerHint, SortField, SortOrder, Offset, Limit).ToList();
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
                                Cycle = item.Cycle,
                                TransactionType = item.TransactionType,
                                KeyValue = item.KeyValue,
                                Guidance = item.Guidance,
                                Item = item.Item,
                                ItemAccountsCategory = item.ItemAccountsCategory,
                                ItemTaxCategory = item.ItemTaxCategory,
                                Supplier = item.Supplier,
                                BatchPrefix = item.BatchPrefix,
                                SupplierAccountsCategory = item.SupplierAccountsCategory,
                                SupplierTaxCategory = item.SupplierTaxCategory,
                                Customer = item.Customer,
                                SupplierTaxSubCategory = item.SupplierTaxSubCategory,
                                CustomerTaxCategory = item.CustomerTaxCategory,
                                CustomerAccountsCategory = item.CustomerAccountsCategory,
                                CostComponent = item.CostComponent,
                                CustomerCategory = item.CustomerCategory,
                                Capitilization = item.Capitilization,
                                DepartmentCategory = item.DepartmentCategory,
                                Condition2 = item.Condition2,
                                Condition1 = item.Condition1,
                                Remarks = item.Remarks,
                                DebitAccount = item.DebitAccount,
                                DebitAccountDescription = item.DebitAccountDescription,
                                CreditAccount = item.CreditAccount,
                                CreditAccountDescription = item.CreditAccountDescription
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

        public DatatableResultBO GetSLAValuesList(string Type, string DateHint, string TransationTypeHint, string KeyValueHint, string AmountHint, string EventHint, string DocumentTableHint, string DocumentNumberHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetSLAValuesList(Type,DateHint,TransationTypeHint,KeyValueHint,AmountHint,EventHint,DocumentTableHint,DocumentNumberHint,SortField,SortOrder,Offset,Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                TransationType = item.TrnType,
                                KeyValue = item.KeyValue,
                                Amount = (decimal)item.Amount,
                                Event = item.Event,
                                TransationID = (int)item.TransID,
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                TablePrimaryID = (int)item.TablePrimaryID,
                                Method = item.Action,
                                DocumentNo = item.DocumentNo,
                                DocumentTable = item.DocumentTable
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

        public DatatableResultBO GetSLAToBePostedList(DateTime FromDate,DateTime ToDate,string Type, string DateHint, string DebitAccountHint, string DebitAccountNameHint, string CreditAccountHint, string CreditAccountNameHint, string AmountHint, string ItemNameHint, string SupplierNameHint, string DocumentTableHint, string DocumentNumberHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetSLAToBePostedList(FromDate,ToDate,Type, DateHint, DebitAccountHint, DebitAccountNameHint, CreditAccountHint, CreditAccountNameHint, AmountHint, ItemNameHint, SupplierNameHint, DocumentTableHint, DocumentNumberHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                AccountDebitID = (int)item.AccountDebitID,
                                DebitAccount = item.Debit,
                                AccountCreditID = (int)item.AccountCreditID,
                                CreditAccount = item.Credit,
                                Amount = (decimal)item.Amount,
                                ItemID = (int)item.ItemID,
                                ItemName = item.ItemName,
                                SupplierID = (int)item.SupplierID,
                                SupplierName = item.Supplier,
                                IsProcessed = (bool)item.ISProcessed,
                                SLAMappingItemID = (int)item.SLAMappingItemID,
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                TablePrimaryID = (int)item.TablePrimaryID,
                                Method = item.Action,
                                DocumentNo = item.DocumentNo,
                                DocumentTable = item.DocumentTable
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

        public DatatableResultBO GetSLAPostedList(DateTime FromDate, DateTime ToDate, string Type, string DateHint, string DebitAccountHint, string DebitAccountNameHint, string CreditAccountHint, string CreditAccountNameHint, string AmountHint, string DocumentTableHint, string DocumentNumberHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetSLAPostedList(FromDate, ToDate, Type, DateHint, DebitAccountHint, DebitAccountNameHint, CreditAccountHint, CreditAccountNameHint, AmountHint, DocumentTableHint, DocumentNumberHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                AccountDebitID = item.AccountDebitID,
                                DebitAccount = item.Debit,
                                AccountCreditID = item.AccountCreditID,
                                CreditAccount = item.Credit,
                                Amount = (decimal)item.AccountAmt,
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                DocumentNo = item.DocumentNo,
                                DocumentTable = item.DocumentTable
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

        public DatatableResultBO GetSLAErrorList(DateTime FromDate, DateTime ToDate, string Type, string DateHint, string TransationTypeHint, string KeyValueHint, string EventHint, string ItemNameHint, string SupplierNameHint, string DescriptionHint, string RemarksHint, string DocumentTableHint, string DocumentNumberHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AccountsEntities dbEntity = new AccountsEntities())
                {
                    var result = dbEntity.SpGetSLAErrorList(FromDate, ToDate, Type, DateHint, TransationTypeHint, KeyValueHint, EventHint, ItemNameHint, SupplierNameHint, DescriptionHint, RemarksHint, DocumentTableHint, DocumentNumberHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                SLAMappingID = (int)item.SLAMappingID,
                                TrnType = item.TrnType,
                                KeyValue = item.KeyValue,
                                TableName = item.TableName,
                                TransID = (int)item.TransID,
                                SupplierID = (int)item.SupplierID,
                                ItemID = item.ItemID,
                                ItemName = item.ItemName,
                                SupplierName = item.Supplier,
                                Description = item.Description,
                                Remarks = item.Remarks,
                                CreatedDate = ((DateTime)item.CreatedDate).ToString("dd-MMM-yyyy"),
                                TablePrimaryID = (int)item.TablePrimaryID,
                                Area = item.Area,
                                Controller = item.Controller,
                                Method = item.Action,
                                DocumentNo = item.DocumentNo,
                                DocumentTable = item.DocumentTable
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
