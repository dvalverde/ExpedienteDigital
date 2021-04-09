using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpDigital.Models;

namespace ExpDigital.ViewModels
{
    public class ObraArteAutor
    {
        public string nombre { get; set; }
        public int numeroAutores { get; set; }
        public virtual ICollection<AutorXObraArtistica> AutorXObraArtisticas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoObraArtistica> DocumentoObraArtisticas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXObraArtistica> PersonaXObraArtisticas { get; set; }
        //public ICollection<string> nombre { get; set; }
        //public ICollection<string> correoElectronico { get; set; }
        public IList<Autor> autores { get; set; }
        public IList<AutorXArticulo> autorXObraArtis { get; set; }
    }
}