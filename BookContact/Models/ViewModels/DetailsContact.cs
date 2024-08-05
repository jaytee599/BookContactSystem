using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookContact.Models.ViewModels
{
    public class DetailsContact
    {
        public ContactDto Contact { get; set; }
        public IEnumerable<AuthorDto> AuthorOption { get; set; }
        public IEnumerable<RentalDto> RentalOption { get; set; }
    }
}