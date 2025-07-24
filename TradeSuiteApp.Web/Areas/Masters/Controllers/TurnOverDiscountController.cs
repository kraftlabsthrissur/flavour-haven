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
    public class TurnOverDiscountController : Controller
    {
        private ILocationContract locationBL;
        private ITurnOverDiscountContract discountBL;
        private IAttendanceContract atendanceBL;

        public TurnOverDiscountController()
        {
            locationBL = new LocationBL();
            discountBL = new TurnOverDiscountBL();
            atendanceBL = new AttendanceBL();
        }

        public ActionResult Index()
        {
            List<TurnOverDiscountModel> model = new List<TurnOverDiscountModel>();
            model = discountBL.GetTurnOverDiscountList().Select(a => new TurnOverDiscountModel
            {
                ID = a.ID,
                Date = a.Date

            }).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            TurnOverDiscountModel model = new TurnOverDiscountModel();
            model.Date = General.FormatDate(DateTime.Now);
            model.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            model.FromDate = General.FormatDate(DateTime.Now);
            model.MonthList = new SelectList(atendanceBL.GetMonthList(), "ID", "Name");
            return View(model);
        }

        public JsonResult ReadExcel(string Path)
        {
            try
            {
                TurnOverDiscountModel turnOverDiscountModel = new TurnOverDiscountModel();
                List<DiscountItemBO> DiscoundList = discountBL.ReadExcel(Path);
                DiscountItemModel discountItemModel;
                turnOverDiscountModel.Items = new List<DiscountItemModel>();              
                    foreach (var m in DiscoundList)
                    {
                        if (m.TurnOverDiscount > 0)
                        {
                            discountItemModel = new DiscountItemModel()
                            {
                                ID = m.ID,
                                Code = m.Code,
                                CustomerName = m.CustomerName,
                                TurnOverDiscount = m.TurnOverDiscount,
                                FromDate = General.FormatDate(m.FromDate),
                                ToDate = General.FormatDate(m.ToDate),
                                Month = m.Month,
                                Location = m.Location
                            };
                            turnOverDiscountModel.Items.Add(discountItemModel);
                        }
                   }
                    return Json(new { Status = "success", Data = turnOverDiscountModel.Items }, JsonRequestBehavior.AllowGet);
               
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCustomerListForLocation(DatatableModel Datatable)
        {
            try
            {
                int CustomerLocationID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerLocationID", Datatable.Params));

                DatatableResultBO resultBO = discountBL.GetCustomerListForLocation(CustomerLocationID, Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[4].Search.Value, Datatable.Columns[5].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Save(TurnOverDiscountModel model)
        {
            try
            {
                TurnOverDiscountsBO turnOverDiscountBO = new TurnOverDiscountsBO()
                {
                    ID = model.ID,
                    Date = model.Date,

                };
                List<DiscountItemBO> Items = new List<DiscountItemBO>();
                if (model.Items != null)
                {
                    DiscountItemBO DiscountItem;
                    foreach (var item in model.Items)
                    {
                        DiscountItem = new DiscountItemBO()
                        {
                            Code = item.Code,
                            CustomerID = item.CustomerID,
                            CustomerName = item.CustomerName,
                            TurnOverDiscount = item.TurnOverDiscount,
                            FromDate = General.ToDateTime(item.FromDate),
                            ToDate = General.ToDateTime(item.ToDate),
                            Location = item.Location,
                            Month = item.Month
                        };
                        Items.Add(DiscountItem);
                    }
                }
                discountBL.Save(Items, turnOverDiscountBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int ID)
        {
            TurnOverDiscountModel model = discountBL.GetTurnOverDiscountDetails(ID).Select(m => new TurnOverDiscountModel()
            {
                ID = m.ID,
                Date = m.Date
            }).First();

            model.Items = discountBL.GetTurnOverDiscountTransDetails(ID).Select(m => new DiscountItemModel()
            {
                CustomerID = m.CustomerID,
                CustomerName = m.CustomerName,
                TurnOverDiscount = m.TurnOverDiscount,
                FromDate = General.FormatDate(m.FromDate),
                ToDate = General.FormatDate(m.ToDate),
                Location = m.Location,
                Code = m.Code,
                Month = m.Month
            }).ToList();
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            TurnOverDiscountModel model = discountBL.GetTurnOverDiscountDetails(ID).Select(m => new TurnOverDiscountModel()
            {
                ID = m.ID,
                Date = m.Date,
                FromDate = General.FormatDate(DateTime.Now)
            }).First();
            model.LocationID = GeneralBO.LocationID;
            model.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            model.MonthList = new SelectList(atendanceBL.GetMonthList(), "ID", "Name");
            model.Items = discountBL.GetTurnOverDiscountTransDetails(ID).Select(m => new DiscountItemModel()
            {
                CustomerID = m.CustomerID,
                CustomerName = m.CustomerName,
                TurnOverDiscount = m.TurnOverDiscount,
                FromDate = General.FormatDate(m.FromDate),
                ToDate = General.FormatDate(m.ToDate),
                Location = m.Location,
                Code = m.Code,
                Month = m.Month
            }).ToList();
            return View(model);
        }
    }
}