using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAFPAY.Models;
using GAFPAY.ViewModel;

namespace GAFPAY.ViewData
{
    public class StaffViewData
    {

        private DBGAFPAYEntities db=new DBGAFPAYEntities();

        /*----------------------------- Start Generate dropdowns SelectLists -----------------------*/
        public SelectList getRecruit()
        {
            var recruit = (from rec in db.RECRUIT
                         join rank in db.RANK
                         on rec.RANKID equals rank.RANKID
                         where rec.GENERALSTATUSID==1
                         select new Recruit()
                         {
                             RecruitID = rec.RECRIUTID,
                             Surname =   "("+rec.SERVICENUMBER +") "+ rank.RANKSHORT + " " + rec.SURNAME + " " + rec.OTHERNAME 
                         }
               ).ToList();
            SelectList recruitList = new SelectList(recruit, "RecruitID", "Surname");
            return recruitList;

        }

        public SelectList getOfficerCadet()
        {
            var OC = (from ocad in db.OFFICERCADET
                join rank in db.RANK
                    on ocad.RANKID equals rank.RANKID
                where ocad.GENERALSTATUSID == 1
                select new OfficerCadet()
                {
                    OfficerCadetID = ocad.OFFICERCADETID,
                    Surname ="("+ ocad.SERVICENUMBER + ") " + rank.RANKSHORT + " " + ocad.SURNAME + " " + ocad.OTHERNAME
                }
                ).ToList();

            SelectList ocadList = new SelectList(OC, "OfficerCadetID", "Surname");
            return ocadList;
        }

        public SelectList getJuniorCE()
        {
            var JCE = (from juniorCE in db.JUNIORCE
                join title in db.TITLE
                    on juniorCE.TITLEID equals title.TITLEID
                where juniorCE.GENERALSTATUSID == 1
                select new JuniorCE()
                {
                    JuniorCEID = juniorCE.JUNIORCEID,
                    Surname = "("+juniorCE.SERVICENUMBER + ") " + juniorCE.SURNAME + " " + juniorCE.OTHERNAME
                }).ToList();
            SelectList JCEList=new SelectList(JCE,"JuniorCEID","Surname");
            return JCEList;

        }

        public SelectList getSeniorCE()
        {
            var SCE = (from seniorCE in db.SENIORCE
                join title in db.TITLE
                    on seniorCE.TITLEID equals title.TITLEID
                where seniorCE.GENERALSTATUSID == 1
                select new SeniorCE()
                {
                    SeniorCEID = seniorCE.SENIORCEID,
                    Surname ="("+ seniorCE.SERVICENUMBER + ") " + seniorCE.SURNAME + " " + seniorCE.OTHERNAME
                }).ToList();
            SelectList SCEList=new SelectList(SCE,"SeniorCEID","Surname");
            return SCEList;

        }

        public SelectList getSoldier()
        {
            var soldier = (from soja in db.SOLDIER
                join rank in db.RANK
                    on soja.RANKID equals rank.RANKID
                where soja.GENERALSTATUSID == 1
                select new Soldier()
                {
                    SoldierID = soja.SOLDEIRID,
                    Surname ="("+ soja.SERVICENUMBER + ") " + rank.RANKSHORT + " " + soja.SURNAME + " " + soja.OTHERNAME
                }).ToList();
            SelectList SoldierList=new SelectList(soldier,"SoldierID","Surname");
            return SoldierList;
        }

        public SelectList getOfficer()
        {
            var officer = (from ofsa in db.OFFICER
                join rank in db.RANK
                    on ofsa.RANKID equals rank.RANKID
                where ofsa.GENERALSTATUSID == 1
                select new Officer()
                {
                    OfficerID = ofsa.OFFICERID,
                    Surname ="("+ ofsa.SERVICENUMBER + ") " + rank.RANKSHORT + " " + ofsa.SURNAME + " " + ofsa.OTHERNAME
                }).ToList();
            SelectList OfficerList=new SelectList(officer,"OfficeID","Surname");
            return OfficerList;
        }


