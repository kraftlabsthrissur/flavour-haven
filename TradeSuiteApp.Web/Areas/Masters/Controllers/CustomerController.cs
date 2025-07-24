using BusinessLayer;
using BusinessObject;
using DataAccessLayer;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class CustomerController : Controller
    {
        private ICustomerContract customerBL;
        private IDistrictContract districtBL;
        private IStateContract stateBL;
        private IAddressContract addressBL;
        private ICategoryContract categoryBL;
        private ISupplierContract supplierBL;
        private IPaymentTypeContract paymentTypeBL;
        private IGeneralContract generalBL;
        private ILocationContract locationBL;
        private ICurrencyContract currencyBL;

        public CustomerController()
        {
            generalBL = new GeneralBL();
            customerBL = new CustomerBL();
            stateBL = new StateBL();
            districtBL = new DistrictBL();
            addressBL = new AddressBL();
            categoryBL = new CategoryBL();
            supplierBL = new SupplierBL();
            paymentTypeBL = new PaymentTypeBL();
            locationBL = new LocationBL();
            currencyBL = new CurrencyBL();
        }

        // GET: Masters/Customer
        public ActionResult Index()
        {
            return View();
        }

        // GET: Masters/Customer/Details/5
        public ActionResult Details(int id)
        {
            var obj = customerBL.GetCustomerDetails(id);
            CustomerModel customerModel = new CustomerModel();
            customerModel.ID = obj.ID;
            customerModel.Code = obj.Code;
            customerModel.Name = obj.Name;
            customerModel.Name2 = obj.Name2;
            customerModel.OldCode = obj.OldCode;
            customerModel.OldName = obj.OldName;
            customerModel.CategoryID = obj.CategoryID;
            customerModel.CategoryName = obj.CategoryName;
            customerModel.Currency = obj.Currency;
            customerModel.ContactPersonName = obj.ContactPersonName;
            customerModel.PriceListID = obj.PriceListID;
            customerModel.PriceListName = obj.PriceListName;
            customerModel.DiscountPercentage = obj.DiscountPercentage;
            customerModel.CashDiscountPercentage = obj.CashDiscountPercentage;
            customerModel.CustomerTaxCategory = obj.CustomerTaxCategory;
            customerModel.CustomerAccountsCategoryID = obj.CustomerAccountsCategoryID;
            customerModel.CustomerAccountsCategoryName = obj.CustomerAccountsCategoryName;
            customerModel.PaymentTypeName = obj.PaymentTypeName;
            customerModel.IsGSTRegistered = obj.IsGSTRegistered;
            customerModel.AadhaarNo = obj.AadhaarNo;
            customerModel.PanNo = obj.PanNo;
            customerModel.GstNo = obj.GstNo;
            customerModel.FaxNo = obj.FaxNo;
            customerModel.EmailID = obj.EmailID;
            customerModel.CreditDays = obj.CreditDays;
            customerModel.MinCreditLimit = obj.MinCreditLimit;
            customerModel.MaxCreditLimit = obj.MaxCreditLimit;
            customerModel.StartDate = General.FormatDate(obj.StartDate);
            customerModel.ExpiryDate = General.FormatDate(obj.ExpiryDate);
            customerModel.IsInterCompany = obj.IsInterCompany;
            customerModel.IsMappedtoExpsEntries = obj.IsMappedtoExpsEntries;
            customerModel.IsBlockedForSalesOrders = obj.IsBlockedForSalesOrders;
            customerModel.IsBlockedForSalesInvoices = obj.IsBlockedForSalesInvoices;
            customerModel.IsAlsoASupplier = obj.IsAlsoASupplier;
            customerModel.SupplierID = obj.SupplierID;
            customerModel.SupplierName = obj.SupplierName;
            customerModel.CustomerRouteID = obj.CustomerRouteID;
            customerModel.CashDiscountCategoryID = obj.CashDiscountCategoryID;
            customerModel.FSOName = obj.FSOName;
            customerModel.CustomerMonthlyTarget = obj.CustomerMonthlyTarget;
            customerModel.TradeLegalName = obj.TradeLegalName;
            customerModel.IsMappedToServiceSales = obj.IsMappedToServiceSales;
            customerModel.IsDraft = obj.IsDraft;
            customerModel.Currency = obj.Currency;
            customerModel.CurrencyID = obj.CurrencyID;
            customerModel.AddressList = addressBL.GetAddressByPartyType(id, "Customer").Select(a => new CustomerAddressModel()
            {
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                AddressLine3 = a.AddressLine3,
                Place = a.Place,
                PIN = a.PIN,
                ContactPerson = a.ContactPerson,
                LandLine1 = a.LandLine1,
                LandLine2 = a.LandLine2,
                MobileNo = a.MobileNo,
                Fax = a.Fax,
                Email = a.Email,
                District = a.District,
                State = a.State,
                IsBilling = a.IsBilling,
                IsShipping = a.IsShipping,
                IsDefault = (bool)a.IsDefault,
                IsDefaultShipping = (bool)a.IsDefaultShipping
            }).ToList();
            customerModel.LocationList = new SelectList(locationBL.GetCustomerLocationMappingByCustomerID(customerModel.ID), "LocationID", "LocationName");
            return View(customerModel);
        }

        public JsonResult GetAddressList(int CustomerID)
        {
            List<AddressModel> AddressList = addressBL.GetAddressByPartyType(CustomerID, "Customer").Select(a => new AddressModel()
            {
                AddressID = a.AddressID,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                AddressLine3 = a.AddressLine3,
                Place = a.Place,
                PIN = a.PIN,
                ContactPerson = a.ContactPerson,
                LandLine1 = a.LandLine1,
                LandLine2 = a.LandLine2,
                MobileNo = a.MobileNo,
                Fax = a.Fax,
                Email = a.Email,
                District = a.District,
                DistrictID = a.DistrictID,
                StateID = a.StateID,
                State = a.State,
                IsBilling = a.IsBilling,
                IsShipping = a.IsShipping,
                IsDefault = (bool)a.IsDefault,
                IsDefaultShipping = (bool)a.IsDefaultShipping
            }).ToList();
            return Json(AddressList, JsonRequestBehavior.AllowGet);
        }

        // GET: Masters/Customer/Create
        public ActionResult Create()
        {
            CustomerModel customerModel = new CustomerModel();

            customerModel.Code = generalBL.GetSerialNo("Customer", "Code");
            customerModel.CurrencyLists = new SelectList(currencyBL.GetCurrencyList(), "ID", "Name");
            customerModel.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            customerModel.CategoryList = new SelectList(categoryBL.GetCustomerCategoryList(), "ID", "Name");
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            if (currency != null)
            {
                customerModel.CurrencyID = currency.CurrencyID;
            }
            customerModel.CategoryID = 1;
            customerModel.PriceList = new SelectList(customerBL.GetPriceList(), "PriceListID", "PriceListName");
            customerModel.PriceListID = 1;
            customerModel.DiscountList = new SelectList(customerBL.GetDiscountList(), "DiscountCategoryID", "DiscountCategory");
            customerModel.DiscountID = 1;
            customerModel.CashDiscountList = new SelectList(customerBL.GetCashDiscountList(), "DiscountCategoryID", "DiscountCategory");
            customerModel.CustomerTaxCategoryList = new SelectList(categoryBL.GetCustomerTaxCategoryList(), "ID", "Name");
            customerModel.CustomerAccountsCategoryList = new SelectList(categoryBL.GetCustomerAccountsCategoryList(), "ID", "Name");
            customerModel.CustomerAccountsCategoryID = 2;
            customerModel.CreditDaysList = new SelectList(supplierBL.GetCreditDaysgroup(), "CreditDays", "CreditDaysName");
            customerModel.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");
            customerModel.PaymentTypeID = 1;
            customerModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            customerModel.StartDate = General.FormatDate(DateTime.Now);
            customerModel.ExpiryDate = General.FormatDate(Convert.ToDateTime(generalBL.GetConfig("DefaultDate")));
            customerModel.MinCreditLimit = 0.01M;
            customerModel.MaxCreditLimit = 1000000;
            customerModel.LocationID = GeneralBO.LocationID;
            return View(customerModel);
        }

        public JsonResult GetTurnOverDiscount(int CustomerID)
        {
            try
            {
                decimal TurnOverDiscount = customerBL.GetTurnOverDiscount(CustomerID);
                return Json(new { Status = "success", TurnOverDiscount = TurnOverDiscount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "GetTurnOverDiscount", CustomerID, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Masters/Customer/Save
        [HttpPost]
        public ActionResult Save(CustomerModel model)
        {
            //if (ModelState.IsValid)
            //{
            try
            {
                CustomerBO customerBO = new CustomerBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    Name2 = model.Name2,
                    OldCode = model.OldCode,
                    Currency = model.Currency,
                    AadhaarNo = model.AadhaarNo,
                    PanNo = model.PanNo,
                    EmailID = model.EmailID,
                    FaxNo = model.FaxNo,
                    SupplierID = model.SupplierID,
                    CreditDays = model.CreditDays,
                    StartDate = General.ToDateTime(model.StartDate),
                    ExpiryDate = General.ToDateTime(model.ExpiryDate),
                    ContactPersonName = model.ContactPersonName,
                    CategoryID = model.CategoryID,
                    CustomerAccountsCategoryID = model.CustomerAccountsCategoryID,
                    PaymentTypeID = model.PaymentTypeID,
                    CustomerTaxCategoryID = model.CustomerTaxCategoryID,
                    PriceListID = model.PriceListID,
                    DiscountID = model.DiscountID,
                    CashDiscountID = model.CashDiscountID,
                    MinCreditLimit = model.MinCreditLimit,
                    MaxCreditLimit = model.MaxCreditLimit,
                    IsInterCompany = model.IsInterCompany,
                    IsMappedtoExpsEntries = model.IsMappedtoExpsEntries,
                    IsBlockedForSalesOrders = model.IsBlockedForSalesOrders,
                    IsBlockedForSalesInvoices = model.IsBlockedForSalesInvoices,
                    IsAlsoASupplier = model.IsAlsoASupplier,
                    IsGSTRegistered = model.IsGSTRegistered,
                    GstNo = model.GstNo,
                    FSOID = model.FSOID,
                    CustomerMonthlyTarget = model.CustomerMonthlyTarget,
                    TradeLegalName = model.TradeLegalName,
                    IsMappedToServiceSales = model.IsMappedToServiceSales,
                    IsDraft = model.IsDraft,
                    CurrencyID = model.CurrencyID
                };

                List<AddressBO> AddressCreateList = new List<AddressBO>();
                //List<AddressBO> AddressUpadatedList = new List<AddressBO>();

                AddressBO AddressItem;
                if (model.AddressList != null && model.AddressList.Count() > 0)
                {
                    foreach (var item in model.AddressList)
                    {
                        AddressItem = new AddressBO()
                        {
                            AddressID = item.AddressID,
                            AddressLine1 = item.AddressLine1,
                            AddressLine2 = item.AddressLine2,
                            AddressLine3 = item.AddressLine3,
                            Place = item.Place,
                            ContactPerson = item.ContactPerson,
                            LandLine1 = item.LandLine1,
                            LandLine2 = item.LandLine2,
                            MobileNo = item.MobileNo,
                            StateID = item.StateID,
                            PIN = item.PIN,
                            Fax = item.Fax,
                            DistrictID = item.DistrictID,
                            Email = item.Email,
                            IsBilling = item.IsBilling,
                            IsShipping = item.IsShipping,
                            IsDefault = item.IsDefault,
                            IsDefaultShipping = item.IsDefaultShipping,
                        };
                        AddressCreateList.Add(AddressItem);
                    }
                }
                else
                {
                    var addressItem = new AddressBO()
                    {
                        AddressID = 0,
                        AddressLine1 = "Default",
                        AddressLine2 = "Default",
                        AddressLine3 = "",

                        Place = "Default",
                        ContactPerson = "",
                        LandLine1 = "",
                        LandLine2 = "",

                        MobileNo = "Default",
                        StateID = 0,
                        PIN = "Default",
                        Fax = "",
                        DistrictID = 0,
                        Email = "",
                        IsBilling = true,
                        IsShipping = true,
                        IsDefault = true,
                        IsDefaultShipping = true,
                    };
                    AddressCreateList.Add(addressItem);


                }
                List<CustomerBO> CustomerLocationList = new List<CustomerBO>();
                CustomerBO CustomerLocationMapping;
                if (model.CustomerLocationList == null)
                {
                    CustomerLocationMapping = new CustomerBO()
                    {
                        LocationID = GeneralBO.LocationID
                    };
                    CustomerLocationList.Add(CustomerLocationMapping);
                }
                else
                {
                    foreach (var item in model.CustomerLocationList)
                    {
                        CustomerLocationMapping = new CustomerBO()
                        {
                            LocationID = item.LocationID
                        };
                        CustomerLocationList.Add(CustomerLocationMapping);
                    }
                }
                int CustomerID = model.ID;
                CustomerID = customerBL.CreateCustomer(customerBO, AddressCreateList, CustomerLocationList);
                return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "Save", model.ID, e);
                return Json(new
                {
                    Status = "failure",
                    Message = e.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(CustomerModel model)
        {
            var result = new List<object>();
            if (model.ID != 0)
            {
                //Edit
                //Check whether editable or not
                CustomerBO Temp = customerBL.GetCustomerDetails(model.ID);
                if (!Temp.IsDraft)
                {
                    result.Add(new { ErrorMessage = "Not Editable" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            return Save(model);
        }

        // GET: Masters/Customer/Edit/5
        public ActionResult Edit(int id)
        {
            CustomerModel customerModel = new CustomerModel();
            customerModel.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            customerModel.CategoryList = new SelectList(categoryBL.GetCustomerCategoryList(), "ID", "Name");
            customerModel.PriceList = new SelectList(customerBL.GetPriceList(), "PriceListID", "PriceListName");
            customerModel.DiscountList = new SelectList(customerBL.GetDiscountList(), "DiscountCategoryID", "DiscountCategory");
            customerModel.CashDiscountList = new SelectList(customerBL.GetCashDiscountList(), "DiscountCategoryID", "DiscountCategory");
            customerModel.CustomerTaxCategoryList = new SelectList(categoryBL.GetCustomerTaxCategoryList(), "ID", "Name");
            customerModel.CustomerAccountsCategoryList = new SelectList(categoryBL.GetCustomerAccountsCategoryList(), "ID", "Name");
            customerModel.CreditDaysList = new SelectList(supplierBL.GetCreditDaysgroup(), "CreditDays", "CreditDaysName");
            customerModel.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");
            customerModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            customerModel.CurrencyLists = new SelectList(currencyBL.GetCurrencyList(), "ID", "Name");

            var obj = customerBL.GetCustomerDetails(id);
            customerModel.ID = obj.ID;
            customerModel.Code = obj.Code;
            customerModel.Name = obj.Name;
            customerModel.Name2 = obj.Name2;
            customerModel.OldCode = obj.OldCode;
            customerModel.OldName = obj.OldName;
            customerModel.CategoryID = obj.CategoryID;
            customerModel.PriceListID = obj.PriceListID;
            customerModel.DiscountID = obj.DiscountID;
            customerModel.CashDiscountID = obj.CashDiscountID;
            customerModel.DiscountPercentage = obj.DiscountPercentage;
            customerModel.CustomerTaxCategoryID = obj.CustomerTaxCategoryID;
            customerModel.CustomerAccountsCategoryID = obj.CustomerAccountsCategoryID;
            customerModel.IsGSTRegistered = obj.IsGSTRegistered;
            customerModel.AadhaarNo = obj.AadhaarNo;
            customerModel.PanNo = obj.PanNo;
            customerModel.Currency = obj.Currency;
            customerModel.CurrencyID = obj.CurrencyID;
            customerModel.GstNo = obj.GstNo;
            customerModel.FaxNo = obj.FaxNo;
            customerModel.EmailID = obj.EmailID;
            customerModel.CreditDays = obj.CreditDays;
            customerModel.MinCreditLimit = obj.MinCreditLimit;
            customerModel.MaxCreditLimit = obj.MaxCreditLimit;
            customerModel.StartDate = General.FormatDate(obj.StartDate);
            customerModel.ExpiryDate = General.FormatDate(obj.ExpiryDate);
            customerModel.IsInterCompany = obj.IsInterCompany;
            customerModel.IsMappedtoExpsEntries = obj.IsMappedtoExpsEntries;
            customerModel.IsBlockedForSalesOrders = obj.IsBlockedForSalesOrders;
            customerModel.IsBlockedForSalesInvoices = obj.IsBlockedForSalesInvoices;
            customerModel.IsAlsoASupplier = obj.IsAlsoASupplier;
            customerModel.SupplierID = obj.SupplierID;
            customerModel.SupplierName = obj.SupplierName;
            customerModel.CustomerRouteID = obj.CustomerRouteID;
            customerModel.PaymentTypeID = obj.PaymentTypeID;
            customerModel.CashDiscountCategoryID = obj.CashDiscountCategoryID;
            customerModel.FSOName = obj.FSOName;
            customerModel.FSOID = obj.FSOID;
            customerModel.TradeLegalName = obj.TradeLegalName;
            customerModel.CustomerMonthlyTarget = obj.CustomerMonthlyTarget;
            customerModel.IsMappedToServiceSales = obj.IsMappedToServiceSales;
            customerModel.IsBlockedForChequeReceipt = obj.IsBlockedForChequeReceipt;
            customerModel.IsDraft = obj.IsDraft;
            customerModel.AddressList = addressBL.GetAddressByPartyType(id, "Customer").Select(a => new CustomerAddressModel()
            {
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                AddressLine3 = a.AddressLine3,
                Place = a.Place,
                PIN = a.PIN,
                ContactPerson = a.ContactPerson,
                LandLine1 = a.LandLine1,
                LandLine2 = a.LandLine2,
                MobileNo = a.MobileNo,
                Fax = a.Fax,
                Email = a.Email,
                District = a.District,
                State = a.State,
                IsBilling = a.IsBilling,
                IsShipping = a.IsShipping,
                IsDefault = (bool)a.IsDefault
            }).ToList();
            return View(customerModel);
        }

        public JsonResult GetCustomerList(DatatableModel Datatable)
        {
            try
            {
                int CustomerCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerCategoryID", Datatable.Params));
                int StateID = Convert.ToInt32(Datatable.GetValueFromKey("StateID", Datatable.Params));
                string CodeHit = Datatable.Columns[2].Search.Value;
                string NameHit = Datatable.Columns[3].Search.Value;
                string LocationHit = Datatable.Columns[4].Search.Value;
                string CustomerCategoryHit = Datatable.Columns[5].Search.Value;
                string LandLineHit = Datatable.Columns[6].Search.Value;
                string MobibleHit = Datatable.Columns[7].Search.Value;
                string CurrencyNameHit = Datatable.Columns[8].Search.Value;
                DatatableResultBO resultBO = customerBL.GetCustomerList("All", CustomerCategoryID, StateID, CodeHit, NameHit, LocationHit, CustomerCategoryHit, CurrencyNameHit, LandLineHit, MobibleHit, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "GetCustomerList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSalesOrderCustomerList(DatatableModel Datatable)
        {
            try
            {
                int CustomerCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerCategoryID", Datatable.Params));
                string CodeHit = Datatable.Columns[2].Search.Value;
                string NameHit = Datatable.Columns[3].Search.Value;
                string LocationHit = Datatable.Columns[4].Search.Value;
                string CustomerCategoryHit = Datatable.Columns[5].Search.Value;
                string LandLineHit = Datatable.Columns[6].Search.Value;
                string MobibleHit = Datatable.Columns[7].Search.Value;
                string CurrencyNameHit = Datatable.Columns[8].Search.Value;

                int StateID = Convert.ToInt32(Datatable.GetValueFromKey("StateID", Datatable.Params));
                DatatableResultBO resultBO = customerBL.GetCustomerList("Sales-Order", CustomerCategoryID, StateID, CodeHit, NameHit, LocationHit, CustomerCategoryHit, CurrencyNameHit, LandLineHit, MobibleHit, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "GetSalesOrderCustomerList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSalesInvoiceCustomerList(DatatableModel Datatable)
        {
            try
            {
                int CustomerCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerCategoryID", Datatable.Params));
                int StateID = Convert.ToInt32(Datatable.GetValueFromKey("StateID", Datatable.Params));
                string LandLineHit = Datatable.Columns[6].Search.Value;
                string MobibleHit = Datatable.Columns[7].Search.Value;
                string CurrencyNameHit = Datatable.Columns[8].Search.Value;
                DatatableResultBO resultBO = customerBL.GetCustomerList("Sales-Invoice", CustomerCategoryID, StateID, Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[4].Search.Value, Datatable.Columns[5].Search.Value, CurrencyNameHit, LandLineHit, MobibleHit, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "GetSalesInvoiceCustomerList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPartyList(DatatableModel Datatable)
        {
            try
            {
                DatatableResultBO resultBO = customerBL.GetPartyList(Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "GetPartyList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCustomerForMenuList(DatatableModel Datatable)
        {
            try
            {
                int CustomerCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerCategoryID", Datatable.Params));
                string CodeHint = Datatable.Columns[1].Search.Value;
                string NameHint = Datatable.Columns[2].Search.Value;
                string LocationHint = Datatable.Columns[3].Search.Value;
                string CustomerCategoryHint = Datatable.Columns[4].Search.Value;
                string PropratorNameHint = Datatable.Columns[5].Search.Value;
                string OldCodeHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = customerBL.GetCustomerMainList(Type, CustomerCategoryID, CodeHint, NameHint, LocationHint, CustomerCategoryHint, PropratorNameHint, OldCodeHint, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "GetCustomerForMenuList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCustomersAutoComplete(string Hint = "", int CustomerCategoryID = 0, int SateID = 0)
        {
            DatatableResultBO resultBO = customerBL.GetCustomerList("All", CustomerCategoryID, SateID, "", Hint, "", "", "", "", "", "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSalesOrderCustomersAutoComplete(string Hint = "", int CustomerCategoryID = 0, int SateID = 0)
        {
            DatatableResultBO resultBO = customerBL.GetCustomerList("Sales-Order", CustomerCategoryID, SateID, "", Hint, "", "", "", "", "", "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSalesInvoiceCustomersAutoComplete(string Hint = "", int CustomerCategoryID = 0, int SateID = 0)
        {
            DatatableResultBO resultBO = customerBL.GetCustomerList("Sales-Invoice", CustomerCategoryID, SateID, "", Hint, "", "", "", "", "", "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPartyAutoComplete(string Hint = "")
        {
            DatatableResultBO resultBO = customerBL.GetPartyList(Hint, "", "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckOutstanding(int CustomerID)
        {
            try
            {
                bool HasOutstanding = customerBL.HasOutstandingAmount(CustomerID);
                return Json(new { Status = "success", HasOutstanding = HasOutstanding }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "CheckOutstanding", CustomerID, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetBatchType(int CustomerID)
        {
            try
            {
                BatchTypeBO batchType = customerBL.GetBatchTypeID(CustomerID);
                return Json(new { Status = "success", BatchTypeID = batchType.ID, BatchType = batchType.Name }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "GetBatchType", CustomerID, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAddresses(int CustomerID)
        {
            try
            {
                List<AddressBO> BillingAddressList = addressBL.GetBillingAddress("Customer", CustomerID, "");
                List<AddressBO> ShippingAddressList = addressBL.GetShippingAddress("Customer", CustomerID, "");
                var DefaultBillingAddress = BillingAddressList.Where(a => a.IsDefault == true).FirstOrDefault();
                var DefaultShippingAddress = ShippingAddressList.Where(a => a.IsDefaultShipping == true).FirstOrDefault();
                if (DefaultBillingAddress == null || DefaultShippingAddress == null)
                {
                    StringBuilder error = new StringBuilder();
                    if (DefaultBillingAddress == null)
                    { error.Append("Default Billing Address is missing,"); }
                    if (DefaultShippingAddress == null)
                    { error.Append("Default Shipping Address is missing."); }
                    generalBL.LogError("Masters", "Customer", "GetAddresses", CustomerID, error.ToString(), "", "");
                    return Json(new { Status = "success", Message = error.ToString() }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    Status = "success",
                    BillingAddressList = BillingAddressList,
                    ShippingAddressList = ShippingAddressList,
                    DefaultBillingAddressID = DefaultBillingAddress.AddressID,
                    DefaultShippingAddressID = DefaultShippingAddress.AddressID
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "GetAddresses", CustomerID, e);
                return Json(new { Status = "success", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetCustomerLocationMapping(int CustomerID)
        {
            List<CustomerModel> CustomerLocationList = locationBL.GetCustomerLocationMappingByCustomerID(CustomerID).Select(a => new CustomerModel()
            {
                LocationID = a.ID,
                LocationName = a.LocationName,
                CustomerLocationID = a.LocationID
            }).ToList();
            return Json(CustomerLocationList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerDetails()
        {
            return View();
        }

        public JsonResult GetCustomerDetails(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string CustomerNameHint = Datatable.Columns[2].Search.Value;
                string CategoryHint = Datatable.Columns[3].Search.Value;
                string LocationHint = Datatable.Columns[4].Search.Value;
                string CustomerSchemeHint = Datatable.Columns[5].Search.Value;
                string DiscountPercentageHint = Datatable.Columns[6].Search.Value;
                string PriceListHint = Datatable.Columns[7].Search.Value;
                string MinCreditLimitHint = Datatable.Columns[8].Search.Value;
                string MaxCreditLimitHint = Datatable.Columns[9].Search.Value;
                string OutStandingAmountHint = Datatable.Columns[10].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = customerBL.GetCustomerDetails(CodeHint, CustomerNameHint, CategoryHint, LocationHint, CustomerSchemeHint, DiscountPercentageHint, PriceListHint, MinCreditLimitHint, MaxCreditLimitHint, OutStandingAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Customer", "GetCustomerDetails", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCustomerItemDetails(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string ItemNameHint = Datatable.Columns[2].Search.Value;
                string MRPHint = Datatable.Columns[3].Search.Value;
                string DiscountPercentageHint = Datatable.Columns[4].Search.Value;
                string QuantityHint = Datatable.Columns[5].Search.Value;
                string OfferQuantityHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int CustomerID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerID", Datatable.Params));

                DatatableResultBO resultBO = customerBL.GetCustomerItemDetails(CustomerID, CodeHint, ItemNameHint, MRPHint, DiscountPercentageHint, QuantityHint, OfferQuantityHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Customer", "GetCustomerItemDetails", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CustomerItemDetails()
        {
            return View();
        }

        public JsonResult CheckCustomerAlradyExist(int ID, string Name, string GstNo, string PanCardNo, string AdhaarCardNo, string Mobile, string LandLine1, string landline2)
        {
            try
            {
                var message = customerBL.CheckCustomerAlradyExist(ID, Name, GstNo, PanCardNo, AdhaarCardNo, Mobile, LandLine1, landline2);
                var isDuplicate = message.ToLower() == "do you want to save" ? false : true;

                return Json(new { Status = "success", Message = message, IsDuplicate = isDuplicate }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "CheckCustomerAlradyExist", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetServiceCustomerList(DatatableModel Datatable)
        {
            try
            {
                int CustomerCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerCategoryID", Datatable.Params));
                int StateID = Convert.ToInt32(Datatable.GetValueFromKey("StateID", Datatable.Params));
                DatatableResultBO resultBO = customerBL.GetServiceCustomerList("all", CustomerCategoryID, StateID, Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[4].Search.Value, Datatable.Columns[5].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "GetServiceCustomerList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetServiceCustomersAutoComplete(string Hint = "", int CustomerCategoryID = 0, int SateID = 0)
        {
            DatatableResultBO resultBO = customerBL.GetServiceCustomerList("all", CustomerCategoryID, SateID, "", Hint, "", "", "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServiceSalesOrderCustomersAutoComplete(string Hint = "", int CustomerCategoryID = 0, int SateID = 0)
        {
            DatatableResultBO resultBO = customerBL.GetServiceCustomerList("Sales-Order", CustomerCategoryID, SateID, "", Hint, "", "", "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServiceSalesInvoiceCustomersAutoComplete(string Hint = "", int CustomerCategoryID = 0, int SateID = 0)
        {
            DatatableResultBO resultBO = customerBL.GetServiceCustomerList("Sales-Invoice", CustomerCategoryID, SateID, "", Hint, "", "", "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServiceSalesOrderCustomerList(DatatableModel Datatable)
        {
            try
            {
                int CustomerCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerCategoryID", Datatable.Params));
                int StateID = Convert.ToInt32(Datatable.GetValueFromKey("StateID", Datatable.Params));
                DatatableResultBO resultBO = customerBL.GetServiceCustomerList("Sales-Order", CustomerCategoryID, StateID, Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[4].Search.Value, Datatable.Columns[5].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "GetSalesOrderCustomerList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetServiceSalesInvoiceCustomerList(DatatableModel Datatable)
        {
            try
            {
                int CustomerCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerCategoryID", Datatable.Params));
                int StateID = Convert.ToInt32(Datatable.GetValueFromKey("StateID", Datatable.Params));
                DatatableResultBO resultBO = customerBL.GetServiceCustomerList("Sales-Invoice", CustomerCategoryID, StateID, Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[4].Search.Value, Datatable.Columns[5].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Customer", "GetSalesInvoiceCustomerList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteCustomer(int id)
        {
            try
            {
                var obj = customerBL.DeleteCustomer(id);
                return
                     Json(new
                     {
                         Status = "success",
                         data = "",
                         message = ""
                     }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
