using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Discovery;
using System.Web.WebPages;

namespace GAFPAY.ViewModel
{
    public class StaffViewModel
    {
        [Display(Name = "Surname ")]
        [Required]
        [StringLength(25, ErrorMessage = "{0} must be {1} characters long.")]
        public string Surname  { get; set; }
        [Display(Name = "Othername(s) ")]
        [Required]
        [StringLength(50, ErrorMessage = "{0} must be {1} characters long.")]
        public string Othername { get; set; }
        [Display(Name = "DOB ")]
        [Required]
        public DateTime DOB { get; set; }

        [Required]
        [RegularExpression(@"\+?\(?\d{2,4}\)?[\d\s-]{3,}$", ErrorMessage = "{0} is not valid")]  
        [Display(Name = "Phone")]
        [StringLength(10,ErrorMessage = "{0} must be {1} characters long.",MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Display(Name = "Email ")]
        [StringLength(35,ErrorMessage = "{0} must be {1} characters long.")]
        public string EmailAddress { get; set; }
        [Display(Name = "Service Number ")]
        [StringLength(6,ErrorMessage = "{0} must be {1} characters long.",MinimumLength = 6)]
        [Required]
        public string ServiceNumber { get; set; }
        [Display(Name = "Hometown ")]
        public string Hometown { get; set; }
        [Display(Name = "Address")]
        [Required]
        [StringLength(30,ErrorMessage = "{0} must be {1} characters long.")]
        public string ResAddress { get; set; }
        public DateTime DateTimeInserted { get; set; }
        public DateTime DateTimeUpdated { get; set; }
        public string InsertedBy { get; set; }
        public string UpdatedBy { get; set; } 
        public bool IsMedical { get; set; }
        [Display(Name = "Region")]
        [Required] 
        public int RegionID { get; set; } 
        [Display(Name = "Unit")]
        [Required]
        public int UnitID { get; set; }
        [Required]
        [Display(Name = "Religion ")]
        public int ReligionID { get; set; }
        [Required]
        [Display(Name = "Gender ")]
        public int GenderID { get; set; }
        [Required]
        [Display(Name = "Blood Group ")]
        public int BloodGroupID { get; set; } 
        [Display(Name = "Status")]
        public int GeneralStatusID   { get; set; }
        [StringLength(15,ErrorMessage = "{0} must be between {2} and {1} characters.",MinimumLength = 10)]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }
        [Display(Name = "Branch")]
        [Required]
        public int BankID { get; set; }
        [Display(Name = "Bank")]
        public int BankNameID { get; set; }
        public SelectList RegionList { get; set; }
        public SelectList ReligionList { get; set; }
        public SelectList UnitList { get; set; }
        public SelectList GenderList { get; set; }
        public SelectList BloodGroupList { get; set; }
        public SelectList GeneralStatusList  { get; set; }
        public SelectList RankList { get; set; }
        public SelectList LevStepList { get; set; }
        public SelectList BankNameList { get; set; }
        public SelectList BankList { get; set; }    
        public string GenderName { get; set; }
        public string RegionName { get; set; }
        public string ReligionName  { get; set; }
        public string GeneralStatusName { get; set; }
        public string BloodGroupName { get; set; }




    }

    public class StaffPicture
    {
        public int StaffID { get; set; }
        public string PicturePath { get; set; }
        public string PictureName  { get; set; }
    }

    public class StaffAllowance
    {
        public int StaffAllowanceID   { get; set; }
        [Display(Name = "Amount")]
        public decimal Amount { get; set; } 
        public int Status { get; set; }
        public int AllowanceID { get; set; }
        public int StaffID { get; set; }

    }

    public class StaffDeduction
    {
        public int StaffDeductionID { get; set; }
        [Display(Name = "Total")]
        public decimal TotalAmount { get; set; }
        [Display(Name = "Deduction")]
        public decimal DeductionAmount { get; set; }
        [Display(Name = "Balance")]
        public decimal Balance { get; set; }
        public DateTime DeductionDate { get; set; }
        public int DeductionID { get; set; }
        public int StaffID { get; set; }

    }

