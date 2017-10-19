using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.PostgreSql;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryAppCore.Domain.Concrete
{
    public class LibraryPostgreSqlContext : IdentityDbContext<User>
    {
        public LibraryPostgreSqlContext(DbContextOptions<LibraryPostgreSqlContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<AuthorPostgreSql> Authors { get; set; }
        public DbSet<BookPostgreSql> Books { get; set; }
        public DbSet<SectionsPostgreSql> Sections { get; set; }
    }
}
