using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class SupplierDAL
    {

        public int CreateSupplier(SupplierBO supplierBO, List<AddressBO> _AddressList, List<SupplierBO> _SupplierItemCategoryList, List<SupplierBO> _SupplierLocationList, List<RelatedSupplierBO> RelatedSuppliers)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                string FormName = "Supplier";
                ObjectParameter SupplierId = new ObjectParameter("SupplierID", typeof(int));
                ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                ObjectParameter AccountID = new ObjectParameter("AccountID", typeof(int));
                if (!supplierBO.IsActiveSupplier)
                {
                    FormName = "DraftSupplier";
                }
                var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, 0, GeneralBO.ApplicationID, SerialNo);
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        var i = dbEntity.SpCreateSupplier(
                            SerialNo.Value.ToString(),
                            supplierBO.Name,
                            supplierBO.SupplierCategoryID,
                            supplierBO.Currency,
                            supplierBO.GstNo,
                            supplierBO.AdhaarCardNo,
                            supplierBO.PanCardNo,
                            supplierBO.PaymentDays,
                            supplierBO.SupplierAccountsCategoryID,
                            supplierBO.SupplierTaxCategoryID,
                            supplierBO.SupplierTaxSubCategoryID,
                            supplierBO.PaymentMethodID,
                            supplierBO.PaymentGroupID,
                            null,
                            supplierBO.StartDate,
                            false,
                            supplierBO.DeactivatedDate,
                            false,
                            false,
                            false,
                            supplierBO.IsGSTRegistered,
                            supplierBO.IsCustomer,
                            supplierBO.CustomerID,
                            supplierBO.OldCode,
                            supplierBO.UanNo,
                            supplierBO.BankName,
                            supplierBO.BranchName,
                            supplierBO.AcNo,
                            supplierBO.IfscNo,
                            supplierBO.IsEmployee,
                            supplierBO.EmployeeID,
                            supplierBO.IsActiveSupplier,

                            GeneralBO.CreatedUserID,
                            DateTime.Now,
                            supplierBO.TradeLegalName,
                            GeneralBO.ApplicationID,
                            SupplierId,
                              supplierBO.CurrencyID);

                        foreach (var item in RelatedSuppliers)
                        {
                            dbEntity.SpCreateRelatedSuppliers(
                                   Convert.ToInt32(SupplierId.Value),
                                   item.RelatedSupplierID,
                                   item.RelatedSupplierLocation,
                                   GeneralBO.LocationID,
                                   GeneralBO.ApplicationID
                                );
                        }
                        dbEntity.SaveChanges();
                        string PartyType = "Supplier";

                        int PartyID = Convert.ToInt32(SupplierId.Value);

                        if (SupplierId.Value != null)
                        {

                            foreach (var item in _SupplierItemCategoryList)
                            {
                                dbEntity.SpCreateSupplierItemCategory(PartyID, item.CategoryID, GeneralBO.CreatedUserID, DateTime.Now);
                            }
                            dbEntity.SpUpdateItemCategoryInSupplier(PartyID, GeneralBO.CreatedUserID, DateTime.Now);

                            transaction.Commit();
                            foreach (var item in _AddressList)
                            {
                                dbEntity.SpCreateAddress(PartyType, PartyID, item.AddressLine1, item.AddressLine2, item.AddressLine3, 0, item.ContactPerson, item.Place, item.DistrictID, item.StateID, item.PIN, item.LandLine1, item.LandLine2, item.MobileNo, item.Fax, item.Email, item.IsBilling, item.IsShipping, item.IsDefault, item.IsDefaultShipping, GeneralBO.CreatedUserID, DateTime.Now, GeneralBO.LocationID, GeneralBO.ApplicationID);
                            }
                            using (MasterEntities dbEntity2 = new MasterEntities())
                            {
                                foreach (var item in _SupplierLocationList)
                                {
                                    dbEntity2.SpCreateSupplierLocation(PartyID, supplierBO.Name, item.LocationID);
                                }
                                dbEntity2.SpUpdateLocationInSupplier(PartyID);
                                if (supplierBO.IsActiveSupplier)
                                {
                                    dbEntity2.SpCreateAccountHeadByType(
                                    PartyType,
                                    PartyID,
                                    GeneralBO.CreatedUserID,
                                    GeneralBO.ApplicationID,
                                    GeneralBO.LocationID,
                                    GeneralBO.FinYear);
                                }
                            }


                        };
                        return PartyID;

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }

                }
            }
        }

        public List<SupplierBO> GetSupplierAutoCompleteForService(string term, int LocationID, int ApplicationID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetServiceSupplier(LocationID, ApplicationID, term, 1, 20).Select(a => new SupplierBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        StateID = a.StateID,
                        SupplierCategoryID = a.SupplierCategoryID,
                        Location = a.Location,
                        Name = a.Name,
                        IsGSTRegistered = a.IsGSTRegistered,
                        GstNo = a.GSTNo

                    }).ToList();
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public List<SupplierBO> GetSupplierAutoComplete(string term, int LocationID, int ApplicationID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetSupplier(LocationID, ApplicationID, term, 1, 20).Select(a => new SupplierBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        StateID = a.StateID,
                        SupplierCategoryID = a.SupplierCategoryID,
                        Location = a.Location,
                        Name = a.Name,
                        IsGSTRegistered = a.IsGSTRegistered,
                        PaymentDays = (int)a.CreditDays,
                        GstNo = a.GSTNo

                    }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public SupplierBO GetSupplierDetails(int SupplierID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetSupplierByID(SupplierID, GeneralBO.ApplicationID).Select(a => new SupplierBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        IsGSTRegistered = a.IsGSTRegistered,
                        GstNo = a.GSTNo,
                        AdhaarCardNo = a.AdharNo,
                        PanCardNo = a.PanCardNo,
                        PaymentDays = (int)a.CreditDays,
                        SupplierCategoryID = (int)a.SupplierCategoryID,
                        SupplierCategoryName = a.SupplierCategoryName,
                        SupplierAccountsCategoryID = (int)a.SupplierAccountsCategoryID,
                        SupplierAccountsCategoryName = a.SupplierAccountsCategoryName,
                        SupplierTaxCategoryID = (int)a.SupplierTaxCategoryID,
                        SupplierTaxCategoryName = a.SupplierTaxCategoryName,
                        SupplierTaxSubCategoryID = (int)a.SupplierTaxSubCategoryID,
                        SupplierTaxSubCategoryName = a.SupplierTaxSubCategoryName,
                        PaymentGroupID = (int)a.PaymentGroupID,
                        PaymentGroupName = a.PaymentGroupName,
                        PaymentMethodID = (int)a.PaymentMethodID,
                        PaymentMethodName = a.PaymentMethodName,
                        IsDeactivated = (bool)a.IsDeactivated,
                        IsBlockForPayment = (bool)a.IsBlockForPayment,
                        IsBlockForPurcahse = (bool)a.IsBlockForPurchase,
                        IsBlockForReceipt = (bool)a.IsBlockForReceipts,
                        CreatedDate = (DateTime)a.CreatedDate,
                        StartDate = (DateTime)a.StartDate,
                        DeactivatedDate = (DateTime)a.DeactivatedDate,
                        OldCode = a.OldSupplierCode,
                        UanNo = a.UANNo,
                        BankName = a.BankName,
                        BranchName = a.BranchName,
                        AcNo = a.BankACNo,
                        IfscNo = a.IFSCNo,
                        CustomerID = (int)a.CustomerID,
                        CustomerName = a.CustomerName,
                        EmployeeID = (int)a.EmployeeID,
                        EmployeeName = a.EmployeeName,
                        IsCustomer = (bool)a.IsCustomer,
                        IsEmployee = (bool)a.IsEmployee,
                        TradeLegalName = a.TradeLegalName,
                        IsActiveSupplier = a.IsActiveSupplier,
                        Currency = a.CurrencyName,
                        CurrencyID = a.CurrencyID

                    }).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<SupplierBO> GetAllSupplierList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetALLSupplier().Select(a => new SupplierBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DatatableResultBO GetSupplierList(string Type, string CodeHint, string NameHint, string LocationHint, string SupplierCategoryHint, string ItemCategoryHint, string OldCodeHint, string GSTRegisteredHint, string LandLine, string MobileNo, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetSuppliersList(Type, CodeHint, NameHint, LocationHint, ItemCategoryHint, SupplierCategoryHint, OldCodeHint, GSTRegisteredHint, LandLine, MobileNo, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Code = item.Code,
                                Name = item.Name,
                                Location = item.Location,
                                StateID = item.StateID,
                                IsGSTRegistered = item.IsGSTRegistered,
                                ItemCategory = item.ItemCategory,
                                SupplierCategory = item.SupplierCategory,
                                SupplierCategoryID = 0, ///TODO
                                GstNo = item.GSTNo,
                                PaymentDays = item.CreditDays,
                                OldCode = item.Oldcode,
                                IsInterCompany = item.IsInterCompany,
                                GSTRegistered = item.GSTRegistered,
                                InterCompanyLocationID = item.InterCompanyLocationID,
                                BankACNo = item.BankACNo,
                                BankName = item.BankName,
                                Status = item.Status,
                                Email = item.Email,
                                CurrencyConversionRate = item.CurrencyConversionRate,
                                CurrencyCode = item.CurrencyCode,
                                CurrencyName = item.CurrencyName,
                                CurrencyID = item.CurrencyID,
                                CurrencyPrefix = item.CurrencyPrefix,
                                DecimalPlaces = item.DecimalPlaces,
                                LandLine1 = item.LandLine1,
                                LandLine2= item.LandLine2,
                                MobileNo = item.MobileNo,

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

        public DatatableResultBO GetSupplierListForPaymentReturn(string Type, string CodeHint, string NameHint, string LocationHint, string SupplierCategoryHint, string ItemCategoryHint, string OldCodeHint, string GSTRegisteredHint, string LandLine, string MobileNo, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetSuppliersList(Type, CodeHint, NameHint, LocationHint, ItemCategoryHint, SupplierCategoryHint, OldCodeHint, GSTRegisteredHint, LandLine, MobileNo, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Code = item.Code,
                                Name = item.Name,
                                Location = item.Location,
                                StateID = item.StateID,
                                IsGSTRegistered = item.IsGSTRegistered,
                                ItemCategory = item.ItemCategory,
                                SupplierCategory = item.SupplierCategory,
                                SupplierCategoryID = 0, ///TODO
                                GstNo = item.GSTNo,
                                PaymentDays = item.CreditDays,
                                OldCode = item.Oldcode,
                                IsInterCompany = item.IsInterCompany,
                                GSTRegistered = item.GSTRegistered,
                                InterCompanyLocationID = item.InterCompanyLocationID,
                                BankACNo = item.BankACNo,
                                BankName = item.BankName,
                                Status = item.Status,
                                Email = item.Email

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

        public List<SupplierBO> GetGRNWiseSupplierForAutoComplete(string term)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetGRNWiseSupplierForAutoComplete(term, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SupplierBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        StateID = a.StateID,
                        SupplierCategoryID = a.SupplierCategoryID,
                        Location = a.Location,
                        Name = a.Name,
                        IsGSTRegistered = a.IsGSTRegistered,
                        ItemCategory = a.SupplierCategory


                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SupplierBO> getCreditorsForAutoComplete(string term)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetSupplier(GeneralBO.ApplicationID, GeneralBO.LocationID, term, 1, 20).Select(a => new SupplierBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        StateID = a.StateID,
                        SupplierCategoryID = a.SupplierCategoryID,
                        Location = a.Location,
                        Name = a.Name,
                        IsGSTRegistered = a.IsGSTRegistered,

                    }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SupplierBO> GetCreditDaysgroup()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCreditDays().Select(a => new SupplierBO
                    {
                        CreditDaysID = a.ID,
                        CreditDaysName = a.CreditDays,
                        CreditDays = (int)a.Days,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        //already in PaymentTypeDAL
        public List<SupplierBO> PaymentMethodGroup()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetPaymentType().Select(a => new SupplierBO
                    {
                        PaymentMethodID = a.ID,
                        PaymentMethodName = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<SupplierBO> PaymentGroupList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetPaymentGroup().Select(a => new SupplierBO
                    {
                        PaymentGroupID = a.ID,
                        PaymentGroupName = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public int UpdateSupplier(SupplierBO supplierBO, List<AddressBO> _AddressList, List<SupplierBO> _SupplierItemCategoryList, List<SupplierBO> _SupplierLocationList, List<RelatedSupplierBO> RelatedSuppliers)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    if (supplierBO.ID != 0)
                    {

                        dbEntity.SpUpdateSupplier(
                           supplierBO.ID,
                           supplierBO.Name,
                           supplierBO.SupplierCategoryID,
                           supplierBO.GstNo,
                           supplierBO.AdhaarCardNo,
                           supplierBO.PanCardNo,
                           supplierBO.PaymentDays,
                           supplierBO.SupplierAccountsCategoryID,
                           supplierBO.SupplierTaxCategoryID,
                           supplierBO.SupplierTaxSubCategoryID,
                           supplierBO.PaymentMethodID,
                           supplierBO.PaymentGroupID,
                           0,
                           supplierBO.StartDate,
                           supplierBO.IsDeactivated,
                           supplierBO.DeactivatedDate,
                           supplierBO.IsBlockForPurcahse,
                           supplierBO.IsBlockForReceipt,
                           supplierBO.IsBlockForPayment,
                           supplierBO.IsGSTRegistered,
                           supplierBO.IsCustomer,
                           supplierBO.CustomerID,
                           supplierBO.OldCode,
                           supplierBO.UanNo,
                           supplierBO.BankName,
                           supplierBO.BranchName,
                           supplierBO.AcNo,
                           supplierBO.IfscNo,
                           supplierBO.IsEmployee,
                           supplierBO.TradeLegalName,
                           supplierBO.EmployeeID,
                           supplierBO.IsActiveSupplier,

                           supplierBO.CurrencyID,
                           supplierBO.Currency,
                           GeneralBO.CreatedUserID,
                           GeneralBO.LocationID,
                           GeneralBO.ApplicationID,
                           GeneralBO.FinYear);

                        foreach (var item in RelatedSuppliers)
                        {
                            dbEntity.SpCreateRelatedSuppliers(
                                   supplierBO.ID,
                                   item.RelatedSupplierID,
                                   item.RelatedSupplierLocation,
                                   GeneralBO.LocationID,
                                   GeneralBO.ApplicationID
                                );
                        }

                        string PartyType = "Supplier";
                        int PartyID = Convert.ToInt32(supplierBO.ID);


                        foreach (var item in _SupplierItemCategoryList)
                        {
                            dbEntity.SpCreateSupplierItemCategory(PartyID, item.CategoryID, GeneralBO.CreatedUserID, DateTime.Now);
                        }
                        dbEntity.SpUpdateItemCategoryInSupplier(PartyID, GeneralBO.CreatedUserID, DateTime.Now);
                        foreach (var item in _AddressList)
                        {

                            if (item.AddressID == 0)
                            {
                                dbEntity.SpCreateAddress(PartyType, PartyID, item.AddressLine1, item.AddressLine2, item.AddressLine3, 0, item.ContactPerson, item.Place, item.DistrictID, item.StateID, item.PIN, item.LandLine1, item.LandLine2, item.MobileNo, item.Fax, item.Email, item.IsBilling, item.IsShipping, item.IsDefault, item.IsDefaultShipping, GeneralBO.CreatedUserID, DateTime.Now, GeneralBO.LocationID, GeneralBO.ApplicationID);
                            }
                            else
                            {
                                dbEntity.SpUpdateAddress(item.AddressID, PartyType, PartyID, item.AddressLine1, item.AddressLine2, item.AddressLine3, item.ContactPerson, item.Place, item.DistrictID, item.StateID, item.PIN, item.LandLine1, item.LandLine2, item.MobileNo, item.Fax, item.Email, item.IsBilling, item.IsShipping, item.IsDefault, item.IsDefaultShipping);
                            }

                        }
                        foreach (var item in _SupplierLocationList)
                        {
                            dbEntity.SpCreateSupplierLocation(PartyID, supplierBO.Name, item.LocationID);
                        }
                        dbEntity.SpUpdateLocationInSupplier(PartyID);
                        dbEntity.SpUpdateAccountHeadByType(
                            PartyType,
                            PartyID,
                            supplierBO.Name,
                            GeneralBO.CreatedUserID,
                            GeneralBO.ApplicationID,
                            GeneralBO.LocationID,
                            GeneralBO.FinYear);
                    };
                    return supplierBO.ID;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string CheckSupplierAlradyExist(int ID, string Name, string GstNo, string PanCardNo, string AdhaarCardNo, string Mobile, string LandLine1, string landline2, string AcNo)
        {
            try
            {

                ObjectParameter message = new ObjectParameter("RetVal", typeof(string));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpCheckIsSupplierDetailsExit(ID, Name, GstNo, PanCardNo, AdhaarCardNo, Mobile, LandLine1, landline2, AcNo, message);
                }
                return message.Value.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SupplierBO> GetAllSupplierAutoComplete(string Term)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetSuppliersAutoComplete("All", Term, GeneralBO.FinYear, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new SupplierBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        StateID = a.StateID,
                        SupplierCategoryID = a.SupplierCategoryID,
                        Location = a.Location,
                        Name = a.Name,
                        IsGSTRegistered = a.IsGSTRegistered,

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SupplierBO> InterCompanySupplier(string Term)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetInterCompanyList(Term, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SupplierBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        StateID = a.StateID,
                        Name = a.Name,
                        IsGSTRegistered = a.IsGSTRegistered,
                        CustomerID = (int)a.CustomerID,
                        SupplierCategoryName = a.SupplierCategory,
                        Location = a.BillingPlace,
                        ItemCategory = a.ItemCategory,
                        LocationID = a.LocationID
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SupplierBO> InterCompanySupplierListForLocation(string Term)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetInterCompanySuppliers(Term, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SupplierBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        StateID = a.StateID,
                        Name = a.Name,
                        IsGSTRegistered = a.IsGSTRegistered,
                        CustomerID = (int)a.CustomerID,
                        SupplierCategoryName = a.SupplierCategory,
                        Location = a.BillingPlace,
                        ItemCategory = a.ItemCategory,
                        LocationID = a.LocationID
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool IsInterCompanySupplier(int SupplierID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ObjectParameter IsInterCompanySupplier = new ObjectParameter("IsInterCompanySupplier", typeof(bool));
                    dbEntity.SpIsInterCompanySupplier(SupplierID, GeneralBO.ApplicationID, IsInterCompanySupplier);
                    return Convert.ToBoolean(IsInterCompanySupplier.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetCustomerID(int SupplierID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ObjectParameter CustomerID = new ObjectParameter("CustomerID", typeof(int));
                    dbEntity.SpGetCustomerIDOfSupplier(SupplierID, GeneralBO.ApplicationID, CustomerID);
                    return Convert.ToInt32(CustomerID.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetCustomerID()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ObjectParameter CustomerID = new ObjectParameter("CustomerID", typeof(int));
                    dbEntity.SpGetCustomerID(GeneralBO.LocationID, GeneralBO.ApplicationID, CustomerID);
                    return Convert.ToInt32(CustomerID.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetSupplierLocationID(int SupplierID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ObjectParameter LocationID = new ObjectParameter("LocationID", typeof(int));
                    dbEntity.SpGetSupplierLocationID(SupplierID, GeneralBO.ApplicationID, LocationID);
                    return Convert.ToInt32(LocationID.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int DeleteSupplier(int id)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpDeleteSupplier(id, GeneralBO.ApplicationID);
                    return 1;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SupplierBO> GetDoctorList()
        {
            List<SupplierBO> doctor = new List<SupplierBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    doctor = dbEntity.SpGetDoctor(GeneralBO.LocationID).Select(a => new SupplierBO
                    {
                        ID = a.ID,
                        Name = a.Name

                    }).ToList();
                }
                return doctor;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<RelatedSupplierBO> GetRelatedSupplier(int SupplierID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetRelatedSuppliers(SupplierID, GeneralBO.ApplicationID).Select(a => new RelatedSupplierBO
                    {
                        RelatedSupplierID = (int)a.RelatedSupplierID,
                        RelatedSupplierLocation = a.Location,
                        RelatedSupplierName = a.RelatedSupplierName

                    }).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SupplierDescriptionBO> GetDescription(int SupplierID, string Type)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {

                    return dbEntity.SpGetSupplierDescription(Type, SupplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SupplierDescriptionBO
                    {

                        Name = a.Name,
                        Key = a.Keys,
                        Value = a.Value

                    }).ToList();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }


    }
}
