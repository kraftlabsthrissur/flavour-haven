using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Areas.Stock.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Stock.Controllers
{
    public class ServiceItemReceiptController : Controller
    {

        private IWareHouseContract warehouseBL;
        private IBatchTypeContract batchTypeBL;
        private IServiceItemReceiptContract serviceItemReceiptBL;
        private ILocationContract locationBL;
        private IStockIssueContract stockIssueBL;
        private IGeneralContract generalBL;
        private IAddressContract addressBL;

        public ServiceItemReceiptController()
        {
            locationBL = new LocationBL();
            warehouseBL = new WarehouseBL();
            batchTypeBL = new BatchTypeBL();
            serviceItemReceiptBL = new ServiceItemReceiptBL();
            stockIssueBL = new StockIssueBL();
            generalBL = new GeneralBL();
            addressBL = new AddressBL();
        }
        // GET: Stock/ServiceItemReceipt
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            StockReceiptViewModel StockReceipt = new StockReceiptViewModel();
            List<WareHouseBO> PremisesList = warehouseBL.GetWareHouses();

            StockReceipt.ReceiptNo = generalBL.GetSerialNo("ServiceItemReceipt", "IntraLocation");
            StockReceipt.Date = General.FormatDate(DateTime.Now);
            StockReceipt.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            StockReceipt.ReceiptLocationID = GeneralBO.LocationID;
            StockReceipt.IssueLocationID = locationBL.GetHeadLocation(GeneralBO.LocationID).ID;
            StockReceipt.PremiseList = new SelectList(PremisesList, "ID", "Name");
            if (PremisesList.Count() == 1)
            {
                StockReceipt.ReceiptPremiseID = PremisesList.FirstOrDefault().ID;
            }
            PremisesList = warehouseBL.GetWareHousesByLocation(StockReceipt.IssueLocationID);
            StockReceipt.IssuePremiseList = new SelectList(PremisesList, "ID", "Name");
            if (PremisesList.Count() == 1)
            {
                StockReceipt.IssuePremiseID = PremisesList.FirstOrDefault().ID;
            }
            else if (StockReceipt.IssueLocationID != GeneralBO.LocationID)
            {
                StockReceipt.IssuePremiseID = PremisesList.FirstOrDefault().ID;
            }
            StockReceipt.LocationList = new SelectList(locationBL.GetTransferableLocationList(), "ID", "Name");
            StockReceipt.LocationStateID = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;

            StockReceipt.IssueLocationList = locationBL.GetTransferableLocationList().Select(a => new LocationModel()
            {
                ID = a.ID,
                Name = a.Name,
                StateID = a.StateID
            }).ToList();

            return View(StockReceipt);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            else
            {
                bool IsExist = serviceItemReceiptBL.IsServiceOrStockReceipt((int)id, "Service");
                if (IsExist == false)
                {
                    return View("PageNotFound");
                }
                else
                {
                    try
                    {
                        StockReceiptViewModel StockReceipt = serviceItemReceiptBL.GetServiceItemReceiptDetail((int)id).Select(a => new StockReceiptViewModel()
                        {
                            ID = a.ID,
                            ReceiptNo = a.ReceiptNo,
                            Date = General.FormatDate(a.Date, "view"),
                            ReceiptPremiseName = a.ReceiptPremiseName,
                            IssueLocationName = a.IssueLocationName,
                            IssuePremiseName = a.IssuePremiseName,
                            ReceiptLocationName = a.ReceiptLocationName,
                            BatchType = a.BatchType,
                            NetAmount = a.NetAmount
                        }).FirstOrDefault();
                        StockReceipt.Item = serviceItemReceiptBL.GetServiceItemReceiptTrans((int)id).Select(a => new StockReceiptItem()
                        {
                            Name = a.Name,
                            Unit = a.Unit,
                            IssueQty = a.IssueQty,
                            ReceiptQty = a.ReceiptQty,
                            RequestedQty = a.RequestedQty,
                            BatchName = a.BatchName,
                            BatchType = a.BatchType,
                            Rate = a.Rate,
                            BasicPrice = a.BasicPrice,
                            GrossAmount = a.GrossAmount,
                            TradeDiscountPercent = a.TradeDiscountPercent,
                            TradeDiscount = a.TradeDiscount,
                            TaxableAmount = a.TaxableAmount,
                            CGSTPercentage = a.CGSTPercentage,
                            SGSTPercentage = a.SGSTPercentage,
                            IGSTPercentage = a.IGSTPercentage,
                            CGSTAmount = a.CGSTAmount,
                            SGSTAmount = a.SGSTAmount,
                            IGSTAmount = a.IGSTAmount,
                            NetAmount = a.NetAmount
                        }).ToList();
                        return View(StockReceipt);
                    }
                    catch (Exception e)
                    {
                        return View(e);
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Save(StockReceiptViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                StockReceiptBO receipt = new StockReceiptBO();
                receipt.ReceiptNo = model.ReceiptNo;
                receipt.Date = General.ToDateTime(model.Date);
                receipt.ReceiptType = model.ReceiptType;
                receipt.NetAmount = model.NetAmount;
                receipt.IsService = true;
                List<StockReceiptItemBO> ItemList = new List<StockReceiptItemBO>();
                StockReceiptItemBO stockReceiptItemBO;
                foreach (var itm in model.Item)
                {
                    stockReceiptItemBO = new StockReceiptItemBO();
                    stockReceiptItemBO.StockIssueTransID = itm.StockIssueTransID;
                    stockReceiptItemBO.StockIssueID = itm.StockIssueID;
                    stockReceiptItemBO.ItemID = itm.ItemID;
                    stockReceiptItemBO.IssueQty = itm.IssueQty;
                    stockReceiptItemBO.ReceiptQty = itm.ReceiptQty;
                    stockReceiptItemBO.BatchID = itm.BatchID;
                    stockReceiptItemBO.BatchTypeID = itm.BatchTypeID;
                    stockReceiptItemBO.IssuePremiseID = itm.IssuePremiseID;
                    stockReceiptItemBO.ReceiptPremiseID = itm.ReceiptPremiseID;
                    stockReceiptItemBO.IssueLocationID = itm.IssueLocationID;
                    stockReceiptItemBO.ReceiptLocationID = itm.ReceiptLocationID;
                    stockReceiptItemBO.Rate = itm.Rate;
                    stockReceiptItemBO.NetAmount = itm.NetAmount;
                    stockReceiptItemBO.GrossAmount = itm.GrossAmount;
                    stockReceiptItemBO.TradeDiscount = itm.TradeDiscount;
                    stockReceiptItemBO.TradeDiscountPercent = itm.TradeDiscountPercent;
                    stockReceiptItemBO.IGSTPercentage = itm.IGSTPercentage;
                    stockReceiptItemBO.CGSTPercentage = itm.CGSTPercentage;
                    stockReceiptItemBO.SGSTPercentage = itm.SGSTPercentage;
                    stockReceiptItemBO.IGSTAmount = itm.IGSTAmount;
                    stockReceiptItemBO.CGSTAmount = itm.CGSTAmount;
                    stockReceiptItemBO.SGSTAmount = itm.SGSTAmount;
                    stockReceiptItemBO.BasicPrice = itm.BasicPrice;
                    stockReceiptItemBO.TaxableAmount = itm.TaxableAmount;
                    stockReceiptItemBO.UnitID = itm.UnitID;
                    ItemList.Add(stockReceiptItemBO);
                }

                var outId = serviceItemReceiptBL.Save(receipt, ItemList);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSerialNo(string ReceiptType)
        {
            try
            {
                string ReceiptNo = generalBL.GetSerialNo("ServiceItemReceipt", ReceiptType);
                return Json(new { Status = "success", ReceiptNo = ReceiptNo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetServiceItemReceiptList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string IssueLocationHint = Datatable.Columns[3].Search.Value;
                string IssuePremiseHint = Datatable.Columns[4].Search.Value;
                string ReceiptLocationHint = Datatable.Columns[5].Search.Value;
                string ReceiptPremiseHint = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = serviceItemReceiptBL.GetServiceItemReceiptList(TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit);
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