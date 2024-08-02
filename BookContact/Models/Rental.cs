using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookContact.Models
{

    public class Rental
    {
        [Key]
        public int RentalId { get; set; }
        public string ISBN { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool RentalAvailable { get; set; }

        public virtual ApplicationUser User { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }

    public class RentalDto
    {
        public int RentalId { get; set; }
        public string ISBN { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool RentalAvailable { get; set; }

        public int BookId { get; set; }
        public string Title { get; set; }

    }
}