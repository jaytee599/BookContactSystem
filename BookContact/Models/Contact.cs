using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookContact.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public string UserId { get; set; }
        public string AuthorId { get; set; }
        public string RentalId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Author Author { get; set; }
        public virtual Rental Rental { get; set; }

    }

    public class ContactDto
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string UserId { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int RentalId { get; set; }
    }
}