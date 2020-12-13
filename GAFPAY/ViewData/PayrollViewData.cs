using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAFPAY.Models;
using GAFPAY.ViewModel;
using Microsoft.Owin.Security.Provider;

namespace GAFPAY.ViewData
{
    public class PayrollViewData
    {
        private DBGAFPAYEntities db = new DBGAFPAYEntities();
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
        public SelectList getTrialPayMonth()
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
            months.Add(monthIDCurrent, month1);
            months.Add(monthIDNext, monthNext1); 
            SelectList payMonths = new SelectList(months, "Key", "Value");
            return payMonths;
        }

        public List<RecruitBatchTrial> GetRecruitBatchTrialList()
        {
            var trialPay =db.procGetRecruitBatchList()
                    .Select(a => new RecruitBatchTrial
                    { 
                        PayDate = a.Value
                    }).OrderByDescending(a => a.PayDate).ToList();
            return trialPay;
        }

        public List<RecruitBatchTrialDetails> GetRecruitBatchTrialDetailsList(DateTime paydate)
        {
            var details = db.RECRUITTRIALPAY.Where(a => a.PAYDATE == paydate).Select(a => new RecruitBatchTrialDetails()
            {
                RecruitID = a.RECRIUTID,
                PayDate = a.PAYDATE,
                Surname = a.RECRUIT.SURNAME,
                Othernames = a.RECRUIT.OTHERNAME,
                ConstPay = a.CONSTPAY,
                ServiceNumber = a.RECRUIT.SERVICENUMBER,
                RecruitTrialPayID = a.RTRIALPAYID

            }).ToList();

            return details;
        } 

        public List<OCBatchTrial> GetOCBatchTrialList()
        {
           
            var trialPay =db.procGetOCBatchList()
                    .Select(a => new OCBatchTrial
                    { 
                        PayDate = a.Value
                    }).OrderByDescending(a => a.PayDate).ToList();
            return trialPay;
        }

        public List<OCBatchTrialDetails> GetOCBatchTrialDetailsList(DateTime paydate)
        {
            var details = db.OFFICERCADETTRIALPAY.Where(a => a.PAYDATE == paydate).Select(a => new OCBatchTrialDetails()
            {
                OCID = a.OFFICERCADETID,
                PayDate = a.PAYDATE,
                Surname = a.OFFICERCADET.SURNAME,
                Othernames = a.OFFICERCADET.OTHERNAME,
                ConstPay = a.CONSTPAY, 
                ServiceNumber = a.OFFICERCADET.SERVICENUMBER,
                OCTrialPayID=a.OCTRIALPAYID

            }).ToList();

            return details;
        }

        public List<JCEBatchTrial> GetJCEBatchTrialList()
        {
            var trialPay = db.procGetJCEBatchList()
                    .Select(a => new JCEBatchTrial()
                    {
                        PayDate = a.Value
                    }).OrderByDescending(a => a.PayDate).ToList();
            return trialPay;
        }

        public List<JCEBatchTrialDetails> GetJCEBatchTrialDetailsList(DateTime paydate)
        {
            var details = db.JUNIORCETRIALPAY.Where(a => a.PAYDATE == paydate && a.STATUS==1).Select(a => new JCEBatchTrialDetails()
            {
                JuniorCEID = a.JUNIORCEID,
                TrialPayID= a.JUNIORCETRIALPAYID,
                PayDate = a.PAYDATE,
                Surname = a.JUNIORCE.SURNAME,
                Othernames = a.JUNIORCE.OTHERNAME,
                ConstPay = a.CONSTPAY,
                NetPay = a.NETPAY,
                ServiceNumber = a.JUNIORCE.SERVICENUMBER

            }).ToList();

            return details;
        }

        public List<SCEBatchTrial> GetSCEBatchTrialList()
        {
            var trialPay = db.procGetSCEBatchList()
                    .Select(a => new SCEBatchTrial()
                    {
                        PayDate = a.Value
                    }).OrderByDescending(a => a.PayDate).ToList();
            return trialPay;
        }


        public List<SCEBatchTrialDetails> GetSCEBatchTrialDetailsList(DateTime paydate)
        {
            var details = db.SENIORCETRIALPAY.Where(a => a.PAYDATE == paydate && a.STATUS==1 ).Select(a => new SCEBatchTrialDetails()
            {
                TrialPayID = a.SENIORCETRIALPAYID,
                SeniorCEID = a.SENIORCEID,
                PayDate = a.PAYDATE,
                Surname = a.SENIORCE.SURNAME,
                Othernames = a.SENIORCE.OTHERNAME,
                ConstPay = a.CONSTPAY,
                NetPay = a.NETPAY, 
                ServiceNumber = a.SENIORCE.SERVICENUMBER

            }).ToList();

            return details;
        }

