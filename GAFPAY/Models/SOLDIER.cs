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
    
    public partial class SOLDIER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SOLDIER()
        {
            this.SOLDIERALLOWANCE = new HashSet<SOLDIERALLOWANCE>();
            this.SOLDIERDEDUCTION = new HashSet<SOLDIERDEDUCTION>();
            this.SOLDIERPAYROLL = new HashSet<SOLDIERPAYROLL>();
            this.SOLDIERTRIALPAY = new HashSet<SOLDIERTRIALPAY>();
        }
    
        public int SOLDEIRID { get; set; }
        public string SURNAME { get; set; }
        public string OTHERNAME { get; set; }
        public System.DateTime DOB { get; set; }
        public string PHONENUMBER { get; set; }
        public string EMAILADDRESS { get; set; }
        public string RESADDRESS { get; set; }
        public string SERVICENUMBER { get; set; }
        public System.DateTime DATEPASSOUT { get; set; }
        public System.DateTime DATEPROMOTED { get; set; }
        public string HOMETOWN { get; set; }
        public System.DateTime DATETIMEINSERTED { get; set; }
        public string INSERTEDBY { get; set; }
        public Nullable<System.DateTime> DATETIMEUPDATED { get; set; }
        public string UPDATEDBY { get; set; }
        public bool ISMEDICAL { get; set; }
        public int GENDERID { get; set; }
        public int REGIONID { get; set; }
        public int RELIGIONID { get; set; }
        public int SERVICEID { get; set; }
        public int RANKID { get; set; }
        public int GENERALSTATUSID { get; set; }
        public int BLOODGROUPID { get; set; }
        public int MILITARYLEVSTEPID { get; set; }
        public int UNITID { get; set; }
        public int PROVIDENTFUNDID { get; set; }
    
        public virtual BLOODGROUP BLOODGROUP { get; set; }
        public virtual GENDER GENDER { get; set; }
        public virtual GENERALSTATUS GENERALSTATUS { get; set; }
        public virtual MILITARYLEVSTEP MILITARYLEVSTEP { get; set; }
        public virtual PROVIDENTFUND PROVIDENTFUND { get; set; }
        public virtual RANK RANK { get; set; }
        public virtual REGION REGION { get; set; }
        public virtual RELIGION RELIGION { get; set; }
        public virtual SERVICE SERVICE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOLDIERALLOWANCE> SOLDIERALLOWANCE { get; set; }
        public virtual SOLDIERBANK SOLDIERBANK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOLDIERDEDUCTION> SOLDIERDEDUCTION { get; set; }
        public virtual SOLDIERIMAGE SOLDIERIMAGE { get; set; }
        public virtual SOLDIERMEDP SOLDIERMEDP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOLDIERPAYROLL> SOLDIERPAYROLL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOLDIERTRIALPAY> SOLDIERTRIALPAY { get; set; }
        public virtual UNIT UNIT { get; set; }
        public virtual PROVIDENTFUND PROVIDENTFUND1 { get; set; }
    }
}