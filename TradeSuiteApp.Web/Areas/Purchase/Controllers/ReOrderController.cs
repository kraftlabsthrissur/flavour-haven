using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Admin.Controllers;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using TradeSuiteApp.Web.Areas.Reports.Controllers;
using TradeSuiteApp.Web.Utils;
using TradeSuiteApp.Web.Areas.Reports.Models;
using Microsoft.Reporting.WebForms;
using DataAccessLayer.DBContext;
using System.IO;

namespace TradeSuiteApp.Web.Areas.Purchase.Controllers
{
    public class ReOrderController : Controller
    {
        string AppName;
        string RDLCPath;
        protected ReportViewer reportViewer;

        protected ReportParameter CompanyNameParam;
        protected ReportParameter Address1Param;
        protected ReportParameter Address2Param;
        protected ReportParameter Address3Param;
        protected ReportParameter Address4Param;
        protected ReportParameter Address5Param;
        protected ReportParameter FromDateParam;
        protected ReportParameter ToDateParam;
        protected ReportParameter ReportNameParam;
        protected ReportParameter ImagePathParam;
        protected ReportParameter UserParam;
        protected ReportParameter MonthParam;
        protected ReportParameter FilterParam;
        protected ReportParameter DateAsOnParam;
        protected ReportParameter GSTNoParam;
        protected ReportParameter CINNoParam;
        protected ReportParameter PINParam;
        protected ReportParameter LandLine1Param;
        protected ReportParameter MobileNoParam;
       
        private IReOrderContract reorderBL;
        private IPurchaseOrder purchaseOrderBL;
        private IAddressContract addressBL;

