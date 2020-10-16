using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Web;
using System.Web.Mvc;

namespace GAFPAY.ViewModel
{
    public class Unit
    { 
        public int UnitID { get; set; }
        [Required]
        [Display(Name = "Unit Name *")] 
        [StringLength(50,ErrorMessage = "{0} must be {2} to {1} charaters long.",MinimumLength = 6)]
        public string UnitName { get; set; }
        [Required]
        [Display(Name = "Short Name")]
        [StringLength(10,ErrorMessage = "{0} must be {1} characters long.")]
        public string UnitShort { get; set; }
        public int Status { get; set; }
        public bool IsEdit { get; set; }
    }

    public class CivilianLevStep
    {
        public int CivilianLevStepID { get; set; }
        [Required]
        [Display(Name = "Lev Step")]
        [StringLength(6,ErrorMessage = "{0} must be {1} characters long.")]
        public string LevStepName { get; set; }
        [Display(Name = "Const Pay")]
        [Required]
        public decimal ConstPay { get; set; }
        public bool IsSenior { get; set; }
        [Display(Name = "Is Senior?")]
        [Required]
        public int IsSeniorID { get; set; }
        public int Status { get; set; }
        public SelectList IsSeniorList { get; set; }
        public bool IsEdit { get; set; }
    }

    public class Title
    {
        public int TitleID { get; set; }
        [Display(Name = "Title")]
        [Required]
        [StringLength(10,ErrorMessage = "{0} must be {1} characters long.")]
        public string TitleName { get; set; }

        public bool IsEdit { get; set; }   
    }

    public class Gender
    {
        public int GenderID { get; set; }
        [Display(Name = "Gender")]
        [Required]
        [StringLength(8,ErrorMessage = "{0} must be {1} characters long.")]
        public string GenderName { get; set; }
        public bool IsEdit { get; set; }

    }

    public class Service
    {
        public int ServiceID { get; set; }
        [Display(Name = "Service")]
        [Required]
        public string ServiceName { get; set; }
        [Display(Name = "Short Name")]
        [Required]
        public string ServiceShort { get; set; }
        public int Status { get; set; }
        public bool IsEdit { get; set; }
    }

    public class MilitaryLevStep
    {
        
        public int MilitaryLevStepID { get; set; }
        [Display(Name = "Lev Step")]
        [Required]
        [StringLength(15,ErrorMessage = "{0} must be {1} characters long")]
        public string LevStepName { get; set; }
        [Display(Name = "Const Pay")]
        [Required]
        public decimal ConstPay { get; set; }
        public int Status { get; set; }
        public bool IsOfficer { get; set; }
        [Display(Name = "Is Officer?")]
        [Required]
        public int IsOfficerID { get; set; }
        [Display(Name = "Service")]
        [Required]
        public int ServiceID  { get; set; } 
        public SelectList ServiceList { get; set; }
        public string ServiceName { get; set; }
        public string ServiceNameShort { get; set; }
        public SelectList IsOfficerList { get; set; }
        public bool IsEdit { get; set; }
    }

    public class Bank
    {
        public int BankID { get; set; }
        [Display(Name = "Branch")]
        [Required]
        [StringLength(25,ErrorMessage = "{0} must be {1} characters long.")]
        public string BankBranch { get; set; }
        [Display(Name = "Code")]
        [StringLength(6,ErrorMessage = "{0} must be {1} characters long.")]
        public string BankCode { get; set; }
        public int Status { get; set; }
        [Display(Name = "Bank")]
        [Required]
        public int BankNameID { get; set; }
        public SelectList BankNameList { get; set; }
        public string BankNameX { get; set; }
        public bool IsEdit { get; set; }
        public SelectList BankBranchList { get; set; }
    }


    public class BankName
    {
        public int BankNameID { get; set; }
        [Display(Name = "Bank")]
        [Required]
        [StringLength(40,ErrorMessage = "{0} must be {1} characters long.")]
        public string BankNameX { get; set; }
        public int Status { get; set; }
        [Display(Name = "ABB")]
        [Required]
        [StringLength(10,ErrorMessage = "{0} must be {1} characters long.")]
        public string ShortName { get; set; }

        public bool IsEdit { get; set; }
    }

    public class Rank
    {
        public int RankID { get; set; }
        [Display(Name = "Rank")]
        [Required]
        [StringLength(25,ErrorMessage = "{0} must be {1} characters long.")]
        public string RankName { get; set; }
        [Display(Name = "ABB")]
        [Required]
        [StringLength(10,ErrorMessage = "{0} must be {1} characters long.")]
        public string RankShort { get; set; }
        public int Status { get; set; }
        [Display(Name = "Is Officer?")]
        public int IsOfficerID { get; set; }

        public bool IsOfficer { get; set; }
        [Display(Name = "Service")]
        public int ServiceID { get; set; } 

        public SelectList ServiceList { get; set; } 
        public string ServiceName { get; set; }
        public string ServiceNameShort { get; set; }
        public bool IsEdit { get; set; }
        public SelectList IsOfficerList { get; set; }
    }

    public class GeneralStatus
    {
        public int GeneralStatusID { get; set; }
        [Display(Name = "Status")]
        [Required]
        [StringLength(15,ErrorMessage = "{0} must be {1} characters long.")]
        public string GSName { get; set; }
        [Display(Name = "ABB")]
        [Required]
        [StringLength(1, ErrorMessage = "{0} must be {1} characters long.")]
        public string GSShort { get; set; }
        [Display(Name = "Rate")]
        [Required] 
        public double Rate { get; set; }
        public int Status { get; set; }
        public bool IsEdit { get; set; }
    }

