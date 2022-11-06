using ABE.LTRIC.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Infrastructure
{
    public class LTRIC_Context : DbContext
    {
        public LTRIC_Context(DbContextOptions<LTRIC_Context> options) : base(options)
        {
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Doc> Docs { get; set; }
        public DbSet<DocDtl> DocDtls { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