        public ReOrderController()
        {
            reorderBL = new ReOrderBL();
            purchaseOrderBL = new PurchaseOrderBL();
            addressBL = new AddressBL();
            reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.AsyncRendering = false;
            reportViewer.ZoomMode = ZoomMode.FullPage;
            reportViewer.BackColor = System.Drawing.Color.White;
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.KeepSessionAlive = true;

            string a = reportViewer.ResolveClientUrl("test");
            AppName = System.Configuration.ConfigurationManager.AppSettings["AppName"];
            RDLCPath = "Areas\\Reports\\RDLC\\";

            CompanyNameParam = new ReportParameter("CompanyName", GeneralBO.CompanyName);
            Address1Param = new ReportParameter("Address1", GeneralBO.Address1);
            Address2Param = new ReportParameter("Address2", GeneralBO.Address2);
            Address3Param = new ReportParameter("Address3", GeneralBO.Address3);
            Address4Param = new ReportParameter("Address4", GeneralBO.Address4);
            Address5Param = new ReportParameter("Address5", GeneralBO.Address5);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            GSTNoParam = new ReportParameter("GSTNo", GeneralBO.GSTNo);
            CINNoParam = new ReportParameter("CINNo", GeneralBO.CINNo);
            PINParam = new ReportParameter("PIN", GeneralBO.PIN);
            LandLine1Param = new ReportParameter("LandLine1", GeneralBO.LandLine1);
            MobileNoParam = new ReportParameter("MobileNo", GeneralBO.MobileNo);
             FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());
     ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
  ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.PurchaseOrder);

    }
    // GET: Purchase/ReOrder
    public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            ReOrderModel model = new ReOrderModel();
            model.ItemTypeList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "All", Value ="All", },
                                                 new SelectListItem { Text = "Medicine", Value = "Medicine"},
                                                 new SelectListItem { Text = "Non-Medicine", Value = "Non-Medicine"},
                                                 }, "Value", "Text");
            model.ItemType = "All";
            ReOrderBO reOrderBO = reorderBL.GetReOrderConfigvalues();
            model.OrderDays = reOrderBO.OrderDays;
            model.ReOrderDays = reOrderBO.ReOrderDays;
            return View(model);
        }

        public PartialViewResult ReOrderList(int ReOrderDays, int OrderDays, string ItemType)
        {
            ReOrderModel model = new ReOrderModel();
            model.Items = reorderBL.ReOrderList(ReOrderDays, OrderDays, ItemType).Select(a => new ReOrderItemModel()
            {
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                Stock = a.Stock,
                ReOrderQty = a.ReOrderQty,
                ReOrderQtyFull = a.ReOrderQtyFull,
                LastPurchasedDate = General.FormatDate(a.LastPurchasedDate),
                Unit = a.Unit,
                UnitID = a.UnitID,
                Supplier = a.Supplier,
                Rate = a.Rate,
                LastPurchasedSupplier = a.Supplier,
                OrderedQty = a.OrderedQty,
                LastPurchaseQty = a.LastPurchaseQty,
                LastPurchaseOfferQty = a.LastPurchaseOfferQty,
                SupplierList = new SelectList(reorderBL.GetReOrderSupplierList(a.ItemID), "ID", "Name"),
                SupplierID = a.SupplierID,
                PurchaseUnit = a.PurchaseUnit
                //TreatmentRoomList = new SelectList(treatmentScheduleBL.GetTreatmentRoomDetails(), "TreatmentRoomID", "TreatmentRoom"),
                //TherapistList = new SelectList(treatmentScheduleBL.GetTherapistDetails(), "TherapistID", "TherapistName"),
            }).ToList();
            return PartialView(model);
        }

        public ActionResult Save(ReOrderModel model)
        {
            try
            {
                ReOrderBO ReOrderBO = new ReOrderBO()
                {
                    ReOrderDays = model.ReOrderDays,
                    OrderDays = model.OrderDays,
                    ItemType = model.ItemType
                };


                List<ReOrderItemBO> Items = new List<ReOrderItemBO>();
                if (model.Items != null)
                {
                    ReOrderItemBO ReOrderItem;
                    foreach (var item in model.Items)
                    {
                        ReOrderItem = new ReOrderItemBO()
                        {
                            ItemID = item.ItemID,
                            ItemName = item.ItemName,
                            Stock = item.Stock,
                            ReOrderQty = item.ReOrderQty,
                            //LastPurchasedDate = General.ToDateTime(item.LastPurchasedDate),
                            Unit = item.Unit,
                            UnitID = item.UnitID,
                            Supplier = item.Supplier,
                            Rate = item.Rate,
                            SupplierID = item.SupplierID,
                            IsOrdered = item.IsOrdered,
                            Qty = item.Qty
                        };

                        Items.Add(ReOrderItem);
                    }
                }
               string ids= reorderBL.Save(ReOrderBO, Items);
              //  var ctrl = new PurchaseController(iDropdownContract);
               // ctrl.ControllerContext = ControllerContext;

                //var mail = new MailController();
                //mail.ControllerContext = ControllerContext;
                //string subject = "Automaticaly generated";
                //string body = "This is an automaticaly generated mail.";
                //string ToMailID = "neethu@Kraftlabs.com";

                ////call action

                //string[] idlist = ids.Split(',');
                //foreach (string id in idlist)
                //{
                //    if (id!="")
                //    {
                //       string url = PurchaseOrderPrint(Convert.ToInt32(id));
                //        mail.SendMailToUser(url, "PurchaseOrder", Convert.ToInt32(id), subject, body, ToMailID);
                //    }
                //}
                return Json(new { Status = "success", Data = ids }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public string PurchaseOrderPrint(int Id)
        {
            
             Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PurchaseOrder = purchaseOrderBL.GetPurchaseOrder(Id);
            List<SpGetPurchaseOrderDetails_Result> purchaseOrders = new List<SpGetPurchaseOrderDetails_Result>();
            SpGetPurchaseOrderDetails_Result purchaseOrder = new SpGetPurchaseOrderDetails_Result()
            {
                CGSTAmt = PurchaseOrder.CGSTAmt,
                DeliveryWithin = PurchaseOrder.DeliveryWithin,
                IGSTAmt = PurchaseOrder.IGSTAmt,
                NetAmt = (decimal)PurchaseOrder.NetAmt,
                SGSTAmt = PurchaseOrder.SGSTAmt,
                PurchaseOrderDate = PurchaseOrder.PurchaseOrderDate,
                PurchaseOrderNo = PurchaseOrder.PurchaseOrderNo,
                SupplierID = PurchaseOrder.SupplierID,
                SupplierName = PurchaseOrder.SupplierName,
                PaymentMode = PurchaseOrder.PaymentMode,
                PaymentWithin = (int)PurchaseOrder.PaymentWithin,
                TermsOfPrice = PurchaseOrder.TermsOfPrice,
                BillingLocation = PurchaseOrder.BillingLocation,
                SupplierReferenceNo = PurchaseOrder.SupplierReferenceNo,
                Remarks = PurchaseOrder.Remarks,
            };
            purchaseOrders.Add(purchaseOrder);
            var PurchaseOrderTrans = purchaseOrderBL.GetPurchaseOrderItems(Id).Select(a => new SpGetPurchaseOrderTransDetails_Result()
            {
                Amount = a.Amount,
                CGSTAmt = a.CGSTAmt,
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                ItemName = a.Name,
                QtyMet = a.QtyMet,
                Quantity = a.Quantity,
                NetAmount = (decimal)a.NetAmount,
                Rate = a.Rate,
                Remarks = a.Remarks,
                Unit = a.Unit,
            }).ToList();
            var BillingAddress = addressBL.GetBillingAddress("Supplier", PurchaseOrder.SupplierID, "").ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("PurchaseOrderDetailPagePrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam, ImagePathParam, GSTNoParam, CINNoParam, PINParam, LandLine1Param, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderPrintPdfDataSet", purchaseOrders));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PurchaseOrderTransPrintPdfDataSet", PurchaseOrderTrans));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SalesInvoiceBillingAddressPrintPdfDataSet", BillingAddress));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            string FileName = "PurchaseOrderPrint" + Id + ".pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/PurchaseOrder/"), FileName);
            string URL = "/Outputs/PurchaseOrder/" + FileName;
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }

            return URL;
        }

        protected string GetReportPath(string RDLCName)
        {
            System.Web.Routing.RouteData RouteData = Request.RequestContext.RouteData;
            string Controller = "Purchase";

            string ReportPath = Request.MapPath(Request.ApplicationPath) + RDLCPath + "_" + AppName + "\\" + Controller + "\\" + RDLCName + ".rdlc";

            if (!System.IO.File.Exists(ReportPath))
            {
                ReportPath = Request.MapPath(Request.ApplicationPath) + RDLCPath + Controller + "\\" + RDLCName + ".rdlc";
            }

            return ReportPath;
        }

    }
}