﻿// <auto-generated />
using BookRecommender.DataManipulation;
using BookRecommender.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BookRecommender.Migrations
{
    [DbContext(typeof(BookRecommenderContext))]
    [Migration("20180319171432_newMigration")]
    partial class newMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("BookRecommender.Models.Database.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("HasManageAccess");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DateBirthString")
                        .HasColumnName("DateBirth");

                    b.Property<string>("DateDeathString")
                        .HasColumnName("DateDeath");

                    b.Property<string>("Description");

                    b.Property<string>("GoogleImageCache");

                    b.Property<string>("Name");

                    b.Property<string>("NameCs");

                    b.Property<string>("NameEn");

                    b.Property<string>("OriginalImage");

                    b.Property<int?>("Sex");

                    b.Property<string>("Uri")
                        .IsRequired();

                    b.Property<string>("WikipediaPage");

                    b.HasKey("AuthorId");

                    b.HasAlternateKey("Uri")
                        .HasName("AlternateKey_Uri");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("FreeBase");

                    b.Property<string>("GndId");

                    b.Property<string>("GoogleImageCache");

                    b.Property<string>("ISBN10");

                    b.Property<string>("ISBN13");

                    b.Property<string>("NameCs");

                    b.Property<string>("NameEn");

                    b.Property<string>("NameOrig");

                    b.Property<string>("OpenLibId");

                    b.Property<string>("OrigLang");

                    b.Property<string>("OriginalImage");

                    b.Property<string>("PublicationDateString")
                        .HasColumnName("PublicationDate");

                    b.Property<string>("Publisher");

                    b.Property<string>("Title");

                    b.Property<string>("Uri")
                        .IsRequired();

                    b.Property<string>("WikipediaPage");

                    b.HasKey("BookId");

                    b.HasAlternateKey("Uri")
                        .HasName("AlternateKey_Uri");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.BookAuthor", b =>
                {
                    b.Property<int>("BookId");

                    b.Property<int>("AuthorId");

                    b.HasKey("BookId", "AuthorId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BookId");

                    b.ToTable("BooksAuthors");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.BookCharacter", b =>
                {
                    b.Property<int>("BookId");

                    b.Property<int>("CharacterId");

                    b.HasKey("BookId", "CharacterId");

                    b.HasIndex("BookId");

                    b.HasIndex("CharacterId");

                    b.ToTable("BooksCharacters");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.BookGenre", b =>
                {
                    b.Property<int>("BookId");

                    b.Property<int>("GenreId");

                    b.HasKey("BookId", "GenreId");

                    b.HasIndex("BookId");

                    b.HasIndex("GenreId");

                    b.ToTable("BooksGenres");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.BookRating", b =>
                {
                    b.Property<int>("BookRatingId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<int>("Rating");

                    b.Property<string>("Review");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("BookRatingId");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.Character", b =>
                {
                    b.Property<int>("CharacterId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NameCs");

                    b.Property<string>("NameEn");

                    b.Property<string>("Uri")
                        .IsRequired();

                    b.HasKey("CharacterId");

                    b.HasAlternateKey("Uri")
                        .HasName("AlternateKey_Uri");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.Feedback", b =>
                {
                    b.Property<int>("FeedbackId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("FeedbackId");

                    b.HasIndex("UserId");

                    b.ToTable("Feedback");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.Genre", b =>
                {
                    b.Property<int>("GenreId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NameCs");

                    b.Property<string>("NameEn");

                    b.Property<string>("Uri")
                        .IsRequired();

                    b.HasKey("GenreId");

                    b.HasAlternateKey("Uri")
                        .HasName("AlternateKey_Uri");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookId");

                    b.Property<string>("Lang")
                        .IsRequired();

                    b.Property<double?>("Score");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("TagId");

                    b.HasIndex("BookId");

                    b.HasIndex("Value");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.TagCount", b =>
                {
                    b.Property<int>("TagCountId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Count");

                    b.Property<string>("Lang")
                        .IsRequired();

                    b.HasKey("TagCountId");

                    b.HasIndex("Lang")
                        .IsUnique();

                    b.ToTable("TagsCount");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.UserActivity", b =>
                {
                    b.Property<int>("UserActivityId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<int>("Type");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("UserActivityId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersActivities");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.WikiStorageEntry", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Lang");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.HasKey("Id", "Lang");

                    b.HasIndex("Id");

                    b.ToTable("WikiStorage");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BookRecommender.Models.Database.BookAuthor", b =>
                {
                    b.HasOne("BookRecommender.Models.Database.Author", "Author")
                        .WithMany("BooksAuthors")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookRecommender.Models.Database.Book", "Book")
                        .WithMany("BooksAuthors")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookRecommender.Models.Database.BookCharacter", b =>
                {
                    b.HasOne("BookRecommender.Models.Database.Book", "Book")
                        .WithMany("BooksCharacters")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookRecommender.Models.Database.Character", "Character")
                        .WithMany("BooksCharacters")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookRecommender.Models.Database.BookGenre", b =>
                {
                    b.HasOne("BookRecommender.Models.Database.Book", "Book")
                        .WithMany("BooksGenres")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookRecommender.Models.Database.Genre", "Genre")
                        .WithMany("BooksGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookRecommender.Models.Database.BookRating", b =>
                {
                    b.HasOne("BookRecommender.Models.Database.Book", "Book")
                        .WithMany("Ratings")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookRecommender.Models.Database.ApplicationUser", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookRecommender.Models.Database.Feedback", b =>
                {
                    b.HasOne("BookRecommender.Models.Database.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookRecommender.Models.Database.Tag", b =>
                {
                    b.HasOne("BookRecommender.Models.Database.Book", "Book")
                        .WithMany("Tags")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookRecommender.Models.Database.UserActivity", b =>
                {
                    b.HasOne("BookRecommender.Models.Database.ApplicationUser", "User")
                        .WithMany("Activities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BookRecommender.Models.Database.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BookRecommender.Models.Database.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookRecommender.Models.Database.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BookRecommender.Models.Database.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}