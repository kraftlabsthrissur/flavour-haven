using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.User.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.User.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class DashboardController : Controller
    {
        private IDashBoardContract dashBoardBL;
        private ICategoryContract categoryBL;
        private ICounterSalesContract counterSalesBL;
        private IGeneralContract generalBL;

        public DashboardController()
        {
            dashBoardBL = new DashBoardBL();
            counterSalesBL = new CounterSalesBL();
            categoryBL = new CategoryBL();
            generalBL = new GeneralBL();
        }
        // GET: User/Dashboard
        public ActionResult Index()
        {
            DashboardModel model = new DashboardModel();
            model.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(222), "ID", "Name");
            model.BatchTypeList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "ALL", Value ="ALL", },
                                                 new SelectListItem { Text = "ISK", Value = "ISK"},
                                                 new SelectListItem { Text = "OSK", Value = "OSK"},
                                                 }, "Value", "Text");
            model.LocationList = dashBoardBL.GetSalesByLocation(DateTime.Today, 0).Select(a => new SalesByLocationModel()
            {
                LocationCode = a.LocationCode,
                PreviousMonthAmount = a.PreviousMonthAmount,
                LocationName = a.LocationName,
                Amount = a.Amount
            }).ToList();
            return View(model);
        }

        public async Task<JsonResult> GetMonthWiseSalesSummary()
        {
            try
            {

                SalesSummaryModel salessummary = new SalesSummaryModel();
                salessummary.XaxisNames = new List<XaxisNameModel>();
                salessummary.SalesSummaryValues = new List<SalesSummaryValueModel>();
                List<SalesSummaryBO> dashboardBO = dashBoardBL.GetMonthWiseSalesSummary();
                XaxisNameModel XaxisName;
                foreach (var m in dashboardBO)
                {
                    XaxisName = new XaxisNameModel()
                    {
                        Name = m.XaxisName
                    };
                    salessummary.XaxisNames.Add(XaxisName);
                }
                SalesSummaryValueModel value;
                foreach (var m in dashboardBO)
                {
                    value = new SalesSummaryValueModel()
                    {
                        BudgetValue = m.BudgetValue,
                        ActualValue = m.ActualValue,
                        ActualSalesInKgValue = m.ActualSalesInKgValue,
                        ActualSalesRevenue = m.ActualSalesRevenue,
                        BudgetGrossRevenue = m.BudgetGrossRevenue,
                        BudgetInKgValue = m.BudgetInKgValue,
                    };
                    salessummary.SalesSummaryValues.Add(value);
                }

                return Json(new { Status = "success", Data = salessummary }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> GetYearWiseSalesSummary()
        {
            try
            {

                SalesSummaryModel salessummary = new SalesSummaryModel();
                salessummary.XaxisNames = new List<XaxisNameModel>();
                salessummary.SalesSummaryValues = new List<SalesSummaryValueModel>();
                List<SalesSummaryBO> dashboardBO = dashBoardBL.GetActualAndBudgetSalesDetailsForYear();
                XaxisNameModel XaxisName;
                foreach (var m in dashboardBO)
                {
                    XaxisName = new XaxisNameModel()
                    {
                        Name = m.XaxisName
                    };
                    salessummary.XaxisNames.Add(XaxisName);
                }
                SalesSummaryValueModel value;
                foreach (var m in dashboardBO)
                {
                    value = new SalesSummaryValueModel()
                    {
                        BudgetValue = m.BudgetValue,
                        ActualValue = m.ActualValue,
                        ActualSalesInKgValue = m.ActualSalesInKgValue,
                        ActualSalesRevenue = m.ActualSalesRevenue,
                        BudgetGrossRevenue = m.BudgetGrossRevenue,
                        BudgetInKgValue = m.BudgetInKgValue
                    };
                    salessummary.SalesSummaryValues.Add(value);
                }

                return Json(new { Status = "success", Data = salessummary }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> GetSalesAmountByDayWise()
        {
            try
            {
                List<DateWiseSalesModel> DateWiseSales = dashBoardBL.GetSalesAmtByDayWise().Select(a => new DateWiseSalesModel()
                {
                    Date = a.Date,
                    Value = a.Value
                }).ToList();
                return Json(new { Status = "success", Data = DateWiseSales }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> GetCategoryWiseSalesWithLocationHead()
        {
            try
            {
                List<SalesSummaryBO> dashboardBO = dashBoardBL.GetCategoryWiseSalesWithLocationHead();
                return Json(new { Status = "success", Data = dashboardBO }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> GetBusinessCategoryWiseStock()
        {
            try
            {
                List<CategoryWiseStockBO> dashboardBO = dashBoardBL.GetBusinessCategoryWiseStock();
                return Json(new { Status = "success", Data = dashboardBO }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> GetTotalProductionInYear()
        {
            try
            {
                ProductionSummaryModel productionsummary = new ProductionSummaryModel();
                productionsummary.XaxisNames = new List<XaxisNameModel>();
                productionsummary.TotalProductionQty = new List<ProductionQtyModel>();
                List<ProductionQtyBO> productionQtyBO = dashBoardBL.GetTotalProductionInYear();
                XaxisNameModel XaxisName;
                foreach (var m in productionQtyBO)
                {
                    XaxisName = new XaxisNameModel()
                    {
                        Name = m.LocationHead
                    };
                    productionsummary.XaxisNames.Add(XaxisName);
                }
                ProductionQtyModel value;
                foreach (var m in productionQtyBO)
                {
                    value = new ProductionQtyModel()
                    {
                        TotalProduction = m.TotalProduction,
                    };
                    productionsummary.TotalProductionQty.Add(value);
                }

                return Json(new { Status = "success", Data = productionsummary }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> GetTotalProductionInMonth()
        {
            try
            {

                ProductionSummaryModel productionsummary = new ProductionSummaryModel();
                productionsummary.XaxisNames = new List<XaxisNameModel>();
                productionsummary.TotalProductionQty = new List<ProductionQtyModel>();
                List<ProductionQtyBO> productionQtyBO = dashBoardBL.GetProductionOutputInMonth();
                XaxisNameModel XaxisName;
                foreach (var m in productionQtyBO)
                {
                    XaxisName = new XaxisNameModel()
                    {
                        Name = m.LocationHead
                    };
                    productionsummary.XaxisNames.Add(XaxisName);
                }
                ProductionQtyModel value;
                foreach (var m in productionQtyBO)
                {
                    value = new ProductionQtyModel()
                    {
                        TotalProduction = m.TotalProduction,
                    };
                    productionsummary.TotalProductionQty.Add(value);
                }

                return Json(new { Status = "success", Data = productionsummary }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> GetMonthWiseSalesByLocationHead(string LocationHead, string Batchtype)
        {
            try
            {

                SalesSummaryModel salesSummaryModel = new SalesSummaryModel();
                salesSummaryModel.XaxisNames = new List<XaxisNameModel>();
                salesSummaryModel.SalesSummaryValues = new List<SalesSummaryValueModel>();
                List<SalesSummaryBO> salesSummaryBO = dashBoardBL.GetMonthWiseSalesByLocationHead(LocationHead, Batchtype);
                XaxisNameModel XaxisName;
                int year = salesSummaryBO.Count > 0 ? salesSummaryBO.FirstOrDefault().Year : 0;
                foreach (var m in salesSummaryBO)
                {
                    if (year != m.Year)
                    {
                        year = m.Year;
                        XaxisName = new XaxisNameModel()
                        {
                            Name = m.XaxisName + " " + year.ToString()
                        };
                    }
                    else
                    {
                        XaxisName = new XaxisNameModel()
                        {
                            Name = m.XaxisName
                        };
                    }
                    salesSummaryModel.XaxisNames.Add(XaxisName);
                }
                SalesSummaryValueModel value;
                foreach (var m in salesSummaryBO)
                {
                    value = new SalesSummaryValueModel()
                    {
                        BudgetValue = m.BudgetValue,
                        ActualValue = m.ActualValue,
                        ActualSalesInKgValue = m.ActualSalesInKgValue,
                        ActualSalesRevenue = m.ActualSalesRevenue,
                        BudgetGrossRevenue = m.BudgetGrossRevenue,
                        BudgetInKgValue = m.BudgetInKgValue,
                        PrevoiusYearGrossRevenue = m.PrevoiusYearGrossRevenue,
                        PrevoiusYearSalesInKgValue = m.PrevoiusYearSalesInKgValue,
                        PrevoiusYearSalesValue = m.PrevoiusYearSalesValue
                    };
                    salesSummaryModel.SalesSummaryValues.Add(value);
                }

                return Json(new { Status = "success", Data = salesSummaryModel }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> GetMonthWiseProduction()
        {
            try
            {
                ProductionSummaryModel productionsummary = new ProductionSummaryModel();
                productionsummary.XaxisNames = new List<XaxisNameModel>();
                productionsummary.MonthWiseProductionQty = new List<MonthWiseProductionQtyModel>();
                List<ProductionQtyMonthWiseBO> dashboardBO = dashBoardBL.GetProductionOutputMonthWise();
                XaxisNameModel XaxisName;
                int year = dashboardBO.FirstOrDefault().Year;
                foreach (var m in dashboardBO)
                {
                    if (year != m.Year)
                    {
                        year = m.Year;
                        XaxisName = new XaxisNameModel()
                        {
                            Name = m.Month + " " + year.ToString()
                        };
                    }
                    else
                    {
                        XaxisName = new XaxisNameModel()
                        {
                            Name = m.Month
                        };
                    }

                    productionsummary.XaxisNames.Add(XaxisName);
                }
                MonthWiseProductionQtyModel value;
                foreach (var m in dashboardBO)
                {
                    value = new MonthWiseProductionQtyModel()
                    {
                        TotalProductionInCHU = m.TotalProductionInCHU,
                        TotalProductionInPOL = m.TotalProductionInPOL,
                        TotalProductionInTKY = m.TotalProductionInTKY

                    };
                    productionsummary.MonthWiseProductionQty.Add(value);
                }

                return Json(new { Status = "success", Data = productionsummary }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    Status = "failure",
                    Message = e.Message
                }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> GetLocationWiseSales(string Type, string Batchtype)
        {
            try
            {
                List<SalesSummaryBO> Sales = dashBoardBL.GetLocationWiseSales(Type, Batchtype);
                return Json(new { Status = "success", Data = Sales }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> GetCategoryWiseSales(string Type, string Batchtype)
        {
            try
            {
                List<SalesSummaryBO> Sales = dashBoardBL.GetCategoryWiseSales(Type, Batchtype);
                return Json(new { Status = "success", Data = Sales }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> GetProductionQtyInProgress(string Type, int SalesCategoryID = 0)
        {
            try
            {
                ProductionSummaryModel productionsummary = new ProductionSummaryModel();
                List<ProductionProgressBO> production = dashBoardBL.GetProductionQtyInProgress(Type, SalesCategoryID);
                ProductionProgressModel productions;
                productionsummary.ProductionInProgress = new List<ProductionProgressModel>();
                foreach (var m in production)
                {
                    productions = new ProductionProgressModel()
                    {
                        ExpectedDate = m.ExpectedDate == null ? "" : General.FormatDate((DateTime)m.ExpectedDate),
                        ItemName = m.ItemName,
                        StandardOutput = m.StandardOutput,
                        BatchNo = m.BatchNo
                    };
                    productionsummary.ProductionInProgress.Add(productions);
                }
                return Json(new { Status = "success", Data = productionsummary.ProductionInProgress }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> SalesTickers()
        {
            List<SalesByLocationModel> sales = dashBoardBL.GetSalesByLocation(DateTime.Today, 0).Select(a => new SalesByLocationModel()
            {
                LocationCode = a.LocationCode,
                LocationType = a.LocationType,
                LocationName = a.LocationName,
                Amount = a.Amount,
                PreviousMonthAmount = a.PreviousMonthAmount,
                Budget = a.Budget
            }).ToList();
            int[] UserIDs = generalBL.GetUsersWithPermission("user", "dashboard", "tikers", "tab");

            string html = RenderRazorViewToString("Tickers", sales);
            return Json(new { Status = "success", Html = html, UserIDs = UserIDs }, JsonRequestBehavior.AllowGet);
        }

        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}