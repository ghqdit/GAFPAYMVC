using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAFPAY.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _StaffMenu()
        {
            return PartialView();
        }
        public ActionResult _SettingsMenu()
        {
            return PartialView();
        }

        public ActionResult _AllowancesMenu()
        {
            return PartialView();
        }
        public ActionResult _DeductionsMenu()
        {
            return PartialView();
        }
        public ActionResult _ProcessMenu()
        {
            return PartialView();
        }
          public ActionResult _TrialPayMenu()
        {
            return PartialView();
        }
         
        public ActionResult _ReportsMenu()
        {
            return PartialView();
        }

        public ActionResult _SecurityMenu()
        {
            return PartialView();
        }
         

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}