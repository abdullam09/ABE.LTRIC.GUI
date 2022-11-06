using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LTRIC_Context>
    {
        public LTRIC_Context CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<LTRIC_Context>();
            var connectionString = configuration.GetConnectionString("LTRIC_Database");
            builder.UseSqlServer(connectionString);
            return new LTRIC_Context(builder.Options);
        }
    }
}
