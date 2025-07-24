using BusinessLayer;
using BusinessObject;
using DataAccessLayer;
using DataAccessLayer.DBContext;
using Microsoft.Reporting.WebForms;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Reports.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Reports.Controllers
{
    public class ManufacturingController : BaseReportController
    {
        private ICategoryContract categoryBL;
        private IProductionSchedule _productionSchedule;
        private IPreprocessIssue _preprocessIssue;
        private IStatusContract statusBL;
        private IBatchTypeContract batchTypeBL;
        private ReportsEntities RptEntity;
        private DateTime StartDate, EndDate, PSBatchDateFrom, PSBatchDateTo, ReceiptDateFrom, ReceiptDateTo;
        private IReportContract reportBL;
        private SelectList AtoZRange = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());
        ProductionScheduleRepository ProductionScheduleDal;
        public ManufacturingController(IPreprocessIssue preprocessIssue, IProductionSchedule iproductionSchedule)
        {
            RptEntity = new ReportsEntities();
            reportBL = new ReportBL();
            categoryBL = new CategoryBL();
            statusBL = new StatusBL();
            batchTypeBL = new BatchTypeBL();
            _preprocessIssue = preprocessIssue;
            ViewBag.FinStartDate = GeneralBO.FinStartDate;
            ViewBag.CurrentDate = General.FormatDate(DateTime.Now);
            _productionSchedule = iproductionSchedule;
            ProductionScheduleDal = new ProductionScheduleRepository();
        }

        // GET: Reports/Manufacturing
        public ActionResult Index()
        {
            return View();
        }

        // GET: Reports/Manufacturing/Production
        public ActionResult ProductionStdCostEndItem()
        {
            ManufacturingModel rep = new ManufacturingModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.FromOutputItemNameRangeList = AtoZRange;
            rep.ToOutputItemNameRangeList = AtoZRange;
            rep.FromInputItemNameRangeList = AtoZRange;
            rep.ToInputItemNameRangeList = AtoZRange;
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        [HttpPost]
        public ActionResult ProductionStdCostEndItem(ManufacturingModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Standard Cost" + " " + model.StdCost + " " + model.Summary);
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);

            if (model.StdCost == "EndItem")
            {
                var Production = RptEntity.SpRptProductionStdCostEndItemSummary(
                                            StartDate,
                                            EndDate,
                                            model.FromOutputItemNameRange,
                                            model.ToOutputItemNameRange,
                                            model.ItemCodeFromID,
                                            model.ItemCodeToID,
                                            model.OuputItemNameID,
                                            model.CategoryFromID,
                                            model.CategoryToID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionStdCostEndItemSummary.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionStdCostEndItemSummaryDataset", Production));
            }
            else if (model.StdCost == "PurchaseItem")
            {
                var Production = RptEntity.SpRptProductionStdCostPurchaseItemSummary(
                                            StartDate,
                                            EndDate,
                                            model.FromInputItemNameRange,
                                            model.ToInputItemNameRange,
                                            model.InputItemCodeFromID,
                                            model.InputItemCodeToID,
                                            model.InputItemNameID,
                                            model.CategoryFromID,
                                            model.CategoryToID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionStdCostPurchaseItemSummary.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionStdCostPurchaseItemSummaryDataset", Production));
            }
            else
            {
                var Production = RptEntity.SpRptProductionStdCostComponent(
                                            StartDate, EndDate,
                                            model.FromOutputItemNameRange,
                                            model.ToOutputItemNameRange,
                                            model.ItemCodeFromID,
                                            model.ItemCodeToID,
                                            model.OuputItemNameID,
                                            model.CategoryFromID,
                                            model.CategoryToID,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionStdCostCostComponent.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionStdCostCostComponentDataset", Production));
            }
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        // GET: Reports/Manufacturing/ProductionAtGlance
        public ActionResult ProductionSchedule()
        {
            ManufacturingModel rep = new ManufacturingModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.PSBatchDateFrom = GeneralBO.FinStartDate;
            rep.PSBatchDateTo = General.FormatDate(DateTime.Now);
            rep.FromOutputItemCodeRangeList = AtoZRange;
            rep.ToOutputItemCodeRangeList = AtoZRange;
            rep.FromOutputItemNameRangeList = AtoZRange;
            rep.ToOutputItemNameRangeList = AtoZRange;
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        [HttpPost]
        public ActionResult ProductionSchedule(ManufacturingModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            if (model.PSBatchDateFrom != null)
                PSBatchDateFrom = General.ToDateTime(model.PSBatchDateFrom);
            if (model.PSBatchDateTo != null)
            {
                PSBatchDateTo = General.ToDateTime(model.PSBatchDateTo);
                PSBatchDateTo = PSBatchDateTo.AddDays(1);
            }
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Production Schedule Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);

            var ProductionSchedule = RptEntity.SpRptProductionSchedule(
                                                StartDate,
                                                EndDate,
                                                model.PSTransNoFromID,
                                                model.PSTransNoToID,
                                                PSBatchDateFrom, PSBatchDateTo,
                                                model.PSBatchNoFromID, model.PSBatchNoToID,
                                                model.ItemCodeFromID, model.ItemCodeToID,
                                                model.FromOutputItemNameRange,
                                                model.ToOutputItemNameRange,
                                                model.OuputItemNameID,
                                                model.ProductionGroupID,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionSchedule.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionScheduleDataSet", ProductionSchedule));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        // GET: Reports/Manufacturing/ProductionAtGlance

        public ActionResult ProductionAtGlance()
        {
            ManufacturingModel rep = new ManufacturingModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        public ActionResult ProductionAtGlance(ManufacturingModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);

            return View();
        }

        // GET: Reports/Manufacturing/ProductionPacking
        public ActionResult ProductionPacking()
        {
            ManufacturingModel rep = new ManufacturingModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        public ActionResult ProductionPacking(ManufacturingModel model)
        {
            model.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            return View();
        }

        // GET: Reports/Manufacturing/YieldRegister
        public ActionResult YieldRegister()
        {
            ManufacturingModel rep = new ManufacturingModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        public ActionResult YieldRegister(ManufacturingModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.MaterialWhereUsed);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            var YieldRegister = RptEntity.SpRptYieldRegister(
                                            StartDate,
                                            EndDate,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/YieldRegisterReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("YieldRegisterDataSet", YieldRegister));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        public ActionResult ProductionIssue()
        {
            ManufacturingModel rep = new ManufacturingModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        [HttpPost]
        public ActionResult ProductionIssue(ManufacturingModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.YieldRegister);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            var ProductionIssue = RptEntity.SpRptYieldRegister(
                                            StartDate,
                                            EndDate,
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/YieldRegisterReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("YieldRegisterDataSet", ProductionIssue));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        public ActionResult ProductionMaterialWhereUsed()
        {
            ManufacturingModel rep = new ManufacturingModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            ViewBag.ReportViewer = reportViewer;
            rep.FromOutputItemCodeRangeList = AtoZRange;
            rep.ToOutputItemCodeRangeList = AtoZRange;
            rep.FromOutputItemNameRangeList = AtoZRange;
            rep.ToOutputItemNameRangeList = AtoZRange;
            return View(rep);
        }

        [HttpPost]
        public ActionResult ProductionMaterialWhereUsed(ManufacturingModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Production Material Usage Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            var MaterialWhereUsed = RptEntity.SpRptProductionMaterialWhereUsed(
                                                StartDate,
                                                EndDate,
                                                model.FromOutputItemNameRange,
                                                model.ToOutputItemNameRange,
                                                model.ItemCodeFromID,
                                                model.ItemCodeToID,
                                                model.OuputItemNameID,
                                                model.InputItemNameID,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionMaterialWhereUsed.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionMaterialWhereUsedDataSet", MaterialWhereUsed));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        public ActionResult ProductionTimeUtilisation()
        {
            ManufacturingModel rep = new ManufacturingModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.PSBatchDateFrom = GeneralBO.FinStartDate;
            rep.PSBatchDateTo = General.FormatDate(DateTime.Now);
            rep.FromOutputItemCodeRangeList = AtoZRange;
            rep.ToOutputItemCodeRangeList = AtoZRange;
            rep.FromOutputItemNameRangeList = AtoZRange;
            rep.ToOutputItemNameRangeList = AtoZRange;
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        [HttpPost]
        public ActionResult ProductionTimeUtilisation(ManufacturingModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            if (model.PSBatchDateFrom != null)
                PSBatchDateFrom = General.ToDateTime(model.PSBatchDateFrom);
            if (model.PSBatchDateTo != null)
            {
                PSBatchDateTo = General.ToDateTime(model.PSBatchDateTo);
                PSBatchDateTo = PSBatchDateTo.AddDays(1);
            }
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Production Time Utilisation By Item");
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            var ProductionTimeUtilisationByItem = RptEntity.SpRptProductionTimeUtilisationByItem(
                                                                StartDate,
                                                                EndDate,
                                                                PSBatchDateFrom,
                                                                PSBatchDateTo,
                                                                model.PSBatchNoFrom,
                                                                model.PSBatchNoTo,
                                                                model.ItemCodeFromID,
                                                                model.ItemCodeToID,
                                                                model.FromOutputItemNameRange,
                                                                model.ToOutputItemNameRange,
                                                                model.OuputItemNameID,
                                                                model.CategoryFromID,
                                                                model.CategoryToID,
                                                                GeneralBO.FinYear,
                                                                GeneralBO.LocationID,
                                                                GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionTimeUtilisationByItem.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionTimeUtilisationDataSet", ProductionTimeUtilisationByItem));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        //Reports/Manufacturing/ProductionByBatch
        public ActionResult ProductionByBatch()
        {
            ManufacturingModel rep = new ManufacturingModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.PSBatchDateFrom = GeneralBO.FinStartDate;
            rep.PSBatchDateTo = General.FormatDate(DateTime.Now);
            rep.FromOutputItemCodeRangeList = AtoZRange;
            rep.ToOutputItemCodeRangeList = AtoZRange;
            rep.FromOutputItemNameRangeList = AtoZRange;
            rep.ToOutputItemNameRangeList = AtoZRange;
            rep.FromInputItemCodeRangeList = AtoZRange;
            rep.ToInputItemCodeRangeList = AtoZRange;
            rep.FromInputItemNameRangeList = AtoZRange;
            rep.ToInputItemNameRangeList = AtoZRange;
            rep.StatusList = new SelectList(statusBL.GetStatusList("ProductionByBatchReport"), "Value", "Text");

            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        //Reports/Manufacturing/ProductionByBatch
        public ActionResult ProductionByBatch(ManufacturingModel model)
        {
            var RateValue = "No";
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            if (model.PSBatchDateFrom != null)
                PSBatchDateFrom = General.ToDateTime(model.PSBatchDateFrom);
            if (model.PSBatchDateTo != null)
                PSBatchDateTo = General.ToDateTime(model.PSBatchDateTo);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Production" + " " + model.ProductionType + " " + model.Summary);
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);

            if (model.ProductionType == "OutputByBatch")
            {
                if (model.RateValue == "Yes")
                {
                    RateValue = "Yes";
                    if (model.Summary == "Summary")
                    {
                        var ProductionByBatchSummary = RptEntity.SpRptProductionReportByBatchSummary(StartDate, EndDate, RateValue,
                            PSBatchDateFrom, PSBatchDateTo, model.PSBatchNoFrom, model.PSBatchNoTo, model.FromOutputItemNameRange,
                            model.ToOutputItemNameRange, model.ItemCodeFromID, model.ItemCodeToID, model.OuputItemNameID,
                            model.BatchStatusFrom, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByBatchSummary.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionByBatchSummaryDataSet", ProductionByBatchSummary));
                    }
                    else
                    {
                        var ProductionByBatchDetail = RptEntity.SpRptProductionByBatchDetail(StartDate, EndDate, PSBatchDateFrom, PSBatchDateTo, model.PSBatchNoFrom,
                            model.PSBatchNoTo, model.FromOutputItemNameRange, model.ToOutputItemNameRange, model.ItemCodeFromID, model.ItemCodeToID,
                            model.OuputItemNameID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByBatchDetail.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionByBatchDetailDataSet", ProductionByBatchDetail));
                    }
                }
                else
                {
                    if (model.Summary == "Summary")
                    {
                        var ProductionByBatchSummary = RptEntity.SpRptProductionReportByBatchSummary(StartDate, EndDate, RateValue,
                            PSBatchDateFrom, PSBatchDateTo, model.PSBatchNoFrom, model.PSBatchNoTo, model.FromOutputItemNameRange,
                            model.ToOutputItemNameRange, model.ItemCodeFromID, model.ItemCodeToID, model.OuputItemNameID,
                            model.BatchStatusFrom, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByBatchRateValueSummary.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionByBatchSummaryDataSet", ProductionByBatchSummary));
                    }
                    else
                    {
                        var ProductionByBatchDetail = RptEntity.SpRptProductionByBatchDetail(StartDate, EndDate, PSBatchDateFrom, PSBatchDateTo,
                            model.PSBatchNoFrom, model.PSBatchNoTo, model.FromOutputItemNameRange, model.ToOutputItemNameRange,
                            model.ItemCodeFromID, model.ItemCodeToID, model.OuputItemNameID, GeneralBO.FinYear, GeneralBO.LocationID,
                            GeneralBO.ApplicationID).ToList();
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByBatchRateValueDetailNew.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionByBatchDetailDataSet", ProductionByBatchDetail));
                    }
                }
            }
            else
            {
                if (model.RateValue == "Yes")
                {
                    RateValue = "Yes";
                    var ProductionInputByBatchSummary = RptEntity.SpRptProductionInputByBatchSummary(StartDate, EndDate, RateValue,
                        PSBatchDateFrom, PSBatchDateTo, model.PSBatchNoFrom, model.PSBatchNoTo, model.FromInputItemNameRange,
                        model.ToInputItemNameRange, model.InputItemCodeFromID, model.InputItemCodeToID, model.InputItemNameID,
                        model.BatchStatusFrom, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByBatchInputSummary.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionInputItemByBatchDataSet", ProductionInputByBatchSummary));

                }
                else
                {
                    var ProductionInputByBatchSummary = RptEntity.SpRptProductionInputByBatchSummary(StartDate, EndDate, RateValue,
                        PSBatchDateFrom, PSBatchDateTo, model.PSBatchNoFrom, model.PSBatchNoTo, model.FromInputItemNameRange,
                        model.ToInputItemNameRange, model.InputItemCodeFromID, model.InputItemCodeToID, model.InputItemNameID,
                        model.BatchStatusFrom, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByBatchInputRateValueSummary.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionInputItemByBatchDataSet", ProductionInputByBatchSummary));

                }
            }

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        //Reports/Manufacturing/ProductionByItem
        public ActionResult ProductionByItem()
        {
            ManufacturingModel rep = new ManufacturingModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.PSBatchDateFrom = GeneralBO.FinStartDate;
            rep.PSBatchDateTo = General.FormatDate(DateTime.Now);
            rep.FromOutputItemCodeRangeList = AtoZRange;
            rep.ToOutputItemCodeRangeList = AtoZRange;
            rep.FromOutputItemNameRangeList = AtoZRange;
            rep.ToOutputItemNameRangeList = AtoZRange;
            rep.FromInputItemCodeRangeList = AtoZRange;
            rep.ToInputItemCodeRangeList = AtoZRange;
            rep.FromInputItemNameRangeList = AtoZRange;
            rep.ToInputItemNameRangeList = AtoZRange;
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        //Reports/Manufacturing/ProductionByItem
        public ActionResult ProductionByItem(ManufacturingModel model)
        {
            var RateValue = "No";
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            if (model.PSBatchDateFrom != null)
                PSBatchDateFrom = General.ToDateTime(model.PSBatchDateFrom);
            if (model.PSBatchDateTo != null)
                PSBatchDateTo = General.ToDateTime(model.PSBatchDateTo);

            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Production" + " " + model.ProductionType + " " + model.Summary);
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);

            if (model.ProductionType == "OutputByItem")
            {
                if (model.RateValue == "Yes")
                {
                    RateValue = "Yes";
                    if (model.Summary == "Summary")
                    {
                        var ProductionByItemSummary = RptEntity.SpRptProductionByItemSummary(StartDate, EndDate, PSBatchDateFrom, PSBatchDateTo,
                            model.FromOutputItemNameRange, model.ToOutputItemNameRange, model.ItemCodeFromID, model.ItemCodeToID, model.OuputItemNameID,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByItemSummary.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionByItemSummaryDataSet", ProductionByItemSummary));

                    }
                    else
                    {
                        var ProductionByItemDetail = RptEntity.SpRptProductionByItemDetail(StartDate, EndDate, PSBatchDateFrom, PSBatchDateTo,
                            model.FromOutputItemNameRange, model.ToOutputItemNameRange, model.ItemCodeFromID, model.ItemCodeToID, model.OuputItemNameID,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByItemDetail.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionByItemDetailDataSet", ProductionByItemDetail));

                    }
                }
                else
                {
                    if (model.Summary == "Summary")
                    {
                        var ProductionByItemSummary = RptEntity.SpRptProductionByItemSummary(StartDate, EndDate, PSBatchDateFrom, PSBatchDateTo,
                            model.FromOutputItemNameRange, model.ToOutputItemNameRange, model.ItemCodeFromID, model.ItemCodeToID, model.OuputItemNameID,
                            GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByItemRateValueSummary.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionByItemSummaryDataSet", ProductionByItemSummary));

                    }
                    else
                    {
                        var ProductionByItemDetail = RptEntity.SpRptProductionByItemDetail(StartDate, EndDate, PSBatchDateFrom, PSBatchDateTo, model.FromOutputItemNameRange, model.ToOutputItemNameRange, model.ItemCodeFromID, model.ItemCodeToID, model.OuputItemNameID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByItemRateValueDetailNew.rdlc";
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionByItemDetailDataSet", ProductionByItemDetail));

                    }
                }
            }
            else
            {
                if (model.RateValue == "Yes")
                {
                    RateValue = "Yes";
                    var ProductionInputByItemSummary = RptEntity.SpRptProductionInputByItemSummary(StartDate, EndDate, PSBatchDateFrom, PSBatchDateTo,
                        model.FromInputItemNameRange, model.ToInputItemNameRange, model.InputItemCodeFromID, model.InputItemCodeToID, model.InputItemNameID,
                        GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByItemInputSummary.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionInputByItemDataSet", ProductionInputByItemSummary));

                }
                else
                {
                    var ProductionInputByItemSummary = RptEntity.SpRptProductionInputByItemSummary(StartDate, EndDate, PSBatchDateFrom, PSBatchDateTo,
                        model.FromInputItemNameRange, model.ToInputItemNameRange, model.InputItemCodeFromID, model.InputItemCodeToID, model.InputItemNameID,
                        GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByItemInputRateValueSummary.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionInputByItemDataSet", ProductionInputByItemSummary));

                }
            }

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        //Reports/Manufacturing/ProductionByCategory
        public ActionResult ProductionByCategory()
        {
            ManufacturingModel rep = new ManufacturingModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.PSBatchDateFrom = GeneralBO.FinStartDate;
            rep.PSBatchDateTo = General.FormatDate(DateTime.Now);
            rep.FromOutputItemCodeRangeList = AtoZRange;
            rep.ToOutputItemCodeRangeList = AtoZRange;
            rep.FromOutputItemNameRangeList = AtoZRange;
            rep.ToOutputItemNameRangeList = AtoZRange;
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        [HttpPost]
        //Reports/Manufacturing/ProductionByCategory
        public ActionResult ProductionByCategory(ManufacturingModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            if (model.PSBatchDateFrom != null)
                PSBatchDateFrom = General.ToDateTime(model.PSBatchDateFrom);
            if (model.PSBatchDateTo != null)
                PSBatchDateTo = General.ToDateTime(model.PSBatchDateTo);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Production By Category" + " " + model.Summary);
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            var ProductionByCategoryDetail = RptEntity.SpRptProductionByCategoryDetail(StartDate, EndDate, PSBatchDateFrom, PSBatchDateTo,
                model.PSBatchNoFrom, model.PSBatchNoTo, model.FromOutputItemNameRange, model.ToOutputItemNameRange,
                model.ItemCodeFromID, model.ItemCodeToID, model.OuputItemNameID, model.CategoryFromID, model.CategoryToID,
                GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
            if (model.Summary == "Detail")
            {
                if (model.RateValue == "Yes")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByCategoryDetail.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionByCategoryDetailDataSet", ProductionByCategoryDetail));
                }
                else
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByCategoryRateValueDetail.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionByCategoryDetailDataSet", ProductionByCategoryDetail));

                }
            }
            else
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionByCategorySummary.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionByCategoryDetailDataSet", ProductionByCategoryDetail));

            }
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult DayMonthWiseProductionReport()
        {
            ManufacturingModel rep = new ManufacturingModel();
            rep.SalesCategoryList = new SelectList(categoryBL.GetSalesCategoryList(0), "ID", "Name");
            rep.ProductionCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(10), "ID", "Name");
            rep.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        public ActionResult DayMonthWiseProductionReport(ManufacturingModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseRequisitionSummaryTitle);
            var DayMonthWise = RptEntity.SpRptDayAndMonthWiseProduction(
                                                StartDate,
                                                EndDate,
                                                model.ProductionGroupNameFrom,
                                                model.ProductionGroupNameTo,
                                                model.ProductionGroupID,
                                                model.SalesCategoryID,
                                                model.ProductionCategoryID,
                                                model.ItemID,
                                                model.BatchTypeID,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID).ToList();

            if (model.ProductionType == "DayWise")
            {
                if (model.ReportType == "Summary")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/DayWiseProductionSummary.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DayMonthWiseProductionDataSet", DayMonthWise));
                    ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.DayWiseProductionSummary);
                }
                else if (model.ReportType == "Detail")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/DayWiseProductionDetail.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DayMonthWiseProductionDataSet", DayMonthWise));
                    ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.DayWiseProductionDetail);
                }
            }
            else if (model.ProductionType == "MonthWise")
            {
                if (model.ReportType == "Summary")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/MonthWiseProductionSummary.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DayMonthWiseProductionDataSet", DayMonthWise));
                    ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.MonthWiseProductionSummary);
                }
                else if (model.ReportType == "Detail")
                {
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/MonthWiseProductionDetail.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DayMonthWiseProductionDataSet", DayMonthWise));
                    ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.MonthWiseProductionDetail);
                }

            }
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        public ActionResult ProductionScheduleStatus()
        {
            ManufacturingModel rep = new ManufacturingModel();
            rep.FromDate = GeneralBO.FinStartDate;
            rep.ToDate = General.FormatDate(DateTime.Now);
            rep.FromOutputItemCodeRangeList = AtoZRange;
            rep.ToOutputItemCodeRangeList = AtoZRange;
            rep.FromOutputItemNameRangeList = AtoZRange;
            rep.ToOutputItemNameRangeList = AtoZRange;
            rep.LocationID = GeneralBO.LocationID;
            rep.StatusList = new SelectList(statusBL.GetStatusList("ProductionScheduleStatusReport"), "Value", "Text");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }
        [HttpPost]
        public ActionResult ProductionScheduleStatus(ManufacturingModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", "Production Schedule Status" + " " + model.ReportType);
            FilterParam = new ReportParameter("Filters", model.Filters);

            var ProductionSchedule = RptEntity.SpRptProductionScheduleStatus(
                                                StartDate,
                                                EndDate,
                                                model.PSTransNoFromID,
                                                model.PSTransNoToID,
                                                model.ProductionGroupID,
                                                model.PSBatchNoFromID,
                                                model.PSBatchNoToID,
                                                model.ItemCodeFromID,
                                                model.ItemCodeToID,
                                                model.FromOutputItemNameRange,
                                                model.ToOutputItemNameRange,
                                                model.OuputItemNameID,
                                                model.IssueNoFromID,
                                                model.IssueNoToID,
                                                model.ReceiptNoFromID,
                                                model.ReceiptNoToID,
                                                model.ProductionIssueNoFromID,
                                                model.ProductionIssueNoToID,
                                                model.Status,
                                                GeneralBO.CreatedUserID,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID
                                                ).ToList();

            if (model.ReportType == "Summary")
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionScheduleStatusSummary.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionScheduleStatusDataSet", ProductionSchedule));

            }
            else if (model.ReportType == "Detail")
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionScheduleStatusDetail.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionScheduleStatusDataSet", ProductionSchedule));

            }

            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        public ActionResult MaterialPurification()
        {
            MaterialPurificationModel rep = new MaterialPurificationModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDate(DateTime.Now);
            rep.ReceiptDateFrom = GeneralBO.FinStartDate;
            rep.ReceiptDateTo = General.FormatDate(DateTime.Now);
            rep.ProcessList = new SelectList(_preprocessIssue.GetProcessList(), "ID", "Name");
            ViewBag.ReportViewer = reportViewer;
            return View(rep);
        }

        [HttpPost]
        public ActionResult MaterialPurification(MaterialPurificationModel model)
        {
            if (model.ReceiptDateFrom != null)
                ReceiptDateFrom = General.ToDateTime(model.ReceiptDateFrom);
            if (model.ReceiptDateTo != null)
                ReceiptDateTo = General.ToDateTime(model.ReceiptDateTo);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", " Material Purification Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            var MaterialPurification = RptEntity.SpRptMaterialPurification(
                                                model.FromDate,
                                                model.ToDate,
                                                ReceiptDateFrom,
                                                ReceiptDateTo,
                                                model.IssueNoID,
                                                model.ReceiptNoID,
                                                model.SupplierID,
                                                model.IssueItemCodeID,
                                                model.IssueItemID,
                                                model.ReceiptItemCodeID,
                                                model.ReceiptItemID,
                                                model.ProcessID,
                                                GeneralBO.FinYear,
                                                GeneralBO.LocationID,
                                                GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/MaterialPurification.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MaterialPurificationDataSet", MaterialPurification));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpPost]
        public JsonResult ProductionSchedulePrintPdf(int Id)
        {
            FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ProductionSchedule);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var ProductionSchedule = _productionSchedule.GetProductionSchedule(Id);
            List<SpGetProductionSchedule_Result> productionSchedules = new List<SpGetProductionSchedule_Result>();
            SpGetProductionSchedule_Result productionSchedule = new SpGetProductionSchedule_Result()
            {
                TransNo = ProductionSchedule.TransNo,
                TransDate = ProductionSchedule.TransDate,
                ActualBatchSize = ProductionSchedule.ActualBatchSize,
                ProductionGroupName = ProductionSchedule.ProductionGroupName,
                ProductionStartDate = ProductionSchedule.ProductionStartDate,
                StandardBatchSize = ProductionSchedule.StandardBatchSize,
                BatchNO = ProductionSchedule.BatchNo,
            };
            productionSchedules.Add(productionSchedule);
            var ProductionScheduleTrans = ProductionScheduleDal.GetProductionScheduleTransDetails(Id, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionSchedulePrintPdf.rdlc";
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionSchedulePrintPdfDataSet", productionSchedules));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionScheduleTransPrintPdfDataSet", ProductionScheduleTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = ProductionSchedule.TransNo + ".pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/ProductionSchedule/"), FileName);
            string URL = "/Outputs/ProductionSchedule/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductionGroups(string ItemHind)
        {
            var productionGroups = _productionSchedule.GetProductionGroups(ItemHind);
            return Json(productionGroups, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAutoComplete(string Term = "", string Table = "")
        {
            List<ReportViewModel> CodeList = new List<ReportViewModel>();
            CodeList = reportBL.GetAutoComplete(Term, Table).Select(a => new ReportViewModel()
            {
                ID = a.ID,
                Code = a.Code
            }).ToList();
            return Json(CodeList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemRange(string from_range)
        {
            char range = Convert.ToChar(from_range);
            ManufacturingModel rep = new ManufacturingModel();
            rep.ToOutputItemCodeRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            return Json(new { Status = "success", data = rep.ToOutputItemCodeRangeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemNameRange(string from_range)
        {
            char range = Convert.ToChar(from_range);
            ManufacturingModel rep = new ManufacturingModel();
            rep.ToOutputItemNameRangeList = new SelectList(Enumerable.Range(range, 'Z' - range + 1).Select(c => (char)c).ToList());
            return Json(new { Status = "success", data = rep.ToOutputItemNameRangeList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BatchwiseProductionPacking()
        {
            BatchwiseProductionPackingModel rep = new BatchwiseProductionPackingModel();
            rep.FromDateString = GeneralBO.FinStartDate;
            rep.ToDateString = General.FormatDate(DateTime.Now);
            rep.SalesCategoryList = new SelectList(categoryBL.GetSalesCategoryList(0), "ID", "Name");
            rep.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            return View(rep);
        }
        [HttpPost]
        public ActionResult BatchwiseProductionPacking(BatchwiseProductionPackingModel model)
        {

            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", " Batchwise Production Packing Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            var BatchwiseProductionPacking = RptEntity.SpRptBatchwiseProductionPacking(
                                                        model.FromDate,
                                                        model.ToDate,
                                                        model.ItemID,
                                                        model.SalesCategoryID,
                                                        model.BatchID,
                                                        model.BatchTypeID,
                                                        0,
                                                        0,0,//Error
                                                        GeneralBO.FinYear,
                                                        GeneralBO.LocationID,
                                                        GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/BatchwiseProductionPacking.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("BatchwiseProductionPackingDataSet", BatchwiseProductionPacking));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");

        }
        [HttpGet]
        public ActionResult ProductionOutputAnalysis()
        {
            ProductionOutputAnalysisModel model = new ProductionOutputAnalysisModel();
            model.FromDateString = GeneralBO.FinStartDate;
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.PackingFromDateString = GeneralBO.FinStartDate;
            model.PackingToDateString = General.FormatDate(DateTime.Now);
            model.StatusList = new SelectList(statusBL.GetStatusList("ProductionOutputAnalysisReport"), "Value", "Text");
            return View(model);
        }

        [HttpPost]
        public ActionResult ProductionOutputAnalysis(ProductionOutputAnalysisModel model)
        {
            try
            {
                FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
                ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
                ReportNameParam = new ReportParameter("ReportName", "Production Output Analysis Report");
                FilterParam = new ReportParameter("Filters", model.Filters);
                UserParam = new ReportParameter("User", GeneralBO.EmployeeName);

                var Production = new object();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionOutputAnalysis.rdlc";
                Production = RptEntity.SpRptProductionOutputAnalysis(
                    model.FromDate,
                    model.ToDate,
                    model.FromDate,
                    model.ToDate,
                    model.ProductionGroupID,
                    model.BatchNo,
                    model.Status,
                    GeneralBO.CreatedUserID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID
                    ).ToList();
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionOutputAnalysisDataSet", Production));
                ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
                ViewBag.ReportViewer = reportViewer;
                return View("~/Areas/Reports/Views/ReportViewer.cshtml");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ProductionTargetVSActual()
        {
            ProductionTargetVSActual model = new ProductionTargetVSActual();
            model.FromDateString = GeneralBO.FinStartDate;
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            return View(model);
        }
        [HttpPost]
        public ActionResult ProductionTargetVSActual(ProductionTargetVSActual model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            ReportNameParam = new ReportParameter("ReportName", "Production Output Analysis Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);

            var Production = new object();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionTargetVSActual.rdlc";
            Production = RptEntity.SpRptTargetVSActual(
                model.FromDate,
                model.ToDate,
                model.ItemCodeID,
                model.ItemID,
                model.BatchTypeID,
                //GeneralBO.CreatedUserID,
                GeneralBO.FinYear,
                GeneralBO.LocationID,
                GeneralBO.ApplicationID
                ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionTargetVSActualDataSet", Production));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult ProductionWorkInProgress()
        {
            ProductionWorkInProgressModel model = new ProductionWorkInProgressModel();
            model.FromDateString = GeneralBO.FinStartDate;
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.SalesCategoryList = new SelectList(categoryBL.GetSalesCategoryList(0), "ID", "Name");
            model.ProductionCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(10), "ID", "Name");
            return View(model);
        }
        [HttpPost]
        public ActionResult ProductionWorkInProgress(ProductionWorkInProgressModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
           
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            if (model.ReportType == "Production")
            {
                var ProductionWIP = RptEntity.SpRptProductionWorkInProgress(
                    model.FromDate,
                    model.ToDate,
                    model.ProductionGroupID,
                    model.BatchID,
                    model.ProductionCategoryID,
                    model.SalesCategoryID,
                    GeneralBO.FinYear,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID
                    ).ToList();
                ReportNameParam = new ReportParameter("ReportName", "Production Work In Progress Report");
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionWorkInProgress.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionWorkInProgressDataSet", ProductionWIP));
            }
            else if (model.ReportType == "QC")
            {
                var QCWIP = RptEntity.SpRptQCWorkInProgress(
                   model.FromDate,
                   model.ToDate,
                   model.ProductionGroupID,
                   model.BatchID,
                   model.ProductionCategoryID,
                   model.SalesCategoryID,
                   GeneralBO.FinYear,
                   GeneralBO.LocationID,
                   GeneralBO.ApplicationID
                   ).ToList();
                ReportNameParam = new ReportParameter("ReportName", "QC Work In Progress Report");
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/QCWorkInProgress.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("QCWorkInProgressDataSet", QCWIP));
            }
            else
            {
                var PackingWIP = RptEntity.SpRptPackingWorkInProgress(
                   model.FromDate,
                   model.ToDate,
                   model.ProductionGroupID,
                   model.BatchID,
                   model.ProductionCategoryID,
                   model.SalesCategoryID,
                   GeneralBO.FinYear,
                   GeneralBO.LocationID,
                   GeneralBO.ApplicationID
                   ).ToList();
                ReportNameParam = new ReportParameter("ReportName", "Packing Work In Progress Report");
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/PackingWorkInProgress.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PackingWorkInProgressDataSet", PackingWIP));
            }
            
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }


        [HttpGet]
        public ActionResult ProductionDefinition()
        {
            ProductionDefinitionModel model = new ProductionDefinitionModel();
            model.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            model.ItemPackingList = new SelectList(categoryBL.GetItemsWithPackSize(0), "ID", "Name");

            return View(model);
        }

        [HttpPost]
        public ActionResult ProductionDefinition(ProductionDefinitionModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "Production Definition Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            var ProductionDefinition = RptEntity.SpRptProductionDefinition(
                                    model.ReportType,
                                    model.ProductionGroupID,
                                    model.StandardBatchSize,
                                    model.ActualBatchSize,
                                    model.BatchTypeID,
                                    model.PackSize,
                                    model.Qty,
                                    GeneralBO.CreatedUserID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID,
                                    model.PackingItemID
                                    ).ToList();

            if (model.ReportType == "Production")
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/ProductionDefinition.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionDefinitionDataSet", ProductionDefinition));

            }
            else if (model.ReportType == "Packing")
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Production/PackingDefinition.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductionDefinitionDataSet", ProductionDefinition));

            }

            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

      

    }
}