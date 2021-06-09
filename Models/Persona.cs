//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Persona
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Persona()
        {
            this.PersonaXActividadFortalecimientoes = new HashSet<PersonaXActividadFortalecimiento>();
            this.PersonaXArticuloes = new HashSet<PersonaXArticulo>();
            this.PersonaXCapacitacionInternas = new HashSet<PersonaXCapacitacionInterna>();
            this.PersonaXCapacitacionProfesionals = new HashSet<PersonaXCapacitacionProfesional>();
            this.PersonaXCapituloes = new HashSet<PersonaXCapitulo>();
            this.PersonaXCitas = new HashSet<PersonaXCita>();
            this.PersonaXColaboracionEventoes = new HashSet<PersonaXColaboracionEvento>();
            this.PersonaXColaboracionExternoes = new HashSet<PersonaXColaboracionExterno>();
            this.PersonaXConocimientoSoftwares = new HashSet<PersonaXConocimientoSoftware>();
            this.PersonaXCursoCentroCapacitacions = new HashSet<PersonaXCursoCentroCapacitacion>();
            this.PersonaXCursoContinuos = new HashSet<PersonaXCursoContinuo>();
            this.PersonaXCursoLibres = new HashSet<PersonaXCursoLibre>();
            this.PersonaXCursoNoRemuneradoes = new HashSet<PersonaXCursoNoRemunerado>();
            this.PersonaXDesarrolloSoftwares = new HashSet<PersonaXDesarrolloSoftware>();
            this.PersonaXDesignacions = new HashSet<PersonaXDesignacion>();
            this.PersonaXDireccionRangoSuperiors = new HashSet<PersonaXDireccionRangoSuperior>();
            this.PersonaXDisenoIndustrials = new HashSet<PersonaXDisenoIndustrial>();
            this.PersonaXGradoAcademicoes = new HashSet<PersonaXGradoAcademico>();
            this.PersonaXIdiomas = new HashSet<PersonaXIdioma>();
            this.PersonaXJuradoes = new HashSet<PersonaXJurado>();
            this.PersonaXMembresiaConsejos = new HashSet<PersonaXMembresiaConsejo>();
            this.PersonaXMembresiaOrganismos = new HashSet<PersonaXMembresiaOrganismo>();
            this.PersonaXMiembroOrganos = new HashSet<PersonaXMiembroOrgano>();
            this.PersonaXModeloIndustrials = new HashSet<PersonaXModeloIndustrial>();
            this.PersonaXModeloUtilidads = new HashSet<PersonaXModeloUtilidad>();
            this.PersonaXObraAdministrativaDesarrolloes = new HashSet<PersonaXObraAdministrativaDesarrollo>();
            this.PersonaXObraArtisticas = new HashSet<PersonaXObraArtistica>();
            this.PersonaXObraDidacticas = new HashSet<PersonaXObraDidactica>();
            this.PersonaXObtencionVegetals = new HashSet<PersonaXObtencionVegetal>();
            this.PersonaXOrganizacionEventoes = new HashSet<PersonaXOrganizacionEvento>();
            this.PersonaXOtraObraProfesionals = new HashSet<PersonaXOtraObraProfesional>();
            this.PersonaXParticipacionAIRs = new HashSet<PersonaXParticipacionAIR>();
            this.PersonaXParticipacionComisions = new HashSet<PersonaXParticipacionComision>();
            this.PersonaXParticipacionCongresoes = new HashSet<PersonaXParticipacionCongreso>();
            this.PersonaXParticipacionDeportivas = new HashSet<PersonaXParticipacionDeportiva>();
            this.PersonaXParticipacionDestacadas = new HashSet<PersonaXParticipacionDestacada>();
            this.PersonaXPatentes = new HashSet<PersonaXPatente>();
            this.PersonaXPonenciaCongresoes = new HashSet<PersonaXPonenciaCongreso>();
            this.PersonaXPremioNacionals = new HashSet<PersonaXPremioNacional>();
            this.PersonaXProyectoGraduacions = new HashSet<PersonaXProyectoGraduacion>();
            this.PersonaXProyectoInvestigacions = new HashSet<PersonaXProyectoInvestigacion>();
            this.PersonaXPublicacions = new HashSet<PersonaXPublicacion>();
            this.PersonaXResponsableUnidads = new HashSet<PersonaXResponsableUnidad>();
            this.PersonaXSecretoes = new HashSet<PersonaXSecreto>();
            this.PersonaXSistemaCIs = new HashSet<PersonaXSistemaCI>();
            this.PersonaXTrabajoNoRemuneradoes = new HashSet<PersonaXTrabajoNoRemunerado>();
        }
    
        public int ID { get; set; }
        public string cedula { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string categoria { get; set; }
        public int rol { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string puesto { get; set; }
        public string correoElectronico { get; set; }
        public string departamento { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXActividadFortalecimiento> PersonaXActividadFortalecimientoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXArticulo> PersonaXArticuloes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXCapacitacionInterna> PersonaXCapacitacionInternas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXCapacitacionProfesional> PersonaXCapacitacionProfesionals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXCapitulo> PersonaXCapituloes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXCita> PersonaXCitas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXColaboracionEvento> PersonaXColaboracionEventoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXColaboracionExterno> PersonaXColaboracionExternoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXConocimientoSoftware> PersonaXConocimientoSoftwares { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXCursoCentroCapacitacion> PersonaXCursoCentroCapacitacions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXCursoContinuo> PersonaXCursoContinuos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXCursoLibre> PersonaXCursoLibres { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXCursoNoRemunerado> PersonaXCursoNoRemuneradoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXDesarrolloSoftware> PersonaXDesarrolloSoftwares { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXDesignacion> PersonaXDesignacions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXDireccionRangoSuperior> PersonaXDireccionRangoSuperiors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXDisenoIndustrial> PersonaXDisenoIndustrials { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXGradoAcademico> PersonaXGradoAcademicoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXIdioma> PersonaXIdiomas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXJurado> PersonaXJuradoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXMembresiaConsejo> PersonaXMembresiaConsejos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXMembresiaOrganismo> PersonaXMembresiaOrganismos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXMiembroOrgano> PersonaXMiembroOrganos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXModeloIndustrial> PersonaXModeloIndustrials { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXModeloUtilidad> PersonaXModeloUtilidads { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXObraAdministrativaDesarrollo> PersonaXObraAdministrativaDesarrolloes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXObraArtistica> PersonaXObraArtisticas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXObraDidactica> PersonaXObraDidacticas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXObtencionVegetal> PersonaXObtencionVegetals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXOrganizacionEvento> PersonaXOrganizacionEventoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXOtraObraProfesional> PersonaXOtraObraProfesionals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXParticipacionAIR> PersonaXParticipacionAIRs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXParticipacionComision> PersonaXParticipacionComisions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXParticipacionCongreso> PersonaXParticipacionCongresoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXParticipacionDeportiva> PersonaXParticipacionDeportivas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXParticipacionDestacada> PersonaXParticipacionDestacadas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXPatente> PersonaXPatentes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXPonenciaCongreso> PersonaXPonenciaCongresoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXPremioNacional> PersonaXPremioNacionals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXProyectoGraduacion> PersonaXProyectoGraduacions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXProyectoInvestigacion> PersonaXProyectoInvestigacions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXPublicacion> PersonaXPublicacions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXResponsableUnidad> PersonaXResponsableUnidads { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXSecreto> PersonaXSecretoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXSistemaCI> PersonaXSistemaCIs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaXTrabajoNoRemunerado> PersonaXTrabajoNoRemuneradoes { get; set; }
    }
}
