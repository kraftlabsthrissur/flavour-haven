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
    public class CustomerDAL
    {
        public DatatableResultBO GetCustomerList(string Type, int CustomerCategoryID, int StateID, string CodeHint, string NameHint, string LocationHint, string CustomerCategoryHint, string CurrencyNameHint, string LandLineHint, string MobileHit, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetCustomerList(Type, CustomerCategoryID, StateID, CodeHint, NameHint, LocationHint, CustomerCategoryHint, CurrencyNameHint, LandLineHint, MobileHit, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result.Skip(i).Take(1).FirstOrDefault();
                            obj = new
                            {
                                item.ID,
                                item.Code,
                                Name = item.Name.Trim(),
                                item.Location,
                                item.StateID,
                                item.IsGSTRegistered,
                                item.PriceListID,
                                item.CustomerCategory,
                                item.Address,
                                item.DistrictID,
                                item.CountryID,
                                item.CustomerCategoryID,
                                item.SchemeID,
                                LandLine1 = item.LandLine1 ?? "",
                                LandLine2 = item.LandLine2 ?? "",
                                item.MobileNo,
                                MaxCreditLimit = (decimal)item.MaxCreditLimit,
                                MinimumCreditLimit = (decimal)item.MinimumCreditLimit,
                                item.CashDiscountPercentage,
                                item.IsBlockedForChequeReceipt,
                                OutStandingAmount = item.OutstandingAmount,
                                item.CurrencyName,
                                item.CurrencyCode,
                                item.CurrencyConversionRate,
                                item.CurrencyPrefix,
                                item.DecimalPlaces,
                                item.CurrencyID
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

        public DatatableResultBO GetCustomerMainList(string Type, int CustomerCategoryID, string CodeHint, string NameHint, string LocationHint, string CustomerCategoryHint, string PropratorNameHint, string OldCodeHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetAllCustomerList(Type, CustomerCategoryID, CodeHint, NameHint, LocationHint, CustomerCategoryHint, PropratorNameHint, OldCodeHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Name = (item.Name).Trim(),
                                Location = item.Location,
                                StateID = item.StateID,
                                IsGSTRegistered = item.IsGSTRegistered,
                                PriceListID = item.PriceListID,
                                CustomerCategory = item.CustomerCategory,
                                Address = item.Address,
                                DistrictID = item.DistrictID,
                                CountryID = item.CountryID,
                                CustomerCategoryID = item.CustomerCategoryID,
                                SchemeID = item.SchemeID,
                                PropratorName = item.PropratorName,
                                OldCode = item.OldCode,
                                Status = (bool)item.IsDraftCustomer ? "draft" : ""
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

        public DatatableResultBO GetPartyList(string NameHint, string DoctorNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetPartyList(NameHint, DoctorNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Doctor = item.DoctorName,
                                Name = item.PartyName,

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

        public List<CustomerBO> GetCustomerAutoComplete(string Hint, int CustomerCategoryID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCustomers(Hint, CustomerCategoryID, 1, 20, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new CustomerBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Location = a.Location,
                        Name = (a.Name).Trim(),
                        StateID = (int)a.StateID,
                        IsGSTRegistered = (bool)a.IsGSTRegistered,
                        PriceListID = (int)a.PriceListID,
                        CustomerCategory = a.CustomerCategory
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int CreateCustomer(CustomerBO customerBO, List<AddressBO> AddressCreateList, List<CustomerBO> CustomerLocationList)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    string FormName = "Customer";
                    string Type = "Customer";
                    ObjectParameter CustomerID = new ObjectParameter("CustomerID", typeof(int));
                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                    // ObjectParameter AccountHeadID = new ObjectParameter("AccountHeadID", typeof(int));
                    if (customerBO.IsDraft)
                    {
                        FormName = "DraftCustomer";
                    }

                    var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                    using (var transaction = dbEntity.Database.BeginTransaction())
                    {
                        try
                        {
                            var i = dbEntity.SpCreateCustomer(
                                SerialNo.Value.ToString(),
                                customerBO.Name,
                                customerBO.Name2,
                                GeneralBO.LocationID,
                                customerBO.OldCode,
                                null,
                                customerBO.CategoryID,
                                customerBO.Currency,
                                customerBO.ContactPersonName,
                                customerBO.PriceListID,
                                customerBO.DiscountID,
                                customerBO.CashDiscountID,
                                customerBO.CustomerTaxCategoryID,
                                customerBO.CustomerAccountsCategoryID,
                                customerBO.IsGSTRegistered,
                                customerBO.AadhaarNo,
                                customerBO.PanNo,
                                customerBO.GstNo,
                                customerBO.FaxNo,
                                customerBO.EmailID,
                                customerBO.CreditDays,
                                customerBO.MinCreditLimit,
                                customerBO.MaxCreditLimit,
                                customerBO.PaymentTypeID,
                                null,
                                customerBO.StartDate,
                                customerBO.ExpiryDate,
                                customerBO.IsInterCompany,
                                customerBO.IsMappedtoExpsEntries,
                                customerBO.IsBlockedForSalesOrders,
                                customerBO.IsBlockedForSalesInvoices,
                                customerBO.IsAlsoASupplier,
                                customerBO.IsMappedToServiceSales,
                                customerBO.SupplierID, 0,
                                0,
                                customerBO.FSOID,

                                GeneralBO.ApplicationID,
                                customerBO.CustomerMonthlyTarget,
                                customerBO.TradeLegalName,
                                customerBO.IsDraft,
                                CustomerID,
                                customerBO.CurrencyID
                               );
                            dbEntity.SaveChanges();
                            transaction.Commit();
                            String PartyType = "Customer";
                            int PartyID = Convert.ToInt32(CustomerID.Value);
                            if (CustomerID.Value != null)
                            {
                                foreach (var item in AddressCreateList)
                                {
                                    dbEntity.SpCreateAddress(PartyType, PartyID, item.AddressLine1, item.AddressLine2, item.AddressLine3, 0, item.ContactPerson, item.Place, item.DistrictID, item.StateID, item.PIN, item.LandLine1, item.LandLine2, item.MobileNo, item.Fax, item.Email, item.IsBilling, item.IsShipping, item.IsDefault, item.IsDefaultShipping, GeneralBO.CreatedUserID, DateTime.Now, GeneralBO.LocationID, GeneralBO.ApplicationID);
                                }
                                using (MasterEntities dbEntity2 = new MasterEntities())
                                {
                                    foreach (var item in CustomerLocationList)
                                    {
                                        dbEntity2.SpCreateCustomerLocationMapping(PartyID, customerBO.Name, item.LocationID);
                                    }
                                    dbEntity2.SpUpdateLocationMappingInCustomer(PartyID);
                                    dbEntity2.SpFSOIncentiveMapping(PartyID, customerBO.FSOID, 0, customerBO.StartDate);
                                    dbEntity2.SpUpdateSchemeForCustomer(PartyID);

                                    if (!customerBO.IsDraft)
                                    {
                                        dbEntity2.SpCreateAccountHeadByType(
                                        Type,
                                        PartyID,
                                        GeneralBO.CreatedUserID,
                                        GeneralBO.ApplicationID,
                                        GeneralBO.LocationID,
                                        GeneralBO.FinYear);
                                    }
                                }

                            }
                            return (Convert.ToInt32(CustomerID.Value));

                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            throw e;
                        }
                    }
                }
            }


            catch (Exception e)
            {
                throw e;
            }
        }

        public int UpdateCustomer(CustomerBO customerBO, List<AddressBO> AddressCreateList, List<CustomerBO> CustomerLocationList)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    if (customerBO.ID != 0)
                    {

                        dbEntity.SpUpdateCustomer(customerBO.ID, customerBO.Code, customerBO.Name, customerBO.Name2, customerBO.CategoryID, customerBO.ContactPersonName, customerBO.PriceListID, customerBO.DiscountID, customerBO.CashDiscountID, customerBO.CustomerTaxCategoryID, customerBO.CustomerAccountsCategoryID, customerBO.IsGSTRegistered, customerBO.AadhaarNo, customerBO.PanNo, customerBO.GstNo, customerBO.FaxNo,
                          customerBO.EmailID, customerBO.CreditDays, customerBO.MinCreditLimit, customerBO.MaxCreditLimit, customerBO.PaymentTypeID, customerBO.StartDate, customerBO.ExpiryDate
                          , customerBO.IsInterCompany, customerBO.IsMappedtoExpsEntries, customerBO.IsBlockedForSalesOrders, customerBO.IsBlockedForSalesInvoices, customerBO.IsAlsoASupplier,
                          customerBO.SupplierID, 0, 0, customerBO.FSOID, customerBO.OldCode, customerBO.CustomerMonthlyTarget, customerBO.TradeLegalName, customerBO.IsMappedToServiceSales, customerBO.IsDraft, customerBO.CurrencyID,
                           customerBO.Currency, GeneralBO.LocationID, GeneralBO.ApplicationID, GeneralBO.FinYear, GeneralBO.CreatedUserID);

                    };
                    String PartyType = "Customer";
                    int PartyID = customerBO.ID;
                    if (customerBO.ID != 0)
                    {
                        foreach (var item in AddressCreateList)
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
                        foreach (var item in CustomerLocationList)
                        {
                            dbEntity.SpCreateCustomerLocationMapping(PartyID, customerBO.Name, item.LocationID);
                        }
                        dbEntity.SpUpdateLocationMappingInCustomer(PartyID);

                        dbEntity.SpFSOIncentiveMapping(PartyID, customerBO.FSOID, 0, customerBO.StartDate);
                        dbEntity.SpUpdateSchemeForCustomer(PartyID);
                        dbEntity.SpUpdateAccountHeadByType(
                            PartyType,
                            PartyID,
                            customerBO.Name,
                            GeneralBO.CreatedUserID,
                            GeneralBO.ApplicationID,
                            GeneralBO.LocationID,
                            GeneralBO.FinYear);
                    }
                    return customerBO.ID;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int EditCustomer(CustomerBO customerBO)
        {
            try
            {

                using (MasterEntities dbEntity = new MasterEntities())
                {
                    //return dbEntity.SpCreateSupplier(supplierBO.Code,
                    //    supplierBO.Name,
                    //    supplierBO.IsGSTRegistered);
                    return 0;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CategoryBO> GetCustomerCategories()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCustomerCategory().Select(a => new CategoryBO
                    {
                        ID = a.ID,
                        Name = a.CustomerCategory,
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CustomerBO> GetPriceList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.PriceLists.Select(a => new CustomerBO
                    {
                        PriceListID = a.ID,
                        PriceListName = a.Name,
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CustomerBO> GetDiscountList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetDiscountListForDropdown().Select(a => new CustomerBO
                    {
                        DiscountCategoryID = a.ID,
                        DiscountCategory = a.DiscountCategory,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CustomerBO> GetCashDiscountList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCashDiscountList().Select(a => new CustomerBO
                    {
                        DiscountCategoryID = a.ID,
                        DiscountCategory = a.DiscountCategory,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CustomerBO GetCustomerDetails(int CustomerID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetCustomerByID(CustomerID, GeneralBO.ApplicationID).Select(a => new CustomerBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Name2 = a.Name2,
                        OldCode = a.OldCode,
                        CategoryID = (int)a.CategoryID,
                        CategoryName = a.CustomerCategoryName,
                        Currency = a.CurrencyName,
                        ContactPersonName = a.ContactPersonName,
                        PriceListID = (int)a.PriceListID,
                        PriceListName = a.PriceListName,
                        DiscountID = (int)a.DiscountID,
                        CashDiscountID = (int)a.CashDiscountID,
                        CashDiscountPercentage = (decimal)a.CashDiscountPercent,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                        CustomerTaxCategoryID = (int)a.CustomerTaxCategoryID,
                        CustomerTaxCategory = a.CustomerTaxCategory,
                        CustomerAccountsCategoryID = (int)a.CustomerAccountsCategoryID,
                        CustomerAccountsCategoryName = a.CustomerAccountCategory,
                        PaymentTypeName = a.PaymentTypeName,
                        IsGSTRegistered = (bool)a.IsGSTRegistered,
                        AadhaarNo = a.AadhaarNo,
                        PanNo = a.PANNo,
                        GstNo = a.GSTNo,
                        FaxNo = a.FaxNo,
                        EmailID = a.EmailID,
                        CreditDays = (int)a.Creditdays,
                        MinCreditLimit = (decimal)a.MinimumCreditLimit,
                        MaxCreditLimit = (decimal)a.MaxCreditLimit,
                        PaymentTypeID = (int)a.PaymentTypeID,
                        StartDate = (DateTime)a.StartDate,
                        ExpiryDate = (DateTime)a.ExpiryDate,
                        IsInterCompany = (bool)a.IsInterCompany,
                        IsMappedtoExpsEntries = (bool)a.IsMappedtoExpsEntries,
                        IsBlockedForSalesOrders = (bool)a.IsBlockedForSalesOrders,
                        IsBlockedForSalesInvoices = (bool)a.IsBlockedForSalesInvoices,
                        IsAlsoASupplier = (bool)a.IsAlsoASupplier,
                        CustomerRouteID = (int)a.CustomerRouteID,
                        CashDiscountCategoryID = (int)a.CashDiscountCategoryID,
                        Color = a.Color,
                        OutstandingAmount = a.OutstandingAmount,
                        FSOName = a.FSO,
                        FSOID = (int)a.FSOID,
                        CustomerMonthlyTarget = (decimal)a.CustomerMonthlyTarget,
                        TradeLegalName = a.TradeLegalName,
                        IsMappedToServiceSales = (bool)a.IsMappedtoServiceSales,
                        SupplierID = (int)a.SupplierID,
                        SupplierName = a.Supplier,
                        IsBlockedForChequeReceipt = (bool)a.IsBlockedForChequeReceipt,
                        IsDraft = (bool)a.IsDraftCustomer,
                        CurrencyName = a.CurrencyName,
                        CurrencyID = a.CurrencyID,
                        CurrencyCode = a.CurrencyCode

                    }).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CustomerBO GetCustomer(int CustomerID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetCustomer(CustomerID, GeneralBO.ApplicationID).Select(a => new CustomerBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        CategoryID = (int)a.CategoryID,
                        PriceListID = (int)a.PriceListID,
                        IsGSTRegistered = (bool)a.IsGSTRegistered,
                        IsInterCompany = (bool)a.IsInterCompany,
                        StateID = (int)a.StateID,
                        SchemeID = (int)a.SchemeID
                    }).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CustomerBO> GetCustomersByOldCodes(string Codes)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCustomersByCodes(Codes, GeneralBO.ApplicationID).Select(a => new CustomerBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        OldCode = a.OldCode,
                        Code = a.Code
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool IsInterCompanyCustomer(int CustomerID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ObjectParameter IsInterCompanyCustomer = new ObjectParameter("IsInterCompanyCustomer", typeof(bool));
                    dbEntity.SpIsInterCompanyCustomer(CustomerID, GeneralBO.ApplicationID, IsInterCompanyCustomer);
                    return Convert.ToBoolean(IsInterCompanyCustomer.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool HasOutstandingAmount(int CustomerID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    ObjectParameter HasOutstandingAmount = new ObjectParameter("HasOutstandingAmount", typeof(bool));
                    dbEntity.SpHasCustomerOutstandingAmount(CustomerID, GeneralBO.ApplicationID, HasOutstandingAmount);
                    return Convert.ToBoolean(HasOutstandingAmount.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public decimal GetTurnOverDiscount(int CustomerID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    ObjectParameter Amount = new ObjectParameter("Amount", 11111.0100);
                    dbEntity.SpGetTurnOverDiscount(CustomerID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, Amount);
                    return Convert.ToDecimal(Amount.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public decimal CashDiscountPercentage(int CustomerID)
        {
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    ObjectParameter CashDiscountPercentage = new ObjectParameter("CashDiscountPercentage", 11.01);
                    //dbEntity.SpCashDiscountPercentage(CustomerID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, CashDiscountPercentage);
                    return Convert.ToDecimal(CashDiscountPercentage.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public BatchTypeBO GetBatchTypeID(int CustomerID)
        {
            // int GetBatchTypeID = 0;
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    ObjectParameter BatchTypeID = new ObjectParameter("BatchTypeID", typeof(int));
                    ObjectParameter BatchType = new ObjectParameter("BatchType", typeof(string));
                    dbEntity.SpGetBatchTypeForCustomer(CustomerID, GeneralBO.ApplicationID, BatchTypeID, BatchType);
                    return new BatchTypeBO() { ID = Convert.ToInt16(BatchTypeID.Value), Name = BatchType.Value.ToString() };

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public DatatableResultBO GetCustomerDetails(string CodeHint, string CustomerNameHint, string CategoryHint, string LocationHint, string CustomerSchemeHint, string DiscountPercentageHint, string PriceListHint, string MinCreditLimitHint, string MaxCreditLimitHint, string OutStandingAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetCustomerDetails(CodeHint, CustomerNameHint, CategoryHint, LocationHint, CustomerSchemeHint, DiscountPercentageHint, PriceListHint, MinCreditLimitHint, MaxCreditLimitHint, OutStandingAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = (int)item.ID,
                                Code = item.Code,
                                CustomerName = item.CustomerName,
                                CategoryName = item.CategoryName,
                                Scheme = item.Scheme,
                                DiscountPercentage = item.DiscountPercentage,
                                PriceList = item.PriceList,
                                MinimumCreditLimit = item.MinimumCreditLimit,
                                MaxCreditLimit = item.MaxCreditLimit,
                                OutStandingAmount = item.OutStandingAmount,
                                Location = item.LocationName
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

        public DatatableResultBO GetCustomerItemDetails(int CustomerID, string CodeHint, string ItemNameHint, string MRPHint, string DiscountPercentageHint, string QuantityHint, string OfferQuantityHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetCustomerItemDetails(CustomerID, CodeHint, ItemNameHint, MRPHint, DiscountPercentageHint, QuantityHint, OfferQuantityHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ItemID = (int)item.ID,
                                Code = item.Code,
                                ItemName = item.ItemName,
                                MRP = item.MRP,
                                DiscountPercentage = item.DiscountPercentage,
                                InvoiceQty = item.InvoiceQty,
                                OfferQty = item.OfferQty
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

        public string CheckCustomerAlradyExist(int ID, string Name, string GstNo, string PanCardNo, string AdhaarCardNo, string Mobile, string LandLine1, string landline2)
        {
            try
            {

                ObjectParameter message = new ObjectParameter("RetVal", typeof(string));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpCheckIsCustomerDetailsExit(ID, Name, GstNo, PanCardNo, AdhaarCardNo, Mobile, LandLine1, landline2, message);

                }
                return message.Value.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetServiceCustomerList(string Type, int CustomerCategoryID, int StateID, string CodeHint, string NameHint, string LocationHint, string CustomerCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetServiceCustomerList(Type, CustomerCategoryID, StateID, CodeHint, NameHint, LocationHint, CustomerCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Name = (item.Name).Trim(),
                                Location = item.Location,
                                StateID = item.StateID,
                                IsGSTRegistered = item.IsGSTRegistered,
                                PriceListID = item.PriceListID,
                                CustomerCategory = item.CustomerCategory,
                                Address = item.Address,
                                DistrictID = item.DistrictID,
                                CountryID = item.CountryID,
                                CustomerCategoryID = item.CustomerCategoryID,
                                SchemeID = item.SchemeID,
                                MaxCreditLimit = (decimal)item.MaxCreditLimit,
                                MinimumCreditLimit = (decimal)item.MinimumCreditLimit,
                                CashDiscountPercentage = item.CashDiscountPercentage

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

        public int DeleteCustomer(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpDeleteCustomer(ID, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