        /*----------------------------- End Generate dropdowns SelectLists -----------------------*/


        /*----------------------------- Start Generate List for Views under settings-----------------------*/

        public List<Recruit> GetCurrentRecruitsList()
        {
            var recruits = db.RECRUIT.Where(a => a.GENERALSTATUSID == 1).Select(a => new Recruit()
            {
                RecruitID = a.RECRIUTID,
                Surname = a.SURNAME,
                Othername = a.OTHERNAME,
                ServiceID = a.SERVICEID,
                ServiceNumber = a.SERVICENUMBER,
                DOB = a.DOB,
                PhoneNumber = a.PHONENUMBER,
                ResAddress = a.RESADDRESS,
                EmailAddress = a.EMAILADDRESS,
                DateRecruitStart = a.RECRUITSTARTDATE,
                //DateRecruitEnd = a.RECRUITENDDATE.Value,
                Hometown = a.HOMETOWN,
                DateTimeInserted = a.DATETIMEINSERTED,
                InsertedBy = a.INSERTEDBY, 
                GenderName = a.GENDER.GENDERNAME,
                TCName = a.TRAININGCENTER.TCNAME,
                TCLocation = a.TRAININGCENTER.LOCATION,
                RegionName = a.REGION.REGIONNAME,
                RankName = a.RANK.RANKNAME,
                RankNameShort=a.RANK.RANKSHORT,
                RCName = a.RECRUITCOURSE.RCNAME,
                ReligionName = a.RELIGION.RELIGIONNAME,
                GeneralStatusName = a.GENERALSTATUS.GSNAME,
                MLevStepName = a.MILITARYLEVSTEP.LEVSTEPNAME,
                BloodGroupName=a.BLOODGROUP.BLOODGROUPNAME,
                ServiceName=a.SERVICE.SERVICENAME
                 
            }).OrderBy(a => a.Surname).ToList();
            return recruits; 
        }

        public List<RecruitPay> GetRecruitTrialPayList(int yearPrev,int prev,int yearCurr, int curr,int yearNext,int next)
        {
            var rec = db.procGetRecruitTrialPayList(yearPrev, prev, yearCurr,curr,yearNext,next).Select(a=> new RecruitPay()
            {
                RecruitID = a.RECRIUTID,
                PayID = a.RTRIALPAYID,
                Surname = a.SURNAME,
                Othername = a.OTHERNAME,
                RankName = a.RANKNAME,
                
                ServiceNumber = a.SERVICENUMBER,
                PayDate=a.PAYDATE
                

            }).OrderBy(a=>a.Surname).ToList();
            return rec;
        }

        public List<OfficerCadetPay> GetOfficerCadetTrialPayList(int yearPrev, int prev, int yearCurr, int curr,
            int yearNext, int next)
        {
            var oc =
                db.progGetOfficerCadetTrialPayList(yearPrev, prev, yearCurr, curr, yearNext, next)
                    .Select(a => new OfficerCadetPay()
                    {
                        PayID = a.OCTRIALPAYID,
                        Surname = a.SURNAME,
                        Othername = a.OTHERNAME,
                        RankName = a.RANKNAME,
                        ServiceNumber = a.SERVICENUMBER,
                        PayDate = a.PAYDATE
                    }).OrderBy(a => a.Surname).ToList();
            return oc;
        } 

