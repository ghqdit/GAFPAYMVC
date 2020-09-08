using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAFPAY.Models;
using GAFPAY.ViewModel;

namespace GAFPAY.ViewData
{
    public class SettingsViewData
    { 
        private DBGAFPAYEntities db = new DBGAFPAYEntities();

        /*----------------------------- Start Generate dropdowns SelectLists -----------------------*/

        public SelectList getGenders()
        {
            var genders = db.GENDER.Where(a=>a.STATUS==1).OrderBy(a=>a.GENDERNAME).ToList();
            SelectList genderList = new SelectList(genders, "GenderID", "GenderName");
            return genderList; 
        }

        public SelectList getYesNo()
        {
            Dictionary<int, string> response = new Dictionary<int, string>();  
            response.Add(0,"No");
            response.Add(1,"Yes");
            SelectList resp = new SelectList(response, "Key", "Value");
            return resp; 

        }


        public SelectList getTitles()
        {
            var titles = db.TITLE.Where(a=>a.STATUS==1).OrderBy(a=>a.TITLENAME).ToList();
            SelectList getTitlesList=new SelectList(titles,"TitleID","TitleName");
            return getTitlesList;
        }

        public SelectList getServices()
        {
            var services = db.SERVICE.Where(a=>a.STATUS==1).OrderBy(a=>a.SERVICENAME).ToList();
            SelectList getServicesList=new SelectList(services,"ServiceID","ServiceName");
            return getServicesList;
        }

        public SelectList getBanks(int id)
        {
            var banks = db.BANK.Where(a=>a.STATUS==1 && a.BANKNAMEID==id).Select(a=> new {a.BANKID,a.BANKBRANCH}).OrderBy(a=>a.BANKBRANCH).ToList();
            SelectList getBanksList=new SelectList(banks,"BankID","BankBranch");
            return getBanksList;
        }
        public SelectList getProvidentFund()
        {
            var provident = db.PROVIDENTFUND.Where(a => a.STATUS == 1).OrderBy(a => a.PROVIDENTRATE).ToList();
            SelectList getProvidentList = new SelectList(provident, "ProvidentFundID", "ProvidentRate");
            return getProvidentList;
        }

        public SelectList getBankNames()
        {
            var bankMain = db.BANKNAME.Where(a=>a.STATUS==1).OrderBy(a=>a.BANKNAMEX).ToList();
            SelectList getBankMainList=new SelectList(bankMain,"BankNameID","BankNameX");
            return getBankMainList;
        }

        public SelectList getRecruitRanks()
        {
            var ranks = db.RANK.Where(a=>a.STATUS==3).OrderBy(a=>a.RANKNAME).ToList();
            SelectList getRankList=new SelectList(ranks,"RankID","RankName");
            return getRankList;
        }
        public SelectList getOfficerCadetRanks(int id)
        {
            var ranks = db.RANK.Where(a=>a.STATUS==2 && a.SERVICEID==id).OrderBy(a=>a.RANKNAME).ToList();
            SelectList getRankList = new SelectList(ranks, "RankID", "RankName");
            return getRankList;
        }

        public SelectList getSoldierRanks(int id)
        {
            var ranks = db.RANK.Where(a=>a.ISOFFICER==false && a.SERVICEID==id && a.STATUS==1).OrderBy(a=>a.RANKNAME).ToList();
            SelectList getRankList = new SelectList(ranks, "RankID", "RankName");
            return getRankList;
        }
        public SelectList getOfficerRanks(int id)
        {
            var ranks = db.RANK.Where(a=>a.ISOFFICER && a.SERVICEID==id && a.STATUS==1).OrderBy(a=>a.RANKNAME).ToList();
            SelectList getRankList = new SelectList(ranks, "RankID", "RankName");
            return getRankList;
        }

        public SelectList getGeneralStatus()
        {
            var status = db.GENERALSTATUS.Where(a => a.STATUS == 1).OrderBy(a=>a.GSNAME).ToList();
            SelectList getStatusList=new SelectList(status,"GeneralStatusID","GSName");
            return getStatusList;
        }

