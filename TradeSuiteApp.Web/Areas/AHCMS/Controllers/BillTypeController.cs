using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.AHCMS.Models;

namespace TradeSuiteApp.Web.Areas.AHCMS.Controllers
{
    public class BillTypeController : Controller
    {
        private IBillTypeContract billTypeBL;

        public BillTypeController()
        {
            billTypeBL = new BillTypeBL();

        }
        // GET: AHCMS/BillType
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            BillTypeModel model = new BillTypeModel();
            var TypeOP = "OP";
            var TypeIP = "IP";
            model.OPItems = billTypeBL.GetTreatmentServices(TypeOP).Select(m => new BillTypeItemModel()
            {
               BillTypeID=m.BillTypeID,
               BillTypeName=m.BillTypeName,
               State=m.State
            }).ToList();
            model.IPItems = billTypeBL.GetTreatmentServices(TypeIP).Select(m => new BillTypeItemModel()
            {
                BillTypeID = m.BillTypeID,
                BillTypeName = m.BillTypeName,
                State=m.State
            }).ToList();
            return View(model);
        }
        public ActionResult Save(BillTypeModel model)
        {
            try
            {
                List<BillTypeBO> OPItems = new List<BillTypeBO>();
                if (model.OPItems != null)
                {
                    BillTypeBO Item;

                    foreach (var item in model.OPItems)
                    {
                        Item = new BillTypeBO()
                        {
                           BillTypeID=item.BillTypeID,
                           Type=item.Type
                        };
                        OPItems.Add(Item);
                    }
                }
                List<BillTypeBO> IPItems = new List<BillTypeBO>();
                if (model.IPItems != null)
                {
                    BillTypeBO Item;

                    foreach (var item in model.IPItems)
                    {
                        Item = new BillTypeBO()
                        {
                            BillTypeID = item.BillTypeID,
                            Type = item.Type
                        };
                        IPItems.Add(Item);
                    }
                }
                billTypeBL.Save(OPItems, IPItems);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Save BillType failed" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetBillTypeItems(string Type)
        {
            List<BillTypeBO> BillTypeID = billTypeBL.GetBillTypeItems(Type);
            return Json(new { BillTypeID = BillTypeID }, JsonRequestBehavior.AllowGet);
        }
    }
}