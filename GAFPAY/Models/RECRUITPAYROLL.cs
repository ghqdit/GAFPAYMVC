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
    
    public partial class RECRUITPAYROLL
    {
        public int RPAYROLLID { get; set; }
        public System.DateTime PAYDATE { get; set; }
        public decimal CONSTPAY { get; set; }
        public System.DateTime DATETIMEINSERTED { get; set; }
        public string INSERTEDBY { get; set; }
        public int RECRIUTID { get; set; }
        public int BANKID { get; set; }
    
        public virtual BANK BANK { get; set; }
        public virtual RECRUIT RECRUIT { get; set; }
    }
}