        public SelectList getRegion()
        {
            var region = db.REGION.Where(a => a.STATUS == 1).OrderBy(a=>a.REGIONNAME).ToList();
            SelectList getRegionList=new SelectList(region,"RegionID","RegionName");
            return getRegionList;
        }
        public SelectList getReligion()
        {
            var religion = db.RELIGION.Where(a => a.STATUS == 1).OrderBy(a=>a.RELIGIONNAME).ToList();
            SelectList getReligionList = new SelectList(religion, "ReligionID", "ReligionName");
            return getReligionList;
        }

        public SelectList getBloodGroups()
        {
            var bloodGroups = db.BLOODGROUP.Where(a=>a.STATUS==1).OrderBy(a=>a.BLOODGROUPNAME).ToList();
            SelectList getBlodGroupList = new SelectList(bloodGroups, "BloodGroupID", "BloodGroupName");
            return getBlodGroupList;
        }

        //public SelectList getOfficerIntake()
        //{
        //    var officerIntake = db.OFFICERINTAKE.Where(a => a.STATUS == 1).OrderBy(a=>a.OFFICERINTAKENAME).ToList();
        //    SelectList getOfficerIntakeList=new SelectList(officerIntake,"OfficerIntakeID","OfficerIntakeName");
        //    return getOfficerIntakeList;
        //}

        public SelectList getCommissionType()
        {
            var commissionType = db.COMMISSIONTYPE.Where(a => a.STATUS == 1).OrderBy(a=>a.COMMISSIONTYPENAME).ToList();
            SelectList getCommissionTypeList=new SelectList(commissionType,"CommissionTypeID","CommissionTypeName");
            return getCommissionTypeList;
        }

        public SelectList getRecruitCourse(int id)
        {
            var recruitCourse = db.RECRUITCOURSE.Where(a => a.STATUS == 1 && a.SERVICEID==id).Select(a => new { a.RCID, a.RCNAME }).OrderBy(a=>a.RCNAME).ToList();
            SelectList getRecruitCourseList=new SelectList(recruitCourse,"RCID","RCName");
            return getRecruitCourseList;

        }

        public SelectList getTrainingCenter(int id)
        {
            //var vehicleModel = db.VEHICLEMODEL.Where(a => a.BRANDID == id)..ToList();
            var trainingCenter = db.TRAININGCENTER.Where(a => a.STATUS == 1 && a.SERVICEID == id).Select(a => new { a.TCID, a.TCNAME }).OrderBy(a=>a.TCNAME).ToList();
            SelectList getTrainingCenterList=new SelectList(trainingCenter,"TCID","TCName");
            return getTrainingCenterList;
            
        }
         


        public SelectList getDeductionClass()
        {
            var deductionClass = db.DEDUCTIONCLASS.Where(a => a.STATUS == 1).OrderBy(a=>a.DEDUCTIONCLASSNAME).ToList();
            SelectList getDeductionClassList=new SelectList(deductionClass,"DeductionClassID","DeductionClassName");
            return getDeductionClassList;
        }

        public SelectList getRecruitLevStep()
        {
            var mLevStep = db.MILITARYLEVSTEP.Where(a => a.STATUS == 1).OrderBy(a=>a.LEVSTEPNAME).ToList();
            SelectList getLevStep=new SelectList(mLevStep,"MilitaryLevStepID","LevStepName");
            return getLevStep;
        }
        public SelectList getOfficerCadetLevStep()
        {
            var mLevStep = db.MILITARYLEVSTEP.Where(a => a.STATUS == 1).OrderBy(a=>a.LEVSTEPNAME).ToList();
            SelectList getLevStep = new SelectList(mLevStep, "MilitaryLevStepID", "LevStepName");
            return getLevStep;
        }

