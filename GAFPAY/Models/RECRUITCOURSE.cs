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
    
    public partial class RECRUITCOURSE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RECRUITCOURSE()
        {
            this.RECRUIT = new HashSet<RECRUIT>();
        }
    
        public int RCID { get; set; }
        public string RCNAME { get; set; }
        public int STATUS { get; set; }
        public int SERVICEID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RECRUIT> RECRUIT { get; set; }
        public virtual SERVICE SERVICE { get; set; }
    }
}
