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
    
    public partial class DireccionRangoSuperior
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DireccionRangoSuperior()
        {
            this.DocumentoDireccionRangoSuperiors = new HashSet<DocumentoDireccionRangoSuperior>();
            this.PersonaXDireccionRangoSuperiors = new HashSet<PersonaXDireccionRangoSuperior>();
        }
    
        public int ID { get; set; }
        public string nombre { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoDireccionRangoSuperior> DocumentoDireccionRangoSuperiors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXDireccionRangoSuperior> PersonaXDireccionRangoSuperiors { get; set; }
    }
}
