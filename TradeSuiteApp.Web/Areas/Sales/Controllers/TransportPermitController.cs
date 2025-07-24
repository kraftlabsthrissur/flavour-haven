using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PresentationContractLayer;
using BusinessLayer;
using TradeSuiteApp.Web.Areas.Sales.Models;
using TradeSuiteApp.Web.Utils;
using BusinessObject;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Sales.Controllers
{
    public class TransportPermitController : Controller
    {
        private ITransportPermitContract transportPermitBL;
        private IGeneralContract generalBL;
        public TransportPermitController()
        {
            transportPermitBL = new TransportPermitBL();
            generalBL = new GeneralBL();
        }


        // GET: Sales/TransportPermit
        public ActionResult Index()
        {
            return View();
        }

        // GET: Sales/TransportPermit/Details/5
        public ActionResult Details(int id)
        {
            TransportPermitModel permit = transportPermitBL.GetTransportPermit(id).Select(m => new TransportPermitModel()
            {

                TransNo = m.TransNo,
                ValidFromDate = General.FormatDate(m.ValidFromDate, "view"),
                ValidToDate = General.FormatDate(m.ValidToDate, "view"),
                DriverName = m.DriverName,
                VehicleNumber = m.VehicleNumber,
                ID = m.ID


            }).FirstOrDefault();
            permit.Items = transportPermitBL.GetTransportPermitTrans(id).Select(m => new TransportPermitItemModel()
            {

                TransNo = m.TransNo,
                Type = m.Type,
                TransDate = General.FormatDate(m.TransDate, "view"),
                CustomerName = m.CustomerName,
                District = m.District

            }).ToList();
            return View(permit);
        }

        // GET: Sales/TransportPermit/Create
        public ActionResult Create()
        {
            TransportPermitModel permit = new TransportPermitModel();
            permit.FromDate = General.FormatDate(DateTime.Now);
            permit.ToDate = General.FormatDate(DateTime.Now);
            permit.TransNo = generalBL.GetSerialNo("Transport Permit", "Code");
            permit.ValidFromDate = General.FormatDate(DateTime.Now);
            permit.ValidToDate = General.FormatDate(DateTime.Now);
            return View(permit);
        }

        // POST: Sales/TransportPermit/Create
        [HttpPost]
        public ActionResult Create(TransportPermitModel model)
        {
            return View();
        }

        // GET: Sales/TransportPermit/Edit/5
        [HttpPost]
        public JsonResult TransportPermit(TransportPermitModel model)
        {
            TransportPermitBO permitBO = new TransportPermitBO();
            permitBO.StockTransferNoFromID = model.StockTransferNoFromID;
            permitBO.StockTransferNoToID = model.StockTransferNoToID;
            permitBO.SalesInvoiceNoToID = model.SalesInvoiceNoToID;
            permitBO.SalesInvoiceNoFromID = model.SalesInvoiceNoFromID;
            permitBO.Type = model.Type;
            permitBO.FromDate = General.ToDateTime(model.FromDate);
            permitBO.ToDate = General.ToDateTime(model.ToDate);

            TransportPermitModel permit = new TransportPermitModel();
            //DateTime fromdate = General.ToDateTime(model.FromDate);
            //DateTime todate = General.ToDateTime(model.ToDate);
            permit.Items = transportPermitBL.PendingPermitList(permitBO).Select(x => new TransportPermitItemModel()
            {
                TransNo = x.TransNo,
                TransDate = General.FormatDate(x.TransDate),
                TransID = x.TransID,
                CustomerName = x.CustomerName,
                CustomerID = x.CustomerID,
                LocationID = x.LocationID,
                LocationName = x.LocationName,
                District = x.District,
                DistrictID = (int)x.DistrictID,
                Type = x.Type,
                ItemID = x.ItemID,
                Quantity = x.Quantity
            }).ToList();
            return Json(new { Status = "success", Data = permit.Items }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SaveAndPrint(TransportPermitModel model)
        {
            try
            {
                TransportPermitBO permitBO = new TransportPermitBO();
                permitBO.TransNo = model.TransNo;
                permitBO.DriverName = model.DriverName;
                permitBO.VehicleNumber = model.VehicleNumber;
                permitBO.ValidToDate = General.ToDateTime(model.ValidToDate);
                permitBO.ValidFromDate = General.ToDateTime(model.ValidFromDate);
                permitBO.FromDate = General.ToDateTime(model.FromDate);
                permitBO.ToDate = General.ToDateTime(model.ToDate);
                var ItemList = new List<TransportPermitItemBO>();
                TransportPermitItemBO itemBO;
                foreach (var itm in model.Items)
                {
                    itemBO = new TransportPermitItemBO();
                    itemBO.TransDate = General.ToDateTime(itm.TransDate);
                    itemBO.TransNo = itm.TransNo;
                    itemBO.LocationID = itm.LocationID;
                    itemBO.DistrictID = itm.DistrictID;
                    itemBO.CustomerID = itm.CustomerID;
                    itemBO.TransID = itm.TransID;
                    itemBO.Type = itm.Type;
                    itemBO.ItemID = itm.ItemID;
                    itemBO.BatchTypeID = itm.BatchTypeID;
                    itemBO.Quantity = itm.Quantity;
                    ItemList.Add(itemBO);
                }
                int outId = transportPermitBL.SaveTransportPermit(permitBO, ItemList);
                return Json(new { Status = "success", data = outId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "TransportPermit", "SaveAndPrint", model.ID, e);
                return Json(new { Status = "faiilure" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTransportPermitListForDataTable(DatatableModel Datatable)
        {
            try
            {
                string TransNo = Datatable.Columns[1].Search.Value;
                string ValidFromdate = Datatable.Columns[2].Search.Value;
                string ValidTodate = Datatable.Columns[3].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = transportPermitBL.GetTransportPermitListForDataTable(TransNo, ValidFromdate, ValidTodate, SortField, SortOrder, Offset, Limit);
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
