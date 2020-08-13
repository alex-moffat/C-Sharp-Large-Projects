using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contact_List_Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Create MVC project that imports an Excel spreadsheet as a data source for SQL Server.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Alex Moffat";

            return View();
        }
    }
}