        public SelectList getSoldierLevStep(int id)
        {
            var mLevStep = db.MILITARYLEVSTEP.Where(a => a.STATUS == 1 && a.ISOFFICER==false).OrderBy(a=>a.LEVSTEPNAME).ToList();            SelectList getLevStep = new SelectList(mLevStep, "MilitaryLevStepID", "LevStepName");
            return getLevStep;
        }
        public SelectList getOfficerLevStep(int id)
        {
            var mLevStep = db.MILITARYLEVSTEP.Where(a => a.STATUS == 1  && a.ISOFFICER).OrderBy(a=>a.LEVSTEPNAME).ToList();
            SelectList getLevStep = new SelectList(mLevStep, "MilitaryLevStepID", "LevStepName");
            return getLevStep;
        }

        public SelectList getJCELevStep()
        {
            var cLevStep = db.CIVILIANLEVSTEP.Where(a => a.STATUS == 1 && a.ISSENIOR==false).OrderBy(a=>a.LEVSTEPNAME).ToList();
            SelectList getLevStep = new SelectList(cLevStep, "CivilianLevStepID", "LevStepName");
            return getLevStep;
        }

        public SelectList getSCELevStep()
        {
            var cLevStep = db.CIVILIANLEVSTEP.Where(a => a.STATUS == 1 && a.ISSENIOR).OrderBy(a=>a.LEVSTEPNAME).ToList();
            SelectList getLevStep = new SelectList(cLevStep, "CivilianLevStepID", "LevStepName");
            return getLevStep;
        }
        public SelectList getUnits()
        {
            var units = db.UNIT.Where(a => a.STATUS == 1).OrderBy(a=>a.UNITNAME).ToList();
            SelectList getUnit = new SelectList(units, "UnitID", "UnitName");
            return getUnit;
        }



        /*----------------------------- End Generate dropdowns SelectLists-----------------------*/



        /*----------------------------- Start Generate List for Views under settings-----------------------*/

        public List<Unit> GetUnitList()
        {
            var unit = db.UNIT.Where(d => d.STATUS==1).Select(d => new Unit
            {
                UnitID = d.UNITID,
                UnitName = d.UNITNAME,
                UnitShort = d.UNITSHORT,
                Status = d.STATUS
            }).OrderBy(d =>d.UnitName).ToList();
            return unit;
        }


        public List<CivilianLevStep> GetCivilianLevStepsList()
        {
            var cLevStep = db.CIVILIANLEVSTEP.Where(a => a.STATUS == 1).Select(a => new CivilianLevStep
            {
                CivilianLevStepID = a.CIVILIANLEVSTEPID,
                LevStepName = a.LEVSTEPNAME,
                ConstPay = a.CONSTPAY,
                Status = a.STATUS,
                IsSenior = a.ISSENIOR
            }).OrderBy(a => a.LevStepName).ToList();
            return cLevStep;
        }

        public List<MilitaryLevStep> GetMilitaryLevStepsList()
        {
            var mLevStep = db.MILITARYLEVSTEP.Where(a => a.STATUS == 1).Select(a => new MilitaryLevStep
            {
                MilitaryLevStepID = a.MILITARYLEVSTEPID,
                LevStepName = a.LEVSTEPNAME,
                ConstPay = a.CONSTPAY,
                Status = a.STATUS,
                IsOfficer = a.ISOFFICER 
            }).OrderBy(a => a.LevStepName).ToList();
            return mLevStep;
        }
        public List<ProvidentFund> GetProvidentFundList()
        {
            var mLevStep = db.PROVIDENTFUND.Where(a => a.STATUS == 1).Select(a => new ProvidentFund()
            {
               ProvidentFundID = a.PROVIDENTFUNDID,
               ProvidentRate = a.PROVIDENTRATE
                
            }).OrderBy(a => a.ProvidentRate).ToList();
            return mLevStep;
        }

