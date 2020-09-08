using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

    public class RecruitTrialPay:TrialPay
    {
        [Display(Name = "Recruit")]
        public int RecruitID { get; set; }
           
    }

    public class OcTrialPay : TrialPay
    {
        [Display(Name = "Officer Cadet")]
        public int OfficerCadetID  { get; set; }
    }


}