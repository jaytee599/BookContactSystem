using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookContact.Models.ViewModels
{
    public class UpdateBook
    {
        public BookDto SelectedBook { get; set; }

        public IEnumerable<AuthorDto> AuthorOptions { get; set; }
    }
}