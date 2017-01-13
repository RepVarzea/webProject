//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel;

namespace SiteVarzea.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class GASTO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GASTO()
        {
            this.GASTO_MORADOR = new HashSet<GASTO_MORADOR>();
        }

        [Key]
        public int id_gasto { get; set; }

        [Required(ErrorMessage = "Favor fazer o login para continuar.")]
        public int id_morador { get; set; }

        [Required(ErrorMessage = "Favor informar a descri��o.")]
        public string descricao { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Favor informar a data.")]
        public DateTime? data { get; set; }

        [Required(ErrorMessage = ("Favor inserir o valor"))]
        [UIHint("Currency")]
        public double valor { get; set; }

        public virtual string nomeMorador { get; set; }

        public virtual string nomeDevedores { get; set; }

        public virtual MORADOR MORADOR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GASTO_MORADOR> GASTO_MORADOR { get; set; }
    }
}
