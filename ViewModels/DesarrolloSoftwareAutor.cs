using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpDigital.Models;

namespace ExpDigital.ViewModels
{
    public class DesarolloSoftwareAutor
    {
        public string nombre { get; set; }
        public int numeroAutores { get; set; }

        public virtual ICollection<AutorXDesarrolloSoftware> AutorXDesarolloSoftwares { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoDesarrolloSoftware> DocumentoDesarrolloSoftware { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXDesarrolloSoftware> PersonaXDesarrolloSoftware { get; set; }
        public IList<Autor> autores { get; set; }
        public IList<AutorXDesarrolloSoftware> autorXDesarrolloSoftware { get; set; }
    }
}
