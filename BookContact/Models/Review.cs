using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookContact.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public string Reviews { get; set; }
        public string Comments { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }

    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public string Reviews { get; set; }
        public string Comments { get; set; }

        public int BookId { get; set; }
        public string Title { get; set; }

    }
}