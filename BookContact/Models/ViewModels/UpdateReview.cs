using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookContact.Models.ViewModels
{
    public class UpdateReview
    {
        public ReviewDto SelectedReview { get; set; }
        public IEnumerable<BookDto> BookOptions { get; set; }
    }
}