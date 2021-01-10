using GAFPAY.Models;
using GAFPAY.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using GAFPAY.Extensions;
using GAFPAY.ViewModel;
using Microsoft.Ajax.Utilities;

namespace GAFPAY.Controllers
{

    public class PayController : Controller
    {
        private DBGAFPAYEntities db = new DBGAFPAYEntities();
        private PayrollViewData payViewData =new PayrollViewData();
        private SettingsViewData settingsViewData =new SettingsViewData();
        private StaffViewData staffViewData =new StaffViewData();
        private int CEWelfareDeductionID = 6;
        private int PresentGeneralStatusID = 1;
        private int TaxDeductionID = 5;
        private bool success = false;
        private string errorMessage = "";


        /*------------------------------------------Start General Trial Pay-----------------------------------   */

        public ActionResult IndexGeneralTrialPay()
        {
            var model=new GeneralTrialPay();
            model.MonthList = payViewData.getTrialPayMonth();
            return View("IndexGeneralTrialPay",model);
        }

        public ActionResult GeneralTrialPay(GeneralTrialPay data)
        {
           
            var model=new GeneralTrialPay();
            var payDate = payViewData.getPayDate(data.MonthID);
            model.TotalRecruit = payViewData.GetMonthTotalRecruitTrial(payDate);
            model.TotalOC = payViewData.GetMonthTotalOCTrial(payDate);
            model.TotalJCE = payViewData.GetMonthTotalJCETrial(payDate);
            model.TotalSCE = payViewData.GetMonthTotalSCETrial(payDate);
            model.TotalSoldiers = 0;
            model.TotalOfficers = 0;
            model.GrandTotal = (model.TotalRecruit + model.TotalOC + model.TotalJCE + model.TotalSCE +
                                model.TotalSoldiers + model.TotalOfficers);
            model.TrialPayDate = payDate;
               
            return View("GeneralTrialPay",model);
        }



        /*------------------------------------------End General Trial Pay-----------------------------------   */



        /*-----------------------------------------Start Recruit Trial Pay--------------------------------------   */
        public ActionResult IndexRecruitTrialPay()
        {
            var year = DateTime.Now.Year;
            var yearPrev = DateTime.Now.Year;
            var yearNext = DateTime.Now.Year;
            var monthPrev = DateTime.Now.AddMonths(-1).Month;
            var monthNext = DateTime.Now.AddMonths(1).Month;
            var month = DateTime.Now.Month;  
            if (month == 1)
            {
                
                yearPrev = DateTime.Now.AddYears(-1).Year; 
                 
            }else if (month==12)
            {
                
                yearNext = DateTime.Now.AddYears(1).Year;
            } 

            List<RecruitPay> recTrialPay = staffViewData.GetRecruitTrialPayList(yearPrev,monthPrev,year,month,yearNext,monthNext);
            return View("IndexRecruitTrialPay",recTrialPay);
        }



        public ActionResult CreateRecruitSingleTrial()
        {
            
            var model=new RecruitTrialPay();
            model.MonthList = payViewData.getPayrollMonth();
            model.StaffList = staffViewData.getRecruit();
            ViewBag.User = "Recruit";

            return View("CreateRecruitSingleTrial",model);
        }


