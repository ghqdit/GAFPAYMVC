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
    
    public partial class SENIORCEPAYROLL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SENIORCEPAYROLL()
        {
            this.SENIORCEPAYROLLALLOWANCE = new HashSet<SENIORCEPAYROLLALLOWANCE>();
            this.SENIORCEPAYROLLDEDUCTION = new HashSet<SENIORCEPAYROLLDEDUCTION>();
        }
    
        public int SENIORCEPAYROLLID { get; set; }
        public System.DateTime PAYDATE { get; set; }
        public decimal CONSTPAY { get; set; }
        public Nullable<decimal> NETPAY { get; set; }
        public string SERVICENUMBER { get; set; }
        public Nullable<decimal> TOTALALLOWANCE { get; set; }
        public Nullable<decimal> TOTALDEDUCTION { get; set; }
        public System.DateTime DATETIMEINSERTED { get; set; }
        public string INSERTEDBY { get; set; }
        public int STATUS { get; set; }
        public int SENIORCEID { get; set; }
        public int BANKID { get; set; }
        public string NAME { get; set; }
        public string GRADE { get; set; }
        public string UNIT { get; set; }
        public string BANK { get; set; }
        public string ACCOUNTNUMBER { get; set; }
        public string LEVSTEP { get; set; }
        public Nullable<System.DateTime> DATEPROMOTED { get; set; }
        public string SSNITNUMBER { get; set; }
    
        public virtual BANK BANK1 { get; set; }
        public virtual SENIORCE SENIORCE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCEPAYROLLALLOWANCE> SENIORCEPAYROLLALLOWANCE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCEPAYROLLDEDUCTION> SENIORCEPAYROLLDEDUCTION { get; set; }
    }
}
