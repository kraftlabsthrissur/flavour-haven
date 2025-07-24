using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using Microsoft.Reporting.WebForms;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Sales.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Sales.Controllers
{
    public class GatePassController : Controller
    {
        private IGeneralContract generalBL;
        private IGatePassContract gatepassBL;
        private IFleetContract fleetBL;
        private IDriverContract driverBL;
        private ISalesInvoice salesInvoiceBL;
        private IStockIssueContract stockIssueBL;
        SalesEntities db;
        ReportViewer reportViewer;
        ReportParameter p1, p2, p3, p4, p5, p6, p7, p8, p9, p10;
        public GatePassController()
        {
            generalBL = new GeneralBL();
            gatepassBL = new GatePassBL();
            fleetBL = new FleetBL();
            driverBL = new DriverBL();
            salesInvoiceBL = new SalesInvoiceBL();
            stockIssueBL = new StockIssueBL();
        }
        // GET: Sales/GatePass
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        // GET: Sales/GatePass/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Page Not Found");
            }
            GatePassModel gatePassModel = gatepassBL.GetGatePassDetails((int)id).Select(m => new GatePassModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                TransDate = General.FormatDate(m.TransDate),
                FromDate = General.FormatDate(m.FromDate),
                ToDate = General.FormatDate(m.FromDate),
                Salesman = m.Salesman,
                VehicleNo = m.VehicleNo,
                VehicleNoID = m.VehicleNoID,
                DespatchDateTime = General.FormatDate((DateTime)m.DespatchDateTime),
                Time = m.Time,
                DriverName = m.DriverName,
                DriverID = m.DriverID,
                DrivingLicense = m.DrivingLicense,
                VehicleOwner = m.VehicleOwner,
                TransportingAgency = m.TransportingAgency,
                HelperName = m.HelperName,
                Area = m.Area,
                StartingKilometer = m.StartingKilometer,
                IssuedBy = m.IssuedBy,
                IsDraft = m.IsDraft,
                BagCount = m.BagCount,
                CanCount = m.CanCount,
                BoxCount = m.BoxCount,
                TotalAmount = m.TotalAmount
            }).FirstOrDefault();

            List<GatePassItemsBO> gatePassItems = gatepassBL.GetGatePassTransDetails((int)id);
            GatePassItems gatepassItems;
            gatePassModel.GatepassItems = new List<GatePassItems>();
            foreach (var items in gatePassItems)
            {
                gatepassItems = new GatePassItems()
                {
                    GatePassTransID = items.GatePassTransID,
                    ID = items.ID,
                    TransNo = items.TransNo,
                    TransDate = General.FormatDate(items.TransDate),
                    Name = items.Name,
                    Amount = items.Amount,
                    Area = items.Area,
                    NoOfCans = items.NoOfCans,
                    NoOfboxes = items.NoOfboxes,
                    NoOfBags = items.NoOfBags,
                    DeliveryDate = items.DeliveryDate == null ? "" : General.FormatDate((DateTime)items.DeliveryDate, "view"),
                };
                gatePassModel.GatepassItems.Add(gatepassItems);
                gatePassModel.DDLDriver = new SelectList(driverBL.GetDriverList(), "ID", "Name");
                gatePassModel.DDLVehicleNo = new SelectList(fleetBL.GetFleetList(), "ID", "VehicleNo");
            }
            return View(gatePassModel);
        }

        // GET: Sales/GatePass/Create
        public ActionResult Create()
        {
            GatePassModel gatepass = new GatePassModel();
            gatepass.TransNo = generalBL.GetSerialNo("GatePass", "Code");
            gatepass.FromDate = General.FormatDate(DateTime.Now);
            gatepass.ToDate = General.FormatDate(DateTime.Now);
            gatepass.TransDate = General.FormatDate(DateTime.Now);
            gatepass.DespatchDateTime = General.FormatDate(DateTime.Now);
            gatepass.DDLDriver = new SelectList(driverBL.GetDriverList(), "ID", "Name");
            gatepass.DDLVehicleNo = new SelectList(fleetBL.GetFleetList(), "ID", "VehicleNo");
            gatepass.invoiceitems = new List<SalesInvoiceModel>();
            gatepass.TypeList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Sales Invoice", Value = "SalesInvoice"},
                                                 new SelectListItem { Text = "Stock Issue", Value = "StockIssue"},
                                                 }, "Value", "Text");
            return View(gatepass);
        }

        // POST: Sales/GatePass/Save
        [HttpPost]
        public ActionResult Save(GatePassModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    GatePassBO Temp = gatepassBL.GetGatePassDetails(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft || Temp.IsCancelled)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                GatePassBO gatePass = new GatePassBO()
                {
                    ID = model.ID,
                    TransNo = model.TransNo,
                    TransDate = General.ToDateTime(model.TransDate),
                    FromDate = General.ToDateTime(model.FromDate),
                    ToDate = General.ToDateTime(model.ToDate),
                    Salesman = model.Salesman,
                    VehicleNoID = model.VehicleNoID,
                    Time = model.Time,
                    DriverID = model.DriverID,
                    DrivingLicense = model.DrivingLicense,
                    VehicleOwner = model.VehicleOwner,
                    TransportingAgency = model.TransportingAgency,
                    HelperName = model.HelperName,
                    Area = model.Area,
                    StartingKilometer = model.StartingKilometer,
                    IssuedBy = model.IssuedBy,
                    BagCount = model.BagCount,
                    CanCount = model.CanCount,
                    BoxCount = model.BoxCount,
                    IsDraft = model.IsDraft,
                    TotalAmount = model.TotalAmount,
                    VehicleNo = model.VehicleNo,
                };
                if (gatePass.DespatchDateTime != null)
                {
                    gatePass.DespatchDateTime = General.ToDateTime(model.DespatchDateTime);
                }
                List<GatePassItemsBO> GatePassItems = new List<GatePassItemsBO>();
                GatePassItemsBO GatePassItem;
                foreach (var item in model.GatepassItems)
                {
                    GatePassItem = new GatePassItemsBO()
                    {
                        ID = item.ID,
                        Type = item.Type,
                        Area = item.Area,
                        NoOfBags = item.NoOfBags,
                        NoOfboxes = item.NoOfboxes,
                        NoOfCans = item.NoOfCans
                    };
                    GatePassItems.Add(GatePassItem);
                }
                var outId = gatepassBL.SaveGatePass(gatePass, GatePassItems);
                return Json(new { Status = "success", data = gatePass }, JsonRequestBehavior.AllowGet);
                //return Json(new { Status = "success", data = gatePass }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Sales", "GatePass", "Save", model.ID, e);
                return Json(new { Status = "failure", Message = result, Text = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(GatePassModel model)
        {
            return Save(model);
        }

        // GET: Sales/GatePass/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return View("Page Not Found");
            }
            GatePassModel gatePassModel = gatepassBL.GetGatePassDetails((int)id).Select(m => new GatePassModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                TransDate = General.FormatDate(m.TransDate),
                FromDate = General.FormatDate(m.FromDate),
                ToDate = General.FormatDate(m.FromDate),
                Salesman = m.Salesman,
                VehicleNo = m.VehicleNo,
                VehicleNoID = m.VehicleNoID,
                DespatchDateTime = General.FormatDate((DateTime)m.DespatchDateTime),
                Time = m.Time,
                DriverName = m.DriverName,
                DriverID = m.DriverID,
                DrivingLicense = m.DrivingLicense,
                VehicleOwner = m.VehicleOwner,
                TransportingAgency = m.TransportingAgency,
                HelperName = m.HelperName,
                Area = m.Area,
                StartingKilometer = m.StartingKilometer,
                IssuedBy = m.IssuedBy,
                BagCount = m.BagCount,
                CanCount = m.CanCount,
                BoxCount = m.BoxCount,
                TotalAmount = m.TotalAmount,
                IsCancelled = m.IsCancelled,
                IsDraft = m.IsDraft
            }).FirstOrDefault();
            if (!gatePassModel.IsDraft || gatePassModel.IsCancelled)
            {
                return RedirectToAction("Index");
            }
            List<GatePassItemsBO> gatePassItems = gatepassBL.GetGatePassTransDetails((int)id);
            GatePassItems gatepassItems;
            gatePassModel.GatepassItems = new List<GatePassItems>();
            foreach (var items in gatePassItems)
            {
                gatepassItems = new GatePassItems()
                {
                    GatePassTransID = items.GatePassTransID,
                    ID = items.ID,
                    TransNo = items.TransNo,
                    TransDate = General.FormatDate(items.TransDate),
                    Name = items.Name,
                    Amount = items.Amount,
                    Area = items.Area,
                    Type = items.Type,
                    NoOfCans = items.NoOfCans,
                    NoOfboxes = items.NoOfboxes,
                    NoOfBags = items.NoOfBags,
                    DeliveryDate = items.DeliveryDate == null ? "" : General.FormatDate((DateTime)items.DeliveryDate, "view"),
                };
                gatePassModel.GatepassItems.Add(gatepassItems);
                gatePassModel.TransNo = generalBL.GetSerialNo("GatePass", "Code");
                gatePassModel.FromDate = General.FormatDate(DateTime.Now);
                gatePassModel.ToDate = General.FormatDate(DateTime.Now);
                gatePassModel.DespatchDateTime = General.FormatDate(DateTime.Now);
                gatePassModel.DDLDriver = new SelectList(driverBL.GetDriverList(), "ID", "Name");
                gatePassModel.DDLVehicleNo = new SelectList(fleetBL.GetFleetList(), "ID", "VehicleNo");
                gatePassModel.TypeList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "SalesInvoice", Value = "SalesInvoice"},
                                                 new SelectListItem { Text = "StockIssue", Value = "StockIssue"},
                                                 }, "Value", "Text");
                gatePassModel.invoiceitems = new List<SalesInvoiceModel>();
            }
            return View(gatePassModel);
        }

        // POST: Sales/GatePass/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sales/GatePass/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        public void Print(int ID)
        {
            p9 = new ReportParameter("ReportName", App_LocalResources.Reports.GatePassReport);

            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            p1 = new ReportParameter("CompanyName", GeneralBO.CompanyName);
            p2 = new ReportParameter("Address1", GeneralBO.Address1);
            p3 = new ReportParameter("Address2", GeneralBO.Address2);
            p4 = new ReportParameter("Address3", GeneralBO.Address3);
            p5 = new ReportParameter("Address4", GeneralBO.Address4);
            p6 = new ReportParameter("Address5", GeneralBO.Address5);

            //ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            p9 = new ReportParameter("ReportName", App_LocalResources.Reports.GatePassReport);

            reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = System.Web.UI.WebControls.Unit.Percentage(900);
            reportViewer.Height = System.Web.UI.WebControls.Unit.Percentage(900);
            reportViewer.BackColor = System.Drawing.Color.White;
            reportViewer.LocalReport.EnableExternalImages = true;
            db = new SalesEntities();
            var GPPrint = db.SpGetGatePassPrint(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/GatePass.rdlc";
            ReportParameter p8 = new ReportParameter("ReportName", App_LocalResources.Reports.GatePassReport);
            p10 = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p8, p9 });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("GatePassPrintDataSet", GPPrint));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            Response.Buffer = true;
            Response.Clear();
            Response.Charset = "";
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.BinaryWrite(bytes);
            //Response.AddHeader("content-disposition", "attachment; filename= GrnLocalReport" + "." + extension);
            //Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
            Response.Flush(); // send it to the client to download  
            Response.End();
            ViewBag.ReportViewer = reportViewer;

        }
        public ActionResult DeliveryDateUpdate(int? ID)
        {
            if (ID == null)
            {
                return View("Page Not Found");
            }

            GatePassModel gatePassModel = gatepassBL.GetGatePassDetails((int)ID).Select(m => new GatePassModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                FromDate = General.FormatDate(m.FromDate),
                ToDate = General.FormatDate(m.FromDate),
                Salesman = m.Salesman,
                VehicleNo = m.VehicleNo,
                VehicleNoID = m.VehicleNoID,
                DespatchDateTime = General.FormatDate((DateTime)m.DespatchDateTime),
                Time = m.Time,
                DriverName = m.DriverName,
                DriverID = m.DriverID,
                DrivingLicense = m.DrivingLicense,
                VehicleOwner = m.VehicleOwner,
                TransportingAgency = m.TransportingAgency,
                HelperName = m.HelperName,
                Area = m.Area,
                StartingKilometer = m.StartingKilometer,
                IssuedBy = m.IssuedBy,
                IsDraft = m.IsDraft,
                BagCount = m.BagCount,
                CanCount = m.CanCount,
                BoxCount = m.BoxCount,
                TotalAmount = m.TotalAmount
            }).FirstOrDefault();

            List<GatePassItemsBO> gatePassItems = gatepassBL.GetGatePassTransDetails((int)ID);
            GatePassItems gatepassItems;
            gatePassModel.GatepassItems = new List<GatePassItems>();
            foreach (var items in gatePassItems)
            {
                gatepassItems = new GatePassItems()
                {
                    GatePassTransID = items.GatePassTransID,
                    ID = items.ID,
                    TransNo = items.TransNo,
                    TransDate = General.FormatDate(items.TransDate),
                    Name = items.Name,
                    Amount = items.Amount,
                    Area = items.Area,
                    Type = items.Type,
                    NoOfCans = items.NoOfCans,
                    NoOfboxes = items.NoOfboxes,
                    NoOfBags = items.NoOfBags,
                    DeliveryDate = items.DeliveryDate == null ? "" : General.FormatDate((DateTime)items.DeliveryDate),
                };
                gatePassModel.GatepassItems.Add(gatepassItems);
                gatePassModel.DDLDriver = new SelectList(driverBL.GetDriverList(), "ID", "Name");
                gatePassModel.DDLVehicleNo = new SelectList(fleetBL.GetFleetList(), "ID", "VehicleNo");

            }
            return View(gatePassModel);
        }

        public JsonResult UpdateDeliveryDate(GatePassModel model)
        {

            List<GatePassItemsBO> GatePassItems = new List<GatePassItemsBO>();
            GatePassItemsBO GatePassItem;
            foreach (var item in model.GatepassItems)
            {
                GatePassItem = new GatePassItemsBO()
                {
                    GatePassTransID = item.GatePassTransID,
                    DeliveryDate = General.ToDateTime(item.DeliveryDate)
                };

                GatePassItems.Add(GatePassItem);
            }

            var outId = gatepassBL.SaveDeliveryDate(GatePassItems);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cancel(int ID, string Table)
        {
            generalBL.Cancel(ID, Table);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGatePassDocumentBetweenDates(string FromDate, string ToDate, string Type)
        {
            try
            {
                DateTime From = General.ToDateTime(FromDate);
                DateTime To = General.ToDateTime(ToDate);

                List<GatePassItems> GatePassItemsBetweenDates = gatepassBL.getGatePassItems(From, To, Type).Select(a => new GatePassItems()
                {
                    ID = a.ID,
                    TransNo = a.TransNo,
                    TransDate = Convert.ToString(General.FormatDate(a.TransDate)),
                    Name = a.Name,
                    Amount = a.Amount,
                    Area = a.Area,
                    Type = a.Type,
                    NoOfBags = a.NoOfBags,
                    NoOfboxes = a.NoOfboxes,
                    NoOfCans = a.NoOfCans
                }).ToList();
                return Json(GatePassItemsBetweenDates, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "GatePass", "GetGatePassDocumentBetweenDates", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public void CLPrint(int ID)
        {
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            p1 = new ReportParameter("CompanyName", GeneralBO.CompanyName);
            p2 = new ReportParameter("Address1", GeneralBO.Address1);
            p3 = new ReportParameter("Address2", GeneralBO.Address2);
            p4 = new ReportParameter("Address3", GeneralBO.Address3);
            p5 = new ReportParameter("Address4", GeneralBO.Address4);
            p6 = new ReportParameter("Address5", GeneralBO.Address5);

            p8 = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            p9 = new ReportParameter("ReportName", App_LocalResources.Reports.CollectionListReport);

            reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = System.Web.UI.WebControls.Unit.Percentage(900);
            reportViewer.Height = System.Web.UI.WebControls.Unit.Percentage(900);
            reportViewer.BackColor = System.Drawing.Color.White;

            db = new SalesEntities();
            var CLPrint = db.SpGetCollectionList(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/CollectonList.rdlc";
            ReportParameter p7 = new ReportParameter("ReportName", App_LocalResources.Reports.CollectionListReport);
            p10 = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7 });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CollectionListDataSet", CLPrint));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            Response.Buffer = true;
            Response.Clear();
            Response.Charset = "";
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.BinaryWrite(bytes);
            //Response.AddHeader("content-disposition", "attachment; filename= GrnLocalReport" + "." + extension);
            //Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
            Response.Flush(); // send it to the client to download  
            Response.End();

        }
        public void EwayBill(int ID)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            //FromDateParam = new ReportParameter("FromDate", DateTime.Now.ToShortDateString());

            p1 = new ReportParameter("CompanyName", GeneralBO.CompanyName);
            p2 = new ReportParameter("Address1", GeneralBO.Address1);
            p3 = new ReportParameter("Address2", GeneralBO.Address2);
            p4 = new ReportParameter("Address3", GeneralBO.Address3);
            p5 = new ReportParameter("Address4", GeneralBO.Address4);
            p6 = new ReportParameter("Address5", GeneralBO.Address5);

            //ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
            p9 = new ReportParameter("ReportName", App_LocalResources.Reports.EwayReport);
            reportViewer = new ReportViewer();
            db = new SalesEntities();
            var Eway = db.SpGetEwayBill(
                        ID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/Eway.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("EwayDataSet", Eway));
            p10 = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p9, p10 });
            byte[] bytes = reportViewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename= GrnLocalReport" + "." + extension);
            Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
            Response.Flush(); // send it to the client to download  
            Response.End();
        }

        public JsonResult PrintGatePass(GatePassModel gatepass)
        {
            string URL = "";
            //ArrayList myList = new ArrayList();
            List<string> urlList = new List<string>();
            foreach (var item in gatepass.GatepassItems)
            {
                if (item.Type == "SalesInvoice")
                {
                    URL = salesInvoiceBL.GetPrintTextFile(item.ID);
                    urlList.Add(URL);
                }
                else
                {
                    URL = stockIssueBL.GetPrintTextFile(item.ID);
                    urlList.Add(URL);
                }
            }
            URL = MultipleFilesIntoSingleFile(urlList);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        private string MultipleFilesIntoSingleFile(List<string> urlList)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            List<string> inputFilePaths = new List<string>();
            foreach (var url in urlList)
            {
                inputFilePaths.Add(path + url);
            }
            string outputFilePath = path + "/Outputs/GatePass/GatePass.txt";
            using (var outputStream = System.IO.File.Create(outputFilePath))
            {
                foreach (var inputFilePath in inputFilePaths)
                {
                    using (var inputStream = System.IO.File.OpenRead(inputFilePath))
                    {
                        inputStream.CopyTo(outputStream);
                    }
                    Console.WriteLine(inputFilePath);
                }
            }
            return Request.Url.GetLeftPart(UriPartial.Authority) + "/Outputs/GatePass/GatePass.txt";
        }


        public JsonResult GetGatePassListForDataTable(DatatableModel Datatable)
        {
            try
            {
                string TransNo = Datatable.Columns[1].Search.Value;
                string TransDate = Datatable.Columns[2].Search.Value;
                string VehicleNo = Datatable.Columns[3].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = gatepassBL.GetGatePassListForDataTable(Type, TransNo, TransDate, VehicleNo, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

