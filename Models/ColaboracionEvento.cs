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
    
    public partial class ColaboracionEvento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ColaboracionEvento()
        {
            this.AutorXColaboracionEventoes = new HashSet<AutorXColaboracionEvento>();
            this.DocumentoColaboracionEventoes = new HashSet<DocumentoColaboracionEvento>();
            this.PersonaXColaboracionEventoes = new HashSet<PersonaXColaboracionEvento>();
        }
    
        public int ID { get; set; }
        public string nombreEvento { get; set; }
        public string nombreEntidad { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AutorXColaboracionEvento> AutorXColaboracionEventoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoColaboracionEvento> DocumentoColaboracionEventoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXColaboracionEvento> PersonaXColaboracionEventoes { get; set; }
    }
}
