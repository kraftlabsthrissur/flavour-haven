using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Areas.Sales.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class SupplierController : Controller
    {
        #region Private Declarations
        private readonly IDropdownContract _dropdown;
        private ISupplierContract supplierBL;
        private IDistrictContract districtBL;
        private IStateContract stateBL;
        private IAddressContract addressBL;
        private ICategoryContract categoryBL;
        private IPaymentTypeContract paymentTypeBL;
        private IGeneralContract generalBL;
        private ILocationContract locationBL;
        private IPaymentDaysContract paymentDaysBL;
        private ICurrencyContract currencyBL;
        private ICountryContract countryBL;
        #endregion

        #region Constructors

        public SupplierController(IDropdownContract IDropdown)
        {
            this._dropdown = IDropdown;
            currencyBL = new CurrencyBL();
            supplierBL = new SupplierBL();
            stateBL = new StateBL();
            districtBL = new DistrictBL();
            addressBL = new AddressBL();
            categoryBL = new CategoryBL();
            paymentTypeBL = new PaymentTypeBL();
            generalBL = new GeneralBL();
            locationBL = new LocationBL();
            paymentDaysBL = new PaymentDaysBL();

        }
        #endregion
        // GET: Masters/Supplier
        public ActionResult Index()
        {

            List<SupplierModel> supplierList = new List<SupplierModel>();
            return View(supplierList);
        }

        // GET: Masters/Supplier/Details/5
        public ActionResult Details(int id)
        {
            var obj = supplierBL.GetSupplierDetails(id);
            SupplierModel supplierModel = new SupplierModel();
            SupplierBO supplierBO = new SupplierBO();
            supplierModel.Currency = obj.Currency;
            supplierModel.CurrencyName = obj.Currency;
            supplierModel.CurrencyID = obj.CurrencyID;
            supplierModel.ID = obj.ID;
            supplierModel.Code = obj.Code;
            supplierModel.Name = obj.Name;
            supplierModel.IsGSTRegistered = obj.IsGSTRegistered;
            supplierModel.GstNo = obj.GstNo;
            supplierModel.AdhaarCardNo = obj.AdhaarCardNo;
            supplierModel.PanCardNo = obj.PanCardNo;
            supplierModel.PaymentDays = obj.PaymentDays;
            supplierModel.SupplierCategoryName = obj.SupplierCategoryName;
            supplierModel.SupplierAccountsCategoryName = obj.SupplierAccountsCategoryName;
            supplierModel.SupplierTaxCategoryName = obj.SupplierTaxCategoryName;
            supplierModel.SupplierTaxSubCategoryName = obj.SupplierTaxSubCategoryName;
            supplierModel.PaymentGroupName = obj.PaymentGroupName;
            supplierModel.PaymentMethodName = obj.PaymentMethodName;
            supplierModel.CreatedDate = obj.CreatedDate;
            supplierModel.StartDate = General.FormatDate(obj.StartDate);
            supplierModel.IsBlockForPurcahse = obj.IsBlockForPurcahse;
            supplierModel.IsBlockForPayment = obj.IsBlockForPayment;
            supplierModel.IsBlockForReceipt = obj.IsBlockForReceipt;
            supplierModel.IsDeactivated = obj.IsDeactivated;
            supplierModel.DeactivatedDate = General.FormatDate(obj.DeactivatedDate);
            supplierModel.OldCode = obj.OldCode;
            supplierModel.UanNo = obj.UanNo;
            supplierModel.BankName = obj.BankName;
            supplierModel.BranchName = obj.BranchName;
            supplierModel.AcNo = obj.AcNo;
            supplierModel.IfscNo = obj.IfscNo;
            supplierModel.CustomerName = obj.CustomerName;
            supplierModel.EmployeeName = obj.EmployeeName;
            supplierModel.IsCustomer = obj.IsCustomer;
            supplierModel.IsEmployee = obj.IsEmployee;
            supplierModel.CustomerID = obj.CustomerID;
            supplierModel.EmployeeID = obj.EmployeeID;
            supplierModel.IsDeactivated = obj.IsDeactivated;
            supplierModel.TradeLegalName = obj.TradeLegalName;
            supplierModel.RelatedSupplierList = supplierBL.GetRelatedSupplier(id).Select(m => new RelatedSupplierModel()
            {
                RelatedSupplierID = m.RelatedSupplierID,
                RelatedSupplierLocation = m.RelatedSupplierLocation,
                RelatedSupplierName = m.RelatedSupplierName
            }).ToList();
            supplierModel.AddressList = addressBL.GetAddressByPartyType(id, "Supplier").Select(a => new AddressModel()
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

            supplierModel.LocationList = new SelectList(locationBL.GetSupplierLocationBySupplierID(supplierModel.ID), "LocationID", "LocationName");
            supplierModel.CurrencyList = new SelectList(currencyBL.GetCurrencyList(), "ID", "Name");
            return View(supplierModel);
        }

        // GET: Masters/Supplier/Create
        public ActionResult Create()
        {
            SupplierModel supplierModel = new SupplierModel();
            supplierModel.PaymentDaysList = paymentDaysBL.GetPaymentDaysList().Select(a => new PaymentDaysModel()
            {
                ID = a.ID,

                Name = a.Name,
                Days = a.Days
            }).ToList();
            supplierModel.CategoryItemGroup = new SelectList(categoryBL.GetSuppliersCategoryList(), "ID", "Name");
            supplierModel.SupplierCategoryID = 6;
            supplierModel.CurrencyList = new SelectList(currencyBL.GetCurrencyList(), "ID", "Name");
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            if (currency != null)
            {
                supplierModel.CurrencyID = currency.CurrencyID;
            }
            supplierModel.SuppliersAccountCategory = new SelectList(categoryBL.GetSuppliersAccountCategoryGroup(), "ID", "Name");
            supplierModel.SuppliersTaxCategory = new SelectList(categoryBL.GetSuppliersTaxCategoryGroup(), "ID", "Name");
            supplierModel.SuppliersTaxSubCategory = new SelectList(categoryBL.GetSuppliersSubTaxCategoryGroup(), "ID", "Name");
            supplierModel.ItemCategoryList = new SelectList(categoryBL.GetSupplierItemCategoryList(), "CategoryID", "CategoryName");

            supplierModel.PaymentMethod = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");
            supplierModel.PaymentGroup = new SelectList(supplierBL.PaymentGroupList(), "PaymentGroupID", "PaymentGroupName");
            supplierModel.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            supplierModel.Code = generalBL.GetSerialNo("Supplier", "Code");
            supplierModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            supplierModel.StartDate = General.FormatDate(DateTime.Now);
            supplierModel.DeactivatedDate = General.FormatDate(Convert.ToDateTime(generalBL.GetConfig("DefaultDate")));
            return View(supplierModel);
        }

        // POST: Masters/Supplier/Save
        [HttpPost]
        public ActionResult Save(SupplierModel model)
        {
            //if (ModelState.IsValid)
            //{
            var result = new List<object>();
            try
            {

                SupplierBO supplierBO = new SupplierBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    AdhaarCardNo = model.AdhaarCardNo,
                    PanCardNo = model.PanCardNo,
                    PaymentDays = model.PaymentDays,
                    SupplierCategoryID = model.SupplierCategoryID,
                    StartDate = General.ToDateTime(model.StartDate),
                    DeactivatedDate = General.ToDateTime(model.DeactivatedDate),
                    SupplierAccountsCategoryID = model.SupplierAccountsCategoryID,
                    SupplierTaxCategoryID = model.SupplierTaxCategoryID,
                    SupplierTaxSubCategoryID = model.SupplierTaxSubCategoryID,
                    PaymentMethodID = model.PaymentMethodID,
                    PaymentGroupID = model.PaymentGroupID,
                    Currency = model.Currency,
                    IsBlockForPayment = model.IsBlockForPayment,
                    IsBlockForReceipt = model.IsBlockForReceipt,
                    IsBlockForPurcahse = model.IsBlockForPurcahse,
                    IsGSTRegistered = model.IsGSTRegistered,
                    GstNo = model.GstNo,
                    OldCode = model.OldCode,
                    UanNo = model.UanNo,
                    BankName = model.BankName,
                    BranchName = model.BranchName,
                    AcNo = model.AcNo,
                    IfscNo = model.IfscNo,
                    CustomerID = model.CustomerID,
                    EmployeeID = model.EmployeeID,
                    IsCustomer = model.IsCustomer,
                    IsEmployee = model.IsEmployee,
                    TradeLegalName = model.TradeLegalName,
                    IsActiveSupplier = model.IsActiveSupplier,
                    CurrencyID = model.CurrencyID
                };

                if (model.IsDeactivated == true)
                {
                    supplierBO.IsDeactivated = true;
                }

                List<SupplierBO> SupplierItemCategoryList = new List<SupplierBO>();
                SupplierBO CategoryItem;
                foreach (var item in model.SupplierItemCategoryList)
                {
                    CategoryItem = new SupplierBO()
                    {
                        CategoryID = item.CategoryID
                    };
                    SupplierItemCategoryList.Add(CategoryItem);
                }

                List<SupplierBO> SupplierLocationList = new List<SupplierBO>();
                SupplierBO SupplierLocation;
                foreach (var item in model.SupplierLocationList)
                {
                    SupplierLocation = new SupplierBO()
                    {
                        LocationID = item.LocationID
                    };
                    SupplierLocationList.Add(SupplierLocation);
                }

                List<AddressBO> AddressList = new List<AddressBO>();
                AddressBO AddressItem;
                if (model.AddressList != null && model.AddressList.Count > 0)
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
                        AddressList.Add(AddressItem);
                    }
                }
                else
                {
                    AddressItem = new AddressBO()
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
                    AddressList.Add(AddressItem);
                }
                List<RelatedSupplierBO> RelatedSuppliers = new List<RelatedSupplierBO>();
                {
                    if (model.RelatedSupplierList != null)
                    {
                        RelatedSupplierBO RelatedSuppliersList;
                        foreach (var item in model.RelatedSupplierList)
                        {
                            RelatedSuppliersList = new RelatedSupplierBO()
                            {
                                RelatedSupplierID = item.RelatedSupplierID,
                                RelatedSupplierLocation = item.RelatedSupplierLocation
                            };
                            RelatedSuppliers.Add(RelatedSuppliersList);
                        }
                    }
                }

                var outId = supplierBL.CreateSupplier(supplierBO, AddressList, SupplierItemCategoryList, SupplierLocationList, RelatedSuppliers);
                return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = "Failed To Save" });
                generalBL.LogError("Masters", "Supplier", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
            //}
            //else
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors);
            //    return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
            //}
        }

        // GET: Masters/Supplier/Edit/5
        public ActionResult Edit(int id)
        {
            SupplierModel supplierModel = new SupplierModel();
            supplierModel.PaymentDaysList = paymentDaysBL.GetPaymentDaysList().Select(a => new PaymentDaysModel()
            {
                ID = a.ID,

                Name = a.Name,
                Days = a.Days
            }).ToList();
            supplierModel.CategoryItemGroup = new SelectList(categoryBL.GetSuppliersCategoryList(), "ID", "Name");
            supplierModel.SuppliersAccountCategory = new SelectList(categoryBL.GetSuppliersAccountCategoryGroup(), "ID", "Name");
            supplierModel.SuppliersTaxCategory = new SelectList(categoryBL.GetSuppliersTaxCategoryGroup(), "ID", "Name");
            supplierModel.SuppliersTaxSubCategory = new SelectList(categoryBL.GetSuppliersSubTaxCategoryGroup(), "ID", "Name");
            supplierModel.ItemCategoryList = new SelectList(categoryBL.GetSupplierItemCategoryList(), "CategoryID", "CategoryName");
            supplierModel.PaymentMethod = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");
            //  supplierModel.PaymentMethod = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");
            supplierModel.PaymentGroup = new SelectList(supplierBL.PaymentGroupList(), "PaymentGroupID", "PaymentGroupName");
            supplierModel.DisitrictList = new SelectList(districtBL.GetDistrictList(0), "ID", "Name");
            supplierModel.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            supplierModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            supplierModel.CurrencyList = new SelectList(currencyBL.GetCurrencyList(), "ID", "Name");

            var obj = supplierBL.GetSupplierDetails(id);
            SupplierBO supplierBO = new SupplierBO();
            supplierModel.IsActiveSupplier = obj.IsActiveSupplier;
            supplierModel.ID = obj.ID;
            supplierModel.Code = obj.Code;
            supplierModel.Name = obj.Name;
            supplierModel.IsGSTRegistered = obj.IsGSTRegistered;
            supplierModel.GstNo = obj.GstNo;
            supplierModel.AdhaarCardNo = obj.AdhaarCardNo;
            supplierModel.PanCardNo = obj.PanCardNo;
            supplierModel.PaymentDays = obj.PaymentDays;
            supplierModel.SupplierCategoryID = obj.SupplierCategoryID;
            supplierModel.SupplierAccountsCategoryID = obj.SupplierAccountsCategoryID;
            supplierModel.SupplierAccountsCategoryName = obj.SupplierAccountsCategoryName;
            supplierModel.SupplierTaxCategoryID = obj.SupplierTaxCategoryID;
            supplierModel.SupplierTaxCategoryName = obj.SupplierTaxCategoryName;
            supplierModel.SupplierTaxSubCategoryID = obj.SupplierTaxSubCategoryID;
            supplierModel.SupplierTaxSubCategoryName = obj.SupplierTaxSubCategoryName;
            supplierModel.PaymentGroupID = obj.PaymentGroupID;
            supplierModel.PaymentGroupName = obj.PaymentGroupName;
            supplierModel.PaymentMethodID = obj.PaymentMethodID;
            supplierModel.PaymentMethodName = obj.PaymentMethodName;
            supplierModel.CreatedDate = obj.CreatedDate;
            supplierModel.IsDeactivated = obj.IsDeactivated;
            supplierModel.IsBlockForPurcahse = obj.IsBlockForPurcahse;
            supplierModel.IsBlockForPayment = obj.IsBlockForPayment;
            supplierModel.IsBlockForReceipt = obj.IsBlockForReceipt;
            supplierModel.StartDate = General.FormatDate(obj.StartDate);
            supplierModel.DeactivatedDate = General.FormatDate(obj.DeactivatedDate);
            supplierModel.OldCode = obj.OldCode;
            supplierModel.UanNo = obj.UanNo;
            supplierModel.BankName = obj.BankName;
            supplierModel.BranchName = obj.BranchName;
            supplierModel.AcNo = obj.AcNo;
            supplierModel.IfscNo = obj.IfscNo;
            supplierModel.CustomerName = obj.CustomerName;
            supplierModel.EmployeeName = obj.EmployeeName;
            supplierModel.IsCustomer = obj.IsCustomer;
            supplierModel.IsEmployee = obj.IsEmployee;
            supplierModel.CustomerID = obj.CustomerID;
            supplierModel.EmployeeID = obj.EmployeeID;
            supplierModel.TradeLegalName = obj.TradeLegalName;
            supplierModel.Currency = obj.Currency;
            supplierModel.CurrencyName = obj.Currency;
            supplierModel.CurrencyID = obj.CurrencyID;
            supplierModel.RelatedSupplierList = supplierBL.GetRelatedSupplier(id).Select(m => new RelatedSupplierModel()
            {
                RelatedSupplierID = m.RelatedSupplierID,
                RelatedSupplierLocation = m.RelatedSupplierLocation,
                RelatedSupplierName = m.RelatedSupplierName
            }).ToList();
            return View(supplierModel);
        }

        public JsonResult DeleteSupplier(int id)
        {
            try
            {
                var obj = supplierBL.DeleteSupplier(id);
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
        public JsonResult GetAddresses(int SupplierID)
        {
            try
            {
                List<AddressBO> BillingAddressList = addressBL.GetBillingAddress("Supplier", SupplierID, "");
                List<AddressBO> ShippingAddressList = addressBL.GetShippingAddress("Supplier", SupplierID, "");
                var DefaultBillingAddress = BillingAddressList.Where(a => a.IsDefault == true).FirstOrDefault();
                var DefaultShippingAddress = ShippingAddressList.Where(a => a.IsDefaultShipping == true).FirstOrDefault();
                if (DefaultBillingAddress == null || DefaultShippingAddress == null)
                {
                    StringBuilder error = new StringBuilder();
                    if (DefaultBillingAddress == null)
                    { error.Append("Default Billing Address is missing,"); }
                    if (DefaultShippingAddress == null)
                    { error.Append("Default Shipping Address is missing."); }
                    generalBL.LogError("Masters", "Supplier", "GetAddresses", SupplierID, error.ToString(), "", "");
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
                generalBL.LogError("Masters", "Supplier", "GetAddresses", SupplierID, e);
                return Json(new { Status = "success", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult GetAddressList(int SupplierID)
        {
            List<AddressModel> AddressList = addressBL.GetAddressByPartyType(SupplierID, "Supplier").Select(a => new AddressModel()
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

        public JsonResult GetSupplierItemCategory(int SupplierID)
        {
            List<SupplierModel> SupplierItemCategoryList = categoryBL.GetSupplierItemCategoryBySupplierID(SupplierID).Select(a => new SupplierModel()
            {
                CategoryID = a.CategoryID,
                CategoryName = a.CategoryName,
                SupplierItemCategoryID = a.ID
            }).ToList();
            return Json(SupplierItemCategoryList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSupplierForAutoComplete(string term = "")
        {
            try
            {
                DatatableResultBO resultBO = supplierBL.GetSupplierList("stock", "", term, "", "", "", "", "", "", "", "Name", "ASC", 0, 20);
                return Json(resultBO.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Supplier", "getSupplierForAutoComplete", 0, e);
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getSupplierNonInterCompanyForAutoComplete(string term = "")
        {
            try
            {
                DatatableResultBO resultBO = supplierBL.GetSupplierList("NotInterCompany", "", term, "", "", "", "", "", "", "", "Name", "ASC", 0, 20);
                return Json(resultBO.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Supplier", "getSupplierNonInterCompanyForAutoComplete", 0, e);
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getServiceSupplierForAutoComplete(string term = "")
        {
            try
            {
                DatatableResultBO resultBO = supplierBL.GetSupplierList("service", "", term, "", "", "", "", "", "", "", "Name", "ASC", 0, 20);
                return Json(resultBO.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Supplier", "getServiceSupplierForAutoComplete", 0, e);
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getCreditorsForAutoComplete(string term = "")
        {
            List<SupplierModel> supplierList = new List<SupplierModel>();
            supplierList = supplierBL.getCreditorsForAutoComplete(term).Select(a => new SupplierModel()
            {
                ID = a.ID,
                Name = a.Name,
                Code = a.Code,
                Location = a.Location,
                SupplierCategoryID = a.SupplierCategoryID,
                StateID = a.StateID,
                IsGSTRegistered = a.IsGSTRegistered,
            }).ToList();

            return Json(supplierList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllSupplierList(DatatableModel Datatable)
        {
            try
            {
                string LandLine = Datatable.Columns[7].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;

                DatatableResultBO resultBO = supplierBL.GetSupplierList(
                    "all",
                    Datatable.Columns[2].Search.Value,
                    Datatable.Columns[3].Search.Value,
                    Datatable.Columns[4].Search.Value,
                    Datatable.Columns[5].Search.Value,
                    Datatable.Columns[6].Search.Value,
                    "",
                    Datatable.Columns[7].Search.Value,
                    LandLine, MobileNo,
                    Datatable.Columns[Datatable.Order[0].Column].Data,
                    Datatable.Order[0].Dir,
                    Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "GetAllSupplierList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllSupplierListForMainList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string NameHint = Datatable.Columns[2].Search.Value;
                string LocationHint = Datatable.Columns[3].Search.Value;
                string SupplierCategoryHint = Datatable.Columns[4].Search.Value;
                string ItemCategoryHint = Datatable.Columns[5].Search.Value;
                string OldCodeHint = Datatable.Columns[6].Search.Value;
                string GSTRegisteredHint = Datatable.Columns[7].Search.Value;
                string LandLine = Datatable.Columns[7].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DatatableResultBO resultBO = supplierBL.GetSupplierList(
                          "all", CodeHint, NameHint, LocationHint, SupplierCategoryHint, ItemCategoryHint, OldCodeHint, GSTRegisteredHint, LandLine, MobileNo, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "GetAllSupplierListForMainList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetStockSupplierList(DatatableModel Datatable)
        {
            try
            {
                string LandLine = Datatable.Columns[6].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;
                DatatableResultBO resultBO = supplierBL.GetSupplierList("stock", Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value,
                    Datatable.Columns[4].Search.Value, Datatable.Columns[5].Search.Value, Datatable.Columns[8].Search.Value,
                    "", Datatable.Columns[9].Search.Value, LandLine, MobileNo, Datatable.Columns[Datatable.Order[0].Column].Data,
                    Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "GetAllSupplierListForMainList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetServiceSupplierList(DatatableModel Datatable)
        {
            try
            {
                string LandLine = Datatable.Columns[7].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;
                DatatableResultBO resultBO = supplierBL.GetSupplierList(
                    "service", Datatable.Columns[2].Search.Value,
                    Datatable.Columns[3].Search.Value,
                    Datatable.Columns[4].Search.Value,
                    Datatable.Columns[5].Search.Value,
                    Datatable.Columns[6].Search.Value,
                    "",
                    Datatable.Columns[7].Search.Value, LandLine, MobileNo,
                    Datatable.Columns[Datatable.Order[0].Column].Data,

                    Datatable.Order[0].Dir,
                    Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "GetServiceSupplierList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetGRNWiseSupplierList(DatatableModel Datatable)
        {
            try
            {
                string LandLine = Datatable.Columns[7].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;
                DatatableResultBO resultBO = supplierBL.GetSupplierList(
                    "GRNWise",
                    Datatable.Columns[2].Search.Value,
                    Datatable.Columns[3].Search.Value,
                    Datatable.Columns[4].Search.Value,
                    Datatable.Columns[5].Search.Value,
                    Datatable.Columns[6].Search.Value,
                    "",
                    Datatable.Columns[7].Search.Value,
                    LandLine, MobileNo,
                    Datatable.Columns[Datatable.Order[0].Column].Data,
                    Datatable.Order[0].Dir,
                    Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "GetGRNWiseSupplierList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetGRNWiseSupplierForAutoComplete(string term = "")
        {
            List<SupplierModel> supplierList = new List<SupplierModel>();
            supplierList = supplierBL.GetGRNWiseSupplierForAutoComplete(term).Select(a => new SupplierModel()
            {
                ID = a.ID,
                Name = a.Name,
                Code = a.Code,
                Location = a.Location,
                SupplierCategoryID = a.SupplierCategoryID,
                StateID = a.StateID,
                IsGSTRegistered = a.IsGSTRegistered,
            }).ToList();

            return Json(supplierList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCreditorSupplierList(DatatableModel Datatable)
        {
            try
            {
                string LandLine = Datatable.Columns[7].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;
                DatatableResultBO resultBO = supplierBL.GetSupplierList(
                    "creditor", Datatable.Columns[2].Search.Value,
                    Datatable.Columns[3].Search.Value,
                    Datatable.Columns[4].Search.Value,
                    Datatable.Columns[5].Search.Value,
                    Datatable.Columns[6].Search.Value,
                    "",
                    Datatable.Columns[7].Search.Value,
                    LandLine, MobileNo,
                    Datatable.Columns[Datatable.Order[0].Column].Data,
                    Datatable.Order[0].Dir,
                    Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "GetCreditorSupplierList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMilkSupplierAutoComplete(string NameHint)
        {
            try
            {
                DatatableResultBO resultBO = supplierBL.GetSupplierList("milk", "", NameHint, "", "", "", "", "", "", "", "Name", "ASC", 0, 20);
                return Json(new { Status = "failure", Data = resultBO.data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Supplier", "GetMilkSupplierAutoComplete", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckSupplierAlradyExist(int ID, string Name, string GstNo, string PanCardNo, string AdhaarCardNo, string Mobile, string LandLine1, string landline2, string AcNo)
        {
            try
            {
                var message = supplierBL.CheckSupplierAlradyExist(ID, Name, GstNo, PanCardNo, AdhaarCardNo, Mobile, LandLine1, landline2, AcNo);
                var isDuplicate = message.ToLower() == "do you want to save" ? false : true;
                return Json(new { Status = "success", Message = message, IsDuplicate = isDuplicate }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Supplier", "CheckSupplierAlradyExist", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMilkSupplierList(DatatableModel Datatable)
        {
            try
            {
                string LandLine = Datatable.Columns[7].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;
                DatatableResultBO resultBO = supplierBL.GetSupplierList(
                    "milk",
                    Datatable.Columns[2].Search.Value,
                    Datatable.Columns[3].Search.Value,
                    Datatable.Columns[4].Search.Value,
                    Datatable.Columns[5].Search.Value,
                    Datatable.Columns[6].Search.Value,
                    "",
                    Datatable.Columns[7].Search.Value,
                    LandLine, MobileNo,
                    Datatable.Columns[Datatable.Order[0].Column].Data,
                    Datatable.Order[0].Dir,
                    Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "GetMilkSupplierList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetStockAndMilkSupplierAutoComplete(string NameHint)
        {
            try
            {
                DatatableResultBO resultBO = supplierBL.GetSupplierList("stock-and-milk", "", NameHint, "", "", "", "", "", "", "", "Name", "ASC", 0, 20);
                return Json(new { Status = "failure", Data = resultBO.data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Supplier", "GetStockAndMilkSupplierAutoComplete", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetStockAndMilkSupplierList(DatatableModel Datatable)
        {
            try
            {
                string LandLine = Datatable.Columns[7].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;
                DatatableResultBO resultBO = supplierBL.GetSupplierList(
                    "stock-and-milk",
                    Datatable.Columns[2].Search.Value,
                    Datatable.Columns[3].Search.Value,
                    Datatable.Columns[4].Search.Value,
                    Datatable.Columns[5].Search.Value,
                    Datatable.Columns[6].Search.Value,
                    "",
                    Datatable.Columns[7].Search.Value,
                    LandLine, MobileNo,
                    Datatable.Columns[Datatable.Order[0].Column].Data,
                    Datatable.Order[0].Dir,
                    Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "GetStockAndMilkSupplierList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllSupplierAutoComplete(string Term = "")
        {
            List<SupplierModel> supplierList = new List<SupplierModel>();
            supplierList = supplierBL.GetAllSupplierAutoComplete(Term).Select(a => new SupplierModel()
            {
                ID = a.ID,
                Name = a.Name,
                Code = a.Code,
                Location = a.Location,
                SupplierCategoryID = a.SupplierCategoryID,
                StateID = a.StateID,
                IsGSTRegistered = a.IsGSTRegistered,
            }).ToList();

            return Json(supplierList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSupplierLocationMapping(int SupplierID)
        {
            List<SupplierModel> SupplierLocationList = locationBL.GetSupplierLocationBySupplierID(SupplierID).Select(a => new SupplierModel()
            {
                LocationID = a.ID,
                LocationName = a.LocationName,
                SupplierLocationID = a.LocationID
            }).ToList();
            return Json(SupplierLocationList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNotInterCompanySupplierList(DatatableModel Datatable)
        {
            try
            {
                string Code = Datatable.Columns[2].Search.Value;
                string Name = Datatable.Columns[3].Search.Value;
                string Location = Datatable.Columns[4].Search.Value;
                string SupplierCategory = Datatable.Columns[5].Search.Value;
                string LandLine = Datatable.Columns[6].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;
                string ItemCategory = Datatable.Columns[8].Search.Value;
                string GSTRegistered = Datatable.Columns[9].Search.Value;
                DatatableResultBO resultBO = supplierBL.GetSupplierList(
                    "NotInterCompany", Code, Name, Location, SupplierCategory,
                    ItemCategory, "", GSTRegistered, LandLine, MobileNo,
                    Datatable.Columns[Datatable.Order[0].Column].Data,
                    Datatable.Order[0].Dir,
                    Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "GetNotInterCompanySupplierList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetNotInterCompanyAndMilkSupplierList(DatatableModel Datatable)
        {
            try
            {
                string LandLine = Datatable.Columns[7].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;
                DatatableResultBO resultBO = supplierBL.GetSupplierList(
                    "NotInterCompanyAndMilk",
                    Datatable.Columns[2].Search.Value,
                    Datatable.Columns[3].Search.Value,
                    Datatable.Columns[4].Search.Value,
                    Datatable.Columns[5].Search.Value,
                    Datatable.Columns[6].Search.Value,
                    "",
                    Datatable.Columns[7].Search.Value,
                    LandLine, MobileNo,
                    Datatable.Columns[Datatable.Order[0].Column].Data,
                    Datatable.Order[0].Dir,
                    Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "GetNotInterCompanyAndMilkSupplierList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetNotInterCompanyAndMilkSupplierAutoComplete(string NameHint)
        {
            try
            {
                DatatableResultBO resultBO = supplierBL.GetSupplierList("NotInterCompanyAndMilk", "", NameHint, "", "", "", "", "", "", "", "Name", "ASC", 0, 20);
                return Json(new { Status = "failure", Data = resultBO.data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Supplier", "GetNotInterCompanyAndMilkSupplierList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDoctorList(DatatableModel Datatable)
        {
            try
            {
                string LandLine = Datatable.Columns[7].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;
                DatatableResultBO resultBO = supplierBL.GetSupplierList(
                    "Branch Doctor",
                    Datatable.Columns[2].Search.Value,
                    Datatable.Columns[3].Search.Value,
                    "",
                    "",
                    "",
                    "",
                    "",
                    LandLine, MobileNo,
                    Datatable.Columns[Datatable.Order[0].Column].Data,
                    Datatable.Order[0].Dir,
                    Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "GetDoctorList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPaymentSupplierList(DatatableModel Datatable)
        {
            try
            {
                string LandLine = Datatable.Columns[7].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;
                DatatableResultBO resultBO = supplierBL.GetSupplierList(
                    "Payment",
                    Datatable.Columns[2].Search.Value,
                    Datatable.Columns[3].Search.Value,
                    Datatable.Columns[4].Search.Value,
                    Datatable.Columns[5].Search.Value,
                    Datatable.Columns[6].Search.Value,
                    "",
                    Datatable.Columns[7].Search.Value,
                    LandLine, MobileNo,
                    Datatable.Columns[Datatable.Order[0].Column].Data,
                    Datatable.Order[0].Dir,
                    Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "GetPaymentSupplierList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSupplierListForPaymentReturn(DatatableModel Datatable)
        {
            try
            {
                string LandLine = Datatable.Columns[7].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;
                DatatableResultBO resultBO = supplierBL.GetSupplierListForPaymentReturn(
                    "Payment",
                    Datatable.Columns[2].Search.Value,
                    Datatable.Columns[3].Search.Value,
                    Datatable.Columns[4].Search.Value,
                    Datatable.Columns[5].Search.Value,
                    Datatable.Columns[6].Search.Value,
                    "",
                    Datatable.Columns[7].Search.Value,
                    LandLine, MobileNo,
                    Datatable.Columns[Datatable.Order[0].Column].Data,
                    Datatable.Order[0].Dir,
                    Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "GetSupplierListForPaymentReturn", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult InterCompanySupplier(String Term)
        {
            try
            {
                List<SupplierModel> supplierList = new List<SupplierModel>();
                supplierList = supplierBL.InterCompanySupplier(Term).Select(a => new SupplierModel()
                {
                    ID = a.ID,
                    Code = a.Code,
                    StateID = a.StateID,
                    Name = a.Name,
                    IsGSTRegistered = a.IsGSTRegistered,
                    CustomerID = a.CustomerID,
                    SupplierCategoryName = a.SupplierCategoryName,
                    Location = a.Location,
                    ItemCategory = a.ItemCategory,
                    LocationID = a.LocationID

                }).ToList();

                return Json(supplierList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "InterCompanySupplier", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult InterCompanySupplierForLocation(String Term)
        {
            try
            {
                List<SupplierModel> supplierList = new List<SupplierModel>();
                supplierList = supplierBL.InterCompanySupplierListForLocation(Term).Select(a => new SupplierModel()
                {
                    ID = a.ID,
                    Code = a.Code,
                    StateID = a.StateID,
                    Name = a.Name,
                    IsGSTRegistered = a.IsGSTRegistered,
                    CustomerID = a.CustomerID,
                    SupplierCategoryName = a.SupplierCategoryName,
                    Location = a.Location,
                    ItemCategory = a.ItemCategory,
                    LocationID = a.LocationID

                }).ToList();

                return Json(supplierList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Masters", "Supplier", "InterCompanySupplierForLocation", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDoctorAutoComplete(string term = "")
        {
            try
            {
                DatatableResultBO resultBO = supplierBL.GetSupplierList("Branch Doctor", "", term, "", "", "", "", "", "", "", "Name", "ASC", 0, 20);
                return Json(resultBO.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Supplier", "GetDoctorAutoComplete", 0, e);
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPaymentSupplierForAutoComplete(string term = "")
        {
            try
            {
                DatatableResultBO resultBO = supplierBL.GetSupplierList("Payment", "", term, "", "", "", "", "", "", "", "Name", "ASC", 0, 20);
                return Json(resultBO.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Supplier", "GetPaymentSupplierForAutoComplete", 0, e);
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetInterCompanySupplierForAutoComplete(string term = "")
        {
            try
            {
                DatatableResultBO resultBO = supplierBL.GetSupplierList("InterCompany", "", term, "", "", "", "", "", "", "", "Name", "ASC", 0, 20);
                return Json(resultBO.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Supplier", "GetInterCompanySupplierForAutoComplete", 0, e);
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveAsDraft(SupplierModel model)
        {
            var result = new List<object>();
            if (model.ID != 0)
            {
                //Edit
                //Check whether editable or not
                SupplierBO Temp = supplierBL.GetSupplierDetails(model.ID);
                if (Temp.IsActiveSupplier)
                {
                    result.Add(new { ErrorMessage = "Not Editable" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            return Save(model);
        }

        public JsonResult GetSuppliersForListView(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string NameHint = Datatable.Columns[2].Search.Value;
                string LocationHint = Datatable.Columns[3].Search.Value;
                string SupplierCategoryHint = Datatable.Columns[4].Search.Value;
                string ItemCategoryHint = Datatable.Columns[5].Search.Value;
                string LandLine = Datatable.Columns[7].Search.Value;
                string MobileNo = Datatable.Columns[7].Search.Value;
                string OldCodeHint = Datatable.Columns[6].Search.Value;
                string GSTRegisteredHint = Datatable.Columns[7].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;

                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = supplierBL.GetSupplierList(Type, CodeHint, NameHint, LocationHint, SupplierCategoryHint, ItemCategoryHint, OldCodeHint, GSTRegisteredHint, LandLine, MobileNo, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDescription(int SupplierID, string Type = "")
        {
            List<SupplierDescriptionModel> supplierList = new List<SupplierDescriptionModel>();
            var Name = "";
            supplierList = supplierBL.GetDescription(SupplierID, Type).Select(a => new SupplierDescriptionModel()
            {
                Name = a.Name,
                Key = a.Key,
                Value = a.Value
            }).ToList();
            Name = supplierBL.GetDescription(SupplierID, Type).FirstOrDefault().Name;
            return Json(new { data = supplierList, Name = Name }, JsonRequestBehavior.AllowGet);

        }
    }
}