    public class StaffPayroll
    {
        public int StaffPayrollID { get; set; }
        public DateTime PayDate { get; set; }
        public decimal ConstPay { get; set; }
        public decimal NetPay { get; set; }
        public DateTime DateTimeInserted { get; set; }
        public string InsertedBy { get; set; }
        public int Status { get; set; }
        public int StaffID { get; set; }

    }

    public class StaffPayrollAllowance
    {
        public int StaffPayrollAllowanceID { get; set; }
        public decimal Amount { get; set; }
        public int StaffPayrollID { get; set; }
        public int AllowanceID { get; set; }

    }

    public class StaffPayrollDeduction
    {
        public int StaffPayrollDeductionID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DeductionAmount { get; set; }
        public decimal Balance { get; set; }
        public DateTime DeductionDate { get; set; }
        public int DeductionID   { get; set; }
        public int StaffPayrollID { get; set; }
         
    }

    public class StaffTrialPayAllowance
    {
        public int StaffPayrollAllowanceID { get; set; }
        public decimal Amount { get; set; }
        public int StaffPayrollID { get; set; }
        public int AllowanceID { get; set; }
    }

    public class StaffTrialPayDeduction
    {
        public int StaffPayrollDeductionID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DeductionAmount { get; set; }
        public decimal Balance { get; set; }
        public DateTime DeductionDate { get; set; }
        public int DeductionID { get; set; }
        public int StaffPayrollID { get; set; }
    }


    public class JuniorCE : StaffViewModel
    {
        public int JuniorCEID { get; set; }

        [Display(Name = "Title")]
        public int TitleID { get; set; }
        [Display(Name = "Lev Step ")]
        public int CLevStepID { get; set; }

        [Display(Name = "Employment Date ")] 
        [Required]
        public DateTime DateEmployed { get; set; }
        [Display(Name = "Promotion Date ")]
        public DateTime DatePromoted { get; set; }
        [Display(Name = "SSNIT No ")]
        [Required]
        [StringLength(15,ErrorMessage = "{0} must be {1} characters long.")]
        public string SSNITNo { get; set; }
        [Display(Name = "MEDP Code")]
        [StringLength(5,ErrorMessage = "{0} must be {1} characters long.")]
        public string MedPCode { get; set; }
        [Display(Name = "Provident Fund")] 
        public int ProvidentID   { get; set; }

        public List<StaffAllowance> JuniorCEAllowancesList { get; set; }
        public List<StaffDeduction> JuniorCEDeductionsList { get; set; }
        public List<StaffPayrollAllowance> JuniorCEPayrollAllowancesList { get; set; }
        public List<StaffPayrollDeduction> JuniorCEPayrollDeductionsList { get; set; }
        public List<StaffTrialPayAllowance> JuniorCETrialPayAllowancesList { get; set; }
        public List<StaffTrialPayDeduction> JuniorCETrialPayDeductionsList { get; set; }
        public string CLevStepName { get; set; }
        public string UnitName { get; set; }
        public string UnitShort { get; set; }
        public SelectList TitleList { get; set; }
        public SelectList CLevStepList { get; set; }
        public SelectList IsMedicalList { get; set; }
        [NotMapped]
        //[Required]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }
        public SelectList ProvidentFundList { get; internal set; }
    }

    
    public class SeniorCE : StaffViewModel
    {
        public int SeniorCEID { get; set; }

        [Display(Name = "Title")]
        public int TitleID { get; set; }
        [Display(Name = "Lev Step ")]
        public int CLevStep { get; set; }

