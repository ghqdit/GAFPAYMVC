using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;

namespace GAFPAY.ViewModel
{
    public class TrialPay
    {
          
        [Display(Name = "Month")]
        public int MonthID { get; set; }  
        public int TrialPayID { get; set; }
        public DateTime TrialPayDate { get; set; }
        public DateTime DateTimeInserted { get; set; }
        public string InsertedBy { get; set; }
        public SelectList MonthList { get; set; }
        public SelectList StaffList { get; set; } 
        

    }

    public class GeneralTrialPay : TrialPay
    {
        public decimal TotalRecruit { get; set; }
        public decimal TotalOC { get; set; }
        public decimal TotalJCE { get; set; }
        public decimal TotalSCE { get; set; } 
    
    }

    public class RecruitTrialPay : TrialPay
    {
        [Display(Name = "Recruit")]
        public int RecruitID { get; set; }

    }

    public class JCETrialPay : TrialPay
    {
        [Display(Name = "Junior CE")]
        public int JCEID { get; set; }

    }
    public class SCETrialPay : TrialPay
    {
        [Display(Name = "Senior CE")]
        public int SCEID { get; set; }

    }

    public class OcTrialPay : TrialPay
    {
        [Display(Name = "Officer Cadet")]
        public int OfficerCadetID  { get; set; }
    }

    public class RecruitBatchTrial
    {
        public DateTime PayDate { get; set; }

    }
    public class RecruitBatchTrialDetails
    {
        public DateTime PayDate { get; set; }
        public int RecruitID { get; set; }
        public string Surname { get; set; }
        public string Othernames { get; set; }
        public decimal ConstPay { get; set; }
        public string ServiceNumber { get; set; }
        public int RecruitTrialPayID { get; set; }
         

    }

    public class OCBatchTrial
    {
        public DateTime PayDate { get; set; }

    }

    public class OCBatchTrialDetails
    {
        public DateTime PayDate { get; set; }
        public int OCID { get; set; }
        public string Surname { get; set; }
        public string Othernames { get; set; }
        public decimal ConstPay { get; set; }
        public string ServiceNumber { get; set; }
        public int OCTrialPayID { get; set; }
    }

    public class JCEBatchTrial
    {
        public DateTime PayDate { get; set; }

    }

    public class JCEBatchTrialDetails
    {
        public int TrialPayID { get; set; }
        public DateTime PayDate { get; set; }
        public int JuniorCEID { get; set; }
        public string Surname { get; set; }
        public string Othernames { get; set; }
        public decimal ConstPay { get; set; }
        public decimal? NetPay { get; set; }
        public string ServiceNumber { get; set; }


    }

    public class SCEBatchTrial
    {
        public DateTime PayDate { get; set; }

    }
    public class SCEBatchTrialDetails
    {
        public int TrialPayID { get; set; } 
        public DateTime PayDate { get; set; }
        public int SeniorCEID { get; set; }
        public string Surname { get; set; }
        public string Othernames { get; set; }
        public decimal ConstPay { get; set; }
        public decimal? NetPay { get; set; }
        public string ServiceNumber { get; set; }


    }

}