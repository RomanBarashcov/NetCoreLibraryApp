using LibraryAppCore.Domain.Entities.MsSql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Concrete
{
    public class LibraryPostgreSqlContext : DbContext
    {
        public LibraryPostgreSqlContext(DbContextOptions<LibraryPostgreSqlContext> options) : base(options) { }

        public DbSet<AuthorPostgreSql> Authors { get; set; }
        public DbSet<BookPostgreSql> Books { get; set; }
    }
}
