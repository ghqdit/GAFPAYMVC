using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using GAFPAY.Models;
using GAFPAY.ViewModel;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security.Provider;

namespace GAFPAY.ViewData
{
    public class PayrollViewData
    {
        private DBGAFPAYEntities db = new DBGAFPAYEntities();
        private static int CEWelfareDeductionID = 6;
        private int PresentGeneralStatusID = 1;
        private static int TaxDeductionID = 5;
        private static int RefundsAllowanceID = 25;
        private static int COsAdvanceDeductionID = 10;

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

        //public List<JCETrialReport> GetJCETrialReportDetailsList(DateTime paydate)
        //{
        //    var details = db.procGetTrialPayReportDetails(paydate).Select(a => new JCETrialReport()
        //    {
        //        JuniorCEID = a.JUNIORCEID,
        //        PayDate = a.PAYDATE,
        //        Othername = a.OTHERNAME,
        //        Surname = a.SURNAME,
        //        NetPay = a.NETPAY.Value,
        //        TrialPayID = a.JUNIORCETRIALPAYID,
        //        ConstPay=a.CONSTPAY,
                
                
        //    }).ToList();
        //    var jceTrial=new JCETrialReport();
        //    //jceTrial.JuniorCETrialReport.Add(details);

        //    return details;
        //}

        //public List<JuniorCEAllowance> GetJCETrialAllowancesReportDetails(int id)
        //{
        //    var details =
        //        db.JUNIORCETRIALPAYALLOWANCE.Where(a => a.JUNIORCETRIALPAYID == id).Select(a => new JuniorCEAllowance()
        //        {
        //            AllowanceID = a.ALLOWANCEID,
        //            AllowanceName = a.ALLOWANCE.ALLOWANCENAME,
        //            Amount = a.AMOUNT

        //        }).ToList();
        //    return details;
        //} 
        //public List<JuniorCEDeduction2> GetJCETrialDeductionsReportDetails(int id)
        //{
        //    var details =
        //        db.JUNIORCETRIALPAYDEDUCTION.Where(a => a.JUNIORCETRIALPAYID == id).Select(a => new JuniorCEDeduction2()
        //        {
        //            DeductionID = a.DEDUCTIONID,
        //            DeductionName = a.DEDUCTION.DEDUCTIONNAME,
        //            Amount = a.DEDUCTIONAMOUNT

        //        }).ToList();
        //    return details;
        //} 



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

        public decimal GetMonthTotalRecruitTrial(DateTime date)
        { 
            var tPay=   db.procGetTotalRecruitTrial(date).FirstOrDefault();
            if (tPay!=null)
            {
               return tPay.Value;
            }

            return 0;
        }

        public decimal GetMonthTotalOCTrial(DateTime date)
        {
            var tPay = db.procGetTotalOCTrial(date).FirstOrDefault();
            if (tPay!=null)
            {
                return tPay.Value;
            }
            return 0;
        }

        public decimal GetMonthTotalJCETrial(DateTime date)
        {
            var tPay = db.procGetTotalJCETrial(date).FirstOrDefault();
            if (tPay!=null)
            {
                return tPay.Value;

            }
            return 0;
        }

        public decimal GetMonthTotalSCETrial(DateTime date)
        {
            var tPay = db.procGetTotalSCETrial(date).FirstOrDefault();
            if (tPay!=null)
            {
                return tPay.Value;
            }
            return 0;
        }



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

        public decimal calculateCivilClothingAllowance()
        {
            var clothingAllowance = 50;
            return clothingAllowance;
        }

        public decimal calculateCivilRationAllowance()
        {
            var rationAllowance = 100;
            return rationAllowance;
        }

        public decimal calculateCivilHazardousAllowance()
        {
            var hazardAllowance = 150;
            return hazardAllowance;
        }

        public decimal calculateCivilWelfare()
        {
            var CEWelfareAmount =15;
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
            var ssnitRate = Convert.ToDecimal(5.5);
            var ssnitDeduction = (ssnitRate / 100) * constPay;
            return ssnitDeduction;

        }

        public DateTime getPayDate(int monthID)
        {
            var date = "";
            var dayID = 15;
            var yearID = DateTime.Now.Year;
            if (monthID == 1)
            {
                yearID = DateTime.Now.AddYears(1).Year;
            }

            date = (dayID + "-" + monthID + "-" + yearID);
            DateTime date1 = DateTime.ParseExact(date, @"d-M-yyyy",
                System.Globalization.CultureInfo.InvariantCulture);

            return date1;
        }



