using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpDigital.Models;

namespace ExpDigital.ViewModels
{
    public class ObraAdminAutor
    {
        public string titulo { get; set; }
        public int numeroAutores { get; set; }
        public int id_tipo { get; set; }

        public virtual TipoObraAdmi TipoObraAdmi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AutorXObraAdministrativaDesarrollo> AutorXObraAdministrativaDesarrolloes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoObraAdministrativa> DocumentoObraAdministrativaes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXObraAdministrativaDesarrollo> PersonaXObraAdministrativaDesarrolloes { get; set; }

        public IList<Autor> autores { get; set; }
        public IList<AutorXObraAdministrativaDesarrollo> autorXObrasAdmin { get; set; }
    }
}