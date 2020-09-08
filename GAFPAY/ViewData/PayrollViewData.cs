using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAFPAY.ViewData
{
    public class PayrollViewData
    {

        public SelectList getPayrollMonth()
        {
            Dictionary<int, string> months = new Dictionary<int, string>();
            string[] monthArray = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            var monthPrev = DateTime.Now.AddMonths(-1).ToString("MMMM");
            var monthPrev1 = DateTime.Now.AddMonths(-1).ToString("MMMM-yyyy");
            var monthNext = DateTime.Now.AddMonths(+1).ToString("MMMM");
            var monthNext1 = DateTime.Now.AddMonths(+1).ToString("MMMM-yyyy");
            var month = DateTime.Now.ToString("MMMM");
            var mon = DateTime.Now.Month;
            var month1 = DateTime.Now.ToString("MMMM-yyyy");
            var date = DateTime.Now.Month;

            var nextMonth = date + 1;
            var PrevMonth = date - 1;
            var monthIDCurrent = Array.IndexOf(monthArray, (month)) + 1;
            var monthIDPrev = Array.IndexOf(monthArray, monthPrev) + 1;
            var monthIDNext = Array.IndexOf(monthArray, monthNext) + 1;
             
            // months.Add(monthIDPrev, monthPrev1); 
            //months.Add(monthIDCurrent, month1);
            months.Add(monthIDNext, monthNext1); 
            SelectList payMonths = new SelectList(months, "Key", "Value");
            return payMonths;
        }

    }
}