        [Display(Name = "Employment Date ")]
        [Required]
        public DateTime DateEmployed { get; set; }
        [Display(Name = "Promotion Date ")]
        public DateTime DatePromoted { get; set; }
        [Display(Name = "SSNIT No ")]
        [Required]
        [StringLength(15, ErrorMessage = "{0} must be {1} characters long.")]
        public string SSNITNo { get; set; }
        [Display(Name = "MEDP Code")]
        [StringLength(5, ErrorMessage = "{0} must be {1} characters long.")]
        public string MedPCode { get; set; }

        public List<StaffAllowance> SeniorCEAllowancesList { get; set; }
        public List<StaffDeduction> SeniorCEDeductionsList { get; set; }
        public List<StaffPayrollAllowance> SeniorCEPayrollAllowancesList { get; set; }
        public List<StaffPayrollDeduction> SeniorCEPayrollDeductionsList { get; set; }
        public List<StaffTrialPayAllowance> SeniorCETrialPayAllowancesList { get; set; }
        public List<StaffTrialPayDeduction> SeniorCETrialPayDeductionsList { get; set; }
        public string UnitName { get; set; }
        public string UnitShort { get; set; }
        public string CLevStepName { get; set; }
        [NotMapped]
        //[Required]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }
    }

    public class Soldier : StaffViewModel
    {
        public int SoldierID { get; set; }
        [Display(Name = "Rank ")]
        public int RankID { get; set; }
        [Display(Name = "Lev Step ")]
        public int MLevStepID { get; set; }
        [Display(Name = "Passout Date ")]
        [Required]
        public DateTime DatePassout { get; set; }
        [Display(Name = "Promotion Date ")]
        public DateTime DatePromoted { get; set; }
        [Display(Name = "Service ")]
        public int ServiceID { get; set; }
        [Display(Name = "MEDP Code")]
        [StringLength(5, ErrorMessage = "{0} must be {1} characters long.")]
        public string MedPCode { get; set; }

        public List<StaffAllowance> SoldierllowancesList { get; set; }
        public List<StaffDeduction> SoldierDeductionsList { get; set; }
        public List<StaffPayrollAllowance>SoldierPayrollAllowancesList { get; set; }
        public List<StaffPayrollDeduction> SoldierayrollDeductionsList { get; set; }
        public List<StaffTrialPayAllowance> SoldierTrialPayAllowancesList { get; set; }
        public List<StaffTrialPayDeduction> SoldierTrialPayDeductionsList { get; set; }
        public string UnitName { get; set; }
        public string MLevStepName { get; set; }
        public string UnitShort { get; set; }
        [NotMapped]
        //[Required]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }
    }

    public class Officer : StaffViewModel
    {
        public int OfficerID { get; set; }
        [Display(Name = "Rank ")]
        [Required]
        public int RankID { get; set; }
        [Display(Name = "Lev Step ")]
        [Required]
        public int MLevStepID { get; set; }
        [Required]
        [Display(Name = "Service ")]
        public int ServiceID { get; set; } 
        [Display(Name = "Commission Date")]
        [Required]
        public DateTime DateCommission { get; set; }
        [Display(Name = "Promotion Date ")]

        public DateTime DatePromoted { get; set; }
        [Display(Name = "MEDP Code")]
        [StringLength(5,ErrorMessage = "{0} must be {1} characters long.")]
        public string MedPCode { get; set; }

        public List<StaffAllowance> OfficerAllowancesList { get; set; }
        public List<StaffDeduction> OfficerDeductionsList { get; set; }
        public List<StaffPayrollAllowance> OfficerPayrollAllowancesList { get; set; }
        public List<StaffPayrollDeduction> OfficerPayrollDeductionsList { get; set; }
        public List<StaffTrialPayAllowance> OfficerTrialPayAllowancesList { get; set; }
        public List<StaffTrialPayDeduction> OfficerTrialPayDeductionsList { get; set; }
        public string MLevStepName { get; set; }
        public string UnitName { get; set; }
        public string UnitShort { get; set; }
        [NotMapped]
        //[Required]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }
    }