        //public List<RecruitBatchTrial> GetrecruitBatchTrialList1()
        //{
        //    var tPay = db.RECRUITTRIALPAY.GroupBy(a => a.PAYDATE).OrderByDescending(a=>a.Key);
        //    return null;
        //} 



        public decimal calculateDisabilityAllowance(decimal constPay)
        {
           
            var disableRate = Convert.ToDecimal(0.2);
            var disabilityAmount = disableRate * constPay;

            return disabilityAmount;
        }


        public decimal calculateOperationAllowance(decimal constPay)
        {
            var operationRate = Convert.ToDecimal(0.2);
            var operationAmount = operationRate * constPay;
            return operationAmount;
        }

        public decimal calculateMarketPremium(decimal constPay, int gradeID)
        {
            
            var grade = db.GRADE.Find(gradeID);
            var marketPrem = Convert.ToDecimal(grade.MARKETPREM);
            var marketPremAmount = (marketPrem / 100) * constPay;
            return marketPremAmount;

        }

        public decimal calculateCivilUniformAllowance()
        {
            var uniformAllowance = 40;
            return uniformAllowance;
        }

        public decimal calculateCivilRationAllowance()
        {
            var rationAllowance = 10;
            return rationAllowance;
        }

        public decimal calculateCivilWelfare()
        {
            var CEWelfareAmount = Convert.ToDecimal(15);
            return CEWelfareAmount;
        }

        public decimal calculateIncomeTax(decimal constPay,decimal provDeduction)      
        {
            var tax = Convert.ToDecimal(0);
            var tax1 = Convert.ToDecimal(0);
            var tax2 = Convert.ToDecimal(0);
            var tax3 = Convert.ToDecimal(0);
            var tax4 = Convert.ToDecimal(0);
            var taxValue1 = Convert.ToDecimal(100);
            var taxValue2 = Convert.ToDecimal(120);
            var taxValue3 = Convert.ToDecimal(3000);
            var taxValue4 = Convert.ToDecimal(16461);
            var taxRate1 = Convert.ToDecimal(0.05);
            var taxRate2 = Convert.ToDecimal(0.10);
            var taxRate3 = Convert.ToDecimal(0.175);
            var taxRate4 = Convert.ToDecimal(0.25);
            var taxCredit = (constPay - provDeduction);

            tax1 = taxValue1 * taxRate1;
            taxCredit = taxCredit - taxValue1;

            if (taxCredit > 0 && taxCredit <= taxValue2)
            {
                tax2 = taxCredit * taxRate2;
                taxCredit = taxCredit - taxValue2;
            }
            else if (taxCredit >= taxValue2)
            {
                tax2 = taxValue2 * taxRate2;
                taxCredit = taxCredit - taxValue2;
            }

            if (taxCredit > 0 && taxCredit <= taxValue3)
            {
                tax3 = taxCredit * taxRate3;
                taxCredit = taxCredit - taxValue3;
            }
            else if (taxCredit >= taxValue3)
            {
                tax3 = taxValue3 * taxRate3;
                taxCredit = taxCredit - taxValue3;
            }

            if (taxCredit > 0 && taxCredit <= taxValue4)
            {
                tax4 = taxCredit * taxRate4;
                taxCredit = taxCredit - taxValue4;
            }
            else if (taxCredit >= taxValue4)
            {
                tax4 = taxValue4 * taxRate4;
                taxCredit = taxCredit - taxValue4;
            }

            tax = tax1 + tax2 + tax3 + tax4; 
            return tax;

        }

         

        public decimal calculateProvidentFund(decimal constPay,int providentID)
        {
            var provident = db.PROVIDENTFUND.Find(providentID);
            var provRate = Convert.ToDecimal(provident.PROVIDENTRATE);
            var provDeduction = (provRate / 100) * constPay;

            return provDeduction; 
        }


        public decimal calculateSSNIT(decimal constPay)
        {
            var ssnitRate = Convert.ToDecimal(18.5);
            var ssnitDeduction = (ssnitRate / 100) * constPay;
            return ssnitDeduction;

        }

    } 
}