        public dynamic RecruitTrialPay(DateTime date, int id,decimal constPay)
        {
            var rec=new RecruitTrialPay();

            var checkAvail = db.RECRUITTRIALPAY.FirstOrDefault(a => a.PAYDATE == date && a.RECRIUTID == id && a.STATUS==1|| a.PAYDATE==date && a.RECRIUTID==id && a.STATUS==2);
            if (checkAvail != null) return rec;

            var recruitTrialPay = new RECRUITTRIALPAY();
            recruitTrialPay.PAYDATE = date;
            recruitTrialPay.RECRIUTID = id;
            recruitTrialPay.CONSTPAY = constPay;
            recruitTrialPay.STATUS = 1;
            recruitTrialPay.DATETIMEINSERTED = DateTime.Now;
            //recruitTrialPay.INSERTEDBY = User.Identity.Name;
            recruitTrialPay.INSERTEDBY = "admin";

            db.RECRUITTRIALPAY.Add(recruitTrialPay);

            try
            {
                db.SaveChanges();
                rec.success = true;
            }
            catch (Exception e)
            {
                rec.Message = e.Message;
            }

            return rec;
        }

        public dynamic  OfficerCadetTrialPay(DateTime date, int id, decimal constPay)
        {
            var oc=new OcTrialPay();

            var checkAvail = db.OFFICERCADETTRIALPAY.FirstOrDefault(a => a.PAYDATE == date && a.OFFICERCADETID == id || a.PAYDATE==date && a.OFFICERCADETID==id && a.STATUS==2);
            if (checkAvail != null) return oc;

            var ocTrialPay = new OFFICERCADETTRIALPAY();
            ocTrialPay.PAYDATE = date;
            ocTrialPay.OFFICERCADETID = id;
            ocTrialPay.CONSTPAY = constPay;
            ocTrialPay.STATUS = 1;
            ocTrialPay.DATETIMEINSERTED = DateTime.Now;
            //ocTrialPay.INSERTEDBY = User.Identity.Name;
            ocTrialPay.INSERTEDBY = "admin";

            db.OFFICERCADETTRIALPAY.Add(ocTrialPay);

            try
            {
                db.SaveChanges();
                oc.success=true;
            }
            catch (Exception e)
            {
                oc.Message = e.Message;
            }
            return oc;
        }


