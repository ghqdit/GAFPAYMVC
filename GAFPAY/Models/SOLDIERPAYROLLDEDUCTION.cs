//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GAFPAY.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SOLDIERPAYROLLDEDUCTION
    {
        public int SOLDIERPAYROLLDEDUCTIONID { get; set; }
        public Nullable<decimal> TOTALAMOUNT { get; set; }
        public Nullable<System.DateTime> DEDUCTIONDATE { get; set; }
        public Nullable<decimal> BALANCE { get; set; }
        public decimal DEDUCTIONAMOUNT { get; set; }
        public int DEDUCTIONID { get; set; }
        public int SOLDIERPAYROLLID { get; set; }
    
        public virtual DEDUCTION DEDUCTION { get; set; }
        public virtual SOLDIERPAYROLL SOLDIERPAYROLL { get; set; }
    }
}