        public List<Title> GetTitleList()
        {
            var title = db.TITLE.Select(a => new Title
            {
                TitleID = a.TITLEID,
                TitleName = a.TITLENAME
            }).OrderBy(a => a.TitleName).ToList();
            return title;
        }
         public List<Gender> GetGenderList()
        {
            var gender = db.GENDER.Select(a => new Gender()
            {
                GenderID = a.GENDERID,
                GenderName = a.GENDERNAME
            }).OrderBy(a => a.GenderName).ToList();
            return gender;
        }

        public List<Service> GetServiceList()
        {
            var service = db.SERVICE.Where(a => a.STATUS == 1).Select(a => new Service
            {
                ServiceID = a.SERVICEID,
                ServiceName = a.SERVICENAME,
                ServiceShort = a.SERVICESHORTNAME,
                Status = a.STATUS
            }).OrderBy(a => a.ServiceName).ToList();
            return service;
        }

        public List<Bank> GetBankList()
        {
            var bank = db.BANK.Where(a => a.STATUS == 1).Select(a => new Bank
            {
                BankID = a.BANKID,
                BankBranch = a.BANKBRANCH,
                BankNameID = a.BANKNAMEID,
                BankCode = a.BANKCODE,
                Status = a.STATUS,
                BankNameX = a.BANKNAME.BANKNAMEX
            }).OrderBy(a => a.BankNameX).ToList();
            return bank;
        }

        public List<BankName> GetBankNameXList()
        {
            var bankName = db.BANKNAME.Where(a => a.STATUS == 1).Select(a => new BankName
            {
                BankNameID = a.BANKNAMEID,
                BankNameX = a.BANKNAMEX,
                ShortName = a.BANKSHORT,
                Status = a.STATUS
            }).OrderBy(a => a.BankNameX).ToList();
            return bankName;

        }

        public List<Rank> GetRankList()
        {
            var ranks = db.RANK.Where(a => a.STATUS == 1).Select(a => new Rank
            {
                RankID = a.RANKID,
                RankName = a.RANKNAME,
                RankShort = a.RANKSHORT,
                Status = a.STATUS,
                ServiceName = a.SERVICE.SERVICENAME,
                ServiceNameShort = a.SERVICE.SERVICESHORTNAME
            }).OrderBy(a => a.RankName).ToList();
            return ranks;
        }

        public List<GeneralStatus> GetGeneralStatusList()
        {
            var generalStatus = db.GENERALSTATUS.Where(a => a.STATUS == 1).Select(a => new GeneralStatus
            {
                GeneralStatusID = a.GENERALSTATUSID,
                GSName = a.GSNAME,
                GSShort = a.GSSHORT,
                Status = a.STATUS,
                Rate = a.RATE
            }).OrderBy(a => a.GSName).ToList();
            return generalStatus;
        }


        public List<RegionX> GetRegionList()
        {
            var regions = db.REGION.Where(a => a.STATUS == 1).Select(a => new RegionX
            {
                RegionID = a.REGIONID,
                RegionName = a.REGIONNAME,
                RegionShort = a.REGIONSHORT,
                Status = a.STATUS

            }).OrderBy(a => a.RegionName).ToList();
            return regions;
        }

        public List<Religion> GetReligionList()
        {
            var religions = db.RELIGION.Where(a => a.STATUS == 1).Select(a => new Religion()
            {
                ReligionID = a.RELIGIONID,
                ReligionName = a.RELIGIONNAME,
                Status = a.STATUS
            }).OrderBy(a => a.ReligionName).ToList();
            return religions;
        }

        public List<BloodGroup> GetBloodGroupList()
        {
            var bloodGroups = db.BLOODGROUP.Select(a => new BloodGroup()
            {
                BloodGroupID = a.BLOODGROUPID,
                BloodGroupName = a.BLOODGROUPNAME
            }).OrderBy(a => a.BloodGroupName).ToList();
            return bloodGroups;
        }

