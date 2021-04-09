using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpDigital.Models;

namespace ExpDigital.ViewModels
{
    public class ObraDidacticaAutor
    {
        public string nombre { get; set; }
        public int numeroAutores { get; set; }

        public virtual ICollection<AutorXObraDidactica> AutorXObraDidacticas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoObraDidactica> DocumentoObraDidactica { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXObraDidactica> PersonaXObraDidactica { get; set; }
        //public ICollection<string> nombre { get; set; }
        //public ICollection<string> correoElectronico { get; set; }
        public IList<Autor> autores { get; set; }
        public IList<AutorXObraDidactica> autorXObraDidactica { get; set; }
    }
}
