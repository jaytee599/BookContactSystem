using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookContact.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Please enter a Name")]
        public string AuthorName { get; set; }

        public string Biography { get; set; }
        public ICollection<Book> Books { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }

    public class AuthorDto
    {
        public int AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string Biography { get; set; }
    }
}