        //public List<OfficerIntake> GetOfficerIntakeList()
        //{
        //    var intakes = db.OFFICERINTAKE.Where(a => a.STATUS == 1).Select(a => new OfficerIntake()
        //    {
        //        OfficerIntakeID = a.OFFICERINTAKEID,
        //        OfficerIntakeName = a.OFFICERINTAKENAME,
        //        Status = a.STATUS
        //    }).OrderBy(a => a.OfficerIntakeName).ToList();
        //    return intakes;
        //}

        public List<CommissionType> GetCommissionTypeList()
        {
            var cType = db.COMMISSIONTYPE.Where(a => a.STATUS == 1).Select(a => new CommissionType()
            {
                CommissionTypeID = a.COMMISSIONTYPEID,
                CommissionTypeName = a.COMMISSIONTYPENAME,
                CommissionShort = a.COMMISSIONTYPESHORT,
                Status = a.STATUS
            }).OrderBy(a => a.CommissionTypeName).ToList();
            return cType; 
        }

        public List<RecruitCourse> GetRecruitCourseList()
        {
            var recCourses = db.RECRUITCOURSE.Where(a => a.STATUS == 1).Select(a => new RecruitCourse()
            {
                RecruitCourseID = a.RCID,
                RecruitCourseName = a.RCNAME,
                ServiceName =a.SERVICE.SERVICENAME, 
                Status = a.STATUS
            }).OrderBy(a => a.ServiceName).ToList();
            return recCourses;
        }

        public List<TrainingCenter> GetTrainingCenterList()
        {
            var trainingCenters = db.TRAININGCENTER.Where(a => a.STATUS == 1).Select(a => new TrainingCenter()
            {
                TrainingCenterID = a.TCID,
                TrainingCenterName = a.TCNAME,
                Location = a.LOCATION,
                Status = a.STATUS,
                ServiceID = a.SERVICEID,
                ServiceName = a.SERVICE.SERVICENAME,
                ServiceNameShort = a.SERVICE.SERVICESHORTNAME

            }).OrderBy(a => a.ServiceName).ToList();
            return trainingCenters;
        }

        public List<Allowance> GetAllowanceList()
        {
            var allowances = db.ALLOWANCE.Where(a => a.STATUS == 1).Select(a => new Allowance()
            {
                AllowanceID = a.ALLOWANCEID,
                AllowanceName = a.ALLOWANCENAME,
                IsTaxable = a.ISTAXABLE,
                Status = a.STATUS
            }).OrderBy(a => a.AllowanceName).ToList();
            return allowances;
        }

        public List<Deduction> GetDeductionList()
        {
            var deductions = db.DEDUCTION.Where(a => a.STATUS == 1).Select(a => new Deduction()
            {
                DeductionID = a.DEDUCTIONID,
                DeductionName = a.DEDUCTIONNAME,
                Status = a.STATUS,
                DeductionClassID = a.DEDUCTIONCLASSID,
                DeductionClassName = a.DEDUCTIONCLASS.DEDUCTIONCLASSNAME,
            }).OrderBy(a => a.DeductionName).ToList();
            return deductions;
        }

        public List<DeductionClass> GetDeductionClassList()
        {
            var deductionClass = db.DEDUCTIONCLASS.Where(a => a.STATUS == 1).Select(a => new DeductionClass()
            {
                DeductionClassID = a.DEDUCTIONCLASSID,
                DeductionClassName = a.DEDUCTIONCLASSNAME,
                Status = a.STATUS
            }).OrderBy(a => a.DeductionClassName).ToList();
            return deductionClass;
        } 




        /*----------------------------- End Generate List for Views under settings-----------------------*/





        /*----------------------------- Start Data Processing under settings-----------------------*/



        /*----------------------------- End Data Processing under settings-----------------------*/

        public static KeyValuePair<bool,string> SaveTitle(Title data)
        {
            KeyValuePair<bool,string> report=new KeyValuePair<bool, string>(true,"added Sucessful");
            
            throw new NotImplementedException();
        }
    }
}