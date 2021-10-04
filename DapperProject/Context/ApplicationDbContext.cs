using Domain.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DapperProject.Context
{
    /// <summary>
    ///     IApplicationDbContext will be used to share EF Core and Dapper Context 
    ///     by using this interface 
    /// </summary>
    public interface IApplicationDbContext
    {
        /// <summary>
        ///     EF Core uses the base IDbConnextion which is the base SQL Connection.
        ///     You will be able to call dapper from this SQL Connection.
        /// </summary>
        public IDbConnection Connection { get; }
        /// <summary>
        ///     Database Facade provides all additional EF Core Functionality that isn't
        ///     Attached to a DbSet e.g context.Database.UseTransaction(transaction);
        ///     becomes available
        /// </summary>
        DatabaseFacade Database { get; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<AuthorsDOB> AuthorsDOB { get; set; }
        public DbSet<Books_Genres> BooksGenres { get; set; }
        public DbSet<Authors_Books> AuthorsBooks { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<AuthorsDOB> AuthorsDOB { get; set; }
        public DbSet<Books_Genres> BooksGenres { get; set; }
        public DbSet<Authors_Books> AuthorsBooks { get; set; }

        public IDbConnection Connection => Database.GetDbConnection();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  Author 1:1 DOB
            modelBuilder.Entity<Author>()
                .HasOne(auth => auth.DOB)
                .WithOne(dob => dob.Author)
                .HasForeignKey<AuthorsDOB>(dob => dob.AuthorId);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Authors)
                .WithMany(b => b.Books)
                .UsingEntity<Authors_Books>(
                        eb => eb
                            .HasOne(bk => bk.Author)
                            .WithMany(p => p.AuthorsBooks)
                            .HasForeignKey(bk => bk.AuthorId),
                        eb => eb
                            .HasOne(auth => auth.Book)
                            .WithMany(t => t.BooksAuthors)
                            .HasForeignKey(auth => auth.BookId));

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Genres)
                .WithMany(b => b.Books)
                .UsingEntity<Books_Genres>(
                        eb => eb
                            .HasOne(bkgen => bkgen.Genre)
                            .WithMany(gen => gen.GenresBooks)
                            .HasForeignKey(bkgen => bkgen.GenreId),
                        eb => eb
                            .HasOne(bkgen => bkgen.Book)
                            .WithMany(bk => bk.BooksGenres)
                            .HasForeignKey(bkgen => bkgen.BookId));
        }
    }
}
