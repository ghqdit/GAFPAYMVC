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
    
    public partial class DEDUCTION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEDUCTION()
        {
            this.JUNIORCEDEDUCTION = new HashSet<JUNIORCEDEDUCTION>();
            this.JUNIORCEPAYROLLDEDUCTION = new HashSet<JUNIORCEPAYROLLDEDUCTION>();
            this.JUNIORCETRIALPAYDEDUCTION = new HashSet<JUNIORCETRIALPAYDEDUCTION>();
            this.OFFICERDEDUCTION = new HashSet<OFFICERDEDUCTION>();
            this.OFFICERPAYROLLDEDUCTION = new HashSet<OFFICERPAYROLLDEDUCTION>();
            this.OFFICERTRIALPAYDEDUCTION = new HashSet<OFFICERTRIALPAYDEDUCTION>();
            this.SENIORCEDEDUCTION = new HashSet<SENIORCEDEDUCTION>();
            this.SENIORCEPAYROLLDEDUCTION = new HashSet<SENIORCEPAYROLLDEDUCTION>();
            this.SENIORCETRIALPAYDEDUCTION = new HashSet<SENIORCETRIALPAYDEDUCTION>();
            this.SOLDIERDEDUCTION = new HashSet<SOLDIERDEDUCTION>();
            this.SOLDIERPAYROLLDEDUCTION = new HashSet<SOLDIERPAYROLLDEDUCTION>();
            this.SOLDIERTRIALPAYDEDUCTION = new HashSet<SOLDIERTRIALPAYDEDUCTION>();
        }
    
        public int DEDUCTIONID { get; set; }
        public string DEDUCTIONNAME { get; set; }
        public int STATUS { get; set; }
        public int DEDUCTIONCLASSID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JUNIORCEDEDUCTION> JUNIORCEDEDUCTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JUNIORCEPAYROLLDEDUCTION> JUNIORCEPAYROLLDEDUCTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JUNIORCETRIALPAYDEDUCTION> JUNIORCETRIALPAYDEDUCTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OFFICERDEDUCTION> OFFICERDEDUCTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OFFICERPAYROLLDEDUCTION> OFFICERPAYROLLDEDUCTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OFFICERTRIALPAYDEDUCTION> OFFICERTRIALPAYDEDUCTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCEDEDUCTION> SENIORCEDEDUCTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCEPAYROLLDEDUCTION> SENIORCEPAYROLLDEDUCTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCETRIALPAYDEDUCTION> SENIORCETRIALPAYDEDUCTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOLDIERDEDUCTION> SOLDIERDEDUCTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOLDIERPAYROLLDEDUCTION> SOLDIERPAYROLLDEDUCTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOLDIERTRIALPAYDEDUCTION> SOLDIERTRIALPAYDEDUCTION { get; set; }
        public virtual DEDUCTIONCLASS DEDUCTIONCLASS { get; set; }
    }
}
