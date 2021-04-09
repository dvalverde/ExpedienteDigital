using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExpDigital.Models;
using ExpDigital.ViewModels;
using System.IO;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Transactions;
using System.Data.Entity.Validation;
using System.Threading;

namespace ExpDigital.Controllers
{
    public class BuscarAtestadosController : Controller
    {
        private ExpedienteDigitalEntities db = new ExpedienteDigitalEntities();

        // GET: BuscarAtestados
        public ActionResult Index()
        {
            return View();
        }

        // GET: BuscarAtestados/Create
        public ActionResult CreateBuscarAtestados()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateBuscarAtestados2(string fileName, string tableName)
        {
            /* Thread myNewThread = new Thread(() => Page_Load(1));
             myNewThread.Start();*/
            //Page_Load(ID);
            Page_Load2(fileName, tableName);
            
            return View();
        }

        public ActionResult BuscarAtestados()
        {
            //var laSecion = Session["Email"].ToString();
            var query1 = from persona in db.Personas
                        join personaXactividad in db.PersonaXActividadFortalecimientoes on persona.ID equals personaXactividad.id_persona
                        join actividad in db.ActividadFortalecimientoes on personaXactividad.id_actividad equals actividad.ID
                        join docActividad in db.DocumentoActividadFortalecimientoes on actividad.ID equals docActividad.id_actividad
                        //where persona.correoElectronico == laSecion
                        select new QueryResult{ Persona = persona.correoElectronico,Nombre = actividad.nombre, tipoAtestado = "Actividad de Fortalecimiento", fileName = docActividad.FileName, fileId = docActividad.ID };
            var query2 = from persona in db.Personas
                         join personaXarticulo in db.PersonaXArticuloes on persona.ID equals personaXarticulo.id_persona
                         join articulo in db.Articuloes on personaXarticulo.id_articulo equals articulo.ID
                         join docArticulo in db.DocumentoArticuloes on articulo.ID equals docArticulo.id_articulo
                         join autor in db.AutorXArticuloes on articulo.ID equals autor.id_articulo
                         join nombreAutor in db.Autors on autor.id_autor equals nombreAutor.ID
                         select new QueryResult { Persona = persona.correoElectronico, Nombre = articulo.titulo, tipoAtestado = "Articulo", fileName = docArticulo.FileName, fileId = docArticulo.ID, autores = nombreAutor.nombre};
            var query3 = from persona in db.Personas
                         join personaXcapinter in db.PersonaXCapacitacionInternas on persona.ID equals personaXcapinter.id_persona
                         join capinter in db.CapacitacionInternas on personaXcapinter.id_capacitacion equals capinter.ID
                         join docCapinter in db.DocumentoCapacitacionInternas on capinter.ID equals docCapinter.id_capacitacion
                         select new QueryResult { Persona = persona.correoElectronico, Nombre = capinter.nombre, tipoAtestado = "Capacitación Interna", fileName = docCapinter.FileName, fileId = docCapinter.ID };
            var query4 = from persona in db.Personas
                         join personaXcappro in db.PersonaXCapacitacionProfesionals on persona.ID equals personaXcappro.id_persona
                         join cappro in db.CapacitacionProfesionals on personaXcappro.id_capacitacion equals cappro.ID
                         join docCappro in db.DocumentoCapacitacionProfesionals on cappro.ID equals docCappro.id_capacitacion
                         select new QueryResult { Persona = persona.correoElectronico, Nombre = cappro.nombre, tipoAtestado = "Capacitación Profesional", fileName = docCappro.FileName, fileId = docCappro.ID };
            var query5 = from persona in db.Personas
                         join personaXcolabeve in db.PersonaXColaboracionEventoes on persona.ID equals personaXcolabeve.id_persona
                         join colabeve in db.ColaboracionEventoes on personaXcolabeve.id_colaboracion equals colabeve.ID
                         join docColabeve in db.DocumentoColaboracionEventoes on colabeve.ID equals docColabeve.id_colaboracion
                         select new QueryResult { Persona = persona.correoElectronico, Nombre = colabeve.nombreEntidad, tipoAtestado = "Colaboracion en Eventos", fileName = docColabeve.FileName, fileId = docColabeve.ID };
            var query6 = from persona in db.Personas
                         join personaXconso in db.PersonaXConocimientoSoftwares on persona.ID equals personaXconso.id_persona
                         join conso in db.ConocimientoSoftwares on personaXconso.id_software equals conso.ID
                         join docConso in db.DocumentoConocimientoSoftwares on conso.ID equals docConso.id_software
                         select new QueryResult { Persona = persona.correoElectronico, Nombre = conso.nombre, tipoAtestado = "Conocimiento de Software", fileName = docConso.FileName, fileId = docConso.ID };
            var query7 = from persona in db.Personas
                         join personaXcurcon in db.PersonaXCursoContinuos on persona.ID equals personaXcurcon.id_persona
                         join curcon in db.CursoContinuos on personaXcurcon.id_curso equals curcon.ID
                         join docCurcon in db.DocumentoCursoContinuos on curcon.ID equals docCurcon.id_curso
                         select new QueryResult { Persona = persona.correoElectronico, Nombre = curcon.nombre, tipoAtestado = "Curso Continuo", fileName = docCurcon.FileName, fileId = docCurcon.ID };
            var query8 = from persona in db.Personas
                         join personaXcurli in db.PersonaXCursoLibres on persona.ID equals personaXcurli.id_persona
                         join curli in db.CursoLibres on personaXcurli.id_curso equals curli.ID
                         join docCurli in db.DocumentoCursoLibres on curli.ID equals docCurli.id_curso
                         select new QueryResult { Persona = persona.correoElectronico, Nombre = curli.nombre, tipoAtestado = "Curso Libre", fileName = docCurli.FileName, fileId = docCurli.ID };
            var query9 = from persona in db.Personas
                         join personaXcurno in db.PersonaXCursoNoRemuneradoes on persona.ID equals personaXcurno.id_persona
                         join curno in db.CursoNoRemuneradoes on personaXcurno.id_curso equals curno.ID
                         join docCurno in db.DocumentoCursoNoRemuneradoes on curno.ID equals docCurno.id_curso
                         select new QueryResult { Persona = persona.correoElectronico, Nombre = curno.nombre, tipoAtestado = "Curso No Remunerado", fileName = docCurno.FileName, fileId = docCurno.ID };
            var query10 = from persona in db.Personas
                         join personaXdesaso in db.PersonaXDesarrolloSoftwares on persona.ID equals personaXdesaso.id_persona
                         join desaso in db.DesarrolloSoftwares on personaXdesaso.id_desarrollo_software equals desaso.ID
                         join docDesaso in db.DocumentoDesarrolloSoftwares on desaso.ID equals docDesaso.id_desarrollo_software
                         join autor in db.AutorXDesarrolloSoftwares on desaso.ID equals autor.id_desarrollo_software
                         join nombreAutor in db.Autors on autor.id_autor equals nombreAutor.ID
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = desaso.nombre, tipoAtestado = "Desarrollo de Software", fileName = docDesaso.FileName, fileId = docDesaso.ID, autores = nombreAutor.nombre };
            var query11 = from persona in db.Personas
                         join personaXdesig in db.PersonaXDesignacions on persona.ID equals personaXdesig.id_persona
                         join desig in db.Designacions on personaXdesig.id_designacion equals desig.ID
                         join docDesig in db.DocumentoDesignacions on desig.ID equals docDesig.id_designacion
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = desig.nombre, tipoAtestado = "Designación", fileName = docDesig.FileName, fileId = docDesig.ID };
            var query12 = from persona in db.Personas
                         join personaXdirec in db.PersonaXDireccionRangoSuperiors on persona.ID equals personaXdirec.id_persona
                         join direc in db.DireccionRangoSuperiors on personaXdirec.id_direccion equals direc.ID
                         join docDirec in db.DocumentoDireccionRangoSuperiors on direc.ID equals docDirec.id_direccion
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = direc.nombre, tipoAtestado = "Dirección de Rangos Superiores", fileName = docDirec.FileName, fileId = docDirec.ID };
            var query13 = from persona in db.Personas
                          join personaXdise in db.PersonaXDisenoIndustrials on persona.ID equals personaXdise.id_persona
                          join dise in db.DisenoIndustrials on personaXdise.id_diseno equals dise.ID
                          join docDise in db.DocumentoDisenoIndustrials on dise.ID equals docDise.id_diseno
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = dise.nombre, tipoAtestado = "Diseño Industrial", fileName = docDise.FileName, fileId = docDise.ID };
            var query14 = from persona in db.Personas
                          join personaX in db.PersonaXGradoAcademicoes on persona.ID equals personaX.id_persona
                          join atestado in db.GradoAcademicoes on personaX.id_grado equals atestado.ID
                          join doc in db.DocumentoGradoAcademicoes on atestado.ID equals doc.id_grado
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Grado Academico", fileName = doc.FileName, fileId = doc.ID };
            var query15 = from persona in db.Personas
                          join personaX in db.PersonaXIdiomas on persona.ID equals personaX.id_persona
                          join atestado in db.Idiomas on personaX.id_idioma equals atestado.ID
                          join doc in db.DocumentoIdiomas on atestado.ID equals doc.id_idioma
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Idioma", fileName = doc.FileName, fileId = doc.ID };
            var query16 = from persona in db.Personas
                          join personaX in db.PersonaXJuradoes on persona.ID equals personaX.id_persona
                          join atestado in db.Juradoes on personaX.id_jurado equals atestado.ID
                          join doc in db.DocumentoJuradoes on atestado.ID equals doc.id_jurado
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Jurado", fileName = doc.FileName, fileId = doc.ID };
            var query17 = from persona in db.Personas
                          join personaX in db.PersonaXCapituloes on persona.ID equals personaX.id_persona
                          join atestado in db.Capituloes on personaX.id_capitulo equals atestado.ID
                          join doc in db.DocumentoLibroes on atestado.ID equals doc.id_libro
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.titulo, tipoAtestado = "Libro", fileName = doc.FileName, fileId = doc.ID };
            var query18 = from persona in db.Personas
                          join personaX in db.PersonaXMembresiaConsejos on persona.ID equals personaX.id_persona
                          join atestado in db.MembresiaConsejos on personaX.id_membresia equals atestado.ID
                          join doc in db.DocumentoMembresiaConsejos on atestado.ID equals doc.id_membresia
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Membresía a Consejos", fileName = doc.FileName, fileId = doc.ID };
            var query19 = from persona in db.Personas
                          join personaX in db.PersonaXMembresiaOrganismos on persona.ID equals personaX.id_persona
                          join atestado in db.MembresiaOrganismos on personaX.id_membresia equals atestado.ID
                          join doc in db.DocumentoMembresiaOrganismos on atestado.ID equals doc.id_membresia
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Membresía a Organismos", fileName = doc.FileName, fileId = doc.ID };
            var query20 = from persona in db.Personas
                          join personaX in db.PersonaXMiembroOrganos on persona.ID equals personaX.id_persona
                          join atestado in db.MiembroOrganos on personaX.id_miembro equals atestado.ID
                          join doc in db.DocumentoMiembroOrganos on atestado.ID equals doc.id_miembro
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Miembro a Organos", fileName = doc.FileName, fileId = doc.ID };
            var query21 = from persona in db.Personas
                          join personaX in db.PersonaXModeloIndustrials on persona.ID equals personaX.id_persona
                          join atestado in db.ModeloIndustrials on personaX.id_modelo equals atestado.ID
                          join doc in db.DocumentoModeloIndustrials on atestado.ID equals doc.id_modelo
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Modelo Industrial", fileName = doc.FileName, fileId = doc.ID };
            var query22 = from persona in db.Personas
                          join personaX in db.PersonaXModeloUtilidads on persona.ID equals personaX.id_persona
                          join atestado in db.ModeloUtilidads on personaX.id_modelo_utilidad equals atestado.ID
                          join doc in db.DocumentoModeloUtilidads on atestado.ID equals doc.id_modelo_utilidad
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Modelo de Utilidad", fileName = doc.FileName, fileId = doc.ID };
            var query23 = from persona in db.Personas
                          join personaX in db.PersonaXObraAdministrativaDesarrolloes on persona.ID equals personaX.id_persona
                          join atestado in db.ObraAdministrativaDesarrolloes on personaX.id_obra_administrativa equals atestado.ID
                          join doc in db.DocumentoObraAdministrativas on atestado.ID equals doc.id_obra_administrativa
                          join autor in db.AutorXObraAdministrativaDesarrolloes on atestado.ID equals autor.id_obra_administrativa
                          join nombreAutor in db.Autors on autor.id_autor equals nombreAutor.ID
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.titulo, tipoAtestado = "Obra Administrativa de Desarrollo", fileName = doc.FileName, fileId = doc.ID, autores = nombreAutor.nombre };
            var query24 = from persona in db.Personas
                          join personaX in db.PersonaXObraArtisticas on persona.ID equals personaX.id_persona
                          join atestado in db.ObraArtisticas on personaX.id_obra_artistica equals atestado.ID
                          join doc in db.DocumentoObraArtisticas on atestado.ID equals doc.id_obra_artistica
                          join autor in db.AutorXObraArtisticas on atestado.ID equals autor.id_obra_artistica
                          join nombreAutor in db.Autors on autor.id_autor equals nombreAutor.ID
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Obra Artistica", fileName = doc.FileName, fileId = doc.ID, autores = nombreAutor.nombre };
            var query25 = from persona in db.Personas
                          join personaX in db.PersonaXObraDidacticas on persona.ID equals personaX.id_persona
                          join atestado in db.ObraDidacticas on personaX.id_obra_didactica equals atestado.ID
                          join doc in db.DocumentoObraDidacticas on atestado.ID equals doc.id_obra_didactica
                          join autor in db.AutorXObraDidacticas on atestado.ID equals autor.id_obra_didactica
                          join nombreAutor in db.Autors on autor.id_autor equals nombreAutor.ID
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Obra Didactica", fileName = doc.FileName, fileId = doc.ID, autores = nombreAutor.nombre };
            var query26 = from persona in db.Personas
                          join personaX in db.PersonaXObtencionVegetals on persona.ID equals personaX.id_persona
                          join atestado in db.ObtencionVegetals on personaX.id_obtencion_vegetal equals atestado.ID
                          join doc in db.DocumentoObtencionVegetals on atestado.ID equals doc.id_obtencion_vegetal
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Obtención Vegetal", fileName = doc.FileName, fileId = doc.ID };
            var query27 = from persona in db.Personas
                          join personaX in db.PersonaXOrganizacionEventoes on persona.ID equals personaX.id_persona
                          join atestado in db.OrganizacionEventoes on personaX.id_evento equals atestado.ID
                          join doc in db.DocumentoOrganizacionEventoes on atestado.ID equals doc.id_evento
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Organización de Eventos", fileName = doc.FileName, fileId = doc.ID };
            var query28 = from persona in db.Personas
                          join personaX in db.PersonaXOtraObraProfesionals on persona.ID equals personaX.id_persona
                          join atestado in db.OtraObraProfesionals on personaX.id_obra_profesional equals atestado.ID
                          join doc in db.DocumentoOtraObraProfesionals on atestado.ID equals doc.id_obra_profesional
                          join autor in db.AutorXOtraObraProfesionals on atestado.ID equals autor.id_obra_profesional
                          join nombreAutor in db.Autors on autor.id_autor equals nombreAutor.ID
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.titulo, tipoAtestado = "Otra Obra Profesional", fileName = doc.FileName, fileId = doc.ID, autores = nombreAutor.nombre };
            var query29 = from persona in db.Personas
                          join personaX in db.PersonaXParticipacionComisions on persona.ID equals personaX.id_persona
                          join atestado in db.ParticipacionComisions on personaX.id_participacion equals atestado.ID
                          join doc in db.DocumentoParticipacionComisions on atestado.ID equals doc.id_participacion
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Participación en Comisiones", fileName = doc.FileName, fileId = doc.ID };
            var query30 = from persona in db.Personas
                          join personaX in db.PersonaXParticipacionCongresoes on persona.ID equals personaX.id_persona
                          join atestado in db.ParticipacionCongresoes on personaX.id_participacion equals atestado.ID
                          join doc in db.DocumentoParticipacionCongresoes on atestado.ID equals doc.id_participacion
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Participación en Congresos", fileName = doc.FileName, fileId = doc.ID };
            var query31 = from persona in db.Personas
                          join personaX in db.PersonaXParticipacionDeportivas on persona.ID equals personaX.id_persona
                          join atestado in db.ParticipacionDeportivas on personaX.id_participacion equals atestado.ID
                          join doc in db.DocumentoParticipacionDeportivas on atestado.ID equals doc.id_participacion
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombreEvento, tipoAtestado = "Participación en Eventos Deportivos", fileName = doc.FileName, fileId = doc.ID };
            var query32 = from persona in db.Personas
                          join personaX in db.PersonaXParticipacionDestacadas on persona.ID equals personaX.id_persona
                          join atestado in db.ParticipacionDestacadas on personaX.id_participacion equals atestado.ID
                          join doc in db.DocumentoParticipacionDestacadas on atestado.ID equals doc.id_participacion
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Participación Destacada", fileName = doc.FileName, fileId = doc.ID };
            var query33 = from persona in db.Personas
                          join personaX in db.PersonaXPatentes on persona.ID equals personaX.id_persona
                          join atestado in db.Patentes on personaX.id_patente equals atestado.ID
                          join doc in db.DocumentoPatentes on atestado.ID equals doc.id_patente
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Patente", fileName = doc.FileName, fileId = doc.ID };
            var query34 = from persona in db.Personas
                          join personaX in db.PersonaXPremioNacionals on persona.ID equals personaX.id_persona
                          join atestado in db.PremioNacionals on personaX.id_premio equals atestado.ID
                          join doc in db.DocumentoPremioNacionals on atestado.ID equals doc.id_premio
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Premios Nacionales", fileName = doc.FileName, fileId = doc.ID };
            var query35 = from persona in db.Personas
                          join personaX in db.PersonaXProyectoGraduacions on persona.ID equals personaX.id_persona
                          join atestado in db.ProyectoGraduacionGalardonadoes on personaX.id_proyecto_graduacion equals atestado.ID
                          join doc in db.DocumentoProyectoGraduacions on atestado.ID equals doc.id_proyecto
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Proyecto de Graduación Galardonado", fileName = doc.FileName, fileId = doc.ID };
            var query36 = from persona in db.Personas
                          join personaX in db.PersonaXProyectoInvestigacions on persona.ID equals personaX.id_persona
                          join atestado in db.ProyectoGraduacionGalardonadoes on personaX.id_proyecto_investigacion equals atestado.ID
                          join doc in db.DocumentoProyectoInvestigacions on atestado.ID equals doc.id_proyecto_investigacion
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Proyecto de Investigación", fileName = doc.FileName, fileId = doc.ID };
            var query37 = from persona in db.Personas
                          join personaX in db.PersonaXPublicacions on persona.ID equals personaX.id_persona
                          join atestado in db.Publicacions on personaX.id_publicacion equals atestado.ID
                          join doc in db.DocumentoPublicacions on atestado.ID equals doc.id_publicacion
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Publicación", fileName = doc.FileName, fileId = doc.ID };
            var query38 = from persona in db.Personas
                          join personaX in db.PersonaXResponsableUnidads on persona.ID equals personaX.id_persona
                          join atestado in db.ResponsableUnidads on personaX.id_unidad equals atestado.ID
                          join doc in db.DocumentoResponsableUnidads on atestado.ID equals doc.id_unidad
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Responsable de Unidad", fileName = doc.FileName, fileId = doc.ID };
            var query39 = from persona in db.Personas
                          join personaX in db.PersonaXSecretoes on persona.ID equals personaX.id_persona
                          join atestado in db.Secretoes on personaX.id_secreto equals atestado.ID
                          join doc in db.DocumentoSecretoes on atestado.ID equals doc.id_secreto
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Secreto", fileName = doc.FileName, fileId = doc.ID };
            var query40 = from persona in db.Personas
                          join personaX in db.PersonaXSistemaCIs on persona.ID equals personaX.id_persona
                          join atestado in db.SistemaCIs on personaX.id_sistema equals atestado.ID
                          join doc in db.DocumentoSistemaCIs on atestado.ID equals doc.id_sistema
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Sistema de Circuitos Integrados", fileName = doc.FileName, fileId = doc.ID };
            var query41 = from persona in db.Personas
                          join personaX in db.PersonaXTrabajoNoRemuneradoes on persona.ID equals personaX.id_persona
                          join atestado in db.TrabajoNoRemuneradoes on personaX.id_trabajo equals atestado.ID
                          join doc in db.DocumentoTrabajoNoRemuneradoes on atestado.ID equals doc.id_trabajo
                          select new QueryResult { Persona = persona.correoElectronico, Nombre = atestado.nombre, tipoAtestado = "Trabajo No Remunerado", fileName = doc.FileName, fileId = doc.ID };
            /*string elJson = "[{";
            foreach(var item in query)
            {
                elJson += "\'fileid\':" + item.FileID + ",";
                elJson += "\'name\':\'" + item.Nombre + "\',";
                elJson += "\'filename\':\'" + item.NombreArchivo + "\'},{";
            }
            elJson = elJson.Substring(0, elJson.Length - 2);
            elJson += "]";
            Session["Query"] = elJson;*/
            JsonLista elJson = new JsonLista();
            elJson.resultado = new List<QueryResult>();
            foreach(var item in query1)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query2)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query3)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query4)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query5)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query6)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query7)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query8)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query9)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query10)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query11)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query12)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query13)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query14)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query15)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query16)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query17)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query18)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query19)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query20)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query21)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query22)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query23)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query24)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query25)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query26)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query27)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query28)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query29)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query30)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query31)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query32)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query33)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query34)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query35)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query36)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query37)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query38)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query39)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query40)
            {
                elJson.resultado.Add(item);
            }
            foreach (var item in query41)
            {
                elJson.resultado.Add(item);
            }
            return View(elJson);
        }

        [HttpGet]
        public void Page_Load(int ID)
        {
            string ConnStr = System.Configuration.ConfigurationManager.
        ConnectionStrings["ExpedienteDigitalEntities"].ConnectionString;
            int indice = ConnStr.IndexOf("data source");
            ConnStr = ConnStr.Substring(indice, ConnStr.Length - 2 - indice);
            const string SelectTSql = @"
        SELECT FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
         FROM DocumentoMiembroOrganos d
         INNER JOIN MiembroOrganos t ON d.id_miembro = t.ID
         WHERE t.ID = @ID ";
            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string serverPath;
                    byte[] serverTxn;
                    using (SqlCommand cmd = new SqlCommand(SelectTSql, conn))
                    {
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            serverPath = rdr.GetSqlString(0).Value;
                            serverTxn = rdr.GetSqlBinary(1).Value;
                            rdr.Close();
                        }
                    }
                    this.StreamPhotoImage(serverPath, serverTxn);
                }
                ts.Complete();
            }
        }
        public void Page_Load2(string FileName, string nombreTabla)
        {
            string ConnStr = System.Configuration.ConfigurationManager.
        ConnectionStrings["ExpedienteDigitalEntities"].ConnectionString;
            int indice = ConnStr.IndexOf("data source");
            ConnStr = ConnStr.Substring(indice, ConnStr.Length - 2 - indice);
            string SelectTSql = "SELECT TOP 1 FileStreamCol.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT() FROM Documento" + nombreTabla + @" d WHERE d.FileName = @FileName";
            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string serverPath;
                    byte[] serverTxn;
                    using (SqlCommand cmd = new SqlCommand(SelectTSql, conn))
                    {
                        cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = FileName;
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            serverPath = rdr.GetSqlString(0).Value;
                            serverTxn = rdr.GetSqlBinary(1).Value;
                            rdr.Close();
                        }
                    }
                    this.StreamPhotoImage(serverPath, serverTxn);
                }
                ts.Complete();
            }
        }

        public void StreamPhotoImage(string serverPath, byte[] serverTxn)
        {
            const int BlockSize = 1024 * 512;
            const string PDFContentType = "application/pdf";
            using (SqlFileStream sfs =
              new SqlFileStream(serverPath, serverTxn, FileAccess.Read))
            {
                byte[] buffer = new byte[BlockSize];
                int bytesRead;
                Response.BufferOutput = false;
                Response.ContentType = PDFContentType;
                while ((bytesRead = sfs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Response.OutputStream.Write(buffer, 0, bytesRead);
                    Response.Flush();
                }
                sfs.Close();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuscarAtestados(int i) {
            ViewBag.Query = (from d in db.Personas where d.correoElectronico == "kk" select d).FirstOrDefault();
            return View();
        }
    }
}