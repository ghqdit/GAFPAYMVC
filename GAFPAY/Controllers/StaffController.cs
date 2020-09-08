using System;
using System.Collections.Generic;
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
                oc.OFFICERINTAKE = data.OfficerIntake;
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




        /*-----------------------------Start Junior CE------------------------------------*/

        public ActionResult IndexJCE()
        {
            List<JuniorCE> getCurrentJCEList = staffViewData.GetCurrentJuniorCEList();
            return View("IndexJCE", getCurrentJCEList);
        }

        public ActionResult CreateJCE()
        {
            var model = new JuniorCE();
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
            model.ProvidentFundList = settingsViewData.getProvidentFund();


            return View("CreateJCE", model);
        }


        [HttpPost]
        public ActionResult CreateJCE(JuniorCE data)
        {
            if (ModelState.IsValid)
            {
                var jce=new JUNIORCE(); 
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
                jce.UNITID = data.UnitID; 
                jce.BLOODGROUPID = data.BloodGroupID; 
                jce.GENERALSTATUSID = genStatus;
                jce.CIVILIANLEVSTEPID=data.CLevStepID;
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
                if (data.IsMedical)
                {
                    var jceMed=new JUNIORCEMEDP();
                    jceMed.JUNIORCEID = jceID;
                    jceMed.MEDPCODE = data.MedPCode;

                    db.JUNIORCEMEDP.Add(jceMed);
                     
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
        public ActionResult EditJCEBank(int id, OfficerCadetBank data)
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



        public ActionResult RemoveJCE()
        {
            return Json(success ? JsonResponse.SuccessResponse("Officer Cadet") : JsonResponse.ErrorResponse(errorMessage));
        }


        /*-----------------------------End Junior CE------------------------------------*/





    }
}