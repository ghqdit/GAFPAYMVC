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
    
    public partial class SENIORCE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SENIORCE()
        {
            this.SENIORCEPAYROLL = new HashSet<SENIORCEPAYROLL>();
            this.SENIORCETRIALPAY = new HashSet<SENIORCETRIALPAY>();
            this.SENIORCEALLOWANCE = new HashSet<SENIORCEALLOWANCE>();
            this.SENIORCEDEDUCTION = new HashSet<SENIORCEDEDUCTION>();
            this.DELETEDSENIORCE = new HashSet<DELETEDSENIORCE>();
        }
    
        public int SENIORCEID { get; set; }
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
        public int RELIGIONID { get; set; }
        public int REGIONID { get; set; }
        public int GENDERID { get; set; }
        public int GENERALSTATUSID { get; set; }
        public int CIVILIANLEVSTEPID { get; set; }
        public int BLOODGROUPID { get; set; }
        public int PROVIDENTFUNDID { get; set; }
        public int GRADEID { get; set; }
        public bool ISDISABLED { get; set; }
        public string GHANAPOSTGPS { get; set; }
    
        public virtual BLOODGROUP BLOODGROUP { get; set; }
        public virtual CIVILIANLEVSTEP CIVILIANLEVSTEP { get; set; }
        public virtual GENDER GENDER { get; set; }
        public virtual GENERALSTATUS GENERALSTATUS { get; set; }
        public virtual PROVIDENTFUND PROVIDENTFUND { get; set; }
        public virtual REGION REGION { get; set; }
        public virtual RELIGION RELIGION { get; set; }
        public virtual SENIORCEBANK SENIORCEBANK { get; set; }
        public virtual SENIORCEIMAGE SENIORCEIMAGE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCEPAYROLL> SENIORCEPAYROLL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCETRIALPAY> SENIORCETRIALPAY { get; set; }
        public virtual TITLE TITLE { get; set; }
        public virtual UNIT UNIT { get; set; }
        public virtual GRADE GRADE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCEALLOWANCE> SENIORCEALLOWANCE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENIORCEDEDUCTION> SENIORCEDEDUCTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DELETEDSENIORCE> DELETEDSENIORCE { get; set; }
    }
}
