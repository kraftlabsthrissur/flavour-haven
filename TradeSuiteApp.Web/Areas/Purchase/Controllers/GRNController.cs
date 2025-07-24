using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using BusinessLayer;
using TradeSuiteApp.Web.Utils;
using DataAccessLayer.DBContext;
using Microsoft.Reporting.WebForms;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Areas.Masters.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using System.IO;
using System.Web.Hosting;
namespace TradeSuiteApp.Web.Areas.Purchase.Controllers
{
    /// <summary>
    public class CustomPdfPageEventHelper : PdfPageEventHelper
    {
        private readonly Image _headerImage;
        private readonly Image _footerImage;
        private readonly float _headerHeight = 100f; // Header height
        private readonly float _headerMargin = 0f;  // No additional margin for header
        private readonly float _footerHeight = 80f; // Footer height
        private readonly float _footerMargin = -80f;  // Add more bottom margin in A4 Document to handle this margin

        public CustomPdfPageEventHelper()
        {
            string headerImagePath = HostingEnvironment.MapPath("~/Assets/img/headers/header.jpg");
            string footerImagePath = HostingEnvironment.MapPath("~/Assets/img/headers/footer.jpg");

            _headerImage = Image.GetInstance(headerImagePath);
            _footerImage = Image.GetInstance(footerImagePath);

            // Scaling the images to fit A4 width (minus margins if any)
            // Assuming 36 points for left/right margins, available width for content is 595 - 72 = 523
            float availableWidth = PageSize.A4.Width - 72; // Total width of the page minus left and right margins
            _headerImage.ScaleToFit(availableWidth, _headerHeight);
            _footerImage.ScaleToFit(availableWidth, _footerHeight);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            // Draw header image
            if (_headerImage != null)
            {
                float headerImageY = document.PageSize.Height - _headerHeight - _headerMargin; // Calculate Y position for the header
                _headerImage.SetAbsolutePosition(document.LeftMargin, headerImageY); // Align with left margin
                writer.DirectContent.AddImage(_headerImage);
            }

            // Draw footer image
            if (_footerImage != null)
            {
                float footerImageY = document.BottomMargin + _footerMargin; // Calculate Y position for the footer
                _footerImage.SetAbsolutePosition(document.LeftMargin, footerImageY); // Align with left margin
                writer.DirectContent.AddImage(_footerImage);
            }
        }
    }
    /// </summary>
    public class GRNController : Controller
    {

        private IGoodsReceiptNoteContract goodsReceiptNoteBL;
        private IGeneralContract generalBL;
        private IWareHouseContract warehouseBL;
        private IUnitContract unitBL;
        private IDiscountCategoryContract discountCategoryBL;
        private IGSTCategoryContract gSTCategoryBL;
        private IAddressContract addressBL;
        private IGSTContract GstBL;
        private ICategoryContract categroyBL;
        private ICounterSalesContract counterSalesBL;
        AyurwareEntities db;
        ReportViewer reportViewer;

        ReportParameter p1, p2, p3, p4, p5, p6;

        public GRNController()
        {
            goodsReceiptNoteBL = new GoodsReceiptNoteBL();
            warehouseBL = new WarehouseBL();
            generalBL = new GeneralBL();
            unitBL = new UnitBL();
            discountCategoryBL = new DiscountCategoryBL();
            gSTCategoryBL = new GSTCategoryBL();
            addressBL = new AddressBL();
            GstBL = new GSTBL();
            categroyBL = new CategoryBL();
            counterSalesBL = new CounterSalesBL();
        }

        // GET: Purchase/GRN
        public ActionResult Index()
        {

            GRNModel grnModel = new GRNModel();
            grnModel.IsBarCodeGenerator = goodsReceiptNoteBL.IsBarCodeGenerator();
            return View(grnModel);
        }

        public ActionResult IndexV3()
        {
            GRNModel grnModel = new GRNModel();
            grnModel.IsBarCodeGenerator = goodsReceiptNoteBL.IsBarCodeGenerator();
            return View(grnModel);
        }

        public ActionResult IndexV4()
        {
            GRNModel grnModel = new GRNModel();
            grnModel.IsBarCodeGenerator = goodsReceiptNoteBL.IsBarCodeGenerator();
            return View(grnModel);
        }

        // GET: Purchase/GRN/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            GRNModel grnModel = goodsReceiptNoteBL.GetGoodsReceiptNoteDetails((int)id).Select(a => new GRNModel()
            {
                Code = a.Code,
                GRNDate = General.FormatDate(a.ReceiptDate, "view"),
                SupplierName = a.SupplierName,
                Warehouse = a.WarehouseName,
                InvoiceNo = a.DeliveryChallanNo,
                InvoiceDate = a.DeliveryChallanDate == null ? "" : General.FormatDate((DateTime)a.DeliveryChallanDate, "view"),
                SupplierID = a.SupplierID,
                IsDraft = a.IsDraft,
                SupplierCode = a.SupplierCode,
                Id = a.ID,
                IsPurchaseCompleted = a.PurchaseCompleted



            }).FirstOrDefault();
            List<GRNTransItemBO> grnItemsBO = goodsReceiptNoteBL.GetGoodsRecieptNoteItems((int)id);
            GRNItemModel grnItem;
            grnModel.grnItems = new List<GRNItemModel>();
            foreach (var m in grnItemsBO)
            {
                grnItem = new GRNItemModel()
                {
                    ItemID = m.ItemID,
                    ItemName = m.ItemName,
                    Batch = m.Batch,
                    // ExpiryDate = General.FormatDate((DateTime)m.ExpiryDate),
                    PurchaseOrderQty = m.PurchaseOrderQty,
                    ReceivedQty = m.ReceivedQty,
                    QualityCheckQty = m.QualityCheckQty,
                    AcceptedQty = (decimal)m.AcceptedQty,
                    PendingPOQty = (decimal)m.PendingPOQty,
                    PurchaseOrderNo = m.PurchaseOrderNo,
                    Unit = m.Unit,
                    BatchType = m.BatchType,
                    Remarks = m.Remarks,
                    UnitID = m.UnitID
                };
                if (m.ExpiryDate != null)
                {
                    grnItem.ExpiryDate = General.FormatDate((DateTime)m.ExpiryDate);
                }
                grnModel.grnItems.Add(grnItem);
            }

