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
        public decimal NETPAY { get; set; }
        public System.DateTime DATETIMEINSERTED { get; set; }
        public string INSERTEDBY { get; set; }
        public int STATUS { get; set; }
        public int SENIORCEID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCEPAYROLLALLOWANCE> SENIORCEPAYROLLALLOWANCE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCEPAYROLLDEDUCTION> SENIORCEPAYROLLDEDUCTION { get; set; }
        public virtual SENIORCE SENIORCE { get; set; }
    }
}
