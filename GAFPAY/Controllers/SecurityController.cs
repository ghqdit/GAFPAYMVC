using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAFPAY.ViewData;
using GAFPAY.ViewModel;

namespace GAFPAY.Controllers
{

    public class SecurityController : Controller
    {
        private StaffViewData staffViewData = new StaffViewData();
        public ActionResult DeletedRecruit()
        {
            List<Recruit> getCurrentRecruitList = staffViewData.GetDeletedRecruitsList();
            return View("DeletedRecruit",getCurrentRecruitList);
        }


        public ActionResult DeletedOC()
        {
            List<OfficerCadet> getCurrentRecruitList = staffViewData.GetDeletedOfficerCadetList();
            return View("DeletedOC", getCurrentRecruitList);
        }




    }
}