﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ExpedienteDigitalEntities : DbContext
    {
        public ExpedienteDigitalEntities()
            : base("name=ExpedienteDigitalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ActividadFortalecimiento> ActividadFortalecimientoes { get; set; }
        public virtual DbSet<Articulo> Articuloes { get; set; }
        public virtual DbSet<Autor> Autors { get; set; }
        public virtual DbSet<AutorXActividadFortalecimiento> AutorXActividadFortalecimientoes { get; set; }
        public virtual DbSet<AutorXArticulo> AutorXArticuloes { get; set; }
        public virtual DbSet<AutorXCapitulo> AutorXCapituloes { get; set; }
        public virtual DbSet<AutorXColaboracionEvento> AutorXColaboracionEventoes { get; set; }
        public virtual DbSet<AutorXCursoCentroCapacitacion> AutorXCursoCentroCapacitacions { get; set; }
        public virtual DbSet<AutorXDesarrolloSoftware> AutorXDesarrolloSoftwares { get; set; }
        public virtual DbSet<AutorXObraAdministrativaDesarrollo> AutorXObraAdministrativaDesarrolloes { get; set; }
        public virtual DbSet<AutorXObraArtistica> AutorXObraArtisticas { get; set; }
        public virtual DbSet<AutorXObraDidactica> AutorXObraDidacticas { get; set; }
        public virtual DbSet<AutorXOrganizacionEvento> AutorXOrganizacionEventoes { get; set; }
        public virtual DbSet<AutorXOtraObraProfesional> AutorXOtraObraProfesionals { get; set; }
        public virtual DbSet<AutorXParticipacionDeportiva> AutorXParticipacionDeportivas { get; set; }
        public virtual DbSet<AutorXParticipacionDestacada> AutorXParticipacionDestacadas { get; set; }
        public virtual DbSet<AutorXPatente> AutorXPatentes { get; set; }
        public virtual DbSet<AutorXPonenciaCongreso> AutorXPonenciaCongresoes { get; set; }
        public virtual DbSet<AutorXProyectoGraduacion> AutorXProyectoGraduacions { get; set; }
        public virtual DbSet<AutorXProyectoInvestigacion> AutorXProyectoInvestigacions { get; set; }
        public virtual DbSet<AutorXPublicacion> AutorXPublicacions { get; set; }
        public virtual DbSet<CapacitacionInterna> CapacitacionInternas { get; set; }
        public virtual DbSet<CapacitacionProfesional> CapacitacionProfesionals { get; set; }
        public virtual DbSet<Capitulo> Capituloes { get; set; }
        public virtual DbSet<CitaDeAtestado> CitaDeAtestados { get; set; }
        public virtual DbSet<ColaboracionEvento> ColaboracionEventoes { get; set; }
        public virtual DbSet<ColaboracionExterno> ColaboracionExternoes { get; set; }
        public virtual DbSet<ComisionInstitucional> ComisionInstitucionals { get; set; }
        public virtual DbSet<ConocimientoSoftware> ConocimientoSoftwares { get; set; }
        public virtual DbSet<CursoCentroCapacitacion> CursoCentroCapacitacions { get; set; }
        public virtual DbSet<CursoContinuo> CursoContinuos { get; set; }
        public virtual DbSet<CursoLibre> CursoLibres { get; set; }
        public virtual DbSet<CursoNoRemunerado> CursoNoRemuneradoes { get; set; }
        public virtual DbSet<DesarrolloSoftware> DesarrolloSoftwares { get; set; }
        public virtual DbSet<Designacion> Designacions { get; set; }
        public virtual DbSet<DireccionRangoSuperior> DireccionRangoSuperiors { get; set; }
        public virtual DbSet<DisenoIndustrial> DisenoIndustrials { get; set; }
        public virtual DbSet<DocumentoActividadFortalecimiento> DocumentoActividadFortalecimientoes { get; set; }
        public virtual DbSet<DocumentoArticulo> DocumentoArticuloes { get; set; }
        public virtual DbSet<DocumentoCapacitacionInterna> DocumentoCapacitacionInternas { get; set; }
        public virtual DbSet<DocumentoCapacitacionProfesional> DocumentoCapacitacionProfesionals { get; set; }
        public virtual DbSet<DocumentoColaboracionEvento> DocumentoColaboracionEventoes { get; set; }
        public virtual DbSet<DocumentoColaboracionExterno> DocumentoColaboracionExternoes { get; set; }
        public virtual DbSet<DocumentoComisionInstitucional> DocumentoComisionInstitucionals { get; set; }
        public virtual DbSet<DocumentoConocimientoSoftware> DocumentoConocimientoSoftwares { get; set; }
        public virtual DbSet<DocumentoCursoCentroCapacitacion> DocumentoCursoCentroCapacitacions { get; set; }
        public virtual DbSet<DocumentoCursoContinuo> DocumentoCursoContinuos { get; set; }
        public virtual DbSet<DocumentoCursoLibre> DocumentoCursoLibres { get; set; }
        public virtual DbSet<DocumentoCursoNoRemunerado> DocumentoCursoNoRemuneradoes { get; set; }
        public virtual DbSet<DocumentoDesarrolloSoftware> DocumentoDesarrolloSoftwares { get; set; }
        public virtual DbSet<DocumentoDesignacion> DocumentoDesignacions { get; set; }
        public virtual DbSet<DocumentoDireccionRangoSuperior> DocumentoDireccionRangoSuperiors { get; set; }
        public virtual DbSet<DocumentoDisenoIndustrial> DocumentoDisenoIndustrials { get; set; }
        public virtual DbSet<DocumentoGradoAcademico> DocumentoGradoAcademicoes { get; set; }
        public virtual DbSet<DocumentoIdioma> DocumentoIdiomas { get; set; }
        public virtual DbSet<DocumentoJurado> DocumentoJuradoes { get; set; }
        public virtual DbSet<DocumentoLibro> DocumentoLibroes { get; set; }
        public virtual DbSet<DocumentoMembresiaConsejo> DocumentoMembresiaConsejos { get; set; }
        public virtual DbSet<DocumentoMembresiaOrganismo> DocumentoMembresiaOrganismos { get; set; }
        public virtual DbSet<DocumentoMiembroOrgano> DocumentoMiembroOrganos { get; set; }
        public virtual DbSet<DocumentoModeloIndustrial> DocumentoModeloIndustrials { get; set; }
        public virtual DbSet<DocumentoModeloUtilidad> DocumentoModeloUtilidads { get; set; }
        public virtual DbSet<DocumentoObraAdministrativa> DocumentoObraAdministrativas { get; set; }
        public virtual DbSet<DocumentoObraArtistica> DocumentoObraArtisticas { get; set; }
        public virtual DbSet<DocumentoObraDidactica> DocumentoObraDidacticas { get; set; }
        public virtual DbSet<DocumentoObtencionVegetal> DocumentoObtencionVegetals { get; set; }
        public virtual DbSet<DocumentoOrganizacionEvento> DocumentoOrganizacionEventoes { get; set; }
        public virtual DbSet<DocumentoOtraObraProfesional> DocumentoOtraObraProfesionals { get; set; }
        public virtual DbSet<DocumentoParticipacionAIR> DocumentoParticipacionAIRs { get; set; }
        public virtual DbSet<DocumentoParticipacionComision> DocumentoParticipacionComisions { get; set; }
        public virtual DbSet<DocumentoParticipacionCongreso> DocumentoParticipacionCongresoes { get; set; }
        public virtual DbSet<DocumentoParticipacionDeportiva> DocumentoParticipacionDeportivas { get; set; }
        public virtual DbSet<DocumentoParticipacionDestacada> DocumentoParticipacionDestacadas { get; set; }
        public virtual DbSet<DocumentoPatente> DocumentoPatentes { get; set; }
        public virtual DbSet<DocumentoPonenciaCongreso> DocumentoPonenciaCongresoes { get; set; }
        public virtual DbSet<DocumentoPremioNacional> DocumentoPremioNacionals { get; set; }
        public virtual DbSet<DocumentoProyectoGraduacion> DocumentoProyectoGraduacions { get; set; }
        public virtual DbSet<DocumentoProyectoInvestigacion> DocumentoProyectoInvestigacions { get; set; }
        public virtual DbSet<DocumentoPublicacion> DocumentoPublicacions { get; set; }
        public virtual DbSet<DocumentoResponsableUnidad> DocumentoResponsableUnidads { get; set; }
        public virtual DbSet<DocumentoSecreto> DocumentoSecretoes { get; set; }
        public virtual DbSet<DocumentoSistemaCI> DocumentoSistemaCIs { get; set; }
        public virtual DbSet<DocumentoTrabajoNoRemunerado> DocumentoTrabajoNoRemuneradoes { get; set; }
        public virtual DbSet<GradoAcademico> GradoAcademicoes { get; set; }
        public virtual DbSet<Idioma> Idiomas { get; set; }
        public virtual DbSet<Jurado> Juradoes { get; set; }
        public virtual DbSet<Libro> Libroes { get; set; }
        public virtual DbSet<MembresiaConsejo> MembresiaConsejos { get; set; }
        public virtual DbSet<MembresiaOrganismo> MembresiaOrganismos { get; set; }
        public virtual DbSet<MiembroOrgano> MiembroOrganos { get; set; }
        public virtual DbSet<ModeloIndustrial> ModeloIndustrials { get; set; }
        public virtual DbSet<ModeloUtilidad> ModeloUtilidads { get; set; }
        public virtual DbSet<ObraAdministrativaDesarrollo> ObraAdministrativaDesarrolloes { get; set; }
        public virtual DbSet<ObraArtistica> ObraArtisticas { get; set; }
        public virtual DbSet<ObraDidactica> ObraDidacticas { get; set; }
        public virtual DbSet<ObtencionVegetal> ObtencionVegetals { get; set; }
        public virtual DbSet<OrganizacionEvento> OrganizacionEventoes { get; set; }
        public virtual DbSet<OtraObraProfesional> OtraObraProfesionals { get; set; }
        public virtual DbSet<Pai> Pais { get; set; }
        public virtual DbSet<ParticipacionAIR> ParticipacionAIRs { get; set; }
        public virtual DbSet<ParticipacionComision> ParticipacionComisions { get; set; }
        public virtual DbSet<ParticipacionCongreso> ParticipacionCongresoes { get; set; }
        public virtual DbSet<ParticipacionDeportiva> ParticipacionDeportivas { get; set; }
        public virtual DbSet<ParticipacionDestacada> ParticipacionDestacadas { get; set; }
        public virtual DbSet<Patente> Patentes { get; set; }
        public virtual DbSet<Persona> Personas { get; set; }
        public virtual DbSet<PersonaXActividadFortalecimiento> PersonaXActividadFortalecimientoes { get; set; }
        public virtual DbSet<PersonaXArticulo> PersonaXArticuloes { get; set; }
        public virtual DbSet<PersonaXCapacitacionInterna> PersonaXCapacitacionInternas { get; set; }
        public virtual DbSet<PersonaXCapacitacionProfesional> PersonaXCapacitacionProfesionals { get; set; }
        public virtual DbSet<PersonaXCapitulo> PersonaXCapituloes { get; set; }
        public virtual DbSet<PersonaXCita> PersonaXCitas { get; set; }
        public virtual DbSet<PersonaXColaboracionEvento> PersonaXColaboracionEventoes { get; set; }
        public virtual DbSet<PersonaXColaboracionExterno> PersonaXColaboracionExternoes { get; set; }
        public virtual DbSet<PersonaXConocimientoSoftware> PersonaXConocimientoSoftwares { get; set; }
        public virtual DbSet<PersonaXCursoCentroCapacitacion> PersonaXCursoCentroCapacitacions { get; set; }
        public virtual DbSet<PersonaXCursoContinuo> PersonaXCursoContinuos { get; set; }
        public virtual DbSet<PersonaXCursoLibre> PersonaXCursoLibres { get; set; }
        public virtual DbSet<PersonaXCursoNoRemunerado> PersonaXCursoNoRemuneradoes { get; set; }
        public virtual DbSet<PersonaXDesarrolloSoftware> PersonaXDesarrolloSoftwares { get; set; }
        public virtual DbSet<PersonaXDesignacion> PersonaXDesignacions { get; set; }
        public virtual DbSet<PersonaXDireccionRangoSuperior> PersonaXDireccionRangoSuperiors { get; set; }
        public virtual DbSet<PersonaXDisenoIndustrial> PersonaXDisenoIndustrials { get; set; }
        public virtual DbSet<PersonaXGradoAcademico> PersonaXGradoAcademicoes { get; set; }
        public virtual DbSet<PersonaXIdioma> PersonaXIdiomas { get; set; }
        public virtual DbSet<PersonaXJurado> PersonaXJuradoes { get; set; }
        public virtual DbSet<PersonaXMembresiaConsejo> PersonaXMembresiaConsejos { get; set; }
        public virtual DbSet<PersonaXMembresiaOrganismo> PersonaXMembresiaOrganismos { get; set; }
        public virtual DbSet<PersonaXMiembroOrgano> PersonaXMiembroOrganos { get; set; }
        public virtual DbSet<PersonaXModeloIndustrial> PersonaXModeloIndustrials { get; set; }
        public virtual DbSet<PersonaXModeloUtilidad> PersonaXModeloUtilidads { get; set; }
        public virtual DbSet<PersonaXObraAdministrativaDesarrollo> PersonaXObraAdministrativaDesarrolloes { get; set; }
        public virtual DbSet<PersonaXObraArtistica> PersonaXObraArtisticas { get; set; }
        public virtual DbSet<PersonaXObraDidactica> PersonaXObraDidacticas { get; set; }
        public virtual DbSet<PersonaXObtencionVegetal> PersonaXObtencionVegetals { get; set; }
        public virtual DbSet<PersonaXOrganizacionEvento> PersonaXOrganizacionEventoes { get; set; }
        public virtual DbSet<PersonaXOtraObraProfesional> PersonaXOtraObraProfesionals { get; set; }
        public virtual DbSet<PersonaXParticipacionAIR> PersonaXParticipacionAIRs { get; set; }
        public virtual DbSet<PersonaXParticipacionComision> PersonaXParticipacionComisions { get; set; }
        public virtual DbSet<PersonaXParticipacionCongreso> PersonaXParticipacionCongresoes { get; set; }
        public virtual DbSet<PersonaXParticipacionDeportiva> PersonaXParticipacionDeportivas { get; set; }
        public virtual DbSet<PersonaXParticipacionDestacada> PersonaXParticipacionDestacadas { get; set; }
        public virtual DbSet<PersonaXPatente> PersonaXPatentes { get; set; }
        public virtual DbSet<PersonaXPonenciaCongreso> PersonaXPonenciaCongresoes { get; set; }
        public virtual DbSet<PersonaXPremioNacional> PersonaXPremioNacionals { get; set; }
        public virtual DbSet<PersonaXProyectoGraduacion> PersonaXProyectoGraduacions { get; set; }
        public virtual DbSet<PersonaXProyectoInvestigacion> PersonaXProyectoInvestigacions { get; set; }
        public virtual DbSet<PersonaXPublicacion> PersonaXPublicacions { get; set; }
        public virtual DbSet<PersonaXResponsableUnidad> PersonaXResponsableUnidads { get; set; }
        public virtual DbSet<PersonaXSecreto> PersonaXSecretoes { get; set; }
        public virtual DbSet<PersonaXSistemaCI> PersonaXSistemaCIs { get; set; }
        public virtual DbSet<PersonaXTrabajoNoRemunerado> PersonaXTrabajoNoRemuneradoes { get; set; }
        public virtual DbSet<PonenciaCongreso> PonenciaCongresoes { get; set; }
        public virtual DbSet<PremioNacional> PremioNacionals { get; set; }
        public virtual DbSet<ProyectoGraduacionGalardonado> ProyectoGraduacionGalardonadoes { get; set; }
        public virtual DbSet<ProyectoInvestigacion> ProyectoInvestigacions { get; set; }
        public virtual DbSet<Publicacion> Publicacions { get; set; }
        public virtual DbSet<ResponsableUnidad> ResponsableUnidads { get; set; }
        public virtual DbSet<Secreto> Secretoes { get; set; }
        public virtual DbSet<SistemaCI> SistemaCIs { get; set; }
        public virtual DbSet<TipoObraAdmi> TipoObraAdmis { get; set; }
        public virtual DbSet<TipoObraProfesional> TipoObraProfesionals { get; set; }
        public virtual DbSet<TrabajoNoRemunerado> TrabajoNoRemuneradoes { get; set; }
    
        public virtual int DBbackup(string nameDB, string nameFile)
        {
            var nameDBParameter = nameDB != null ?
                new ObjectParameter("nameDB", nameDB) :
                new ObjectParameter("nameDB", typeof(string));
    
            var nameFileParameter = nameFile != null ?
                new ObjectParameter("nameFile", nameFile) :
                new ObjectParameter("nameFile", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DBbackup", nameDBParameter, nameFileParameter);
        }
    
        public virtual int DBRestore(string nameDB, string nameFile)
        {
            var nameDBParameter = nameDB != null ?
                new ObjectParameter("nameDB", nameDB) :
                new ObjectParameter("nameDB", typeof(string));
    
            var nameFileParameter = nameFile != null ?
                new ObjectParameter("nameFile", nameFile) :
                new ObjectParameter("nameFile", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DBRestore", nameDBParameter, nameFileParameter);
        }
    
        public virtual ObjectResult<ServerBackUpsQuerry_Result> ServerBackUpsQuerry()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ServerBackUpsQuerry_Result>("ServerBackUpsQuerry");
        }
    
        public virtual int sp_DBbackup(string nameDB, string nameFile)
        {
            var nameDBParameter = nameDB != null ?
                new ObjectParameter("nameDB", nameDB) :
                new ObjectParameter("nameDB", typeof(string));
    
            var nameFileParameter = nameFile != null ?
                new ObjectParameter("nameFile", nameFile) :
                new ObjectParameter("nameFile", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_DBbackup", nameDBParameter, nameFileParameter);
        }
    
        public virtual ObjectResult<sp_ServerBackUpsQuerry_Result> sp_ServerBackUpsQuerry()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_ServerBackUpsQuerry_Result>("sp_ServerBackUpsQuerry");
        }
    }
}