        public dynamic  JuniorCETrialPay(DateTime date, int id, decimal constPay, bool isMedical)
        {
            var jce=new JCETrialPay();

            var checkAvail = db.JUNIORCETRIALPAY.FirstOrDefault(a => a.PAYDATE == date && a.JUNIORCEID == id && a.STATUS == 1|| a.PAYDATE==date && a.JUNIORCEID==id && a.STATUS==2);
            if (checkAvail != null) return jce;

            var jceTrialPay = new JUNIORCETRIALPAY();
            jceTrialPay.PAYDATE = date;
            jceTrialPay.JUNIORCEID = id;
            jceTrialPay.CONSTPAY = constPay;
            jceTrialPay.DATETIMEINSERTED = DateTime.Now;
            //recruitTrialPay.INSERTEDBY = User.Identity.Name;
            jceTrialPay.STATUS = 1;
            jceTrialPay.INSERTEDBY = "admin";

            db.JUNIORCETRIALPAY.Add(jceTrialPay);

            try
            {
                db.SaveChanges();

            }
            catch (Exception e)
            {
                jce.Message = e.Message;
            }

            var JCEA = db.JUNIORCEALLOWANCE.Where(a => a.JUNIORCEID == id && a.STATUS == 1).ToList(); 
            decimal TotalAllow = 0;
            decimal NetPay = 0;
            foreach (var details in JCEA)
            {
                var Allow = new JUNIORCETRIALPAYALLOWANCE();
                Allow.ALLOWANCEID = details.ALLOWANCEID;
                Allow.AMOUNT = details.AMOUNT;
                TotalAllow += details.AMOUNT;
                Allow.JUNIORCETRIALPAYID = jceTrialPay.JUNIORCETRIALPAYID;

                db.JUNIORCETRIALPAYALLOWANCE.Add(Allow);

            }


            var JCED1 = db.JUNIORCEDEDUCTION.Where(a => a.JUNIORCEID == id && a.STATUS == 1 ).ToList();
            decimal TotalDeduc = 0;
             
            foreach (var details in JCED1)
            {
                var Deduc = new JUNIORCETRIALPAYDEDUCTION();
                if (details.DEDUCTIONID != CEWelfareDeductionID)
                {

                    if (details.DEDUCTIONID == TaxDeductionID && isMedical)
                    {

                    }
                    else
                    {
                        Deduc.DEDUCTIONID = details.DEDUCTIONID;
                        Deduc.DEDUCTIONAMOUNT = details.DEDUCTIONAMOUNT;
                        TotalDeduc += details.DEDUCTIONAMOUNT;
                        Deduc.JUNIORCETRIALPAYID = jceTrialPay.JUNIORCETRIALPAYID;
                       
                        db.JUNIORCETRIALPAYDEDUCTION.Add(Deduc);
                    }

                }
                else
                {
                    if (date.Month == 3 || date.Month == 6 || date.Month == 9 || date.Month == 12)
                    {
                        Deduc.DEDUCTIONID = details.DEDUCTIONID;
                        Deduc.DEDUCTIONAMOUNT = details.DEDUCTIONAMOUNT;
                        Deduc.JUNIORCETRIALPAYID = jceTrialPay.JUNIORCETRIALPAYID;
                        TotalDeduc += details.DEDUCTIONAMOUNT;
                        db.JUNIORCETRIALPAYDEDUCTION.Add(Deduc);
                    }
                }

            }

            var JCED2 = db.JUNIORCEDEDUCTION.Where(a => a.STATUS == 2 && a.JUNIORCEID == id).ToList();
            foreach (var  details in JCED2)
            {
                var Deduc=new JUNIORCETRIALPAYDEDUCTION();
                if (details.DEDUCTIONID==COsAdvanceDeductionID && date.Month==12|| details.DEDUCTIONID==COsAdvanceDeductionID &&date.Month==1)
                {
                    
                }
                else
                {

                    if (details.BALANCE>details.DEDUCTIONAMOUNT)
                    {
                        Deduc.DEDUCTIONAMOUNT = details.DEDUCTIONAMOUNT;
                        Deduc.BALANCE = details.BALANCE - details.DEDUCTIONAMOUNT;
                        Deduc.DEDUCTIONDATE = details.DEDUCTIONDATE;
                        Deduc.TOTALAMOUNT = details.TOTALAMOUNT;
                        Deduc.JUNIORCETRIALPAYID = jceTrialPay.JUNIORCETRIALPAYID;
                        Deduc.DEDUCTIONID = details.DEDUCTIONID;
                        TotalDeduc += details.DEDUCTIONAMOUNT;
                        db.JUNIORCETRIALPAYDEDUCTION.Add(Deduc);

                    }
                    else
                    {
                        Deduc.DEDUCTIONAMOUNT = details.BALANCE.Value;
                        Deduc.BALANCE = 0;
                        Deduc.DEDUCTIONDATE = details.DEDUCTIONDATE;
                        Deduc.TOTALAMOUNT = details.TOTALAMOUNT;
                        Deduc.JUNIORCETRIALPAYID = jceTrialPay.JUNIORCETRIALPAYID;
                        Deduc.DEDUCTIONID = details.DEDUCTIONID;
                        TotalDeduc += details.DEDUCTIONAMOUNT;
                        db.JUNIORCETRIALPAYDEDUCTION.Add(Deduc);
                    }
                    
                }
            }


            NetPay = (constPay + TotalAllow - TotalDeduc);

            jceTrialPay.NETPAY = NetPay;
            jceTrialPay.TOTALALLOWANCE = TotalAllow;
            jceTrialPay.TOTALDEDUCTION = TotalDeduc;
            try
            {
                db.SaveChanges();
                jce.success = true;
            }
            catch (Exception e)
            {
                jce.Message = e.Message;
            }


            return jce;
        }
        public dynamic  SeniorCETrialPay(DateTime date, int id, decimal constPay, bool isMedical)
        {
            var sce=new SCETrialPay();

            var checkAvail = db.SENIORCETRIALPAY.FirstOrDefault(a => a.PAYDATE == date && a.SENIORCEID == id && a.STATUS == 1|| a.PAYDATE==date && a.SENIORCEID==id && a.STATUS==2);
            if (checkAvail != null) return sce;

            var sceTrialPay = new SENIORCETRIALPAY();
            sceTrialPay.PAYDATE = date;
            sceTrialPay.SENIORCEID = id;
            sceTrialPay.CONSTPAY = constPay;
            sceTrialPay.DATETIMEINSERTED = DateTime.Now;
            //recruitTrialPay.INSERTEDBY = User.Identity.Name;
            sceTrialPay.STATUS = 1;
            sceTrialPay.INSERTEDBY = "admin";

            db.SENIORCETRIALPAY.Add(sceTrialPay);

            try
            {
                db.SaveChanges();

            }
            catch (Exception e)
            {
                sce.Message = e.Message;
            }

            var SCEA = db.SENIORCEALLOWANCE.Where(a => a.SENIORCEID == id && a.STATUS == 1).ToList(); 
            decimal TotalAllow = 0;
            decimal NetPay = 0;
            foreach (var details in SCEA)
            {
                var Allow = new SENIORCETRIALPAYALLOWANCE();
                Allow.ALLOWANCEID = details.ALLOWANCEID;
                Allow.AMOUNT = details.AMOUNT;
                TotalAllow += details.AMOUNT;
                Allow.SENIORCETRIALPAYID = sceTrialPay.SENIORCETRIALPAYID;

                db.SENIORCETRIALPAYALLOWANCE.Add(Allow);

            }


            var SCED1 = db.SENIORCEDEDUCTION.Where(a => a.SENIORCEID == id && a.STATUS == 1 ).ToList();
            decimal TotalDeduc = 0;
             
            foreach (var details in SCED1)
            {
                var Deduc = new SENIORCETRIALPAYDEDUCTION();
                if (details.DEDUCTIONID != CEWelfareDeductionID)
                {

                    if (details.DEDUCTIONID == TaxDeductionID && isMedical)
                    {

                    }
                    else
                    {
                        Deduc.DEDUCTIONID = details.DEDUCTIONID;
                        Deduc.DEDUCTIONAMOUNT = details.DEDUCTIONAMOUNT;
                        TotalDeduc += details.DEDUCTIONAMOUNT;
                        Deduc.SENIORCETRIALPAYID = sceTrialPay.SENIORCETRIALPAYID;
                       
                        db.SENIORCETRIALPAYDEDUCTION.Add(Deduc);
                    }

                }
                else
                {
                    if (date.Month == 3 || date.Month == 6 || date.Month == 9 || date.Month == 12)
                    {
                        Deduc.DEDUCTIONID = details.DEDUCTIONID;
                        Deduc.DEDUCTIONAMOUNT = details.DEDUCTIONAMOUNT;
                        Deduc.SENIORCETRIALPAYID = sceTrialPay.SENIORCETRIALPAYID;
                        TotalDeduc += details.DEDUCTIONAMOUNT;
                        db.SENIORCETRIALPAYDEDUCTION.Add(Deduc);
                    }
                }

            }

            var SCED2 = db.SENIORCEDEDUCTION.Where(a => a.STATUS == 2 && a.SENIORCEID == id).ToList();
            foreach (var  details in SCED2)
            {
                var Deduc=new SENIORCETRIALPAYDEDUCTION();
                if (details.DEDUCTIONID==COsAdvanceDeductionID && date.Month==12|| details.DEDUCTIONID==COsAdvanceDeductionID &&date.Month==1)
                {
                    
                }
                else
                {

                    if (details.BALANCE>details.DEDUCTIONAMOUNT)
                    {
                        Deduc.DEDUCTIONAMOUNT = details.DEDUCTIONAMOUNT;
                        Deduc.BALANCE = details.BALANCE - details.DEDUCTIONAMOUNT;
                        Deduc.DEDUCTIONDATE = details.DEDUCTIONDATE;
                        Deduc.TOTALAMOUNT = details.TOTALAMOUNT;
                        Deduc.SENIORCETRIALPAYID = sceTrialPay.SENIORCETRIALPAYID;
                        Deduc.DEDUCTIONID = details.DEDUCTIONID;
                        TotalDeduc += details.DEDUCTIONAMOUNT;
                        db.SENIORCETRIALPAYDEDUCTION.Add(Deduc);

                    }
                    else
                    {
                        Deduc.DEDUCTIONAMOUNT = details.BALANCE.Value;
                        Deduc.BALANCE = 0;
                        Deduc.DEDUCTIONDATE = details.DEDUCTIONDATE;
                        Deduc.TOTALAMOUNT = details.TOTALAMOUNT;
                        Deduc.SENIORCETRIALPAYID = sceTrialPay.SENIORCETRIALPAYID;
                        Deduc.DEDUCTIONID = details.DEDUCTIONID;
                        TotalDeduc += details.DEDUCTIONAMOUNT;
                        db.SENIORCETRIALPAYDEDUCTION.Add(Deduc);
                    }
                    
                }
            }


            NetPay = (constPay + TotalAllow - TotalDeduc);

            sceTrialPay.NETPAY = NetPay;
            sceTrialPay.TOTALALLOWANCE = TotalAllow;
            sceTrialPay.TOTALDEDUCTION = TotalDeduc;

            try
            {
                db.SaveChanges();
                sce.success = true;
            }
            catch (Exception e)
            {
                sce.Message = e.Message;
            }


            return sce;
        }

         
        public dynamic  RecruitPayProcess(DateTime date)      
        {
            var recruit = new RecruitTrialPay();

            var tPay = db.RECRUITTRIALPAY.Where(a => a.PAYDATE == date && a.STATUS==1).ToList();
            if (tPay.Count!=0)
            {
                  
                foreach (var item in tPay)
                {
                    var pay = new RECRUITPAYROLL();
                    pay.PAYDATE = item.PAYDATE;
                    pay.RECRIUTID = item.RECRIUTID;
                    pay.CONSTPAY = item.CONSTPAY;
                    pay.DATETIMEINSERTED = DateTime.Now;
                    pay.INSERTEDBY = "admin"; 
                    pay.BANK = item.RECRUIT.RECRUITBANK.BANK.BANKNAME.BANKNAMEX+ ", "+ item.RECRUIT.RECRUITBANK.BANK.BANKBRANCH;
                    pay.STATUS = 1;
                    pay.BANKID = item.RECRUIT.RECRUITBANK.BANKID;
                    pay.NAME = item.RECRUIT.RANK.RANKSHORT+" "+ item.RECRUIT.SURNAME + " " + item.RECRUIT.OTHERNAME;
                    pay.ACCOUNTNUMBER = item.RECRUIT.RECRUITBANK.ACCOUNTNUMBER;
                    pay.SERVICENUMBER = item.RECRUIT.SERVICENUMBER;
                    pay.TRAININGCENTER = item.RECRUIT.TRAININGCENTER.TCNAME;
                    pay.RECRUITCOURCE = item.RECRUIT.RECRUITCOURSE.RCNAME;
                    pay.DATESTART = item.RECRUIT.RECRUITSTARTDATE;
                    pay.SERVICE = item.RECRUIT.SERVICE.SERVICENAME;

                    db.RECRUITPAYROLL.Add(pay);

                    item.STATUS = 2;
                }

                try
                {
                    db.SaveChanges();
                    recruit.success = true;
                    recruit.Message = " Recruit Pay Process successful. ";
                }
                catch (Exception e)
                {
                    recruit.Message = e.Message;
                }
            }
            else
            {
                 recruit.Message = "Recruit Trial Pay for " + date.ToString("MMMM yyyy") +
                          " is already processed / not been processed. Please check and try again.";
            }
           
            return recruit;
        }


