using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAFPAY.Extensions;
using GAFPAY.Models;
using GAFPAY.ViewData;
using GAFPAY.ViewModel;

namespace GAFPAY.Controllers
{
    public class StaffController : Controller
    {
        private StaffViewData staffViewData = new StaffViewData();
        private SettingsViewData settingsViewData = new SettingsViewData();
        private bool success = false;
        private string errorMessage = "";
        private DBGAFPAYEntities db = new DBGAFPAYEntities();
        // GET: Staff
        /*-----------------------------Start Recruit------------------------------------*/

        public ActionResult IndexRecruit()
        {
            List<Recruit> getCurrentRecruitList = staffViewData.GetCurrentRecruitsList();
            return View("IndexRecruit",getCurrentRecruitList);
        }

        public ActionResult CreateRecruit()
        {
            var model=new Recruit();
            model.GenderList = settingsViewData.getGenders();
            model.RegionList = settingsViewData.getRegion();
            model.ReligionList = settingsViewData.getReligion();
            model.BloodGroupList = settingsViewData.getBloodGroups();
            model.RankList = settingsViewData.getRecruitRanks();
            model.ServiceList = settingsViewData.getServices();
            model.GeneralStatusList = settingsViewData.getGeneralStatus();
            //model.RecruitCourseList = settingsViewData.getRecruitCourse();
            model.BankNameList = settingsViewData.getBankNames();
            
            return View("CreateRecruit",model);
        }
        [HttpPost]
        public ActionResult CreateRecruit(Recruit data)
        {
            if (ModelState.IsValid)
            {
                var Rankid = 52;
                var genstatus = 1;
                var reclevstep = 2;
                var rec=new RECRUIT();
                var recBank=new RECRUITBANK();

                rec.SURNAME = data.Surname;
                rec.OTHERNAME = data.Othername;
                rec.DOB = data.DOB;
                rec.PHONENUMBER = data.PhoneNumber;
                rec.EMAILADDRESS = data.EmailAddress;
                rec.RESADDRESS = data.ResAddress;
                rec.SERVICENUMBER = data.ServiceNumber;
                rec.HOMETOWN = data.Hometown;
                rec.GENDERID = data.GenderID;
                rec.TCID = data.TrainingCenterID;
                rec.REGIONID = data.RegionID;
                rec.RELIGIONID = data.ReligionID;
                rec.RANKID = Rankid; 
                rec.BLOODGROUPID = data.BloodGroupID;
                //rec.INSERTEDBY = User.Identity.Name;
                rec.INSERTEDBY = "admin";
                rec.DATETIMEINSERTED = DateTime.Now;
                rec.RECRUITSTARTDATE = data.DateRecruitStart;
                rec.SERVICEID = data.ServiceID;
                rec.GENERALSTATUSID = genstatus;
                rec.MILITARYLEVSTEPID = reclevstep;
                rec.RCID = data.RecruitCourseID;

                db.RECRUIT.Add(rec);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                    errorMessage = e.Message;
                }
                

                var RecruitID = rec.RECRIUTID;
                recBank.ACCOUNTNUMBER = data.AccountNumber;
                recBank.BANKID = data.BankID;
                recBank.RECRIUTID = RecruitID;
                db.RECRUITBANK.Add(recBank);


                if (data.ImageUpload != null)
                {
                    var recPic=new RECRUITIMAGE();
                    HttpPostedFileBase imageUpload = data.ImageUpload;
                    byte[] picBytes = staffViewData.processImage(imageUpload);
                    var picUrl=new Uri(Request.Url,Url.Content(staffViewData.storeRecruitImage(imageUpload,rec.RECRIUTID)));
                    var PicPath = RecruitID + ".jpg";
                    recPic.PICTUREPATH = PicPath;
                    recPic.RECRIUTID = RecruitID;

                    db.RECRUITIMAGE.Add(recPic);

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

            return Json(success ? JsonResponse.SuccessResponse("Recruit") : JsonResponse.ErrorResponse(errorMessage));
        }



        public ActionResult EditRecruit(int id)
        {
            var model=new Recruit();
            var rec = db.RECRUIT.Find(id);
            var recImage = db.RECRUITIMAGE.Find(id);
            if (rec!=null)
            {
                model.Surname = rec.SURNAME;
                model.Othername = rec.OTHERNAME;
                model.GenderID = rec.GENDERID;
                model.DOB = rec.DOB;
                model.ServiceNumber = rec.SERVICENUMBER;
                model.PhoneNumber = rec.PHONENUMBER;
                model.ReligionID = rec.RELIGIONID;
                model.EmailAddress = rec.EMAILADDRESS;
                model.RegionID = rec.REGIONID;
                model.Hometown = rec.HOMETOWN;
                model.ResAddress = rec.RESADDRESS;
                model.GeneralStatusID = rec.GENERALSTATUSID;
                model.BloodGroupID = rec.BLOODGROUPID;
                if (recImage!=null)
                {
                    model.ImageName = recImage.PICTUREPATH;
                }

            }
            else
            {
                ViewBag.ErrorMessage = "Recruit does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            model.GenderList = settingsViewData.getGenders();
            model.RegionList = settingsViewData.getRegion();
            model.ReligionList = settingsViewData.getReligion();
            model.GeneralStatusList = settingsViewData.getGeneralStatus();
            model.BloodGroupList = settingsViewData.getBloodGroups();

            return View("EditRecruit",model);
        }

        
        [HttpPost]
        public ActionResult EditRecruit(int id, Recruit data)
        {
            if (ModelState.IsValid)
            {
                var rec = db.RECRUIT.Find(id);
                rec.SURNAME = data.Surname;
                rec.OTHERNAME = data.Othername;
                rec.DOB = data.DOB;
                rec.SERVICENUMBER = data.ServiceNumber;
                rec.RELIGIONID = data.ReligionID;
                rec.PHONENUMBER = data.PhoneNumber;
                rec.EMAILADDRESS = data.EmailAddress;
                rec.REGIONID = data.RegionID;
                rec.HOMETOWN = data.Hometown;
                rec.RESADDRESS = data.ResAddress;
                rec.GENERALSTATUSID = data.GeneralStatusID;
                
              
                
                if (data.ImageUpload!=null)
                {
                    var dataPic = db.RECRUITIMAGE.Find(id);
                    if (dataPic!=null)
                    {

                        HttpPostedFileBase imageUpload = data.ImageUpload;
                        byte[] picBytes = staffViewData.processImage(imageUpload);
                        var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeRecruitImage(imageUpload, rec.RECRIUTID)));
                        var PicPath = id + ".jpg";
                        dataPic.PICTUREPATH = PicPath;
                        dataPic.RECRIUTID = id;
                        var MIMEType = imageUpload.GetType();
                    }
                    else
                    {

                        var recPic=new RECRUITIMAGE();
                        HttpPostedFileBase imageUpload = data.ImageUpload;
                        byte[] picBytes = staffViewData.processImage(imageUpload);
                        var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeRecruitImage(imageUpload, rec.RECRIUTID)));
                        var PicPath = id + ".jpg";
                        recPic.PICTUREPATH = PicPath;
                        recPic.RECRIUTID = id; 
                        var MIMEType = imageUpload.GetType();
                        

                        db.RECRUITIMAGE.Add(recPic);
                        

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


            return Json(success ? JsonResponse.SuccessResponse("Recruit") : JsonResponse.ErrorResponse(errorMessage));
        }


        public ActionResult EditRecruitBank(int id)
        {

            var rec = db.RECRUITBANK.Find(id);
            var model=new RecruitBank();
            if (rec != null)
            {
                model.BankNameList = settingsViewData.getBankNames();
                //model.AccountNumber = rec.ACCOUNTNUMBER;
            }
            else
            {
                ViewBag.ErrorMessage = "Recruit bank does not exist. Kinldy contact Database Administrator for assistance";
                return View ("Error");
            }


            return View("EditRecruitBank",model);
        }

        [HttpPost]
        public ActionResult EditRecruitBank(int id, RecruitBank data)
        {
            if (ModelState.IsValid)
            {
                var rec = db.RECRUITBANK.Find(id);
                rec.ACCOUNTNUMBER = data.AccountNumber;
                rec.BANKID = data.BankID;

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


            return Json(success ? JsonResponse.SuccessResponse("Recruit Bank") : JsonResponse.ErrorResponse(errorMessage));
        }


        public ActionResult RemoveRecruit()
        {
            return Json(success ? JsonResponse.SuccessResponse("Recruit ") : JsonResponse.ErrorResponse(errorMessage));
        }






        /*-----------------------------End Recruit------------------------------------*/

        /*-----------------------------Start Oficer Cadet------------------------------------*/

        public ActionResult IndexOfficerCadet()
        {
            List<OfficerCadet> getCurrentOCList = staffViewData.GetCurrentOfficerCadetList();
            return View("IndexOfficerCadet",getCurrentOCList);
        }

        public ActionResult CreateOfficerCadet()
        {
            var model=new OfficerCadet();
            model.GenderList = settingsViewData.getGenders();
            model.RegionList = settingsViewData.getRegion();
            model.ReligionList = settingsViewData.getReligion();
            model.BloodGroupList = settingsViewData.getBloodGroups();
            model.RankList = settingsViewData.getRecruitRanks();
            model.ServiceList = settingsViewData.getServices();
            model.GeneralStatusList = settingsViewData.getGeneralStatus(); 
            model.CommissionTypeList = settingsViewData.getCommissionType();
            //model.OfficerIntake=settingsViewData.geto
            model.BankNameList = settingsViewData.getBankNames();
            return View("CreateOfficerCadet",model);
        }

        [HttpPost]

        public ActionResult CreateOfficerCadet(OfficerCadet data)
        {
            if (ModelState.IsValid)
            {
                var oc=new OFFICERCADET();
                var armyRank = 54;
                var navyRank = 58;
                var airfRank = 55;

                var genStatus = 1;
                var ocLevStep = 61;

                if (data.ServiceID==1)
                {
                    oc.RANKID = armyRank;
                }else if (data.ServiceID==2)
                {
                    oc.RANKID = airfRank;

                }
                else
                {
                    oc.RANKID = navyRank;
                }
                 
                oc.SURNAME = data.Surname;
                oc.OTHERNAME = data.Othername;
                oc.GENDERID = data.GenderID;
                oc.SERVICENUMBER = data.ServiceNumber;
                oc.DOB = data.DOB;
                oc.RELIGIONID = data.ReligionID;
                oc.PHONENUMBER = data.PhoneNumber;
                oc.EMAILADDRESS = data.EmailAddress;
                oc.REGIONID = data.RegionID;
                oc.HOMETOWN = data.Hometown;
                oc.RESADDRESS = data.ResAddress;
                oc.OFFICERSTARTDATE = data.OfficerStartDate;
                oc.SERVICEID = data.ServiceID;
                oc.BLOODGROUPID = data.BloodGroupID;
                oc.OFFICERINTAKEID = data.OfficerIntakeID;
                oc.COMMISSIONTYPEID = data.CommissionTypeID;
                oc.GENERALSTATUSID = genStatus;
                oc.MILITARYLEVSTEPID = ocLevStep;
                //oc.INSERTEDBY = User.Identity.Name;
                oc.INSERTEDBY = "admin";
                oc.DATETIMEINSERTED=DateTime.Now;


                db.OFFICERCADET.Add(oc);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = e.Message;
                }

                var ocID = oc.OFFICERCADETID;
                var ocBank=new OFFICERCADETBANK();
                ocBank.OCACCOUNTNUMBER = data.AccountNumber;
                ocBank.BANKID = data.BankID;
                ocBank.OFFICERCADETID = ocID;

                db.OFFICERCADETBANK.Add(ocBank);


                if (data.ImageUpload != null)
                {
                    var ocPic = new OFFICERCADETIMAGE();
                    HttpPostedFileBase imageUpload = data.ImageUpload;
                    byte[] picBytes = staffViewData.processImage(imageUpload);
                    var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeOfficerCadetImage(imageUpload, ocID)));
                    var PicPath = ocID + ".jpg";
                    ocPic.PICTUREPATH = PicPath;
                    ocPic.OFFICERCADETID = ocID;

                    db.OFFICERCADETIMAGE.Add(ocPic);

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

            return Json(success ? JsonResponse.SuccessResponse("Officer Cadet") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult EditOfficerCadet(int id)
        {

            var oc = db.OFFICERCADET.Find(id);
            var model=new OfficerCadet();
            var ocPic = db.OFFICERCADETIMAGE.Find(id);
            if (oc!=null)
            {
                model.Surname = oc.SURNAME;
                model.Othername = oc.OTHERNAME;
                model.GenderID = oc.GENDERID;
                model.DOB = oc.DOB;
                model.ServiceNumber = oc.SERVICENUMBER;
                model.PhoneNumber = oc.PHONENUMBER;
                model.ReligionID = oc.RELIGIONID;
                model.EmailAddress = oc.EMAILADDRESS;
                model.RegionID = oc.REGIONID;
                model.Hometown = oc.HOMETOWN;
                model.ResAddress = oc.RESADDRESS;
                model.GeneralStatusID = oc.GENERALSTATUSID;
                model.RankID = oc.RANKID;
                model.ServiceID = oc.SERVICEID;
                model.BloodGroupID = oc.BLOODGROUPID;

                if (ocPic != null)
                {
                    model.ImageName = ocPic.PICTUREPATH;
                }
                 

            }
            else
            {
                ViewBag.ErrorMessage = "Officer Cadet does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            model.RankList = settingsViewData.getOfficerCadetRanks(oc.SERVICEID);
            model.ServiceList = settingsViewData.getServices();
            model.GeneralStatusList = settingsViewData.getGeneralStatus();
            model.GenderList = settingsViewData.getGenders();
            model.ReligionList = settingsViewData.getReligion();
            model.RegionList = settingsViewData.getRegion();
            model.BloodGroupList = settingsViewData.getBloodGroups();

            return View("EditOfficerCadet",model);
        }

        [HttpPost]
        public ActionResult EditOfficerCadet(int id, OfficerCadet data)
        {
            if (ModelState.IsValid)
            {
                var oc = db.OFFICERCADET.Find(id);
                var ocLevStep = 62;
                var armyRank = 53;
                var navyRank = 57;
                var airfRank = 56;
                oc.SURNAME = data.Surname;
                oc.OTHERNAME = data.Othername;
                oc.GENDERID = data.GenderID;
                oc.DOB = data.DOB;
                oc.SERVICENUMBER = data.ServiceNumber;
                oc.RELIGIONID = data.ReligionID;
                oc.PHONENUMBER = data.PhoneNumber;
                oc.EMAILADDRESS = data.EmailAddress;
                oc.REGIONID = data.RegionID;
                oc.HOMETOWN = data.Hometown;
                oc.BLOODGROUPID = data.BloodGroupID;
                oc.RESADDRESS = data.ResAddress;
                oc.GENERALSTATUSID = data.GeneralStatusID;
                oc.RANKID = data.RankID;
                if (data.RankID==armyRank||data.RankID==airfRank||data.RankID==navyRank)
                {
                    oc.MILITARYLEVSTEPID = ocLevStep;
                }

                if (data.ImageUpload != null)
                {
                    var dataPic = db.OFFICERCADETIMAGE.Find(id);
                    if (dataPic != null)
                    { 
                        HttpPostedFileBase imageUpload = data.ImageUpload;
                        byte[] picBytes = staffViewData.processImage(imageUpload);
                        var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeOfficerCadetImage(imageUpload, oc.OFFICERCADETID)));
                        var PicPath = id + ".jpg";
                        dataPic.PICTUREPATH = PicPath;
                        dataPic.OFFICERCADETID = id;
                        //var MIMEType = imageUpload.GetType();
                    }
                    else
                    {

                        var ocPic = new OFFICERCADETIMAGE();
                        HttpPostedFileBase imageUpload = data.ImageUpload;
                        byte[] picBytes = staffViewData.processImage(imageUpload);
                        var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeOfficerCadetImage(imageUpload, oc.OFFICERCADETID)));
                        var PicPath = id + ".jpg";
                        ocPic.PICTUREPATH = PicPath;
                        ocPic.OFFICERCADETID = id;
                         
                        db.OFFICERCADETIMAGE.Add(ocPic); 

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


            return Json(success ? JsonResponse.SuccessResponse("Officer Cadet") : JsonResponse.ErrorResponse(errorMessage));
        }


         
        public ActionResult EditOfficerCadetBank(int id)    
        {
                
            var model=new OfficerCadetBank();
            var oc = db.OFFICERCADETBANK.Find(id);
             
            if (oc != null)
            {
                model.BankNameList = settingsViewData.getBankNames();
                //model.AccountNumber = rec.ACCOUNTNUMBER;
            }
            else
            {
                ViewBag.ErrorMessage = "Officer Cadet bank does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }

            return View("EditOfficerCadetBank",model);
        }

        [HttpPost]
        public ActionResult EditOfficerCadetBank(int id, OfficerCadetBank data)
        {
            if (ModelState.IsValid)
            {
                var rec = db.OFFICERCADETBANK.Find(id);
                rec.OCACCOUNTNUMBER = data.AccountNumber;
                rec.BANKID = data.BankID;

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


            return Json(success ? JsonResponse.SuccessResponse("Officer Cadet Bank") : JsonResponse.ErrorResponse(errorMessage));
        }



        public ActionResult RemoveOfficerCadet()
        {
            return Json(success ? JsonResponse.SuccessResponse("Officer Cadet") : JsonResponse.ErrorResponse(errorMessage));
        }


        /*-----------------------------End Oficer Cadet------------------------------------*/

        


        /*-----------------------------Start Partial View content------------------------------------*/

        public ActionResult _MEDPCodeBox()
        {
            var medP=new JuniorCE();
            return PartialView(medP);
        }



        /*-----------------------------End Partial View content------------------------------------*/

             


        /*-----------------------------Start Junior CE------------------------------------*/

        public ActionResult IndexJCE()
        {
            List<JuniorCE> getCurrentJCEList = staffViewData.GetCurrentJuniorCEList();
            return View("IndexJCE", getCurrentJCEList);
        }

        public ActionResult CreateJCE()
        {
            var model = new JuniorCE();
            model.IsMedical = 0;
            model.GenderList = settingsViewData.getGenders();
            model.RegionList = settingsViewData.getRegion();
            model.ReligionList = settingsViewData.getReligion();
            model.BloodGroupList = settingsViewData.getBloodGroups();
            model.RankList = settingsViewData.getRecruitRanks();
            model.TitleList = settingsViewData.getTitles(); 
            model.GeneralStatusList = settingsViewData.getGeneralStatus(); 
            model.BankNameList = settingsViewData.getBankNames();
            model.CLevStepList = settingsViewData.getJCELevStep();
            model.UnitList = settingsViewData.getUnits();
            model.IsMedicalList = settingsViewData.getYesNo();
            model.IsDisabledList = settingsViewData.getYesNo();
            model.ProvidentFundList = settingsViewData.getProvidentFund();
            model.GradeList = settingsViewData.getGrade();


            return View("CreateJCE", model);
        }


        [HttpPost]
        public ActionResult CreateJCE(JuniorCE data)
        {
            if (ModelState.IsValid)
            {
                if (data.IsMedical == 1 && data.MedPCode == null)
                {
                    errorMessage = "MED P Code is required";
                    return Json(success ? JsonResponse.SuccessResponse("Junior CE") : JsonResponse.ErrorResponse(errorMessage));
                }
                var jce = new JUNIORCE();
                var genStatus = 1;


                jce.SURNAME = data.Surname;
                jce.OTHERNAME = data.Othername;
                jce.GENDERID = data.GenderID;
                jce.SERVICENUMBER = data.ServiceNumber;
                jce.DOB = data.DOB;
                jce.RELIGIONID = data.ReligionID;
                jce.PHONENUMBER = data.PhoneNumber;
                jce.EMAILADDRESS = data.EmailAddress;
                jce.REGIONID = data.RegionID;
                jce.HOMETOWN = data.Hometown;
                jce.RESADDRESS = data.ResAddress;
                jce.DATEEMPLOYED = data.DateEmployed;
                jce.DATEPROMOTED = data.DateEmployed;
                jce.SSNITNUMBER = data.SSNITNo;
                jce.ISMEDICAL = Convert.ToBoolean(data.IsMedical);
                jce.ISDISABLED = Convert.ToBoolean(data.IsDisabled);
                jce.UNITID = data.UnitID;
                jce.BLOODGROUPID = data.BloodGroupID;
                jce.GENERALSTATUSID = genStatus;
                jce.CIVILIANLEVSTEPID = data.CLevStepID;
                jce.TITLEID = data.TitleID;
                jce.GRADEID = data.GradeID;
                //jce.INSERTEDBY = User.Identity.Name;
                jce.INSERTEDBY = "admin";
                jce.DATETIMEINSERTED = DateTime.Now;
                jce.PROVIDENTFUNDID = data.ProvidentID;


                db.JUNIORCE.Add(jce);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = e.Message;
                }

                var jceID = jce.JUNIORCEID;
                var jceBank = new JUNIORCEBANK();
                jceBank.ACCOUNTNUMBER = data.AccountNumber;
                jceBank.BANKID = data.BankID;
                jceBank.JUNIORCEID = jceID;

                db.JUNIORCEBANK.Add(jceBank);

                if (data.ImageUpload != null)
                {
                    var jcePic = new JUNIORCEIMAGE();
                    HttpPostedFileBase imageUpload = data.ImageUpload;
                    byte[] picBytes = staffViewData.processImage(imageUpload);
                    var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeJCEImage(imageUpload, jceID)));
                    var PicPath = jceID + ".jpg";
                    jcePic.PICTUREPATH = PicPath;
                    jcePic.JUNIORCEID = jceID;
                    db.JUNIORCEIMAGE.Add(jcePic);

                }

                if (data.IsMedical == 1)
                {
                    var jceMed = new JUNIORCEMEDP();
                    jceMed.JUNIORCEID = jceID;
                    jceMed.MEDPCODE = data.MedPCode;

                    db.JUNIORCEMEDP.Add(jceMed);

                }

                //getting the const pay
                var levstepID = data.CLevStepID;
                var civilLevStep = db.CIVILIANLEVSTEP.FirstOrDefault(a => a.CIVILIANLEVSTEPID == levstepID);
                var constPay = civilLevStep.CONSTPAY;


                //Allowances insertion
                //var jceID = 1000;
                var clothingAmount = 40;
                var clothingAllowanceID = 1;

                var rationAllowanceID = 2;
                var rationAmount = 10;

                var disableID = 4;
                var disableRate = Convert.ToDecimal(0.2);
                var disabilityAmount = disableRate * constPay;




                var allowances = db.ALLOWANCE.Where(a => a.STATUS == 2).ToList();
                foreach (var item in allowances)
                {
                    var jceallow = new JUNIORCEALLOWANCE();
                    if (item.ALLOWANCEID == clothingAllowanceID)
                    {
                        jceallow.AMOUNT = clothingAmount;
                    }
                    else if (item.ALLOWANCEID == rationAllowanceID)
                    {
                        jceallow.AMOUNT = rationAmount;
                    }
                    jceallow.STATUS = 1;
                    jceallow.ALLOWANCEID = item.ALLOWANCEID;
                    jceallow.JUNIORCEID = jceID;

                    db.JUNIORCEALLOWANCE.Add(jceallow);

                }

                if (data.IsDisabled == 1)
                {
                    var disableAllow = new JUNIORCEALLOWANCE();
                    disableAllow.ALLOWANCEID = disableID;
                    disableAllow.AMOUNT = disabilityAmount;
                    disableAllow.STATUS = 1;
                    disableAllow.JUNIORCEID = jceID;

                    db.JUNIORCEALLOWANCE.Add(disableAllow);
                }
                 
                //Deductions insertion
                var taxID = 5;
                var ssnitID = 4;
                var CEWelfareID = 6;
                var provDeductionID = 2;
                var CEWelfareAmount = Convert.ToDecimal(15);
                var tax = Convert.ToDecimal(0);
                var tax1 = Convert.ToDecimal(0);
                var tax2 = Convert.ToDecimal(0);
                var tax3 = Convert.ToDecimal(0);
                var tax4 = Convert.ToDecimal(0);
                var taxValue1 = Convert.ToDecimal(100);
                var taxValue2 = Convert.ToDecimal(120);
                var taxValue3 = Convert.ToDecimal(3000);
                var taxValue4 = Convert.ToDecimal(16461);
                var taxRate1 = Convert.ToDecimal(0.05);
                var taxRate2 = Convert.ToDecimal(0.10);
                var taxRate3 = Convert.ToDecimal(0.175);
                var taxRate4 = Convert.ToDecimal(0.25);
                


                var ssnitRate = Convert.ToDecimal(18.5);
                var welfareID = 6;
                var providentID = data.ProvidentID;

                var provident = db.PROVIDENTFUND.Find(providentID);
                var provRate =Convert.ToDecimal(provident.PROVIDENTRATE);

                var provDeduction = (provRate/100)*constPay;
                var ssnitDeduction = (ssnitRate/100)*constPay;

                var taxCredit = (constPay - provDeduction);
                
                tax1 = taxValue1*taxRate1;
                taxCredit = taxCredit - taxValue1;

                if (taxCredit > 0 && taxCredit <= taxValue2)
                {
                    tax2 = taxCredit*taxRate2;
                    taxCredit = taxCredit - taxValue2;
                }
                else if (taxCredit >= taxValue2)  
                {
                    tax2 = taxValue2*taxRate2;
                    taxCredit = taxCredit - taxValue2;
                }
                 
                if (taxCredit>0 && taxCredit<=taxValue3)
                {
                    tax3 = taxCredit*taxRate3;
                    taxCredit = taxCredit - taxValue3;
                }
                else if (taxCredit>=taxValue3)  
                {
                    tax3 = taxValue3*taxRate3;
                    taxCredit = taxCredit - taxValue3;
                }

                if (taxCredit>0 && taxCredit<=taxValue4)
                {
                    tax4 = taxCredit*taxRate4;
                    taxCredit = taxCredit - taxValue4;
                }
                else if (taxCredit>=taxValue4)  
                {
                    tax4 = taxValue4*taxRate4;
                    taxCredit = taxCredit - taxValue4;
                }
                  
                tax = tax1 + tax2 + tax3 + tax4;

                var deductions = db.DEDUCTION.Where(a => a.STATUS == 2).ToList();
                foreach (var items in deductions)
                {
                    var jcededuc = new JUNIORCEDEDUCTION();
                    if (items.DEDUCTIONID==welfareID)
                    {
                        jcededuc.DEDUCTIONAMOUNT = CEWelfareAmount;
                        
                    }else if (items.DEDUCTIONID==ssnitID)
                    {
                        jcededuc.DEDUCTIONAMOUNT = ssnitDeduction;
                    }
                    else if (items.DEDUCTIONID==taxID)
                    {
                        jcededuc.DEDUCTIONAMOUNT = tax;
                    }else if (items.DEDUCTIONID==provDeductionID)
                    {
                        jcededuc.DEDUCTIONAMOUNT = provDeduction;
                    }

                    jcededuc.STATUS = 1;
                    jcededuc.DEDUCTIONID = items.DEDUCTIONID;
                    jcededuc.JUNIORCEID = jceID;

                    db.JUNIORCEDEDUCTION.Add(jcededuc);

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

            return Json(success ? JsonResponse.SuccessResponse("Junior CE") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult EditJCE(int id)
        {

            var oc = db.OFFICERCADET.Find(id);
            var model = new OfficerCadet();
            var ocPic = db.OFFICERCADETIMAGE.Find(id);
            if (oc != null)
            {
                model.Surname = oc.SURNAME;
                model.Othername = oc.OTHERNAME;
                model.GenderID = oc.GENDERID;
                model.DOB = oc.DOB;
                model.ServiceNumber = oc.SERVICENUMBER;
                model.PhoneNumber = oc.PHONENUMBER;
                model.ReligionID = oc.RELIGIONID;
                model.EmailAddress = oc.EMAILADDRESS;
                model.RegionID = oc.REGIONID;
                model.Hometown = oc.HOMETOWN;
                model.ResAddress = oc.RESADDRESS;
                model.GeneralStatusID = oc.GENERALSTATUSID;
                model.RankID = oc.RANKID;
                model.ServiceID = oc.SERVICEID;
                model.BloodGroupID = oc.BLOODGROUPID;

                if (ocPic != null)
                {
                    model.ImageName = ocPic.PICTUREPATH;
                }


            }
            else
            {
                ViewBag.ErrorMessage = "Officer Cadet does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            model.RankList = settingsViewData.getOfficerCadetRanks(oc.SERVICEID);
            model.ServiceList = settingsViewData.getServices();
            model.GeneralStatusList = settingsViewData.getGeneralStatus();
            model.GenderList = settingsViewData.getGenders();
            model.ReligionList = settingsViewData.getReligion();
            model.RegionList = settingsViewData.getRegion();
            model.BloodGroupList = settingsViewData.getBloodGroups();

            return View("EditOfficerCadet", model);
        }

        [HttpPost]
        public ActionResult EditJCE(int id, OfficerCadet data)
        {
            if (ModelState.IsValid)
            {
                var oc = db.OFFICERCADET.Find(id);
                oc.SURNAME = data.Surname;
                oc.OTHERNAME = data.Othername;
                oc.GENDERID = data.GenderID;
                oc.DOB = data.DOB;
                oc.SERVICENUMBER = data.ServiceNumber;
                oc.RELIGIONID = data.ReligionID;
                oc.PHONENUMBER = data.PhoneNumber;
                oc.EMAILADDRESS = data.EmailAddress;
                oc.REGIONID = data.RegionID;
                oc.HOMETOWN = data.Hometown;
                oc.BLOODGROUPID = data.BloodGroupID;
                oc.RESADDRESS = data.ResAddress;
                oc.GENERALSTATUSID = data.GeneralStatusID;
                oc.RANKID = data.RankID;

                if (data.ImageUpload != null)
                {
                    var dataPic = db.OFFICERCADETIMAGE.Find(id);
                    if (dataPic != null)
                    {
                        HttpPostedFileBase imageUpload = data.ImageUpload;
                        byte[] picBytes = staffViewData.processImage(imageUpload);
                        var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeOfficerCadetImage(imageUpload, oc.OFFICERCADETID)));
                        var PicPath = id + ".jpg";
                        dataPic.PICTUREPATH = PicPath;
                        dataPic.OFFICERCADETID = id;
                        //var MIMEType = imageUpload.GetType();
                    }
                    else
                    {

                        var ocPic = new OFFICERCADETIMAGE();
                        HttpPostedFileBase imageUpload = data.ImageUpload;
                        byte[] picBytes = staffViewData.processImage(imageUpload);
                        var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeOfficerCadetImage(imageUpload, oc.OFFICERCADETID)));
                        var PicPath = id + ".jpg";
                        ocPic.PICTUREPATH = PicPath;
                        ocPic.OFFICERCADETID = id;

                        db.OFFICERCADETIMAGE.Add(ocPic);

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
            
            return Json(success ? JsonResponse.SuccessResponse("Officer Cadet") : JsonResponse.ErrorResponse(errorMessage));
        }



        public ActionResult EditJCEBank(int id)
        {
            var model = new JuniorCEBank();
            var jce = db.JUNIORCEBANK.Find(id);

            if (jce != null)
            {
                model.BankNameList = settingsViewData.getBankNames();
                //model.AccountNumber = rec.ACCOUNTNUMBER;
            }
            else
            {
                ViewBag.ErrorMessage = "Junior CE bank does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }

            return View("EditJCEBank", model);
        }

        [HttpPost]
        public ActionResult EditJCEBank(int id, JuniorCEBank data)
        {
            if (ModelState.IsValid)
            {
                var jce = db.JUNIORCEBANK.Find(id);
                jce.ACCOUNTNUMBER = data.AccountNumber;
                jce.BANKID = data.BankID;

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


            return Json(success ? JsonResponse.SuccessResponse("Junior CE Bank") : JsonResponse.ErrorResponse(errorMessage));
        }



        public ActionResult RemoveJCE()
        {
            return Json(success ? JsonResponse.SuccessResponse("Junior CE") : JsonResponse.ErrorResponse(errorMessage));
        }


        /*-----------------------------End Junior CE------------------------------------*/

        /*-----------------------------Start Senior CE------------------------------------*/

        public ActionResult IndexSCE()
        {
            List<SeniorCE> getCurrentSCEList = staffViewData.GetCurrentSeniorCEList();
            return View("IndexSCE", getCurrentSCEList);
        }

        public ActionResult CreateSCE()
        {
            var model = new SeniorCE();
            model.IsMedical = 0;
            model.GenderList = settingsViewData.getGenders();
            model.RegionList = settingsViewData.getRegion();
            model.ReligionList = settingsViewData.getReligion();
            model.BloodGroupList = settingsViewData.getBloodGroups();
            model.RankList = settingsViewData.getRecruitRanks();
            model.TitleList = settingsViewData.getTitles(); 
            model.GeneralStatusList = settingsViewData.getGeneralStatus(); 
            model.BankNameList = settingsViewData.getBankNames();
            model.CLevStepList = settingsViewData.getJCELevStep();
            model.UnitList = settingsViewData.getUnits();
            model.IsMedicalList = settingsViewData.getYesNo();
            model.IsDisabledList = settingsViewData.getYesNo();
            model.ProvidentFundList = settingsViewData.getProvidentFund();
            model.GradeList = settingsViewData.getGrade();
            
            return View("CreateSCE", model);
        }


        [HttpPost]
        public ActionResult CreateSCE(SeniorCE data)
        {
            if (ModelState.IsValid)
            {
                if (data.IsMedical == 1 && data.MedPCode == null)
                {
                    errorMessage = "MED P Code is required";
                    return Json(success ? JsonResponse.SuccessResponse("Junior CE") : JsonResponse.ErrorResponse(errorMessage));
                }
                var sce = new SENIORCE();
                var genStatus = 1;


                sce.SURNAME = data.Surname;
                sce.OTHERNAME = data.Othername;
                sce.GENDERID = data.GenderID;
                sce.SERVICENUMBER = data.ServiceNumber;
                sce.DOB = data.DOB;
                sce.RELIGIONID = data.ReligionID;
                sce.PHONENUMBER = data.PhoneNumber;
                sce.EMAILADDRESS = data.EmailAddress;
                sce.REGIONID = data.RegionID;
                sce.HOMETOWN = data.Hometown;
                sce.RESADDRESS = data.ResAddress;
                sce.DATEEMPLOYED = data.DateEmployed;
                sce.DATEPROMOTED = data.DateEmployed;
                sce.SSNITNUMBER = data.SSNITNo;
                sce.ISMEDICAL = Convert.ToBoolean(data.IsMedical);
                sce.ISDISABLED = Convert.ToBoolean(data.IsDisabled);
                sce.UNITID = data.UnitID;
                sce.BLOODGROUPID = data.BloodGroupID;
                sce.GENERALSTATUSID = genStatus;
                sce.CIVILIANLEVSTEPID = data.CLevStepID;
                sce.TITLEID = data.TitleID;
                sce.GRADEID = data.GradeID;
                //jce.INSERTEDBY = User.Identity.Name;
                sce.INSERTEDBY = "admin";
                sce.DATETIMEINSERTED = DateTime.Now;
                sce.PROVIDENTFUNDID = data.ProvidentID;


                db.SENIORCE.Add(sce);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = e.Message;
                }

                var sceID = sce.SENIORCEID;
                var sceBank = new SENIORCEBANK();
                sceBank.ACCOUNTNUMBER = data.AccountNumber;
                sceBank.BANKID = data.BankID;
                sceBank.SENIORCEID = sceID;

                db.SENIORCEBANK.Add(sceBank);

                if (data.ImageUpload != null)
                {
                    var scePic = new SENIORCEIMAGE();
                    HttpPostedFileBase imageUpload = data.ImageUpload;
                    byte[] picBytes = staffViewData.processImage(imageUpload);
                    var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeSCEImage(imageUpload, sceID)));
                    var PicPath = sceID + ".jpg";
                    scePic.PICTUREPATH = PicPath;
                    scePic.SENIORCEID = sceID;
                    db.SENIORCEIMAGE.Add(scePic);

                }

                if (data.IsMedical == 1)
                {
                    var sceMed = new SENIORCEMEDP();
                    sceMed.SENIORCEID = sceID;
                    sceMed.MEDPCODE = data.MedPCode;

                    db.SENIORCEMEDP.Add(sceMed);

                }

                //getting the const pay
                var levstepID = data.CLevStepID;
                var civilLevStep = db.CIVILIANLEVSTEP.FirstOrDefault(a => a.CIVILIANLEVSTEPID == levstepID);
                var constPay = civilLevStep.CONSTPAY;


                //Allowances insertion
                //var jceID = 1000;
                var clothingAmount= 40;
                var clothingAllowanceID = 1;

                var rationAllowanceID = 2;
                var rationAmount = 10;

                var disableID = 4;
                var disableRate = Convert.ToDecimal(0.2);             
                var disabilityAmount = disableRate * constPay;
                
                
                

                var allowances = db.ALLOWANCE.Where(a => a.STATUS == 2).ToList();
                foreach (var item in allowances)
                {
                    var sceallow=new SENIORCEALLOWANCE();
                    if (item.ALLOWANCEID==clothingAllowanceID)
                    {
                        sceallow.AMOUNT = clothingAmount;
                    }else if (item.ALLOWANCEID==rationAllowanceID)
                    {
                        sceallow.AMOUNT = rationAmount;
                    }
                    sceallow.STATUS = 1; 
                    sceallow.ALLOWANCEID = item.ALLOWANCEID;
                    sceallow.SENIORCEID = sceID;

                    db.SENIORCEALLOWANCE.Add(sceallow);

                }

                if (data.IsDisabled==1)
                {
                    var disableAllow=new SENIORCEALLOWANCE();
                    disableAllow.ALLOWANCEID = disableID;
                    disableAllow.AMOUNT = disabilityAmount;
                    disableAllow.STATUS = 1;
                    disableAllow.SENIORCEID = sceID;

                    db.SENIORCEALLOWANCE.Add(disableAllow);
                }
                 

                //Deductions insertion
                var taxID = 5;
                var ssnitID = 4;
                var CEWelfareID = 6;
                var provDeductionID = 2;
                var CEWelfareAmount = Convert.ToDecimal(15);
                var tax = Convert.ToDecimal(0);
                var tax1 = Convert.ToDecimal(0);
                var tax2 = Convert.ToDecimal(0);
                var tax3 = Convert.ToDecimal(0);
                var tax4 = Convert.ToDecimal(0);
                var taxValue1 = Convert.ToDecimal(100);
                var taxValue2 = Convert.ToDecimal(120);
                var taxValue3 = Convert.ToDecimal(3000);
                var taxValue4 = Convert.ToDecimal(16461);
                var taxRate1 = Convert.ToDecimal(0.05);
                var taxRate2 = Convert.ToDecimal(0.10);
                var taxRate3 = Convert.ToDecimal(0.175);
                var taxRate4 = Convert.ToDecimal(0.25);
                


                var ssnitRate = Convert.ToDecimal(18.5);
                var welfareID = 6;
                var providentID = data.ProvidentID;

                var provident = db.PROVIDENTFUND.Find(providentID);
                var provRate =Convert.ToDecimal(provident.PROVIDENTRATE);

                var provDeduction = (provRate/100)*constPay;
                var ssnitDeduction = (ssnitRate/100)*constPay;

                var taxCredit = (constPay - provDeduction);
                
                tax1 = taxValue1*taxRate1;
                taxCredit = taxCredit - taxValue1; 
                tax2 = taxValue2*taxRate2;
                taxCredit = taxCredit - taxValue2;
                
                if (taxCredit>=taxValue3)
                {
                    tax3 = taxCredit*taxRate3;
                    taxCredit = taxCredit - taxValue3;
                }
                else
                {
                    tax3 = taxValue3*taxRate3;
                    taxCredit = taxCredit - taxValue3;
                }
                 


                tax = tax1 + tax2 + tax3 + tax4;

                var deductions = db.DEDUCTION.Where(a => a.STATUS == 2).ToList();
                foreach (var items in deductions)
                {
                    var scededuc = new SENIORCEDEDUCTION();
                    if (items.DEDUCTIONID==welfareID)
                    {
                        scededuc.DEDUCTIONAMOUNT = CEWelfareAmount;
                        
                    }else if (items.DEDUCTIONID==ssnitID)
                    {
                        scededuc.DEDUCTIONAMOUNT = ssnitDeduction;
                    }
                    else if (items.DEDUCTIONID==taxID)
                    {
                        scededuc.DEDUCTIONAMOUNT = tax;
                    }else if (items.DEDUCTIONID==provDeductionID)
                    {
                        scededuc.DEDUCTIONAMOUNT = provDeduction;
                    }

                    scededuc.STATUS = 1;
                    scededuc.DEDUCTIONID = items.DEDUCTIONID;
                    scededuc.SENIORCEID = sceID;

                    db.SENIORCEDEDUCTION.Add(scededuc);

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

            return Json(success ? JsonResponse.SuccessResponse("Senior CE") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult EditSCE(int id)
        {

            var oc = db.OFFICERCADET.Find(id);
            var model = new OfficerCadet();
            var ocPic = db.OFFICERCADETIMAGE.Find(id);
            if (oc != null)
            {
                model.Surname = oc.SURNAME;
                model.Othername = oc.OTHERNAME;
                model.GenderID = oc.GENDERID;
                model.DOB = oc.DOB;
                model.ServiceNumber = oc.SERVICENUMBER;
                model.PhoneNumber = oc.PHONENUMBER;
                model.ReligionID = oc.RELIGIONID;
                model.EmailAddress = oc.EMAILADDRESS;
                model.RegionID = oc.REGIONID;
                model.Hometown = oc.HOMETOWN;
                model.ResAddress = oc.RESADDRESS;
                model.GeneralStatusID = oc.GENERALSTATUSID;
                model.RankID = oc.RANKID;
                model.ServiceID = oc.SERVICEID;
                model.BloodGroupID = oc.BLOODGROUPID;

                if (ocPic != null)
                {
                    model.ImageName = ocPic.PICTUREPATH;
                }


            }
            else
            {
                ViewBag.ErrorMessage = "Officer Cadet does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            model.RankList = settingsViewData.getOfficerCadetRanks(oc.SERVICEID);
            model.ServiceList = settingsViewData.getServices();
            model.GeneralStatusList = settingsViewData.getGeneralStatus();
            model.GenderList = settingsViewData.getGenders();
            model.ReligionList = settingsViewData.getReligion();
            model.RegionList = settingsViewData.getRegion();
            model.BloodGroupList = settingsViewData.getBloodGroups();

            return View("EditOfficerCadet", model);
        }

        [HttpPost]
        public ActionResult EditSCE(int id, OfficerCadet data)
        {
            if (ModelState.IsValid)
            {
                var oc = db.OFFICERCADET.Find(id);
                oc.SURNAME = data.Surname;
                oc.OTHERNAME = data.Othername;
                oc.GENDERID = data.GenderID;
                oc.DOB = data.DOB;
                oc.SERVICENUMBER = data.ServiceNumber;
                oc.RELIGIONID = data.ReligionID;
                oc.PHONENUMBER = data.PhoneNumber;
                oc.EMAILADDRESS = data.EmailAddress;
                oc.REGIONID = data.RegionID;
                oc.HOMETOWN = data.Hometown;
                oc.BLOODGROUPID = data.BloodGroupID;
                oc.RESADDRESS = data.ResAddress;
                oc.GENERALSTATUSID = data.GeneralStatusID;
                oc.RANKID = data.RankID;

                if (data.ImageUpload != null)
                {
                    var dataPic = db.OFFICERCADETIMAGE.Find(id);
                    if (dataPic != null)
                    {
                        HttpPostedFileBase imageUpload = data.ImageUpload;
                        byte[] picBytes = staffViewData.processImage(imageUpload);
                        var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeOfficerCadetImage(imageUpload, oc.OFFICERCADETID)));
                        var PicPath = id + ".jpg";
                        dataPic.PICTUREPATH = PicPath;
                        dataPic.OFFICERCADETID = id;
                        //var MIMEType = imageUpload.GetType();
                    }
                    else
                    {

                        var ocPic = new OFFICERCADETIMAGE();
                        HttpPostedFileBase imageUpload = data.ImageUpload;
                        byte[] picBytes = staffViewData.processImage(imageUpload);
                        var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeOfficerCadetImage(imageUpload, oc.OFFICERCADETID)));
                        var PicPath = id + ".jpg";
                        ocPic.PICTUREPATH = PicPath;
                        ocPic.OFFICERCADETID = id;

                        db.OFFICERCADETIMAGE.Add(ocPic);

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
            
            return Json(success ? JsonResponse.SuccessResponse("Officer Cadet") : JsonResponse.ErrorResponse(errorMessage));
        }



        public ActionResult EditSCEBank(int id)
        {
            var model = new JuniorCEBank();
            var jce = db.JUNIORCEBANK.Find(id);

            if (jce != null)
            {
                model.BankNameList = settingsViewData.getBankNames();
                //model.AccountNumber = rec.ACCOUNTNUMBER;
            }
            else
            {
                ViewBag.ErrorMessage = "Junior CE bank does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }

            return View("EditJCEBank", model);
        }

        [HttpPost]
        public ActionResult EditSCEBank(int id, JuniorCEBank data)
        {
            if (ModelState.IsValid)
            {
                var jce = db.JUNIORCEBANK.Find(id);
                jce.ACCOUNTNUMBER = data.AccountNumber;
                jce.BANKID = data.BankID;

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


            return Json(success ? JsonResponse.SuccessResponse("Senior CE Bank") : JsonResponse.ErrorResponse(errorMessage));
        }



        public ActionResult RemoveSCE()
        {
            return Json(success ? JsonResponse.SuccessResponse("Senior CE") : JsonResponse.ErrorResponse(errorMessage));
        }


        /*-----------------------------End Senior CE------------------------------------*/





    }
}