//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExpDigital.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ModeloIndustrial
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ModeloIndustrial()
        {
            this.DocumentoModeloIndustrials = new HashSet<DocumentoModeloIndustrial>();
            this.PersonaXModeloIndustrials = new HashSet<PersonaXModeloIndustrial>();
        }
    
        public int ID { get; set; }
        public string nombre { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoModeloIndustrial> DocumentoModeloIndustrials { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXModeloIndustrial> PersonaXModeloIndustrials { get; set; }
    }
}
