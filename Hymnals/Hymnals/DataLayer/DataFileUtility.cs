using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Hymnals.DataLayer
{
    public static class DataFileUtility
    {
        // DB resources for shipped collections and for user-defined collections and other usage data


        // now using global variables intead of these
        //internal static string DefaultDBName = "DefaultContent.db";
        //internal static string UserDBName = "UserContent.db";

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
                ResourceFile = await Package.Current.InstalledLocation.GetFileAsync(@"DataLayer\" + App.DefaultDBName);
                NewDBSize = (await ResourceFile.GetBasicPropertiesAsync()).Size;

                InstalledFile = await ApplicationData.Current.LocalFolder.GetFileAsync(App.DefaultDBName);
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
                ResourceFile = await Package.Current.InstalledLocation.GetFileAsync(@"DataLayer\" + dbName);
                StorageFolder dest = ApplicationData.Current.LocalFolder;
                await ResourceFile.CopyAsync(ApplicationData.Current.LocalFolder);

                // DEBUG
                //var instPath = ResourceFile.Path.ToString();
                //App.ContentPath = instPath.Remove(instPath.LastIndexOf('\\')+1);
                //App.ContentPath = dest.Path;
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
                ResourceFile = await Package.Current.InstalledLocation.GetFileAsync(@"DataLayer\" + App.DefaultDBName);
                await ResourceFile.CopyAsync(ApplicationData.Current.LocalFolder, App.DefaultDBName, NameCollisionOption.ReplaceExisting);
            }
            catch(Exception e)
            {
                ErrorMessage = e.Message;
                //TODO: log message
            }
        }


        public static async Task InstallDatabasesAsync()
        {
            bool DefaultDatabaseIsPresent = await DatabaseIsInstalledAsync(App.DefaultDBName);
            bool UserDatabaseIsPresent = await DatabaseIsInstalledAsync(App.UserDBName);
            bool DatabaseSizeHasChanged;

            //If neither default nor user db is installed, copy both resources
            if (!DefaultDatabaseIsPresent && !UserDatabaseIsPresent)
            {
                await CopyDatabaseAsync(App.DefaultDBName);
                await CopyDatabaseAsync(App.UserDBName);
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
