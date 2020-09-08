using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using GAFPAY.Extensions;
using GAFPAY.Models;
using GAFPAY.ViewData;
using GAFPAY.ViewModel;

namespace GAFPAY.Controllers
{
    public class SettingsController : Controller
    {
        private SettingsViewData settingsViewData =new SettingsViewData();
        private bool success = false;
        private string errorMessage = "";
        private DBGAFPAYEntities db=new DBGAFPAYEntities();
        


        /*----------------------------------------- Start Title -----------------------------------------------------  */
        public ActionResult IndexTitle()
        {
            List<Title> titleList = settingsViewData.GetTitleList();
            return View("IndexTitle",titleList);
        }

        public ActionResult CreateTitle()
        {
            var model=new Title();

            return View("EditorTitle", model);
        }
        [HttpPost]
        public ActionResult CreateTitle(Title data)
        {
             
            if (ModelState.IsValid)
            {
                
                var title=new TITLE();
                title.TITLENAME = data.TitleName;
                title.STATUS = 1;
                db.TITLE.Add(title);

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

            return Json(success ? JsonResponse.SuccessResponse("Title") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditTitle(int id)
        {
            var title = db.TITLE.Find(id);
            var model=new Title();
            if (title != null)
            {
                model.IsEdit = true;
                model.TitleName = title.TITLENAME;
                model.TitleID = title.TITLEID; 
            }
            else
            {
                ViewBag.ErrorMessage = "Title does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            } 
            return View("EditorTitle",model);
        }
        [HttpPost]
        public ActionResult EditTitle(int id, Title data)
        {

            if (ModelState.IsValid)
            { 
                var title = db.TITLE.Find(id);
                 
                    title.TITLENAME = data.TitleName; 

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

            return Json(success ? JsonResponse.SuccessResponse("Title") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult RemoveTitle(int id)
        {
            var title = db.TITLE.Find(id);
            title.STATUS = 0;
            //db.TITLE.Remove(title);
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(title.TITLENAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Title -----------------------------------------------------  */
        
        
        /*----------------------------------------- Start Gender -----------------------------------------------------  */

        public ActionResult IndexGender()
        {
            List<Gender> getGenderList = settingsViewData.GetGenderList();
            return View("IndexGender",getGenderList);
        }


        public ActionResult CreateGender()
        {
            var model = new Gender();

            return View("EditorGender", model);
        }
        [HttpPost]
        public ActionResult CreateGender(Gender data)
        {

            if (ModelState.IsValid)
            {

                var gender = new GENDER();
                gender.GENDERNAME = data.GenderName;
                gender.STATUS = 1;
                db.GENDER.Add(gender);

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

            return Json(success ? JsonResponse.SuccessResponse("Gender") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditGender(int id)
        {
            var gender = db.GENDER.Find(id);
            var model = new Gender();
            if (gender != null)
            {
                model.IsEdit = true;
                model.GenderName = gender.GENDERNAME;
                model.GenderID = gender.GENDERID;
            }
            else
            {
                ViewBag.ErrorMessage = "Gender does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorGender", model);
        }
        [HttpPost]
        public ActionResult EditGender(int id, Gender data)
        { 

            if (ModelState.IsValid)
            {
                var gender = db.GENDER.Find(id);
                gender.GENDERNAME = data.GenderName; 

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

            return Json(success ? JsonResponse.SuccessResponse("Gender") : JsonResponse.ErrorResponse(errorMessage)); 
        }

        public ActionResult RemoveGender(int id)
        {
            var gender = db.GENDER.Find(id);

            //db.GENDER.Remove(gender);
            gender.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(gender.GENDERNAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Gender -----------------------------------------------------  */

        /*----------------------------------------- Start Religion -----------------------------------------------------  */

        public ActionResult IndexReligion()
        {
            List<Religion> getReligionList = settingsViewData.GetReligionList();
            return View("IndexReligion", getReligionList);
        }


        public ActionResult CreateReligion()
        {
            var model = new Religion(); 
            return View("EditorReligion", model);
        }
        [HttpPost]
        public ActionResult CreateReligion(Religion data)
        {

            if (ModelState.IsValid)
            {

                var religion = new RELIGION();
                religion.RELIGIONNAME = data.ReligionName;
                religion.STATUS = 1;
                db.RELIGION.Add(religion);

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

            return Json(success ? JsonResponse.SuccessResponse("Religion") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditReligion(int id)
        {
            var religion = db.RELIGION.Find(id);
            var model = new Religion();
            if (religion != null)
            {
                model.IsEdit = true;
                model.ReligionName = religion.RELIGIONNAME;
                model.ReligionID = religion.RELIGIONID;
            }
            else
            {
                ViewBag.ErrorMessage = "Religion does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorReligion", model);
        }
        [HttpPost]
        public ActionResult EditReligion(int id, Religion data)
        {

            if (ModelState.IsValid)
            {
                var religion = db.RELIGION.Find(id);
                religion.RELIGIONNAME = data.ReligionName;

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

            return Json(success ? JsonResponse.SuccessResponse("Religion") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveReligion(int id)
        {
            var religion = db.RELIGION.Find(id);
            religion.STATUS = 0; 
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(religion.RELIGIONNAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Religion -----------------------------------------------------  */

        /*----------------------------------------- Start Region -----------------------------------------------------  */

        public ActionResult IndexRegion()
        {
            List<RegionX> getRegionList = settingsViewData.GetRegionList();
            return View("IndexRegion", getRegionList);
        }


        public ActionResult CreateRegion()
        {
            var model = new RegionX();
            return View("EditorRegion", model);
        }
        [HttpPost]
        public ActionResult CreateRegion(RegionX data)
        {

            if (ModelState.IsValid)
            {

                var region = new REGION();
                region.REGIONNAME = data.RegionName;
                region.REGIONSHORT = data.RegionShort.ToUpper();
                region.STATUS = 1;
                db.REGION.Add(region);

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

            return Json(success ? JsonResponse.SuccessResponse("Region") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditRegion(int id)
        {
            var region = db.REGION.Find(id);
            var model = new RegionX();
            if (region != null)
            {
                model.IsEdit = true;
                model.RegionName = region.REGIONNAME;
                model.RegionShort = region.REGIONSHORT;
                model.RegionID = region.REGIONID;
            }
            else
            {
                ViewBag.ErrorMessage = "Region does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorRegion", model);
        }
        [HttpPost]
        public ActionResult EditRegion(int id, RegionX data)
        {

            if (ModelState.IsValid)
            {
                var region = db.REGION.Find(id);
                region.REGIONNAME = data.RegionName;
                region.REGIONSHORT = data.RegionShort;

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

            return Json(success ? JsonResponse.SuccessResponse("Region") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveRegion(int id)
        {
            var region = db.REGION.Find(id);
            region.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(region.REGIONNAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Religion -----------------------------------------------------  */

        /*----------------------------------------- Start Blood Group -----------------------------------------------------  */

        public ActionResult IndexBloodGroup()
        {
            List<BloodGroup> getBloodGroupList = settingsViewData.GetBloodGroupList();
            return View("IndexBloodGroup", getBloodGroupList);
        }


        public ActionResult CreateBloodGroup()
        {
            var model = new BloodGroup();
            return View("EditorBloodGroup", model);
        }
        [HttpPost]
        public ActionResult CreatebloodGroup(BloodGroup data)
        {

            if (ModelState.IsValid)
            {

                var blood = new BLOODGROUP();
                blood.BLOODGROUPNAME = data.BloodGroupName;
                blood.STATUS = 1;
                
                db.BLOODGROUP.Add(blood);

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

            return Json(success ? JsonResponse.SuccessResponse("Blood group") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditBloodGroup(int id)
        {
            var blood = db.BLOODGROUP.Find(id);
            var model = new BloodGroup();
            if (blood != null)
            {
                model.IsEdit = true;
                model.BloodGroupName = blood.BLOODGROUPNAME; 
                model.BloodGroupID = blood.BLOODGROUPID;
            }
            else
            {
                ViewBag.ErrorMessage = "Blood Group does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorBloodGroup", model);
        }
        [HttpPost]
        public ActionResult EditBloodGroup(int id, BloodGroup data)
        {

            if (ModelState.IsValid)
            {
                var blood =db.BLOODGROUP.Find(id);
                blood.BLOODGROUPNAME = data.BloodGroupName;
                
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

            return Json(success ? JsonResponse.SuccessResponse("Blood group") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveBloodGroup(int id)
        {
            var blood = db.BLOODGROUP.Find(id);
            blood.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(blood.BLOODGROUPNAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Religion -----------------------------------------------------  */



        /*----------------------------------------- Start Service -----------------------------------------------------  */

        public ActionResult IndexService()
        {
            List<Service> getServiceList = settingsViewData.GetServiceList();
            return View("IndexService", getServiceList);
        }


        public ActionResult CreateService()
        {
            var model = new Service();
            return View("EditorService", model);
        }
        [HttpPost]
        public ActionResult CreateService(Service data)
        {

            if (ModelState.IsValid)
            {

                var service = new SERVICE();
                service.SERVICENAME = data.ServiceName;
                service.SERVICESHORTNAME = data.ServiceShort.ToUpper();
                service.STATUS = 1;
                db.SERVICE.Add(service);

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

            return Json(success ? JsonResponse.SuccessResponse("Service") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditService(int id)
        {
            var service = db.SERVICE.Find(id);
            var model = new Service();
            if (service != null)
            {
                model.IsEdit = true;
                model.ServiceName = service.SERVICENAME;
                model.ServiceShort = service.SERVICESHORTNAME;
                model.ServiceID = service.SERVICEID;
            }
            else
            {
                ViewBag.ErrorMessage = "Service does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorService", model);
        }
        [HttpPost]
        public ActionResult EditService(int id, Service data)
        {

            if (ModelState.IsValid)
            {
                var service = db.SERVICE.Find(id);
                service.SERVICENAME = data.ServiceName;
                service.SERVICESHORTNAME = data.ServiceShort;

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

            return Json(success ? JsonResponse.SuccessResponse("Service") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveService(int id)
        {
            var service = db.SERVICE.Find(id);
            service.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(service.SERVICENAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Service -----------------------------------------------------  */



        /*----------------------------------------- Start Unit -----------------------------------------------------  */

        public ActionResult IndexUnit()
        {
            List<Unit> getUnitList = settingsViewData.GetUnitList();
            return View("IndexUnit", getUnitList);
        }


        public ActionResult CreateUnit()
        {
            var model = new Unit();
            return View("EditorUnit", model);
        }
        [HttpPost]
        public ActionResult CreateUnit(Unit data)
        {

            if (ModelState.IsValid)
            {

                var unit = new UNIT();
                unit.UNITNAME = data.UnitName;
                unit.UNITSHORT = data.UnitShort.ToUpper();
                unit.STATUS = 1;
                db.UNIT.Add(unit);

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

            return Json(success ? JsonResponse.SuccessResponse("Unit") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditUnit(int id)
        {
            var unit = db.UNIT.Find(id);
            var model = new Unit();
            if (unit != null)
            {
                model.IsEdit = true;
                model.UnitName = unit.UNITNAME;
                model.UnitShort = unit.UNITSHORT;
                model.UnitID = unit.UNITID;
            }
            else
            {
                ViewBag.ErrorMessage = "Unit does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorUnit", model);
        }
        [HttpPost]
        public ActionResult EditUnit(int id, Unit data)
        {

            if (ModelState.IsValid)
            {
                var unit = db.UNIT.Find(id);
                unit.UNITNAME = data.UnitName;
                unit.UNITSHORT = data.UnitShort;

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

            return Json(success ? JsonResponse.SuccessResponse("Unit") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveUnit(int id)
        {
            var unit = db.UNIT.Find(id);
            unit.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(unit.UNITNAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Service -----------------------------------------------------  */



        /*----------------------------------------- Start Comission Type -----------------------------------------------------  */

        public ActionResult IndexCommissionType()
        {
            List<CommissionType> getCommissionTypeList = settingsViewData.GetCommissionTypeList();
            return View("IndexCommissionType", getCommissionTypeList);
        }


        public ActionResult CreateCommissionType()
        {
            var model = new CommissionType();
            return View("EditorCommissionType", model);
        }
        [HttpPost]
        public ActionResult CreateCommissionType(CommissionType data)   
        {

            if (ModelState.IsValid)
            {

                var cType = new COMMISSIONTYPE();
                cType.COMMISSIONTYPENAME = data.CommissionTypeName;
                cType.COMMISSIONTYPESHORT = data.CommissionShort.ToUpper();
                cType.STATUS = 1;
                db.COMMISSIONTYPE.Add(cType);

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

            return Json(success ? JsonResponse.SuccessResponse("Commission type") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditCommissionType(int id)
        {
            var cType = db.COMMISSIONTYPE.Find(id);
            var model = new CommissionType();
            if (cType != null)
            {
                model.IsEdit = true;
                model.CommissionTypeName = cType.COMMISSIONTYPENAME;
                model.CommissionShort = cType.COMMISSIONTYPESHORT;
                model.CommissionTypeID = cType.COMMISSIONTYPEID;
            }
            else
            {
                ViewBag.ErrorMessage = "Commission Type does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorCommissionType", model);
        }
        [HttpPost]
        public ActionResult EditCommissionType(int id, CommissionType data)
        {

            if (ModelState.IsValid)
            {
                var cType = db.COMMISSIONTYPE.Find(id);
                cType.COMMISSIONTYPENAME = data.CommissionTypeName;
                cType.COMMISSIONTYPESHORT = data.CommissionShort.ToUpper();

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

            return Json(success ? JsonResponse.SuccessResponse("Commission Type") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveCommissionType(int id)
        {
            var cType = db.COMMISSIONTYPE.Find(id);
            cType.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(cType.COMMISSIONTYPENAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Commission type -----------------------------------------------------  */



        /*----------------------------------------- Start Deduction Class -----------------------------------------------------  */

        public ActionResult IndexDeductionClass()
        {
            List<DeductionClass> getDeductionClassList = settingsViewData.GetDeductionClassList();
            return View("IndexDeductionClass", getDeductionClassList);
        }


        public ActionResult CreateDeductionClass()
        {
            var model = new DeductionClass();
            return View("EditorDeductionClass", model);
        }
        [HttpPost]
        public ActionResult CreateDeductionClass(DeductionClass data)
        {

            if (ModelState.IsValid)
            {

                var deduc = new DEDUCTIONCLASS();
                deduc.DEDUCTIONCLASSNAME = data.DeductionClassName;
                deduc.STATUS = 1;
                db.DEDUCTIONCLASS.Add(deduc);

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

            return Json(success ? JsonResponse.SuccessResponse("Deduction class") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditDeductionClass(int id)
        {
            var deduc = db.DEDUCTIONCLASS.Find(id);
            var model = new DeductionClass();
            if (deduc != null)
            {
                model.IsEdit = true;
                model.DeductionClassName = deduc.DEDUCTIONCLASSNAME;
                 
            }
            else
            {
                ViewBag.ErrorMessage = "Deduction Class does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorDeductionClass", model);
        }
        [HttpPost]
        public ActionResult EditDeductionClass(int id, DeductionClass data)
        {

            if (ModelState.IsValid)
            {
                var deduc = db.DEDUCTIONCLASS.Find(id);
                deduc.DEDUCTIONCLASSNAME = data.DeductionClassName;
                
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

            return Json(success ? JsonResponse.SuccessResponse("Deduction class") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveDeductionClass(int id)
        {
            var deduc = db.DEDUCTIONCLASS.Find(id);
            deduc.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(deduc.DEDUCTIONCLASSNAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Deduction Class -----------------------------------------------------  */



        /*----------------------------------------- Start General Status -----------------------------------------------------  */

        public ActionResult IndexGStatus()
        {
            List<GeneralStatus> getGeneralStatusList = settingsViewData.GetGeneralStatusList();
            return View("IndexGStatus", getGeneralStatusList);
        }


        public ActionResult CreateGStatus()
        {
            var model = new GeneralStatus();
            return View("EditorGStatus", model);
        }
        [HttpPost]
        public ActionResult CreateGStatus(GeneralStatus data)
        {

            if (ModelState.IsValid)
            {

                var status = new GENERALSTATUS();
                status.GSNAME = data.GSName;
                status.GSSHORT = data.GSShort.ToUpper();
                status.RATE = data.Rate; 
                status.STATUS = 1;
                db.GENERALSTATUS.Add(status);

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

            return Json(success ? JsonResponse.SuccessResponse("Status") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditGStatus(int id)
        {
            var status = db.GENERALSTATUS.Find(id);
            var model = new GeneralStatus();
            if (status != null)
            {
                model.IsEdit = true;
                model.GSName = status.GSNAME;
                model.GSShort = status.GSSHORT;
                model.Rate = status.RATE;
                model.GeneralStatusID = status.GENERALSTATUSID;
            }
            else
            {
                ViewBag.ErrorMessage = "General Status does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorGStatus", model);
        }
        [HttpPost]
        public ActionResult EditGStatus(int id, GeneralStatus data)
        {

            if (ModelState.IsValid)
            {
                var status = db.GENERALSTATUS.Find(id);
                status.GSNAME = data.GSName;
                status.GSSHORT = data.GSShort;
                status.RATE = data.Rate;

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

            return Json(success ? JsonResponse.SuccessResponse("Status") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveGStatus(int id)
        {
            var status = db.GENERALSTATUS.Find(id);
            status.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(status.GSNAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End General Status -----------------------------------------------------  */


        ///*----------------------------------------- Start Officer Intake -----------------------------------------------------  */

        //public ActionResult IndexOIntake()
        //{
        //    List<OfficerIntake> getOIntakeList = settingsViewData.GetOfficerIntakeList();
        //    return View("IndexOIntake", getOIntakeList);
        //}


        //public ActionResult CreateOIntake()
        //{
        //    var model = new OfficerIntake();
        //    return View("EditorOIntake", model);
        //}
        //[HttpPost]
        //public ActionResult CreateOIntake(OfficerIntake data)
        //{

        //    if (ModelState.IsValid)
        //    {

        //        var oi = new OFFICERINTAKE();
        //        oi.OFFICERINTAKENAME = data.OfficerIntakeName;
        //        oi.STATUS = 1;
        //        db.OFFICERINTAKE.Add(oi);

        //        try
        //        {
        //            db.SaveChanges();
        //            success = true;
        //        }
        //        catch (Exception e)
        //        {
        //            errorMessage = e.Message;
        //        }

        //    }
        //    else
        //    {
        //        errorMessage = string.Join(" | ", ModelState.Values
        //            .SelectMany(v => v.Errors)
        //            .Select(e => e.ErrorMessage));
        //    }

        //    return Json(success ? JsonResponse.SuccessResponse("Officer intake") : JsonResponse.ErrorResponse(errorMessage));

        //}

        //public ActionResult EditOIntake(int id)
        //{
        //    var oi = db.OFFICERINTAKE.Find(id);
        //    var model = new OfficerIntake();
        //    if (oi != null)
        //    {
        //        model.IsEdit = true;
        //        model.OfficerIntakeName = oi.OFFICERINTAKENAME;
        //        model.OfficerIntakeID = oi.OFFICERINTAKEID;
        //    }
        //    else
        //    {
        //        ViewBag.ErrorMessage = "Officer Intake does not exist. Kinldy contact Database Administrator for assistance";
        //        return View("Error");
        //    }
        //    return View("EditorOIntake", model);
        //}
        //[HttpPost]
        //public ActionResult EditOIntake(int id, OfficerIntake data)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var oi = db.OFFICERINTAKE.Find(id);
        //        oi.OFFICERINTAKENAME = data.OfficerIntakeName;
                
        //        try
        //        {
        //            db.SaveChanges();
        //            success = true;
        //        }
        //        catch (Exception e)
        //        {
        //            errorMessage = e.Message;
        //        }

        //    }
        //    else
        //    {
        //        errorMessage = string.Join(" | ", ModelState.Values
        //            .SelectMany(v => v.Errors)
        //            .Select(e => e.ErrorMessage));
        //    }

        //    return Json(success ? JsonResponse.SuccessResponse("Officer intake") : JsonResponse.ErrorResponse(errorMessage));
        //}

        //public ActionResult RemoveOIntake(int id)
        //{
        //    var oi = db.OFFICERINTAKE.Find(id);
        //    oi.STATUS = 0;
        //    try
        //    {
        //        db.SaveChanges();
        //        success = true;
        //    }
        //    catch (Exception e)
        //    {

        //        errorMessage = e.Message;
        //    }
        //    return Json(success ? JsonResponse.SuccessResponse(oi.OFFICERINTAKENAME) : JsonResponse.ErrorResponse(errorMessage));
        //}

        ///*----------------------------------------- End Officer Intake -----------------------------------------------------  */



        /*----------------------------------------- Start Recruit Course -----------------------------------------------------  */

        public ActionResult IndexRCourse()
        {
            List<RecruitCourse> getRecCourseList = settingsViewData.GetRecruitCourseList();
            return View("IndexRCourse", getRecCourseList);
        }


        public ActionResult CreateRCourse()
        {
            var model = new RecruitCourse();
            model.ServiceList = settingsViewData.getServices();
            return View("EditorRCourse", model);
        }
        [HttpPost]
        public ActionResult CreateRCourse(RecruitCourse data)
        {

            if (ModelState.IsValid)
            {

                var rc = new RECRUITCOURSE();
                rc.RCNAME = data.RecruitCourseName;
                rc.SERVICEID = data.ServiceID;
                rc.STATUS = 1;
                db.RECRUITCOURSE.Add(rc);

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

            return Json(success ? JsonResponse.SuccessResponse("Recruit Course") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditRCourse(int id)
        {
            var rc = db.RECRUITCOURSE.Find(id);
            var model = new RecruitCourse();
            if (rc != null)
            {
                model.IsEdit = true;
                model.RecruitCourseName = rc.RCNAME;
                model.ServiceID = rc.SERVICEID;
                model.RecruitCourseID = rc.RCID;
            }
            else
            {
                ViewBag.ErrorMessage = "Recruit Course does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorRCourse", model);
        }
        [HttpPost]
        public ActionResult EditRCourse(int id, RecruitCourse data)
        {

            if (ModelState.IsValid)
            {
                var rc = db.RECRUITCOURSE.Find(id);
                rc.RCNAME = data.RecruitCourseName;
                rc.SERVICEID = data.ServiceID;
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

            return Json(success ? JsonResponse.SuccessResponse("Recruit Course") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveRCourse(int id)
        {
            var rc = db.RECRUITCOURSE.Find(id);
            rc.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(rc.RCNAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Recruit Course -----------------------------------------------------  */



        /*----------------------------------------- Start Region -----------------------------------------------------  */

        public ActionResult IndexTCenter()
        {
            List<TrainingCenter> getTCList = settingsViewData.GetTrainingCenterList();
            return View("IndexTCenter", getTCList);
        }


        public ActionResult CreateTCenter()
        {
            var model = new TrainingCenter();
            model.ServiceList = settingsViewData.getServices();
            return View("EditorTCenter", model);
        }
        [HttpPost]
        public ActionResult CreateTCenter(TrainingCenter data)
        {

            if (ModelState.IsValid)
            {

                var tc = new TRAININGCENTER();
                tc.TCNAME = data.TrainingCenterName;
                tc.LOCATION = data.Location;
                tc.SERVICEID = data.ServiceID;
                tc.STATUS = 1;
                db.TRAININGCENTER.Add(tc);

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

            return Json(success ? JsonResponse.SuccessResponse("Training Center") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditTCenter(int id)
        {
            var tc = db.TRAININGCENTER.Find(id);
            var model = new TrainingCenter();
            if (tc != null)
            {
                model.IsEdit = true;
                model.TrainingCenterName = tc.TCNAME;
                model.Location = tc.LOCATION;
                model.ServiceID = tc.SERVICEID;
                model.TrainingCenterID = tc.TCID;
            }
            else
            {
                ViewBag.ErrorMessage = "Training Center does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorTCenter", model);
        }
        [HttpPost]
        public ActionResult EditTCenter(int id, TrainingCenter data)
        {

            if (ModelState.IsValid)
            {
                var tc = db.TRAININGCENTER.Find(id);
                tc.TCNAME = data.TrainingCenterName;
                tc.LOCATION = data.Location;
                tc.SERVICEID = data.ServiceID;

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

            return Json(success ? JsonResponse.SuccessResponse("Training Center") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveTCenter(int id)
        {
            var tc = db.TRAININGCENTER.Find(id);
            tc.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(tc.TCNAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Training Center -----------------------------------------------------  */



        /*----------------------------------------- Start Allowance -----------------------------------------------------  */

        public ActionResult IndexAllowance()
        {
            List<Allowance> getAllowList = settingsViewData.GetAllowanceList();
            return View("IndexAllowance", getAllowList);
        }


        public ActionResult CreateAllowance()
        {
            var model = new Allowance();
            model.IsTaxableList = settingsViewData.getYesNo();
            return View("EditorAllowance", model);
        }
        [HttpPost]
        public ActionResult CreateAllowance(Allowance data)
        {

            if (ModelState.IsValid)
            {

                var all = new ALLOWANCE();
                all.ALLOWANCENAME = data.AllowanceName;
                all.ISTAXABLE =Convert.ToBoolean(data.IsTaxable);
                all.STATUS = 1;
                db.ALLOWANCE.Add(all);

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

            return Json(success ? JsonResponse.SuccessResponse("Allowance") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditAllowance(int id)
        {
            var all = db.ALLOWANCE.Find(id);
            var model = new Allowance();
            if (all != null)
            {
                model.IsEdit = true;
                model.AllowanceName = all.ALLOWANCENAME;
                model.TaxableID = Convert.ToInt32(all.ISTAXABLE);
                model.IsTaxableList = settingsViewData.getYesNo(); 
                model.AllowanceID = all.ALLOWANCEID;
            }
            else
            {
                ViewBag.ErrorMessage = "Allowance does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorAllowance", model);
        }
        [HttpPost]
        public ActionResult EditAllowance(int id, Allowance data)
        {

            if (ModelState.IsValid)
            {
                var all = db.ALLOWANCE.Find(id);
                all.ALLOWANCENAME = data.AllowanceName;
                all.ISTAXABLE = Convert.ToBoolean(data.TaxableID);

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

            return Json(success ? JsonResponse.SuccessResponse("Allowance") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveAllowance(int id)
        {
            var all = db.ALLOWANCE.Find(id);
            all.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(all.ALLOWANCENAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Allowance -----------------------------------------------------  */


        /*----------------------------------------- Start Deduction -----------------------------------------------------  */

        public ActionResult IndexDeduction()
        {
            List<Deduction> getDeducList = settingsViewData.GetDeductionList();
            return View("IndexDeduction", getDeducList);
        }


        public ActionResult CreateDeduction()
        {
            var model = new Deduction();
            model.DeductionClassList = settingsViewData.getDeductionClass();
            return View("EditorDeduction", model);
        }
        [HttpPost]
        public ActionResult CreateDeduction(Deduction data)
        {

            if (ModelState.IsValid)
            {

                var ded = new DEDUCTION();
                ded.DEDUCTIONNAME = data.DeductionName;
                ded.DEDUCTIONCLASSID = data.DeductionClassID;
                ded.STATUS = 1;
                db.DEDUCTION.Add(ded);

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

            return Json(success ? JsonResponse.SuccessResponse("Deduction") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditDeduction(int id)
        {
            var ded = db.DEDUCTION.Find(id);
            var model = new Deduction();
            if (ded != null)
            {
                model.IsEdit = true;
                model.DeductionName = ded.DEDUCTIONNAME;
                model.DeductionClassID = ded.DEDUCTIONCLASSID;
                model.DeductionClassList = settingsViewData.getDeductionClass();
                model.DeductionID = ded.DEDUCTIONID;
            }
            else
            {
                ViewBag.ErrorMessage = "Deduction does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorDeduction", model);
        }
        [HttpPost]
        public ActionResult EditDeduction(int id, Deduction data)
        {

            if (ModelState.IsValid)
            {
                var ded = db.DEDUCTION.Find(id);
                ded.DEDUCTIONNAME = data.DeductionName;
                ded.DEDUCTIONCLASSID = data.DeductionClassID;  

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

            return Json(success ? JsonResponse.SuccessResponse("Deduction") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveDeduction(int id)
        {
            var ded = db.DEDUCTION.Find(id);
            ded.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(ded.DEDUCTIONNAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Deduction -----------------------------------------------------  */

        /*----------------------------------------- Start Ranks -----------------------------------------------------  */

        public ActionResult IndexRank()
        {
            List<Rank> getRankList = settingsViewData.GetRankList();
            return View("IndexRank", getRankList);
        }


        public ActionResult CreateRank()
        {
            var model = new Rank();
            model.ServiceList = settingsViewData.getServices();
            model.IsOfficerList = settingsViewData.getYesNo();

            return View("EditorRank", model);
        }
        [HttpPost]
        public ActionResult CreateRank(Rank data)
        {

            if (ModelState.IsValid)
            { 
                var r = new RANK();
                r.RANKNAME = data.RankName;
                r.RANKSHORT = data.RankShort.ToUpper();
                r.STATUS = 1;
                r.SERVICEID = data.ServiceID;
                r.ISOFFICER = Convert.ToBoolean(data.IsOfficerID);
                db.RANK.Add(r);

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

            return Json(success ? JsonResponse.SuccessResponse("Rank") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditRank(int id)
        {
            var r = db.RANK.Find(id);
            var model = new Rank();
            if (r != null)
            {
                model.IsEdit = true;
                model.RankName = r.RANKNAME;
                model.RankShort = r.RANKSHORT;
                model.ServiceList = settingsViewData.getServices();
                model.IsOfficerList = settingsViewData.getYesNo();
                model.IsOfficer = r.ISOFFICER;
                model.ServiceID = r.SERVICEID;
                model.RankID = r.RANKID;
            }
            else
            {
                ViewBag.ErrorMessage = "Rank does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorRank", model);
        }
        [HttpPost]
        public ActionResult EditRank(int id, Rank data)
        {

            if (ModelState.IsValid)
            {
                var r = db.RANK.Find(id);
                r.RANKNAME = data.RankName;
                r.RANKSHORT = data.RankShort.ToUpper();
                r.SERVICEID = data.ServiceID;
                r.ISOFFICER = data.IsOfficer;
                  
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

            return Json(success ? JsonResponse.SuccessResponse("Rank") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveRank(int id)
        {
            var r = db.RANK.Find(id);
            r.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }   
            return Json(success ? JsonResponse.SuccessResponse(r.RANKNAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Ranks -----------------------------------------------------  */



        /*----------------------------------------- Start Banks -----------------------------------------------------  */

        public ActionResult IndexBank()
        {
            List<Bank> getBankList = settingsViewData.GetBankList();
            return View("IndexBank", getBankList);
        }


        public ActionResult CreateBank()
        {
            var model = new Bank();
            model.BankNameList = settingsViewData.getBankNames();
            
            return View("EditorBank", model);
        }
        [HttpPost]
        public ActionResult CreateBank(Bank data)
        {

            if (ModelState.IsValid)
            {
                var b = new BANK();
                b.BANKBRANCH = data.BankBranch;
                b.BANKCODE = data.BankCode;
                b.STATUS = 1;
                b.BANKNAMEID = data.BankNameID;
                
                db.BANK.Add(b);

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

            return Json(success ? JsonResponse.SuccessResponse("Bank") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditBank(int id)
        {
            var b = db.BANK.Find(id);
            var model = new Bank();
            if (b != null)
            {
                model.IsEdit = true;
                model.BankBranch =b.BANKBRANCH;
                model.BankNameID =b.BANKNAMEID;
                model.BankCode = b.BANKCODE;
                model.BankNameList = settingsViewData.getBankNames();
                model.BankID = b.BANKID;
                
            }
            else
            {
                ViewBag.ErrorMessage = "Bank Branch does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorBank", model);
        }
        [HttpPost]
        public ActionResult EditBank(int id, Bank data)
        {

            if (ModelState.IsValid)
            {
                var b = db.BANK.Find(id);
                b.BANKBRANCH = data.BankBranch;
                b.BANKCODE = data.BankCode;
                b.BANKNAMEID = data.BankNameID;
                
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

            return Json(success ? JsonResponse.SuccessResponse("Bank") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveBank(int id)
        {
            var b = db.BANK.Find(id);
            b.STATUS = 0;
            var name = b.BANKNAME.BANKNAMEX;
            var nameX = b.BANKBRANCH;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(name +" " + nameX) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Bank -----------------------------------------------------  */


        /*----------------------------------------- Start Bank Name -----------------------------------------------------  */

        public ActionResult IndexBankName()
        {
            List<BankName> getBankNameList = settingsViewData.GetBankNameXList();
            return View("IndexBankName", getBankNameList);
        }


        public ActionResult CreateBankName()
        {
            var model = new BankName(); 
            return View("EditorBankName", model);
        }
        [HttpPost]
        public ActionResult CreateBankName(BankName data)
        {

            if (ModelState.IsValid)
            {
                var b = new BANKNAME();
                b.BANKNAMEX = data.BankNameX;
                b.BANKSHORT = data.ShortName; 
                b.STATUS = 1;
               
                db.BANKNAME.Add(b);

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

            return Json(success ? JsonResponse.SuccessResponse("Bank Name") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditBankName(int id)
        {
            var b = db.BANKNAME.Find(id);
            var model = new BankName();
            if (b != null)
            {
                model.IsEdit = true;
                model.BankNameX = b.BANKNAMEX;
                model.ShortName = b.BANKSHORT; 

            }
            else
            {
                ViewBag.ErrorMessage = "Bank Name does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorBankName", model);
        }
        [HttpPost]
        public ActionResult EditBankName(int id, BankName data) 
        {

            if (ModelState.IsValid)
            {
                var b = db.BANKNAME.Find(id);
                b.BANKNAMEX = data.BankNameX;
                b.BANKSHORT = data.ShortName;


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

            return Json(success ? JsonResponse.SuccessResponse("Bank Name") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveBankName(int id)
        {
            var b = db.BANKNAME.Find(id);
            b.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(b.BANKNAMEX) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Bank Name -----------------------------------------------------  */

        /*----------------------------------------- Start Civilian Lev Step -----------------------------------------------------  */

        public ActionResult IndexCLevStep()
        {
            List<CivilianLevStep> getCLevStepList = settingsViewData.GetCivilianLevStepsList();
            return View("IndexCLevStep", getCLevStepList);
        }


        public ActionResult CreateCLevStep()
        {
            var model = new CivilianLevStep();
            model.IsSeniorList = settingsViewData.getYesNo();
            return View("EditorCLevStep", model);
        }
        [HttpPost]
        public ActionResult CreateCLevStep(CivilianLevStep data)
        {

            if (ModelState.IsValid)
            {
                var c = new CIVILIANLEVSTEP();
                c.LEVSTEPNAME = data.LevStepName;
                c.CONSTPAY = data.ConstPay;
                c.ISSENIOR = Convert.ToBoolean(data.IsSeniorID); 
                c.STATUS = 1;

                db.CIVILIANLEVSTEP.Add(c);

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

            return Json(success ? JsonResponse.SuccessResponse("Lev Step") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditCLevStep(int id)
        {
            var c = db.CIVILIANLEVSTEP.Find(id);
            var model = new CivilianLevStep();
            if (c != null)
            {
                model.IsEdit = true;
                model.LevStepName = c.LEVSTEPNAME;
                model.ConstPay = c.CONSTPAY;
                model.IsSeniorID = Convert.ToInt32(c.ISSENIOR);
                model.IsSeniorList = settingsViewData.getYesNo();
                 
            }
            else
            {
                ViewBag.ErrorMessage = "Civilian Lev Step does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorCLevStep", model);
        }
        [HttpPost]
        public ActionResult EditCLevStep(int id, CivilianLevStep data)
        {

            if (ModelState.IsValid)
            {
                var c = db.CIVILIANLEVSTEP.Find(id);
                c.LEVSTEPNAME = data.LevStepName;
                c.CONSTPAY = data.ConstPay; 
                c.ISSENIOR = Convert.ToBoolean(data.IsSeniorID);

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

            return Json(success ? JsonResponse.SuccessResponse("Lev Step") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveCLevStep(int id)
        {
            var b = db.CIVILIANLEVSTEP.Find(id);
            b.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(b.LEVSTEPNAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Civilian Lev Step -----------------------------------------------------  */


        /*----------------------------------------- Start Military Lev Step -----------------------------------------------------  */

        public ActionResult IndexMLevStep()
        {
            List<MilitaryLevStep> getMLevStepList = settingsViewData.GetMilitaryLevStepsList();
            return View("IndexMLevStep", getMLevStepList);
        }


        public ActionResult CreateMLevStep()
        {
            var model = new MilitaryLevStep(); 
            model.IsOfficerList = settingsViewData.getYesNo();
            return View("EditorMLevStep", model);
        }
        [HttpPost]
        public ActionResult CreateMLevStep(MilitaryLevStep data)
        {

            if (ModelState.IsValid)
            {
                var m = new MILITARYLEVSTEP();
                m.LEVSTEPNAME = data.LevStepName;
                m.CONSTPAY = data.ConstPay; 
                m.ISOFFICER = Convert.ToBoolean(data.IsOfficerID);
                m.STATUS = 1;

                db.MILITARYLEVSTEP.Add(m);

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

            return Json(success ? JsonResponse.SuccessResponse("Lev Step") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditMLevStep(int id)
        {
            var m = db.MILITARYLEVSTEP.Find(id);
            var model = new MilitaryLevStep();
            if (m != null)
            {
                model.IsEdit = true;
                model.LevStepName = m.LEVSTEPNAME;
                model.ConstPay = m.CONSTPAY; 
                model.IsOfficerID = Convert.ToInt16(m.ISOFFICER);
                model.IsOfficerList = settingsViewData.getYesNo();
               

            }
            else
            {
                ViewBag.ErrorMessage = "Military Lev Step does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorMLevStep", model);
        }
        [HttpPost]
        public ActionResult EditMLevStep(int id, MilitaryLevStep data)
        {

            if (ModelState.IsValid)
            {
                var m = db.MILITARYLEVSTEP.Find(id);
                m.LEVSTEPNAME = data.ServiceName;
                m.CONSTPAY = data.ConstPay; 
                m.LEVSTEPNAME = data.LevStepName;
                m.ISOFFICER = Convert.ToBoolean(data.IsOfficerID);

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

            return Json(success ? JsonResponse.SuccessResponse("Lev Step") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveMLevStep(int id)
        {
            var b = db.MILITARYLEVSTEP.Find(id);
            b.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse(b.LEVSTEPNAME) : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Military Lev Step -----------------------------------------------------  */     
        
            
        /*----------------------------------------- Start Provident Fund -----------------------------------------------------  */

        public ActionResult IndexProvidentFund()
        {
            List<ProvidentFund> getProvidentFundList = settingsViewData.GetProvidentFundList();
            return View("IndexProvidentFund", getProvidentFundList);
        }


        public ActionResult CreateProvidentFund()
        {
            var model = new ProvidentFund(); 
             
            return View("EditorProvidentFund", model);
        }
        [HttpPost]
        public ActionResult CreateProvidentFund(ProvidentFund data)
        {

            if (ModelState.IsValid)
            {
                var p = new PROVIDENTFUND();
                p.PROVIDENTRATE = data.ProvidentRate; 
                p.STATUS = 1;

                db.PROVIDENTFUND.Add(p);

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

            return Json(success ? JsonResponse.SuccessResponse("Provident Fund") : JsonResponse.ErrorResponse(errorMessage));

        }

        public ActionResult EditProvidentFund(int id)
        {
            var p = db.PROVIDENTFUND.Find(id);
            var model = new ProvidentFund();
            if (p != null)
            {
                model.IsEdit = true;
                model.ProvidentRate = p.PROVIDENTRATE;
                
            }
            else
            {
                ViewBag.ErrorMessage = "Provident Fund does not exist. Kinldy contact Database Administrator for assistance";
                return View("Error");
            }
            return View("EditorProvidentFund", model);
        }
        [HttpPost]
        public ActionResult EditProvidentFund(int id, ProvidentFund data)
        {

            if (ModelState.IsValid)
            {
                var p = db.PROVIDENTFUND.Find(id);
                p.PROVIDENTRATE = data.ProvidentRate;
                 
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

            return Json(success ? JsonResponse.SuccessResponse("Provident Fund") : JsonResponse.ErrorResponse(errorMessage));
        }

        public ActionResult RemoveProvidentFund(int id) 
        {
            var p = db.PROVIDENTFUND.Find(id);
            p.STATUS = 0;
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception e)
            {

                errorMessage = e.Message;
            }
            return Json(success ? JsonResponse.SuccessResponse("Provient Fund") : JsonResponse.ErrorResponse(errorMessage));
        }

        /*----------------------------------------- End Provident Fund -----------------------------------------------------  */

        /*----------------------------------------- Start Fill Training Center -----------------------------------------------------  */

        public ActionResult FillTC(int id)
        {
            var TC = new TrainingCenter();
            TC.TrainingCenterList = settingsViewData.getTrainingCenter(id);
            return Json(TC.TrainingCenterList.Items, JsonRequestBehavior.AllowGet);
        }
        /*----------------------------------------- End Fill Training Center -----------------------------------------------------  */

        /*----------------------------------------- Start Fill Training Center -----------------------------------------------------  */

        public ActionResult FillRC(int id)
        {
            var RC = new RecruitCourse();
            RC.RecruitCourseNameList = settingsViewData.getRecruitCourse(id);
            return Json(RC.RecruitCourseNameList.Items, JsonRequestBehavior.AllowGet);
        }
        /*----------------------------------------- End Fill Training Center -----------------------------------------------------  */


        /*----------------------------------------- Start Bank Branch -----------------------------------------------------  */

        public ActionResult FillBankBranch(int id)
        {
            var b = new Bank();

            b.BankBranchList = settingsViewData.getBanks(id);
            return Json(b.BankBranchList.Items, JsonRequestBehavior.AllowGet);
        }
        /*----------------------------------------- End Bank Branch -----------------------------------------------------  */


    }
}