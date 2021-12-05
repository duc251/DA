using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Da.Data.Ef
{
     public class CafeDbContextFactory : IDesignTimeDbContextFactory<CafeDbContext>
    {
        public CafeDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("CafeSolutionDb");

            var optionsBuilder = new DbContextOptionsBuilder<CafeDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new CafeDbContext(optionsBuilder.Options);
        }
    }
}
