using AutoMapper;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class SLAController : Controller
    {
        private ISLAContract iSLABL;
        ICategoryContract categoryBL;
        IGSTCategoryContract GSTCategoryBL;

        #region Constructors

        public SLAController()
        {
            iSLABL = new SLABL();
            categoryBL = new CategoryBL();
            GSTCategoryBL = new GSTCategoryBL();
        }
        #endregion
        // GET: Masters/SLA
        public ActionResult Index()
        {
            SLAModel sLAModel = new SLAModel();
            sLAModel.ProcessCycleList = new SelectList(iSLABL.GetSLAFilterByType("Cycle"), "SLAFilter", "SLAFilter");
            sLAModel.TransactionTypeList = new SelectList(iSLABL.GetSLAFilterByType("TransactionType"), "SLAFilter", "SLAFilter");
            sLAModel.SLAKeyValueList = new SelectList(iSLABL.GetSLAFilterByType("KeyValue"), "SLAFilter", "SLAFilter");
            sLAModel.ItemList = new SelectList(iSLABL.GetSLAFilterByType("Item"), "SLAFilter", "SLAFilter");
            sLAModel.SupplierList = new SelectList(iSLABL.GetSLAFilterByType("Supplier"), "SLAFilter", "SLAFilter");
            sLAModel.CustomerList = new SelectList(iSLABL.GetSLAFilterByType("Customer"), "SLAFilter", "SLAFilter");
            return View(sLAModel);
        }
        // GET: Masters/SLA/Details/5
        public ActionResult Details(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SLABO, SLAModel>().ForMember(d => d.ID, o => o.Ignore());
            });

            IMapper iMapper = config.CreateMapper();
            SLAModel sLAModel = iMapper.Map<SLABO, SLAModel>(iSLABL.GetSLADetails(id));
            sLAModel.ID = id;
            return View(sLAModel);
        }

        // GET: Masters/SLA/Create
        public ActionResult Create()
        {
            SLAModel sLAModel = new SLAModel();
            List<CategoryBO> CommonOptions = new List<CategoryBO>() {
                   new CategoryBO() {Name = "ALL", ID = 0  },
                   new CategoryBO() {Name = "No", ID = -1 },
            };
            List<GSTCategoryBO> CommonGSTOptions = new List<GSTCategoryBO>() {
                   new GSTCategoryBO() {Name = "ALL", ID = 0  },
                   new GSTCategoryBO() {Name = "No", ID = -1 },
            };

            List<CategoryBO> SuppliersSubTaxCategoryGroup = categoryBL.GetSuppliersSubTaxCategoryGroup();
            List<CategoryBO> CustomerTaxCategoryList = categoryBL.GetCustomerTaxCategoryList();
            List<CategoryBO> ItemAccountCategoryList = categoryBL.GetAccountsCategoryList();
            List<GSTCategoryBO> GSTCategoryList = GSTCategoryBL.GetGSTCategoryList();
            List<CategoryBO> SuppliersAccountCategoryList = categoryBL.GetSuppliersAccountCategoryGroup();
            List<CategoryBO> CustomerAccountsCategoryList = categoryBL.GetCustomerAccountsCategoryList();
            List<CategoryBO> CustomerCategoryList = categoryBL.GetCustomerCategoryList();

            SuppliersSubTaxCategoryGroup.InsertRange(0, CommonOptions);
            CustomerTaxCategoryList.InsertRange(0, CommonOptions);
            ItemAccountCategoryList.InsertRange(0, CommonOptions);
            GSTCategoryList.InsertRange(0, CommonGSTOptions);
            SuppliersAccountCategoryList.InsertRange(0, CommonOptions);
            CustomerAccountsCategoryList.InsertRange(0, CommonOptions);
            CustomerCategoryList.InsertRange(0, CommonOptions);

            sLAModel.ItemAccountCategoryList = new SelectList(ItemAccountCategoryList, "ID", "Name");
            sLAModel.GSTCategoryList = new SelectList(GSTCategoryList, "ID", "Name");
            sLAModel.CustomerAccountsCategoryList = new SelectList(CustomerAccountsCategoryList, "ID", "Name");
            sLAModel.CustomerCategoryList = new SelectList(CustomerCategoryList, "ID", "Name");
            sLAModel.CustomerTaxCategoryList = new SelectList(CustomerTaxCategoryList, "ID", "Name");
            sLAModel.SuppliersAccountCategoryList = new SelectList(SuppliersAccountCategoryList, "ID", "Name");
            sLAModel.TransactionTypeList = new SelectList(iSLABL.GetSLAFilterByType("TransactionType"), "SLAFilter", "SLAFilter");
            sLAModel.SLAKeyValueLists = new SelectList(iSLABL.GetSLAFilterByType("KeyValue"), "SLAFilter", "SLAFilter");
            sLAModel.ProcessCycleList = new SelectList(iSLABL.GetProcessCycleList(), "ProcessCycle", "ProcessCycle");
            sLAModel.SuppliersTaxSubCategoryList = new SelectList(SuppliersSubTaxCategoryGroup, "ID", "Name");
            //sLAModel.StartDate = DateTime.Now;
            //sLAModel.enddate = DateTime.Now.AddDays(365);
            return View(sLAModel);
        }

        // POST: Masters/SLA/Create
        [HttpPost]
        public ActionResult Save(SLAModel model)
        {
            try
            {

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<SLAModel, SLABO>();
                });
                IMapper iMapper = config.CreateMapper();
                var SLAData = iMapper.Map<SLAModel, SLABO>(model);
                SLAData.Startdate = General.ToDateTime(model.StartDateStr);
                SLAData.enddate = General.ToDateTime(model.EndDateStr);
                var result = iSLABL.CreateSLA(SLAData);
                if (result != 0)
                {
                    return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = 409, data = model }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Masters/SLA/Edit/5
        public ActionResult Edit(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SLABO, SLAModel>().ForMember(d => d.ID, o => o.Ignore());
            });
            IMapper iMapper = config.CreateMapper();
            SLAModel sLAModel = iMapper.Map<SLABO, SLAModel>(iSLABL.GetSLADetails(id));

            List<CategoryBO> CommonOptions = new List<CategoryBO>() {
                   new CategoryBO() {Name = "ALL", ID = 0  },
                   new CategoryBO() {Name = "No", ID = -1 },
            };
            List<GSTCategoryBO> CommonGSTOptions = new List<GSTCategoryBO>() {
                   new GSTCategoryBO() {Name = "ALL", ID = 0  },
                   new GSTCategoryBO() {Name = "No", ID = -1 },
            };

            List<CategoryBO> SuppliersSubTaxCategoryGroup = categoryBL.GetSuppliersSubTaxCategoryGroup();
            List<CategoryBO> CustomerTaxCategoryList = categoryBL.GetCustomerTaxCategoryList();
            List<CategoryBO> ItemAccountCategoryList = categoryBL.GetAccountsCategoryList();
            List<GSTCategoryBO> GSTCategoryList = GSTCategoryBL.GetGSTCategoryList();
            List<CategoryBO> SuppliersAccountCategoryList = categoryBL.GetSuppliersAccountCategoryGroup();
            List<CategoryBO> CustomerAccountsCategoryList = categoryBL.GetCustomerAccountsCategoryList();
            List<CategoryBO> CustomerCategoryList = categoryBL.GetCustomerCategoryList();

            SuppliersSubTaxCategoryGroup.InsertRange(0, CommonOptions);
            CustomerTaxCategoryList.InsertRange(0, CommonOptions);
            ItemAccountCategoryList.InsertRange(0, CommonOptions);
            GSTCategoryList.InsertRange(0, CommonGSTOptions);
            SuppliersAccountCategoryList.InsertRange(0, CommonOptions);
            CustomerAccountsCategoryList.InsertRange(0, CommonOptions);
            CustomerCategoryList.InsertRange(0, CommonOptions);

            sLAModel.ID = id;
            sLAModel.ItemAccountCategoryList = new SelectList(ItemAccountCategoryList, "ID", "Name");
            sLAModel.GSTCategoryList = new SelectList(GSTCategoryList, "ID", "Name");
            sLAModel.CustomerAccountsCategoryList = new SelectList(CustomerAccountsCategoryList, "ID", "Name");
            sLAModel.CustomerCategoryList = new SelectList(CustomerCategoryList, "ID", "Name");
            sLAModel.CustomerTaxCategoryList = new SelectList(CustomerTaxCategoryList, "ID", "Name");
            sLAModel.SuppliersAccountCategoryList = new SelectList(SuppliersAccountCategoryList, "ID", "Name");
            sLAModel.ProcessCycleList = new SelectList(iSLABL.GetProcessCycleList(), "ProcessCycle", "ProcessCycle");
            sLAModel.SuppliersTaxSubCategoryList = new SelectList(SuppliersSubTaxCategoryGroup, "ID", "Name");
            sLAModel.TransactionTypeList = new SelectList(iSLABL.GetTransactionType(sLAModel.Cycle), "TransactionType", "TransactionType");
            sLAModel.SLAKeyValueLists = new SelectList(iSLABL.GetSLAKeyValueByTransactionType(sLAModel.TransactionType), "KeyValue", "KeyValue");
            sLAModel.ItemID = sLAModel.Item;
            sLAModel.SupplierID = sLAModel.Supplier;
            sLAModel.CustomerID = sLAModel.Customer;
            sLAModel.ItemTaxCategory = sLAModel.ItemTaxCategory;
            sLAModel.ItemTaxCategoryName = sLAModel.ItemTaxCategoryName;
            sLAModel.ItemAccountsCategory = sLAModel.ItemAccountsCategory;
            sLAModel.ItemAccountsCategoryName = sLAModel.ItemAccountsCategoryName;
            sLAModel.SupplierAccountsCategory = sLAModel.SupplierAccountsCategory;
            sLAModel.SupplierAccountsCategoryName = sLAModel.SupplierAccountsCategoryName;
            sLAModel.SupplierTaxSubCategory = sLAModel.SupplierTaxSubCategory;
            sLAModel.SupplierTaxSubCategoryName = sLAModel.SupplierTaxSubCategoryName;
            sLAModel.CustomerCategory = sLAModel.CustomerCategory;
            sLAModel.CustomerCategoryName = sLAModel.CustomerCategory;
            sLAModel.CustomerAccountsCategoryName = sLAModel.CustomerAccountsCategoryName;
            sLAModel.CustomerAccountsCategory = sLAModel.CustomerAccountsCategory;
            sLAModel.Item = sLAModel.ItemName;
            sLAModel.Supplier = sLAModel.SupplierName;
            sLAModel.Customer = sLAModel.CustomerName;
            sLAModel.Cycle = sLAModel.Cycle;
            sLAModel.TransactionType = sLAModel.TransactionType;
            sLAModel.KeyValue = sLAModel.KeyValue;
            sLAModel.StartDateStr = General.FormatDateTime(sLAModel.Startdate);
            sLAModel.EndDateStr = General.FormatDateTime(sLAModel.enddate);
            sLAModel.DebitAccountID = sLAModel.DebitAccountID;
            sLAModel.CreditAccountID = sLAModel.CreditAccountID;
            return View(sLAModel);
        }


        public JsonResult GetSLAKeyValueByTransactionType(string TransactionType)
        {
            List<SLABO> SLAKeyValueList = iSLABL.GetSLAKeyValueByTransactionType(TransactionType).ToList();
            return Json(new { Status = "success", data = SLAKeyValueList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransactionTypeByProcessCycle(string ProcessCycle)
        {
            List<SLABO> TransactionTypeList = iSLABL.GetTransactionType(ProcessCycle).ToList();
            return Json(new { Status = "success", data = TransactionTypeList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllSLAList(DatatableModel Datatable)
        {
            try
            {
                string CycleHint = Datatable.Columns[1].Search.Value;
                string TransactionTypeHint = Datatable.Columns[2].Search.Value;
                string KeyValueHint = Datatable.Columns[3].Search.Value;
                string ItemHint = Datatable.Columns[4].Search.Value;
                string SupplierHint = Datatable.Columns[5].Search.Value;
                string CustomerHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                DatatableResultBO resultBO = iSLABL.GetAllSLAList(CycleHint, TransactionTypeHint, KeyValueHint, ItemHint, SupplierHint, CustomerHint, SortField, SortOrder, Offset, Limit);
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