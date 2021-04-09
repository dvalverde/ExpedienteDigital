using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpDigital.Models;

namespace ExpDigital.ViewModels
{
    public class LibroCapituloAutor
    {
        public string titulo { get; set; }
        public string editorial { get; set; }
        public int id_pais { get; set; }

        public int numeroCapitulos { get; set; }
        public string consejoEditorial { get; set; }
        public string anno { get; set; }
        public virtual Pai Pai { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        /* public IList<Capitulo> capitulos { get; set; }*/



        public string[] capitulos { get; set; }

        /*MATRIZ PARA ALMACENAR LOS AUTORES  ,el espacio 0 autor 0 ...*/

        public static Autor[][] matrizAutoresG(int rows, int cols)
        {
            Autor[][] result = new Autor[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new Autor[cols];
            return result;
        }
        public Autor[][] matrizAutores { get; set; }


        public static AutorXCapitulo[][] matrizDistribucionG(int rows, int cols)
        {
            AutorXCapitulo[][] result = new AutorXCapitulo[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new AutorXCapitulo[cols];
            return result;
        }
        public AutorXCapitulo[][] matrizDistribucion { get; set; }

    }
}
