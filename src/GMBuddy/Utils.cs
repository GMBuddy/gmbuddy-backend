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
        public static string GetDatabasePath(string dbName)
        {
            if (string.IsNullOrEmpty(dbName))
            {
                throw new ArgumentException();
            }

            string fileName = $"{dbName}.sqlite";

            string path = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".gmbuddy", "database", fileName)
                : Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "GMBuddy", "Databases", fileName);

            return path;
        }
    }
}
