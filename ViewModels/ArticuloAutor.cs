using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpDigital.Models;

namespace ExpDigital.ViewModels
{
    public class ArticuloAutor
    {
        public string titulo { get; set; }
        public int numeroAutores { get; set; }
        public string anno { get; set; }
        public string revista { get; set; }
        public int id_pais { get; set; }
        public string consejoEditorial { get; set; }

        public virtual Pai Pai { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AutorXArticulo> AutorXArticuloes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoArticulo> DocumentoArticuloes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXArticulo> PersonaXArticuloes { get; set; }
        //public ICollection<string> nombre { get; set; }
        //public ICollection<string> correoElectronico { get; set; }
        public IList<Autor> autores { get; set; }
        public IList<AutorXArticulo> autorXArticulos { get; set; }
    }
}
