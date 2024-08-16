using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookContact.Models.ViewModels
{
    public class DetailsReview
    {
        public ReviewDto SelectedReview { get; set; }
        public IEnumerable<BookDto> RelatedBooks { get; set; }
    }
}