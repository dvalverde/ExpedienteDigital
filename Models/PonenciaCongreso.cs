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
    
    public partial class PonenciaCongreso
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PonenciaCongreso()
        {
            this.AutorXPonenciaCongresoes = new HashSet<AutorXPonenciaCongreso>();
            this.DocumentoPonenciaCongresoes = new HashSet<DocumentoPonenciaCongreso>();
            this.PersonaXPonenciaCongresoes = new HashSet<PersonaXPonenciaCongreso>();
        }
    
        public int ID { get; set; }
        public string nombre { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AutorXPonenciaCongreso> AutorXPonenciaCongresoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoPonenciaCongreso> DocumentoPonenciaCongresoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXPonenciaCongreso> PersonaXPonenciaCongresoes { get; set; }
    }
}
