using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Hymns_UWP.Helpers
{
    public static class DataFileUtility
    {
        // DB resources for shipped collections and for user-defined collections and other usage data
        internal static string DefaultDBName = "DefaultContent.db";
        internal static string UserDBName = "UserContent.db";

        private static async Task<bool> DatabaseIsInstalledAsync(string dbName)
        {
            bool IsInstalled = false;

            try
            {
                StorageFile DBFile = await ApplicationData.Current.LocalFolder.GetFileAsync(dbName);
                IsInstalled = true;
            }
            catch (FileNotFoundException e)
            {
                string msg = e.Message;
                //TODO: log message

                IsInstalled = false;
            }

            return IsInstalled;
        }

        private static async Task<bool> DefaultDatabaseSizeHasChangedAsync()
        {
            bool DatabaseSizeHasChanged = false;
            string ErrorMessage;
            StorageFile ResourceFile, InstalledFile;
            ulong NewDBSize, CurrentDBSize;

            try
            {
                ResourceFile = await Package.Current.InstalledLocation.GetFileAsync(@"DataSource\" + DefaultDBName);
                NewDBSize = (await ResourceFile.GetBasicPropertiesAsync()).Size;

                InstalledFile = await ApplicationData.Current.LocalFolder.GetFileAsync(DefaultDBName);
                CurrentDBSize = (await InstalledFile.GetBasicPropertiesAsync()).Size;

                DatabaseSizeHasChanged = NewDBSize != CurrentDBSize;
            }
            catch(Exception e)
            {
                ErrorMessage = e.Message;
                //TODO: log message
            }

            return DatabaseSizeHasChanged;
        }

        private static async Task CopyDatabaseAsync(string dbName)
        {
            string ErrorMessage;
            StorageFile ResourceFile;

            try
            {
                ResourceFile = await Package.Current.InstalledLocation.GetFileAsync(@"DataSource\" + dbName);
                await ResourceFile.CopyAsync(ApplicationData.Current.LocalFolder);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                //TODO: log message
            }
        }

        private static async Task UpdateDatabaseAsync()
        {
            string ErrorMessage;
            StorageFile ResourceFile;

            try
            {
                ResourceFile = await Package.Current.InstalledLocation.GetFileAsync(@"DataSource\" + DefaultDBName);
                await ResourceFile.CopyAsync(ApplicationData.Current.LocalFolder, DefaultDBName, NameCollisionOption.ReplaceExisting);
            }
            catch(Exception e)
            {
                ErrorMessage = e.Message;
                //TODO: log message
            }
        }


        public static async Task InstallDatabasesAsync()
        {
            bool DefaultDatabaseIsPresent = await DatabaseIsInstalledAsync(DefaultDBName);
            bool UserDatabaseIsPresent = await DatabaseIsInstalledAsync(UserDBName);
            bool DatabaseSizeHasChanged;

            //If neither default nor user db is installed, copy both resources
            if (!DefaultDatabaseIsPresent && !UserDatabaseIsPresent)
            {
                await CopyDatabaseAsync(DefaultDBName);
                await CopyDatabaseAsync(UserDBName);
            }

            //Otherwise, they have already been installed, simply update default db if size has changed
            else
            {
                DatabaseSizeHasChanged = await DefaultDatabaseSizeHasChangedAsync();

                if (DatabaseSizeHasChanged)
                    await UpdateDatabaseAsync();
            }
        }
    }
}
