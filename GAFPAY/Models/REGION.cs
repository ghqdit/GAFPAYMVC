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
    
    public partial class REGION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public REGION()
        {
            this.JUNIORCE = new HashSet<JUNIORCE>();
            this.OFFICER = new HashSet<OFFICER>();
            this.OFFICERCADET = new HashSet<OFFICERCADET>();
            this.RECRUIT = new HashSet<RECRUIT>();
            this.SENIORCE = new HashSet<SENIORCE>();
            this.SOLDIER = new HashSet<SOLDIER>();
        }
    
        public int REGIONID { get; set; }
        public string REGIONNAME { get; set; }
        public string REGIONSHORT { get; set; }
        public int STATUS { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JUNIORCE> JUNIORCE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OFFICER> OFFICER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OFFICERCADET> OFFICERCADET { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RECRUIT> RECRUIT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCE> SENIORCE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOLDIER> SOLDIER { get; set; }
    }
}