        public List<OfficerCadet> GetCurrentOfficerCadetList()
        {
            var officerCadet = db.OFFICERCADET.Where(a => a.GENERALSTATUSID == 1).Select(a => new OfficerCadet()
            {
                OfficerCadetID = a.OFFICERCADETID,
                Surname = a.SURNAME,
                Othername = a.OTHERNAME,
                DOB = a.DOB,
                PhoneNumber = a.PHONENUMBER,
                EmailAddress = a.EMAILADDRESS,
                ResAddress = a.RESADDRESS,
                ServiceNumber = a.SERVICENUMBER,
                OfficerStartDate = a.OFFICERSTARTDATE, 
                Hometown = a.HOMETOWN,
                CommissionTypeName = a.COMMISSIONTYPE.COMMISSIONTYPESHORT, 
                OfficerIntake = a.OFFICERINTAKE,
                ReligionName = a.RELIGION.RELIGIONNAME,
                RegionName = a.REGION.REGIONNAME,
                GenderName = a.GENDER.GENDERNAME,
                BloodGroupName = a.BLOODGROUP.BLOODGROUPNAME,
                ServiceName = a.SERVICE.SERVICENAME,
                RankName = a.RANK.RANKNAME,
                RankNameShort = a.RANK.RANKSHORT,
                GeneralStatusName = a.GENERALSTATUS.GSNAME,
                MLevStepName = a.MILITARYLEVSTEP.LEVSTEPNAME,

            }).OrderBy(a => a.Surname).ToList(); 

            return officerCadet;
        }


        public List<JuniorCE> GetCurrentJuniorCEList()
        {
            var JCE = db.JUNIORCE.Where(a => a.GENERALSTATUSID == 1).Select(a => new JuniorCE()
            {
                JuniorCEID = a.JUNIORCEID,
                Surname = a.SURNAME,
                Othername = a.OTHERNAME,
                DOB = a.DOB,
                PhoneNumber = a.PHONENUMBER,
                EmailAddress = a.EMAILADDRESS,
                ResAddress = a.RESADDRESS,
                ServiceNumber = a.SERVICENUMBER,
                Hometown = a.HOMETOWN,
                DateEmployed = a.DATEEMPLOYED,
                DatePromoted = a.DATEPROMOTED,
                SSNITNo = a.SSNITNUMBER,
                UnitName=a.UNIT.UNITNAME,
                UnitShort=a.UNIT.UNITSHORT,
                RegionName = a.REGION.REGIONNAME,
                ReligionName = a.RELIGION.RELIGIONNAME,
                GenderName = a.GENDER.GENDERNAME,
                GeneralStatusName = a.GENERALSTATUS.GSNAME,
                BloodGroupName = a.BLOODGROUP.BLOODGROUPNAME,
                CLevStepName = a.CIVILIANLEVSTEP.LEVSTEPNAME

            }).OrderBy(a => a.Surname).ToList();
            return JCE;
        }
        public List<SeniorCE> GetCurrentSeniorCEList()
        {
            var SCE = db.SENIORCE.Where(a => a.GENERALSTATUSID == 1).Select(a => new SeniorCE()
            {
                SeniorCEID = a.SENIORCEID,
                Surname = a.SURNAME,
                Othername = a.OTHERNAME,
                DOB = a.DOB,
                PhoneNumber = a.PHONENUMBER,
                EmailAddress = a.EMAILADDRESS,
                ResAddress = a.RESADDRESS,
                ServiceNumber = a.SERVICENUMBER,
                Hometown = a.HOMETOWN,
                DateEmployed = a.DATEEMPLOYED,
                DatePromoted = a.DATEPROMOTED,
                SSNITNo = a.SSNITNUMBER,
                UnitName = a.UNIT.UNITNAME,
                UnitShort = a.UNIT.UNITSHORT,
                RegionName = a.REGION.REGIONNAME,
                ReligionName = a.RELIGION.RELIGIONNAME,
                GenderName = a.GENDER.GENDERNAME,
                GeneralStatusName = a.GENERALSTATUS.GSNAME,
                BloodGroupName = a.BLOODGROUP.BLOODGROUPNAME,
                CLevStepName = a.CIVILIANLEVSTEP.LEVSTEPNAME

            }).OrderBy(a => a.Surname).ToList();
            return SCE;
        }
        public List<Soldier> GetCurrentSoldierList()
        {
            var soja = db.SOLDIER.Where(a => a.GENERALSTATUSID == 1).Select(a => new Soldier()
            {
                SoldierID = a.SOLDEIRID,
                Surname = a.SURNAME,
                Othername = a.OTHERNAME,
                DOB = a.DOB,
                PhoneNumber = a.PHONENUMBER,
                EmailAddress = a.EMAILADDRESS,
                ResAddress = a.RESADDRESS,
                ServiceNumber = a.SERVICENUMBER,
                Hometown = a.HOMETOWN,
                DatePassout = a.DATEPASSOUT,
                DatePromoted = a.DATEPROMOTED,
                UnitName = a.UNIT.UNITNAME,
                UnitShort = a.UNIT.UNITSHORT,
                RegionName = a.REGION.REGIONNAME,
                ReligionName = a.RELIGION.RELIGIONNAME,
                GenderName = a.GENDER.GENDERNAME,
                GeneralStatusName = a.GENERALSTATUS.GSNAME,
                BloodGroupName = a.BLOODGROUP.BLOODGROUPNAME,
                MLevStepName = a.MILITARYLEVSTEP.LEVSTEPNAME

            }).OrderBy(a => a.Surname).ToList();
            return soja;
        }
        public List<Officer> GetCurrentOfficerList()
        {
            var ofsa = db.OFFICER.Where(a => a.GENERALSTATUSID == 1).Select(a => new Officer()
            {
                OfficerID = a.OFFICERID,
                Surname = a.SURNAME,
                Othername = a.OTHERNAME,
                DOB = a.DOB,
                PhoneNumber = a.PHONENUMBER,
                EmailAddress = a.EMAILADDRESS,
                ResAddress = a.RESADDRESS,
                ServiceNumber = a.SERVICENUMBER,
                Hometown = a.HOMETOWN,
                DateCommission = a.DATECOMMISSION,
                DatePromoted = a.DATEPROMOTED,
                UnitName = a.UNIT.UNITNAME,
                UnitShort = a.UNIT.UNITSHORT,
                RegionName = a.REGION.REGIONNAME,
                ReligionName = a.RELIGION.RELIGIONNAME,
                GenderName = a.GENDER.GENDERNAME,
                GeneralStatusName = a.GENERALSTATUS.GSNAME,
                BloodGroupName = a.BLOODGROUP.BLOODGROUPNAME,
                MLevStepName = a.MILITARYLEVSTEP.LEVSTEPNAME

            }).OrderBy(a => a.Surname).ToList();
            return ofsa;
        }


