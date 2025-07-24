using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;


namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class DiscountController : Controller
    {
        private ICategoryContract categroyBL;
        private ICustomerContract customerBL;
        private IStateContract stateBL;
        private IDiscountContract discountBL;
        // GET: Masters/Discount/Index   
        public DiscountController()
        {
            stateBL = new StateBL();
            customerBL = new CustomerBL();
            categroyBL = new CategoryBL();
            discountBL = new DiscountBL();
        }
        public ActionResult Index()
        {
            List<DiscountModel> discountList = new List<DiscountModel>();
            return View(discountList);
        }

        // GET:Masters/Discount/Create
        public ActionResult Create()
        {
            DiscountModel discountModel = new DiscountModel();
            discountModel.SalesCategoryList = new SelectList(categroyBL.GetSalesCategoryList(222), "ID", "Name");
            discountModel.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            discountModel.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
            discountModel.SalesIncentiveCategoryList = new SelectList(categroyBL.GetSalesIncentiveCategoryList(222), "ID", "Name");
            discountModel.CustomerStateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            discountModel.DiscountPercentageList = categroyBL.GetDiscountCategory().Select(item => new CategoryBO()
            {
                ID = item.ID,
                Name = item.Name,
                Value = item.Value
            }).ToList();
            return View(discountModel);
        }
        [HttpPost]
        public JsonResult GetDiscountDetails(int ItemID = 0, int CustomerID = 0, int CustomerCategoryID = 0, int CustomerStateID = 0, int BusinessCategoryID = 0, int SalesIncentiveCategoryID = 0, int SalesCategoryID = 0)
        {
            try
            {
                List<DiscountBO> Discountlist = discountBL.GetDiscountDetails(ItemID, CustomerID, CustomerCategoryID, CustomerStateID, BusinessCategoryID, SalesIncentiveCategoryID, SalesCategoryID);
                return Json(new { Status = "success", Data = Discountlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Save(DiscountModel discountModel)
        {
            try
            {
                List<DiscountBO> DiscountDetails = new List<DiscountBO>();
                DiscountBO DiscountDetail;
                foreach (var item in discountModel.DiscountDetails)
                {
                    DiscountDetail = new DiscountBO()
                    {
                        ID= item.ID,
                        Code=item.Code,
                        ItemID=item.ItemID,
                        CustomerID=item.CustomerID,
                        CustomerCategoryID=item.CustomerCategoryID,
                        CustomerStateID=item.CustomerStateID,
                        BusinessCategoryID=item.BusinessCategoryID,
                        SalesIncentiveCategoryID=item.SalesIncentiveCategoryID,
                        SalesCategoryID=item.SalesCategoryID,
                        DiscountCategoryID=item.DiscountCategoryID,
                        DiscountPercentage=item.DiscountPercentage
                         };
                         DiscountDetails.Add(DiscountDetail);
                        }
                         discountBL.Save(DiscountDetails);

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
                return
                      Json(new
                      {
                          Status = "",
                          data = "",
                          message = e.Message
                      }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetDiscountList(DatatableModel Datatable)

        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string NameHint = Datatable.Columns[2].Search.Value;
                string CustomerNameHint = Datatable.Columns[3].Search.Value;
                string CustomerCategoryHint = Datatable.Columns[4].Search.Value;
                string StateHint = Datatable.Columns[5].Search.Value;
                string BusinessCategoryHint = Datatable.Columns[6].Search.Value;
                string SalesIncentiveCategoryHint = Datatable.Columns[7].Search.Value;
                string SalesCategoryHint = Datatable.Columns[8].Search.Value;
                string DiscountCategoryHint = Datatable.Columns[9].Search.Value;
                string DiscountPercentageHint = Datatable.Columns[10].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = discountBL.GetDiscountList(CodeHint, NameHint, CustomerNameHint,  CustomerCategoryHint,  StateHint,  BusinessCategoryHint, SalesIncentiveCategoryHint, SalesCategoryHint, DiscountCategoryHint, DiscountPercentageHint, SortField,  SortOrder,  Offset,  Limit);
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