using System;
using System.IO;
using System.Runtime.InteropServices;

namespace GMBuddy
{
    public static class Utils
    {
        /// <summary>
        /// Gets an OS-specific path to the dbName.sqlite file.
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns>
        /// On *nix platforms, $HOME/.gmbuddy/databases/dbName.sqlite is returned.
        /// On Windows, %LOCALAPPDATA%\GMBuddy\Databases\dbName.sqlite is returned.
        /// </returns>
        /// <exception cref="ArgumentException">Throws an ArgumentException if gameType is null or empty</exception>
        [Obsolete("Use GetDataDirectory instead")]
        public static string GetDatabasePath(string dbName)
        {
            if (string.IsNullOrEmpty(dbName))
            {
                throw new ArgumentException();
            }

            string fileName = $"{dbName}.sqlite";

            string path = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".gmbuddy", "databases", fileName)
                : Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "GMBuddy", "Databases", fileName);

            return path;
        }

        /// <summary>
        /// Gets an OS-specific path to a directory for storing various files
        /// </summary>
        /// <returns>
        /// On *nix platforms, $HOME/.gmbuddy is returned.
        /// On Windows, %LOCALAPPDATA%\GMBuddy is returned.
        /// </returns>
        public static string GetDataDirectory()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "GMBuddy")
                : Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".gmbuddy");
        }
    }
}