            return View(grnModel);


        }

        // GET: Purchase/GRN/Create
        public ActionResult Create()
        {
            GRNModel grnModel = new GRNModel();
            grnModel.WarehoueList = warehouseBL.GetWareHouses();
            grnModel.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultRMStore"));
            grnModel.GRNNo = generalBL.GetSerialNo("GRN", "Code");
            return View(grnModel);
        }
        // POST: Purchase/GRN/Create
        [HttpPost]
        public ActionResult Save(GRNModel grnModel)
        {
            var result = new List<object>();
            try
            {
                if (grnModel.Id != 0)
                {
                    //Edit
                    //Check whether editable or not
                    GRNBO Temp = goodsReceiptNoteBL.GetGoodsReceiptNoteDetails(grnModel.Id).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                GRNBO grnBO = new GRNBO()
                {
                    ID = grnModel.Id,
                    Code = grnModel.GRNNo,
                    Date = General.ToDateTime(grnModel.GRNDate),
                    SupplierID = grnModel.SupplierID,
                    ReceiptDate = General.ToDateTime(grnModel.GRNDate),
                    DeliveryChallanNo = grnModel.InvoiceNo,
                    WarehouseID = grnModel.WarehouseID,
                    PurchaseCompleted = false,
                    IsCancelled = false,
                    CancelledDate = null,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = GeneralBO.CreatedUserID,
                    IsDraft = grnModel.IsDraft,
                };
                if (grnModel.InvoiceDate != null)
                {
                    grnBO.DeliveryChallanDate = General.ToDateTime(grnModel.InvoiceDate);
                }
                List<GRNTransItemBO> grnItemsBO = new List<GRNTransItemBO>();
                GRNTransItemBO grnItemBO;
                if (grnModel.grnItems != null)
                {
                    foreach (var item in grnModel.grnItems)
                    {

                        grnItemBO = new GRNTransItemBO()
                        {
                            PurchaseOrderID = item.PurchaseOrderID,
                            POTransID = item.POTransID,
                            ItemID = item.ItemID,
                            Batch = item.Batch,
                            ReceivedQty = item.ReceivedQty,
                            AcceptedQty = item.AcceptedQty,
                            Remarks = (item.Remarks == null) ? "" : item.Remarks,
                            IsQCRequired = item.IsQCRequired,
                            QtyTolerance = item.QtyTolerance,
                            UnitID = item.UnitID
                        };
                        if (item.ExpiryDate != null)
                        {
                            grnItemBO.ExpiryDate = General.ToDateTime(item.ExpiryDate);
                        }

                        grnItemsBO.Add(grnItemBO);
                    }
                }
                if (goodsReceiptNoteBL.SaveGRN(grnBO, grnItemsBO))
                {
                    return Json(new { Status = "success", data = grnModel }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Unknown Error" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "GRN", "Save", grnModel.Id, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult CreateGRN(GRNModel grnModel)
        {
            var result = new List<object>();
            try
            {
                if (grnModel.Id != 0)
                {
                    //Edit
                    //Check whether editable or not
                    GRNBO Temp = goodsReceiptNoteBL.GetGoodsReceiptNoteDetails(grnModel.Id).FirstOrDefault();
                    if (Temp.PurchaseCompleted)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }

                }
                GRNBO grnBO = new GRNBO()
                {
                    ID = grnModel.Id,
                    Code = grnModel.GRNNo,
                    Date = General.ToDateTime(grnModel.GRNDate),
                    SupplierID = grnModel.SupplierID,
                    ReceiptDate = General.ToDateTime(grnModel.GRNDate),
                    DeliveryChallanNo = grnModel.InvoiceNo,
                    WarehouseID = grnModel.WarehouseID,
                    PurchaseCompleted = false,
                    IsCancelled = false,
                    CancelledDate = null,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = GeneralBO.CreatedUserID,
                    IsDraft = grnModel.IsDraft,
                    IGSTAmt = grnModel.IGSTAmt,
                    SGSTAmt = grnModel.SGSTAmt,
                    CGSTAmt = grnModel.CGSTAmt,
                    RoundOff = grnModel.RoundOff,
                    DiscountAmt = grnModel.DiscountAmt,
                    VATAmount = grnModel.VATAmount,
                    SuppDocAmount = grnModel.SuppDocAmount,
                    SuppShipAmount = grnModel.SuppShipAmount,
                    PackingForwarding = grnModel.PackingForwarding,
                    SuppOtherCharges = grnModel.SuppOtherCharges,
                    SuppFreight = grnModel.SuppFreight,
                    LocalCustomsDuty = grnModel.LocalCustomsDuty,
                    LocalFreight = grnModel.LocalFreight,
                    LocalMiscCharge = grnModel.LocalMiscCharge,
                    LocalOtherCharges = grnModel.LocalOtherCharges,
                    Remarks = grnModel.Remarks,
                    GrossAmt = grnModel.GrossAmt,
                    NetAmount = grnModel.NetAmount,
                    IsCheckedDirectInvoice = grnModel.IsCheckedDirectInvoice,
                    LocalLandinngCost = grnModel.LocalLandinngCost,
                    CurrencyExchangeRate = grnModel.CurrencyExchangeRate,
                    LandingCost = ((grnModel.SuppDocAmount + grnModel.SuppFreight + grnModel.SuppOtherCharges + grnModel.SuppShipAmount + grnModel.PackingForwarding) / (grnModel.CurrencyExchangeRate > 0 ? grnModel.CurrencyExchangeRate : 1)) + grnModel.LocalLandinngCost
                };
                if (grnModel.InvoiceDate != null)
                {
                    grnBO.DeliveryChallanDate = General.ToDateTime(grnModel.InvoiceDate);
                }
                List<GRNTransItemBO> grnItemsBO = new List<GRNTransItemBO>();
                GRNTransItemBO grnItemBO;
                if (grnModel.grnItems != null)
                {
                    foreach (var item in grnModel.grnItems)
                    {

                        grnItemBO = new GRNTransItemBO()
                        {
                            PurchaseOrderID = item.PurchaseOrderID,
                            POTransID = item.POTransID,
                            ItemID = item.ItemID,
                            ItemCode = item.ItemCode,
                            ItemName = item.ItemName,
                            PartsNumber = item.PartsNumber,
                            Remark = item.Remark,
                            Model = item.Model,
                            IsGST = item.IsGST,
                            IsVat = item.IsVat,
                            CurrencyID = item.CurrencyID,
                            Batch = item.Batch,
                            ReceivedQty = item.ReceivedQty,
                            LooseQty = item.LooseQty,
                            LooseRate = item.LooseRate,
                            Remarks = (item.Remarks == null) ? "" : item.Remarks,
                            UnitID = item.UnitID,
                            PurchaseOrderQty = item.PurchaseOrderQty,
                            PurchaseRate = item.PurchaseRate,
                            SecondaryUnit = item.SecondaryUnit,
                            SecondaryRate = item.SecondaryRate,
                            SecondaryUnitSize = item.SecondaryUnitSize,
                            OfferQty = item.OfferQty,
                            DiscountID = item.DiscountID,
                            GrossAmount = item.GrossAmount,
                            NetAmount = item.NetAmount,
                            DiscountAmount = item.DiscountAmount,
                            DiscountPercent = item.DiscountPercent,
                            VATPercentage = item.VATPercentage,
                            VATAmount = item.VATAmount,
                            TaxableAmount = item.TaxableAmount,
                            BatchID = item.BatchID,
                            IGSTPercent = item.IGSTPercent,
                            CGSTPercent = item.CGSTPercent,
                            SGSTPercent = item.SGSTPercent,
                            IGSTAmt = item.IGSTAmt,
                            SGSTAmt = item.SGSTAmt,
                            CGSTAmt = item.CGSTAmt,
                            LandingCost = (item.NetAmount / (grnBO.GrossAmt) * grnBO.LandingCost) / item.ReceivedQty,
                            PurchaseOrderNo = grnModel.PurchaseOrderNo,
                            BinCode = grnModel.BinCode,
                        };
                        if (item.ExpiryDate != null)
                        {
                            grnItemBO.ExpiryDate = General.ToDateTime(item.ExpiryDate);
                        }

                        grnItemsBO.Add(grnItemBO);
                    }
                }
                int GRNID = goodsReceiptNoteBL.CreateGRN(grnBO, grnItemsBO);
                if (GRNID > 0)
                {
                    return Json(new { Status = "success", data = grnModel, GRNID }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Unknown Error" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                result.Add(new
                {
                    ErrorMessage = e.Message
                });
                generalBL.LogError("Purchase", "GRN", "Save", grnModel.Id, e);
                return Json(new
                {
                    Status = "failure",
                    data = result
                }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public JsonResult GetPurchaseOrders(int id)
        {
            try
            {
                GRNModel grnModel = new GRNModel();

                grnModel.UnProcesedPOList = goodsReceiptNoteBL.GetUnProcessedPurchaseOrderForGrn(id).Select(m => new PurchaseOrderModel()
                {
                    ID = m.ID,
                    PurchaseOrderNo = m.PurchaseOrderNo,
                    PurchaseOrderDate = General.FormatDate(m.PurchaseOrderDate),
                    RequestedBy = m.RequestedBy,
                    SupplierName = m.SupplierName,
                    NetAmt = m.NetAmt
                }
                ).ToList();
                return Json(grnModel.UnProcesedPOList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //public JsonResult GetPurchaseOrderItems(int[] PurchaseOrderIDS)
        //{
        //    try
        //    {
        //        GRNModel grnModel = new GRNModel();
        //        grnModel.UnProcesedPOTransList = new List<PurchaseOrderTransBO>();
        //        if (PurchaseOrderIDS.Length > 0)
        //        {
        //            foreach (var PurchaseOrderID in PurchaseOrderIDS)
        //            {
        //                var list = goodsReceiptNoteBL.GetUnProcessedPurchaseOrderTransItemForGrn(PurchaseOrderID);
        //                if (list != null)
        //                {
        //                    grnModel.UnProcesedPOTransList.AddRange(list);
        //                }
        //            }
        //        }

        //        return Json(grnModel.UnProcesedPOTransList, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //        // return Json("failure", JsonRequestBehavior.AllowGet);
        //    }
        //}

        public PartialViewResult GetUnProcessedMilkBySupplier(int SupplierID)
        {
            List<GRNBO> grnBOList;

            grnBOList = goodsReceiptNoteBL.GetUnProcessedMilkPurchase(SupplierID);

            return PartialView("~/Areas/Purchase/Views/MilkPurchase/MilkPO.cshtml", grnBOList);
        }
        public JsonResult GetGRNNoWithItemID(int id)
        {
            List<GRNBO> GRNNo = goodsReceiptNoteBL.GetGRNNoWithItemID(id).ToList();
            return Json(new { Status = "success", data = GRNNo }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveAsDraft(GRNModel grnModel)
        {
            return Save(grnModel);
        }
        public ActionResult DraftGRN(GRNModel grnModel)
        {
            return CreateGRN(grnModel);
        }
        [HttpPost]
        public ActionResult Print(int SupplierID, string Code)
        {
            try
            {
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                p1 = new ReportParameter("CompanyName", GeneralBO.CompanyName);
                p2 = new ReportParameter("Address1", GeneralBO.Address1);
                p3 = new ReportParameter("Address2", GeneralBO.Address2);
                p4 = new ReportParameter("Address3", GeneralBO.Address3);
                p5 = new ReportParameter("Address4", GeneralBO.Address4);
                p6 = new ReportParameter("Address5", GeneralBO.Address5);

                reportViewer = new ReportViewer();
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.SizeToReportContent = true;
                reportViewer.Width = System.Web.UI.WebControls.Unit.Percentage(900);
                reportViewer.Height = System.Web.UI.WebControls.Unit.Percentage(900);
                reportViewer.BackColor = System.Drawing.Color.White;

                db = new AyurwareEntities();

                //var GrnPrint = db.viLocalGRNPrints.AsQueryable();

                //if (grn.SupplierName != null)
                //    GrnPrint = GrnPrint.Where(a => a.LocalSupplierName == grn.SupplierName);
                //if (SupplierCodePrint == "LOCALSUP")
                //{
                var GrnPrint = db.SpGetGRNLocalPrint(SupplierID, Code, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Purchase/GRNLocal.rdlc";
                ReportParameter p7 = new ReportParameter("ReportName", App_LocalResources.Reports.GRNLocalReport);
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7 });

                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GRNLocalPrintDataSet", GrnPrint));
                byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename= GrnLocalReport" + "." + extension);
                Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                Response.Flush(); // send it to the client to download  
                Response.End();

                ViewBag.ReportViewer = reportViewer;
                return View();

            }
            catch (Exception e)
            {
                return View();
            }
        }

        public JsonResult GetGrnInvoiceAutoComplete(string Term = "")
        {
            List<GRNModel> InvoiceList = new List<GRNModel>();
            InvoiceList = goodsReceiptNoteBL.GetGrnInvoiceAutoComplete(Term).Select(a => new GRNModel()
            {
                Id = a.ID,
                Code = a.Code,
            }).ToList();
            return Json(InvoiceList, JsonRequestBehavior.AllowGet);
        }
        // GET: Purchase/GRN/Edit/
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }

            if (id > 0)
            {
                GRNModel grnModel = goodsReceiptNoteBL.GetGoodsReceiptNoteDetails((int)id).Select(a => new GRNModel()
                {
                    Code = a.Code,
                    GRNDate = General.FormatDate(a.ReceiptDate),
                    SupplierName = a.SupplierName,
                    Warehouse = a.WarehouseName,
                    InvoiceNo = a.DeliveryChallanNo,
                    InvoiceDate = a.DeliveryChallanDate == null ? "" : General.FormatDate((DateTime)a.DeliveryChallanDate),
                    SupplierID = a.SupplierID,
                    IsDraft = a.IsDraft,
                    SupplierCode = a.SupplierCode,
                    Id = a.ID,
                    WarehouseID = a.WarehouseID,
                    PurchaseOrderDate = General.FormatDateNull(a.PurchaseOrderDate)
                }).FirstOrDefault();
                if (!grnModel.IsDraft)
                {
                    return RedirectToAction("Index");
                }
                grnModel.WarehoueList = warehouseBL.GetWareHouses();
                List<GRNTransItemBO> grnItemsBO = goodsReceiptNoteBL.GetGoodsRecieptNoteItems((int)id);
                GRNItemModel grnItem;
                grnModel.grnItems = new List<GRNItemModel>();
                foreach (var m in grnItemsBO)
                {
                    grnItem = new GRNItemModel()
                    {
                        ItemID = m.ItemID,
                        ItemName = m.ItemName,
                        Batch = m.Batch,
                        PurchaseOrderQty = m.PurchaseOrderQty,
                        ReceivedQty = m.ReceivedQty,
                        QualityCheckQty = m.QualityCheckQty,
                        AcceptedQty = (decimal)m.AcceptedQty,
                        PendingPOQty = (decimal)m.PendingPOQty,
                        PurchaseOrderNo = m.PurchaseOrderNo,
                        Unit = m.Unit,
                        BatchType = m.BatchType,
                        PurchaseOrderID = m.PurchaseOrderID,
                        POTransID = m.POTransID,
                        POQuantity = m.Quantity,
                        QtyTolerancePercent = m.QtyTolerancePercent,
                        IsQCRequired = m.IsQCRequired,
                        ItemCategory = m.ItemCategory,
                        AllowedQty = m.AllowedQty,
                        Remarks = m.Remarks,
                        UnitID = m.UnitID
                    };
                    if (m.ExpiryDate != null)
                    {
                        grnItem.ExpiryDate = General.FormatDate((DateTime)m.ExpiryDate);
                    }
                    grnModel.grnItems.Add(grnItem);
                    grnModel.UnProcesedPOList = goodsReceiptNoteBL.GetUnProcessedPurchaseOrderForGrn(grnModel.SupplierID).Select(g => new PurchaseOrderModel()
                    {
                        ID = g.ID,
                        PurchaseOrderNo = g.PurchaseOrderNo,
                        PurchaseOrderDate = General.FormatDate(g.PurchaseOrderDate),
                        RequestedBy = g.RequestedBy,
                        SupplierName = g.SupplierName,
                        NetAmt = g.NetAmt
                    }).ToList();
                }
                grnModel.ShippingStateID = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
                return View(grnModel);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Cancel(int ID)
        {
            goodsReceiptNoteBL.Cancel(ID);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvoiceNumberCount(string Hint, string Table, int SupplierID)
        {
            var count = goodsReceiptNoteBL.GetInvoiceNumberCount(Hint, Table, SupplierID);
            return Json(new { Status = "success", data = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetGRNList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string SupplierNameHint = Datatable.Columns[3].Search.Value;
                string DeliveryChallanNoHint = Datatable.Columns[4].Search.Value;
                string DeliveryChallanDateHint = Datatable.Columns[5].Search.Value;
                string WarehouseNameHint = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = goodsReceiptNoteBL.GetGRNList(Type, TransNoHint, TransDateHint, SupplierNameHint, DeliveryChallanNoHint, DeliveryChallanDateHint, WarehouseNameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Generate()
        {
            GRNModel grnModel = new GRNModel();
            grnModel.WarehoueList = warehouseBL.GetWareHouses();
            grnModel.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultRMStore"));
            grnModel.IsBarCodeGenerator = goodsReceiptNoteBL.IsBarCodeGenerator();
            grnModel.ShippingStateID = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            grnModel.DiscountList = discountCategoryBL.GetDiscountList().Select(a => new DiscountCategoryModel()
            {
                ID = (int)a.ID,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountType = a.DiscountType,
                Days = (int)a.Days,
                DiscountCategory = a.DiscountCategory
            }).ToList();
            grnModel.UOMList = unitBL.GetUnitsList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                QOM = a.QOM,
                PackSize = a.PackSize
            }).ToList();
            grnModel.GSTPercentageList = gSTCategoryBL.GetGSTList().Select(a => new GSTCategoryModel()
            {
                ID = (int)a.ID,
                IGSTPercent = (decimal)a.IGSTPercent
            }).ToList();
            //grnModel.GSTPercentageList = new SelectList(gSTCategoryBL.GetGSTList(), "ID", "IGSTPercent");
            ViewBag.TaxPercentages = GstBL.GetGstList();
            grnModel.GRNNo = generalBL.GetSerialNo("GRN", "Code");
            return View(grnModel);
        }

        public PartialViewResult ItemDetails(int ItemID, string ItemName, int UnitID, string UnitName, decimal Qty, string Batch, int BatchID, decimal MRP, decimal Rate)
        {
            List<PurchaseOrderTransModel> Items = new List<PurchaseOrderTransModel>();

            PurchaseOrderTransModel Item = new PurchaseOrderTransModel();

            //Item.ID = 0;
            Item.ItemID = ItemID;
            Item.Name = ItemName;
            Item.Batch = Batch;
            Item.BatchID = BatchID;
            Item.MRP = MRP;
            Item.Rate = Rate;
            Item.Category = "";
            Item.PendingPOQty = Qty;
            Item.QtyTolerancePercent = 0;
            Item.Unit = UnitName;
            Item.IsQCRequired = false;
            Item.BatchType = "";
            Item.BatchTypeID = 1;
            Item.UnitID = UnitID;
            Item.QtyOrdered = 0;
            Item.DiscountID = 0;
            Item.PackSize = 0;
            Item.UOMList = unitBL.GetUnitsList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                QOM = a.QOM,
                PackSize = a.PackSize
            }).ToList();
            Item.DiscountList = discountCategoryBL.GetDiscountList().Select(a => new DiscountCategoryModel()
            {
                ID = (int)a.ID,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountType = a.DiscountType,
                Days = (int)a.Days,
                DiscountCategory = a.DiscountCategory
            }).ToList();
            Item.GSTPercentageList = gSTCategoryBL.GetGSTList().Select(a => new GSTCategoryModel()
            {
                ID = (int)a.ID,
                IGSTPercent = (decimal)a.IGSTPercent
            }).ToList();
            Item.SGSTPercent = 0;
            Item.CGSTPercent = 0;
            Item.IGSTPercent = 0;
            Item.GSTPercentage = 0;
            Items.Add(Item);
            return PartialView("~/Areas/Purchase/Views/GRN/_unProcessedPOItems.cshtml", Items);
        }


        public PartialViewResult GetPurchaseOrderTrans(int[] PurchaseOrderIDS)
        {
            try
            {
                GRNModel grnModel = new GRNModel();
                grnModel.UnProcesedPOTransList = new List<PurchaseOrderTransModel>();
                if (PurchaseOrderIDS.Length > 0)
                {
                    foreach (var PurchaseOrderID in PurchaseOrderIDS)
                    {
                        var list = goodsReceiptNoteBL.GetUnProcessedPurchaseOrderTransItemForGrn(PurchaseOrderID).Select(m => new PurchaseOrderTransModel
                        {
                            ID = m.ID,
                            ItemID = m.ItemID,
                            Name = m.Name,
                            ItemCode = m.ItemCode,
                            PartsNumber = m.PartsNumber,
                            Model = m.Model,
                            Remark = m.Remark,
                            CurrencyID = m.CurrencyID,
                            CurrencyName = m.CurrencyName,
                            Discount = m.Discount.HasValue ? m.Discount.Value : 0,
                            DiscountPercent = m.DiscountPercent,
                            IsGST = m.IsVat,
                            IsVat = m.IsVat,
                            VATAmount = m.VATAmount.HasValue ? m.VATAmount.Value : 0,
                            VATPercentage = m.VATPercentage.HasValue ? m.VATPercentage.Value : 0,
                            NetAmount = m.NetAmount,
                            Category = m.Category,
                            PurchaseOrderID = m.PurchaseOrderID,
                            PurchaseOrderNo = m.PurchaseOrderNo,
                            PendingPOQty = m.PendingPOQty,
                            Quantity = m.Quantity,
                            SecondaryQuantity = m.Quantity / m.SecondaryUnitSize,
                            PendingPOSecondaryQty = m.PendingPOSecondaryQty,
                            SecondaryRate = m.SecondaryRate.HasValue ? m.SecondaryRate.Value : 0,
                            SecondaryUnit = m.SecondaryUnit,
                            SecondaryUnitSize = m.SecondaryUnitSize.HasValue ? m.SecondaryUnitSize.Value : 0,
                            QtyTolerancePercent = m.QtyTolerancePercent,
                            Unit = m.Unit,
                            IsQCRequired = m.IsQCRequired,
                            BatchType = m.BatchType,
                            BatchTypeID = m.BatchTypeID,
                            UnitID = m.UnitID,
                            QtyOrdered = m.QtyOrdered,
                            DiscountID = 0,
                            PackSize = m.PackSize,
                            UOMList = unitBL.GetUnitsList().Select(a => new UnitModel()
                            {
                                UOM = a.UOM,
                                ID = a.ID,
                                QOM = a.QOM,
                                PackSize = a.PackSize
                            }).ToList(),
                            DiscountList = discountCategoryBL.GetDiscountList().Select(a => new DiscountCategoryModel()
                            {
                                ID = (int)a.ID,
                                DiscountPercentage = (decimal)a.DiscountPercentage,
                                DiscountType = a.DiscountType,
                                Days = (int)a.Days,
                                DiscountCategory = a.DiscountCategory
                            }).ToList(),
                            GSTPercentageList = gSTCategoryBL.GetGSTList().Select(a => new GSTCategoryModel()
                            {
                                ID = (int)a.ID,
                                IGSTPercent = (decimal)a.IGSTPercent
                            }).ToList(),
                            SGSTPercent = m.SGSTPercent.HasValue ? m.SGSTPercent.Value : 0,
                            CGSTPercent = m.CGSTPercent.HasValue ? m.CGSTPercent.Value : 0,
                            IGSTPercent = m.IGSTPercent.HasValue ? m.IGSTPercent.Value : 0,
                            GSTPercentage = m.GSTPercentage.HasValue ? m.GSTPercentage.Value : 0,
                            SuppOtherCharge = m.SuppOtherCharge,
                            SuppDocAmount = m.SuppDocAmount,
                            SuppShipAmount = m.SuppShipAmount,
                        }).ToList();

                        //ViewBag.TaxPercentages = GstBL.GetGstList();
                        grnModel.UnProcesedPOTransList.AddRange(list);

                        grnModel.SuppOtherCharges += list.Count > 0 ? list.First().SuppOtherCharge : 0;
                        grnModel.SuppDocAmount += list.Count > 0 ? list.First().SuppDocAmount : 0;
                        grnModel.SuppShipAmount += list.Count > 0 ? list.First().SuppShipAmount : 0;

                    }

                }
                return PartialView("~/Areas/Purchase/Views/GRN/_unProcessedPOItems.cshtml", grnModel.UnProcesedPOTransList);

            }
            catch (Exception e)
            {
                throw e;
                // return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GenerateDetails(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            GRNModel grnModel = goodsReceiptNoteBL.GetGRNDetail((int)id).Select(a => new GRNModel()
            {
                Code = a.Code,
                GRNDate = General.FormatDate(a.ReceiptDate, "view"),
                SupplierName = a.SupplierName,
                Warehouse = a.WarehouseName,
                InvoiceNo = a.DeliveryChallanNo,
                InvoiceDate = a.DeliveryChallanDate == null ? "" : General.FormatDate((DateTime)a.DeliveryChallanDate, "view"),
                SupplierID = a.SupplierID,
                IsDraft = a.IsDraft,
                SupplierCode = a.SupplierCode,
                Id = a.ID,
                IsPurchaseCompleted = a.PurchaseCompleted,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                CGSTAmt = a.CGSTAmt,
                RoundOff = a.RoundOff,
                DiscountAmt = a.DiscountAmt,
                GrossAmt = a.GrossAmt,
                SuppFreight = a.SuppFreight,
                PackingForwarding = a.PackingForwarding,
                LocalCustomsDuty = a.LocalCustomsDuty,
                LocalMiscCharge = a.LocalMiscCharge,
                LocalFreight = a.LocalFreight,
                LocalOtherCharges = a.LocalOtherCharges,
                NetAmount = a.NetAmount,
                Remarks = a.Remarks
            }).FirstOrDefault();
            List<GRNTransItemBO> grnItemsBO = goodsReceiptNoteBL.GetGRNItems((int)id);
            GRNItemModel grnItem;
            grnModel.grnItems = new List<GRNItemModel>();
            foreach (var m in grnItemsBO)
            {
                grnItem = new GRNItemModel()
                {
                    ItemID = m.ItemID,
                    ItemName = m.ItemName,
                    Batch = m.Batch,
                    PurchaseOrderQty = m.PurchaseOrderQty,
                    ReceivedQty = m.ReceivedQty,
                    Unit = m.Unit,
                    Remarks = m.Remarks,
                    UnitID = m.UnitID,
                    LooseRate = m.LooseRate,
                    LooseQty = m.LooseQty,
                    PurchaseRate = m.PurchaseRate,
                    OfferQty = m.OfferQty,
                    DiscountID = m.DiscountID,
                    DiscountAmount = m.DiscountAmount,
                    DiscountPercent = m.DiscountPercent,
                    RetailMRP = m.RetailMRP,
                    SGSTPercent = (decimal)m.SGSTPercent,
                    CGSTPercent = (decimal)m.CGSTPercent,
                    IGSTPercent = (decimal)m.IGSTPercent,
                    GSTPercentage = (decimal)m.GSTPercentage,
                    IGSTAmt = (decimal)m.IGSTAmt,
                    SGSTAmt = (decimal)m.SGSTAmt,
                    CGSTAmt = (decimal)m.CGSTAmt,
                };
                if (m.ExpiryDate != null)
                {
                    grnItem.ExpiryDate = General.FormatDate((DateTime)m.ExpiryDate, "view");
                }
                grnModel.grnItems.Add(grnItem);
            }

            return View(grnModel);


        }
        public ActionResult GenerateEdit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            GRNModel grnModel = goodsReceiptNoteBL.GetGRNDetail((int)id).Select(a => new GRNModel()
            {
                Code = a.Code,
                GRNDate = General.FormatDate(a.ReceiptDate),
                SupplierName = a.SupplierName,
                Warehouse = a.WarehouseName,
                InvoiceNo = a.DeliveryChallanNo,
                InvoiceDate = a.DeliveryChallanDate == null ? "" : General.FormatDate((DateTime)a.DeliveryChallanDate, "view"),
                SupplierID = a.SupplierID,
                IsDraft = a.IsDraft,
                SupplierCode = a.SupplierCode,
                Id = a.ID,
                IsPurchaseCompleted = a.PurchaseCompleted,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                CGSTAmt = a.CGSTAmt,
                RoundOff = a.RoundOff,
                DiscountAmt = a.DiscountAmt,
                GrossAmt = a.GrossAmt,
                NetAmount = a.NetAmount,
                WarehouseID = a.WarehouseID


            }).FirstOrDefault();
            grnModel.DiscountList = discountCategoryBL.GetDiscountList().Select(a => new DiscountCategoryModel()
            {
                ID = (int)a.ID,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountType = a.DiscountType,
                Days = (int)a.Days,
                DiscountCategory = a.DiscountCategory
            }).ToList();
            grnModel.UOMList = unitBL.GetUnitsList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                QOM = a.QOM,
                PackSize = a.PackSize
            }).ToList();
            List<GRNTransItemBO> grnItemsBO = goodsReceiptNoteBL.GetGRNItems((int)id);
            GRNItemModel grnItem;
            grnModel.grnItems = new List<GRNItemModel>();
            foreach (var m in grnItemsBO)
            {
                grnItem = new GRNItemModel()
                {
                    ItemID = m.ItemID,
                    ItemName = m.ItemName,
                    Batch = m.Batch,
                    PurchaseOrderQty = m.PurchaseOrderQty,
                    ReceivedQty = m.ReceivedQty,
                    Unit = m.Unit,
                    Remarks = m.Remarks,
                    LooseRate = m.LooseRate,
                    LooseQty = m.LooseQty,
                    PurchaseRate = m.PurchaseRate,
                    OfferQty = m.OfferQty,
                    DiscountAmount = m.DiscountAmount,
                    DiscountPercent = m.DiscountPercent,
                    RetailMRP = m.RetailMRP,
                    BatchID = m.BatchID,
                    POTransID = m.POTransID,
                    PurchaseOrderID = m.PurchaseOrderID,
                    PendingPOQty = m.PendingPOQty,
                    DiscountList = discountCategoryBL.GetDiscountList().Select(a => new DiscountCategoryModel()
                    {
                        ID = (int)a.ID,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                        DiscountType = a.DiscountType,
                        Days = (int)a.Days,
                        DiscountCategory = a.DiscountCategory
                    }).ToList(),
                    DiscountID = m.DiscountID,
                    UnitID = m.UnitID,
                    PackSize = m.PackSize,
                    SGSTPercent = (decimal)m.SGSTPercent,
                    CGSTPercent = (decimal)m.CGSTPercent,
                    IGSTPercent = (decimal)m.IGSTPercent,
                    GSTPercentage = (decimal)m.GSTPercentage,
                    IGSTAmt = (decimal)m.IGSTAmt,
                    SGSTAmt = (decimal)m.SGSTAmt,
                    CGSTAmt = (decimal)m.CGSTAmt,
                };
                if (m.ExpiryDate != null)
                {
                    grnItem.ExpiryDate = General.FormatDate((DateTime)m.ExpiryDate);
                }
                grnModel.grnItems.Add(grnItem);
            }
            ViewBag.TaxPercentages = GstBL.GetGstList();
            grnModel.WarehoueList = warehouseBL.GetWareHouses();

            return View(grnModel);


        }

        public PartialViewResult QRCodeGenerate()
        {
            return PartialView();
        }

        public JsonResult GetItemForQRCodeGenerator(int GRNID)
        {
            try
            {
                List<GRNTransItemBO> QRCodeItems = goodsReceiptNoteBL.GetItemForQRCodeGenerator(GRNID);

                return Json(new { Status = "success", Data = QRCodeItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveQRCode(GRNModel model)
        {
            try
            {
                List<GRNBO> QRCodeList = new List<GRNBO>();
                if (model.QRCodeItems != null)
                {
                    GRNBO Item;

                    foreach (var item in model.QRCodeItems)
                    {
                        Item = new GRNBO()
                        {
                            BatchID = item.BatchID,
                            ItemID = item.ItemID,
                            QRCode = item.QRCode
                        };
                        QRCodeList.Add(Item);
                    }
                }
                goodsReceiptNoteBL.SaveQRCode(QRCodeList);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Purchase", "GRN", "SaveQR", model.Id, e);
                return Json(new
                {
                    Status = "failure",
                    Message = e.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        //GenerateV4 For Janaushadhi view(Batch Creation At Grn Save time)
        public ActionResult GenerateV4()
        {
            GRNModel grnModel = new GRNModel();
            grnModel.WarehoueList = warehouseBL.GetWareHouses();
            grnModel.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultRMStore"));
            grnModel.IsBarCodeGenerator = goodsReceiptNoteBL.IsBarCodeGenerator();
            grnModel.ShippingStateID = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            grnModel.UnitList = new SelectList(
               new List<SelectListItem>
                {
                    new SelectListItem { Text = "", Value = "0"}

                 }, "Value", "Text");
            grnModel.DiscountList = discountCategoryBL.GetDiscountList().Select(a => new DiscountCategoryModel()
            {
                ID = (int)a.ID,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountType = a.DiscountType,
                Days = (int)a.Days,
                DiscountCategory = a.DiscountCategory
            }).ToList();
            grnModel.UOMList = unitBL.GetUnitsList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                QOM = a.QOM,
                PackSize = a.PackSize
            }).ToList();
            grnModel.GSTPercentageList = gSTCategoryBL.GetGSTList().Select(a => new GSTCategoryModel()
            {
                ID = (int)a.ID,
                IGSTPercent = (decimal)a.IGSTPercent
            }).ToList();
            grnModel.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
            grnModel.BusinessCategoryID = Convert.ToInt16(generalBL.GetConfig("DefaultBusinessCategory"));
            //grnModel.GSTPercentageList = new SelectList(gSTCategoryBL.GetGSTList(), "ID", "IGSTPercent");
            ViewBag.TaxPercentages = GstBL.GetGstList();
            grnModel.GRNNo = generalBL.GetSerialNo("GRN", "Code");
            return View(grnModel);
        }

        public ActionResult GenerateEditV4(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            GRNModel grnModel = goodsReceiptNoteBL.GetGRNDetail((int)id).Select(a => new GRNModel()
            {
                Code = a.Code,
                GRNDate = General.FormatDate(a.ReceiptDate),
                SupplierName = a.SupplierName,
                Warehouse = a.WarehouseName,
                InvoiceNo = a.DeliveryChallanNo,
                InvoiceDate = a.DeliveryChallanDate == null ? "" : General.FormatDate((DateTime)a.DeliveryChallanDate, "view"),
                SupplierID = a.SupplierID,
                IsDraft = a.IsDraft,
                SupplierCode = a.SupplierCode,
                Id = a.ID,
                IsPurchaseCompleted = a.PurchaseCompleted,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                CGSTAmt = a.CGSTAmt,
                RoundOff = a.RoundOff,
                DiscountAmt = a.DiscountAmt,
                GrossAmt = a.GrossAmt,
                NetAmount = a.NetAmount,
                WarehouseID = a.WarehouseID


            }).FirstOrDefault();
            grnModel.DiscountList = discountCategoryBL.GetDiscountList().Select(a => new DiscountCategoryModel()
            {
                ID = (int)a.ID,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountType = a.DiscountType,
                Days = (int)a.Days,
                DiscountCategory = a.DiscountCategory
            }).ToList();
            grnModel.UOMList = unitBL.GetUnitsList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                QOM = a.QOM,
                PackSize = a.PackSize
            }).ToList();
            List<GRNTransItemBO> grnItemsBO = goodsReceiptNoteBL.GetGRNItems((int)id);
            GRNItemModel grnItem;
            grnModel.grnItems = new List<GRNItemModel>();
            foreach (var m in grnItemsBO)
            {
                grnItem = new GRNItemModel()
                {
                    ItemID = m.ItemID,
                    ItemName = m.ItemName,
                    Batch = m.Batch,
                    PurchaseOrderQty = m.PurchaseOrderQty,
                    ReceivedQty = m.ReceivedQty,
                    Unit = m.Unit,
                    Remarks = m.Remarks,
                    LooseRate = m.LooseRate,
                    LooseQty = m.LooseQty,
                    PurchaseRate = m.PurchaseRate,
                    OfferQty = m.OfferQty,
                    DiscountAmount = m.DiscountAmount,
                    DiscountPercent = m.DiscountPercent,
                    RetailMRP = m.RetailMRP,
                    BatchID = m.BatchID,
                    POTransID = m.POTransID,
                    PurchaseOrderID = m.PurchaseOrderID,
                    PendingPOQty = m.PendingPOQty,
                    DiscountList = discountCategoryBL.GetDiscountList().Select(a => new DiscountCategoryModel()
                    {
                        ID = (int)a.ID,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                        DiscountType = a.DiscountType,
                        Days = (int)a.Days,
                        DiscountCategory = a.DiscountCategory
                    }).ToList(),
                    DiscountID = m.DiscountID,
                    UnitID = m.UnitID,
                    PackSize = m.PackSize,
                    SGSTPercent = (decimal)m.SGSTPercent,
                    CGSTPercent = (decimal)m.CGSTPercent,
                    IGSTPercent = (decimal)m.IGSTPercent,
                    GSTPercentage = (decimal)m.GSTPercentage,
                    IGSTAmt = (decimal)m.IGSTAmt,
                    SGSTAmt = (decimal)m.SGSTAmt,
                    CGSTAmt = (decimal)m.CGSTAmt,
                    QtyTolerancePercent = (decimal)m.QtyTolerancePercent,
                };
                if (m.ExpiryDate != null)
                {
                    grnItem.ExpiryDate = General.FormatDate((DateTime)m.ExpiryDate);
                }
                grnModel.grnItems.Add(grnItem);
            }
            ViewBag.TaxPercentages = GstBL.GetGstList();
            grnModel.WarehoueList = warehouseBL.GetWareHouses();

            return View(grnModel);


        }

        [HttpPost]
        public ActionResult CreateGRNV4(GRNModel grnModel)
        {
            var result = new List<object>();
            try
            {
                if (grnModel.Id != 0)
                {
                    //Edit
                    //Check whether editable or not
                    GRNBO Temp = goodsReceiptNoteBL.GetGoodsReceiptNoteDetails(grnModel.Id).FirstOrDefault();
                    if (Temp.PurchaseCompleted)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }

                }
                GRNBO grnBO = new GRNBO()
                {
                    ID = grnModel.Id,
                    Code = grnModel.GRNNo,
                    Date = General.ToDateTime(grnModel.GRNDate),
                    SupplierID = grnModel.SupplierID,
                    ReceiptDate = General.ToDateTime(grnModel.GRNDate),
                    DeliveryChallanNo = grnModel.InvoiceNo,
                    WarehouseID = grnModel.WarehouseID,
                    PurchaseCompleted = false,
                    IsCancelled = false,
                    CancelledDate = null,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = GeneralBO.CreatedUserID,
                    IsDraft = grnModel.IsDraft,
                    IGSTAmt = grnModel.IGSTAmt,
                    SGSTAmt = grnModel.SGSTAmt,
                    CGSTAmt = grnModel.CGSTAmt,
                    RoundOff = grnModel.RoundOff,
                    DiscountAmt = grnModel.DiscountAmt,
                    GrossAmt = grnModel.GrossAmt,
                    NetAmount = grnModel.NetAmount
                };
                if (grnModel.InvoiceDate != null)
                {
                    grnBO.DeliveryChallanDate = General.ToDateTime(grnModel.InvoiceDate);
                }
                List<GRNTransItemBO> grnItemsBO = new List<GRNTransItemBO>();
                GRNTransItemBO grnItemBO;
                if (grnModel.grnItems != null)
                {
                    foreach (var item in grnModel.grnItems)
                    {

                        grnItemBO = new GRNTransItemBO()
                        {
                            PurchaseOrderID = item.PurchaseOrderID,
                            POTransID = item.POTransID,
                            ItemID = item.ItemID,
                            Batch = item.Batch,
                            ReceivedQty = item.ReceivedQty,
                            LooseQty = item.LooseQty,
                            LooseRate = item.LooseRate,
                            Remarks = (item.Remarks == null) ? "" : item.Remarks,
                            UnitID = item.UnitID,
                            PurchaseOrderQty = item.PurchaseOrderQty,
                            PurchaseRate = item.PurchaseRate,
                            OfferQty = item.OfferQty,
                            DiscountID = item.DiscountID,
                            DiscountAmount = item.DiscountAmount,
                            DiscountPercent = item.DiscountPercent,
                            BatchID = item.BatchID,
                            IGSTPercent = item.IGSTPercent,
                            CGSTPercent = item.CGSTPercent,
                            SGSTPercent = item.SGSTPercent,
                            IGSTAmt = item.IGSTAmt,
                            SGSTAmt = item.SGSTAmt,
                            CGSTAmt = item.CGSTAmt,

                        };
                        if (item.ExpiryDate != null)
                        {
                            grnItemBO.ExpiryDate = General.ToDateTime(item.ExpiryDate);
                        }

                        grnItemsBO.Add(grnItemBO);
                    }
                }
                int GRNID = goodsReceiptNoteBL.CreateGRNV4(grnBO, grnItemsBO);
                if (GRNID > 0)
                {
                    return Json(new { Status = "success", data = grnModel, GRNID }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Unknown Error" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "GRN", "Save", grnModel.Id, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetPurchaseOrdersV4(int SupplierID, int BusinessCategoryID)
        {
            try
            {
                GRNModel grnModel = new GRNModel();

                grnModel.UnProcesedPOList = goodsReceiptNoteBL.GetUnProcessedPurchaseOrderForGrnV4(SupplierID, BusinessCategoryID).Select(m => new PurchaseOrderModel()
                {
                    ID = m.ID,
                    PurchaseOrderNo = m.PurchaseOrderNo,
                    PurchaseOrderDate = General.FormatDate(m.PurchaseOrderDate),
                    RequestedBy = m.RequestedBy,
                    SupplierName = m.SupplierName,
                    NetAmt = m.NetAmt
                }
                ).ToList();
                return Json(grnModel.UnProcesedPOList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult GetUnProcessedGRNV4(int BusinessCategoryID, string Hint = "")
        {
            List<GRNBO> grnBOList;

            grnBOList = goodsReceiptNoteBL.GetUnProcessedGRNV4(BusinessCategoryID, Hint);

            return PartialView("~/Areas/Purchase/Views/GRN/_purchaseInvoiceGRNList.cshtml", grnBOList);

        }


        public JsonResult GetUnProcessedGRNAutoCompleteV4(int BusinessCategoryID, string Hint)
        {
            List<GRNBO> grnBOList = new List<GRNBO>();
            grnBOList = goodsReceiptNoteBL.GetUnProcessedGRNV4(BusinessCategoryID, Hint).Select(a => new GRNBO()
            {
                ID = a.ID,
                Code = a.Code,
                Date = a.Date,
                SupplierID = a.SupplierID,
                SupplierName = a.SupplierName,
                LocationID = a.LocationID,
                Location = a.Location,
                PurchaseOrderDate = (DateTime)a.PurchaseOrderDate,
                DeliveryChallanNo = a.DeliveryChallanNo,
                IsGSTRegistered = (bool)a.IsGSTRegistered,
                StateID = a.StateID
            }).ToList();

            return Json(new { Status = "success", data = grnBOList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateV3()
        {
            GRNModel grnModel = new GRNModel();
            grnModel.WarehoueList = warehouseBL.GetWareHouses();
            grnModel.WarehouseID = Convert.ToInt32(generalBL.GetConfig("DefaultRMStore"));
            grnModel.IsBarCodeGenerator = goodsReceiptNoteBL.IsBarCodeGenerator();
            grnModel.IsDirectPurchaseInvoice = goodsReceiptNoteBL.IsDirectPurchaseInvoice();
            var getAddress = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault();
            grnModel.ShippingStateID = getAddress != null ? getAddress.StateID : 0;
            grnModel.DiscountList = discountCategoryBL.GetDiscountList().Select(a => new DiscountCategoryModel()
            {
                ID = (int)a.ID,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountType = a.DiscountType,
                Days = (int)a.Days,
                DiscountCategory = a.DiscountCategory
            }).ToList();
            grnModel.UOMList = unitBL.GetUnitsList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                QOM = a.QOM,
                PackSize = a.PackSize
            }).ToList();
            grnModel.GSTPercentageList = gSTCategoryBL.GetGSTList().Select(a => new GSTCategoryModel()
            {
                ID = (int)a.ID,
                IGSTPercent = (decimal)a.IGSTPercent
            }).ToList();
            grnModel.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
            grnModel.BusinessCategoryID = Convert.ToInt16(generalBL.GetConfig("DefaultBusinessCategory"));
            //grnModel.GSTPercentageList = new SelectList(gSTCategoryBL.GetGSTList(), "ID", "IGSTPercent");
            ViewBag.TaxPercentages = GstBL.GetGstList();
            grnModel.GRNNo = generalBL.GetSerialNo("GRN", "Code");
            return View(grnModel);
        }

        public ActionResult GenerateEditV3(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            GRNModel grnModel = goodsReceiptNoteBL.GetGRNDetail((int)id).Select(a => new GRNModel()
            {
                Code = a.Code,
                GRNDate = General.FormatDate(a.ReceiptDate),
                SupplierName = a.SupplierName,
                Warehouse = a.WarehouseName,
                InvoiceNo = a.DeliveryChallanNo,
                InvoiceDate = a.DeliveryChallanDate == null ? "" : General.FormatDate((DateTime)a.DeliveryChallanDate, "view"),
                SupplierID = a.SupplierID,
                IsDraft = a.IsDraft,
                SupplierCode = a.SupplierCode,
                Id = a.ID,
                IsPurchaseCompleted = a.PurchaseCompleted,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                CGSTAmt = a.CGSTAmt,
                RoundOff = a.RoundOff,
                DiscountAmt = a.DiscountAmt,
                GrossAmt = a.GrossAmt,
                NetAmount = a.NetAmount,
                WarehouseID = a.WarehouseID


            }).FirstOrDefault();
            grnModel.DiscountList = discountCategoryBL.GetDiscountList().Select(a => new DiscountCategoryModel()
            {
                ID = (int)a.ID,
                DiscountPercentage = (decimal)a.DiscountPercentage,
                DiscountType = a.DiscountType,
                Days = (int)a.Days,
                DiscountCategory = a.DiscountCategory
            }).ToList();
            grnModel.UOMList = unitBL.GetUnitsList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                QOM = a.QOM,
                PackSize = a.PackSize
            }).ToList();
            List<GRNTransItemBO> grnItemsBO = goodsReceiptNoteBL.GetGRNItems((int)id);
            GRNItemModel grnItem;
            grnModel.grnItems = new List<GRNItemModel>();
            foreach (var m in grnItemsBO)
            {
                grnItem = new GRNItemModel()
                {
                    ItemID = m.ItemID,
                    ItemName = m.ItemName,
                    Batch = m.Batch,
                    PurchaseOrderQty = m.PurchaseOrderQty,
                    ReceivedQty = m.ReceivedQty,
                    Unit = m.Unit,
                    Remarks = m.Remarks,
                    LooseRate = m.LooseRate,
                    LooseQty = m.LooseQty,
                    PurchaseRate = m.PurchaseRate,
                    OfferQty = m.OfferQty,
                    DiscountAmount = m.DiscountAmount,
                    DiscountPercent = m.DiscountPercent,
                    RetailMRP = m.RetailMRP,
                    BatchID = m.BatchID,
                    POTransID = m.POTransID,
                    PurchaseOrderID = m.PurchaseOrderID,
                    PendingPOQty = m.PendingPOQty,
                    DiscountList = discountCategoryBL.GetDiscountList().Select(a => new DiscountCategoryModel()
                    {
                        ID = (int)a.ID,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                        DiscountType = a.DiscountType,
                        Days = (int)a.Days,
                        DiscountCategory = a.DiscountCategory
                    }).ToList(),
                    DiscountID = m.DiscountID,
                    UnitID = m.UnitID,
                    PackSize = m.PackSize,
                    SGSTPercent = (decimal)m.SGSTPercent,
                    CGSTPercent = (decimal)m.CGSTPercent,
                    IGSTPercent = (decimal)m.IGSTPercent,
                    GSTPercentage = (decimal)m.GSTPercentage,
                    IGSTAmt = (decimal)m.IGSTAmt,
                    SGSTAmt = (decimal)m.SGSTAmt,
                    CGSTAmt = (decimal)m.CGSTAmt,
                };
                if (m.ExpiryDate != null)
                {
                    grnItem.ExpiryDate = General.FormatDate((DateTime)m.ExpiryDate);
                }
                grnModel.grnItems.Add(grnItem);
            }
            ViewBag.TaxPercentages = GstBL.GetGstList();
            grnModel.WarehoueList = warehouseBL.GetWareHouses();

            return View(grnModel);


        }

        public ActionResult GenerateDetailsV4(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            GRNModel grnModel = goodsReceiptNoteBL.GetGRNDetail((int)id).Select(a => new GRNModel()
            {
                Code = a.Code,
                GRNDate = General.FormatDate(a.ReceiptDate, "view"),
                SupplierName = a.SupplierName,
                Warehouse = a.WarehouseName,
                InvoiceNo = a.DeliveryChallanNo,
                InvoiceDate = a.DeliveryChallanDate == null ? "" : General.FormatDate((DateTime)a.DeliveryChallanDate, "view"),
                SupplierID = a.SupplierID,
                IsDraft = a.IsDraft,
                SupplierCode = a.SupplierCode,
                Id = a.ID,
                IsPurchaseCompleted = a.PurchaseCompleted,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                CGSTAmt = a.CGSTAmt,
                RoundOff = a.RoundOff,
                DiscountAmt = a.DiscountAmt,
                GrossAmt = a.GrossAmt,
                NetAmount = a.NetAmount,
                PurchaseOrderNo = a.PurchaseOrderNo


            }).FirstOrDefault();
            List<GRNTransItemBO> grnItemsBO = goodsReceiptNoteBL.GetGRNItems((int)id);
            GRNItemModel grnItem;
            grnModel.grnItems = new List<GRNItemModel>();
            foreach (var m in grnItemsBO)
            {
                grnItem = new GRNItemModel()
                {
                    ItemID = m.ItemID,
                    ItemName = m.ItemName,
                    Batch = m.Batch,
                    PurchaseOrderQty = m.PurchaseOrderQty,
                    ReceivedQty = m.ReceivedQty,
                    Unit = m.Unit,
                    Remarks = m.Remarks,
                    UnitID = m.UnitID,
                    LooseRate = m.LooseRate,
                    LooseQty = m.LooseQty,
                    PurchaseRate = m.PurchaseRate,
                    OfferQty = m.OfferQty,
                    DiscountID = m.DiscountID,
                    DiscountAmount = m.DiscountAmount,
                    DiscountPercent = m.DiscountPercent,
                    RetailMRP = m.RetailMRP,
                    SGSTPercent = (decimal)m.SGSTPercent,
                    CGSTPercent = (decimal)m.CGSTPercent,
                    IGSTPercent = (decimal)m.IGSTPercent,
                    GSTPercentage = (decimal)m.GSTPercentage,
                    IGSTAmt = (decimal)m.IGSTAmt,
                    SGSTAmt = (decimal)m.SGSTAmt,
                    CGSTAmt = (decimal)m.CGSTAmt,
                };
                if (m.ExpiryDate != null)
                {
                    grnItem.ExpiryDate = General.FormatDate((DateTime)m.ExpiryDate, "view");
                }
                grnModel.grnItems.Add(grnItem);
            }

            return View(grnModel);


        }

        public ActionResult GenerateDetailsV3(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            GRNModel grnModel = goodsReceiptNoteBL.GetGRNDetail((int)id).Select(a => new GRNModel()
            {
                Code = a.Code,
                GRNDate = General.FormatDate(a.ReceiptDate, "view"),
                SupplierName = a.SupplierName,
                Warehouse = a.WarehouseName,
                InvoiceNo = a.DeliveryChallanNo,
                InvoiceDate = a.DeliveryChallanDate == null ? "" : General.FormatDate((DateTime)a.DeliveryChallanDate, "view"),
                SupplierID = a.SupplierID,
                IsDraft = a.IsDraft,
                SupplierCode = a.SupplierCode,
                Id = a.ID,
                IsPurchaseCompleted = a.PurchaseCompleted,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                CGSTAmt = a.CGSTAmt,
                RoundOff = a.RoundOff,
                DiscountAmt = a.DiscountAmt,
                GrossAmt = a.GrossAmt,
                VATAmount = a.VATAmount,
                SuppOtherCharges = a.SuppOtherCharges,
                SuppShipAmount = a.SuppShipAmount,
                SuppDocAmount = a.SuppDocAmount,
                SuppFreight = a.SuppFreight,
                PackingForwarding = a.PackingForwarding,
                LocalCustomsDuty = a.LocalCustomsDuty,
                LocalMiscCharge = a.LocalMiscCharge,
                LocalFreight = a.LocalFreight,
                LocalOtherCharges = a.LocalOtherCharges,
                LocalLandinngCost = a.LocalCustomsDuty + a.LocalMiscCharge + a.LocalFreight + a.LocalOtherCharges,
                Remarks = a.Remarks,
                NetAmount = a.NetAmount,
            }).FirstOrDefault();
            List<GRNTransItemBO> grnItemsBO = goodsReceiptNoteBL.GetGRNItems((int)id);
            GRNItemModel grnItem;
            grnModel.grnItems = new List<GRNItemModel>();
            foreach (var m in grnItemsBO)
            {
                grnItem = new GRNItemModel()
                {
                    ItemID = m.ItemID,
                    ItemCode = m.ItemCode,
                    ItemName = m.ItemName,
                    PartsNumber = m.PartsNumber,
                    Remark = m.Remark,
                    Model = m.Model,
                    Unit = m.Unit,
                    CurrencyID = m.CurrencyID,
                    PurchaseOrderQty = m.PurchaseOrderQty,
                    ReceivedQty = m.ReceivedQty,
                    SecondaryRate = m.SecondaryRate,
                    SecondaryUnit = m.SecondaryUnit,
                    SecondaryUnitSize = m.SecondaryUnitSize,
                    SecondaryReceivedQty = m.SecondaryReceivedQty,
                    SecondaryOfferQty = m.SecondaryOfferQty,
                    SecondaryPurchaseOrderQty = m.SecondaryPurchaseOrderQty,
                    Remarks = m.Remarks,
                    UnitID = m.UnitID,
                    LooseRate = m.LooseRate,
                    LooseQty = m.LooseQty,
                    PurchaseRate = m.PurchaseRate,
                    OfferQty = m.OfferQty,
                    DiscountID = m.DiscountID,
                    GrossAmount = m.GrossAmount,
                    DiscountAmount = m.DiscountAmount,
                    DiscountPercent = m.DiscountPercent,
                    VATAmount = m.VATAmount,
                    VATPercentage = m.VATPercentage,
                    TaxableAmount = m.TaxableAmount,
                    RetailMRP = m.RetailMRP,
                    SGSTPercent = (decimal)m.SGSTPercent,
                    CGSTPercent = (decimal)m.CGSTPercent,
                    IGSTPercent = (decimal)m.IGSTPercent,
                    GSTPercentage = (decimal)m.GSTPercentage,
                    IGSTAmt = (decimal)m.IGSTAmt,
                    SGSTAmt = (decimal)m.SGSTAmt,
                    CGSTAmt = (decimal)m.CGSTAmt,
                    NetAmount = m.NetAmount,
                    BinCode=m.BinCode,
                    PurchaseOrderNo = m.PurchaseOrderNo,
                    // NetAmount = ((decimal)m.IGSTAmt + (decimal)m.CGSTAmt + (decimal)m.SGSTAmt + ((decimal)m.PurchaseRate * (decimal)m.ReceivedQty)) - (m.DiscountAmount)
                };
                if (m.ExpiryDate != null)
                {
                    grnItem.ExpiryDate = General.FormatDate((DateTime)m.ExpiryDate, "view");
                }
                grnModel.grnItems.Add(grnItem);
            }

            var classdata = grnModel.grnItems.Count() > 0 ? counterSalesBL.GetCurrencyDecimalClassByCurrencyID(grnModel.grnItems.First().CurrencyID) : null;
            if (classdata != null)
            {
                grnModel.DecimalPlaces = classdata.DecimalPlaces;
                grnModel.normalclass = classdata.normalclass;
            }
            return View(grnModel);


        }

        public PartialViewResult GetGrnItems(int ID)
        {
            GRNModel model = new GRNModel();
            model.grnItems = goodsReceiptNoteBL.GetGRNItems(ID).Select(m => new GRNItemModel()
            {
                ItemID = m.ItemID,
                ItemName = m.ItemName,
                Batch = m.Batch,
                PurchaseOrderQty = m.PurchaseOrderQty,
                ReceivedQty = m.ReceivedQty,
                Unit = m.Unit,
                BatchID = m.BatchID
            }).ToList();
            return PartialView(model);
        }

        public PartialViewResult GetUnProcessedGRNV3(int SupplierID, string Hint = "")
        {
            List<GRNBO> grnBOList;

            grnBOList = goodsReceiptNoteBL.GetUnProcessedGRNV3(SupplierID, Hint);

            return PartialView("~/Areas/Purchase/Views/GRN/_purchaseInvoiceGRNListV3.cshtml", grnBOList);

        }

        public JsonResult GetUnProcessedGRNAutoCompleteV3(int SupplierID, string Hint)
        {
            List<GRNBO> grnBOList = new List<GRNBO>();
            grnBOList = goodsReceiptNoteBL.GetUnProcessedGRNV3(SupplierID, Hint).Select(a => new GRNBO()
            {
                ID = a.ID,
                Code = a.Code,
                Date = a.Date,
                SupplierID = a.SupplierID,
                SupplierName = a.SupplierName,
                LocationID = a.LocationID,
                Location = a.Location,
                PurchaseOrderDate = (DateTime)a.PurchaseOrderDate,
                DeliveryChallanNo = a.DeliveryChallanNo,
                IsGSTRegistered = (bool)a.IsGSTRegistered,
                StateID = a.StateID
            }).ToList();

            return Json(new { Status = "success", data = grnBOList }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetPurchaseOrderTransV3(int[] PurchaseOrderIDS, string normalclass)
        {
            try
            {
                GRNModel grnModel = new GRNModel();
                grnModel.UnProcesedPOTransList = new List<PurchaseOrderTransModel>();
                if (PurchaseOrderIDS.Length > 0)
                {
                    foreach (var PurchaseOrderID in PurchaseOrderIDS)
                    {
                        var list = goodsReceiptNoteBL.GetUnProcessedPurchaseOrderTransItemForGrn(PurchaseOrderID).Select(m => new PurchaseOrderTransModel
                        {
                            ID = m.ID,
                            ItemID = m.ItemID,
                            ItemCode = m.ItemCode,
                            Name = m.Name,
                            PartsNumber = m.PartsNumber,
                            Model = m.Model,
                            Remark = m.Remark,
                            CurrencyID = m.CurrencyID,
                            CurrencyName = m.CurrencyName,
                            IsGST = m.IsGST,
                            IsVat = m.IsVat,
                            VATAmount = m.VATAmount.HasValue ? m.VATAmount.Value : 0,
                            VATPercentage = m.VATPercentage.HasValue ? m.VATPercentage.Value : 0,
                            Rate = m.Rate.HasValue ? m.Rate.Value : 0,
                            GrossAmount = (m.Rate.HasValue ? m.Rate.Value : 0) * (m.Quantity.HasValue ? m.Quantity.Value : 0),
                            Discount = m.Discount.HasValue ? m.Discount.Value : 0,
                            DiscountPercent = m.DiscountPercent,
                            TaxableAmount = (m.Rate.HasValue ? m.Rate.Value : 0) * (m.Quantity.HasValue ? m.Quantity.Value : 0) - (m.Discount.HasValue ? m.Discount.Value : 0),
                            SuppDocAmount = m.SuppDocAmount,
                            SuppShipAmount = m.SuppShipAmount,
                            SuppOtherCharge = m.SuppOtherCharge,
                            NetAmount = m.NetAmount,
                            Category = m.Category,
                            PurchaseOrderID = m.PurchaseOrderID,
                            PurchaseOrderNo = m.PurchaseOrderNo,
                            PendingPOQty = m.PendingPOQty,
                            PendingPOSecondaryQty = m.PendingPOSecondaryQty,
                            Quantity = m.Quantity,
                            SecondaryQuantity = m.SecondaryQty,
                            SecondaryUnitSize = m.SecondaryUnitSize.HasValue ? m.SecondaryUnitSize.Value : 0,
                            SecondaryUnit = m.SecondaryUnit,
                            SecondaryRate = m.SecondaryRate.HasValue ? m.SecondaryRate.Value : 0,
                            QtyTolerancePercent = m.QtyTolerancePercent,
                            Unit = m.Unit,
                            IsQCRequired = m.IsQCRequired,
                            BatchType = m.BatchType,
                            BatchTypeID = m.BatchTypeID,
                            UnitID = m.UnitID,
                            QtyOrdered = m.QtyOrdered,
                            DiscountID = 0,
                            PackSize = m.PackSize,
                            BinID = m.BinID,
                            BinCode = m.BinCode,
                            UOMList = unitBL.GetUnitsList().Select(a => new UnitModel()
                            {
                                UOM = a.UOM,
                                ID = a.ID,
                                QOM = a.QOM,
                                PackSize = a.PackSize
                            }).ToList(),
                            //DiscountList = discountCategoryBL.GetDiscountList().Select(a => new DiscountCategoryModel()
                            //{
                            //    ID = (int)a.ID,
                            //    DiscountPercentage = (decimal)a.DiscountPercentage,
                            //    DiscountType = a.DiscountType,
                            //    Days = (int)a.Days,
                            //    DiscountCategory = a.DiscountCategory
                            //}).ToList(),
                            //GSTPercentageList = gSTCategoryBL.GetGSTList().Select(a => new GSTCategoryModel()
                            //{
                            //    ID = (int)a.ID,
                            //    IGSTPercent = (decimal)a.IGSTPercent
                            //}).ToList(),
                            SGSTPercent = m.SGSTPercent.HasValue ? m.SGSTPercent.Value : 0,
                            CGSTPercent = m.CGSTPercent.HasValue ? m.CGSTPercent.Value : 0,
                            IGSTPercent = m.IGSTPercent.HasValue ? m.IGSTPercent.Value : 0,
                            GSTPercentage = m.IGSTPercent.HasValue ? m.IGSTPercent.Value : 0,
                            normalclass = normalclass
                        }).ToList();

                        //ViewBag.TaxPercentages = GstBL.GetGstList();
                        grnModel.UnProcesedPOTransList.AddRange(list);
                        grnModel.SuppOtherCharges += list.Count > 0 ? list.First().SuppOtherCharge : 0;
                        grnModel.SuppDocAmount += list.Count > 0 ? list.First().SuppDocAmount : 0;
                        grnModel.SuppShipAmount += list.Count > 0 ? list.First().SuppShipAmount : 0;
                    }
                    ViewBag.SuppOtherCharges = grnModel.SuppOtherCharges;
                    ViewBag.SuppDocAmount = grnModel.SuppDocAmount;
                    ViewBag.SuppShipAmount = grnModel.SuppShipAmount;
                }
                return PartialView("~/Areas/Purchase/Views/GRN/_unProcessedPOItemsV3.cshtml", grnModel.UnProcesedPOTransList);

            }
            catch (Exception e)
            {
                throw e;
                // return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult GRNPrintPdf(int id)
        //{

        //    GRNModel grnModel = goodsReceiptNoteBL.GetGRNDetail((int)id).Select(m => new GRNModel()
        //    {
        //        GRNDate = General.FormatDate(m.ReceiptDate, "view"),
        //        SupplierName = m.SupplierName,
        //        AddressLine1 = m.AddressLine1,
        //        AddressLine2 = m.AddressLine2,
        //        AddressLine3 = m.AddressLine3,
        //        BAddressLine1 = m.BAddressLine1,
        //        BAddressLine2 = m.BAddressLine2,
        //        BAddressLine3 = m.BAddressLine3,
        //        CountryName = m.CountryName,
        //        Email = m.Email,
        //        CurrencyName = m.CurrencyName,
        //        LocalOtherCharges = m.LocalOtherCharges,
        //        NetAmount = (decimal)m.NetAmount,
        //        Remarks = m.Remarks
        //    }).FirstOrDefault();
        //    List<GRNTransItemBO> grnItemsBO = goodsReceiptNoteBL.GetGRNItems((int)id);
        //    GRNItemModel grnItem;
        //    grnModel.grnItems = new List<GRNItemModel>();
        //    foreach (var m in grnItemsBO)
        //    {
        //        grnItem = new GRNItemModel()
        //        {
        //            ItemCode = m.ItemCode,
        //            ItemName = m.ItemName,
        //            PartsNumber = m.PartsNumber,
        //            ReceivedQty = m.ReceivedQty,
        //            PurchaseOrderQty = m.PurchaseOrderQty,
        //            Unit = m.Unit,
        //            PurchaseRate = m.PurchaseRate,
        //            NetAmount = m.NetAmount,
        //            TaxableAmount = m.TaxableAmount,
        //        };
        //        grnModel.grnItems.Add(grnItem);
        //    }

        //    // HTML content with inline styles
        //    string htmlContent = "<html><head><style>body {font-family: Arial, sans-serif; font-size: 9px; ); background-size: cover; background-position: center; margin: 0; padding: 0; } h2 {font-size: 11px; font-weight: bold; margin-left: 100px; margin-bottom: 10px; text-align: left;} h3 {font-size: 9px; margin-top: 5px; margin-bottom: 3px;} table {width: 100%; border-collapse: collapse; margin: 25px;} td, th {padding: 3px;} table.item-details td, table.item-details th {border: 0.5px solid #000;}.cell1 {width: 15%;} .cell2 {width: 35%;}</style></head><body>";


        //    // Report title
        //    htmlContent += "<h2>Goods Receipt Note Report</h2>";//<br/><br/><br/><br/><br/><br/><br/><br/>

        //    // Main details table
        //    htmlContent += "<table cellpadding='5' cellspacing='0' style='font-size: 9px; width: 100%;'>";
        //    htmlContent += "<tr><td class='cell1'><b>GRN Date:</b></td><td class='cell2'>" + System.Net.WebUtility.HtmlEncode(grnModel.GRNDate) + "</td>";
        //    htmlContent += "<td class='cell1'><b>Supplier Name:</b></td><td class='cell2'>" + System.Net.WebUtility.HtmlEncode(grnModel.SupplierName) + "</td></tr>";

        //    htmlContent += "<tr><td><b>Currency Name:</b></td><td>" + System.Net.WebUtility.HtmlEncode(grnModel.CurrencyName) + "</td>";
        //    htmlContent += "<td><b>Ship Address 1:</b></td><td>" + System.Net.WebUtility.HtmlEncode(grnModel.AddressLine1) + "</td></tr>";

        //    htmlContent += "<tr><td><b>Email:</b></td><td>" + System.Net.WebUtility.HtmlEncode(grnModel.Email) + "</td>";
        //    htmlContent += "<td><b>Ship Address 2:</b></td><td>" + System.Net.WebUtility.HtmlEncode(grnModel.AddressLine2) + "</td></tr>";

        //    htmlContent += "<tr><td><b>Bill Address 1:</b></td><td>" + System.Net.WebUtility.HtmlEncode(grnModel.BAddressLine1) + "</td>";
        //    htmlContent += "<td><b>Ship Address 3:</b></td><td>" + System.Net.WebUtility.HtmlEncode(grnModel.AddressLine3) + "</td></tr>";

        //    htmlContent += "<tr><td><b>Bill Address 2:</b></td><td>" + System.Net.WebUtility.HtmlEncode(grnModel.BAddressLine2) + "</td>";
        //    htmlContent += "<td><b>Currency:</b></td><td>" + System.Net.WebUtility.HtmlEncode(grnModel.CurrencyName) + "</td></tr>";

        //    htmlContent += "<tr><td><b>Mobile No:</b></td><td>" + System.Net.WebUtility.HtmlEncode(grnModel.MobileNo) + "</td>";
        //    htmlContent += "<td><b>Net Amount:</b></td><td>" + grnModel.NetAmount.ToString() + "</td></tr>";

        //    htmlContent += "<tr><td><b>Remarks:</b></td><td>" + System.Net.WebUtility.HtmlEncode(grnModel.Remarks) + "</td>";
        //    htmlContent += "<td></td><td></td></tr>";
        //    htmlContent += "</table>";

        //    // Item details table
        //    htmlContent += "<table class='item-details' cellpadding='5' cellspacing='0' style='border-collapse: collapse; font-size: 10px;'><thead>";
        //    htmlContent += "<tr><th>Sl No</th><th>Item Code</th><th>Item Name</th><th>Parts Number</th><th>Unit</th><th>Received Qty</th><th>Purchase Order Qty</th><th>Purchase Rate</th><th>Net Amount</th></tr></thead><tbody>";
        //    int count = 0;
        //    // Add each GRN item to the HTML table
        //    foreach (var item in grnModel.grnItems)
        //    {
        //        count = count + 1;
        //        htmlContent += $"<tr><td>{System.Net.WebUtility.HtmlEncode(count.ToString())}</td><td>{System.Net.WebUtility.HtmlEncode(item.ItemCode)}</td><td>{System.Net.WebUtility.HtmlEncode(item.ItemName)}</td><td>{System.Net.WebUtility.HtmlEncode(item.PartsNumber)}</td><td>{System.Net.WebUtility.HtmlEncode(item.Unit)}</td><td>{item.ReceivedQty}</td><td>{item.PurchaseOrderQty}</td><td>{item.PurchaseRate.ToString()}</td><td>{item.NetAmount.ToString()}</td></tr>";
        //    }

        //    htmlContent += "</tbody></table></body></html>";

        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        // Set the top margin (for example, 50 points)
        //        Document document = new Document(PageSize.A4, marginLeft: 50, marginRight: 30, marginTop: 110, marginBottom: 90);
        //        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

        //        CustomPdfPageEventHelper pdfPageEventHelper = new CustomPdfPageEventHelper();
        //        writer.PageEvent = pdfPageEventHelper;

        //        document.Open();

        //        using (var stringReader = new StringReader(htmlContent))
        //        {
        //            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
        //        }

        //        document.Close();

        //        return File(memoryStream.ToArray(), "application/pdf", "PurchaseRequisition.pdf");
        //    }

        //}

        // GET: Purchase/GRN/Create
        public ActionResult PrintQRCode()
        {
            GRNModel grnModel = new GRNModel();
            grnModel.WarehoueList = warehouseBL.GetWareHouses();
            grnModel.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultRMStore"));
            grnModel.GRNNo = generalBL.GetSerialNo("GRN", "Code");
            return View(grnModel);
        }

        public PartialViewResult GetBatchListForQRCodePrint(int ItemID)
        {
            GRNModel model = new GRNModel();
            model.grnItems = goodsReceiptNoteBL.GetBatchListForQRCodePrint(ItemID).Select(m => new GRNItemModel()
            {
                ItemID = m.ItemID,
                ItemCode = m.ItemCode,
                ItemName = m.ItemName,
                BatchID = m.BatchID,
                Batch = m.Batch,
                RetailMRP = m.RetailMRP,
                ExpiryDate = General.FormatDate((DateTime)m.ExpiryDate),
                Stock = m.Stock,
                PrintingQty = m.PrintingQty
            }).ToList();
            return PartialView(model);
        }
    }
}
