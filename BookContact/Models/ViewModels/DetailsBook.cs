using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookContact.Models.ViewModels
{
    public class DetailsBook
    {
        public BookDto SelectedBook {  get; set; }
        public IEnumerable<RentalDto> RentedBooks { get; set;}
        public IEnumerable<ReviewDto> BookReview {  get; set; }
    }
}