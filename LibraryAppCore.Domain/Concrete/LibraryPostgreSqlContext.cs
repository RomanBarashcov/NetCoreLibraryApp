using LibraryAppCore.Domain.Entities.MsSql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using LibraryAppCore.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LibraryAppCore.Domain.Concrete
{
    public class LibraryPostgreSqlContext : IdentityDbContext<User>
    {
        public LibraryPostgreSqlContext(DbContextOptions<LibraryPostgreSqlContext> options) : base(options) { }

        public DbSet<AuthorPostgreSql> Authors { get; set; }
        public DbSet<BookPostgreSql> Books { get; set; }
    }
}