        public dynamic  OfficerCadetPayProcess(DateTime date)
        {
            var oc=new OcTrialPay();

            var tPay = db.OFFICERCADETTRIALPAY.Where(a => a.PAYDATE == date && a.STATUS == 1).ToList();
            if (tPay.Count!=0)
            {
                foreach (var item in tPay)
                {
                    var pay=new OFFICERCADETPAYROLL();
                    pay.PAYDATE = item.PAYDATE;
                    pay.CONSTPAY = item.CONSTPAY;
                    pay.DATETIMEINSERTED = DateTime.Now;
                    pay.INSERTEDBY = "admin";
                    pay.OFFICERCADETID = item.OFFICERCADETID;
                    pay.BANK = item.OFFICERCADET.OFFICERCADETBANK.BANK.BANKNAME.BANKNAMEX + ", " +
                               item.OFFICERCADET.OFFICERCADETBANK.BANK.BANKBRANCH;
                    pay.BANKID = item.OFFICERCADET.OFFICERCADETBANK.BANKID;
                    pay.NAME = item.OFFICERCADET.RANK.RANKSHORT+ " " +item.OFFICERCADET.SURNAME + " " + item.OFFICERCADET.OTHERNAME;
                    pay.ACCOUNTNUMBER = item.OFFICERCADET.OFFICERCADETBANK.OCACCOUNTNUMBER;
                    pay.SERVICENUMBER = item.OFFICERCADET.SERVICENUMBER;
                    pay.DATESTART = item.OFFICERCADET.OFFICERSTARTDATE;
                    pay.SERVICE = item.OFFICERCADET.SERVICE.SERVICENAME;
                    pay.COMMTYPE = item.OFFICERCADET.COMMISSIONTYPE.COMMISSIONTYPESHORT;
                    pay.INTAKE = item.OFFICERCADET.OFFICERINTAKE.OFFICERINTAKENAME;
                     

                    db.OFFICERCADETPAYROLL.Add(pay);

                    item.STATUS = 2;


                }
                try
                {
                    db.SaveChanges();
                    oc.success = true;
                    oc.Message = " Officer Cadet Pay Process successful. ";
                }
                catch (Exception e)
                {
                    oc.Message = e.Message;
                }
            }
            else
            {
               oc.Message = "Officer Cadet Trial Pay for " + date.ToString("MMMM yyyy") +
                        " is already processed / not been processed. Please check and try again.";

            }
            
            return oc;
        }
         
