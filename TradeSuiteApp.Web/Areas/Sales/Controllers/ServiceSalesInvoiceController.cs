using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Sales.Models;
using PresentationContractLayer;
using BusinessLayer;
using BusinessObject;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;


namespace TradeSuiteApp.Web.Areas.Sales.Controllers
{
    public class ServiceSalesInvoiceController : Controller
    {
        private ICustomerContract customerBL;
        private ISalesInvoice salesInvoiceBL;
        private IAddressContract addressBL;
        private IGeneralContract generalBL;
        private IServiceSalesInvoiceContract servicesalesInvoiceBL;
        private IPaymentModeContract paymentModeBL;
        private ICategoryContract categroyBL;

        public ServiceSalesInvoiceController()
        {
            customerBL = new CustomerBL();
            salesInvoiceBL = new SalesInvoiceBL();
            addressBL = new AddressBL();
            generalBL = new GeneralBL();
            servicesalesInvoiceBL = new ServiceSalesInvoiceBL();
            paymentModeBL = new PaymentModeBL();
            categroyBL = new CategoryBL();

        }

        public ActionResult Index()
        {
            SalesInvoiceModel ServiceInvoice = new SalesInvoiceModel();
            return View(ServiceInvoice);
        }


        public ActionResult Create()
        {
            SalesInvoiceModel ServiceInvoice = new SalesInvoiceModel();
            ServiceInvoice.InvoiceDate = General.FormatDate(DateTime.Now);
            ServiceInvoice.InvoiceNo = generalBL.GetSerialNo("ServiceSalesInvoice", "Code");
            ServiceInvoice.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            ServiceInvoice.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", 0, ""), "AddressID", "AddressLine1");
            ServiceInvoice.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", 0, ""), "AddressID", "AddressLine1");
            ServiceInvoice.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            ServiceInvoice.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", ServiceInvoice.PaymentModeID);
            ServiceInvoice.Items = new List<SalesInvoiceItemModel>();
            ServiceInvoice.DiscountPercentageList = categroyBL.GetDiscountCategory().Select(item => new CategoryBO()
            {
                ID = item.ID,
                Name = item.Name,
                Value = item.Value
            }).ToList();
            return View(ServiceInvoice);
        }

        [HttpPost]
        public ActionResult Save(SalesInvoiceModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    SalesInvoiceBO Temp = servicesalesInvoiceBL.GetServiceSalesInvoice(model.ID, GeneralBO.LocationID);
                    if (!Temp.IsDraft || Temp.IsCancelled || Temp.IsProcessed)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                SalesInvoiceBO Invoice = model.MapToBO();
                List<SalesItemBO> Items = model.Items.MapToBO();
                List<SalesAmountBO> AmountDetails = model.AmountDetails.MapToBO();

                if (servicesalesInvoiceBL.Save(Invoice, Items, AmountDetails) != 0)
                {
                    return Json(new { Status = "success", Message = "Saved successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "failure", Message = "Failed to save" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Sales", "ServiceSalesInvoice", "Save", model.ID, e);
                return Json(new { Status = "failure", Message = result, Text = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Sales/ServiceSalesInvoice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            SalesInvoiceModel salesInvoice = new SalesInvoiceModel();

            int SalesInvoiceID = (int)id;
            salesInvoice = servicesalesInvoiceBL.GetServiceSalesInvoice(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();
            salesInvoice.Items = servicesalesInvoiceBL.GetServiceSalesInvoiceItems(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();
            salesInvoice.AmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();
            salesInvoice.IsCancelable = salesInvoiceBL.IsCancelable(SalesInvoiceID);

            return View(salesInvoice);
        }

        // GET: Sales/ServiceSalesInvoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            SalesInvoiceModel salesInvoice = new SalesInvoiceModel();

            int SalesInvoiceID = (int)id;
            salesInvoice = servicesalesInvoiceBL.GetServiceSalesInvoice(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();
            if (!salesInvoice.IsDraft || salesInvoice.IsCanceled || salesInvoice.IsProcessed)
            {
                return RedirectToAction("Index");
            }
            salesInvoice.Items = servicesalesInvoiceBL.GetServiceSalesInvoiceItems(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();
            salesInvoice.AmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();
            salesInvoice.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            salesInvoice.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            //proformaInvoice.SalesTypeList = new SelectList();
            salesInvoice.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;

            salesInvoice.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", salesInvoice.CustomerID, ""), "AddressID", "AddressLine1");
            salesInvoice.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", salesInvoice.CustomerID, ""), "AddressID", "AddressLine1");
            salesInvoice.DiscountPercentageList = categroyBL.GetDiscountCategory().Select(item => new CategoryBO()
            {
                ID = item.ID,
                Name = item.Name,
                Value = item.Value
            }).ToList();
            return View(salesInvoice);
        }

        [HttpPost]
        public JsonResult GetServiceSalesInvoiceList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string DateHint = Datatable.Columns[2].Search.Value;
                string CustomerNameHint = Datatable.Columns[3].Search.Value;
                string LocationHint = Datatable.Columns[4].Search.Value;
                string NetAmountHint = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);
                DatatableResultBO resultBO = servicesalesInvoiceBL.GetServiceSalesInvoiceList(Type, CodeHint, DateHint, CustomerNameHint, LocationHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "ServiceSalesInvoice", "GetServiceSalesInvoiceList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(SalesInvoiceModel model)
        {
            return Save(model);
        }

        public JsonResult ServiceSalesInvoicePrintPdf(int Id)
        {
            return null;
        }

        public ActionResult Cancel(int SalesInvoiceID)
        {
            try
            {
                if (salesInvoiceBL.IsCancelable(SalesInvoiceID))
                {
                    servicesalesInvoiceBL.Cancel(SalesInvoiceID);
                    return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "ProformaInvoice", "Cancel", 0, e);
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
