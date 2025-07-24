using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class TDSController : Controller
    {

        private ITDSContract tdsBL;
        private IGeneralContract generalBL;
        public TDSController()
        {
            tdsBL = new TDSBL();
            generalBL = new GeneralBL();
        }
        // GET: Masters/TDS
        public ActionResult Index()
        {
            List<TDSModel> TDSList = new List<TDSModel>();
            TDSList = tdsBL.GetTDSList().Select(a => new TDSModel
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                ItemAccountCategory=a.ItemAccountCategory,
                ITSection=a.ITSection
            }).ToList();

            return View(TDSList);
         
        }
        // GET: Masters/TDS/Create
        public ActionResult Create()
        {
            TDSModel TDS = new TDSModel();
            TDS.StartDate = General.FormatDate(DateTime.Now);
            TDS.EndDate = General.FormatDate(Convert.ToDateTime(generalBL.GetConfig("DefaultDate"))); ;

            return View(TDS);
        }
        public ActionResult Details(int Id)
        {
            TDSModel TDS = tdsBL.GetTDSDetails(Id).Select(m => new TDSModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                ItemAccountCategory=m.ItemAccountCategory,
                TDSRate=m.TDSRate,
                CompanyType=m.CompanyType,
                ExpenseType=m.ExpenseType,
                ITSection =m.ITSection,
                StartDate = General.FormatDate(m.StartDate, "view"),
                EndDate = General.FormatDate(m.EndDate, "view"),
                Remarks = m.Remarks
            }).First();
         
            return View(TDS);
        }

        // GET: Masters/TDS/Edit
        public ActionResult Edit(int Id)
        {
            TDSModel TDS = tdsBL.GetTDSDetails(Id).Select(m => new TDSModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                ItemAccountCategory = m.ItemAccountCategory,
                TDSRate = m.TDSRate,
                CompanyType = m.CompanyType,
                ExpenseType = m.ExpenseType,
                ITSection = m.ITSection,
                StartDate = General.FormatDate(m.StartDate),
                EndDate = General.FormatDate(m.EndDate),
                Remarks = m.Remarks

            }).First();          
            return View(TDS);
        }
        // Post: Masters/TDS/Save
        public ActionResult Save(TDSModel model)
        {
            try
            {
                TDSBO TDS = new TDSBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    ItemAccountCategory = model.ItemAccountCategory,
                    TDSRate = model.TDSRate,
                    CompanyType = model.CompanyType,
                    ExpenseType = model.ExpenseType,
                    ITSection = model.ITSection,
                    StartDate = General.ToDateTime(model.StartDate),
                    EndDate = General.ToDateTime(model.EndDate),
                    Remarks = model.Remarks

                };
                if (TDS.ID == 0)
                {
                    tdsBL.Save(TDS);
                }
                else
                {
                    tdsBL.Update(TDS);
                }
                return Json(new { Status = "Success", Message = "TDS Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (CodeAlreadyExistsException e)
            {
                return Json(new { Status = "Failure", Message = "TDS Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Save TDS failed" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}