        /*----------------------------- End Generate List for Views under settings-----------------------*/


        /*----------------------------- Start Process Image -----------------------*/

        public byte[] processImage(HttpPostedFileBase file)
        {

            Image imageFile = Image.FromStream(file.InputStream, true, true);
            MemoryStream mStream = new MemoryStream();
            imageFile.Save(mStream, ImageFormat.Jpeg);
            byte[] picBytes = mStream.ToArray();
            mStream.Dispose();
            return picBytes;

        }



        public string storeRecruitImage(HttpPostedFileBase uploadedFile, int id)
        {

            var uploadDir = "~/Content/pictures/Recruit/";
            var imagePath = Path.Combine(HttpContext.Current.Server.MapPath(uploadDir));
            var imageUrl = imagePath + id + ".jpg";
            uploadedFile.SaveAs(imageUrl);
            return imageUrl;

        }
        public string storeOfficerCadetImage(HttpPostedFileBase uploadedFile, int id)
        {

            var uploadDir = "~/Content/pictures/OfficerCadet/";
            var imagePath = Path.Combine(HttpContext.Current.Server.MapPath(uploadDir));
            var imageUrl = imagePath + id + ".jpg";
            uploadedFile.SaveAs(imageUrl);
            return imageUrl;

        }
        public string storeJCEImage(HttpPostedFileBase uploadedFile, int id)
        {

            var uploadDir = "~/Content/pictures/JCE/";
            var imagePath = Path.Combine(HttpContext.Current.Server.MapPath(uploadDir));
            var imageUrl = imagePath + id + ".jpg";
            uploadedFile.SaveAs(imageUrl);
            return imageUrl;

        }



        /*----------------------------- End Process Image -----------------------*/






    }
}