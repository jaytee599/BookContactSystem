using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookContact.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        //[Required(ErrorMessage = "Please enter the title of the book")]
        public string Title { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public DateTime? BorrowingDate { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public ICollection<Rental> Rentals { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }

    public class BookDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public DateTime? BorrowingDate { get; set; }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; }

    }
}