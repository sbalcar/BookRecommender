using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BookRecommender.Models.Database
{
    public class BookAuthor
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public BookAuthor(Book book, Author author)
        {
            this.BookId = book.BookId;
            this.AuthorId = author.AuthorId;
        }
        // EF needs simple constructor
        public BookAuthor() { }
    }
}