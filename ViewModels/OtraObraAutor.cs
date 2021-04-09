using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpDigital.Models;

namespace ExpDigital.ViewModels
{
    public class OtraObraAutor
    {
        public string titulo { get; set; }
        public int numeroAutores { get; set; }
        public int id_tipo { get; set; }

        public virtual TipoObraProfesional TipoObraProfesional { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<AutorXOtraObraProfesional> AutorXOtraObraProfesionales { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoOtraObraProfesional> DocumentoOtraObraProfesionales { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXOtraObraProfesional> PersonaXOtraObraProfesionales { get; set; }
        public IList<Autor> autores { get; set; }
        public IList<AutorXOtraObraProfesional> autorXOtraObraProfesionals { get; set; }
    }
}
