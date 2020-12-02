using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Dynamic;
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
        private PayrollViewData payViewData=new PayrollViewData();
        private int UniformAllowanceID = 1;
        private int RationAllowanceID = 2;
        private int MarketPremAllowanceID = 11;
        private int OperationAllowanceID = 12;
        private int DisabilityAllowanceID = 4;
        private int CEWelfareDeductionID = 6;
        private int TaxDeductionID = 5;
        private int SSNITDeductionID = 4;
        private int ProvidentFundDeductionID = 2;

        private int JuniorCadetLevStepID = 61;
        private int SeniorCadetLevStepID = 62;
        private int RecruitLevStepID = 2;

        private int RecruitRankID = 52;
       
        private int ArmyCadetRankID = 54;
        private int NavyCadetRankID = 58;
        private int AirForceCadetRankID = 55;
       
        private int ArmySeniorCadetRankID = 53;
        private int NavySeniorCadetRankID = 58;
        private int AirForceSeniorCadetRankID = 56;
        
        private int PresentGeneralStatusID = 1;
        private int DeletedGeneralStatusID = 3;

        private bool success = false;
        private string errorMessage = "";

        private DBGAFPAYEntities db = new DBGAFPAYEntities();
        // GET: Staff
        /*-----------------------------Start Recruit------------------------------------*/

        public ActionResult IndexRecruit()
        {
            List<Recruit> getCurrentRecruitList = staffViewData.GetCurrentRecruitsList();
            return View("IndexRecruit", getCurrentRecruitList);
        }

        public ActionResult CreateRecruit()
        {
            var model = new Recruit();
            model.GenderList = settingsViewData.getGenders();
            model.RegionList = settingsViewData.getRegion();
            model.ReligionList = settingsViewData.getReligion();
            model.BloodGroupList = settingsViewData.getBloodGroups();
            model.RankList = settingsViewData.getRecruitRanks();
            model.ServiceList = settingsViewData.getServices();
            model.GeneralStatusList = settingsViewData.getGeneralStatus();
            //model.RecruitCourseList = settingsViewData.getRecruitCourse();
            model.BankNameList = settingsViewData.getBankNames();

            return View("CreateRecruit", model);
        }
        [HttpPost]
        public ActionResult CreateRecruit(Recruit data)
        {
            if (ModelState.IsValid)
            {
                if (data.AccountNumber == null)
                {
                    errorMessage = "Account Number is required";
                    return Json(success ? JsonResponse.SuccessResponse("Recruit") : JsonResponse.ErrorResponse(errorMessage));
                }
                 
                var rec = new RECRUIT();
                
                rec.SURNAME = data.Surname;
                rec.OTHERNAME = data.Othername;
                rec.DOB = data.DOB;
                rec.PHONENUMBER = data.PhoneNumber;
                if (data.EmailAddress!=null)
                {   
                    rec.EMAILADDRESS = data.EmailAddress;
                }
                if (data.GhanaPostGPS!=null)
                {
                    rec.GHANAPOSTGPS = data.GhanaPostGPS;
                }
                
                rec.RESADDRESS = data.ResAddress;
                rec.SERVICENUMBER = data.ServiceNumber;
                rec.HOMETOWN = data.Hometown;
                rec.GENDERID = data.GenderID;
                rec.TCID = data.TrainingCenterID;
                rec.REGIONID = data.RegionID;
                rec.RELIGIONID = data.ReligionID;
                rec.RANKID = RecruitRankID;
                rec.BLOODGROUPID = data.BloodGroupID;
                //rec.INSERTEDBY = User.Identity.Name;
                rec.INSERTEDBY = "admin";
                rec.DATETIMEINSERTED = DateTime.Now;
                rec.RECRUITSTARTDATE = data.DateRecruitStart;
                rec.SERVICEID = data.ServiceID;
                rec.GENERALSTATUSID = PresentGeneralStatusID;
                rec.MILITARYLEVSTEPID = RecruitLevStepID;
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

                var recBank = new RECRUITBANK();
                var RecruitID = rec.RECRIUTID;
                recBank.ACCOUNTNUMBER = data.AccountNumber;
                recBank.BANKID = data.BankID;
                recBank.RECRIUTID = RecruitID;
                db.RECRUITBANK.Add(recBank);


                if (data.ImageUpload != null)
                {
                    var recPic = new RECRUITIMAGE();
                    HttpPostedFileBase imageUpload = data.ImageUpload;
                    byte[] picBytes = staffViewData.processImage(imageUpload);
                    var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeRecruitImage(imageUpload, rec.RECRIUTID)));
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
            var model = new Recruit();
            var rec = db.RECRUIT.Find(id);
            var recImage = db.RECRUITIMAGE.Find(id);
            if (rec != null)
            {
                model.Surname = rec.SURNAME;
                model.Othername = rec.OTHERNAME;
                model.GenderID = rec.GENDERID;
                model.DOB = rec.DOB;
                model.ServiceNumber = rec.SERVICENUMBER;
                model.PhoneNumber = rec.PHONENUMBER;
                model.ReligionID = rec.RELIGIONID;
                model.EmailAddress = rec.EMAILADDRESS;
                model.GhanaPostGPS = rec.GHANAPOSTGPS;
                model.RegionID = rec.REGIONID;
                model.Hometown = rec.HOMETOWN;
                model.ResAddress = rec.RESADDRESS;
                model.GeneralStatusID = rec.GENERALSTATUSID;
                model.BloodGroupID = rec.BLOODGROUPID;
                if (recImage != null)
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

            return View("EditRecruit", model);
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
                if (data.EmailAddress!=null)
                {
                    rec.EMAILADDRESS = data.EmailAddress;
                }

                if (data.GhanaPostGPS!=null)
                {
                    rec.GHANAPOSTGPS = data.GhanaPostGPS;
                }
                 
                if (data.ImageUpload != null)
                {
                    var dataPic = db.RECRUITIMAGE.Find(id);
                    if (dataPic != null)
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

                        var recPic = new RECRUITIMAGE();
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
            var model = new RecruitBank();
            if (rec != null)
            {
                model.BankNameList = settingsViewData.getBankNames();
                //model.AccountNumber = rec.ACCOUNTNUMBER;
            }
            else
            {
                ViewBag.ErrorMessage = "Recruit bank does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }


            return View("EditRecruitBank", model);
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


        public ActionResult RemoveRecruit(int id)
        {
            var rec = db.RECRUIT.Find(id);
            rec.GENERALSTATUSID = DeletedGeneralStatusID;
            errorMessage = "cannot delete";
            try
            {
               // db.SaveChanges();
                //success = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
              
            return Json(success ? JsonResponse.SuccessResponse("Recruit") : JsonResponse.ErrorResponse(errorMessage));
        }






        /*-----------------------------End Recruit------------------------------------*/

        /*-----------------------------Start Oficer Cadet------------------------------------*/

        public ActionResult IndexOfficerCadet()
        {
            List<OfficerCadet> getCurrentOCList = staffViewData.GetCurrentOfficerCadetList();
            return View("IndexOfficerCadet", getCurrentOCList);
        }

        public ActionResult CreateOfficerCadet()
        {
            var model = new OfficerCadet();
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
            return View("CreateOfficerCadet", model);
        }

        [HttpPost]

        public ActionResult CreateOfficerCadet(OfficerCadet data)
        {
            if (ModelState.IsValid)
            {

                if (data.AccountNumber == null)
                {
                    errorMessage = "Account Number is re quired";
                    return Json(success ? JsonResponse.SuccessResponse("Officer Cadet") : JsonResponse.ErrorResponse(errorMessage));
                }

                var oc = new OFFICERCADET();  
                if (data.ServiceID == 1)
                {
                    oc.RANKID = ArmyCadetRankID;
                }
                else if (data.ServiceID == 2)
                {
                    oc.RANKID = AirForceCadetRankID;

                }
                else
                {
                    oc.RANKID = NavyCadetRankID;
                }

                oc.SURNAME = data.Surname;
                oc.OTHERNAME = data.Othername;
                oc.GENDERID = data.GenderID;
                oc.SERVICENUMBER = data.ServiceNumber;
                oc.DOB = data.DOB;
                oc.RELIGIONID = data.ReligionID;
                oc.PHONENUMBER = data.PhoneNumber;
                if (data.EmailAddress!=null)
                {
                    oc.EMAILADDRESS = data.EmailAddress;
                }
                if (data.GhanaPostGPS!=null)
                {
                    oc.GHANAPOSTGPS = data.GhanaPostGPS;
                }
                oc.REGIONID = data.RegionID;
                oc.HOMETOWN = data.Hometown;
                oc.RESADDRESS = data.ResAddress;
                oc.OFFICERSTARTDATE = data.OfficerStartDate;
                oc.SERVICEID = data.ServiceID;
                oc.BLOODGROUPID = data.BloodGroupID;
                oc.OFFICERINTAKEID = data.OfficerIntakeID;
                oc.COMMISSIONTYPEID = data.CommissionTypeID;
                oc.GENERALSTATUSID = PresentGeneralStatusID;
                oc.MILITARYLEVSTEPID = JuniorCadetLevStepID;
                //oc.INSERTEDBY = User.Identity.Name;
                oc.INSERTEDBY = "admin";
                oc.DATETIMEINSERTED = DateTime.Now;


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
                var ocBank = new OFFICERCADETBANK();
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
                model.GhanaPostGPS = oc.GHANAPOSTGPS;
                model.RegionID = oc.REGIONID;
                model.Hometown = oc.HOMETOWN;
                model.ResAddress = oc.RESADDRESS;
                //model.GeneralStatusID = oc.GENERALSTATUSID;
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
        public ActionResult EditOfficerCadet(int id, OfficerCadet data)
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

                if (data.EmailAddress!=null)
                {
                    oc.EMAILADDRESS = data.EmailAddress;
                }
                if (data.GhanaPostGPS!=null)
                {
                    oc.GHANAPOSTGPS = data.GhanaPostGPS;
                }
                oc.REGIONID = data.RegionID;
                oc.HOMETOWN = data.Hometown;
                oc.BLOODGROUPID = data.BloodGroupID;
                oc.RESADDRESS = data.ResAddress;
               
                oc.RANKID = data.RankID;
                if (data.RankID == ArmySeniorCadetRankID || data.RankID == AirForceSeniorCadetRankID || data.RankID == NavySeniorCadetRankID)
                {
                    oc.MILITARYLEVSTEPID = SeniorCadetLevStepID;
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

            var model = new OfficerCadetBank();
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

            return View("EditOfficerCadetBank", model);
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



        public ActionResult RemoveOfficerCadet(int id)
        {
            var oc = db.OFFICERCADET.Find(id);
            oc.GENERALSTATUSID = DeletedGeneralStatusID;

            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return Json(success ? JsonResponse.SuccessResponse("Officer Cadet") : JsonResponse.ErrorResponse(errorMessage));
        }


        /*-----------------------------End Oficer Cadet------------------------------------*/




        /*-----------------------------Start Partial View content------------------------------------*/

        public ActionResult _MEDPCodeBox()
        {
            var medP = new JuniorCE();
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
            //model.IsMedical = 0;
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
            model.GradeList = settingsViewData.getGradeX(0);


            return View("CreateJCE", model);
        }


        [HttpPost]
        public ActionResult CreateJCE(JuniorCE data)
        {
            if (ModelState.IsValid)
            {
                //if (data.IsMedical == 1 && data.MedPCode == null)
                //{
                //    errorMessage = "MED P Code is required";
                //    return Json(success ? JsonResponse.SuccessResponse("Junior CE") : JsonResponse.ErrorResponse(errorMessage));
                //}

                if (data.AccountNumber == null)
                {
                    errorMessage = "Account Number is required";
                    return Json(success ? JsonResponse.SuccessResponse("Junior CE") : JsonResponse.ErrorResponse(errorMessage));
                }


                var jce = new JUNIORCE();  
                jce.SURNAME = data.Surname;
                jce.OTHERNAME = data.Othername;
                jce.GENDERID = data.GenderID;
                jce.SERVICENUMBER = data.ServiceNumber;
                jce.DOB = data.DOB;
                jce.RELIGIONID = data.ReligionID;
                jce.PHONENUMBER = data.PhoneNumber;
                if (data.EmailAddress != null)
                {
                    jce.EMAILADDRESS = data.EmailAddress;
                }
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
                jce.GENERALSTATUSID = PresentGeneralStatusID;
                jce.CIVILIANLEVSTEPID = data.CLevStepID;
                jce.TITLEID = data.TitleID;
                jce.GRADEID = data.GradeID;
                if (data.GhanaPostGPS != null)
                {
                    jce.GHANAPOSTGPS = data.GhanaPostGPS.ToUpper();
                } 
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
                var levstepID = data.CLevStepID;
                var civilLevStep = db.CIVILIANLEVSTEP.FirstOrDefault(a => a.CIVILIANLEVSTEPID == levstepID);
                var constPay = civilLevStep.CONSTPAY;
                
                var clothingAmount = payViewData.calculateCivilUniformAllowance(); 
                 
                var rationAmount = payViewData.calculateCivilRationAllowance();
                  
                
                if (data.IsMedical == 1)
                { 
                    var operationAmount = payViewData.calculateOperationAllowance(constPay); 
                    var marketPremAmount = payViewData.calculateMarketPremium(constPay, data.GradeID);

                    var marketPremAllow = new JUNIORCEALLOWANCE(); 
                    marketPremAllow.ALLOWANCEID = MarketPremAllowanceID;
                    marketPremAllow.AMOUNT = marketPremAmount;
                    marketPremAllow.STATUS = 1;
                    marketPremAllow.JUNIORCEID = jceID;
                    marketPremAllow.ID = Guid.NewGuid();
                    marketPremAllow.DATETIMEINSERTED=DateTime.Now;
                    marketPremAllow.INSERTEDBY = "admin";
                    //marketPremAllow.INSERTEDBY = User.Identity.Name;

                    db.JUNIORCEALLOWANCE.Add(marketPremAllow);

                    var operationAllow = new JUNIORCEALLOWANCE(); 
                    operationAllow.ALLOWANCEID = OperationAllowanceID;
                    operationAllow.AMOUNT = operationAmount;
                    operationAllow.STATUS = 1;
                    operationAllow.JUNIORCEID = jceID;
                    operationAllow.ID = Guid.NewGuid();
                    operationAllow.DATETIMEINSERTED = DateTime.Now;
                    operationAllow.INSERTEDBY = "admin";
                    //operationAllow.INSERTEDBY = User.Identity.Name; 

                    db.JUNIORCEALLOWANCE.Add(operationAllow); 
                }
                 
                var allowances = db.ALLOWANCE.Where(a => a.STATUS == 2).ToList();
                foreach (var item in allowances)
                {
                    var jceallow = new JUNIORCEALLOWANCE(); 
                    if (item.ALLOWANCEID == UniformAllowanceID)
                    {
                        jceallow.AMOUNT = clothingAmount;
                    }
                    else if (item.ALLOWANCEID == RationAllowanceID)
                    {
                        jceallow.AMOUNT = rationAmount;
                    }
                    jceallow.STATUS = 1;
                    jceallow.ID = Guid.NewGuid();
                    jceallow.ALLOWANCEID = item.ALLOWANCEID;
                    jceallow.JUNIORCEID = jceID;
                    jceallow.DATETIMEINSERTED = DateTime.Now;
                    jceallow.INSERTEDBY = "admin";
                    //jceallow.INSERTEDBY = User.Identity.Name;

                    db.JUNIORCEALLOWANCE.Add(jceallow);

                }

                if (data.IsDisabled == 1)
                {
                    var disabilityAmount = payViewData.calculateDisabilityAllowance(constPay);
                    var disableAllow = new JUNIORCEALLOWANCE(); 
                    disableAllow.ALLOWANCEID = DisabilityAllowanceID;
                    disableAllow.AMOUNT = disabilityAmount;
                    disableAllow.STATUS = 1;
                    disableAllow.JUNIORCEID = jceID;
                    disableAllow.ID = Guid.NewGuid();
                    disableAllow.DATETIMEINSERTED = DateTime.Now;
                    disableAllow.INSERTEDBY = "admin";
                    //disableAllow.INSERTEDBY = User.Identity.Name;

                    db.JUNIORCEALLOWANCE.Add(disableAllow);
                }

                //Deductions insertion
                 
                var CEWelfareAmount = payViewData.calculateCivilWelfare();
                var providentAmount = payViewData.calculateProvidentFund(constPay, data.ProvidentID);
                var tax = payViewData.calculateIncomeTax(constPay, providentAmount);
                
                var providentID = data.ProvidentID;
                 
                if (providentID != 1)
                {
                    var provDed = new JUNIORCEDEDUCTION();  
                    provDed.DEDUCTIONAMOUNT = providentAmount;
                    provDed.DEDUCTIONID = ProvidentFundDeductionID;
                    provDed.JUNIORCEID = jceID;
                    provDed.STATUS = 1;
                    provDed.ID = Guid.NewGuid();
                    provDed.INSERTEDBY = "admin";
                    //provDed.INSERTEDBY = User.Identity.Name;
                    provDed.DATETIMEINSERTED = DateTime.Now;

                    db.JUNIORCEDEDUCTION.Add(provDed);
                }
                 
                var ssnitAmount = payViewData.calculateSSNIT(constPay); 

                var deductions = db.DEDUCTION.Where(a => a.STATUS == 2).ToList();
                foreach (var items in deductions)
                {
                    var jcededuc = new JUNIORCEDEDUCTION();
                    if (items.DEDUCTIONID == CEWelfareDeductionID)
                    {
                        jcededuc.DEDUCTIONAMOUNT = CEWelfareAmount; 
                    }
                    else if (items.DEDUCTIONID == SSNITDeductionID)
                    {
                        jcededuc.DEDUCTIONAMOUNT = ssnitAmount;
                    }
                    else if (items.DEDUCTIONID == TaxDeductionID)
                    {
                        jcededuc.DEDUCTIONAMOUNT = tax;
                    }

                    jcededuc.STATUS = 1;
                    jcededuc.DEDUCTIONID = items.DEDUCTIONID;
                    jcededuc.JUNIORCEID = jceID;
                    jcededuc.ID= Guid.NewGuid();
                    jcededuc.INSERTEDBY = "admin";
                    //jcededuc.INSERTEDBY = User.Identity.Name;
                    jcededuc.DATETIMEINSERTED = DateTime.Now; 

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

            var jce = db.JUNIORCE.Find(id);
            var model = new JuniorCE();
            var jcePic = db.JUNIORCEIMAGE.Find(id);
            if (jce != null)
            {
                model.TitleID = jce.TITLEID;
                model.Surname = jce.SURNAME;
                model.Othername = jce.OTHERNAME;
                model.GenderID = jce.GENDERID;
                model.DOB = jce.DOB;
                model.ServiceNumber = jce.SERVICENUMBER;
                model.PhoneNumber = jce.PHONENUMBER;
                model.ReligionID = jce.RELIGIONID;
                model.EmailAddress = jce.EMAILADDRESS;
                model.RegionID = jce.REGIONID;
                model.Hometown = jce.HOMETOWN;
                model.ResAddress = jce.RESADDRESS;
                model.GeneralStatusID = jce.GENERALSTATUSID;
                model.DateEmployed = jce.DATEEMPLOYED;
                model.UnitID = jce.UNITID;
                model.SSNITNo = jce.SSNITNUMBER;
                model.BloodGroupID = jce.BLOODGROUPID;
                model.GhanaPostGPS = jce.GHANAPOSTGPS;

                if (jcePic != null)
                {
                    model.ImageName = jcePic.PICTUREPATH;
                }

            }
            else
            {
                ViewBag.ErrorMessage = "Junior CE does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            model.TitleList = settingsViewData.getTitles();
            model.IsMedicalList = settingsViewData.getYesNo();
            model.UnitList = settingsViewData.getUnits();
            model.GeneralStatusList = settingsViewData.getGeneralStatus();
            model.GenderList = settingsViewData.getGenders();
            model.ReligionList = settingsViewData.getReligion();
            model.RegionList = settingsViewData.getRegion();
            model.BloodGroupList = settingsViewData.getBloodGroups();

            return View("EditJCE", model);
        }

        [HttpPost]
        public ActionResult EditJCE(int id, JuniorCE data)
        {
            if (ModelState.IsValid)
            {
                var jce = db.JUNIORCE.Find(id);
                jce.TITLEID = data.TitleID;
                jce.SURNAME = data.Surname;
                jce.OTHERNAME = data.Othername;
                jce.GENDERID = data.GenderID;
                jce.DOB = data.DOB;
                jce.SERVICENUMBER = data.ServiceNumber;
                jce.RELIGIONID = data.ReligionID;
                jce.PHONENUMBER = data.PhoneNumber;
                if (data.EmailAddress != null)
                {
                    jce.EMAILADDRESS = data.EmailAddress;
                }
                if (data.GhanaPostGPS != null)
                {
                    jce.GHANAPOSTGPS = data.GhanaPostGPS.ToUpper();
                }
                jce.REGIONID = data.RegionID;
                jce.HOMETOWN = data.Hometown;
                jce.BLOODGROUPID = data.BloodGroupID;
                jce.RESADDRESS = data.ResAddress;
                jce.GENERALSTATUSID = data.GeneralStatusID;
                jce.DATEEMPLOYED = data.DateEmployed;
                jce.UNITID = data.UnitID;
                jce.SSNITNUMBER = data.SSNITNo;

                if (data.ImageUpload != null)
                {
                    var dataPic = db.JUNIORCEIMAGE.Find(id);
                    if (dataPic != null)
                    {
                        HttpPostedFileBase imageUpload = data.ImageUpload;
                        byte[] picBytes = staffViewData.processImage(imageUpload);
                        var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeJCEImage(imageUpload, jce.JUNIORCEID)));
                        var PicPath = id + ".jpg";
                        dataPic.PICTUREPATH = PicPath;
                        dataPic.JUNIORCEID = id;
                        //var MIMEType = imageUpload.GetType();
                    }
                    else
                    {

                        var jcePic = new JUNIORCEIMAGE();
                        HttpPostedFileBase imageUpload = data.ImageUpload;
                        byte[] picBytes = staffViewData.processImage(imageUpload);
                        var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeJCEImage(imageUpload, jce.JUNIORCEID)));
                        var PicPath = id + ".jpg";
                        jcePic.PICTUREPATH = PicPath;
                        jcePic.JUNIORCEID = id;

                        db.JUNIORCEIMAGE.Add(jcePic);

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

            return Json(success ? JsonResponse.SuccessResponse("Junior CE") : JsonResponse.ErrorResponse(errorMessage));
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

        public ActionResult DetailsJCE(int id)
        {
            var model = new JuniorCEDetails();
            var jce = db.JUNIORCE.Find(id);
            var jceBank = db.JUNIORCEBANK.Find(id);
            model.Surname = jce.SURNAME;
            model.Othername = jce.OTHERNAME;
            model.ServiceNumber = jce.SERVICENUMBER;
            model.TitleName = jce.TITLE.TITLENAME;
            model.UnitName = jce.UNIT.UNITNAME;
            model.GenderName = jce.GENDER.GENDERNAME;
            model.GeneralStatusName = jce.GENERALSTATUS.GSNAME;
            model.ConstPay = jce.CIVILIANLEVSTEP.CONSTPAY;
            model.CLevStepName = jce.CIVILIANLEVSTEP.LEVSTEPNAME;
            model.SSNITNo = jce.SSNITNUMBER;
            model.BankBranch = jceBank.BANK.BANKBRANCH;
            model.BankName = jceBank.BANK.BANKNAME.BANKNAMEX;
            model.ConstPay = jce.CIVILIANLEVSTEP.CONSTPAY;
            model.AccountNumber = jceBank.ACCOUNTNUMBER;
            model.GradeName = jce.GRADE.GRADENAME;
            model.DateEmployed = jce.DATEEMPLOYED;
            model.GenderID = jce.GENDERID;
             


            var jceAllow = new JuniorCEAllowance();
            var JCEA = db.JUNIORCEALLOWANCE.Where(a => a.JUNIORCEID == id && a.STATUS == 1);
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

            var jceDeduc = new JuniorCEDeduction2();
            var JCED = db.JUNIORCEDEDUCTION.Where(a => a.JUNIORCEID == id && a.STATUS == 1 ||a.STATUS==2);
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
            model.NetPay = (model.ConstPay + model.TotalAllow - model.TotalDeduc);


            return View("DetailsJCE", model);
        }




        public ActionResult RemoveJCE(int id)
        {
            var jce = db.JUNIORCE.Find(id);
            jce.GENERALSTATUSID = DeletedGeneralStatusID;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

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
            model.GenderList = settingsViewData.getGenders();
            model.RegionList = settingsViewData.getRegion();
            model.ReligionList = settingsViewData.getReligion();
            model.BloodGroupList = settingsViewData.getBloodGroups();
            model.RankList = settingsViewData.getRecruitRanks();
            model.TitleList = settingsViewData.getTitles();
            model.GeneralStatusList = settingsViewData.getGeneralStatus();
            model.BankNameList = settingsViewData.getBankNames();
            model.CLevStepList = settingsViewData.getSCELevStep();
            model.UnitList = settingsViewData.getUnits();
            model.IsMedicalList = settingsViewData.getYesNo();
            model.IsDisabledList = settingsViewData.getYesNo();
            model.ProvidentFundList = settingsViewData.getProvidentFund();
            model.GradeList = settingsViewData.getGradeX(0);

            return View("CreateSCE", model);
        }


        [HttpPost]
        public ActionResult CreateSCE(SeniorCE data)
        {
            if (ModelState.IsValid)
            {

                if (data.AccountNumber == null)
                {
                    errorMessage = "Account Number is required";
                    return Json(success ? JsonResponse.SuccessResponse("Senior CE") : JsonResponse.ErrorResponse(errorMessage));
                }

                var sce = new SENIORCE(); 
                sce.SURNAME = data.Surname;
                sce.OTHERNAME = data.Othername;
                sce.GENDERID = data.GenderID;
                sce.SERVICENUMBER = data.ServiceNumber;
                sce.DOB = data.DOB;
                sce.RELIGIONID = data.ReligionID;
                sce.PHONENUMBER = data.PhoneNumber;
                if (data.EmailAddress != null)
                {
                    sce.EMAILADDRESS = data.EmailAddress;
                }
                if (data.GhanaPostGPS != null)
                {
                    sce.GHANAPOSTGPS = data.GhanaPostGPS;
                }
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
                sce.GENERALSTATUSID = PresentGeneralStatusID;
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
                //getting the const pay
                var levstepID = data.CLevStepID;
                var civilLevStep = db.CIVILIANLEVSTEP.FirstOrDefault(a => a.CIVILIANLEVSTEPID == levstepID);
                var constPay = civilLevStep.CONSTPAY;


                //Allowances insertion   
                var operationAmount = payViewData.calculateOperationAllowance(constPay); 
                var disabilityAmount = payViewData.calculateDisabilityAllowance(constPay);
                var uniformAmount = payViewData.calculateCivilUniformAllowance();
                var rationAmount = payViewData.calculateCivilRationAllowance();
                var providentAmount = payViewData.calculateProvidentFund(constPay, data.ProvidentID);
                var CEWelfareAmount = payViewData.calculateCivilWelfare();
                var SSNITAmount = payViewData.calculateSSNIT(constPay);
                var taxAmount = payViewData.calculateIncomeTax(constPay, providentAmount);


                if (data.IsMedical == 1)
                {
                    var gradeID = data.GradeID; 
                    var marketPremAmount = payViewData.calculateMarketPremium(constPay,gradeID); 
                    var marketPremAllow = new SENIORCEALLOWANCE();
                    marketPremAllow.ALLOWANCEID = MarketPremAllowanceID;
                    marketPremAllow.AMOUNT = marketPremAmount;
                    marketPremAllow.STATUS = 1;
                    marketPremAllow.SENIORCEID = sceID;
                    marketPremAllow.ID = Guid.NewGuid();
                    marketPremAllow.INSERTEDBY = "admin";
                    //marketPremAllow.INSERTEDBY = User.Identity.Name;
                    marketPremAllow.DATETIMEINSERTED=DateTime.Now;

                    db.SENIORCEALLOWANCE.Add(marketPremAllow);

                    var operationAllow = new SENIORCEALLOWANCE();
                    operationAllow.ALLOWANCEID = OperationAllowanceID;
                    operationAllow.AMOUNT = operationAmount;
                    operationAllow.STATUS = 1;
                    operationAllow.SENIORCEID = sceID;
                    operationAllow.ID=Guid.NewGuid();
                    operationAllow.DATETIMEINSERTED=DateTime.Now;
                    operationAllow.INSERTEDBY = "admin";
                    //operationAllow.INSERTEDBY = User.Identity.Name;


                    db.SENIORCEALLOWANCE.Add(operationAllow);


                } 
                var allowances = db.ALLOWANCE.Where(a => a.STATUS == 2).ToList();
                foreach (var item in allowances)
                {
                    var sceallow = new SENIORCEALLOWANCE();
                    if (item.ALLOWANCEID == UniformAllowanceID)
                    {
                        sceallow.AMOUNT = uniformAmount;
                    }
                    else if (item.ALLOWANCEID == RationAllowanceID)
                    {
                        sceallow.AMOUNT = rationAmount;
                    }
                    sceallow.STATUS = 1;
                    sceallow.ALLOWANCEID = item.ALLOWANCEID;
                    sceallow.SENIORCEID = sceID;
                    sceallow.ID=Guid.NewGuid();
                    sceallow.INSERTEDBY = "admin";
                    //sceallow.INSERTEDBY = User.Identity.Name;
                    sceallow.DATETIMEINSERTED=DateTime.Now;

                    db.SENIORCEALLOWANCE.Add(sceallow);

                }

                if (data.IsDisabled == 1)
                {
                    var disableAllow = new SENIORCEALLOWANCE();
                    disableAllow.ALLOWANCEID = DisabilityAllowanceID;
                    disableAllow.AMOUNT = disabilityAmount;
                    disableAllow.STATUS = 1;
                    disableAllow.SENIORCEID = sceID;
                    disableAllow.ID=Guid.NewGuid();
                    disableAllow.DATETIMEINSERTED=DateTime.Now;
                    disableAllow.INSERTEDBY = "admin";
                    //disableAllow.INSERTEDBY = User.Identity.Name;

                    db.SENIORCEALLOWANCE.Add(disableAllow);
                }
                 
                //Deductions insertion 
                var providentID = data.ProvidentID;
                
                if (providentID != 1)
                {
                    var provDed = new SENIORCEDEDUCTION(); 
                    provDed.DEDUCTIONAMOUNT = providentAmount;
                    provDed.DEDUCTIONID = ProvidentFundDeductionID;
                    provDed.SENIORCEID = sceID;
                    provDed.STATUS = 1;
                    provDed.ID = Guid.NewGuid();
                    provDed.DATETIMEINSERTED=DateTime.Now;
                    provDed.INSERTEDBY = "admin";
                    //provDed.INSERTEDBY = User.Identity.Name;

                    db.SENIORCEDEDUCTION.Add(provDed);
                }
                 
                var deductions = db.DEDUCTION.Where(a => a.STATUS == 2).ToList();
                foreach (var items in deductions)
                {
                    var scededuc = new SENIORCEDEDUCTION();
                    if (items.DEDUCTIONID == CEWelfareDeductionID)
                    {
                        scededuc.DEDUCTIONAMOUNT = CEWelfareAmount ;

                    }
                    else if (items.DEDUCTIONID == SSNITDeductionID)
                    {
                        scededuc.DEDUCTIONAMOUNT = SSNITAmount;
                    }
                    else if (items.DEDUCTIONID == TaxDeductionID)
                    {
                        scededuc.DEDUCTIONAMOUNT = taxAmount;
                    }

                    scededuc.STATUS = 1;
                    scededuc.DEDUCTIONID = items.DEDUCTIONID;
                    scededuc.SENIORCEID = sceID;
                    scededuc.INSERTEDBY = "admin";
                    //scededuc.INSERTEDBY = User.Identity.Name;
                    scededuc.ID=Guid.NewGuid();

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

            var sce = db.SENIORCE.Find(id);
            var model = new SeniorCE();
            var scePic = db.SENIORCEIMAGE.Find(id);
            if (sce != null)
            {
                model.Surname = sce.SURNAME;
                model.Othername = sce.OTHERNAME;
                model.GenderID = sce.GENDERID;
                model.DOB = sce.DOB;
                model.TitleID = sce.TITLEID;
                model.ServiceNumber = sce.SERVICENUMBER;
                model.PhoneNumber = sce.PHONENUMBER;
                model.ReligionID = sce.RELIGIONID;
                model.EmailAddress = sce.EMAILADDRESS;
                model.RegionID = sce.REGIONID;
                model.Hometown = sce.HOMETOWN;
                model.ResAddress = sce.RESADDRESS;
                model.GeneralStatusID = sce.GENERALSTATUSID;
                model.BloodGroupID = sce.BLOODGROUPID;
                model.UnitID = sce.UNITID;
                model.DateEmployed = sce.DATEEMPLOYED;
                model.GradeID = sce.GRADEID;
                model.CLevStepID = sce.CIVILIANLEVSTEPID;
                model.SSNITNo = sce.SSNITNUMBER;
                model.GhanaPostGPS = sce.GHANAPOSTGPS;

                if (scePic != null)
                {
                    model.ImageName = scePic.PICTUREPATH;
                }


            }
            else
            {
                ViewBag.ErrorMessage = "Senior CE does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }

            model.TitleList = settingsViewData.getTitles();
            model.GeneralStatusList = settingsViewData.getGeneralStatus();
            model.GenderList = settingsViewData.getGenders();
            model.ReligionList = settingsViewData.getReligion();
            model.RegionList = settingsViewData.getRegion();
            model.BloodGroupList = settingsViewData.getBloodGroups();
            model.UnitList = settingsViewData.getUnits();
            model.IsMedicalList = settingsViewData.getYesNo();
            model.GeneralStatusList = settingsViewData.getGeneralStatus();


            return View("EditSCE", model);
        }

        [HttpPost]
        public ActionResult EditSCE(int id, SeniorCE data)
        {
            if (ModelState.IsValid)
            {
                var sce = db.SENIORCE.Find(id);
                sce.TITLEID = data.TitleID;
                sce.SURNAME = data.Surname;
                sce.OTHERNAME = data.Othername;
                sce.GENDERID = data.GenderID;
                sce.DOB = data.DOB;
                sce.SERVICENUMBER = data.ServiceNumber;
                sce.RELIGIONID = data.ReligionID;
                sce.PHONENUMBER = data.PhoneNumber;
                sce.EMAILADDRESS = data.EmailAddress;
                sce.REGIONID = data.RegionID;
                sce.HOMETOWN = data.Hometown;
                sce.BLOODGROUPID = data.BloodGroupID;
                sce.RESADDRESS = data.ResAddress;
                sce.GENERALSTATUSID = data.GeneralStatusID;
                sce.DATEEMPLOYED = data.DateEmployed;
                sce.UNITID = data.UnitID;
                sce.SSNITNUMBER = data.SSNITNo;
                if (data.EmailAddress != null)
                {
                    sce.EMAILADDRESS = data.EmailAddress;
                }
                if (data.GhanaPostGPS != null)
                {
                    sce.GHANAPOSTGPS = data.GhanaPostGPS;
                }



                if (data.ImageUpload != null)
                {
                    var dataPic = db.SENIORCEIMAGE.Find(id);
                    if (dataPic != null)
                    {
                        HttpPostedFileBase imageUpload = data.ImageUpload;
                        byte[] picBytes = staffViewData.processImage(imageUpload);
                        var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeSCEImage(imageUpload, sce.SENIORCEID)));
                        var PicPath = id + ".jpg";
                        dataPic.PICTUREPATH = PicPath;
                        dataPic.SENIORCEID = id;
                        //var MIMEType = imageUpload.GetType();
                    }
                    else
                    {
                        var scePic = new SENIORCEIMAGE();
                        HttpPostedFileBase imageUpload = data.ImageUpload;
                        byte[] picBytes = staffViewData.processImage(imageUpload);
                        var picUrl = new Uri(Request.Url, Url.Content(staffViewData.storeSCEImage(imageUpload, sce.SENIORCEID)));
                        var PicPath = id + ".jpg";
                        scePic.PICTUREPATH = PicPath;
                        scePic.SENIORCEID = id;

                        db.SENIORCEIMAGE.Add(scePic);
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

            return Json(success ? JsonResponse.SuccessResponse("Senior CE") : JsonResponse.ErrorResponse(errorMessage));
        }


        public ActionResult DetailsSCE(int id)
        {
            var model = new SeniorCEDetails();
            var sce = db.SENIORCE.Find(id);
            var sceBank = db.SENIORCEBANK.Find(id);
            model.Surname = sce.SURNAME;
            model.Othername = sce.OTHERNAME;
            model.ServiceNumber = sce.SERVICENUMBER;
            model.TitleName = sce.TITLE.TITLENAME;
            model.UnitName = sce.UNIT.UNITNAME;
            model.GenderName = sce.GENDER.GENDERNAME;
            model.GeneralStatusName = sce.GENERALSTATUS.GSNAME;
            model.ConstPay = sce.CIVILIANLEVSTEP.CONSTPAY;
            model.CLevStepName = sce.CIVILIANLEVSTEP.LEVSTEPNAME;
            model.SSNITNo = sce.SSNITNUMBER;
            model.BankBranch = sceBank.BANK.BANKBRANCH;
            model.BankName = sceBank.BANK.BANKNAME.BANKNAMEX;
            model.ConstPay = sce.CIVILIANLEVSTEP.CONSTPAY;
            model.AccountNumber = sceBank.ACCOUNTNUMBER;
            model.GradeName = sce.GRADE.GRADENAME;
            model.DateEmployed = sce.DATEEMPLOYED;
            model.GenderID = sce.GENDERID;





            var SCEA = db.SENIORCEALLOWANCE.Where(a => a.SENIORCEID == id && a.STATUS == 1);
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


            var SCED = db.SENIORCEDEDUCTION.Where(a => a.SENIORCEID == id && a.STATUS == 1);
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
            model.NetPay = (model.ConstPay + model.TotalAllow - model.TotalDeduc);

            return View("DetailsSCE", model);
        }
        public ActionResult EditSCEBank(int id)
        {
            var model = new SeniorCEBank();
            var sce = db.SENIORCEBANK.Find(id);

            if (sce != null)
            {
                model.BankNameList = settingsViewData.getBankNames();
                //model.AccountNumber = rec.ACCOUNTNUMBER;
            }
            else
            {
                ViewBag.ErrorMessage = "Senior CE bank does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }

            return View("EditSCEBank", model);
        }

        [HttpPost]
        public ActionResult EditSCEBank(int id, SeniorCEBank data)
        {
            if (ModelState.IsValid)
            {
                var sce = db.SENIORCEBANK.Find(id);
                sce.ACCOUNTNUMBER = data.AccountNumber;
                sce.BANKID = data.BankID;

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



        public ActionResult RemoveSCE(int id)
        {

            var sce = db.SENIORCE.Find(id);
            sce.GENERALSTATUSID = DeletedGeneralStatusID;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return Json(success ? JsonResponse.SuccessResponse("Senior CE") : JsonResponse.ErrorResponse(errorMessage));
        }


        /*-----------------------------End Senior CE------------------------------------*/



        /*-----------------------------------------Start Edit Junior CE Provident --------------------------------------   */

        public ActionResult IndexJCEProvident()
        {
            List<JuniorCE> getCurrentJCEList = staffViewData.GetCurrentJuniorCEList();
            return View("IndexJCEProvident", getCurrentJCEList);

        }

        public ActionResult JCEProvident(int id)
        {

            var jce = db.JUNIORCE.Find(id);
            var model = new JuniorCEProvident();
            model.ProvidentID = jce.PROVIDENTFUNDID;
            model.ProvidentFundList = settingsViewData.getProvidentFund();

            ViewBag.Name = jce.TITLE.TITLENAME + " " + jce.SURNAME + " " + jce.OTHERNAME;
            return View("JCEProvident", model);
        }
        [HttpPost]
        public ActionResult JCEProvident(int id, JuniorCEProvident data)
        {
            if (ModelState.IsValid)
            {
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
            return Json(success ? JsonResponse.SuccessResponse("Junior CE Provident Fund") : JsonResponse.ErrorResponse(errorMessage));

        }



        /*-----------------------------------------End Edit Junior CE Provident --------------------------------------   */
        /*-----------------------------------------Start Junior CE Promotion --------------------------------------   */

        public ActionResult IndexJCEPromotion()
        {
            List<JuniorCE> getCurrentJCEList = staffViewData.GetCurrentJuniorCEList();
            return View("IndexJCEPromotion", getCurrentJCEList);

        }

        public ActionResult JCEPromotion(int id)
        {

            var jce = db.JUNIORCE.Find(id);
            var model = new JuniorCEPromotion();
            model.GradeID = jce.GRADEID;
            model.IsMedical = Convert.ToInt16(jce.ISMEDICAL);
            model.CLevStepID = jce.CIVILIANLEVSTEPID;
            model.DatePromoted = jce.DATEPROMOTED;

            if (jce.ISMEDICAL)
            {
                model.GradeList = settingsViewData.getGradeX(1);
            }
            else
            {
                model.GradeList = settingsViewData.getGradeX(0);
            }
            model.CLevStepList = settingsViewData.getJCELevStep();

            ViewBag.Name = jce.TITLE.TITLENAME + " " + jce.SURNAME + " " + jce.OTHERNAME;
            return View("JCEPromotion", model);
        }
        [HttpPost]
        public ActionResult JCEPromotion(int id, JuniorCEPromotion data)
        {
            if (ModelState.IsValid)
            {
                var jce = db.JUNIORCE.Find(id); 
                var levstepID = data.CLevStepID;
                var civilLevStep = db.CIVILIANLEVSTEP.FirstOrDefault(a => a.CIVILIANLEVSTEPID == levstepID);
                var constPay = civilLevStep.CONSTPAY;
                jce.DATEPROMOTED = data.DatePromoted;
                jce.CIVILIANLEVSTEPID = data.CLevStepID;
                jce.GRADEID = data.GradeID; 
                
                if (jce.ISMEDICAL)
                { 
                    var gradeID = data.GradeID;  
                    var marketPremAmount = payViewData.calculateMarketPremium(constPay, gradeID);
                    var operationAmount = payViewData.calculateOperationAllowance(constPay);


                    var marketPremAllow = db.JUNIORCEALLOWANCE.FirstOrDefault(a => a.ALLOWANCEID == MarketPremAllowanceID && a.STATUS == 1 && a.JUNIORCEID==id);
                    marketPremAllow.AMOUNT = marketPremAmount;
                    marketPremAllow.UPDATEDBY = "admin";
                    //marketPremAllow.UPDATEDBY = User.Identity.Name;
                    marketPremAllow.DATETIMEUPDATED = DateTime.Now;

                    var operationAllow =db.JUNIORCEALLOWANCE.FirstOrDefault(a => a.ALLOWANCEID == OperationAllowanceID && a.STATUS == 1 && a.JUNIORCEID == id);
                    operationAllow.AMOUNT = operationAmount;
                    operationAllow.UPDATEDBY = "admin";
                    //operationAllow.UPDATEDBY = User.Identity.Name;
                    operationAllow.DATETIMEUPDATED=DateTime.Now;


                }

                if (jce.ISDISABLED)
                { 
                    var disabilityAmount = payViewData.calculateDisabilityAllowance(constPay);
                    var disableAllow =  db.JUNIORCEALLOWANCE.FirstOrDefault(a => a.ALLOWANCEID == DisabilityAllowanceID && a.STATUS == 1 && a.JUNIORCEID == id);

                    disableAllow.AMOUNT = disabilityAmount;
                    disableAllow.UPDATEDBY = "admin";
                    //disableAllow.UPDATEDBY = User.Identity.Name;
                    disableAllow.DATETIMEUPDATED = DateTime.Now;
                }
                var jceProvID = jce.PROVIDENTFUNDID; 
                var providentAmount = payViewData.calculateProvidentFund(constPay, jceProvID);
                if (jce.PROVIDENTFUNDID != 1)
                {
                    var provDed =
                        db.JUNIORCEDEDUCTION.FirstOrDefault(
                            a => a.DEDUCTIONID == ProvidentFundDeductionID && a.STATUS == 1 && a.JUNIORCEID == id); 

                    provDed.DEDUCTIONAMOUNT = providentAmount;
                    provDed.DATETIMEUPDATED = DateTime.Now;
                    provDed.UPDATEDBY = "admin";
                    //provDed.UPDATEDBY = User.Identity.Name;

                } 
                var ssnitAmount = payViewData.calculateSSNIT(constPay);
                var tax = payViewData.calculateIncomeTax(constPay, providentAmount);
                
                var jceSSNIT = db.JUNIORCEDEDUCTION.FirstOrDefault(a => a.DEDUCTIONID == SSNITDeductionID && a.STATUS == 1 && a.JUNIORCEID == id); 
                jceSSNIT.DEDUCTIONAMOUNT = ssnitAmount;
                jceSSNIT.UPDATEDBY = "admin";
                //jceSSNIT.UPDATEDBY = User.Identity.Name;
                jceSSNIT.DATETIMEUPDATED = DateTime.Now;

                var jceTax = db.JUNIORCEDEDUCTION.FirstOrDefault(a => a.DEDUCTIONID == TaxDeductionID && a.STATUS == 1 & a.JUNIORCEID == id); 
                jceTax.DEDUCTIONAMOUNT = tax; 
                jceTax.DATETIMEUPDATED=DateTime.Now;
                jceTax.UPDATEDBY = "admin";
                //jceTax.UPDATEDBY = User.Identity.Name;

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
            return Json(success ? JsonResponse.SuccessResponse("Junior CE Promotion") : JsonResponse.ErrorResponse(errorMessage));
        }



        /*-----------------------------------------End Junior CE Promotion --------------------------------------   */

        /*-----------------------------------------Start Edit Senior CE Provident --------------------------------------   */

        public ActionResult IndexSCEProvident()
        {
            List<SeniorCE> getCurrentSCEList = staffViewData.GetCurrentSeniorCEList();
            return View("IndexSCEProvident", getCurrentSCEList);

        }

        public ActionResult SCEProvident(int id)
        {

            var sce = db.SENIORCE.Find(id);
            var model = new SeniorCEProvident();
            model.ProvidentID = sce.PROVIDENTFUNDID;
            model.ProvidentFundList = settingsViewData.getProvidentFund();

            ViewBag.Name = sce.TITLE.TITLENAME + " " + sce.SURNAME + " " + sce.OTHERNAME;
            return View("SCEProvident", model);
        }
        [HttpPost]
        public ActionResult SCEProvident(int id, SeniorCEProvident data)
        {
            if (ModelState.IsValid)
            {
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
            return Json(success ? JsonResponse.SuccessResponse("Senior CE Provident Fund") : JsonResponse.ErrorResponse(errorMessage));

        }

        /*-----------------------------------------End Edit Senior CE Provident --------------------------------------   */

        /*-----------------------------------------Start Senior CE Promotion --------------------------------------   */

        public ActionResult IndexSCEPromotion()
        {
            List<SeniorCE> getCurrentSCEList = staffViewData.GetCurrentSeniorCEList();
            return View("IndexSCEPromotion", getCurrentSCEList);

        }

        public ActionResult SCEPromotion(int id)
        {

            var sce = db.SENIORCE.Find(id);
            var model = new SeniorCEPromotion();
            model.GradeID = sce.GRADEID;
            model.IsMedical = Convert.ToInt16(sce.ISMEDICAL);
            model.CLevStepID = sce.CIVILIANLEVSTEPID;
            model.DatePromoted = sce.DATEPROMOTED;

            if (sce.ISMEDICAL)
            {
                model.GradeList = settingsViewData.getGradeX(1);
            }
            else
            {
                model.GradeList = settingsViewData.getGradeX(0);
            }
            model.CLevStepList = settingsViewData.getSCELevStep();

            ViewBag.Name = sce.TITLE.TITLENAME + " " + sce.SURNAME + " " + sce.OTHERNAME;
            return View("SCEPromotion", model);
        }
        [HttpPost]
        public ActionResult SCEPromotion(int id, SeniorCEPromotion data)
        {
            if (ModelState.IsValid)
            {

                var sce = db.SENIORCE.Find(id); 
                var levstepID = data.CLevStepID;
                var civilLevStep = db.CIVILIANLEVSTEP.FirstOrDefault(a => a.CIVILIANLEVSTEPID == levstepID);
                var constPay = civilLevStep.CONSTPAY;
                sce.DATEPROMOTED = data.DatePromoted;
                sce.CIVILIANLEVSTEPID = data.CLevStepID;
                sce.GRADEID = data.GradeID;

                if (sce.ISMEDICAL)
                {
                    var gradeID = data.GradeID;
                    var marketPremAmount = payViewData.calculateMarketPremium(constPay, gradeID);
                    var operationAmount = payViewData.calculateOperationAllowance(constPay);

                    var marketPremAllow = db.SENIORCEALLOWANCE.FirstOrDefault(a => a.ALLOWANCEID == MarketPremAllowanceID && a.STATUS == 1 && a.SENIORCEID == id);
                    marketPremAllow.AMOUNT = marketPremAmount;
                    marketPremAllow.UPDATEDBY = "admin";
                   // marketPremAllow.UPDATEDBY = User.Identity.Name;
                    marketPremAllow.DATETIMEUPDATED = DateTime.Now;

                    var operationAllow = db.SENIORCEALLOWANCE.FirstOrDefault(a => a.ALLOWANCEID == OperationAllowanceID && a.STATUS == 1 && a.SENIORCEID == id);
                    operationAllow.AMOUNT = operationAmount;
                    operationAllow.DATETIMEUPDATED=DateTime.Now;
                    operationAllow.UPDATEDBY = "admin";
                   // operationAllow.UPDATEDBY = User.Identity.Name;

                }
                
                if (sce.ISDISABLED)
                {
                    var disabilityAmount = payViewData.calculateDisabilityAllowance(constPay);
                    var disableAllow = db.SENIORCEALLOWANCE.FirstOrDefault(a => a.ALLOWANCEID == DisabilityAllowanceID && a.STATUS == 1 && a.SENIORCEID == id);

                    disableAllow.AMOUNT = disabilityAmount;
                    disableAllow.UPDATEDBY = "admin";
                    //disableAllow.UPDATEDBY = User.Identity.Name;
                    disableAllow.DATETIMEUPDATED=DateTime.Now;
                }
                var sceProvID = sce.PROVIDENTFUNDID;
                var providentAmount = payViewData.calculateProvidentFund(constPay, sceProvID);
                if (sce.PROVIDENTFUNDID != 1)
                {
                    var provDed =
                        db.SENIORCEDEDUCTION.FirstOrDefault(
                            a => a.DEDUCTIONID == ProvidentFundDeductionID && a.STATUS == 1 && a.SENIORCEID == id);

                    provDed.DEDUCTIONAMOUNT = providentAmount;
                    provDed.UPDATEDBY = "admin";
                    //provDed.UPDATEDBY = User.Identity.Name;
                    provDed.DATETIMEUPDATED=DateTime.Now;
                }
                var ssnitAmount = payViewData.calculateSSNIT(constPay);
                var tax = payViewData.calculateIncomeTax(constPay, providentAmount);

                var sceSSNIT = db.SENIORCEDEDUCTION.FirstOrDefault(a => a.DEDUCTIONID == SSNITDeductionID && a.STATUS == 1 && a.SENIORCEID == id);
                sceSSNIT.DEDUCTIONAMOUNT = ssnitAmount;
                sceSSNIT.DATETIMEUPDATED=DateTime.Now;
                sceSSNIT.UPDATEDBY = "admin";
                //sceSSNIT.UPDATEDBY = User.Identity.Name;
                var sceTax = db.SENIORCEDEDUCTION.FirstOrDefault(a => a.DEDUCTIONID == TaxDeductionID && a.STATUS == 1 & a.SENIORCEID == id);
                sceTax.DEDUCTIONAMOUNT = tax;
                sceTax.UPDATEDBY = "admin";
               // sceTax.UPDATEDBY = User.Identity.Name;
                sceTax.DATETIMEUPDATED=DateTime.Now;

                 
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
            return Json(success ? JsonResponse.SuccessResponse("Senior CE Promotion") : JsonResponse.ErrorResponse(errorMessage));
        }



        /*-----------------------------------------End Senior CE Promotion --------------------------------------   */




    }
}