using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Admin.Models;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Admin.Controllers
{
    public class AppController : Controller
    {
        private IGeneralContract generalBL;
        private IAppContract appBL;

        public AppController()
        {
            generalBL = new GeneralBL();
            appBL = new AppBL();
        }

        // GET: Admin/App
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/App/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/App/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/App/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/App/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // GET: Admin/App/InsertActions
        public ActionResult InsertActions()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            List<ActionBO> Actions = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type)) //filter controllers
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new ActionBO
                    {
                        Area = x.DeclaringType.Namespace.Split('.').Length == 3 ? "" : x.DeclaringType.Namespace.Split('.').Skip(3).First(),
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        ReturnType = x.ReturnType.Name,
                        Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))
                    })
                    .OrderBy(x => x.Area).ThenBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            generalBL.InsertActions(Actions);
            return RedirectToAction("Index");
        }

        public JsonResult GetActionList(DatatableModel Datatable)
        {
            try
            {
                string NameHint = Datatable.Columns[2].Search.Value;
                string AreaHint = Datatable.Columns[3].Search.Value;
                string ControllerHint = Datatable.Columns[4].Search.Value;
                string ActionHint = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = appBL.GetActionList(Type, NameHint, AreaHint, ControllerHint, ActionHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EnableItem(AppModel EnabledItem)
        {
            try
            {
                List<AppBO> EnableItems = new List<AppBO>();
                AppBO Items;
                foreach (var item in EnabledItem.enable)
                {
                    Items = new AppBO()
                    {
                        ID = item.ID,
                        Controller=item.Controller,
                        Action=item.Action
                    };
                    EnableItems.Add(Items);
                }
                appBL.EnableItems(EnableItems);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