    public class RegionX
    {
        public int RegionID { get; set; }
        [Display(Name = "Region")]
        [Required]
        [StringLength(20, ErrorMessage = "{0} must be {1} characters long.")]
        public string RegionName { get; set; }
        [Display(Name = "ABB")]
        [Required]
        [StringLength(2, ErrorMessage = "{0} must be {1} characters long.")]
        public string RegionShort { get; set; }
        public int Status { get; set; }
        public bool IsEdit { get; set; }
    }

    public class Religion
    {
        public int ReligionID { get; set; }
        [Display(Name = "Religion")]
        [StringLength(10,ErrorMessage = "{0} must be {1} characters long.")]
        [Required]
        public string ReligionName { get; set; }

        public int Status { get; set; }
        public bool IsEdit { get; set; }
    }

    public class ProvidentFund
    {
        public int ProvidentFundID { get; set; }
        public double ProvidentRate { get; set; }
        public bool IsEdit { get; set; }
    }

    public class Grade
    {
        public int GradeID   { get; set; }

        [Display(Name = "Grade")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(50, ErrorMessage = "{0} must be {1} characters long.")]
        public string GradeName { get; set; }
        public int Status { get; set; }
        public bool IsEdit { get; set; }
    }

    public class BloodGroup
    {
        public int BloodGroupID { get; set; }
        [Display(Name = "Blood Group")]
        [StringLength(5,ErrorMessage = "{0} must be {1} characters long.")]
        [Required]
        public string BloodGroupName { get; set; }

        public bool IsEdit { get; set; }
    }

    public class OfficerIntake
    {
        public int OfficerIntakeID { get; set; }
        [Display(Name = "Intake")]
        [Required]
        [StringLength(10,ErrorMessage = "{0} must be {1} characters long.")]
        public string OfficerIntakeName { get; set; }
        public int Status { get; set; }
        public bool IsEdit { get; set; }
        public string CommissionTypeName { get; set; }
        [Display(Name = "Commission Type")]
        [Required]
        public int CommissionTypeID { get; set; }

        public SelectList CommissionTypeList { get; set; }
        public SelectList OfficerIntakeList { get; set; }
    }

    public class CommissionType
    {
        public int CommissionTypeID { get; set; }
        [Display(Name = "Commission Type")]
        [Required]
        [StringLength(40,ErrorMessage ="{0} must be {1} characters long." )]
        public string CommissionTypeName { get; set; }
        [Display(Name = "ABB")]
        [Required]
        [StringLength(6,ErrorMessage = "{0} must be {1} characters long.")]
        public string CommissionShort { get; set; }
        public int Status { get; set; }
        public bool IsEdit { get; set; }
    }

    public class RecruitCourse
    {
        public int RecruitCourseID { get; set; }
        [Display(Name = "Course")]
        [Required]
        [StringLength(15,ErrorMessage = "{0} must be {1} characters long.")]
        public string RecruitCourseName { get; set; }
        public int Status { get; set; }
        public bool IsEdit { get; set; }

        [Display(Name = "Service")]
        [Required]
        public int ServiceID { get; set; }
        public SelectList RecruitCourseNameList { get; set; }
        public SelectList ServiceList { get; set; }
        public string ServiceName { get; set; }
    }



    public class TrainingCenter
    {
        public int TrainingCenterID { get; set; }
        [Display(Name = "Training Center")]
        [Required]
        [StringLength(25,ErrorMessage = "{0} must be {1} characters long.")]
        public string TrainingCenterName { get; set; }
        [Display(Name = "Location")]
        [Required]
        [StringLength(25, ErrorMessage = "{0} must be {1} characters long.")]
        public string Location { get; set; }
        public int  Status  { get; set; }
        [Display(Name = "Service")]
        [Required] 
        public int ServiceID { get; set; } 
        public SelectList ServiceList { get; set; }
        public string ServiceName { get; set; }
        public string ServiceNameShort { get; set; }
        public bool IsEdit { get; set; }
        public SelectList TrainingCenterList { get; set; }
    }

    public class Allowance
    {
        
        public int AllowanceID { get; set; }
        [Display(Name = "Allowance")]
        [Required]
        [StringLength(25, ErrorMessage = "{0} must be {1} characters long.")]
        public string AllowanceName { get; set; }
        [Display(Name = "Taxable")]
        [Required] 
        public int TaxableID { get; set; }

        public bool IsTaxable { get; set; }
        public int Status { get; set; } 

        public SelectList IsTaxableList { get; set; }
        public bool IsEdit { get; set; }
    }

    public class Deduction
    {
        public int DeductionID  { get; set; }
        [Display(Name = "Deduction")]
        [Required]
        [StringLength(30, ErrorMessage = "{0} must be {1} characters long.")]
        public string DeductionName { get; set; }
        public int Status { get; set; }
        [Display(Name = "Deduction Class")]
        [Required]
        public int DeductionClassID  { get; set; } 
        public SelectList DeductionClassList { get; set; }
        public string DeductionClassName { get; set; }
        public bool IsEdit { get; set; }
    }

    public class DeductionClass
    {
        public int DeductionClassID { get; set; }
        [Display(Name = "Deduction Classs ")]
        [StringLength(25,ErrorMessage = "{0} must be {1} characters long.")]
        public string DeductionClassName { get; set; }
        public int Status { get; set; }
        public bool IsEdit { get; set; }
    }


}