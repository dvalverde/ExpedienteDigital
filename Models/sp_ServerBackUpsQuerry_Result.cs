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
    
    public partial class sp_ServerBackUpsQuerry_Result
    {
        public int backup_set_id { get; set; }
        public string server_name { get; set; }
        public string database_name { get; set; }
        public Nullable<System.DateTime> backup_finish_date { get; set; }
        public Nullable<decimal> backup_size { get; set; }
        public string BackupType { get; set; }
        public string physical_device_name { get; set; }
    }
}
