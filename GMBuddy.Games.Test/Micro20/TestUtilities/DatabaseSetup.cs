﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Games.Micro20.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GMBuddy.Games.Test.Micro20.TestUtilities
{
    public class DatabaseSetup
    {
        public static DbContextOptions<Micro20DataContext> CreateContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<Micro20DataContext>()
                .UseInMemoryDatabase()
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
