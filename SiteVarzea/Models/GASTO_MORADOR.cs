//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiteVarzea.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class GASTO_MORADOR
    {
        [Key]
        public int id_gasto_morador { get; set; }
    
        public virtual GASTO GASTO { get; set; }

        [Required(ErrorMessage = "Favor informar a quem cobrar")]
        public virtual MORADOR MORADOR { get; set; }
    }
}