    public class Recruit : StaffViewModel
    {
        
        public int RecruitID { get; set; }
        [Display(Name = "Rank ")]
        [Required]
        public int RankID { get; set; }
        [Required]
        [Display(Name = "Service ")]
        public int ServiceID { get; set; }
        [Display(Name = "Lev Step ")]
        [Required]
        public int MLevSetpID { get; set; }
        [Required]
        [Display(Name = "Training Center")]
        public int TrainingCenterID { get; set; }
        [Required]
        [Display(Name = "Recruit Course")]
        public int RecruitCourseID { get; set; }
        [Required]
        [Display(Name = "Date Start")]
        public DateTime DateRecruitStart { get; set; }
        [Display(Name = "Date End")]
        public DateTime DateRecruitEnd { get; set; }
         
        public string TCName { get; set; }
        public string TCLocation { get; set; }
        public string RankName { get; set; }
        public string RCName { get; set; }
        public string MLevStepName { get; set; }
        public string ServiceName { get; set; }
        public SelectList ServiceList { get; set; }
        public string ImageName { get; set; }
        public SelectList RecruitCourseList { get; set; }
        [NotMapped]
        //[Required]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }

        public string RankNameShort { get; set; }
    }

    public class RecruitPay : Recruit
    {
        public int PayID { get; set; }
        public DateTime PayDate { get; set; } 

    }
    public class OfficerCadetPay : OfficerCadet
    {
        public int PayID { get; set; }
        public DateTime PayDate { get; set; } 

    }

    public class RecruitBank
    {
        
        public int RecruitID { get; set; }
        [Display(Name = "Branch")]
        [Required]
        public int BankID { get; set; }
        [Display(Name = "Bank")]
        [Required]
        public int BankNameID { get; set; }

        public SelectList BankList { get; set; }
        public SelectList BankNameList { get; set; }
          

        [Display(Name = "Account Number")]
        [Required]
        [StringLength(15,ErrorMessage = "{0} must be between {2} and {1} characters.",MinimumLength = 10)]
        public string AccountNumber { get; set; }
        public string RecruitImageX { get; set; }

    }
    public class OfficerCadetBank
    {

        public int OfficerCadetID  { get; set; }
        [Display(Name = "Branch")]
        [Required]
        public int BankID { get; set; }
        [Display(Name = "Bank")]
        [Required]
        public int BankNameID { get; set; }

        public SelectList BankList { get; set; }
        public SelectList BankNameList { get; set; }


        [Display(Name = "Account Number")]
        [Required]
        [StringLength(15, ErrorMessage = "{0} must be between {2} and {1} characters.", MinimumLength = 10)]
        public string AccountNumber { get; set; }
         
    }



    public class OfficerCadet : StaffViewModel
    {
        public int OfficerCadetID { get; set; }
        [Display(Name = "Rank")]
        [Required]
        public int RankID { get; set; }
        [Display(Name = "Service")]
        [Required]
        public int ServiceID { get; set; }
        [Display(Name = "Lev Step")]
        [Required]
        public int MLevStepID { get; set; }
        [Display(Name = "Commission Type")]
        [Required]
        public int CommissionTypeID { get; set; }
        [Display(Name = "Intake")]
        [Required]
        public int OfficerIntake { get; set; }
        public DateTime OfficerEndDate { get; internal set; }
        [Display(Name = "Start Date")]
        public DateTime OfficerStartDate { get; set; }
        public string CommissionTypeName { get; set; }
        public string OfficerIntakeName { get; set; }
        public string MLevStepName { get; set; }
        public string RankNameShort { get; set; }
        public string ServiceName { get; set; }
        public string RankName { get; set; }
        public SelectList ServiceList { get; set; }
        public string ImageName { get; set; }
        public SelectList OfficerIntakeList { get; set; }
        public SelectList CommissionTypeList     { get; set; }
        [NotMapped]
        //[Required]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }
    }

}