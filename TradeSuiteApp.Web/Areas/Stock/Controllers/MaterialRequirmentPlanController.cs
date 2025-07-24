using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Stock.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Stock.Controllers
{
    public class MaterialRequirmentPlanController : Controller
    {
        IMaterialRequirementPlanContract materialRequirementPlanBL;

        public MaterialRequirmentPlanController()
        {
            materialRequirementPlanBL = new MaterialRequirementPlanBL();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetMaterialRequirmentPlanList(string FromDate, string ToDate)
        {
            try
            {
                DateTime fromDate = General.ToDateTime(FromDate);
                DateTime toDate = General.ToDateTime(ToDate);

                MaterialRequirementPlanModel Model = new MaterialRequirementPlanModel();
                List<MaterialRequirementPlanItemBO> RequirementItemList = materialRequirementPlanBL.GetMaterialRequirmentPlanList(fromDate, toDate);
                MaterialRequirementPlanItemModel materialRequirementPlanModel;
                Model.Items = new List<MaterialRequirementPlanItemModel>();
                foreach (var m in RequirementItemList)
                {
                    materialRequirementPlanModel = new MaterialRequirementPlanItemModel()
                    {
                        ItemID = m.ItemID,
                        UnitID = m.UnitID,
                        ItemName = m.ItemName,
                        RequiredQty = m.RequiredQty,
                        AvailableStock = m.AvailableStock,
                        QtyInQC = m.QtyInQC,
                        OrderedQty = m.OrderedQty,
                        RequestedQty = m.RequestedQty,
                        RequiredDate = General.FormatDate(m.RequiredDate),
                    };
                    Model.Items.Add(materialRequirementPlanModel);
                }

                return Json(new { Status = "success", Data = Model.Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Save(MaterialRequirementPlanModel model)
        {
            try
            {
                MaterialRequirementPlanBO materialRequirementPlanBO = new MaterialRequirementPlanBO()
                {
                    FromDate = model.FromDate,
                    ToDate = model.ToDate


                };
                List<MaterialRequirementPlanItemBO> Items = new List<MaterialRequirementPlanItemBO>();
                if (model.Items != null)
                {
                    MaterialRequirementPlanItemBO RequirementPlanItems;
                    foreach (var item in model.Items)
                    {
                        RequirementPlanItems = new MaterialRequirementPlanItemBO()
                        {
                            ItemID = item.ItemID,
                            RequiredQty = item.RequiredQty,
                            AvailableStock = item.AvailableStock,
                            QtyInQC = item.QtyInQC,
                            OrderedQty = item.OrderedQty,
                            RequestedQty = item.RequestedQty,
                            UnitID = item.UnitID,
                            RequiredDate = General.ToDateTime(item.RequiredDate),
                        };
                        Items.Add(RequirementPlanItems);
                    }
                }
                int ID = materialRequirementPlanBL.Save(materialRequirementPlanBO, Items);
                return Json(new { Status = "success", Data = ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }



    }
}