        [HttpPost]
        public ActionResult CreateRecruitSingleTrial(RecruitTrialPay data)
        {
            if (ModelState.IsValid)
            {   
                var recruitID = data.RecruitID;
                var rec = db.RECRUIT.Find(recruitID);
                var constPay = rec.MILITARYLEVSTEP.CONSTPAY; 

                if (rec != null)
                {

                    var payDate = payViewData.getPayDate(data.MonthID);

                    var process =  payViewData.RecruitTrialPay(payDate, recruitID, constPay);

                    if (process.success)
                    {
                        success = true;
                    }
                    else
                    {
                        errorMessage = "Trial Pay for " + rec.SERVICENUMBER + " " + rec.SURNAME + " " + rec.OTHERNAME + " for period " +payDate.ToString("MMMM yyyy") + " has already been processed.";

                    } 
                    #region OldProcess
                    
                    //var checkAvail =  db.RECRUITTRIALPAY.FirstOrDefault(a => a.PAYDATE == tPay.TrialPayDate && a.RECRIUTID == recruitID);
                    //if (checkAvail!=null)
                    //{
                    //    errorMessage = "Trial Pay for " + rec.SERVICENUMBER+" " + rec.SURNAME + " "+rec.OTHERNAME+ " for period " + tPay.TrialPayDate.ToString("MMMM yyyy") + " has already been processed.";

                    //}
                    //else
                    //{
                    //    var recruitTrialPay=new RECRUITTRIALPAY();
                    //    recruitTrialPay.PAYDATE = date1;
                    //    recruitTrialPay.RECRIUTID = recruitID;
                    //    recruitTrialPay.CONSTPAY = constPay.CONSTPAY;
                    //    recruitTrialPay.BANKID = rec.RECRUITBANK.BANKID;
                    //    recruitTrialPay.DATETIMEINSERTED=DateTime.Now;
                    //    //recruitTrialPay.INSERTEDBY = User.Identity.Name;
                    //    recruitTrialPay.INSERTEDBY = "admin";

                    //    db.RECRUITTRIALPAY.Add(recruitTrialPay);

                    //    try
                    //    {
                    //        db.SaveChanges();
                    //        success = true;
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        errorMessage = e.Message;
                    //    }

                    //}

                    #endregion
                }
                else
                {
                    errorMessage = "Recruit does not exist. Please contact System Admin for assistance.";
                    //return Json(success ? JsonResponse.SuccessResponse("Recruit") : JsonResponse.ErrorResponse(errorMessage));
                }

            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return Json(success ? JsonResponse.SuccessResponse("Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult IndexRecruitBatchTrial()
        {
            List<RecruitBatchTrial> getRecruitBatchTrial = payViewData.GetRecruitBatchTrialList();
            return View("IndexRecruitBatchTrial",getRecruitBatchTrial);
        }



        public ActionResult CreateRecruitBatchTrial()
        {
            var model = new RecruitTrialPay();
            model.MonthList = payViewData.getPayrollMonth();
            model.IsTrial = true;
             
            return View("CreateRecruitBatch",model);
        }
        [HttpPost]
        public ActionResult CreateRecruitBatchTrial(RecruitTrialPay data)
        {
            var Processed = 0;
            var Unprocessed = 0;    
            if (ModelState.IsValid)
            {

                var payDate = payViewData.getPayDate(data.MonthID);
                var recruit = db.RECRUIT.Where(a => a.GENERALSTATUSID == PresentGeneralStatusID).ToList();
                foreach (var item in recruit)
                {
                    var process =payViewData.RecruitTrialPay(payDate, item.RECRIUTID, item.MILITARYLEVSTEP.CONSTPAY);

                    if (process.success)
                    {
                        Processed += 1;
                    }
                    else
                    {
                        Unprocessed += 1;
                    }
                     
                }
                try
                {
                    db.SaveChanges();
                    success = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.Message;
                }
                
            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }
            
            return Json(success ? JsonResponse.SuccessResponse("Total processed "+Processed+". Total not processed "+Unprocessed+". Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RecruitTrialDetails(DateTime date)
        {
            List<RecruitBatchTrialDetails> getRecruitBatchTrial = payViewData.GetRecruitBatchTrialDetailsList(date);
            ViewBag.Date = date;
            return View("RecruitTrialDetails", getRecruitBatchTrial);
        }

        public ActionResult RemoveRecruitTrial(int id)
        {
            var recTrial = db.RECRUITTRIALPAY.Find(id);
            db.RECRUITTRIALPAY.Remove(recTrial);


            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse("Recruit Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveMonthRecruitTrial(DateTime date)
        {
            var recTrial = db.RECRUITTRIALPAY.Where(a => a.PAYDATE == date).ToList();

            foreach (var details in recTrial)
            {
                db.RECRUITTRIALPAY.Remove(details);
            }
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }

            return Json(success ? JsonResponse.SuccessResponse("Recruit Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }

        /*-----------------------------------------End Recruit Trial Pay--------------------------------------   */

        /*-----------------------------------------Start Officer Cadet Trial Pay--------------------------------------   */

        public ActionResult IndexOCTrialPay()
        {
            var year = DateTime.Now.Year;
            var yearPrev = DateTime.Now.Year;
            var yearNext = DateTime.Now.Year;
            var monthPrev = DateTime.Now.AddMonths(-1).Month;
            var monthNext = DateTime.Now.AddMonths(1).Month;
            var month = DateTime.Now.Month;
            if (month == 1)
            {

                yearPrev = DateTime.Now.AddYears(-1).Year;

            }
            else if (month == 12)
            {

                yearNext = DateTime.Now.AddYears(1).Year;
            }

            List<OfficerCadetPay> ocTrialPay = staffViewData.GetOfficerCadetTrialPayList(yearPrev, monthPrev, year, month, yearNext, monthNext);
            return View("IndexOCTrialPay", ocTrialPay);
        }



        public ActionResult CreateOCSingleTrial()
        {

            var model = new OcTrialPay();
            model.MonthList = payViewData.getPayrollMonth();
            model.StaffList = staffViewData.getOfficerCadet(); 

            return View("CreateOCSingleTrial", model);
        }


        [HttpPost]
        public ActionResult CreateOCSingleTrial(OcTrialPay data)
        {
            if (ModelState.IsValid)
            {
                var ocID = data.OfficerCadetID;
                var oc = db.OFFICERCADET.Find(ocID);
                var constPay = oc.MILITARYLEVSTEP.CONSTPAY; 
                if (oc != null)
                {

                    var payDate = payViewData.getPayDate(data.MonthID);

                    var ocTrial = payViewData.OfficerCadetTrialPay(payDate, ocID, constPay);

                    if (ocTrial.success)
                    {
                        success = true;
                    }
                    else
                    {
                        errorMessage = "Trial Pay for " + oc.SERVICENUMBER + " " + oc.SURNAME + " " + oc.OTHERNAME + " for period " + payDate.ToString("MMMM yyyy") + " has already been processed.";

                    } 

                }
                else
                {
                    errorMessage = "Officer Cadet does not exist. Please contact System Admin for assistance.";
                    //return Json(success ? JsonResponse.SuccessResponse("Recruit") : JsonResponse.ErrorResponse(errorMessage));
                }

            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return Json(success ? JsonResponse.SuccessResponse("Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult IndexOCBatchTrial()  
        {
            List<OCBatchTrial> getOCBatchTrial = payViewData.GetOCBatchTrialList();
            return View("IndexOCBatchTrial", getOCBatchTrial);
        }
        public ActionResult CreateOCBatchTrial()
        {
            var model = new OcTrialPay();
            model.MonthList = payViewData.getPayrollMonth();
            model.IsTrial = true; 

            return View("CreateOCBatch", model);
        }
        [HttpPost]
        public ActionResult CreateOCBatchTrial(OcTrialPay data)
        {
            var Processed = 0;
            var Unprocessed = 0;
            if (ModelState.IsValid)
            {

                var payDate = payViewData.getPayDate(data.MonthID);
                var oc = db.OFFICERCADET.Where(a => a.GENERALSTATUSID == PresentGeneralStatusID).ToList();
                foreach (var item in oc)
                {
                    var process =payViewData.OfficerCadetTrialPay(payDate, item.OFFICERCADETID,item.MILITARYLEVSTEP.CONSTPAY);

                    if (process.success)
                    {
                        Processed += 1;
                    }
                    else
                    {
                        Unprocessed += 1;
                    }

                }
                try
                {
                    db.SaveChanges();
                    success = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.Message;
                }

            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }
             
            return Json(success ? JsonResponse.SuccessResponse("Total processed " + Processed + ". Total not processed " + Unprocessed + ". Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }


        public ActionResult OCTrialDetails(DateTime date)
        {
            List<OCBatchTrialDetails> getOCBatchTrial = payViewData.GetOCBatchTrialDetailsList(date);
            ViewBag.Date = date;
            return View("OCTrialDetails", getOCBatchTrial);
        }

        public ActionResult RemoveOCTrial(int id)
        {
            var ocTrial = db.OFFICERCADETTRIALPAY.Find(id);
            db.OFFICERCADETTRIALPAY.Remove(ocTrial); 

            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse("Officer Cadet Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveMonthOCTrial(DateTime date)
        {
            var ocTrial = db.OFFICERCADETTRIALPAY.Where(a => a.PAYDATE == date).ToList();
            foreach (var details in ocTrial)
            {
               db.OFFICERCADETTRIALPAY.Remove(details);
 
            }
             
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse("Officer Cadet Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }


        /*-----------------------------------------End Officer Cadet Trial Pay--------------------------------------   */



        /*-----------------------------------------Start Junior CE Trial Pay--------------------------------------   */
        public ActionResult CreateJCESingleTrial()
        {

            var model = new JCETrialPay();
            model.MonthList = payViewData.getPayrollMonth();
            model.StaffList = staffViewData.getJuniorCE();
            ViewBag.User = "Junior CE";

            return View("CreateJCESinlgeTrial", model);
        }

        [HttpPost]
        public ActionResult CreateJCESingleTrial(JCETrialPay data)
        {
            if (ModelState.IsValid)
            {
                var JCEID = data.JCEID;
                var jce = db.JUNIORCE.Find(JCEID);
                var constPay = jce.CIVILIANLEVSTEP.CONSTPAY;
                var bankID = jce.JUNIORCEBANK.BANKID; 
                if (jce != null)
                {

                    var payDate = payViewData.getPayDate(data.MonthID);
                    var process = payViewData.JuniorCETrialPay(payDate, JCEID, constPay, jce.ISMEDICAL);

                    if (process.success)
                    {
                        success = true;
                    } 
                    else
                    {
                        errorMessage = "Trial Pay for " + jce.SERVICENUMBER +" "+ jce.SURNAME + " " + jce.OTHERNAME + " for period " + payDate.ToString("MMMM yyyy") + " has already been processed.";

                    }
                      
                }
                else
                {
                    errorMessage = "Junior CE does not exist. Please contact System Admin for assistance.";
                    //return Json(success ? JsonResponse.SuccessResponse("Recruit") : JsonResponse.ErrorResponse(errorMessage));
                }

            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return Json(success ? JsonResponse.SuccessResponse("Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult IndexJCEBatchTrial()
        { 
            List<JCEBatchTrial> getJCEBatchTrial = payViewData.GetJCEBatchTrialList();
            return View("IndexJCEBatchTrial", getJCEBatchTrial);
        }

        public ActionResult CreateJCEBatchTrial()
        {
            var model = new JCETrialPay();
            model.MonthList = payViewData.getPayrollMonth();
            model.IsTrial = true;

            return View("CreateJCEBatch", model);
        }
        [HttpPost]
        public ActionResult CreateJCEBatchTrial(JCETrialPay data)
        {
            var Processed = 0;
            var Unprocessed = 0;
            if (ModelState.IsValid)
            {

                var payDate = payViewData.getPayDate(data.MonthID);
                var jce = db.JUNIORCE.Where(a => a.GENERALSTATUSID == PresentGeneralStatusID).ToList();
                foreach (var item in jce)
                {
                    var process = payViewData.JuniorCETrialPay(payDate, item.JUNIORCEID, item.CIVILIANLEVSTEP.CONSTPAY,item.ISMEDICAL);

                    if (process.success)
                    {
                        Processed += 1;
                    }
                    else
                    {
                        Unprocessed += 1;
                    }

                }
                try
                {
                    db.SaveChanges();
                    success = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.Message;
                }

            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }
              
            return Json(success ? JsonResponse.SuccessResponse("Total processed " + Processed + ". Total not processed " + Unprocessed + ". Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }


        public ActionResult JCETrialDetails(DateTime date)
        {
            List<JCEBatchTrialDetails > getJCEBatchTrial = payViewData.GetJCEBatchTrialDetailsList(date);
            ViewBag.Date = date;
            return View("JCETrialDetails", getJCEBatchTrial);
        }
        public ActionResult JCETrialPayDetails(int id)
        {
            var model = new JuniorCEDetails();
            var jceTrial = db.JUNIORCETRIALPAY.Find(id);
            var jce = db.JUNIORCE.Find(jceTrial.JUNIORCEID);
            var jceBank = db.JUNIORCEBANK.Find(jceTrial.JUNIORCEID);
            model.Surname = jce.SURNAME;
            model.Othername = jce.OTHERNAME;
            model.ServiceNumber = jce.SERVICENUMBER;
            model.TitleName = jce.TITLE.TITLENAME;
            model.UnitName = jce.UNIT.UNITNAME;
            model.GenderName = jce.GENDER.GENDERNAME;
            //model.GeneralStatusName = jce.GENERALSTATUS.GSNAME;
            model.ConstPay = jceTrial.CONSTPAY;
            model.CLevStepName = jce.CIVILIANLEVSTEP.LEVSTEPNAME;
            model.SSNITNo = jce.SSNITNUMBER;
            model.BankBranch = jceBank.BANK.BANKBRANCH;
            model.BankName = jceBank.BANK.BANKNAME.BANKNAMEX;
            model.NetPay = jceTrial.NETPAY;
            model.AccountNumber = jceBank.ACCOUNTNUMBER;
            model.GradeName = jce.GRADE.GRADENAME;
            model.DateEmployed = jce.DATEEMPLOYED;
            model.GenderID = jce.GENDERID;

            ViewBag.PayDate = jceTrial.PAYDATE;
            ViewBag.TrialPayID = jceTrial.JUNIORCETRIALPAYID;

            var JCEA = db.JUNIORCETRIALPAYALLOWANCE.Where(a=>a.JUNIORCETRIALPAYID==id);
            model.JuniorCEAllowanceDetails = new List<JuniorCEAllowance>();
            foreach (var details in JCEA)
            {
                var Allow = new JuniorCEAllowance();
                Allow.AllowanceID = details.ALLOWANCEID;
                Allow.AllowanceName = details.ALLOWANCE.ALLOWANCENAME;
                Allow.Amount = details.AMOUNT;
                model.TotalAllow += Allow.Amount;
                model.JuniorCEAllowanceDetails.Add(Allow);
                 
            }
             
            var JCED = db.JUNIORCETRIALPAYDEDUCTION.Where(a => a.JUNIORCETRIALPAYID== id);
            model.JuniorCEDeduction2Details = new List<JuniorCEDeduction2>();
            foreach (var details in JCED)
            {
                var Deduc = new JuniorCEDeduction2();
                Deduc.DeductionID = details.DEDUCTIONID;
                Deduc.DeductionName = details.DEDUCTION.DEDUCTIONNAME;
                Deduc.Amount = details.DEDUCTIONAMOUNT;
                if (details.BALANCE != null)
                {
                    Deduc.Balance = details.BALANCE.Value;
                    model.TotalBalance = Deduc.Balance;
                }
                if (details.TOTALAMOUNT != null)
                {
                    Deduc.TotalAmount = details.TOTALAMOUNT.Value;
                    model.TotalAmountX = Deduc.TotalAmount;
                }

                model.TotalDeduc += Deduc.Amount;
                model.JuniorCEDeduction2Details.Add(Deduc);
                  
            }
             
            return View("JCETrialPayDetails", model);
            
        }


        public ActionResult RemoveJCETrial(int id)
        {
            var jceTrial = db.JUNIORCETRIALPAY.Find(id);
            var jceAllow = db.JUNIORCETRIALPAYALLOWANCE.Where(a => a.JUNIORCETRIALPAYID == id).ToList();
            foreach (var  details in jceAllow)
            {
                db.JUNIORCETRIALPAYALLOWANCE.Remove(details);
            }
            var jceDeduc = db.JUNIORCETRIALPAYDEDUCTION.Where(a => a.JUNIORCETRIALPAYID == id).ToList();
            foreach (var  details in jceDeduc)
            {
                db.JUNIORCETRIALPAYDEDUCTION.Remove(details);
            }
            db.JUNIORCETRIALPAY.Remove(jceTrial);
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse("Junior CE Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveMonthJCETrial(DateTime date)
        {
            var jceTrial = db.JUNIORCETRIALPAY.Where(a => a.PAYDATE == date && a.STATUS==1).ToList();
            foreach (var items in jceTrial)
            {
                var payDetails =
                    db.JUNIORCETRIALPAYALLOWANCE.Where(a => a.JUNIORCETRIALPAYID == items.JUNIORCETRIALPAYID).ToList();

                foreach (var jcepay in payDetails)
                {
                    db.JUNIORCETRIALPAYALLOWANCE.Remove(jcepay);
                }

                var deducDetails =
                    db.JUNIORCETRIALPAYDEDUCTION.Where(a => a.JUNIORCETRIALPAYID == items.JUNIORCETRIALPAYID).ToList();

                foreach (var jcededuc in deducDetails)
                {
                    db.JUNIORCETRIALPAYDEDUCTION.Remove(jcededuc);
                }

                db.JUNIORCETRIALPAY.Remove(items);

            }

            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse("Junior CE Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }

        /*-----------------------------------------End Junior CE Trial Pay--------------------------------------   */


        /*-----------------------------------------Start Senior CE Trial Pay--------------------------------------   */

        public ActionResult CreateSCESingleTrial()
        {

            var model = new SCETrialPay();
            model.MonthList = payViewData.getPayrollMonth();
            model.StaffList = staffViewData.getSeniorCE();
            ViewBag.User = "Senior CE";

            return View("CreateSCESingleTrial", model);
        }

        [HttpPost]
        public ActionResult CreateSCESingleTrial(SCETrialPay data)
        {
            if (ModelState.IsValid)
            {
                var SCEID = data.SCEID;
                var sce = db.SENIORCE.Find(SCEID);
                var constPay = sce.CIVILIANLEVSTEP.CONSTPAY; 
                if (sce != null)
                {

                    var payDate = payViewData.getPayDate(data.MonthID);
                    var process = payViewData.SeniorCETrialPay(payDate, SCEID, constPay, sce.ISMEDICAL);

                    if (process.success)
                    {
                        success = true;
                    }
                    else
                    {
                        errorMessage = "Trial Pay for " + sce.SERVICENUMBER + " " + sce.SURNAME + " " + sce.OTHERNAME + " for period " + payDate.ToString("MMMM yyyy") + " has already been processed.";

                    }

                }
                else
                {
                    errorMessage = "Junior CE does not exist. Please contact System Admin for assistance.";
                    //return Json(success ? JsonResponse.SuccessResponse("Recruit") : JsonResponse.ErrorResponse(errorMessage));
                }

            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return Json(success ? JsonResponse.SuccessResponse("Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult IndexSCEBatchTrial()
        {
            List<SCEBatchTrial> getSCEBatchTrial = payViewData.GetSCEBatchTrialList();
            return View("IndexSCEBatchTrial", getSCEBatchTrial);
        }


        public ActionResult CreateSCEBatchTrial()
        {
            var model = new SCETrialPay(); 
            model.MonthList = payViewData.getPayrollMonth();
            model.IsTrial = true;
            return View("CreateSCEBatch", model);
        }
        [HttpPost]
        public ActionResult CreateSCEBatchTrial(SCETrialPay data)
        {
            var Processed = 0;
            var Unprocessed = 0;
            if (ModelState.IsValid)
            {

                var payDate = payViewData.getPayDate(data.MonthID);
                var sce = db.SENIORCE.Where(a => a.GENERALSTATUSID == PresentGeneralStatusID).ToList();
                foreach (var item in sce)
                {
                    var process = payViewData.SeniorCETrialPay(payDate, item.SENIORCEID, item.CIVILIANLEVSTEP.CONSTPAY, item.ISMEDICAL);

                    if (process.success)
                    {
                        Processed += 1;
                    }
                    else
                    {
                        Unprocessed += 1;
                    }

                }
                try
                {
                    db.SaveChanges();
                    success = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.Message;
                }

            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }


            return Json(success ? JsonResponse.SuccessResponse("Total processed " + Processed + ". Total not processed " + Unprocessed + ". Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }


        public ActionResult SCETrialDetails(DateTime date)
        {
            List<SCEBatchTrialDetails> getSCEBatchTrial = payViewData.GetSCEBatchTrialDetailsList(date);
            ViewBag.Date = date;
            return View("SCETrialDetails", getSCEBatchTrial);
        }
        public ActionResult SCETrialPayDetails(int id)
        {
            var model = new SeniorCEDetails();
            var sceTrial = db.SENIORCETRIALPAY.Find(id);
            var sce = db.SENIORCE.Find(sceTrial.SENIORCEID);
            var sceBank = db.SENIORCEBANK.Find(sceTrial.SENIORCEID);
            model.Surname = sce.SURNAME;
            model.Othername = sce.OTHERNAME;
            model.ServiceNumber = sce.SERVICENUMBER;
            model.TitleName = sce.TITLE.TITLENAME;
            model.UnitName = sce.UNIT.UNITNAME;
            model.GenderName = sce.GENDER.GENDERNAME;
           // model.GeneralStatusName = sce.GENERALSTATUS.GSNAME;
            model.ConstPay = sceTrial.CONSTPAY;
            model.CLevStepName = sce.CIVILIANLEVSTEP.LEVSTEPNAME;
            model.SSNITNo = sce.SSNITNUMBER;
            model.BankBranch = sceBank.BANK.BANKBRANCH;
            model.BankName = sceBank.BANK.BANKNAME.BANKNAMEX;
            model.NetPay = sceTrial.NETPAY;
            model.AccountNumber = sceBank.ACCOUNTNUMBER;
            model.GradeName = sce.GRADE.GRADENAME;
            model.DateEmployed = sce.DATEEMPLOYED;
            model.GenderID = sce.GENDERID;

            ViewBag.PayDate = sceTrial.PAYDATE;

            var SCEA = db.SENIORCETRIALPAYALLOWANCE.Where(a => a.SENIORCETRIALPAYID == id);
            model.SeniorCEAllowanceDetails = new List<SeniorCEAllowance>();
            foreach (var details in SCEA)
            {
                var Allow = new SeniorCEAllowance();
                Allow.AllowanceID = details.ALLOWANCEID;
                Allow.AllowanceName = details.ALLOWANCE.ALLOWANCENAME;
                Allow.Amount = details.AMOUNT;
                model.TotalAllow += Allow.Amount;
                model.SeniorCEAllowanceDetails.Add(Allow);

            }

            var SCED = db.SENIORCETRIALPAYDEDUCTION.Where(a => a.SENIORCETRIALPAYID == id);
            model.SeniorCEDeduction2Details = new List<SeniorCEDeduction2>();
            foreach (var details in SCED)
            {
                var Deduc = new SeniorCEDeduction2();
                Deduc.DeductionID = details.DEDUCTIONID;
                Deduc.DeductionName = details.DEDUCTION.DEDUCTIONNAME;
                Deduc.Amount = details.DEDUCTIONAMOUNT;
                if (details.BALANCE != null)
                {
                    Deduc.Balance = details.BALANCE.Value;
                    model.TotalBalance = Deduc.Balance;
                }
                if (details.TOTALAMOUNT != null)
                {
                    Deduc.TotalAmount = details.TOTALAMOUNT.Value;
                    model.TotalAmountX = Deduc.TotalAmount;
                }

                model.TotalDeduc += Deduc.Amount;
                model.SeniorCEDeduction2Details.Add(Deduc);

            }

            return View("SCETrialPayDetails", model);

        }


        public ActionResult RemoveSCETrial(int id)
        {
            var sceTrial = db.SENIORCETRIALPAY.Find(id);
            var sceAllow = db.SENIORCETRIALPAYALLOWANCE.Where(a => a.SENIORCETRIALPAYID == id).ToList();
            foreach (var details in sceAllow)
            {
                db.SENIORCETRIALPAYALLOWANCE.Remove(details);
            }
            var sceDeduc = db.SENIORCETRIALPAYDEDUCTION.Where(a => a.SENIORCETRIALPAYID == id).ToList();
            foreach (var details in sceDeduc)
            {
                db.SENIORCETRIALPAYDEDUCTION.Remove(details);
            }
            db.SENIORCETRIALPAY.Remove(sceTrial);
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse("Senior CE Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }


        public ActionResult RemoveMonthSCETrial(DateTime date)
        {
            var sceTrial = db.SENIORCETRIALPAY.Where(a => a.PAYDATE == date && a.STATUS == 1).ToList();
            foreach (var items in sceTrial)
            {
                var payDetails =
                    db.SENIORCETRIALPAYALLOWANCE.Where(a => a.SENIORCETRIALPAYID == items.SENIORCETRIALPAYID).ToList();

                foreach (var scepay in payDetails)
                {
                    db.SENIORCETRIALPAYALLOWANCE.Remove(scepay);
                }

                var deducDetails =
                    db.SENIORCETRIALPAYDEDUCTION.Where(a => a.SENIORCETRIALPAYID == items.SENIORCETRIALPAYID).ToList();

                foreach (var scededuc in deducDetails)
                {
                    db.SENIORCETRIALPAYDEDUCTION.Remove(scededuc);
                }

                db.SENIORCETRIALPAY.Remove(items);

            }

            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse("Senior CE Trial Pay") : JsonResponse.ErrorResponse(errorMessage));
        }


        /*-----------------------------------------End Senior CE Trial Pay--------------------------------------   */



        /*-----------------------------------------Start Soldier Trial Pay--------------------------------------   */



        /*-----------------------------------------End Soldier Trial Pay--------------------------------------   */


        /*-----------------------------------------Start Officer Trial Pay--------------------------------------   */

        /*-----------------------------------------End Officer Trial Pay--------------------------------------   */



        /*-----------------------------------------Start Overall Trial Pay--------------------------------------   */

        public ActionResult CreateBatchTrial()
        {
            var model = new TrialPay();
            model.MonthList = payViewData.getPayrollMonth();

            return View("CreateBatchTrial", model);
        }


        [HttpPost]
        public ActionResult CreateBatchTrial(TrialPay data)
        {
            int RecProcessed=0 , OCProcessed=0 , JCEProcessed=0 , SCEProcessed=0;
            int RecUnprocessed = 0,OCUnprocessed=0,JCEUnprocessed=0,SCEUnprocessed=0;
            if (ModelState.IsValid)
            {

                var payDate = payViewData.getPayDate(data.MonthID);
                var recruit = db.RECRUIT.Where(a => a.GENERALSTATUSID == PresentGeneralStatusID).ToList();
                foreach (var item in recruit)
                {
                    var process = payViewData.RecruitTrialPay(payDate, item.RECRIUTID,item.MILITARYLEVSTEP.CONSTPAY);

                    if (process.success)
                    {
                        RecProcessed += 1;
                    }
                    else
                    {
                        RecUnprocessed += 1;
                    }

                }

                var oc = db.OFFICERCADET.Where(a => a.GENERALSTATUSID == PresentGeneralStatusID).ToList();
                foreach (var item in oc)
                {
                    var process = payViewData.OfficerCadetTrialPay(payDate, item.OFFICERCADETID,item.MILITARYLEVSTEP.CONSTPAY);
                    if (process.success)
                    {
                        OCProcessed += 1;
                    }
                    else
                    {
                        OCUnprocessed += 1;
                    }
                }

                var jce = db.JUNIORCE.Where(a => a.GENERALSTATUSID == PresentGeneralStatusID).ToList();
                foreach (var item in jce)
                {
                    var process = payViewData.JuniorCETrialPay(payDate, item.JUNIORCEID,item.CIVILIANLEVSTEP.CONSTPAY, item.ISMEDICAL);
                    if (process.success)
                    {
                        JCEProcessed += 1; 
                    }
                    else
                    {
                        JCEUnprocessed += 1;
                    }
                }

                var sce = db.SENIORCE.Where(a => a.GENERALSTATUSID == PresentGeneralStatusID).ToList();
                foreach (var item in sce)
                {
                    var process = payViewData.SeniorCETrialPay(payDate, item.SENIORCEID, item.CIVILIANLEVSTEP.CONSTPAY, item.ISMEDICAL);
                    if (process.success)
                    {
                        SCEProcessed += 1; 
                    }
                    else
                    {
                        SCEUnprocessed += 1;
                    }
                } 

                try
                {
                    db.SaveChanges();
                    success = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.Message;
                }

            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }


            return Json(success ? JsonResponse.SuccessResponse("Recruits processed " + RecProcessed + ". Recruits not processed " + RecUnprocessed +". Cadets processed " + OCProcessed + ". Cadets not processed " + OCUnprocessed +". Junior CE processed " + JCEProcessed + ". Junior CE not processed " + JCEUnprocessed +". Senior CE processed " + SCEProcessed + ". Senior CE not processed " + SCEUnprocessed + ". Trial Pay") : JsonResponse.ErrorResponse(errorMessage));

        }

        /*-----------------------------------------End Overalll Trial Pay--------------------------------------   */


        /*-----------------------------------------Start General Payroll--------------------------------------   */

        public ActionResult CreateBatchPayroll()
        {
            var model=new TrialPay();
            model.MonthList = payViewData.getPayrollMonth();

            return View("CreateBatchPayroll", model);

        }

        [HttpPost]
        public ActionResult CreateBatchPayroll(TrialPay data)
        {
            if (ModelState.IsValid)
            { 
                var payDate = payViewData.getPayDate(data.MonthID);

                var recruit = payViewData.RecruitPayProcess(payDate);
                errorMessage += recruit.Message;

                var cadet = payViewData.OfficerCadetPayProcess(payDate);
                errorMessage += cadet.Message;

                var jce = payViewData.JuniorCEPayProcess(payDate);
                errorMessage += jce.Message;

                var sce = payViewData.SeniorCEPayProcess(payDate);
                errorMessage += sce.Message;

                success = true;
            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }


            return Json(success ? JsonResponse.SuccessResponse(errorMessage + " Pay") : JsonResponse.ErrorResponse(errorMessage));

        }

        /*-----------------------------------------End General Payroll--------------------------------------   */

        /*-----------------------------------------Start Recruit Payroll--------------------------------------   */

        public ActionResult CreateRecruitPayroll()
        {
            var model=new RecruitTrialPay();
            model.MonthList = payViewData.getPayrollMonth();
            return View("CreateRecruitBatch",model);
        }

        [HttpPost]
        public ActionResult CreateRecruitPayroll(RecruitTrialPay data)
        {
            if (ModelState.IsValid)
            {
                var payDate = payViewData.getPayDate(data.MonthID);

                var recruit = payViewData.RecruitPayProcess(payDate);
                errorMessage += recruit.Message;
                success = true;

            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return Json(success ? JsonResponse.SuccessResponse(errorMessage + " Pay") : JsonResponse.ErrorResponse(errorMessage));

        }
        /*-----------------------------------------End Recruit Payroll--------------------------------------   */

        /*-----------------------------------------Start Officer Cadet Payroll--------------------------------------   */

        public ActionResult CreateOCPayroll()
        {
            var model=new OcTrialPay();
            model.MonthList = payViewData.getPayrollMonth();
            return View("CreateOCBatch",model);
        }
        [HttpPost]
        public ActionResult CreateOCPayroll(OcTrialPay data)
        {
            if (ModelState.IsValid)
            {
                var payDate = payViewData.getPayDate(data.MonthID);

                var oc = payViewData.OfficerCadetPayProcess(payDate);
                errorMessage += oc.Message;
                success = true;
            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return Json(success ? JsonResponse.SuccessResponse(errorMessage + " Pay") : JsonResponse.ErrorResponse(errorMessage));

        }
        /*-----------------------------------------End Officer Cadet Payroll--------------------------------------   */


        /*-----------------------------------------Start Junior CE Payroll--------------------------------------   */

        public ActionResult CreateJCEPayroll()
        {
            var model=new JCETrialPay();
            model.MonthList = payViewData.getPayrollMonth();

            return View("CreateJCEBatch",model);
        }
        [HttpPost]
        public ActionResult CreateJCEPayroll(JCETrialPay data)
        {
            if (ModelState.IsValid)
            {
                var payDate = payViewData.getPayDate(data.MonthID);

                var jce = payViewData.JuniorCEPayProcess(payDate);
                errorMessage += jce.Message;
                success = true;
            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return Json(success ? JsonResponse.SuccessResponse(errorMessage + " Pay") : JsonResponse.ErrorResponse(errorMessage));

        }
        /*-----------------------------------------End Junior CE Payroll--------------------------------------   */

        /*-----------------------------------------Start Senior CE Payroll--------------------------------------   */

        public ActionResult CreateSCEPayroll()
        {
            var model=new SCETrialPay();
            model.MonthList = payViewData.getPayrollMonth();
            return View("CreateSCEBatch",model);
        }
        [HttpPost]
        public ActionResult CreateSCEPayroll(RecruitTrialPay data)
        {
            if (ModelState.IsValid)
            { 
                var payDate = payViewData.getPayDate(data.MonthID);

                var sce = payViewData.SeniorCEPayProcess(payDate);
                errorMessage += sce.Message;
                success = true;
            }
            else
            {
                errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return Json(success ? JsonResponse.SuccessResponse(errorMessage + " Pay") : JsonResponse.ErrorResponse(errorMessage));

        }
        /*-----------------------------------------End Senior CE Payroll--------------------------------------   */


        /*-----------------------------------------Start Junior CE Trial Report--------------------------------------   */
        public ActionResult IndexJCETrialReport()
        {
            List<JCEBatchTrial> getJCEBatchTrial = payViewData.GetJCEBatchTrialList();
            return View("IndexJCETrialReport", getJCEBatchTrial);
        }

        //public ActionResult JCETrialReportDetails(DateTime date)    
        //{
        //    //List<JCETrialReport> getJCETrialReport = payViewData.GetJCETrialReportDetailsList(date);
        //    ViewBag.Date = date; 
        //    var jcetrial=new JCETrialReport();
        //    jcetrial.JuniorCETrialReport = payViewData.GetJCETrialReportDetailsList(date);
        //    var TL = db.JUNIORCETRIALPAY.Where(a => a.PAYDATE == date && a.STATUS == 1).ToList();
        //    foreach (var item in TL)
        //    {
                
                
        //       //jcetrial.JuniorCETrialReport=new List<JCETrialReport>();
             
        //        var JAllow = payViewData.GetJCETrialAllowancesReportDetails(item.TrialPayID).ToList();
        //        jcetrial.JuniorCETrialPayAllowances=new List<JuniorCEAllowance>();
        //        foreach (var JA in JAllow)
        //        {
                    
        //            jcetrial.JuniorCETrialPayAllowances.Add(JA); 
        //        }

        //        var JDeduc = payViewData.GetJCETrialDeductionsReportDetails(item.TrialPayID).ToList();
        //        jcetrial.JuniorCETrialPayDeductions=new List<JuniorCEDeduction2>(); 
        //        foreach (var JD in JDeduc)
        //        {
        //            jcetrial.JuniorCETrialPayDeductions.Add(JD); 
        //        } 
        //        jcetrial.JuniorCEID = item.JuniorCEID;
        //        jcetrial.ConstPay = item.ConstPay;
        //        jcetrial.NetPay = item.NetPay;

        //       //jcetrial.JuniorCETrialReport.Add(jcetrial);
               
        //    }
            

        //    return View("JCETrialReportDetails");
        //}


        /*-----------------------------------------End Junior CE Trial Report--------------------------------------   */





        /*-----------------------------------------Start Junior CE Allowance--------------------------------------   */

        public ActionResult IndexJCEAllowance()
        {
            List<JuniorCE> getCurrentJCEList = staffViewData.GetCurrentJuniorCEList();
            return View("IndexJCEAllowance", getCurrentJCEList); 
        }

        public ActionResult JCEAllowance(int id)
        {
            var allow = db.JUNIORCEALLOWANCE.Where(a => a.JUNIORCEID == id && a.STATUS == 1).ToList();
            var JCEAllow=new JuniorCEAllowance();
            JCEAllow.JuniorCEAllowanceDetails=new List<JuniorCEAllowance>();
            foreach (var details in allow)
            {
                
                var JCEA=new JuniorCEAllowance();
                //JCEA.JuniorCEAllowanceID = details.JUNIORCEALLOWANCESID;
                JCEA.JuniorCEID = details.JUNIORCEID;
                JCEA.AllowanceName = details.ALLOWANCE.ALLOWANCENAME;
                JCEA.Amount = details.AMOUNT; 

                JCEAllow.JuniorCEAllowanceDetails.Add(JCEA); 

            }
            var jce = db.JUNIORCE.Find(id);
            ViewBag.Name = jce.TITLE.TITLENAME + " " + jce.SURNAME + " " + jce.OTHERNAME;
            JCEAllow.JuniorCEID = id;
            return View("JCEAllowance",JCEAllow);
        }
        [HttpPost]
        public ActionResult JCEAllowance(int id,JuniorCEAllowance data)
        {   
            if (data.JuniorCEAllowanceDetails!=null)
            {
               if (ModelState.IsValid)
                {
                    foreach (var details in data.JuniorCEAllowanceDetails)
                    {
                        var item =
                            db.ALLOWANCE.Where(a => a.ALLOWANCENAME == details.AllowanceName)
                                .Select(a => new Allowance() { AllowanceID = a.ALLOWANCEID })
                                .FirstOrDefault();
                        if (item == null)
                        {
                            errorMessage = "Allowance " + details.AllowanceName +
                                           " does not exist. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
                        }

                        if (details.Amount > 10000)
                        {
                            errorMessage = "Allowance " + details.AllowanceName +
                                         " amount entered must not be greater than 10,000. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
                        }

                        var JCEA = db.JUNIORCEALLOWANCE.FirstOrDefault(a => a.JUNIORCEID == id && a.STATUS == 1 && a.ALLOWANCEID == item.AllowanceID);
 
                        JCEA.AMOUNT = details.Amount;   
                        JCEA.DATETIMEUPDATED = DateTime.Now;
                        JCEA.UPDATEDBY = "admin";
                        //JCEA.INSERTEDBY = User.Identity.Name;


                        var jce = db.JUNIORCE.Find(id);
                        jce.DATETIMEUPDATED = DateTime.Now;
                        jce.UPDATEDBY = User.Identity.Name;
                        jce.UPDATEDBY = "admin";
                        jce.ISEDIT = true;

                    }
                    try
                    {
                        db.SaveChanges();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }

                }
                else
                {
                    errorMessage = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                }
            }
            else
            {
                errorMessage = "You have not selected any Allowance to be saved. Please ensure that Junior CE has at least one (1) Allowance before saving changes.";
            }
              
            return Json(success ? JsonResponse.SuccessResponse("Junior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult JCEAllowanceAdd(int id)
        {
            var JCEAllow = new JuniorCEAllowance();
            var jce = db.JUNIORCE.Find(id);
            ViewBag.Name = jce.TITLE.TITLENAME + " " + jce.SURNAME + " " + jce.OTHERNAME;
            JCEAllow.JuniorCEID = id;
            return View("JCEAllowanceAdd", JCEAllow);
        }
        [HttpPost]
        public ActionResult JCEAllowanceAdd(int id,JuniorCEAllowance data)
        {
            if (data.JuniorCEAllowanceDetails != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.JuniorCEAllowanceDetails)
                    {
                        var item =
                            db.ALLOWANCE.Where(a => a.ALLOWANCENAME == details.AllowanceName && a.STATUS==1)
                                .Select(a => new Allowance() { AllowanceID = a.ALLOWANCEID })
                                .FirstOrDefault();
                        if (item == null)
                        {
                            errorMessage = "Allowance " + details.AllowanceName +
                                           " does not exist in this context. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var JCEA = db.JUNIORCEALLOWANCE.FirstOrDefault(a => a.JUNIORCEID == id && a.STATUS == 1 && a.ALLOWANCEID == item.AllowanceID);

                        if (JCEA != null)
                        {
                            errorMessage = "Allowance " + details.AllowanceName +
                                          " already exist for this Junior CE.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
                        }

                        if (details.Amount>10000)
                        {
                            errorMessage = "Allowance " + details.AllowanceName +
                                         " amount entered must not be greater than 10,000. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var JCEAllow = new JUNIORCEALLOWANCE();
                        var guid= Guid.NewGuid();
                        JCEAllow.JUNIORCEID = id;
                        JCEAllow.STATUS = 1;
                        JCEAllow.AMOUNT = details.Amount;
                        JCEAllow.ALLOWANCEID = item.AllowanceID;  
                        JCEAllow.ID = guid;
                        JCEAllow.DATETIMEINSERTED = DateTime.Now;
                        JCEAllow.INSERTEDBY = "admin";
                        //JCEAllow.INSERTEDBY = User.Identity.Name;
                        db.JUNIORCEALLOWANCE.Add(JCEAllow);

                        var jce = db.JUNIORCE.Find(id);
                        jce.DATETIMEUPDATED = DateTime.Now;
                        //jce.UPDATEDBY = User.Identity.Name;
                        jce.UPDATEDBY = "admin";
                        jce.ISEDIT = true;


                    }
                    try
                    {
                        db.SaveChanges();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }

                }
                else
                {
                    errorMessage = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                }
            }
            else
            {
                errorMessage = "You have not selected any Allowance to be saved. Please ensure that Junior CE has at least one (1) Allowance before saving changes.";
            }
             
            return Json(success ? JsonResponse.SuccessResponse("Junior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
        }




        /*-----------------------------------------End Junior CE Allowance--------------------------------------   */
         
        /*-----------------------------------------Start Junior CE Standard Deduction 1--------------------------------------   */

        public ActionResult IndexJCEDeduction1()
        {
            List<JuniorCE> getCurrentJCEList = staffViewData.GetCurrentJuniorCEList();
            return View("IndexJCEDeduction1", getCurrentJCEList); 
        }

        public ActionResult JCEDeduction1(int id)
        {
            var deduc = db.JUNIORCEDEDUCTION.Where(a => a.JUNIORCEID == id && a.STATUS == 1).ToList();
            var JCEDeduc=new JuniorCEDeduction1();
            JCEDeduc.JuniorCEDeduction1Details=new List<JuniorCEDeduction1>();
            foreach (var details in deduc)
            {
                var JCED=new JuniorCEDeduction1(); 
                JCED.JuniorCEID = details.JUNIORCEID;
                JCED.DeductionName = details.DEDUCTION.DEDUCTIONNAME;
                JCED.Amount = details.DEDUCTIONAMOUNT; 

                JCEDeduc.JuniorCEDeduction1Details.Add(JCED);  
            }
            var jce = db.JUNIORCE.Find(id);
            ViewBag.Name = jce.TITLE.TITLENAME + " " + jce.SURNAME + " " + jce.OTHERNAME;
            JCEDeduc.JuniorCEID = id;
            return View("JCEDeduction1",JCEDeduc);
        }
        [HttpPost]
        public ActionResult JCEDeduction1(int id,JuniorCEDeduction1 data)
        {
            if (data.JuniorCEDeduction1Details != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.JuniorCEDeduction1Details)
                    {
                        var item =
                            db.DEDUCTION.Where(a => a.DEDUCTIONNAME == details.DeductionName && a.STATUS == 1 || a.DEDUCTIONNAME == details.DeductionName && a.STATUS == 3)
                                .Select(a => new Deduction() { DeductionID = a.DEDUCTIONID })
                                .FirstOrDefault(); 
                        var JCED = db.JUNIORCEDEDUCTION.FirstOrDefault(a => a.JUNIORCEID == id && a.STATUS == 1 && a.DEDUCTIONID == item.DeductionID); 
                        if (details.Amount > 10000)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                         " amount entered must not be greater than 10,000. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }  
                        JCED.DEDUCTIONAMOUNT = details.Amount;
                        JCED.DATETIMEUPDATED = DateTime.Now;
                        JCED.UPDATEDBY = "admin";
                        //JCED.UPDATEDBY = User.Identity.Name;


                        var jce = db.JUNIORCE.Find(id);
                        jce.DATETIMEUPDATED = DateTime.Now;
                        jce.UPDATEDBY = "admin";
                        //jce.UPDATEDBY = User.Identity.Name;
                        jce.ISEDIT = true;

                    }
                    try
                    {
                        db.SaveChanges();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }

                }
                else
                {
                    errorMessage = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                }
            }
            else
            {
                errorMessage = "You have not selected any Deduction to be saved. Please ensure that Junior CE has at least one (1) Deduction before saving changes.";
            }

            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
        }


        public ActionResult JCEDeduction1Add(int id)
        { 
            var JCEDeduc=new JuniorCEDeduction1();
          
            var jce = db.JUNIORCE.Find(id);
            ViewBag.Name = jce.TITLE.TITLENAME + " " + jce.SURNAME + " " + jce.OTHERNAME;
            JCEDeduc.JuniorCEID = id;
            return View("JCEDeduction1Add",JCEDeduc);
        }

        [HttpPost]
        public ActionResult JCEDeduction1Add(int id,JuniorCEDeduction1 data)
        {
                
            if (data.JuniorCEDeduction1Details != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.JuniorCEDeduction1Details)
                    {
                        var item =
                            db.DEDUCTION.Where(a => a.DEDUCTIONNAME == details.DeductionName && a.STATUS == 1 )
                                .Select(a => new Deduction() { DeductionID = a.DEDUCTIONID })
                                .FirstOrDefault();
                        if (item == null)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                           " does not exist in this context. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var JCED = db.JUNIORCEDEDUCTION.FirstOrDefault(a => a.JUNIORCEID == id && a.STATUS == 1 && a.DEDUCTIONID == item.DeductionID);

                        if (JCED != null)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                          " already exist for this Junior CE.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }

                        if (details.Amount > 10000)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                         " amount entered must not be greater than 10,000. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }

                        var JCEDeduc = new JUNIORCEDEDUCTION(); 
                        JCEDeduc.JUNIORCEID = id;
                        JCEDeduc.STATUS = 1;
                        JCEDeduc.DEDUCTIONAMOUNT = details.Amount;
                        JCEDeduc.DEDUCTIONID = item.DeductionID; 
                        JCEDeduc.ID = Guid.NewGuid();
                        JCEDeduc.DATETIMEINSERTED = DateTime.Now;
                        JCEDeduc.INSERTEDBY = "admin";
                        //JCEAllow.INSERTEDBY = User.Identity.Name;
                        db.JUNIORCEDEDUCTION.Add(JCEDeduc);


                        var jce = db.JUNIORCE.Find(data.JuniorCEID);
                        jce.DATETIMEUPDATED = DateTime.Now;
                        jce.UPDATEDBY = "admin";
                        //jce.UPDATEDBY = User.Identity.Name;
                        jce.ISEDIT = true;

                    }
                    try
                    {
                        db.SaveChanges();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }

                }
                else
                {
                    errorMessage = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                }
            }
            else
            {
                errorMessage = "You have not selected any Deduction to be saved. Please ensure that Junior CE has at least one (1) Deduction before saving changes.";
            } 

            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
        }




        /*-----------------------------------------End Junior CE Standard Deduction 1--------------------------------------   */
       
        /*-----------------------------------------Start Junior CE Other Deduction 2--------------------------------------   */

        public ActionResult IndexJCEDeduction2()
        {
            List<JuniorCE> getCurrentJCEList = staffViewData.GetCurrentJuniorCEList();
            return View("IndexJCEDeduction2", getCurrentJCEList); 
        }
            
        public ActionResult JCEDeduction2(int id)
        {
            var deduc = db.JUNIORCEDEDUCTION.Where(a => a.JUNIORCEID == id && a.STATUS == 2).ToList();
            var JCEDeduc=new JuniorCEDeduction2();
            JCEDeduc.JuniorCEDeduction2Details=new List<JuniorCEDeduction2>();
            foreach (var details in deduc)
            {
                var JCED=new JuniorCEDeduction2();
                 
                JCED.JuniorCEID = details.JUNIORCEID;
                JCED.DeductionName = details.DEDUCTION.DEDUCTIONNAME;
                JCED.Amount = details.DEDUCTIONAMOUNT;
                JCED.TotalAmount = details.TOTALAMOUNT.Value;
                JCED.DeductionDate = details.DEDUCTIONDATE;
                JCED.Balance = details.BALANCE.Value;

                JCEDeduc.JuniorCEDeduction2Details.Add(JCED); 

            }
            var jce = db.JUNIORCE.Find(id);
            ViewBag.Name = jce.TITLE.TITLENAME + " " + jce.SURNAME + " " + jce.OTHERNAME;
            JCEDeduc.JuniorCEID = id;
            return View("JCEDeduction2",JCEDeduc);
        }

        [HttpPost]
        public ActionResult JCEDeduction2(int id,JuniorCEDeduction2 data)
        {
            if (data.JuniorCEDeduction2Details != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.JuniorCEDeduction2Details)
                    {
                        var item =
                            db.DEDUCTION.Where(a => a.DEDUCTIONNAME == details.DeductionName )
                                .Select(a => new Deduction() { DeductionID = a.DEDUCTIONID })
                                .FirstOrDefault();
                       
                        var JCED = db.JUNIORCEDEDUCTION.FirstOrDefault(a => a.JUNIORCEID == data.JuniorCEID && a.STATUS == 2 && a.DEDUCTIONID == item.DeductionID);
                         
                        if (details.TotalAmount > 1000000)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                         " Total Amount entered must not be greater than 1,000,000. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        if (details.Amount >= details.TotalAmount)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                         " Amount " + details.Amount + " must not be greater than Total Amount " + details.TotalAmount + ". Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }

                        //JCED.TOTALAMOUNT = details.TotalAmount;
                        JCED.DEDUCTIONAMOUNT = details.Amount;
                        JCED.DATETIMEUPDATED = DateTime.Now;
                        JCED.UPDATEDBY = "admin";
                        //JCED.UPDATEDBY = User.Identity.Name;

                        var jce = db.JUNIORCE.Find(id);
                        jce.DATETIMEUPDATED=DateTime.Now;
                        jce.UPDATEDBY = "admin";
                        //jce.UPDATEDBY = User.Identity.Name;
                        jce.ISEDIT = true;


                    }
                    try
                    {
                        db.SaveChanges();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }

                }
                else
                {
                    errorMessage = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                }
            }
            else
            {
                errorMessage = "There is no Deduction to be saved.";
            }

            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult JCEDeduction2Add(int id)
        {
            var deduc = db.JUNIORCEDEDUCTION.Where(a => a.JUNIORCEID == id && a.STATUS == 2).ToList();
            var JCEDeduc=new JuniorCEDeduction2();
            JCEDeduc.JuniorCEDeduction2Details=new List<JuniorCEDeduction2>();
             
            var jce = db.JUNIORCE.Find(id);
            ViewBag.Name = jce.TITLE.TITLENAME + " " + jce.SURNAME + " " + jce.OTHERNAME;
            JCEDeduc.JuniorCEID = id;
            return View("JCEDeduction2Add",JCEDeduc);
        }

        [HttpPost]
        public ActionResult JCEDeduction2Add(int id,JuniorCEDeduction2 data)
        {

            if (data.JuniorCEDeduction2Details != null) 
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.JuniorCEDeduction2Details)
                    {
                       if (details.DeductionDate==null)
                        {
                            errorMessage = "Date is required.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var item =
                            db.DEDUCTION.Where(a => a.DEDUCTIONNAME == details.DeductionName && a.STATUS == 2)
                                .Select(a => new Deduction() { DeductionID = a.DEDUCTIONID })
                                .FirstOrDefault();
                        if (item == null)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                           " does not exist in this context. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        

                        var JCED = db.JUNIORCEDEDUCTION.FirstOrDefault(a => a.JUNIORCEID == id && a.STATUS == 2 && a.DEDUCTIONID == item.DeductionID);

                        if (JCED != null)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                          " already exist for this Junior CE.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }

                        if (details.TotalAmount > 1000000)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                         " Total Amount entered must not be greater than 1,000,000. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        if (details.Amount >= details.TotalAmount)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                         " Amount "+details.Amount + " must not be greater than Total Amount "+ details.TotalAmount+". Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }

                        var JCEDeduc = new JUNIORCEDEDUCTION(); 
                        JCEDeduc.JUNIORCEID = id;
                        JCEDeduc.STATUS = 2;
                        JCEDeduc.DEDUCTIONAMOUNT = details.Amount;
                        JCEDeduc.TOTALAMOUNT = details.TotalAmount;
                        JCEDeduc.BALANCE = details.TotalAmount;
                        JCEDeduc.DEDUCTIONID = item.DeductionID;
                        JCEDeduc.DEDUCTIONDATE = details.DeductionDate; 
                        JCEDeduc.ID = Guid.NewGuid();
                        JCEDeduc.DATETIMEINSERTED = DateTime.Now;
                        JCEDeduc.INSERTEDBY = "admin";
                        //JCEAllow.INSERTEDBY = User.Identity.Name;
                        db.JUNIORCEDEDUCTION.Add(JCEDeduc);

                        var jce = db.JUNIORCE.Find(id);
                        jce.UPDATEDBY = "admin";
                        //jce.UPDATEDBY = User.Identity.Name;
                        jce.DATETIMEUPDATED=DateTime.Now;
                        jce.ISEDIT = true;
                         

                    }
                    try
                    {
                        db.SaveChanges();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }

                }
                else
                {
                    errorMessage = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                }
            }
            else
            {
                errorMessage = "You have not selected any Deduction to be saved. Please ensure that Junior CE has at least one (1) Deduction before saving changes.";
            }

            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
        }






        /*-----------------------------------------End Junior CE Other Deduction 2--------------------------------------   */


        /*-----------------------------------------Start Senior CE Allowance--------------------------------------   */

        public ActionResult IndexSCEAllowance()
        {
            List<SeniorCE> getCurrentSCEList = staffViewData.GetCurrentSeniorCEList();
            return View("IndexSCEAllowance", getCurrentSCEList); 
        }

        public ActionResult SCEAllowance(int id)
        {
            var allow = db.SENIORCEALLOWANCE.Where(a => a.SENIORCEID == id && a.STATUS == 1).ToList();
            var SCEAllow=new SeniorCEAllowance();
            SCEAllow.SeniorCEAllowanceDetails=new List<SeniorCEAllowance>();
            foreach (var details in allow)
            {
                var SCEA=new SeniorCEAllowance();
                
                SCEA.SeniorCEID = details.SENIORCEID;
                SCEA.AllowanceName = details.ALLOWANCE.ALLOWANCENAME;
                SCEA.Amount = details.AMOUNT; 

                SCEAllow.SeniorCEAllowanceDetails.Add(SCEA);  
            }
            var sce = db.SENIORCE.Find(id);
            ViewBag.Name = sce.TITLE.TITLENAME + " " + sce.SURNAME + " " + sce.OTHERNAME;
            SCEAllow.SeniorCEID = id;
            return View("SCEAllowance",SCEAllow);
        }

        [HttpPost]
        public ActionResult SCEAllowance(int id,SeniorCEAllowance data)
        {
            if (data.SeniorCEAllowanceDetails != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.SeniorCEAllowanceDetails)
                    {
                        var item =
                            db.ALLOWANCE.Where(a => a.ALLOWANCENAME == details.AllowanceName && a.STATUS == 1)
                                .Select(a => new Allowance() { AllowanceID = a.ALLOWANCEID })
                                .FirstOrDefault();
                        
                        var SCEA = db.SENIORCEALLOWANCE.FirstOrDefault(a => a.SENIORCEID == id && a.STATUS == 1 && a.ALLOWANCEID == item.AllowanceID);
                         
                        if (details.Amount > 10000)
                        {
                            errorMessage = "Allowance " + details.AllowanceName +
                                         " amount must not be greater than 10,000. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }

                        SCEA.AMOUNT = details.Amount;
                        //SCEA.UPDATEDBY = User.Identity.Name;
                        SCEA.UPDATEDBY = "admin";
                        SCEA.DATETIMEUPDATED = DateTime.Now;

                        var sce = db.SENIORCE.Find(id);
                        sce.DATETIMEUPDATED=DateTime.Now;
                        sce.UPDATEDBY = "admin";
                        //sce.UPDATEDBY = User.Identity.Name;
                        sce.ISEDIT = true;

                    }
                    try
                    {
                        db.SaveChanges();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }

                }
                else
                {
                    errorMessage = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                }
            }
            else
            {
                errorMessage = "You have not selected any Allowance to be saved. Please ensure that Senior CE has at least one (1) Allowance before saving changes.";
            }



            return Json(success ? JsonResponse.SuccessResponse("Senior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult SCEAllowanceAdd(int id)
        {
            var SCEAllow = new SeniorCEAllowance();
            var sce = db.SENIORCE.Find(id);
            ViewBag.Name = sce.TITLE.TITLENAME + " " + sce.SURNAME + " " + sce.OTHERNAME;
            SCEAllow.SeniorCEID = id;
            return View("SCEAllowanceAdd", SCEAllow);
        }
        [HttpPost]  
        public ActionResult SCEAllowanceAdd(int id,SeniorCEAllowance data)
        {
            if (data.SeniorCEAllowanceDetails != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.SeniorCEAllowanceDetails)
                    {
                        var item =
                            db.ALLOWANCE.Where(a => a.ALLOWANCENAME == details.AllowanceName && a.STATUS == 1)
                                .Select(a => new Allowance() { AllowanceID = a.ALLOWANCEID })
                                .FirstOrDefault();
                        if (item == null)
                        {
                            errorMessage = "Allowance " + details.AllowanceName +
                                           " does not exist in this context. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var SCEA = db.SENIORCEALLOWANCE.FirstOrDefault(a => a.SENIORCEID == id && a.STATUS == 1 && a.ALLOWANCEID == item.AllowanceID);

                        if (SCEA != null)
                        {
                            errorMessage = "Allowance " + details.AllowanceName +
                                          " already exist for this Senior CE.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        if (details.Amount > 100000)
                        {
                            errorMessage = "Allowance " + details.AllowanceName +
                                         " amount must not be greater than 100,000. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var SCEAllow = new SENIORCEALLOWANCE();
                        SCEAllow.SENIORCEID = id;
                        SCEAllow.STATUS = 1;
                        SCEAllow.AMOUNT = details.Amount;
                        SCEAllow.ALLOWANCEID = item.AllowanceID;
                        SCEAllow.ID = Guid.NewGuid();
                        SCEAllow.DATETIMEINSERTED = DateTime.Now;
                        SCEAllow.INSERTEDBY = "admin";
                        //SCEAllow.INSERTEDBY = User.Identity.Name; 

                        db.SENIORCEALLOWANCE.Add(SCEAllow);

                        var sce = db.SENIORCE.Find(id);
                        sce.DATETIMEUPDATED=DateTime.Now;
                        sce.UPDATEDBY = "admin";
                        //sce.UPDATEDBY = User.Identity.Name;
                        sce.ISEDIT = true;

                    }
                    try
                    {
                        db.SaveChanges();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }

                }
                else
                {
                    errorMessage = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                }
            }
            else
            {
                errorMessage = "You have not selected any Allowance to be saved. Please ensure that Senior CE has at least one (1) Allowance before saving changes.";
            }



            return Json(success ? JsonResponse.SuccessResponse("Senior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
        }
         
        /*-----------------------------------------End Senior CE Allowance--------------------------------------   */


        /*-----------------------------------------Start Senior CE Standard Deduction 1--------------------------------------   */

        public ActionResult IndexSCEDeduction1()
        {
            List<SeniorCE> getCurrentSCEList = staffViewData.GetCurrentSeniorCEList();
            return View("IndexSCEDeduction1", getCurrentSCEList);
        }

        public ActionResult SCEDeduction1(int id)
        {
            var deduc = db.SENIORCEDEDUCTION.Where(a => a.SENIORCEID == id && a.STATUS == 1).ToList();
            var SCEDeduc = new SeniorCEDeduction1();
            SCEDeduc.SeniorCEDeduction1Details = new List<SeniorCEDeduction1>();
            foreach (var details in deduc)
            {
                var SCED = new SeniorCEDeduction1();
                
                SCED.SeniorCEID = details.SENIORCEID;
                SCED.DeductionName = details.DEDUCTION.DEDUCTIONNAME;
                SCED.Amount = details.DEDUCTIONAMOUNT;

                SCEDeduc.SeniorCEDeduction1Details.Add(SCED);

            }
            var sce = db.SENIORCE.Find(id);
            ViewBag.Name = sce.TITLE.TITLENAME + " " + sce.SURNAME + " " + sce.OTHERNAME;
            SCEDeduc.SeniorCEID = id;
            return View("SCEDeduction1", SCEDeduc);
        }

        [HttpPost] 
        public ActionResult SCEDeduction1(int id,SeniorCEDeduction1 data)
        {
            if (data.SeniorCEDeduction1Details != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.SeniorCEDeduction1Details)
                    {
                        var item =
                            db.DEDUCTION.Where(a => a.DEDUCTIONNAME == details.DeductionName && a.STATUS == 1)
                                .Select(a => new Deduction() { DeductionID = a.DEDUCTIONID })
                                .FirstOrDefault();
                         
                        var SCED = db.SENIORCEDEDUCTION.FirstOrDefault(a => a.SENIORCEID == id && a.STATUS == 1 && a.DEDUCTIONID == item.DeductionID);

                        
                        if (details.Amount > 1000000)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                         " Total Amount must not be greater than 1,000,000. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }


                        SCED.DEDUCTIONAMOUNT = details.Amount;
                        SCED.DATETIMEUPDATED=DateTime.Now;
                        //SCED.UPDATEDBY = User.Identity.Name;
                        SCED.UPDATEDBY = "admin";

                        var sce = db.SENIORCE.Find(id);
                        sce.DATETIMEUPDATED=DateTime.Now;
                        sce.UPDATEDBY = "admin";
                        //sce.UPDATEDBY = User.Identity.Name;
                        sce.ISEDIT = true;
                    }
                    try
                    {
                        db.SaveChanges();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }

                }
                else
                {
                    errorMessage = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                }
            }
            else
            {
                errorMessage = "You have not selected any Deduction to be saved. Please ensure that Senior CE has at least one (1) Deduction before saving changes.";
            }

            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult SCEDeduction1Add(int id)
        {
            var deduc = db.SENIORCEDEDUCTION.Where(a => a.SENIORCEID == id && a.STATUS == 1).ToList();
            var SCEDeduc = new SeniorCEDeduction1();
           
            var sce = db.SENIORCE.Find(id);
            ViewBag.Name = sce.TITLE.TITLENAME + " " + sce.SURNAME + " " + sce.OTHERNAME;
            SCEDeduc.SeniorCEID = id;
            return View("SCEDeduction1Add", SCEDeduc);
        }

        [HttpPost]
        public ActionResult SCEDeduction1Add(int id,SeniorCEDeduction1 data)
        {
                
            if (data.SeniorCEDeduction1Details != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.SeniorCEDeduction1Details)
                    {
                        var item =
                            db.DEDUCTION.Where(a => a.DEDUCTIONNAME == details.DeductionName && a.STATUS == 1)
                                .Select(a => new Deduction() { DeductionID = a.DEDUCTIONID })
                                .FirstOrDefault();
                        if (item == null)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                           " does not exist in this context. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var SCED = db.SENIORCEDEDUCTION.FirstOrDefault(a => a.SENIORCEID == id && a.STATUS == 1 && a.DEDUCTIONID == item.DeductionID);

                        if (SCED != null)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                          " already exist for this user.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        if (details.Amount > 1000000)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                         " Total Amount must not be greater than 1,000,000. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }

                        var SCEDeduc = new SENIORCEDEDUCTION(); 
                        SCEDeduc.SENIORCEID = id;
                        SCEDeduc.STATUS = 1;
                        SCEDeduc.DEDUCTIONAMOUNT = details.Amount;
                        SCEDeduc.DEDUCTIONID = item.DeductionID; 
                        SCEDeduc.ID = Guid.NewGuid();
                        SCEDeduc.DATETIMEINSERTED = DateTime.Now;
                        SCEDeduc.INSERTEDBY = "admin";
                        //JCEAllow.INSERTEDBY = User.Identity.Name;
                        db.SENIORCEDEDUCTION.Add(SCEDeduc);

                        var sce = db.SENIORCE.Find(id);
                        sce.DATETIMEUPDATED=DateTime.Now;
                        sce.UPDATEDBY = "admin";
                        //sce.UPDATEDBY = User.Identity.Name;
                        sce.ISEDIT = true;

                    }
                    try
                    {
                        db.SaveChanges();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }

                }
                else
                {
                    errorMessage = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                }
            }
            else
            {
                errorMessage = "You have not selected any Deduction to be saved. Please ensure that Senior CE has at least one (1) Deduction before saving changes.";
            }

            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
        }
         

        /*-----------------------------------------End Senior CE Standard Deduction 1--------------------------------------   */


        /*-----------------------------------------Start Senior CE Standard Deduction 2--------------------------------------   */

        public ActionResult IndexSCEDeduction2()
        {
            List<SeniorCE> getCurrentSCEList = staffViewData.GetCurrentSeniorCEList();
            return View("IndexSCEDeduction2", getCurrentSCEList);
        }

        public ActionResult SCEDeduction2(int id)
        {
            var deduc = db.SENIORCEDEDUCTION.Where(a => a.SENIORCEID == id && a.STATUS == 2).ToList();
            var SCEDeduc = new SeniorCEDeduction2();
            SCEDeduc.SeniorCEDeduction2Details = new List<SeniorCEDeduction2>();
            foreach (var details in deduc)
            {
                var SCED = new SeniorCEDeduction2();
                
                SCED.SeniorCEID = details.SENIORCEID;
                SCED.DeductionName = details.DEDUCTION.DEDUCTIONNAME;
                SCED.Amount = details.DEDUCTIONAMOUNT;
                SCED.TotalAmount = details.TOTALAMOUNT.Value;
                SCED.DeductionDate = details.DEDUCTIONDATE.Value;
                SCED.Balance = details.BALANCE.Value;

                SCEDeduc.SeniorCEDeduction2Details.Add(SCED);

            }
            var sce = db.SENIORCE.Find(id);
            ViewBag.Name = sce.TITLE.TITLENAME + " " + sce.SURNAME + " " + sce.OTHERNAME;
            SCEDeduc.SeniorCEID = id;
            return View("SCEDeduction2", SCEDeduc);
        }

        [HttpPost]
        public ActionResult SCEDeduction2(int id,SeniorCEDeduction2 data)
        {
            if (data.SeniorCEDeduction2Details != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.SeniorCEDeduction2Details)
                    {
                        var item =
                            db.DEDUCTION.Where(a => a.DEDUCTIONNAME == details.DeductionName)
                                .Select(a => new Deduction() { DeductionID = a.DEDUCTIONID })
                                .FirstOrDefault();

                        var SCED = db.SENIORCEDEDUCTION.FirstOrDefault(a => a.SENIORCEID == id && a.STATUS == 1 && a.DEDUCTIONID == item.DeductionID);

                        if (details.TotalAmount > 1000000)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                         " Total Amount entered must not be greater than 1,000,000. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        if (details.Amount >= details.TotalAmount)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                         " Amount " + details.Amount + " must not be greater than Total Amount " + details.TotalAmount + ". Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }

                        SCED.TOTALAMOUNT = details.TotalAmount;
                        SCED.DEDUCTIONAMOUNT = details.Amount;
                        SCED.DATETIMEUPDATED = DateTime.Now;
                        SCED.UPDATEDBY = "admin";
                        //SCED.UPDATEDBY = User.Identity.Name;

                        var sce = db.SENIORCE.Find(id);
                        sce.DATETIMEUPDATED=DateTime.Now;
                        sce.UPDATEDBY = "admin";
                        //sce.UPDATEDBY = User.Identity.Name;
                        sce.ISEDIT = true;

                    }
                    try
                    {
                        db.SaveChanges();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }

                }
                else
                {
                    errorMessage = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                }
            }
            else
            {
                errorMessage = "There is no Deduction to be saved.";
            }

            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult SCEDeduction2Add(int id)
        {
            var deduc = db.SENIORCEDEDUCTION.Where(a => a.SENIORCEID == id && a.STATUS == 2).ToList();
            var SCEDeduc = new SeniorCEDeduction2();
            SCEDeduc.SeniorCEDeduction2Details = new List<SeniorCEDeduction2>();
            
            var sce = db.SENIORCE.Find(id);
            ViewBag.Name = sce.TITLE.TITLENAME + " " + sce.SURNAME + " " + sce.OTHERNAME;
            SCEDeduc.SeniorCEID = id;
            return View("SCEDeduction2Add", SCEDeduc);
        }

        [HttpPost]
        public ActionResult SCEDeduction2Add(int id,SeniorCEDeduction2 data)    
        {   

            if (data.SeniorCEDeduction2Details != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.SeniorCEDeduction2Details)
                    {
                        var item =
                            db.DEDUCTION.Where(a => a.DEDUCTIONNAME == details.DeductionName && a.STATUS == 2)
                                .Select(a => new Deduction() { DeductionID = a.DEDUCTIONID })
                                .FirstOrDefault();
                        if (item == null)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                           " does not exist in this context. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var SCED = db.SENIORCEDEDUCTION.FirstOrDefault(a => a.SENIORCEID == id && a.STATUS == 1 && a.DEDUCTIONID == item.DeductionID);

                        if (SCED != null)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                          " already exist for this user.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        if (details.TotalAmount > 1000000)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                         " Total Amount must not be greater than 1,000,000. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        if (details.Amount >=details.TotalAmount)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                         " Amount must not be greater than Total Amount "+details.TotalAmount+ ". Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }


                        var SCEDeduc = new SENIORCEDEDUCTION(); 
                        SCEDeduc.SENIORCEID = id;
                        SCEDeduc.STATUS = 2;
                        SCEDeduc.DEDUCTIONAMOUNT = details.Amount;
                        SCEDeduc.TOTALAMOUNT = details.TotalAmount;
                        SCEDeduc.BALANCE = details.TotalAmount;
                        SCEDeduc.DEDUCTIONID = item.DeductionID;
                        SCEDeduc.DEDUCTIONDATE = details.DeductionDate; 
                        SCEDeduc.ID = Guid.NewGuid();
                        SCEDeduc.DATETIMEINSERTED = DateTime.Now;
                        SCEDeduc.INSERTEDBY = "admin";
                        //JCEAllow.INSERTEDBY = User.Identity.Name;
                        db.SENIORCEDEDUCTION.Add(SCEDeduc);

                        var sce = db.SENIORCE.Find(id);
                        sce.UPDATEDBY = "admin";
                        //sce.UPDATEDBY = User.Identity.Name;
                        sce.DATETIMEUPDATED=DateTime.Now;
                        sce.ISEDIT = true;

                    }

                    try
                    {
                        db.SaveChanges();
                        success = true;
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }

                }
                else
                {
                    errorMessage = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                }
            }
            else
            {
                errorMessage = "You have not selected any Deduction to be saved. Please ensure that Senior CE has at least one (1) Deduction before saving changes.";
            }

            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
        }

         

        /*-----------------------------------------End Senior CE Standard Deduction 2--------------------------------------   */
         
        /*-----------------------------------------Start Allowance Listings --------------------------------------   */

        [HttpPost]
        public JsonResult GetAllowanceJsonResult(string Prefix)
        {
            var items = (from i in db.ALLOWANCE
                         where i.ALLOWANCENAME.Contains(Prefix) && i.STATUS==1
                         select new
                         {
                             label = i.ALLOWANCENAME,
                             val = i.ALLOWANCEID
                         }).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDeduc1JsonResult(string Prefix)
        {
            var items = (from i in db.DEDUCTION
                         where i.DEDUCTIONNAME.Contains(Prefix) && i.STATUS==1 || i.STATUS==3
                         select new
                         {
                             label = i.DEDUCTIONNAME,
                             val = i.DEDUCTIONID
                         }).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDeduc2JsonResult(string Prefix)
        {
            var items = (from i in db.DEDUCTION
                         where i.DEDUCTIONNAME.Contains(Prefix)  && i.STATUS==2
                         select new
                         {
                             label = i.DEDUCTIONNAME,
                             val = i.DEDUCTIONID
                         }).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
          
        /*-----------------------------------------End Allowance Listings --------------------------------------   */
         
    }
}