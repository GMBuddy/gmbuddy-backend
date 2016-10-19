using System;
using System.IO;
using System.Runtime.InteropServices;

namespace GMBuddy.Games
{
    public static class Utils
    {
        /// <summary>
        /// Gets an OS-specific path to the gameType.sqlite file.
        /// </summary>
        /// <param name="gameType"></param>
        /// <returns>
        /// On *nix platforms, $HOME/.gmbuddy/databases/gameType.sqlite is returned.
        /// On Windows, %LOCALAPPDATA%\GMBuddy\Databases\gameType.sqlite is returned.
        /// </returns>
        /// <exception cref="ArgumentException">Throws an ArgumentException if gameType is null or empty</exception>
        public static string GetDatabasePath(string gameType)
        {
            if (string.IsNullOrEmpty(gameType))
            {
                throw new ArgumentException();
            }

            string fileName = $"{gameType}.sqlite";

            string path = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".gmbuddy", "database", fileName) 
                : Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "GMBuddy", "Databases", fileName);

            Console.WriteLine(path);

            return path;
        }
    }
}
