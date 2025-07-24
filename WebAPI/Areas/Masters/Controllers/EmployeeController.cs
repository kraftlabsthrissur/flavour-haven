using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPI.Areas.Masters.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Masters/Employee
        public ActionResult Index()
        {
            return View();
        }
    }
}