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
    
    public partial class JUNIORCE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JUNIORCE()
        {
            this.JUNIORCEALLOWANCE = new HashSet<JUNIORCEALLOWANCE>();
            this.JUNIORCEDEDUCTION = new HashSet<JUNIORCEDEDUCTION>();
            this.JUNIORCEPAYROLL = new HashSet<JUNIORCEPAYROLL>();
            this.JUNIORCETRIALPAY = new HashSet<JUNIORCETRIALPAY>();
        }
    
        public int JUNIORCEID { get; set; }
        public string SURNAME { get; set; }
        public string OTHERNAME { get; set; }
        public System.DateTime DOB { get; set; }
        public string PHONENUMBER { get; set; }
        public string EMAILADDRESS { get; set; }
        public string RESADDRESS { get; set; }
        public string SERVICENUMBER { get; set; }
        public string HOMETOWN { get; set; }
        public System.DateTime DATETIMEINSERTED { get; set; }
        public string INSERTEDBY { get; set; }
        public Nullable<System.DateTime> DATETIMEUPDATED { get; set; }
        public string UPDATEDBY { get; set; }
        public System.DateTime DATEEMPLOYED { get; set; }
        public System.DateTime DATEPROMOTED { get; set; }
        public string SSNITNUMBER { get; set; }
        public bool ISMEDICAL { get; set; }
        public int UNITID { get; set; }
        public int TITLEID { get; set; }
        public int REGIONID { get; set; }
        public int RELIGIONID { get; set; }
        public int GENDERID { get; set; }
        public int GENERALSTATUSID { get; set; }
        public int BLOODGROUPID { get; set; }
        public int CIVILIANLEVSTEPID { get; set; }
        public int PROVIDENTFUNDID { get; set; }
        public int GRADEID { get; set; }
        public bool ISDISABLED { get; set; }
    
        public virtual BLOODGROUP BLOODGROUP { get; set; }
        public virtual CIVILIANLEVSTEP CIVILIANLEVSTEP { get; set; }
        public virtual GENDER GENDER { get; set; }
        public virtual GENERALSTATUS GENERALSTATUS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JUNIORCEALLOWANCE> JUNIORCEALLOWANCE { get; set; }
        public virtual JUNIORCEBANK JUNIORCEBANK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JUNIORCEDEDUCTION> JUNIORCEDEDUCTION { get; set; }
        public virtual JUNIORCEIMAGE JUNIORCEIMAGE { get; set; }
        public virtual JUNIORCEMEDP JUNIORCEMEDP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JUNIORCEPAYROLL> JUNIORCEPAYROLL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JUNIORCETRIALPAY> JUNIORCETRIALPAY { get; set; }
        public virtual PROVIDENTFUND PROVIDENTFUND { get; set; }
        public virtual REGION REGION { get; set; }
        public virtual RELIGION RELIGION { get; set; }
        public virtual TITLE TITLE { get; set; }
        public virtual UNIT UNIT { get; set; }
        public virtual GRADE GRADE { get; set; }
    }
}
