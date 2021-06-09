using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Data.SqlClient;

namespace ExpDigital.ServerManagement
{
    public class ManejoBackUp
    {
        string con = "data source=DESKTOP-HC81J9H\\SQLEXPRESS;initial catalog=ExpedienteDigital;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"; //System.Configuration.ConfigurationManager.ConnectionStrings["ExpedienteDigitalEntities"].ConnectionString;
        public void CreateBackUp(string fileName)
        {
            // Connect to the local, default instance of SQL Server.   
            Server srv = new Server(new ServerConnection( new SqlConnection(con)));
            // Reference the AdventureWorks2012 database.   
            Database db = default(Database);
            db = srv.Databases["ExpedienteDigital"];

            // Store the current recovery model in a variable.   
            int recoverymod;
            recoverymod = (int)db.DatabaseOptions.RecoveryModel;

            // Define a Backup object variable.   
            Backup bk = new Backup();

            // Specify the type of backup, the description, the name, and the database to be backed up.   
            bk.Action = BackupActionType.Database;
            bk.BackupSetDescription = "Full backup of ExpedienteDigital";
            bk.BackupSetName = "ExpedienteDigital Backup";
            bk.Database = "ExpedienteDigital";

            // Declare a BackupDeviceItem by supplying the backup device file name in the constructor, and the type of device is a file.   
            BackupDeviceItem bdi = default(BackupDeviceItem);
            bdi = new BackupDeviceItem(fileName, DeviceType.File);

            // Add the device to the Backup object.   
            bk.Devices.Add(bdi);
            // Set the Incremental property to False to specify that this is a full database backup.   
            bk.Incremental = false;

            // Set the expiration date.   
            System.DateTime backupdate = new System.DateTime();
            backupdate = DateTime.Today;
            bk.ExpirationDate = backupdate;

            // Specify that the log must be truncated after the backup is complete.   
            bk.LogTruncation = BackupTruncateLogType.Truncate;

            // Run SqlBackup to perform the full database backup on the instance of SQL Server.   
            bk.SqlBackup(srv);

            // Inform the user that the backup has been completed.   
            System.Console.WriteLine("Full Backup complete.");

            // Remove the backup device from the Backup object.   
            bk.Devices.Remove(bdi);
        }
        public void RestoreDB(string fileName)
        {
            // Connect to the local, default instance of SQL Server.   
            Server srv = new Server(new ServerConnection(new SqlConnection(con)));
            // Reference the AdventureWorks2012 database.   
            Database db = default(Database);
            db = srv.Databases["ExpedienteDigital"];

            // Store the current recovery model in a variable.   
            int recoverymod;
            recoverymod = (int)db.DatabaseOptions.RecoveryModel;

            // Define a Restore object variable.  
            Restore rs = new Restore();

            // Set the NoRecovery property to true, so the transactions are not recovered.   
            rs.NoRecovery = true;

            // Declare a BackupDeviceItem by supplying the backup device file name in the constructor, and the type of device is a file.
            BackupDeviceItem bdi = default(BackupDeviceItem);
            bdi = new BackupDeviceItem(fileName, DeviceType.File);

            // Add the device that contains the full database backup to the Restore object.   
            rs.Devices.Add(bdi);

            // Specify the database name.   
            rs.Database = "ExpedienteDigital";

            rs.ReplaceDatabase = true;

            // Restore the full database backup with no recovery.   
            rs.SqlRestore(srv);

            // Inform the user that the Full Database Restore is complete.   
            Console.WriteLine("Full Database Restore complete.");

            // reacquire a reference to the database  
            db = srv.Databases["ExpedienteDigital"];

            // Remove the device from the Restore object.  
            rs.Devices.Remove(bdi);

            // Set the NoRecovery property to False.   
            rs.NoRecovery = false;
        }
    }
}