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
    
    public partial class JUNIORCEMEDP
    {
        public int JUNIORCEID { get; set; }
        public string MEDPCODE { get; set; }
    
        public virtual JUNIORCE JUNIORCE { get; set; }
    }
}
