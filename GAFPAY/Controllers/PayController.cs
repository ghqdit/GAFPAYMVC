using GAFPAY.Models;
using GAFPAY.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAFPAY.Extensions;
using GAFPAY.ViewModel;

namespace GAFPAY.Controllers
{

    public class PayController : Controller
    {
        private DBGAFPAYEntities db = new DBGAFPAYEntities();
        private PayrollViewData payViewData =new PayrollViewData();
        private SettingsViewData settingsViewData =new SettingsViewData();
        private StaffViewData staffViewData =new StaffViewData();
        private bool success = false;
        private string errorMessage = "";
       



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



        public ActionResult CreateRecruitProcess()
        {
            
            var model=new RecruitTrialPay();
            model.MonthList = payViewData.getPayrollMonth();
            model.StaffList = staffViewData.getRecruit();
            ViewBag.User = "Recruit";

            return View("CreateRecruitSingle",model);
        }


        [HttpPost]
        public ActionResult CreateRecruitProcess(RecruitTrialPay data)
        {
            if (ModelState.IsValid)
            {
                var recruitID = data.RecruitID;
                var rec = db.RECRUIT.Find(recruitID);
                var constPay = db.MILITARYLEVSTEP.Find(rec.MILITARYLEVSTEPID);
                var tPay=new TrialPay(); 
                if (rec != null)
                {

                    var date = "";
                    var monthID = data.MonthID;
                    var dayID = 15;
                    var yearID = DateTime.Now.Year;
                    date = (dayID + "-" + monthID + "-" + yearID);
                    DateTime date1 = DateTime.ParseExact(date, @"d-M-yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);
                    tPay.TrialPayDate = date1; 
                    var checkAvail =  db.RECRUITTRIALPAY.FirstOrDefault(a => a.PAYDATE == tPay.TrialPayDate && a.RECRIUTID == recruitID);
                    if (checkAvail!=null)
                    {
                        errorMessage = "Trial Pay for (" + rec.SERVICENUMBER+") " + rec.SURNAME + " "+rec.OTHERNAME+ " for period " + tPay.TrialPayDate.ToString("MMMM yyyy") + " has already been processed.";

                    }
                    else
                    {
                        var recruitTrialPay=new RECRUITTRIALPAY();
                        recruitTrialPay.PAYDATE = date1;
                        recruitTrialPay.RECRIUTID = recruitID;
                        recruitTrialPay.CONSTPAY = constPay.CONSTPAY;
                        recruitTrialPay.BANKID = rec.RECRUITBANK.BANKID;
                        recruitTrialPay.DATETIMEINSERTED=DateTime.Now;
                        //recruitTrialPay.INSERTEDBY = User.Identity.Name;
                        recruitTrialPay.INSERTEDBY = "admin";

                        db.RECRUITTRIALPAY.Add(recruitTrialPay);

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


        public ActionResult CreateRecruitBatch()
        {
            var model = new RecruitTrialPay();
            model.MonthList = payViewData.getPayrollMonth();
             
            return View("CreateRecruitBatch",model);
        }
        [HttpPost]
        public ActionResult CreateRecruitBatch(RecruitTrialPay data)
        {
            var Processed = 0;
            var Unprocessed = 0;
            if (ModelState.IsValid)
            {
               
                var date = "";
                var monthID = data.MonthID;
                var dayID = 15;
                var yearID = DateTime.Now.Year;
                date = (dayID + "-" + monthID + "-" + yearID);
                DateTime date1 = DateTime.ParseExact(date, @"d-M-yyyy",
                    System.Globalization.CultureInfo.InvariantCulture);

                var recruit = db.RECRUIT.Where(a => a.GENERALSTATUSID == 1).ToList();
                foreach (var item in recruit)
                {
                    var tPay=new RecruitTrialPay();
                    tPay.TrialPayDate = date1;
                    tPay.RecruitID = item.RECRIUTID;

                    var recTrialPay = db.RECRUITTRIALPAY.FirstOrDefault(a => a.PAYDATE == date1 && a.RECRIUTID == item.RECRIUTID);
                    if (recTrialPay==null)
                    {
                        var recruitTrialPay = new RECRUITTRIALPAY();
                        recruitTrialPay.PAYDATE = date1;
                        recruitTrialPay.RECRIUTID = item.RECRIUTID;
                        recruitTrialPay.CONSTPAY = item.MILITARYLEVSTEP.CONSTPAY;
                        recruitTrialPay.BANKID = item.RECRUITBANK.BANKID;
                        recruitTrialPay.DATETIMEINSERTED = DateTime.Now;
                        //recruitTrialPay.INSERTEDBY = User.Identity.Name;
                        recruitTrialPay.INSERTEDBY = "admin";

                        db.RECRUITTRIALPAY.Add(recruitTrialPay);

                        
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



        public ActionResult CreateOCProcess()
        {

            var model = new OcTrialPay();
            model.MonthList = payViewData.getPayrollMonth();
            model.StaffList = staffViewData.getOfficerCadet(); 

            return View("CreateOCProcess", model);
        }


        [HttpPost]
        public ActionResult CreateOCProcess(OcTrialPay data)
        {
            if (ModelState.IsValid)
            {
                var ocID = data.OfficerCadetID;
                var oc = db.OFFICERCADET.Find(ocID);
                var constPay = db.MILITARYLEVSTEP.Find(oc.MILITARYLEVSTEPID);
                var tPay = new TrialPay();
                if (oc != null)
                {

                    var date = "";
                    var monthID = data.MonthID;
                    var dayID = 15;
                    var yearID = DateTime.Now.Year;
                    date = (dayID + "-" + monthID + "-" + yearID);
                    DateTime date1 = DateTime.ParseExact(date, @"d-M-yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);
                    tPay.TrialPayDate = date1;
                    var checkAvail = db.OFFICERCADETTRIALPAY.FirstOrDefault(a => a.PAYDATE == tPay.TrialPayDate && a.OFFICERCADETID == ocID);
                    if (checkAvail != null)
                    {
                        errorMessage = "Trial Pay for (" + oc.SERVICENUMBER + ") " + oc.SURNAME + " " + oc.OTHERNAME + " for period " + tPay.TrialPayDate.ToString("MMMM yyyy") + " has already been processed.";

                    }
                    else
                    {
                        var ocTrialPay = new OFFICERCADETTRIALPAY();
                        ocTrialPay.PAYDATE = date1;
                        ocTrialPay.OFFICERCADETID = ocID;
                        ocTrialPay.CONSTPAY = constPay.CONSTPAY;
                        ocTrialPay.BANKID = oc.OFFICERCADETBANK.BANKID;
                        ocTrialPay.DATETIMEINSERTED = DateTime.Now;
                        //ocTrialPay.INSERTEDBY = User.Identity.Name;
                        ocTrialPay.INSERTEDBY = "admin";

                        db.OFFICERCADETTRIALPAY.Add(ocTrialPay);

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


        public ActionResult CreateOCBatch()
        {
            var model = new OcTrialPay();
            model.MonthList = payViewData.getPayrollMonth();

            return View("CreateOCBatch", model);
        }
        [HttpPost]
        public ActionResult CreateOCBatch(OcTrialPay data)
        {
            var Processed = 0;
            var Unprocessed = 0;
            if (ModelState.IsValid)
            {

                var date = "";
                var monthID = data.MonthID;
                var dayID = 15;
                var yearID = DateTime.Now.Year;
                date = (dayID + "-" + monthID + "-" + yearID);
                DateTime date1 = DateTime.ParseExact(date, @"d-M-yyyy",
                    System.Globalization.CultureInfo.InvariantCulture);

                var oc = db.OFFICERCADET.Where(a => a.GENERALSTATUSID == 1).ToList();
                foreach (var item in oc)
                {
                    var tPay = new OcTrialPay();
                    tPay.TrialPayDate = date1;
                    tPay.OfficerCadetID = item.OFFICERCADETID;

                    var ocTrialPay = db.OFFICERCADETTRIALPAY.FirstOrDefault(a => a.PAYDATE == date1 && a.OFFICERCADETID == item.OFFICERCADETID);
                    if (ocTrialPay == null)
                    {
                        var offCadTrialPay = new OFFICERCADETTRIALPAY();
                        offCadTrialPay.PAYDATE = date1;
                        offCadTrialPay.OFFICERCADETID = item.OFFICERCADETID;
                        offCadTrialPay.CONSTPAY = item.MILITARYLEVSTEP.CONSTPAY;
                        offCadTrialPay.BANKID = item.OFFICERCADETBANK.BANKID;
                        offCadTrialPay.DATETIMEINSERTED = DateTime.Now;
                        //offCadTrialPay.INSERTEDBY = User.Identity.Name;
                        offCadTrialPay.INSERTEDBY = "admin";

                        db.OFFICERCADETTRIALPAY.Add(offCadTrialPay);


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


        /*-----------------------------------------End Officer Cadet Trial Pay--------------------------------------   */



        /*-----------------------------------------Start Junior CE Trial Pay--------------------------------------   */


        /*-----------------------------------------End Junior CE Trial Pay--------------------------------------   */


        /*-----------------------------------------Start Senior CE Trial Pay--------------------------------------   */



        /*-----------------------------------------End Senior CE Trial Pay--------------------------------------   */



        /*-----------------------------------------Start Soldier Trial Pay--------------------------------------   */



        /*-----------------------------------------End Soldier Trial Pay--------------------------------------   */


        /*-----------------------------------------Start Officer Trial Pay--------------------------------------   */

        /*-----------------------------------------End Officer Trial Pay--------------------------------------   */


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
                JCEA.JuniorCEAllowanceID = details.JUNIORCEALLOWANCESID;
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

        /*-----------------------------------------End Junior CE Allowance--------------------------------------   */
        
        









    }
}