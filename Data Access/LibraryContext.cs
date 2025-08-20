using Data_Access.DataModels.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access;

public class LibraryContext: DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options): base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Bookshelf> Bookshelfs => Set<Bookshelf>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Bookshelf>()
            .HasMany(shelf => shelf.Books)
            .WithOne(book => book.Bookshelf)
            .HasForeignKey(book => book.BookShelfId);
    }
}