using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAFPAY.Controllers
{
    public class ReportController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult JCETrialPay()
        {
            return Redirect("../Reports/rptJCETrial.aspx");
        }
        public ActionResult SCETrialPay()
        {
            return Redirect("../Reports/rptSCETrial.aspx");
        }

        public ActionResult CTOJCE()
        {
            return Redirect("../Reports/rptCTOJCE.aspx");
        }
        public ActionResult CTOSCE()
        {
            return Redirect("../Reports/rptCTOSCE.aspx");
        }

        public ActionResult JCEPayHistory()
        {
            return Redirect("../Reports/rptJCEPayHistory.aspx");
        }

        public ActionResult SCEPayHistory()
        {
            return Redirect("../Reports/rptSCEPayHistory.aspx");
        }

        public ActionResult JCEBankSummary()
        {
            return Redirect("../Reports/rptJCEBankSummary.aspx");
        }

        public ActionResult SCEBankSummary()
        {
            return Redirect("../Reports/rptSCEBankSummary.aspx");
        }

        public ActionResult RecBankSummary()
        {
            return Redirect("../Reports/rptRecBankSummary.aspx");
        }

        public ActionResult CadetBankSummary()
        {
            return Redirect("../Reports/rptCadetBankSummary.aspx");
        }
        public ActionResult JCEPayList()
        {
            return Redirect("../Reports/rptJCEPayList.aspx");
        }
        public ActionResult SCEPayList()
        {
            return Redirect("../Reports/rptSCEPayList.aspx");
        }
        public ActionResult RecPaySlipBatch()
        {
            return Redirect("../Reports/rptRecPaySlipBatch.aspx");
        }
        public ActionResult CadetPaySlipBatch()
        {
            return Redirect("../Reports/rptCadetPaySlipBatch.aspx");
        }
        public ActionResult JCEPaySlipBatch()
        {
            return Redirect("../Reports/rptJCEPaySlipBatch.aspx");
        } public ActionResult SCEPaySlipBatch()
        {
            return Redirect("../Reports/rptSCEPaySlipBatch.aspx");
        }

    }
}