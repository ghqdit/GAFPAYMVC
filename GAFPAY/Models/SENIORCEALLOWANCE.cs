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
    
    public partial class SENIORCEALLOWANCE
    {
        public int ALLOWANCEID { get; set; }
        public int SENIORCEID { get; set; }
        public int STATUS { get; set; }
        public decimal AMOUNT { get; set; }
        public string INSERTEDBY { get; set; }
        public System.DateTime DATETIMEINSERTED { get; set; }
        public string UPDATEDBY { get; set; }
        public Nullable<System.DateTime> DATETIMEUPDATED { get; set; }
        public System.Guid ID { get; set; }
    
        public virtual ALLOWANCE ALLOWANCE { get; set; }
        public virtual SENIORCE SENIORCE { get; set; }
    }
}
