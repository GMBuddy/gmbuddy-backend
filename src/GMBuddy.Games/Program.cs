using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddy.Games
{
    public static class Program
    {
        /// <summary>
        /// We need to include an empty main method to satisfy Entity Framework Core's tooling requirements
        /// `dotnet ef *` require launching the program as an executable, even if it doesnt actually do anything
        /// So, while this is mostly a class library, it just has a main method as a workaround
        /// </summary>
        public static void Main()
        {
        }
    }
}
