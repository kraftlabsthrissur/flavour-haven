using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;
using Microsoft.Ajax.Utilities;

namespace TradeSuiteApp.Web.Areas.Purchase.Controllers
{

    //[Authorize]
    public class PurchaseRequisitionController : Controller
    {
        private IDropdownContract _dropdown;
        private IPurchaseRequisition purchaseRequisitionBL;
        private IDepartmentContract departmentBL;
        private ICategoryContract categoryBL;
        private IGeneralContract generalBL;

        public PurchaseRequisitionController(IDropdownContract tempDropdown)
        {
            _dropdown = tempDropdown;
            purchaseRequisitionBL = new PurchaseRequisitionBL();
            departmentBL = new DepartmentBL();
            categoryBL = new CategoryBL();
            generalBL = new GeneralBL();
        }

        // GET: Purchase/PurchaseRequisition
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed" };
            return View();
        }

        // GET: Purchase/PurchaseRequisition/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            var outPurchase = this.GetPurchaseRequisitionData(id.Value);
            return View(outPurchase);
        }

        public ActionResult Create()
        {
            PurchaseRequisitionViewModel outPurchase = new PurchaseRequisitionViewModel();
            outPurchase.DDLDepartment = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            var department = departmentBL.GetDepartmentList().FirstOrDefault();
            if (department != null)
            {
                outPurchase.DepartmentFrom = department.ID;
                outPurchase.DepartmentTo = department.ID;
            }
            outPurchase.DDLItemCategory = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            outPurchase.DDLPurchaseCategory = new SelectList(categoryBL.GetPurchaseCategoryList(0), "ID", "Name");
            outPurchase.PurchaseRequisitionNumber = generalBL.GetSerialNo("PurchaseRequisition", "code");
            outPurchase.UnitList = new SelectList(
                                         new List<SelectListItem>
                                         {
                                                new SelectListItem { Text = "", Value = "0"}

                                         }, "Value", "Text");
            return View(outPurchase);
        }

        public JsonResult getItemForAutoComplete(string Areas, string term = "", string ItemCategoryID = "", string PurchaseCategoryID = "")
        {
            //List<ItemBO> _outItems = new List<ItemBO>();
            var ItemCategoryIDInt = (ItemCategoryID != "") ? Convert.ToInt32(ItemCategoryID) : 0;
            var PurchaseCategoryIDInt = (PurchaseCategoryID != "") ? Convert.ToInt32(PurchaseCategoryID) : 0;
            var _outItems = _dropdown.GetItemList(term, ItemCategoryIDInt, PurchaseCategoryIDInt).Select(x => new
            {
                x.ID,
                x.Name,
                x.Code,
                x.RetailMRP,
                x.RetailLooseRate,
                x.VATPercentage,
                x.PartsNumber,
                x.Model,
                x.DeliveryTerm,
                x.Stock,
                x.QtyUnderQC,
                x.PrimaryUnit,
                x.PrimaryUnitID,
                x.PurchaseUnit,
                x.PurchaseUnitID,
                x.QtyOrdered,
                x.GSTPercentage
            }).ToList();
            return Json(_outItems, JsonRequestBehavior.AllowGet);
        }

        // POST: Purchase/PurchaseRequisition/Create
        [HttpPost]
        //  [ValidateAntiForgeryToken] //TODO : how we are implementing this
        public ActionResult Create(PurchaseRequisitionViewModel modal)
        {
            var result = new List<object>();
            try
            {
                // TODO: Add insert logic here
                BusinessObject.RequisitionBO _req = new RequisitionBO();
                if (modal.Id != 0)
                {
                    RequisitionBO obj = new RequisitionBO();
                    var PurchaseRequisitionViewModel = purchaseRequisitionBL.PurchaseRequisitionDetails(modal.Id);
                    obj = PurchaseRequisitionViewModel.FirstOrDefault();
                    if (!obj.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }

                _req.Cancelled = false;
                _req.SalesInquiryID = modal.SalesInquiryID;
                _req.Code = modal.PurchaseRequisitionNumber;
                _req.RequisitionNo = modal.PurchaseRequisitionNumber;
                _req.PurchaseRequisitedCustomer = modal.PurchaseRequisitedCustomer;
                _req.RequisitedCustomerAddress = modal.RequisitedCustomerAddress;
                _req.RequisitedPhoneNumber1 = modal.RequisitedPhoneNumber1;
                _req.RequisitedPhoneNumber2 = modal.RequisitedPhoneNumber2;
                _req.Remarks = modal.Remarks;
                _req.CreatedDate = System.DateTime.Now;
                _req.Date = General.ToDateTime(modal.PrDate);
                _req.FromDepartment = modal.DepartmentFrom.ToString();
                _req.FromDeptID = modal.DepartmentFrom;
                _req.QuotationProcessed = false;
                _req.ToDepartment = modal.DepartmentTo.ToString();
                _req.ToDeptID = modal.DepartmentTo;
                _req.IsDraft = modal.IsDraft;
                _req.ID = modal.Id;
                _req.SupplierID = modal.SupplierID;
                _req.SupplierName = modal.SupplierName;
                
                var ItemList = new List<ItemBO>();
                ItemBO itemBO;
                foreach (var itm in modal.Item)
                {
                    itemBO = new ItemBO();

                    itemBO.ItemID = itm.ItemID;
                    itemBO.ItemTypeID = 1;
                    itemBO.ItemCode = itm.ItemCode;
                    itemBO.ItemName = itm.ItemName;
                    itemBO.Unit = itm.Unit;
                    itemBO.PartsNumber = itm.PartsNumber;
                    itemBO.SalesInquiryItemID = itm.SalesInquiryItemID;
                    itemBO.Remarks = itm.Remarks;
                    itemBO.RetailMRP = itm.MRP.HasValue ? itm.MRP.Value : 0;
                    itemBO.ReqQty = itm.Qty;
                    itemBO.Stock = itm.Stock;
                    itemBO.Remarks = itm.Remarks;
                    itemBO.RequiredStatus = itm.Remarks;
                    itemBO.UnitID = itm.UnitID;
                    if (itm.ExpectedDate != "" && itm.ExpectedDate != null)
                    {
                        itemBO.ExpectedDate = General.ToDateTime(itm.ExpectedDate);
                    }
                    ItemList.Add(itemBO);
                }

                var response = purchaseRequisitionBL.SavePurchaseRequisition(_req, ItemList);

                if (response)
                    return Json("success", JsonRequestBehavior.AllowGet);
                else
                    result.Add(new { ErrorMessage = "Unknown Error" });
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "PurchaseRequisition", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        private PurchaseRequisitionViewModel GetPurchaseRequisitionData(int id)
        {

            PurchaseRequisitionViewModel outPurchase = new PurchaseRequisitionViewModel();
            RequisitionBO obj = purchaseRequisitionBL.PurchaseRequisitionDetails((int)id).FirstOrDefault(); ;
            if (obj == null)
                return null;
            outPurchase.Item = purchaseRequisitionBL.PurchaseRequisitionTransDetails(obj.ID).Select(k => new PurchaseRequisitionItem()
            {
                ItemID = k.ItemID,
                ItemCode = k.ItemCode,
                ItemName = k.ItemName,
                PartsNumber = k.PartsNumber,
                Unit = k.Unit,
                SalesInquiryItemID = k.SalesInquiryItemID,
                UnitID = k.UnitID,
                Stock = k.Stock,
                //QtyUnderQC = k.QtyUnderQC,
                Qty = k.ReqQty,
                //QtyOrdered = k.QtyOrdered,

                Remarks = k.Remarks,
                ItemCategoryID = k.ItemCategoryID,
                ExpectedDate = k.ExpectedDate == null ? "" : General.FormatDate((DateTime)k.ExpectedDate),
                RequiredStatus = k.RequiredStatus,
            }).ToList();

            outPurchase.FullyOrdered = obj.FullyOrdered;
            outPurchase.IsDraft = obj.IsDraft;
            outPurchase.Id = obj.ID;
            outPurchase.DepartmentFrom = obj.FromDeptID;
            outPurchase.DepartmentTo = obj.ToDeptID;
            outPurchase.Code = obj.Code;
            outPurchase.PurchaseRequisitionNumber = obj.RequisitionNo;
            outPurchase.Date = (obj.Date != null) ? General.FormatDate((DateTime)obj.Date) : "";
            outPurchase.FromDepartmentName = obj.FromDepartment;
            outPurchase.ToDepartmentName = obj.ToDepartment;
            outPurchase.PurchaseRequisitedCustomer = obj.PurchaseRequisitedCustomer;
            outPurchase.RequisitedCustomerAddress = obj.RequisitedCustomerAddress;
            outPurchase.RequisitedPhoneNumber1 = obj.RequisitedPhoneNumber1;
            outPurchase.RequisitedPhoneNumber2 = obj.RequisitedPhoneNumber2;
            outPurchase.Remarks = obj.Remarks;
            outPurchase.ItemCategoryID = outPurchase.Item.FirstOrDefault() != null ? outPurchase.Item.FirstOrDefault().ItemCategoryID : 0;
            outPurchase.SupplierID = obj.SupplierID;
            outPurchase.SupplierName = obj.SupplierName;
            return outPurchase;
        }
        // GET: Purchase/PurchaseRequisition/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            try
            {

                PurchaseRequisitionViewModel outPurchase = new PurchaseRequisitionViewModel();

                if (id != null || id == 0)
                {
                    outPurchase = this.GetPurchaseRequisitionData(id.Value);
                    if (outPurchase == null || outPurchase.FullyOrdered || outPurchase.IsDraft == false)
                    {
                        return RedirectToAction("Index");
                    }
                    outPurchase.DDLDepartment = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
                    outPurchase.DDLItemCategory = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
                    outPurchase.DDLPurchaseCategory = new SelectList(categoryBL.GetPurchaseCategoryList(0), "ID", "Name");
                    outPurchase.UnitList = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "", Value = "0" } }, "Value", "Text");
                    return View(outPurchase);
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            catch (Exception e)
            {
                return View("Eror");
            }

        }


        public JsonResult GetPurchaseCategory(int id)
        {
            List<CategoryBO> PurchaseCategories = categoryBL.GetPurchaseCategoryList(id).ToList();
            return Json(new { Status = "success", data = PurchaseCategories }, JsonRequestBehavior.AllowGet);
        }

        // POST: Purchase/PurchaseRequisition/Edit/5
        [HttpPost]
        public ActionResult Edit(PurchaseRequisitionViewModel modal)
        {
            var result = new List<object>();
            try
            {
                // TODO: Add insert logic here
                BusinessObject.RequisitionBO _req = new RequisitionBO();
                if (modal.Id != 0)
                {
                    RequisitionBO obj = new RequisitionBO();
                    var PurchaseRequisitionViewModel = purchaseRequisitionBL.PurchaseRequisitionDetails(modal.Id);
                    obj = PurchaseRequisitionViewModel.FirstOrDefault();
                    if (!obj.IsDraft || obj.FullyOrdered)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                _req.ID = modal.Id;
                _req.FromDeptID = modal.DepartmentFrom;
                _req.ToDeptID = modal.DepartmentTo;
                _req.IsDraft = modal.IsDraft;
                _req.SupplierID = modal.SupplierID;
                _req.SupplierName = modal.SupplierName;
                var ItemList = new List<ItemBO>();

                ItemBO itemBO;
                foreach (var itm in modal.Item)
                {
                    itemBO = new ItemBO();

                    itemBO.ItemID = itm.ItemID;
                    itemBO.ItemTypeID = 1;
                    itemBO.ItemCode = itm.ItemCode;
                    itemBO.ItemName = itm.ItemName;
                    itemBO.PartsNumber = itm.PartsNumber;
                    itemBO.SalesInquiryItemID = itm.SalesInquiryItemID;
                    itemBO.Remarks = itm.Remarks;
                    itemBO.RetailMRP = itm.MRP.HasValue ? itm.MRP.Value : 0;
                    itemBO.ReqQty = itm.Qty;
                    itemBO.Stock = itm.Stock;
                    itemBO.Remarks = itm.Remarks;
                    itemBO.RequiredStatus = itm.Remarks;
                    itemBO.UnitID = itm.UnitID;
                    if (itm.ExpectedDate != "" && itm.ExpectedDate != null)
                    {
                        itemBO.ExpectedDate = General.ToDateTime(itm.ExpectedDate);
                    }
                    ItemList.Add(itemBO);
                }

                bool response = purchaseRequisitionBL.UpdatePurchaseRequisition(_req, ItemList);
                if (response)
                    return Json("success", JsonRequestBehavior.AllowGet);
                else
                    result.Add(new { ErrorMessage = "Unknown Error" });
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "PurchaseRequisition", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetPurchaseRequisitionListForPurchaseOrder(DatatableModel Datatable)
       {
            try
            {
                string TransNoHint = Datatable.Columns[2].Search.Value;
                string TransDateHint = Datatable.Columns[3].Search.Value;
                string SupplierNameHint = Datatable.Columns[4].Search.Value;
                string CategoryNameHint = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = purchaseRequisitionBL.GetPurchaseRequisitionListForPurchaseOrder(Type, TransNoHint, TransDateHint, SupplierNameHint, CategoryNameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetPurchaseRequisitionList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string FromDepartmentHint = Datatable.Columns[3].Search.Value;
                string ToDepartmentHint = Datatable.Columns[4].Search.Value;
                string CategoryNameHint = Datatable.Columns[5].Search.Value;
                string ItemNameHint = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = purchaseRequisitionBL.GetPurchaseRequisitionList(Type, TransNoHint, TransDateHint, FromDepartmentHint, ToDepartmentHint, CategoryNameHint, ItemNameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Save(int id)
        {
            return null;
        }

        public ActionResult SaveAsDraft(PurchaseRequisitionViewModel modal)
        {
            return Create(modal);
        }
        public ActionResult ExportToExcel(int PurchaseRequisitionID)
        {
            var Requisition = this.GetPurchaseRequisitionData(PurchaseRequisitionID);
            var csv = new StringBuilder();
            //csv.AppendLine("Requisition No, Date,From Department,To Department,Customer Name,Customer Address,Phone Number 1 ,Phone Number 2,Item Code,Item Name,Parts Number,Unit,Qty,Net Amount,Remarks");
            csv.AppendLine("Requisition No, Date,Item Name,Parts Number,Unit,Qty,Remarks");
            foreach (var inquiry in Requisition.Item)
            {
                //csv.AppendLine($"{Requisition.Code},{Requisition.Date:yyyy-MM-dd},{Requisition.FromDepartmentName},{Requisition.ToDepartmentName},{Requisition.PurchaseRequisitedCustomer},{Requisition.RequisitedCustomerAddress},{Requisition.RequisitedPhoneNumber1},{Requisition.RequisitedPhoneNumber2},{inquiry.ItemCode},{inquiry.ItemName},{inquiry.PartsNumber},{inquiry.Unit},{inquiry.Qty},{inquiry.NetAmount},{inquiry.Remarks}");
                csv.AppendLine($"{Requisition.Code},{Requisition.Date:yyyy-MM-dd},{inquiry.ItemName},{inquiry.PartsNumber},{inquiry.Unit},{inquiry.Qty},{inquiry.Remarks}");
            }
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "PurchaseRequisition.csv");


        }

        public ActionResult ExportToPDF(int PurchaseRequisitionID)
        {
            var Requisition = this.GetPurchaseRequisitionData(PurchaseRequisitionID);
            // Create HTML content for the PDF
            string htmlContent = "<html><head><style>";
            htmlContent += "body { font-family: 'Times New Roman', serif; font-size: 11px; }"; // Set font style and size for the body
            htmlContent += "h2 { font-size: 14px; font-weight: bold; margin-bottom: 6px; }";   // Set font size for headings
            htmlContent += "h3 { font-size: 12px; margin-top: 10px; margin-bottom: 3px; }";     // Set font size for subheadings
            htmlContent += "table { width: 100%; border-collapse: collapse; margin-top: 5px; }"; // Style for all tables
            htmlContent += "td, th { padding: 3px; vertical-align: top; }"; // Style for table cells and headers without border for the first table
            htmlContent += "table.item-details td, table.item-details th { border: 0.5px solid #808080; }"; // Add border to table cells and headers in the item details table
            htmlContent += "</style></head><body>";

            // Add the report title
            htmlContent += "<h2>Purchase Requisition Report</h2>";

            // Table for main fields, without borders
            htmlContent += "<table cellpadding='5' cellspacing='0' style='font-size: 11px;'>";

            // First row section
            htmlContent += "<tr><td><b>Requisition No:</b></td><td>" + Requisition.Code + "</td>";
            htmlContent += "<td><b>Date:</b></td><td>" + Requisition.Date + "</td></tr>";
            // htmlContent += "<td><b>From Department:</b></td><td>" + Requisition.FromDepartmentName + "</td>";
            //htmlContent += "<td><b>To Department:</b></td><td>" + Requisition.ToDepartmentName + "</td></tr>";

            // Second row section
            //htmlContent += "<tr><td><b>Customer Name:</b></td><td>" + Requisition.PurchaseRequisitedCustomer + "</td>";
            //htmlContent += "<td><b>Phone No 1:</b></td><td>" + Requisition.RequisitedPhoneNumber1 + "</td>";
            //htmlContent += "<td><b>Phone No 2:</b></td><td>" + Requisition.RequisitedPhoneNumber2 + "</td>";
            //htmlContent += "<td><b>Address:</b></td><td>" + Requisition.RequisitedCustomerAddress + "</td></tr>";
            htmlContent += "</table>";

            // List fields section for item details
            htmlContent += "<h3>Item Details</h3>";
            htmlContent += "<table class='item-details' cellpadding='5' cellspacing='0' style='border-collapse: collapse; font-size: 10px;'><thead>";
            //htmlContent += "<tr><th>Item Code</th><th>Item Name</th><th>Parts Number</th><th>Unit</th><th>Qty</th><th>Net Amount</th></tr></thead><tbody>";
            htmlContent += "<tr><th>Item Name</th><th>Parts Number</th><th>Unit</th><th>Qty</th><th>Remarks</th></tr></thead><tbody>";

            //Add each sales inquiry item to the HTML table
            foreach (var inquiry in Requisition.Item)
            {
                htmlContent += $"<tr><td>{inquiry.ItemName}</td><td>{inquiry.PartsNumber}</td><td>{inquiry.Unit}</td><td>{inquiry.Qty}</td><td>{inquiry.Remarks}</td></tr>";
                //htmlContent += $"<tr><td>{inquiry.ItemCode}</td><td>{inquiry.ItemName}</td><td>{inquiry.PartsNumber}</td><td>{inquiry.Unit}</td><td>{inquiry.Qty}</td><td>{inquiry.Qty}</td><td>{inquiry.NetAmount}</td></tr>";
            }

            htmlContent += "</tbody></table></body></html>";

            // Convert the HTML content to a PDF
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                using (var stringReader = new StringReader(htmlContent))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
                }

                document.Close();

                // Return the generated PDF file
                return File(memoryStream.ToArray(), "application/pdf", "PurchaseRequisition.pdf");
            }
        }
    }
}
