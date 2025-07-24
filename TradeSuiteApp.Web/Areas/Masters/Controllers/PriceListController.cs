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
    public class PriceListController : Controller
    {
        private IPriceListContract priceListBL;

        public PriceListController()
        {
            priceListBL = new PriceListBL();

        }

        public ActionResult Index()
        {
            List<PriceListModel> PriceList = new List<PriceListModel>();
            PriceList = priceListBL.GetPriceList().Select(a => new PriceListModel
            {
                ID = a.ID,
                Name = a.Name,
                FromDate = General.FormatDate(a.FromDate),
                ToDate = General.FormatDate(a.ToDate),

            }).ToList();

            return View(PriceList);
        }

        public ActionResult Details(int ID)
        {
            PriceListModel model = priceListBL.GetPriceListDetails(ID).Select(m => new PriceListModel()
            {
                ID = m.ID,
                Name = m.Name,
                FromDate= General.FormatDate(m.FromDate),
                ToDate= General.FormatDate(m.ToDate),
            }).First();

            model.Items = priceListBL.GetPriceListTransDetails(ID).Select(m => new PriceListItemModel()
            {
                ID = m.ID,
                ItemCode = m.ItemCode.TrimStart().TrimEnd(),
                ItemID = m.ItemID,
                ItemName = m.ItemName,
                UnitID = m.UnitID,
                Unit = m.Unit,
                ISKMRP = m.ISKMRP,
                ISKLoosePrice = m.ISKLoosePrice,
            }).ToList();
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            PriceListModel model = priceListBL.GetPriceListDetails(ID).Select(m => new PriceListModel()
            {
                ID = m.ID,
                Name = m.Name,
                FromDate = General.FormatDate(m.FromDate),
                ToDate = General.FormatDate(m.ToDate),
            }).First();

            model.Items = priceListBL.GetPriceListTransDetails(ID).Select(m => new PriceListItemModel()
            {
                ID = m.ID,
                ItemCode = m.ItemCode.TrimStart().TrimEnd(),
                ItemID = m.ItemID,
                ItemName = m.ItemName,
                UnitID = m.UnitID,
                Unit = m.Unit,
                ISKMRP = m.ISKMRP,
                ISKLoosePrice = m.ISKLoosePrice
            }).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            PriceListModel PriceList = new PriceListModel();
            PriceList.Items = new List<PriceListItemModel>();
            PriceList.FromDate = General.FormatDate(DateTime.Now);
            return View(PriceList);
        }

        public ActionResult Save(PriceListModel model)
        {
            try
            {
                PriceListBO priceListBO = new PriceListBO()
                {
                    ID=model.ID,
                    Name = model.Name,
                    FromDate=General.ToDateTime(model.FromDate),
                    ToDate = General.ToDateTime(model.ToDate)


                };
                List<PriceListItemBO> Items = new List<PriceListItemBO>();
                if (model.Items != null)
                {
                    PriceListItemBO priceListItem;
                    foreach (var item in model.Items)
                    {
                        priceListItem = new PriceListItemBO()
                        {
                            //ID = item.ID,
                            ISKMRP = item.ISKMRP,
                            ISKLoosePrice = item.ISKLoosePrice,
                            ItemCode =item.ItemCode,
                            ItemName=item.ItemName
                        };
                        Items.Add(priceListItem);
                    }
                }
                priceListBL.Save(Items, priceListBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ReadExcel(string Path)
        {
            try
            {
                List<PriceListItemBO> Pricelist = priceListBL.ReadExcel(Path);
                return Json(new { Status = "success", Data = Pricelist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}