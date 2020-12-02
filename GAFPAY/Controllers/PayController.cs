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

        public ActionResult IndexRecruitBatch()
        {
            List<RecruitBatchTrial> getRecruitBatchTrial = payViewData.GetRecruitBatchTrialList();
            return View("IndexRecruitBatch",getRecruitBatchTrial);
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

        public ActionResult IndexOCBatch()  
        {
            List<OCBatchTrial> getOCBatchTrial = payViewData.GetOCBatchTrialList();
            return View("IndexOCBatch", getOCBatchTrial);
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
        public ActionResult JCEAllowance(JuniorCEAllowance data)
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
                        var JCEA = db.JUNIORCEALLOWANCE.FirstOrDefault(a => a.JUNIORCEID == data.JuniorCEID && a.STATUS == 1 && a.ALLOWANCEID == item.AllowanceID);

                        if (JCEA != null)
                        {
                            JCEA.AMOUNT = details.Amount;
                        }
                        else
                        {
                            var JCEAllow = new JUNIORCEALLOWANCE();
                            var guid=new Guid();
                            JCEAllow.JUNIORCEID = data.JuniorCEID;
                            JCEAllow.STATUS = 1;
                            JCEAllow.AMOUNT = details.Amount;
                            JCEAllow.ALLOWANCEID = item.AllowanceID;
                            JCEAllow.ID = guid;
                            JCEAllow.DATETIMEINSERTED = DateTime.Now;
                            JCEAllow.INSERTEDBY = "admin";
                            //JCEAllow.INSERTEDBY = User.Identity.Name;
                             
                            db.JUNIORCEALLOWANCE.Add(JCEAllow);
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
        public ActionResult JCEAllowanceAdd(JuniorCEAllowance data)
        {
            if (data.JuniorCEAllowanceDetails != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.JuniorCEAllowanceDetails)
                    {
                        var item =
                            db.ALLOWANCE.Where(a => a.ALLOWANCENAME == details.AllowanceName &&a.STATUS==1)
                                .Select(a => new Allowance() { AllowanceID = a.ALLOWANCEID })
                                .FirstOrDefault();
                        if (item == null)
                        {
                            errorMessage = "Allowance " + details.AllowanceName +
                                           " does not exist in this context. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var JCEA = db.JUNIORCEALLOWANCE.FirstOrDefault(a => a.JUNIORCEID == data.JuniorCEID && a.STATUS == 1 && a.ALLOWANCEID == item.AllowanceID);

                        if (JCEA != null)
                        {
                            errorMessage = "Allowance " + details.AllowanceName +
                                          " already exist for this user.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var JCEAllow = new JUNIORCEALLOWANCE();
                        var guid= Guid.NewGuid();
                        JCEAllow.JUNIORCEID = data.JuniorCEID;
                        JCEAllow.STATUS = 1;
                        JCEAllow.AMOUNT = details.Amount;
                        JCEAllow.ALLOWANCEID = item.AllowanceID; 
                      
                        JCEAllow.ID = guid;
                        JCEAllow.DATETIMEINSERTED = DateTime.Now;
                        JCEAllow.INSERTEDBY = "admin";
                        //JCEAllow.INSERTEDBY = User.Identity.Name;
                        db.JUNIORCEALLOWANCE.Add(JCEAllow);
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
        public ActionResult JCEDeduction1Add(int id)
        {
            var deduc = db.JUNIORCEDEDUCTION.Where(a => a.JUNIORCEID == id && a.STATUS == 1).ToList();
            var JCEDeduc=new JuniorCEDeduction1();
            //JCEDeduc.JuniorCEDeduction1Details=new List<JuniorCEDeduction1>();
            //foreach (var details in deduc)
            //{
            //    var JCED=new JuniorCEDeduction1(); 
            //    JCED.JuniorCEID = details.JUNIORCEID;
            //    JCED.DeductionName = details.DEDUCTION.DEDUCTIONNAME;
            //    JCED.Amount = details.DEDUCTIONAMOUNT; 

            //    JCEDeduc.JuniorCEDeduction1Details.Add(JCED); 

            //}
            var jce = db.JUNIORCE.Find(id);
            ViewBag.Name = jce.TITLE.TITLENAME + " " + jce.SURNAME + " " + jce.OTHERNAME;
            JCEDeduc.JuniorCEID = id;
            return View("JCEDeduction1Add",JCEDeduc);
        }

        [HttpPost]
        public ActionResult JCEDeduction1Add(JuniorCEDeduction1 data)
        {

            if (data.JuniorCEDeduction1Details != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.JuniorCEDeduction1Details)
                    {
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
                        var JCED = db.JUNIORCEDEDUCTION.FirstOrDefault(a => a.JUNIORCEID == data.JuniorCEID && a.STATUS == 1 && a.DEDUCTIONID == item.DeductionID);

                        if (JCED != null)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                          " already exist for this user.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var JCEDeduc = new JUNIORCEDEDUCTION();
                        var guid = Guid.NewGuid();
                        JCEDeduc.JUNIORCEID = data.JuniorCEID;
                        JCEDeduc.STATUS = 1;
                        JCEDeduc.DEDUCTIONAMOUNT = details.Amount;
                        JCEDeduc.DEDUCTIONID = item.DeductionID;

                        JCEDeduc.ID = guid;
                        JCEDeduc.DATETIMEINSERTED = DateTime.Now;
                        JCEDeduc.INSERTEDBY = "admin";
                        //JCEAllow.INSERTEDBY = User.Identity.Name;
                        db.JUNIORCEDEDUCTION.Add(JCEDeduc);
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
                JCED.DeductionDate = details.DEDUCTIONDATE.Value;
                JCED.Balance = details.BALANCE.Value;

                JCEDeduc.JuniorCEDeduction2Details.Add(JCED); 

            }
            var jce = db.JUNIORCE.Find(id);
            ViewBag.Name = jce.TITLE.TITLENAME + " " + jce.SURNAME + " " + jce.OTHERNAME;
            JCEDeduc.JuniorCEID = id;
            return View("JCEDeduction2",JCEDeduc);
        }
        public ActionResult JCEDeduction2Add(int id)
        {
            var deduc = db.JUNIORCEDEDUCTION.Where(a => a.JUNIORCEID == id && a.STATUS == 2).ToList();
            var JCEDeduc=new JuniorCEDeduction2();
            JCEDeduc.JuniorCEDeduction2Details=new List<JuniorCEDeduction2>();
            //foreach (var details in deduc)
            //{
            //    var JCED=new JuniorCEDeduction2();
                 
            //    JCED.JuniorCEID = details.JUNIORCEID;
            //    JCED.DeductionName = details.DEDUCTION.DEDUCTIONNAME;
            //    JCED.Amount = details.DEDUCTIONAMOUNT;
            //    JCED.TotalAmount = details.TOTALAMOUNT.Value;
            //    JCED.DeductionDate = details.DEDUCTIONDATE.Value;

            //    JCEDeduc.JuniorCEDeduction2Details.Add(JCED); 

            //}
            var jce = db.JUNIORCE.Find(id);
            ViewBag.Name = jce.TITLE.TITLENAME + " " + jce.SURNAME + " " + jce.OTHERNAME;
            JCEDeduc.JuniorCEID = id;
            return View("JCEDeduction2Add",JCEDeduc);
        }

        [HttpPost]
        public ActionResult JCEDeduction2Add(JuniorCEDeduction2 data)
        {

            if (data.JuniorCEDeduction2Details != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.JuniorCEDeduction2Details)
                    {
                        var item =
                            db.DEDUCTION.Where(a => a.DEDUCTIONNAME == details.DeductionName && a.STATUS == 1)
                                .Select(a => new Deduction() { DeductionID = a.DEDUCTIONID })
                                .FirstOrDefault();
                        if (item == null)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                           " does not exist in this context. Please check and try again.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var JCED = db.JUNIORCEDEDUCTION.FirstOrDefault(a => a.JUNIORCEID == data.JuniorCEID && a.STATUS == 1 && a.DEDUCTIONID == item.DeductionID);

                        if (JCED != null)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                          " already exist for this user.";
                            return Json(success ? JsonResponse.SuccessResponse("Junior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var JCEDeduc = new JUNIORCEDEDUCTION();
                        var guid = Guid.NewGuid();
                        JCEDeduc.JUNIORCEID = data.JuniorCEID;
                        JCEDeduc.STATUS = 2;
                        JCEDeduc.DEDUCTIONAMOUNT = details.Amount;
                        JCEDeduc.TOTALAMOUNT = details.TotalAmount;
                        JCEDeduc.BALANCE = details.TotalAmount;
                        JCEDeduc.DEDUCTIONID = item.DeductionID;
                        JCEDeduc.DEDUCTIONDATE = details.DeductionDate;

                        JCEDeduc.ID = guid;
                        JCEDeduc.DATETIMEINSERTED = DateTime.Now;
                        JCEDeduc.INSERTEDBY = "admin";
                        //JCEAllow.INSERTEDBY = User.Identity.Name;
                        db.JUNIORCEDEDUCTION.Add(JCEDeduc);
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

        public ActionResult SCEAllowanceAdd(int id)
        {
            var SCEAllow = new SeniorCEAllowance();
            var sce = db.SENIORCE.Find(id);
            ViewBag.Name = sce.TITLE.TITLENAME + " " + sce.SURNAME + " " + sce.OTHERNAME;
            SCEAllow.SeniorCEID = id;
            return View("SCEAllowanceAdd", SCEAllow);
        }
        [HttpPost]
        public ActionResult SCEAllowanceAdd(SeniorCEAllowance data)
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
                        var SCEA = db.SENIORCEALLOWANCE.FirstOrDefault(a => a.SENIORCEID == data.SeniorCEID && a.STATUS == 1 && a.ALLOWANCEID == item.AllowanceID);

                        if (SCEA != null)
                        {
                            errorMessage = "Allowance " + details.AllowanceName +
                                          " already exist for this user.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Allowance") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var SCEAllow = new SENIORCEALLOWANCE();
                        SCEAllow.SENIORCEID = data.SeniorCEID;
                        SCEAllow.STATUS = 1;
                        SCEAllow.AMOUNT = details.Amount;
                        SCEAllow.ALLOWANCEID = item.AllowanceID;
                        //SCEAllow.ID = guid;
                        //SCEAllow.DATETIMEINSERTED = DateTime.Now;
                        //SCEAllow.INSERTEDBY = "admin";
                        //JCEAllow.INSERTEDBY = User.Identity.Name;



                        db.SENIORCEALLOWANCE.Add(SCEAllow);
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


        public ActionResult SCEDeduction1Add(int id)
        {
            var deduc = db.SENIORCEDEDUCTION.Where(a => a.SENIORCEID == id && a.STATUS == 1).ToList();
            var SCEDeduc = new SeniorCEDeduction1();
            //JCEDeduc.JuniorCEDeduction1Details=new List<JuniorCEDeduction1>();
            //foreach (var details in deduc)
            //{
            //    var JCED=new JuniorCEDeduction1(); 
            //    JCED.JuniorCEID = details.JUNIORCEID;
            //    JCED.DeductionName = details.DEDUCTION.DEDUCTIONNAME;
            //    JCED.Amount = details.DEDUCTIONAMOUNT; 

            //    JCEDeduc.JuniorCEDeduction1Details.Add(JCED); 

            //}
            var sce = db.SENIORCE.Find(id);
            ViewBag.Name = sce.TITLE.TITLENAME + " " + sce.SURNAME + " " + sce.OTHERNAME;
            SCEDeduc.SeniorCEID = id;
            return View("SCEDeduction1Add", SCEDeduc);
        }

        [HttpPost]
        public ActionResult SCEDeduction1Add(SeniorCEDeduction1 data)
        {

            if (data.SeniorCEDeduction1Details != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.SeniorCEDeduction1Details)
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
                        var SCED = db.SENIORCEDEDUCTION.FirstOrDefault(a => a.SENIORCEID == data.SeniorCEID && a.STATUS == 1 && a.DEDUCTIONID == item.DeductionID);

                        if (SCED != null)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                          " already exist for this user.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var SCEDeduc = new SENIORCEDEDUCTION();
                        var guid = Guid.NewGuid();
                        SCEDeduc.SENIORCEID = data.SeniorCEID;
                        SCEDeduc.STATUS = 1;
                        SCEDeduc.DEDUCTIONAMOUNT = details.Amount;
                        SCEDeduc.DEDUCTIONID = item.DeductionID;

                        SCEDeduc.ID = guid;
                        SCEDeduc.DATETIMEINSERTED = DateTime.Now;
                        SCEDeduc.INSERTEDBY = "admin";
                        //JCEAllow.INSERTEDBY = User.Identity.Name;
                        db.SENIORCEDEDUCTION.Add(SCEDeduc);
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

        public ActionResult SCEDeduction2Add(int id)
        {
            var deduc = db.SENIORCEDEDUCTION.Where(a => a.SENIORCEID == id && a.STATUS == 2).ToList();
            var SCEDeduc = new SeniorCEDeduction2();
            SCEDeduc.SeniorCEDeduction2Details = new List<SeniorCEDeduction2>();
            //foreach (var details in deduc)
            //{
            //    var JCED=new JuniorCEDeduction2();

            //    JCED.JuniorCEID = details.JUNIORCEID;
            //    JCED.DeductionName = details.DEDUCTION.DEDUCTIONNAME;
            //    JCED.Amount = details.DEDUCTIONAMOUNT;
            //    JCED.TotalAmount = details.TOTALAMOUNT.Value;
            //    JCED.DeductionDate = details.DEDUCTIONDATE.Value;

            //    JCEDeduc.JuniorCEDeduction2Details.Add(JCED); 

            //}
            var sce = db.SENIORCE.Find(id);
            ViewBag.Name = sce.TITLE.TITLENAME + " " + sce.SURNAME + " " + sce.OTHERNAME;
            SCEDeduc.SeniorCEID = id;
            return View("SCEDeduction2Add", SCEDeduc);
        }

        [HttpPost]
        public ActionResult SCEDeduction2Add(SeniorCEDeduction2 data)
        {

            if (data.SeniorCEDeduction2Details != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var details in data.SeniorCEDeduction2Details)
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
                        var SCED = db.SENIORCEDEDUCTION.FirstOrDefault(a => a.SENIORCEID == data.SeniorCEID && a.STATUS == 1 && a.DEDUCTIONID == item.DeductionID);

                        if (SCED != null)
                        {
                            errorMessage = "Deduction " + details.DeductionName +
                                          " already exist for this user.";
                            return Json(success ? JsonResponse.SuccessResponse("Senior CE Deduction") : JsonResponse.ErrorResponse(errorMessage));
                        }
                        var SCEDeduc = new SENIORCEDEDUCTION();
                        var guid = Guid.NewGuid();
                        SCEDeduc.SENIORCEID = data.SeniorCEID;
                        SCEDeduc.STATUS = 2;
                        SCEDeduc.DEDUCTIONAMOUNT = details.Amount;
                        SCEDeduc.TOTALAMOUNT = details.TotalAmount;
                        SCEDeduc.BALANCE = details.TotalAmount;
                        SCEDeduc.DEDUCTIONID = item.DeductionID;
                        SCEDeduc.DEDUCTIONDATE = details.DeductionDate;

                        SCEDeduc.ID = guid;
                        SCEDeduc.DATETIMEINSERTED = DateTime.Now;
                        SCEDeduc.INSERTEDBY = "admin";
                        //JCEAllow.INSERTEDBY = User.Identity.Name;
                        db.SENIORCEDEDUCTION.Add(SCEDeduc);
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
                         where i.DEDUCTIONNAME.Contains(Prefix) && i.STATUS==2 
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
                         where i.DEDUCTIONNAME.Contains(Prefix) 
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