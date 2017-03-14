using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BookRecommender.Models;
using BookRecommender.Models.Database;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BookRecommender.DataManipulation
{
    public class BookRecommenderContext : IdentityDbContext<ApplicationUser>
    {
        //public BookRecommenderContext(DbContextOptions<BookRecommenderContext> options) : base(options)
        public BookRecommenderContext() : base()
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<BookAuthor> BooksAuthors { get; set; }
        public DbSet<BookCharacter> BooksCharacters { get; set; }
        public DbSet<BookGenre> BooksGenres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlite("Filename=C://netcore//SQLite//BookRecommender.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasAlternateKey(c => c.Uri)
                .HasName("AlternateKey_Uri");

            modelBuilder.Entity<Author>()
                .HasAlternateKey(c => c.Uri)
                .HasName("AlternateKey_Uri");

            modelBuilder.Entity<Character>()
                .HasAlternateKey(c => c.Uri)
                .HasName("AlternateKey_Uri");

            modelBuilder.Entity<Genre>()
                .HasAlternateKey(c => c.Uri)
                .HasName("AlternateKey_Uri");

            //--------------------------------
            // Many to many relationships
            //--------------------------------

            modelBuilder.Entity<BookAuthor>()
                .HasKey(t => new { t.BookId, t.AuthorId });

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BooksAuthors)
                .HasForeignKey(ba => ba.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BooksAuthors)
                .HasForeignKey(ba => ba.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------

            modelBuilder.Entity<BookCharacter>()
                .HasKey(t => new { t.BookId, t.CharacterId });

            modelBuilder.Entity<BookCharacter>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BooksCharacters)
                .HasForeignKey(bc => bc.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookCharacter>()
                .HasOne(bc => bc.Character)
                .WithMany(c => c.BooksCharacters)
                .HasForeignKey(bc => bc.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------
                
            modelBuilder.Entity<BookGenre>()
                .HasKey(t => new { t.BookId, t.GenreId });

            modelBuilder.Entity<BookGenre>()
                .HasOne(bg => bg.Book)
                .WithMany(b => b.BooksGenres)
                .HasForeignKey(bc => bc.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookGenre>()
                .HasOne(bg => bg.Genre)
                .WithMany(g => g.BooksGenres)
                .HasForeignKey(bg => bg.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            //--------------------------------

            modelBuilder.Entity<Tag>()
                .HasOne(bt => bt.Book)
                .WithMany(t => t.Tags)
                .HasForeignKey(bt => bt.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}