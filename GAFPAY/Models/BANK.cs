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
    
    public partial class BANK
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BANK()
        {
            this.JUNIORCEBANK = new HashSet<JUNIORCEBANK>();
            this.OFFICERBANK = new HashSet<OFFICERBANK>();
            this.OFFICERCADETBANK = new HashSet<OFFICERCADETBANK>();
            this.RECRUITBANK = new HashSet<RECRUITBANK>();
            this.SENIORCEBANK = new HashSet<SENIORCEBANK>();
            this.SOLDIERBANK = new HashSet<SOLDIERBANK>();
            this.JUNIORCEPAYROLL = new HashSet<JUNIORCEPAYROLL>();
            this.OFFICERCADETPAYROLL = new HashSet<OFFICERCADETPAYROLL>();
            this.OFFICERPAYROLL = new HashSet<OFFICERPAYROLL>();
            this.RECRUITPAYROLL = new HashSet<RECRUITPAYROLL>();
            this.SENIORCEPAYROLL = new HashSet<SENIORCEPAYROLL>();
            this.SOLDIERPAYROLL = new HashSet<SOLDIERPAYROLL>();
        }
    
        public int BANKID { get; set; }
        public string BANKBRANCH { get; set; }
        public string BANKCODE { get; set; }
        public int STATUS { get; set; }
        public int BANKNAMEID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JUNIORCEBANK> JUNIORCEBANK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OFFICERBANK> OFFICERBANK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OFFICERCADETBANK> OFFICERCADETBANK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RECRUITBANK> RECRUITBANK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCEBANK> SENIORCEBANK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOLDIERBANK> SOLDIERBANK { get; set; }
        public virtual BANKNAME BANKNAME { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JUNIORCEPAYROLL> JUNIORCEPAYROLL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OFFICERCADETPAYROLL> OFFICERCADETPAYROLL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OFFICERPAYROLL> OFFICERPAYROLL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RECRUITPAYROLL> RECRUITPAYROLL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCEPAYROLL> SENIORCEPAYROLL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOLDIERPAYROLL> SOLDIERPAYROLL { get; set; }
    }
}
