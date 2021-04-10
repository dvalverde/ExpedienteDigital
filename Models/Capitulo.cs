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
    
    public partial class Capitulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Capitulo()
        {
            this.AutorXCapituloes = new HashSet<AutorXCapitulo>();
            this.PersonaXCapituloes = new HashSet<PersonaXCapitulo>();
        }
    
        public int ID { get; set; }
        public string titulo { get; set; }
        public int numeroAutores { get; set; }
        public int id_libro { get; set; }
        public System.DateTime fechaCreacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AutorXCapitulo> AutorXCapituloes { get; set; }
        public virtual Libro Libro { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXCapitulo> PersonaXCapituloes { get; set; }
    }
}
