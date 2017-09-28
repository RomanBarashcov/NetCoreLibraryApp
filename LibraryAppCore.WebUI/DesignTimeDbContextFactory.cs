using LibraryAppCore.Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAppCore.WebUI
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LibraryPostgreSqlContext>
    {
        public LibraryPostgreSqlContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<LibraryPostgreSqlContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(connectionString, b => b.MigrationsAssembly("LibraryAppCore.WebUI"));

            return new LibraryPostgreSqlContext(builder.Options);
        }
    }
}
