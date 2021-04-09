using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpDigital.ViewModels
{
    public class QueryResult
    {
        //public string json { get; set; }
        public string Persona { get; set; }
        public string Nombre { get; set; }
        public string tipoAtestado { get; set; }
        public string fileName { get; set; }
        public int fileId { get; set; }
        //public List<string> autores { get; set; }

        public string autores { get; set; }

    }
}