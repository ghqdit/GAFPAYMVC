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
    
    public partial class SOLDIERTRIALPAYALLOWANCE
    {
        public int SOLDIERTRIALPAYALLOWANCEID { get; set; }
        public decimal AMOUNT { get; set; }
        public int SOLDIERTRIALPAYID { get; set; }
        public int ALLOWANCEID { get; set; }
    
        public virtual ALLOWANCE ALLOWANCE { get; set; }
        public virtual SOLDIERTRIALPAY SOLDIERTRIALPAY { get; set; }
    }
}
