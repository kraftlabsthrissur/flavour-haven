using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;
using PresentationContractLayer;
using BusinessLayer;
using BusinessObject;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class SchemeAllocationController : Controller
    {
        private ISchemeAllocationContract schemeBL;
        private IGeneralContract generalBL;
        private ICategoryContract categoryBL;
        private ICustomerContract customerBL;
        private IStateContract stateBL;
        private ICountryContract countryBL;
        private IDistrictContract districtBL;
        // GET: Masters/SchemeAllocation
        public SchemeAllocationController()
        {
            schemeBL = new SchemeAllocationBL();
            generalBL = new GeneralBL();
            stateBL = new StateBL();
            customerBL = new CustomerBL();
            categoryBL = new CategoryBL();
            districtBL = new DistrictBL();
            countryBL = new CountryBL();
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: Masters/SchemeAllocation/Details/5
        public ActionResult Details(int id)
        {
            SchemeAllocationBO BO = schemeBL.GetSchemeAllocationDetails((int)id);

            List<SchemeCustomerBO> Customer = schemeBL.GetSchemeCustomerList((int)id).ToList();
            List<SchemeStateBO> State = schemeBL.GetSchemeStateList((int)id).Where(a => a.CountryID == BO.CountryID && a.SchemeStateID != 0).ToList();
            List<SchemeCustomerCategoryBO> CustomerCategory = schemeBL.GetSchemeCategoryList((int)id).Where(a => a.SchemeCategoryID != 0).ToList();
            List<SchemeDistrictBO> District = schemeBL.GetSchemeDistrictList((int)id).Where(a => a.StateID == State.Where(b => b.SchemeStateID != 0).FirstOrDefault()?.StateID && a.SchemeDistrictID != 0).ToList();

            SchemeAllocationModel Model = new SchemeAllocationModel()
            {
                ID = BO.ID,
                Code = BO.Code,
                StartDate = General.FormatDateNull((DateTime)BO.StartDate),
                EndDate = General.FormatDateNull((DateTime)BO.EndDate),
                Scheme = BO.SchemeName,
                Country = BO.Country,
                CountryID = BO.CountryID,
            };

            Model.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");

            List<SchemeItemBO> Item = schemeBL.GetSchemeItemList((int)id);
            Model.Items = Item.Select(a => new SchemeItemModel()
            {
                ID = a.ID,
                StartDate = a.StartDate == null ? "" : General.FormatDate((DateTime)a.StartDate),
                EndDate = a.EndDate == null ? "" : General.FormatDate((DateTime)a.EndDate),
                Item = a.Item,
                ItemID = a.ItemID,
                OfferItem = a.OfferItem,
                OfferItemID = a.OfferItemID,
                OfferQty = a.OfferQty,
                InvoiceQty = a.InvoiceQty,
                BusinessCategory = a.BusinessCategory,
                BusinessCategoryID = a.BusinessCategoryID,
                SalesCategory = a.SalesCategory,
                SalesCategoryID = a.SalesCategoryID
            }).ToList();

            Model.Districts = District.Select(a => new SchemeDistrict()
            {
                District = a.District,
                DistrictID = a.DistrictID
            }).ToList();

            Model.States = State.Select(a => new SchemeState()
            {

                State = a.State,
                StateID = a.StateID
            }).ToList();

            Model.Customers = Customer.Select(a => new SchemeCustomer()
            {
                Customer = a.Customer,
                CustomerID = a.CustomerID
            }).ToList();

            Model.CustomerCategory = CustomerCategory.Select(a => new SchemeCustomerCategory()
            {
                CustomerCategory = a.CustomerCategory,
                CustomerCategoryID = a.CustomerCategoryID
            }).ToList();
            return View(Model);

        }

        // GET: Masters/SchemeAllocation/Create
        public ActionResult Create()
        {
            SchemeAllocationModel Model = new SchemeAllocationModel();
            Model.Code = generalBL.GetSerialNo("Scheme", "Code");
            List<CategoryBO> ItemCategoryList = categoryBL.GetItemCategoryForSales();
            int DefaultCategoryID = ItemCategoryList.FirstOrDefault().ID;
            Model.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(DefaultCategoryID), "ID", "Name");
            List<SchemeCustomerCategoryBO> CustomerCategoryList = schemeBL.GetSchemeCategoryList(0).ToList();
            Model.CustomerCategory = CustomerCategoryList.Select(a => new SchemeCustomerCategory()
            {
                CustomerCategory = a.CustomerCategory,
                CustomerCategoryID = a.CustomerCategoryID,
                SchemeCategoryID = a.SchemeCategoryID
            }).ToList();
            Model.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            Model.States = new List<SchemeState>();
            Model.Districts = new List<SchemeDistrict>();
            Model.Items = new List<SchemeItemModel>();
            return View(Model);
        }

        public JsonResult GetCustomerCategory(int ID)
        {
            List<SchemeCustomerCategoryBO> Categories = schemeBL.GetSchemeCategoryList(ID).ToList();
            return Json(new { Status = "success", data = Categories }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetState(int ID)
        {
            List<SchemeStateBO> state = schemeBL.GetSchemeStateList(ID).ToList();
            return Json(new { Status = "success", data = state }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDistrict(int ID)
        {
            List<SchemeDistrictBO> district = schemeBL.GetSchemeDistrictList(ID).ToList();
            return Json(new { Status = "success", data = district }, JsonRequestBehavior.AllowGet);
        }

        // GET: Masters/SchemeAllocation/Edit/5
        public ActionResult Edit(int id)
        {

            SchemeAllocationBO BO = schemeBL.GetSchemeAllocationDetails((int)id);
            List<SchemeCustomerBO> Customer = schemeBL.GetSchemeCustomerList((int)id).ToList();
            List<SchemeStateBO> States = schemeBL.GetSchemeStateList((int)id).Where(a => a.CountryID == BO.CountryID).ToList();
            List<SchemeDistrictBO> Districts = schemeBL.GetSchemeDistrictList(id).Where(a => a.StateID == States.Where(b => b.SchemeStateID != 0).FirstOrDefault()?.StateID).ToList();
            List<SchemeItemBO> Item = schemeBL.GetSchemeItemList((int)id);
            List<SchemeCustomerCategoryBO> CustomerCategory = schemeBL.GetSchemeCategoryList((int)id).ToList();

            SchemeAllocationModel Model = new SchemeAllocationModel()
            {
                ID = BO.ID,
                Code = BO.Code,
                StartDate = BO.StartDate == null ? "" : General.FormatDate((DateTime)BO.StartDate),
                EndDate = BO.EndDate == null ? "" : General.FormatDate((DateTime)BO.EndDate),
                Scheme = BO.SchemeName,
                CountryID = BO.CountryID,
            };

            Model.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");

            Model.Items = Item.Select(a => new SchemeItemModel()
            {
                ID = a.ID,
                StartDate = a.StartDate == null ? "" : General.FormatDate((DateTime)a.StartDate),
                EndDate = a.EndDate == null ? "" : General.FormatDate((DateTime)a.EndDate),
                Item = a.Item,
                ItemID = a.ItemID,
                OfferItem = a.OfferItem,
                OfferItemID = a.OfferItemID,
                OfferQty = a.OfferQty,
                InvoiceQty = a.InvoiceQty,
                BusinessCategory = a.BusinessCategory,
                BusinessCategoryID = a.BusinessCategoryID,
                SalesCategory = a.SalesCategory,
                SalesCategoryID = a.SalesCategoryID,
                IsEnded = a.IsEnded
            }).ToList();

            Model.Customers = Customer.Select(a => new SchemeCustomer()
            {
                Customer = a.Customer,
                CustomerID = a.CustomerID
            }).ToList();

            List<CategoryBO> ItemCategoryList = categoryBL.GetItemCategoryForSales();
            int DefaultCategoryID = ItemCategoryList.FirstOrDefault().ID;
            Model.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(DefaultCategoryID), "ID", "Name");
            Model.BusinessCategoryList = new SelectList(categoryBL.GetBusinessCategoryList(DefaultCategoryID), "ID", "Name");

            Model.CustomerCategory = CustomerCategory.Select(a => new SchemeCustomerCategory()
            {
                CustomerCategoryID = a.CustomerCategoryID,
                CustomerCategory = a.CustomerCategory,
                SchemeCategoryID = a.SchemeCategoryID
            }).ToList();

            Model.States = States.Select(a => new SchemeState()
            {
                StateID = a.StateID,
                State = a.State,
                CountryID = a.CountryID,
                SchemeStateID = a.SchemeStateID
            }).ToList();

            Model.Districts = Districts.Select(a => new SchemeDistrict()
            {
                DistrictID = a.DistrictID,
                District = a.District,
                SchemeDistrictID = a.SchemeDistrictID,
                CountryID = a.CountryID,
                StateID = a.StateID
            }).ToList();

            Model.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");

            return View(Model);

        }

        public ActionResult Save(SchemeAllocationModel Model)
        {
            try
            {
                SchemeAllocationBO schemeAllocationBO = new SchemeAllocationBO();
                schemeAllocationBO.SchemeName = Model.Scheme;
                schemeAllocationBO.CountryID = Model.CountryID;
                schemeAllocationBO.ID = Model.ID;
                schemeAllocationBO.StartDate = General.ToDateTimeNull(Model.StartDate);
                schemeAllocationBO.EndDate = General.ToDateTimeNull(Model.EndDate);

                if (Model.Customers == null)
                {
                    Model.Customers = new List<SchemeCustomer>();
                }

                if (Model.States == null)
                {
                    Model.States = new List<SchemeState>();
                }

                if (Model.CustomerCategory == null)
                {
                    Model.CustomerCategory = new List<SchemeCustomerCategory>();
                }

                if (Model.Districts == null)
                {
                    Model.Districts = new List<SchemeDistrict>();
                }

                schemeAllocationBO.Items = Model.Items.Select(itm => new SchemeItemBO()
                {
                    ID = itm.ID,
                    ItemID = itm.ItemID,
                    OfferItemID = itm.OfferItemID,
                    SalesCategoryID = itm.SalesCategoryID,
                    BusinessCategoryID = itm.BusinessCategoryID,
                    InvoiceQty = itm.InvoiceQty,
                    OfferQty = itm.OfferQty,
                    IsNewItem = itm.IsNewItem,
                    StartDate = General.ToDateTimeNull(itm.StartDate),
                    EndDate = General.ToDateTimeNull(itm.EndDate),
                }).ToList();

                schemeAllocationBO.Customers = Model.Customers.Select(a => new SchemeCustomerBO()
                {
                    CustomerID = a.CustomerID
                }).ToList();

                schemeAllocationBO.States = Model.States.Select(a => new SchemeStateBO()
                {
                    StateID = a.StateID,
                    DistrictID = a.DistrictID,
                    CountryID = a.CountryID
                }).ToList();

                schemeAllocationBO.Districts = Model.Districts.Select(a => new SchemeDistrictBO()
                {
                    DistrictID = a.DistrictID,
                    StateID = a.StateID,
                    CountryID = a.CountryID
                }).ToList();

                schemeAllocationBO.CustomerCategory = Model.CustomerCategory.Select(a => new SchemeCustomerCategoryBO()
                {
                    CustomerCategoryID = a.CustomerCategoryID
                }).ToList();

                schemeBL.Save(schemeAllocationBO);
                return Json(new { Status = "success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSchemeAllocationList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string NameHint = Datatable.Columns[2].Search.Value;
                string CustomerNameHint = Datatable.Columns[3].Search.Value;
                string CustomerCategoryHint = Datatable.Columns[4].Search.Value;
                string CustomerStateHint = Datatable.Columns[5].Search.Value;
                string CustomerDistrictHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = schemeBL.GetSchemeAllocationList(CodeHint, NameHint, CustomerNameHint, CustomerCategoryHint, CustomerStateHint, CustomerDistrictHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSchemeAllocationTransList(DatatableModel Datatable)
        {
            try
            {
                string NameHint = Datatable.Columns[1].Search.Value;
                string SalesCategoryHint = Datatable.Columns[2].Search.Value;
                string InvoiceQty = Datatable.Columns[3].Search.Value;
                string OfferQty = Datatable.Columns[4].Search.Value;
                string StartDate = Datatable.Columns[5].Search.Value;
                string EndDate = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int ID = Convert.ToInt32(Datatable.GetValueFromKey("ID", Datatable.Params));
                DatatableResultBO resultBO = schemeBL.GetSchemeAllocationTransList(ID, NameHint, SalesCategoryHint, StartDate, EndDate, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