        public dynamic  JuniorCEPayProcess(DateTime date)
        {
            var jce=new JCETrialPay();
            var tPay = db.JUNIORCETRIALPAY.Where(a => a.PAYDATE == date & a.STATUS == 1).ToList();
            if (tPay.Count!=0)
            {
                foreach (var item in tPay)
                {
                    var pay=new JUNIORCEPAYROLL();
                    pay.PAYDATE = item.PAYDATE;
                    pay.CONSTPAY = item.CONSTPAY;
                    pay.NETPAY = item.NETPAY;
                    pay.TOTALALLOWANCE = item.TOTALALLOWANCE;
                    pay.TOTALDEDUCTION = item.TOTALDEDUCTION;
                    pay.DATETIMEINSERTED=DateTime.Now;
                    pay.INSERTEDBY = "admin";
                    pay.STATUS = 1;
                    pay.SERVICENUMBER = item.JUNIORCE.SERVICENUMBER;
                    pay.BANKID = item.JUNIORCE.JUNIORCEBANK.BANKID;
                    pay.LEVSTEP = item.JUNIORCE.CIVILIANLEVSTEP.LEVSTEPNAME;
                    pay.DATEPROMOTED = item.JUNIORCE.DATEPROMOTED;
                    pay.SSNITNUMBER = item.JUNIORCE.SSNITNUMBER;
                    pay.BANK = item.JUNIORCE.JUNIORCEBANK.BANK.BANKNAME.BANKNAMEX + ", " +
                               item.JUNIORCE.JUNIORCEBANK.BANK.BANKBRANCH;  
                    pay.JUNIORCEID = item.JUNIORCEID;
                    pay.NAME = item.JUNIORCE.TITLE.TITLENAME + " " + item.JUNIORCE.SURNAME + " " + item.JUNIORCE.OTHERNAME;
                    pay.GRADE = item.JUNIORCE.GRADE.GRADENAME;
                    pay.UNIT = item.JUNIORCE.UNIT.UNITNAME;
                    pay.ACCOUNTNUMBER = item.JUNIORCE.JUNIORCEBANK.ACCOUNTNUMBER;
                    
                    db.JUNIORCEPAYROLL.Add(pay);

                    try
                    {
                        db.SaveChanges();
                       
                    }
                    catch (Exception e)
                    {
                        jce.Message = e.Message;

                    }

                    var trialAllow =
                        db.JUNIORCETRIALPAYALLOWANCE.Where(a => a.JUNIORCETRIALPAYID == item.JUNIORCETRIALPAYID)
                            .ToList();
                    foreach (var allow in trialAllow)
                    {
                        var payAllow=new JUNIORCEPAYROLLALLOWANCE();
                        payAllow.AMOUNT = allow.AMOUNT;
                        payAllow.ALLOWANCEID = allow.ALLOWANCEID;
                        payAllow.JUNIORCEPAYROLLID = pay.JUNIORCEPAYROLLID;

                        db.JUNIORCEPAYROLLALLOWANCE.Add(payAllow);

                    }
                    
                    //begin Deductions
                    var trialDeduc =
                        db.JUNIORCETRIALPAYDEDUCTION.Where(a => a.JUNIORCETRIALPAYID == item.JUNIORCETRIALPAYID)
                            .ToList();
                    foreach (var deduc in trialDeduc)
                    {
                        var payDeduc=new JUNIORCEPAYROLLDEDUCTION();
                        payDeduc.DEDUCTIONID = deduc.DEDUCTIONID;
                        payDeduc.DEDUCTIONAMOUNT = deduc.DEDUCTIONAMOUNT;
                        payDeduc.JUNIORCEPAYROLLID = pay.JUNIORCEPAYROLLID;
                        if (deduc.BALANCE != null)
                        {
                            payDeduc.BALANCE = deduc.BALANCE;
                            payDeduc.DEDUCTIONDATE = deduc.DEDUCTIONDATE;
                            //model.TotalBalance = Deduc.Balance;
                        }
                        if (deduc.TOTALAMOUNT != null)
                        {
                            payDeduc.TOTALAMOUNT = deduc.TOTALAMOUNT;
                            //model.TotalAmountX = Deduc.TotalAmount;
                        }

                        db.JUNIORCEPAYROLLDEDUCTION.Add(payDeduc);
                    }

                    //put function to perform the various deduction calculations to update jce deduction list here......
                    var deducCalc = db.JUNIORCEDEDUCTION.Where(a =>a.JUNIORCEID==item.JUNIORCEID &&  a.STATUS == 2).ToList();
                    foreach (var ded in deducCalc)
                    {
                        
                        if (ded.BALANCE>ded.DEDUCTIONAMOUNT)
                        {
                            ded.BALANCE = ded.BALANCE - ded.DEDUCTIONAMOUNT;
                        }
                        else
                        {
                            ded.DEDUCTIONAMOUNT = ded.BALANCE.Value;
                            ded.BALANCE = 0;
                        }
                        if (ded.BALANCE==0)
                        {
                            db.JUNIORCEDEDUCTION.Remove(ded);
                        }
                    }
                    item.STATUS = 2;
                    
                }

                //reset jce isedit value here
                var jceEdit = db.JUNIORCE.Where(a => a.ISEDIT).ToList();
                foreach (var J in jceEdit)
                {
                    J.ISEDIT = false;
                }

                //put function to remove all refunds for juniorce here............. 
                var refund = db.JUNIORCEALLOWANCE.Where(a => a.ALLOWANCEID == RefundsAllowanceID).ToList(); 
                foreach (var x in refund)
                {
                    db.JUNIORCEALLOWANCE.Remove(x);
                }
                 
                //put function to remove COs Advance

                var coAdv = db.JUNIORCEDEDUCTION.Where(a => a.DEDUCTIONID == COsAdvanceDeductionID).ToList();
                foreach (var x in coAdv)
                {
                    db.JUNIORCEDEDUCTION.Remove(x);
                }
                 
                try
                {
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    jce.Message = e.Message;
                }
                jce.success = true;
                jce.Message = " Junior CE Pay Process successful. "; 
            }
            else
            {  
                jce.Message = "Junior CE Trial Pay for " + date.ToString("MMMM yyyy") +
                         " is already processed / not been processed. Please check and try again.";
            }
             
            return jce;
        }
        public dynamic SeniorCEPayProcess(DateTime date)
        {
            var sce=new SCETrialPay();
            var tPay = db.SENIORCETRIALPAY.Where(a => a.PAYDATE == date & a.STATUS == 1).ToList();
            if (tPay.Count!=0)
            {
                foreach (var item in tPay)
                {
                    var pay=new SENIORCEPAYROLL();
                    pay.PAYDATE = item.PAYDATE;
                    pay.CONSTPAY = item.CONSTPAY;
                    pay.NETPAY = item.NETPAY;
                    pay.TOTALALLOWANCE = item.TOTALALLOWANCE;
                    pay.TOTALDEDUCTION = item.TOTALDEDUCTION;
                    pay.DATETIMEINSERTED=DateTime.Now;
                    pay.INSERTEDBY = "admin";
                    pay.STATUS = 1;
                    pay.SERVICENUMBER = item.SENIORCE.SERVICENUMBER;
                    pay.BANKID = item.SENIORCE.SENIORCEBANK.BANKID;
                    pay.LEVSTEP = item.SENIORCE.CIVILIANLEVSTEP.LEVSTEPNAME;
                    pay.DATEPROMOTED = item.SENIORCE.DATEPROMOTED;
                    pay.SSNITNUMBER = item.SENIORCE.SSNITNUMBER;
                    pay.BANK = item.SENIORCE.SENIORCEBANK.BANK.BANKNAME.BANKNAMEX + ", " +
                               item.SENIORCE.SENIORCEBANK.BANK.BANKBRANCH;  
                    pay.SENIORCEID = item.SENIORCEID;
                    pay.NAME =item.SENIORCE.TITLE.TITLENAME + " " + item.SENIORCE.SURNAME + " " + item.SENIORCE.OTHERNAME;
                    pay.GRADE = item.SENIORCE.GRADE.GRADENAME;
                    pay.UNIT = item.SENIORCE.UNIT.UNITNAME;
                    pay.ACCOUNTNUMBER = item.SENIORCE.SENIORCEBANK.ACCOUNTNUMBER;

                    db.SENIORCEPAYROLL.Add(pay);

                    try
                    {
                        db.SaveChanges();
                       
                    }
                    catch (Exception e)
                    {
                        sce.Message = e.Message;
                    }

                    var trialAllow =
                        db.SENIORCETRIALPAYALLOWANCE.Where(a => a.SENIORCETRIALPAYID == item.SENIORCETRIALPAYID)
                            .ToList();
                    foreach (var allow in trialAllow)
                    {
                        var payAllow=new SENIORCEPAYROLLALLOWANCE();
                        payAllow.AMOUNT = allow.AMOUNT;
                        payAllow.ALLOWANCEID = allow.ALLOWANCEID;
                        payAllow.SENIORCEPAYROLLID = pay.SENIORCEPAYROLLID;

                        db.SENIORCEPAYROLLALLOWANCE.Add(payAllow);

                    }
                     
                    //begin Deductions
                    var trialDeduc =
                        db.SENIORCETRIALPAYDEDUCTION.Where(a => a.SENIORCETRIALPAYID == item.SENIORCETRIALPAYID)
                            .ToList();
                    foreach (var deduc in trialDeduc)
                    {
                        var payDeduc=new SENIORCEPAYROLLDEDUCTION();
                        payDeduc.DEDUCTIONID = deduc.DEDUCTIONID;
                        payDeduc.DEDUCTIONAMOUNT = deduc.DEDUCTIONAMOUNT;
                        payDeduc.SENIORCEPAYROLLID = pay.SENIORCEPAYROLLID;
                        if (deduc.BALANCE != null)
                        {
                            payDeduc.BALANCE = deduc.BALANCE;
                            payDeduc.DEDUCTIONDATE = deduc.DEDUCTIONDATE;
                            //model.TotalBalance = Deduc.Balance;
                        }
                        if (deduc.TOTALAMOUNT != null)
                        {
                            payDeduc.TOTALAMOUNT = deduc.TOTALAMOUNT;
                            //model.TotalAmountX = Deduc.TotalAmount;
                        }

                        db.SENIORCEPAYROLLDEDUCTION.Add(payDeduc);
                    }

                    //put function to perform the various deduction calculations to update jce deduction list here......
                    var deducCalc = db.SENIORCEDEDUCTION.Where(a =>a.SENIORCEID==item.SENIORCEID &&  a.STATUS == 2).ToList();
                    foreach (var ded in deducCalc)
                    {
                        
                        if (ded.BALANCE>ded.DEDUCTIONAMOUNT)
                        {
                            ded.BALANCE = ded.BALANCE - ded.DEDUCTIONAMOUNT;
                        }
                        else
                        {
                            ded.DEDUCTIONAMOUNT = ded.BALANCE.Value;
                            ded.BALANCE = 0;
                        }
                        if (ded.BALANCE==0)
                        {
                            db.SENIORCEDEDUCTION.Remove(ded);
                        }
                    }
                    item.STATUS = 2;
                     

                }

                //reset sce is edit value here
                var sceEdit = db.SENIORCE.Where(a => a.ISEDIT).ToList();
                foreach (var S in sceEdit)
                {
                    S.ISEDIT = false;
                }
                //put function to remove all refunds for juniorce here............. 
                var refund = db.SENIORCEALLOWANCE.Where(a => a.ALLOWANCEID == RefundsAllowanceID).ToList();
                foreach (var x in refund)
                {
                    db.SENIORCEALLOWANCE.Remove(x);
                }

                //put function to remove COs Advance

                var coAdv = db.SENIORCEDEDUCTION.Where(a => a.DEDUCTIONID == COsAdvanceDeductionID).ToList();
                foreach (var x in coAdv)
                {
                    db.SENIORCEDEDUCTION.Remove(x);
                }
                 
                try
                {
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    sce.Message = e.Message;
                }
                sce.success = true;
                sce.Message = " Senior CE Pay Process successful. "; 
            }
            else
            {  
                sce.Message = "Senior CE Trial Pay for " + date.ToString("MMMM yyyy") +
                          " is already processed / not been processed. Please check and try again."; 
            }
             
            return sce;
        }

         
    }
}