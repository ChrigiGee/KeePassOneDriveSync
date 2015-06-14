﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using KoenZomersKeePassOneDriveSync;
using Newtonsoft.Json;

namespace KoenZomers.KeePass.OneDriveSync
{
    /// <summary>
    /// Plugin configuration class. Contains functions to serialize/deserialize to/from JSON.
    /// </summary>
    [DataContract]
    public class Configuration
    {
        #region Constants

        /// <summary>
        /// Name under which to store these settings in the KeePass configuration store
        /// </summary>
        private const string ConfigurationKey = "KeeOneDrive";

        #endregion

        #region Non serializable Properties

        /// <summary>
        /// Dictionary with configuration settings for all password databases. Key is the local database path, value is the configuration belonging to it.
        /// </summary>
        public static IDictionary<string, Configuration> PasswordDatabases = new Dictionary<string, Configuration>();

        #endregion

        #region Serializable Properties

        /// <summary>
        /// Gets or sets refresh token from authorization
        /// </summary>
        [DataMember]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets database file path on OneDrive relative to the user 
        /// </summary>
        [DataMember]
        public string RemoteDatabasePath { get; set; }

        /// <summary>
        /// Gets or sets database file update time as it is returned from SkyDrive.
        /// </summary>
        [DataMember]
        public string LastModified { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the KeePass database configuration for KeePassOneDriveSync for the KeePass database of which the local path is provided
        /// </summary>
        /// <param name="localPasswordDatabasePath">Full path to where the KeePass database resides locally</param>
        /// <returns>KeePassOneDriveSync settings for the provided database or NULL if no configuration found</returns>
        public static Configuration GetPasswordDatabaseConfiguration(string localPasswordDatabasePath)
        {
            return PasswordDatabases.ContainsKey(localPasswordDatabasePath) ? PasswordDatabases[localPasswordDatabasePath] : null;
        }

        /// <summary>
        /// Loads the configuration stored in KeePass
        /// </summary>
        public static void Load()
        {
            // Retrieve the stored configuration from KeePass
            var value = KoenZomersKeePassOneDriveSyncExt.Host.CustomConfig.GetString(ConfigurationKey, null);

            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            // Convert the retrieved JSON to a typed entity
            PasswordDatabases = JsonConvert.DeserializeObject<Dictionary<string, Configuration>>(value);
        }

        /// <summary>
        /// Saves the current configuration
        /// </summary>
        public static void Save()
        {
            // Serialize the configuration to JSON
            var json = JsonConvert.SerializeObject(PasswordDatabases);

            // Store the configuration in KeePass
            KoenZomersKeePassOneDriveSyncExt.Host.CustomConfig.SetString(ConfigurationKey, json);
        }

        #endregion

    }
}
