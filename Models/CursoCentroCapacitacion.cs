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
    
    public partial class CursoCentroCapacitacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CursoCentroCapacitacion()
        {
            this.AutorXCursoCentroCapacitacions = new HashSet<AutorXCursoCentroCapacitacion>();
            this.DocumentoCursoCentroCapacitacions = new HashSet<DocumentoCursoCentroCapacitacion>();
            this.PersonaXCursoCentroCapacitacions = new HashSet<PersonaXCursoCentroCapacitacion>();
        }
    
        public int ID { get; set; }
        public string nombre { get; set; }
        public string anno { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AutorXCursoCentroCapacitacion> AutorXCursoCentroCapacitacions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoCursoCentroCapacitacion> DocumentoCursoCentroCapacitacions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXCursoCentroCapacitacion> PersonaXCursoCentroCapacitacions { get; set; }
    }
}
